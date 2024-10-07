﻿Imports System.IO
Imports System.Text
Imports Autodesk.Navisworks.Api

''' <summary>
''' Provides functionalities to extract properties from selected Navisworks elements based on predefined mappings.
''' This module handles the validation of elements, extraction of necessary properties, and formatting the data for output.
''' </summary>
Module PropertyExtractionModule

    ''' <summary>
    ''' Stores the mapping of supported element types to their required properties for identifying Level of Detail (LoD).
    ''' Each key in the dictionary represents an element type (e.g., "Wall", "Basic Roof"),
    ''' and its corresponding value is a list of tuples specifying the property categories and property names
    ''' that are necessary for processing and validation.
    ''' </summary>
    ReadOnly AvailableType As New Dictionary(Of String, List(Of (Cat As String, Prop As String))) From {
        {"Wall", New List(Of (Cat As String, Prop As String)) From {
            ("Revit Type", "Width"),
            ("Revit Type", "AUR_MATERIAL_TYPE"),
            ("Item", "Material"),
            ("Element", "Area"),
            ("Element", "Unconnected Height"),
            ("Element", "Length"),
            ("Element", "Id")
        }},
        {"Roof", New List(Of (Cat As String, Prop As String)) From {
            ("Element", "Thickness"),
            ("Element", "Slope")
        }},
        {"Ceiling", New List(Of (Cat As String, Prop As String)) From {
            ("Element", "Area"),
            ("Element", "Length"),
            ("Element", "Thickness")
        }},
        {"Floor", New List(Of (Cat As String, Prop As String)) From {
            ("Element", "Area"),
            ("Element", "Elevation at Top"),
            ("Element", "Elevation at Bottom"),
            ("Element", "Element Thickness"),
            ("Revit Type", "Structural Material")
        }},
        {"Structural Framing", New List(Of (Cat As String, Prop As String)) From {
            ("Element", "Length"),
            ("Revit Type", "AUR_MATERIAL_TYPE")
        }},
        {"Gutter", New List(Of (Cat As String, Prop As String)) From {
            ("Element", "Host")}} ' Gutter is needed for Roof LoD
    }

    '''<summary>
    ''' Retrieves all descendant elements from the currently selected items in the active Navisworks document.
    ''' </summary>
    ''' <returns>
    ''' A <see cref="ModelItemCollection"/> containing all child elements of the currently selected items.
    ''' </returns>
    Public Function GetCurrentSelectionAllElements() As ModelItemCollection
        ' Initialize a new collection to hold all descendant elements
        Dim newCollection As New ModelItemCollection()
        For Each modelItem In Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems
            newCollection.AddRange(modelItem.Descendants)
        Next
        Return newCollection
    End Function

    '''<summary>
    ''' Formats a VariantData value for safe CSV output.
    ''' If the value is a string, it encloses it in double quotes.
    ''' Otherwise, it returns the value as a string without modification.
    ''' </summary>
    ''' <param name="variant">The VariantData value to format.</param>
    ''' <returns>A string formatted for CSV output.</returns>
    Public Function GetPropertyValueForCSV(element As ModelItem, categoryDisplayName As String, propertyDisplayName As String) As String
        Try
            ' Convert the VariantData to its string representation
            Dim propValue As VariantData = element.PropertyCategories.FindPropertyByDisplayName(categoryDisplayName, propertyDisplayName).Value
            Dim stringValue As String = propValue.ToString().Split(":")(1)
            ' Check if the value is a string and contains special CSV characters
            If propValue.IsDisplayString Then
                ' Enclose the string in double quotes
                Return $"""{stringValue}"""
            Else
                ' For non-string types, return the string representation as-is
                Return stringValue
            End If
        Catch ex As Exception
            ' In case of any unexpected errors, return an empty string or handle as needed
            Return ""
        End Try
    End Function

    Public Function ExtractProperties(selectedElements As ModelItemCollection) As List(Of Dictionary(Of String, String))
        Dim extractedElements As New List(Of Dictionary(Of String, String))()

        For Each selectedElement In selectedElements
            If Not selectedElement.IsComposite Then
                Continue For
            End If

            Dim supportedCat As String = Nothing
            ' Iterate through each key in the AvailableType dictionary to determine if the element's category is supported
            For Each key In AvailableType.Keys
                Try
                    ' Retrieve the 'Category' property from the element's 'Element' property category
                    Dim elementCat As String = GetPropertyValueForCSV(selectedElement, "Element", "Category")
                    If elementCat = "" Then
                        Continue For
                    End If

                    ' The element's category value need to contain the key, e.g 'Basic Walls' will have the 'Wall' to be valid
                    If InStr(elementCat, key) > 0 Then
                        supportedCat = key
                        Exit For
                    End If
                Catch ex As Exception
                    Continue For
                End Try
            Next

            ' If no supported element type was found, skip to the next element
            If supportedCat Is Nothing Then
                Continue For
            End If

            ' Extract Basic Properties
            Dim extractedElement As New Dictionary(Of String, String) From {
                {"Item.Guid", GetPropertyValueForCSV(selectedElement, "Item", "GUID")},
                {"Document.Title", GetPropertyValueForCSV(selectedElement, "Document", "Title")},
                {"Element.Category", GetPropertyValueForCSV(selectedElement, "Element", "Category")},
                {"Element.Name", GetPropertyValueForCSV(selectedElement, "Element", "Name")}
            }

            ' Iterate through each property category and property defined for the supported element type
            For Each propCat In AvailableType(supportedCat)
                extractedElement.Add($"{propCat.Cat}.{propCat.Prop}", GetPropertyValueForCSV(selectedElement, propCat.Cat, propCat.Prop))
            Next

            extractedElements.Add(extractedElement)
        Next

        Return extractedElements
    End Function

    Public Function GetUniqueHeaderForCsv() As List(Of String)

        ' Add default header
        Dim uniqueCatPropList As New List(Of String)() From {"Item.Guid", "Document.Title", "Element.Category"}

        ' Add to the unique list of "Cat.Prop" strings using LINQ
        uniqueCatPropList.AddRange(AvailableType.SelectMany(Function(kvp) kvp.Value) _
            .Select(Function(tp) _
                            If(String.IsNullOrWhiteSpace(tp.Cat) OrElse String.IsNullOrWhiteSpace(tp.Prop),
                            String.Empty,
                            $"{tp.Cat}.{tp.Prop}")) _
            .Where(Function(s) Not String.IsNullOrWhiteSpace(s)) _
            .Distinct(StringComparer.OrdinalIgnoreCase) _
            .OrderBy(Function(s) s) _
            .ToList()
            )
        Return uniqueCatPropList
    End Function

    Public Sub WritePropertiesToCsv()

        Dim outputName As String = InputBox("Please input name for this selection", "Algorithm Output Name")
        ' Check if the user pressed Cancel or entered an empty string
        If String.IsNullOrWhiteSpace(outputName) Then
            Throw New System.Exception("Operation canceled or no file name provided.")
            Exit Sub
        End If

        Dim selectedCollection As ModelItemCollection = GetCurrentSelectionAllElements()
        Dim extractedElements As List(Of Dictionary(Of String, String)) = ExtractProperties(selectedCollection)
        Dim headerList As List(Of String) = GetUniqueHeaderForCsv()
        Dim filepath As String = Path.Combine(My.Settings.UserFolderPath, "ExtractData", $"{outputName}.csv")


        ' Write to CSV by iterate through the list and using streamwriter
        Try
            Using writer As New StreamWriter(filepath, False, Encoding.UTF8)
                ' Write the header line
                writer.WriteLine(String.Join(",", headerList))
                For Each element In extractedElements
                    Dim rowValues As New List(Of String)()

                    For Each header In headerList
                        If element.ContainsKey(header) Then
                            Dim value As String = element(header)
                            rowValues.Add(value)
                        Else
                            ' If the key doesn't exist, leave the field empty
                            rowValues.Add(String.Empty)
                        End If
                    Next
                    writer.WriteLine(String.Join(",", rowValues))
                Next
                writer.Close()
            End Using

        Catch ex As Exception
            Throw New System.Exception($"Error saving CSV file: {ex.Message}")
        End Try
    End Sub
End Module
