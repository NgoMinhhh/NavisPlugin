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
                    Dim propValue As VariantData = element.PropertyCategories.FindPropertyByDisplayName(PropCat.Cat, PropCat.Prop).Value

                    ' Append the property value to the output StringBuilder in CSV format, escaping quotes
                    output.AppendFormat("{0},", FormatValueForCSV(propValue))
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

    '''<summary>
    ''' Formats a VariantData value for safe CSV output.
    ''' If the value is a string, it encloses it in double quotes.
    ''' Otherwise, it returns the value as a string without modification.
    ''' </summary>
    ''' <param name="variant">The VariantData value to format.</param>
    ''' <returns>A string formatted for CSV output.</returns>
    Public Function FormatValueForCSV(PropertyValue As VariantData) As String
        Try
            ' Convert the VariantData to its string representation
            Dim value As String = PropertyValue.ToString()

            ' Check if the value is a string and contains special CSV characters
            If PropertyValue.IsDisplayString Then
                ' Enclose the string in double quotes
                Return $"""{value}"""
            Else
                ' For non-string types, return the string representation as-is
                Return value
            End If
        Catch ex As Exception
            ' In case of any unexpected errors, return an empty string or handle as needed
            Return ""
        End Try
    End Function
End Module
