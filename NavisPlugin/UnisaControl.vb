Imports System.IO
Imports System.Windows.Forms
Imports Autodesk.Navisworks.Api



Public Class UnisaControl
    Inherits UserControl

    ''' <summary>
    ''' A variable for valid verified elements loaded from csv for sharing between function
    ''' </summary>
    Private Shared CurrIngestedElements As List(Of IngestedElement)
    Private Shared LodIndex As Integer = -1
    Private Shared GuidIndex As Integer = -1

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Ask for User Folder Path to store AppData if there is none
        If My.Settings.UserFolderPath Is Nothing Then
            SetAppDataFolderModule.SetUserFolderPath()
        End If

        ' Event listener to detect change in current selection and update info panel accordingly
        AddHandler Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.Changed, AddressOf UpdateInfoTab

    End Sub
    ''' <summary>
    ''' Event that resizes the controls on dock pane resizing
    ''' </summary>
    Protected Overrides Sub OnParentChanged(e As EventArgs)
        MyBase.OnParentChanged(e)
        Dock = DockStyle.Fill
    End Sub

    ''' <summary>
    ''' This sub will update the info tab based on matching GUID from loaded CSV, subscribed to changing of current selection event
    ''' </summary>
    Private Sub UpdateInfoTab()

        ' Check if there has been any output loaded
        Try
            Dim dummyCheck As Boolean = CurrIngestedElements.Count > 0
        Catch ex As Exception
            Exit Sub
        End Try

        ' Reset the display
        txbGuid.Clear()
        cmbStatus.Text = String.Empty
        cmbLoD.Text = String.Empty
        txbMissingProperties.Clear()
        BtnSave.Text = "Save"
        BtnSave.Enabled = False

        ' Detect multiple selection to change save button to bulk save
        Dim currentSelection As ModelItemCollection = Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems
        Select Case currentSelection.Count
            Case 1
                Try
                    Dim currentGuid As String = currentSelection.First.PropertyCategories.FindPropertyByName(PropertyCategoryNames.Item, DataPropertyNames.ItemGuid).Value.ToDisplayString()
                    For Each ingestedElement As IngestedElement In CurrIngestedElements
                        If currentGuid = ingestedElement.GUID Then
                            txbGuid.Text = ingestedElement.GUID
                            cmbLoD.SelectedText = ingestedElement.LOD
                            txbMissingProperties.Text = ingestedElement.MissingProperties

                            If String.IsNullOrWhiteSpace(ingestedElement.LOD) Then
                                cmbStatus.SelectedText = "Not Verified"
                            Else
                                cmbStatus.SelectedText = "Verified"
                            End If

                            ' Enable Save Button
                            BtnSave.Enabled = True
                            Exit For
                        Else
                            BtnSave.Enabled = False
                        End If

                    Next

                Catch ex As Exception
                    cmbStatus.SelectedText = "Error"
                End Try
            Case Is >= 2
                Dim selectedGuids As New List(Of String)()
                For Each modelItem In currentSelection
                    selectedGuids.Add(modelItem.PropertyCategories.FindPropertyByName(PropertyCategoryNames.Item, DataPropertyNames.ItemGuid).Value.ToDisplayString())
                Next

                Dim ingestedGuids As New List(Of String)()
                For Each ingestedElement In CurrIngestedElements
                    ingestedGuids.Add(ingestedElement.GUID)
                Next
                ' Create a HashSet from ingestedGuids for efficient lookups
                Dim ingestedGuidsSet As New HashSet(Of String)(ingestedGuids)

                ' Check if all selectedGuids are in ingestedGuids
                Dim allFound As Boolean = selectedGuids.All(Function(sg) ingestedGuidsSet.Contains(sg))
                If allFound Then
                    txbGuid.Text = "Bulk Edit Mode"
                    txbMissingProperties.Text = "Bulk Edit Mode"
                    BtnSave.Enabled = True
                    BtnSave.Text = "Bulk Save"
                End If
            Case Else
                Exit Sub
        End Select


    End Sub

    Private Sub BtnLoadVerifyerOutput_Click(sender As Object, e As EventArgs) Handles BtnLoadVerifyerOutput.Click
        Dim CsvFilePath As String = GetCsvFilePath()
        If CsvFilePath Is String.Empty Then
            MessageBox.Show("No CSV file is chosen!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Try
            ' Ingest elements from the selected CSV file
            Dim result As (Content As List(Of IngestedElement), guidIndex As Integer, lodIndex As Integer) = IngestCsv(CsvFilePath)

            CurrIngestedElements = result.Content
            LodIndex = result.lodIndex
            GuidIndex = result.guidIndex

            ' Use output filename as folder for selection Sets
            Dim folderName As String = Path.GetFileNameWithoutExtension(CsvFilePath)
            CreateLoDSelectionSets(folderName, CurrIngestedElements)

            ''' Apply the selection set to the current selection in the document
            'With Autodesk.Navisworks.Api.Application.ActiveDocument
            '    .Models.ResetAllHidden() ' Unhide all model items
            '    .CurrentSelection.Clear()
            '    .CurrentSelection.CopyFrom(folderName)
            'End With

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
            SetAppDataFolderModule.SetUserFolderPath()
        End If
    End Sub

    Private Sub BtnRunVerifyer_Click(sender As Object, e As EventArgs) Handles BtnRunVerifyer.Click
        Try
            Dim filepath = WritePropertiesToCsv()
            RunLodVerifyer(filepath)
            MessageBox.Show("Run Verifyer is successful.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click

        ' Ask for confirmation
        Dim result As DialogResult = MessageBox.Show("You are bulk editing?" & vbNewLine &
                                                     "Please double check selected elements in before proceeding." & vbNewLine &
                                                     "There is no undo this action!",
                                                     "Confirm Bulk Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            ' User confirmed, proceed with bulk editing
            MessageBox.Show("Bulk editing operation canceled.", "Operation Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        If LodIndex < 0 Or GuidIndex < 0 Then
            MessageBox.Show("Please load a Verified Output first!", "Saving Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Dim currentSelection As ModelItemCollection = Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems
        Dim selectedElements As New List(Of IngestedElement)()
        For Each selectItem In currentSelection
            selectGuid = GetPropertyValueForCSV(selectItem, "Item", "GUID")
            For Each currIngestedElement In CurrIngestedElements
                If selectGuid = currIngestedElement.GUID Then
                    currIngestedElement.LOD = cmbLoD.Text
                    currIngestedElement.SearchResult = "Verified"
                    selectedElements.Add(currIngestedElement)
                    Exit For
                End If
            Next
        Next

        Try
            WriteUpdatedLoDtoCSV(selectedElements, txbCsvPath.Text, GuidIndex, LodIndex)
            CreateLoDSelectionSets(Path.GetFileNameWithoutExtension(txbCsvPath.Text), CurrIngestedElements)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        MessageBox.Show("Bulk editing successfully", "Successful Operation", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class