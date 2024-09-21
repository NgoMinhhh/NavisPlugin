Imports Autodesk.Navisworks.Api
Imports Autodesk.Navisworks.Api.Plugins
Imports Autodesk.Navisworks.Api.Application
Imports Autodesk.Navisworks.Api.DocumentParts
Imports System.Text
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.VisualBasic.FileIO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Header
Imports Autodesk.Navisworks.Api.Interop

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

        Public Overrides Function Execute(ParamArray parameters() As String) As Integer
            Dim CsvFilePath As String = GetCsvFilePath()
            If CsvFilePath Is String.Empty Then
                MessageBox.Show("No CSV file is chosen!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return 0
            End If

            Try
                Dim elementList As List(Of VerifiedElement) = IngestCsv(CsvFilePath)
                For Each element In elementList
                    Debug.Print(element.ToString())
                Next
            Catch ex As Exception
                MessageBox.Show("An error when reading has occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            Return 0
        End Function

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

        Private Class VerifiedElement
            ' Custom class to store only neccessary info from csv
            Public Property GUID As String
            Public Property LOD As String
            Public Property MissingProperties As String
            Public Overrides Function ToString() As String
                Return $"GUID: {GUID}, LOD: {LOD}, Missing Properties: {MissingProperties}"
            End Function
        End Class

        Private Function IngestCsv(CsvFilePath As String) As List(Of VerifiedElement)
            Dim elementList As New List(Of VerifiedElement)()
            Dim failList As New StringBuilder
            ' Initialize the TextFieldParser
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

                While Not parser.EndOfData
                    Try
                        Dim fields As String() = parser.ReadFields()
                        ' Ensure the line has enough fields
                        If fields.Length > Math.Max(guidIndex, Math.Max(lodIndex, missingPropsIndex)) Then
                            Dim item As New VerifiedElement() With {
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
                        failList.Append("Line " & ex.Message)
                    End Try
                End While
            End Using
            ' Display failed lines
            If Not String.IsNullOrEmpty(failList.ToString()) Then
                MessageBox.Show("List of failed lines: " & failList.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Return elementList
        End Function

    End Class
#End Region
End Namespace