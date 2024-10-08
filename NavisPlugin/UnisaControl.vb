Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports Autodesk.Navisworks.Api
Imports Microsoft.VisualBasic.FileIO



Public Class UnisaControl
    Inherits UserControl

    ''' <summary>
    ''' A variable for valid verified elements loaded from csv for sharing between function
    ''' </summary>
    Private Shared CurrIngestedElements As List(Of IngestedElement)
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Ask for User Folder Path to store AppData if there is none
        If My.Settings.UserFolderPath Is Nothing Then
            UserFolderPathModule.SetUserFolderPath()
        End If

        ' Event listener to detect change in current selection and update info panel accordingly
        AddHandler Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.Changed, AddressOf GetCurrentElementLoDInfo

    End Sub

    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyBase.OnParentChanged(e)
        Dock = DockStyle.Fill
    End Sub

    Private Sub GetCurrentElementLoDInfo()
        Dim currentElement As ModelItem = Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems.First
        If currentElement Is Nothing Or CurrIngestedElements Is Nothing Then
            Exit Sub
        End If

        ' Reset the display
        txbGuid.Clear()
        cmbStatus.Text = String.Empty
        cmbLoD.Text = String.Empty
        txbMissingProperties.Clear()
        Try
            Dim currentGuid As String = currentElement.PropertyCategories.FindPropertyByName(PropertyCategoryNames.Item, DataPropertyNames.ItemGuid).Value.ToDisplayString()
            Dim isVerified As Boolean = False
            For Each ingestedElement As IngestedElement In CurrIngestedElements
                If currentGuid = ingestedElement.GUID Then
                    txbGuid.Text = ingestedElement.GUID
                    cmbStatus.SelectedText = "Verified"
                    cmbLoD.SelectedText = ingestedElement.LOD
                    txbMissingProperties.Text = ingestedElement.MissingProperties
                    isVerified = True
                    Exit For
                End If

            Next

            If Not isVerified Then
                cmbStatus.SelectedText = "Not Verified"
            End If
        Catch ex As Exception
            cmbStatus.SelectedText = "Error"
        End Try
    End Sub

    Private Sub btnLoadCsv_Click(sender As Object, e As EventArgs) Handles btnLoadCsv.Click
        Dim CsvFilePath As String = GetCsvFilePath()
        If CsvFilePath Is String.Empty Then
            MessageBox.Show("No CSV file is chosen!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Try
            ' Ingest elements from the selected CSV file
            CurrIngestedElements = IngestCsv(CsvFilePath)

            ' Declare variables to hold the results
            Dim successfulItems As ModelItemCollection
            Dim failedElements As List(Of IngestedElement)

            ' Call the SearchElements function and assign the results
            Dim result As (Success As ModelItemCollection, Fails As List(Of IngestedElement)) = SearchElements(CurrIngestedElements)
            successfulItems = result.Success
            failedElements = result.Fails

            ' Extract a name portion from the CSV filename for the selection set
            Dim extractedName As String = Path.GetFileNameWithoutExtension(CsvFilePath).Replace("Output_", "")
            Dim selectionSet As New SelectionSet(successfulItems) With {
                .DisplayName = $"{extractedName}"
            }

            ' Add the created selection set to the document's selection sets collection
            Autodesk.Navisworks.Api.Application.ActiveDocument.SelectionSets.InsertCopy(0, selectionSet)

            ' Apply the selection set to the current selection in the document
            With Autodesk.Navisworks.Api.Application.ActiveDocument
                .Models.ResetAllHidden() ' Unhide all model items
                .CurrentSelection.Clear()
                .CurrentSelection.CopyFrom(successfulItems)
            End With

            ' Change content of texbox displaying current file path
            txbCsvPath.Text = CsvFilePath
        Catch ex As Exception
            ' Display an error message if an exception occurs during processing
            MessageBox.Show("An error when reading has occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub

    Private Sub btnSetUserFolderPath_Click(sender As Object, e As EventArgs) Handles btnSetUserFolderPath.Click
        Dim result As DialogResult = MessageBox.Show($"The Current AppData folder is" & vbNewLine &
                                                     My.Settings.UserFolderPath & vbNewLine &
                                                    "Do you want to change the settings?",
                                                     "Confirmation",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            ' User chose "Yes" which we'll interpret as "Change"
            UserFolderPathModule.SetUserFolderPath()
        End If
    End Sub

    Private Sub btnExtractProperties_Click(sender As Object, e As EventArgs) Handles btnExtractProperties.Click
        Try
            Dim filepath = WritePropertiesToCsv()
            RunLodVerifyer(filepath)
            'MessageBox.Show("Extraction is successful")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        MessageBox.Show("Result Saved Successfully!", "Save")
    End Sub
End Class