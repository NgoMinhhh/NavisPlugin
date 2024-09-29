Imports Autodesk.Navisworks.Api
Imports System.Text

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
            ("Item", "GUID"),
            ("Document", "Title"),
            ("Revit Type", "Width"),
            ("Revit Type", "AUR_MATERIAL_TYPE"),
            ("Item", "Material"),
            ("Element", "Area"),
            ("Element", "Unconnected Height"),
            ("Element", "Length"),
            ("Element", "Id")
        }},
        {"Basic Roof", New List(Of (Cat As String, Prop As String)) From {
            ("Element", "Thickness"),
            ("Element", "Slope")
        }}
        }

    ''' <summary>
    ''' Iterates through each selected element, validates it, and extracts necessary properties based on a predefined dictionary
    ''' mapping the supported type (Walls, Roofs, etc) with required properties.
    ''' </summary>
    ''' <param name="selectedElements">The collection of currently selected ModelItem elements.</param>
    ''' <returns>A CSV-formatted string containing the extracted properties.</returns>
    Public Function ExtractProperties(selectedElements As ModelItemCollection) As String
        Dim output As New StringBuilder()

        For Each element In selectedElements
            ' Check if the element is a composite (i.e., it has child elements)
            If Not element.IsComposite Then
                Continue For
            End If

            ' Variable to store the supported category if found
            Dim supportedCat As String = Nothing

            ' Iterate through each key in the AvailableType dictionary to determine if the element's category is supported
            For Each key In AvailableType.Keys
                Try
                    ' Retrieve the 'Category' property from the element's 'Element' property category
                    Dim elementCat As String = element.PropertyCategories.FindPropertyByDisplayName("Element", "Category").Value.ToString()

                    ' The element's category value need to contain the key, e.g 'Basic Walls' will have the 'Wall' to be valid
                    If InStr(elementCat, key) > 0 Then
                        supportedCat = key
                        Exit For
                    End If
                Catch ex As Exception
                    Exit For
                End Try
            Next

            ' If no supported element type was found, skip to the next element
            If supportedCat Is Nothing Then
                Continue For
            End If

            ' Iterate through each property category and property defined for the supported element type
            For Each PropCat In AvailableType(supportedCat)
                Try
                    ' Retrieve the property value based on category and property names
                    Dim propValue As String = element.PropertyCategories.FindPropertyByDisplayName(PropCat.Cat, PropCat.Prop).Value.ToString()

                    ' Append the property value to the output StringBuilder in CSV format, escaping quotes
                    output.AppendFormat("""{0}"",", propValue)
                Catch ex As Exception
                    ' If an exception occurs while retrieving a property value, append an empty field
                    output.Append(",")
                End Try
            Next

            ' Append a newline after processing all properties for the current element
            output.Append(Environment.NewLine)
        Next

        Return output.ToString()
    End Function

    '''<summary>
    ''' Retrieves all descendant elements from the currently selected items in the active Navisworks document.
    ''' </summary>
    ''' <returns>
    ''' A <see cref="ModelItemCollection"/> containing all child elements of the currently selected items.
    ''' </returns>
    Private Function GetCurrentSelectionAllElements() As ModelItemCollection
        ' Initialize a new collection to hold all descendant elements
        Dim newCollection As New ModelItemCollection()
        For Each modelItem In Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems
            newCollection.AddRange(modelItem.Descendants)
        Next
        Return newCollection
    End Function
End Module
