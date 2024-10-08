Imports Autodesk.Navisworks.Api
Imports Microsoft.VisualBasic.FileIO
Imports System.Text
Imports System.Windows.Forms
''' <summary>
''' Related class and functions to create a selection set from GUIDs listed in a CSV file
''' </summary>
Module CsvResultLoaderModule
    ''' <summary>
    ''' Represents an element ingested from the CSV file, containing necessary properties.
    ''' </summary>
    Public Class IngestedElement
        ' Custom class to store only necessary info from CSV

        ''' <summary>
        ''' Gets or sets the GUID of the element.
        ''' </summary>
        Public Property GUID As String

        ''' <summary>
        ''' Gets or sets the Level of Detail (LOD) of the element.
        ''' </summary>
        Public Property LOD As String

        ''' <summary>
        ''' Gets or sets the missing properties of the element.
        ''' </summary>
        Public Property MissingProperties As String

        ''' <summary>
        ''' Gets or sets the search result status of the element.
        ''' </summary>
        Public Property SearchResult As String

        ''' <summary>
        ''' Initializes a new instance of the IngestedElement class with default SearchResult.
        ''' </summary>
        Public Sub New()
            ' Constructor to initialize SearchResult with a default value
            SearchResult = "Not Search Yet"
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the IngestedElement class with specified properties.
        ''' </summary>
        ''' <param name="guid">The GUID of the element.</param>
        ''' <param name="lod">The Level of Detail of the element.</param>
        ''' <param name="missingProps">The missing properties of the element.</param>
        Public Sub New(guid As String, lod As String, missingProps As String)
            Me.GUID = guid
            Me.LOD = lod
            MissingProperties = missingProps
            SearchResult = "Not Search Yet" ' Default value
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the IngestedElement class by copying an existing instance and optionally setting a new SearchResult.
        ''' </summary>
        ''' <param name="existingElement">The existing IngestedElement to copy.</param>
        ''' <param name="newSearchResult">The new search result status. Defaults to "Not Search Yet".</param>
        Public Sub New(existingElement As IngestedElement, Optional newSearchResult As String = "Not Search Yet")
            ' Copy constructor with option to set a new SearchResult
            GUID = existingElement.GUID
            LOD = existingElement.LOD
            MissingProperties = existingElement.MissingProperties
            SearchResult = newSearchResult
        End Sub

        ''' <summary>
        ''' Returns a string representation of the IngestedElement.
        ''' </summary>
        ''' <returns>A string containing the GUID, LOD, Missing Properties, and Search Result.</returns>
        Public Overrides Function ToString() As String
            Return $"GUID: {GUID}, LOD: {LOD}, Missing Properties: {MissingProperties}, Search Result: {SearchResult}"
        End Function
    End Class

    ''' <summary>
    ''' Opens a dialog for the user to select a CSV file and returns the selected file path.
    ''' </summary>
    ''' <returns>The full path of the selected CSV file, or an empty string if no file was selected.</returns>
    Public Function GetCsvFilePath() As String
        Using openFileDialog As New OpenFileDialog()
            openFileDialog.Title = "Select CSV File Containing GUIDs"
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                Return openFileDialog.FileName
            Else
                Return String.Empty
            End If
        End Using
    End Function
    ''' <summary>
    ''' Ingests elements from a CSV file and returns a list of IngestedElement instances.
    ''' </summary>
    ''' <param name="CsvFilePath">The full path to the CSV file.</param>
    ''' <returns>A list of IngestedElement objects extracted from the CSV.</returns>
    Public Function IngestCsv(CsvFilePath As String) As List(Of IngestedElement)
        Dim elementList As New List(Of IngestedElement)()
        Dim failList As New StringBuilder()

        ' Initialize the TextFieldParser to read the CSV file
        Using parser As New TextFieldParser(CsvFilePath)
            parser.TextFieldType = FieldType.Delimited
            parser.SetDelimiters(",") ' Set delimiter to comma
            parser.HasFieldsEnclosedInQuotes = True ' Handle quoted fields

            ' Check if file has content
            If parser.EndOfData Then
                Throw New Exception("CSV file is empty")
            End If

            ' Read the header line
            Dim headers As String() = parser.ReadFields()
            headers = headers.Select(Function(s) s.ToLower()).ToArray()
            Dim guidIndex As Integer = Array.IndexOf(headers, "item.guid")
            Dim lodIndex As Integer = Array.IndexOf(headers, "lod")
            Dim missingPropsIndex As Integer = Array.IndexOf(headers, "missing_properties")

            ' Check that all required columns are present
            If guidIndex = -1 OrElse lodIndex = -1 OrElse missingPropsIndex = -1 Then
                Throw New Exception("CSV file does not contain the required headers: Item.GUID, LOD, Missing_Properties.")
            End If

            ' Read each subsequent line and create IngestedElement instances
            While Not parser.EndOfData
                Try
                    Dim fields As String() = parser.ReadFields()
                    ' Ensure the line has enough fields
                    If fields.Length > Math.Max(guidIndex, Math.Max(lodIndex, missingPropsIndex)) Then
                        Dim item As New IngestedElement() With {
                            .GUID = fields(guidIndex).Trim(),
                            .LOD = fields(lodIndex).Trim(),
                            .MissingProperties = fields(missingPropsIndex).Trim()
                        }
                        elementList.Add(item)
                    Else
                        ' Add line with insufficient fields to fail list
                        failList.Append("Line " & parser.LineNumber - 1)
                    End If
                Catch ex As MalformedLineException
                    ' Add malformed line information to fail list
                    failList.Append("Line " & ex.Message)
                End Try
            End While
        End Using

        ' Display failed lines, if any
        If Not String.IsNullOrEmpty(failList.ToString()) Then
            MessageBox.Show("List of failed lines: " & failList.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        Return elementList
    End Function

    ''' <summary>
    ''' Searches for ModelItems based on a list of IngestedElement GUIDs.
    ''' </summary>
    ''' <param name="elementList">A list of IngestedElement objects containing GUIDs to search for.</param>
    ''' <returns>A tuple containing a collection of successfully found ModelItems and a list of failed IngestedElements.</returns>
    Public Function SearchElements(elementList As List(Of IngestedElement)) As (Success As ModelItemCollection, Fails As List(Of IngestedElement))
        Dim foundElements As New ModelItemCollection()
        Dim errorElements As New List(Of IngestedElement)()

        ' Validate input list
        If elementList Is Nothing OrElse elementList.Count = 0 Then
            Throw New ArgumentException("The GUID list cannot be null or empty.", NameOf(elementList))
        End If

        ' Iterate through each element and search for the corresponding ModelItem
        For Each element In elementList
            Try
                ' Initialize the search within the active document's models
                Dim search As New Search()
                search.SearchConditions.Add(SearchCondition.HasPropertyByName(PropertyCategoryNames.Item, DataPropertyNames.ItemGuid).EqualValue(VariantData.FromDisplayString(element.GUID)))
                search.Locations = SearchLocations.DescendantsAndSelf
                search.Selection.SelectAll()

                ' Execute the search and retrieve the first matching item
                Dim foundItem As ModelItem = search.FindFirst(Autodesk.Navisworks.Api.Application.ActiveDocument, False)
                ' Add the found item to the collection if it exists
                If foundItem IsNot Nothing Then
                    foundElements.Add(foundItem)
                Else
                    ' Add to error list if the GUID does not correspond to any ModelItem
                    Dim errorElement As New IngestedElement(element, "Not Found")
                    errorElements.Add(errorElement)
                End If

            Catch ex As Exception
                ' Add to error list if an exception occurs during the search
                Dim errorElement As New IngestedElement(element, $"Error: {ex.Message}")
                errorElements.Add(errorElement)
            End Try
        Next

        ' Return the tuple containing successful and failed searches
        Return (foundElements, errorElements)
    End Function

End Module