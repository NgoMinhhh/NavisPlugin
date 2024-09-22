Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports Autodesk.Navisworks.Api
Imports Autodesk.Navisworks.Api.Application
Imports Autodesk.Navisworks.Api.Plugins
Imports Microsoft.VisualBasic.FileIO

Namespace NavisPlugin
#Region "ExtractProperties"
    <Plugin("ExtractProperties",                                      ' Plugin name
            "UNISA",                                             ' 4 character Developer ID or GUID
            ToolTip:="Extract all properties from selected elements",       ' The tooltip for the item in the ribbon
            DisplayName:="Extract Properties")>          ' Display name for the Plugin in the Ribbon
    Public Class ExtractPropertiesPlguin
        Inherits AddInPlugin                                    ' Derives from AddInPlugin

        Public Function ExtractDataProperties() As List(Of String)
            ' Create a StringBuilder to store the category names
            Dim output As New List(Of String)()

            ' Write Header name
            output.Add("ElementName,CategoryDisplayName,CategoryName,PropertyDisplayName,PropertyName,Value")

            ' Iterate through the ModelItems in the current selection
            For Each item As ModelItem In Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems
                ' Iterate through the item's PropertyCategory entries
                For Each category As PropertyCategory In item.PropertyCategories
                    ' Iterate through the DataProperties
                    For Each dataProperty As DataProperty In category.Properties
                        ' Add content for display
                        Dim line As String = String.Format("""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}""",' Escape quotes
                                                           item.DisplayName, 'Element's name
                                                           category.DisplayName.Replace("""", """"""),
                                                           category.Name.Replace("""", """"""),
                                                           dataProperty.DisplayName.Replace("""", """"""),
                                                           dataProperty.Name.Replace("""", """"""),
                                                           If(dataProperty.Value Is Nothing, "", dataProperty.Value.ToString().Replace("""", """""")))
                        output.Add(line)
                    Next
                Next
            Next

            Return output
        End Function
        Public Function AskUserToSelectOutputFolder() As String
            Using folderBrowser As New FolderBrowserDialog()
                ' Set the description that appears above the tree view
                folderBrowser.Description = "Select the output folder for the CSV file."

                ' Allow the user to create new folders
                folderBrowser.ShowNewFolderButton = True

                ' Show the dialog and check if the user clicked OK
                If folderBrowser.ShowDialog() = DialogResult.OK Then
                    ' Ensure that the selected path is not empty or whitespace
                    If Not String.IsNullOrWhiteSpace(folderBrowser.SelectedPath) Then
                        ' Return the selected path
                        Return folderBrowser.SelectedPath
                    Else
                        ' Handle the case where no folder was selected
                        MessageBox.Show("No folder was selected. Please try again.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return String.Empty
                    End If
                Else
                    ' Handle the case where the user canceled the dialog
                    MessageBox.Show("Folder selection was canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return String.Empty
                End If
            End Using
        End Function
        Public Overrides Function Execute(ParamArray parameters() As String) As Integer
            ' Initialize a variable to hold the selected folder path
            Dim outputFolderPath As String = AskUserToSelectOutputFolder()

            ' Check if a valid folder path was selected
            If String.IsNullOrEmpty(outputFolderPath) Then
                Return 0
            End If

            ' Define the output CSV file path
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim csvFilePath As String = Path.Combine(outputFolderPath, $"Output_{timestamp}.csv")

            'Extract properties from selected elements
            Dim lines As List(Of String) = ExtractDataProperties()
            Try
                ' Write all lines to the CSV file at once
                File.WriteAllLines(csvFilePath, lines, Encoding.UTF8)
                MessageBox.Show("CSV export completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("An error occurred during CSV export: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            Return 0
        End Function
    End Class
#End Region
#Region "CreateSelectionSet"
    <Plugin("CreateSelectionSet",
        "UNISA",
        ToolTip:="Create a selection set from GUIDs of processed elements",
        DisplayName:="Create a selection set")>
    Public Class CreateSelectionSetPlugin
        Inherits AddInPlugin

        ''' <summary>
        ''' Executes the plugin to create a selection set from GUIDs listed in a CSV file.
        ''' </summary>
        ''' <param name="parameters">An array of string parameters (not used).</param>
        ''' <returns>Integer status code (0 for success).</returns>
        Public Overrides Function Execute(ParamArray parameters() As String) As Integer
            Dim CsvFilePath As String = GetCsvFilePath()
            If CsvFilePath Is String.Empty Then
                MessageBox.Show("No CSV file is chosen!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return 0
            End If

            Try
                ' Ingest elements from the selected CSV file
                Dim elementList As List(Of IngestedElement) = IngestCsv(CsvFilePath)

                ' Declare variables to hold the results
                Dim successfulItems As ModelItemCollection
                Dim failedElements As List(Of IngestedElement)

                ' Call the SearchElements function and assign the results
                Dim result As (Success As ModelItemCollection, Fails As List(Of IngestedElement)) = SearchElements(elementList)
                successfulItems = result.Success
                failedElements = result.Fails

                ' Extract a name portion from the CSV filename for the selection set
                Dim extractedName As String = Path.GetFileNameWithoutExtension(CsvFilePath).Replace("Output_", "")
                Dim selectionSet As New SelectionSet(successfulItems) With {
                    .DisplayName = $"{extractedName}"
                }

                ' Add the created selection set to the document's selection sets collection
                ActiveDocument.SelectionSets.InsertCopy(0, selectionSet)

                ' Apply the selection set to the current selection in the document
                With ActiveDocument
                    .Models.ResetAllHidden() ' Unhide all model items
                    .CurrentSelection.Clear()
                    .CurrentSelection.CopyFrom(successfulItems)
                End With

            Catch ex As Exception
                ' Display an error message if an exception occurs during processing
                MessageBox.Show("An error when reading has occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return 0
        End Function

        ''' <summary>
        ''' Opens a dialog for the user to select a CSV file and returns the selected file path.
        ''' </summary>
        ''' <returns>The full path of the selected CSV file, or an empty string if no file was selected.</returns>
        Private Function GetCsvFilePath() As String
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
        ''' Represents an element ingested from the CSV file, containing necessary properties.
        ''' </summary>
        Private Class IngestedElement
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
        ''' Ingests elements from a CSV file and returns a list of IngestedElement instances.
        ''' </summary>
        ''' <param name="CsvFilePath">The full path to the CSV file.</param>
        ''' <returns>A list of IngestedElement objects extracted from the CSV.</returns>
        Private Function IngestCsv(CsvFilePath As String) As List(Of IngestedElement)
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
                Dim guidIndex As Integer = Array.IndexOf(headers, "Item.GUID")
                Dim lodIndex As Integer = Array.IndexOf(headers, "LOD")
                Dim missingPropsIndex As Integer = Array.IndexOf(headers, "Missing_Properties")

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
        Private Function SearchElements(elementList As List(Of IngestedElement)) As (Success As ModelItemCollection, Fails As List(Of IngestedElement))
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
                    Dim foundItem As ModelItem = search.FindFirst(ActiveDocument, False)
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

    End Class
#End Region
End Namespace