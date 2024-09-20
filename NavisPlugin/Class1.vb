Imports Autodesk.Navisworks.Api
Imports Autodesk.Navisworks.Api.Plugins
Imports Autodesk.Navisworks.Api.Application
Imports Autodesk.Navisworks.Api.DocumentParts
Imports System.Text
Imports System.IO
Imports System.Windows.Forms

Namespace BasicPlugIn
    <Plugin("NavisPlugin",                                      ' Plugin name
            "CAPS",                                             ' 4 character Developer ID or GUID
            ToolTip:="UNISA",       ' The tooltip for the item in the ribbon
            DisplayName:="Extract Properties Plugin")>          ' Display name for the Plugin in the Ribbon
    Public Class ABasicPlugin
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

        Public Overrides Function Execute(ParamArray parameters() As String) As Integer

            ' Initialize a variable to hold the selected folder path
            Dim outputFolderPath As String = String.Empty

            ' Create an instance of FolderSelector
            Dim selector As New FolderSelector()

            ' Prompt the user to select the output folder
            selector.AskUserToSelectOutputFolder(outputFolderPath)

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

    Public Class FolderSelector
        ' Prompts the user to select an output folder and returns the selected path.
        ' "selectedFolderPath">A variable to store the selected folder path.

        Public Sub AskUserToSelectOutputFolder(ByRef selectedFolderPath As String)
            ' Initialize a new instance of FolderBrowserDialog
            Using folderBrowser As New FolderBrowserDialog()
                ' Set the description that appears above the tree view
                folderBrowser.Description = "Select the output folder for the CSV file."

                ' Allow the user to create new folders
                folderBrowser.ShowNewFolderButton = True

                ' Show the dialog and check if the user clicked OK
                If folderBrowser.ShowDialog() = DialogResult.OK Then
                    ' Ensure that the selected path is not empty or whitespace
                    If Not String.IsNullOrWhiteSpace(folderBrowser.SelectedPath) Then
                        ' Assign the selected path to the output parameter
                        selectedFolderPath = folderBrowser.SelectedPath
                    Else
                        ' Handle the case where no folder was selected
                        MessageBox.Show("No folder was selected. Please try again.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        selectedFolderPath = String.Empty
                    End If
                Else
                    ' Handle the case where the user canceled the dialog
                    MessageBox.Show("Folder selection was canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    selectedFolderPath = String.Empty
                End If
            End Using
        End Sub

    End Class

End Namespace