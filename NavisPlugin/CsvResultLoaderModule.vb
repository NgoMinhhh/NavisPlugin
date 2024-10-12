Imports Autodesk.Navisworks.Api
Imports Microsoft.VisualBasic.FileIO
Imports System.IO
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
        ''' Gets or sets the source model of the element.
        ''' </summary>
        Public Property Source As String


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
        Public Sub New(guid As String, lod As String, missingProps As String, source As String)
            Me.GUID = guid
            Me.LOD = lod
            MissingProperties = missingProps
            source = source
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
            Source = existingElement.Source
            SearchResult = newSearchResult
        End Sub

        ''' <summary>
        ''' Returns a string representation of the IngestedElement.
        ''' </summary>
        ''' <returns>A string containing the GUID, LOD, Missing Properties, and Search Result.</returns>
        Public Overrides Function ToString() As String
            Return $"GUID: {GUID}, LOD: {LOD}, Missing Properties: {MissingProperties}, Source: {Source},Search Result: {SearchResult}"
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
            openFileDialog.InitialDirectory = Path.Combine(My.Settings.UserFolderPath, "AlgoOutput")

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
            Dim sourceIndex As Integer = Array.IndexOf(headers, "document.title")
            Dim alterSourceIndex As Integer = Array.IndexOf(headers, "item.source file")
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
                            .Source = "document.title: " & fields(sourceIndex).Trim(),
                            .MissingProperties = fields(missingPropsIndex).Trim()
                        }
                        elementList.Add(item)

                        ' Get source from Source File col
                        If item.Source = "document.title: " Then
                            item.Source = "item.source file: " & fields(alterSourceIndex).Trim()
                        End If

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
    Public Function SearchElements(elementList As List(Of IngestedElement)) As ModelItemCollection

        Dim foundElements As New ModelItemCollection()

        ' Validate input list
        If elementList Is Nothing OrElse elementList.Count = 0 Then
            Return New ModelItemCollection()
        End If

        ' Iterate through each element and search for the corresponding ModelItem
        For Each element In elementList
            Try
                ' Initialize the search within the active document's models
                Dim search As New Search()
                search.SearchConditions.Add(SearchCondition.HasPropertyByName(PropertyCategoryNames.Item, DataPropertyNames.ItemGuid).EqualValue(VariantData.FromDisplayString(element.GUID)))
                If element.Source.Split(":")(0) = "document.title" Then
                    search.SearchConditions.Add(SearchCondition.HasPropertyByDisplayName("Document", "Title").DisplayStringContains(element.Source.Split(":")(1).Trim()))
                Else
                    search.SearchConditions.Add(SearchCondition.HasPropertyByDisplayName("Item", "Source File").DisplayStringContains(element.Source.Split(":")(1).Trim()))
                End If
                search.Locations = SearchLocations.DescendantsAndSelf
                search.Selection.SelectAll()

                ' Execute the search and retrieve the first matching item
                Dim foundItem As ModelItem = search.FindFirst(Autodesk.Navisworks.Api.Application.ActiveDocument, False)
                ' Add the found item to the collection if it exists
                If foundItem IsNot Nothing Then
                    foundElements.Add(foundItem)
                End If
            Catch ex As Exception
                Continue For
            End Try
        Next

        ' Return the tuple containing successful and failed searches
        Return foundElements
    End Function


    Public Sub CreateLoDSelectionSets(folderName As String, ingestedList As List(Of IngestedElement))
        ' Create a dict of IngestedElements based on LOD value
        Dim readOutput As New Dictionary(Of String, List(Of IngestedElement)) From {
                {"100", ingestedList.Where(Function(r) r.LOD = "100").ToList()},
                {"200", ingestedList.Where(Function(r) r.LOD = "200").ToList()},
                {"300", ingestedList.Where(Function(r) r.LOD = "300").ToList()},
                {"notVerified", ingestedList.Where(Function(r) String.IsNullOrEmpty(r.LOD)).ToList()}
                }
        ' Create the same dict with value as search result
        Dim searchResults As New Dictionary(Of String, ModelItemCollection)
        For Each key In readOutput.Keys
            searchResults.Add(key, SearchElements(readOutput(key)))
        Next

        Dim activeDoc As Document = Autodesk.Navisworks.Api.Application.ActiveDocument
        Try
            ' Create a folder to group the LoD selection sets
            Dim folderItem As New FolderItem() With {
                .DisplayName = folderName
            }
            activeDoc.SelectionSets.AddCopy(folderItem)

            ' Loop through each sets to find the folder because Navisworks use GUID as the primary key
            ' Newly created folder's reference is not stored
            For Each item In activeDoc.SelectionSets.Value.ToList()
                If item.DisplayName = folderName Then
                    For Each lod In searchResults.Keys
                        If searchResults(lod).Count > 0 Then
                            Dim selectionSet = New SelectionSet(searchResults(lod)) With {
                            .DisplayName = lod
                        }
                            activeDoc.SelectionSets.AddCopy(item, selectionSet)
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Module