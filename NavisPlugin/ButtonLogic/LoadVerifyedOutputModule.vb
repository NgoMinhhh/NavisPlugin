Imports Autodesk.Navisworks.Api
Imports Microsoft.VisualBasic.FileIO
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
''' <summary>
''' Related class and functions to create a selection set from GUIDs listed in a CSV file
''' </summary>
Module LoadVerifyedOutputModule


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
    Public Function IngestCsv(CsvFilePath As String) As (Content As List(Of IngestedElement), guidIndex As Integer, lodIndex As Integer)
        Dim elementList As New List(Of IngestedElement)()
        ' These two index are required to update the csv with new lod values from user editing
        Dim guidIndex As Integer, lodIndex As Integer

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

            guidIndex = Array.IndexOf(headers, "item.guid")
            lodIndex = Array.IndexOf(headers, "lod")
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
                            .MissingProperties = fields(missingPropsIndex).Trim(),
                            .SearchResult = "Verified"
                        }
                        elementList.Add(item)

                        ' Get source from Source File col
                        If item.Source = "document.title: " Then
                            item.Source = "item.source file: " & fields(alterSourceIndex).Trim()
                        End If
                    End If
                Catch ex As MalformedLineException
                    ' Add malformed line information to fail list
                End Try
            End While
        End Using
        Return (elementList, guidIndex, lodIndex)
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
                {"350", ingestedList.Where(Function(r) r.LOD = "350").ToList()},
                {"400", ingestedList.Where(Function(r) r.LOD = "400").ToList()},
                {"notVerified", ingestedList.Where(Function(r) String.IsNullOrEmpty(r.LOD)).ToList()}
                }
        ' Create the same dict with value as search result
        Dim searchResults As New Dictionary(Of String, ModelItemCollection)
        For Each key In readOutput.Keys
            searchResults.Add(key, SearchElements(readOutput(key)))
        Next

        Dim activeDoc As Document = Autodesk.Navisworks.Api.Application.ActiveDocument
        Try
            ' Remove old folder fist
            For Each item In activeDoc.SelectionSets.Value.ToList()
                If item.DisplayName = folderName Then
                    activeDoc.SelectionSets.Remove(item)
                End If
            Next

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