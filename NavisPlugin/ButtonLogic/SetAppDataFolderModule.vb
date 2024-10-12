Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Provides functionalities for user interactions related to user folder path selection and validation.
''' </summary>
Module SetAppDataFolderModule

    ''' <summary>
    ''' Prompts the user to select a base folder, validates it, ensures required subfolders exist,
    ''' and updates the application settings with the new folder path.
    ''' Automatically creates 'ExtractData' and 'AlgoOutput' subfolders within the selected base folder.
    ''' </summary>
    Public Sub SetUserFolderPath()
        Try
            ' Initialize and configure the FolderBrowserDialog
            Using folderDialog As New FolderBrowserDialog()
                folderDialog.Description = "Select the base folder for application data."
                folderDialog.ShowNewFolderButton = True

                ' Pre-select the current folder path if it exists; otherwise, default to the user's Documents folder
                If Directory.Exists(My.Settings.UserFolderPath) Then
                    folderDialog.SelectedPath = My.Settings.UserFolderPath
                Else
                    folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                End If

                ' Show the dialog and process the result
                If folderDialog.ShowDialog() = DialogResult.OK Then
                    Dim selectedPath As String = folderDialog.SelectedPath

                    If Not Directory.Exists(Path.Combine(selectedPath, "UnisaLoDPlugin")) Then
                        Directory.CreateDirectory(Path.Combine(selectedPath, "UnisaLoDPlugin"))
                    End If

                    ' Define the required subfolders
                    If Not Directory.Exists(Path.Combine(selectedPath, "UnisaLoDPlugin", "ExtractData")) Then
                        Directory.CreateDirectory(Path.Combine(selectedPath, "UnisaLoDPlugin", "ExtractData"))
                    End If

                    If Not Directory.Exists(Path.Combine(selectedPath, "UnisaLoDPlugin", "AlgoOutput")) Then
                        Directory.CreateDirectory(Path.Combine(selectedPath, "UnisaLoDPlugin", "AlgoOutput"))
                    End If

                    My.Settings.UserFolderPath = Path.Combine(selectedPath, "UnisaLoDPlugin").ToString()
                    My.Settings.Save()

                    MessageBox.Show("Folder path and subfolders set successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Set User Folder Path is cancelled", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show($"An error occurred while changing the folder path: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Module
