Imports Autodesk.Navisworks.Api.Application
Imports Autodesk.Navisworks.Api.Plugins
Imports System.Windows.Forms

Namespace IngestFilePlugin
    <Plugin("IngestFilePlugin",                                      ' Plugin name
            "CAPS",                                             ' 4 character Developer ID or GUID
            ToolTip:="UNISA",       ' The tooltip for the item in the ribbon
            DisplayName:="Extract Properties Plugin")>          ' Display name for the Plugin in the Ribbon
    Public Class IngestFilePlugin
        Inherits AddInPlugin

        Public Overrides Function Execute(ParamArray parameters() As String) As Integer
            Throw New NotImplementedException()
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
    End Class
End Namespace