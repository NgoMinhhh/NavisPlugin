<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnisaControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tabPlugin = New System.Windows.Forms.TabControl()
        Me.tpInfo = New System.Windows.Forms.TabPage()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.cmbLoD = New System.Windows.Forms.ComboBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblLoD = New System.Windows.Forms.Label()
        Me.txbMissingProperties = New System.Windows.Forms.TextBox()
        Me.lblMissingProperpties = New System.Windows.Forms.Label()
        Me.txbGuid = New System.Windows.Forms.TextBox()
        Me.lblGuid = New System.Windows.Forms.Label()
        Me.tpSettings = New System.Windows.Forms.TabPage()
        Me.lblCurrentLoadout = New System.Windows.Forms.Label()
        Me.btnSetUserFolderPath = New System.Windows.Forms.Button()
        Me.txbInfo = New System.Windows.Forms.TextBox()
        Me.btnLoadCsv = New System.Windows.Forms.Button()
        Me.btnExtractProperties = New System.Windows.Forms.Button()
        Me.txbCsvPath = New System.Windows.Forms.TextBox()
        Me.tabPlugin.SuspendLayout()
        Me.tpInfo.SuspendLayout()
        Me.tpSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabPlugin
        '
        Me.tabPlugin.Controls.Add(Me.tpInfo)
        Me.tabPlugin.Controls.Add(Me.tpSettings)
        Me.tabPlugin.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabPlugin.Location = New System.Drawing.Point(0, 0)
        Me.tabPlugin.Name = "tabPlugin"
        Me.tabPlugin.SelectedIndex = 0
        Me.tabPlugin.Size = New System.Drawing.Size(250, 209)
        Me.tabPlugin.TabIndex = 9
        '
        'tpInfo
        '
        Me.tpInfo.Controls.Add(Me.btnSave)
        Me.tpInfo.Controls.Add(Me.cmbStatus)
        Me.tpInfo.Controls.Add(Me.cmbLoD)
        Me.tpInfo.Controls.Add(Me.lblStatus)
        Me.tpInfo.Controls.Add(Me.lblLoD)
        Me.tpInfo.Controls.Add(Me.txbMissingProperties)
        Me.tpInfo.Controls.Add(Me.lblMissingProperpties)
        Me.tpInfo.Controls.Add(Me.txbGuid)
        Me.tpInfo.Controls.Add(Me.lblGuid)
        Me.tpInfo.Location = New System.Drawing.Point(4, 25)
        Me.tpInfo.Name = "tpInfo"
        Me.tpInfo.Padding = New System.Windows.Forms.Padding(3)
        Me.tpInfo.Size = New System.Drawing.Size(242, 180)
        Me.tpInfo.TabIndex = 0
        Me.tpInfo.Text = "Info"
        Me.tpInfo.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSave.Location = New System.Drawing.Point(161, 95)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 11
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cmbStatus
        '
        Me.cmbStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Items.AddRange(New Object() {"Not Verified", "Verified"})
        Me.cmbStatus.Location = New System.Drawing.Point(58, 35)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(178, 24)
        Me.cmbStatus.TabIndex = 10
        '
        'cmbLoD
        '
        Me.cmbLoD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbLoD.FormattingEnabled = True
        Me.cmbLoD.Items.AddRange(New Object() {"300", "200"})
        Me.cmbLoD.Location = New System.Drawing.Point(58, 65)
        Me.cmbLoD.Name = "cmbLoD"
        Me.cmbLoD.Size = New System.Drawing.Size(178, 24)
        Me.cmbLoD.TabIndex = 9
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(10, 38)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(48, 17)
        Me.lblStatus.TabIndex = 8
        Me.lblStatus.Text = "Status"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLoD
        '
        Me.lblLoD.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblLoD.AutoSize = True
        Me.lblLoD.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLoD.Location = New System.Drawing.Point(10, 68)
        Me.lblLoD.Name = "lblLoD"
        Me.lblLoD.Size = New System.Drawing.Size(34, 17)
        Me.lblLoD.TabIndex = 6
        Me.lblLoD.Text = "LoD"
        Me.lblLoD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txbMissingProperties
        '
        Me.txbMissingProperties.AllowDrop = True
        Me.txbMissingProperties.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txbMissingProperties.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txbMissingProperties.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txbMissingProperties.Location = New System.Drawing.Point(6, 124)
        Me.txbMissingProperties.Multiline = True
        Me.txbMissingProperties.Name = "txbMissingProperties"
        Me.txbMissingProperties.ReadOnly = True
        Me.txbMissingProperties.Size = New System.Drawing.Size(230, 49)
        Me.txbMissingProperties.TabIndex = 3
        Me.txbMissingProperties.Text = "No Output is loaded"
        Me.txbMissingProperties.WordWrap = False
        '
        'lblMissingProperpties
        '
        Me.lblMissingProperpties.AutoSize = True
        Me.lblMissingProperpties.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMissingProperpties.Location = New System.Drawing.Point(10, 98)
        Me.lblMissingProperpties.Name = "lblMissingProperpties"
        Me.lblMissingProperpties.Size = New System.Drawing.Size(124, 17)
        Me.lblMissingProperpties.TabIndex = 4
        Me.lblMissingProperpties.Text = "Missing Properties"
        Me.lblMissingProperpties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txbGuid
        '
        Me.txbGuid.AllowDrop = True
        Me.txbGuid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txbGuid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txbGuid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txbGuid.Location = New System.Drawing.Point(58, 6)
        Me.txbGuid.Name = "txbGuid"
        Me.txbGuid.ReadOnly = True
        Me.txbGuid.Size = New System.Drawing.Size(178, 23)
        Me.txbGuid.TabIndex = 0
        Me.txbGuid.Text = "No Output is loaded"
        '
        'lblGuid
        '
        Me.lblGuid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblGuid.AutoSize = True
        Me.lblGuid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGuid.Location = New System.Drawing.Point(10, 8)
        Me.lblGuid.Name = "lblGuid"
        Me.lblGuid.Size = New System.Drawing.Size(42, 17)
        Me.lblGuid.TabIndex = 2
        Me.lblGuid.Text = "GUID"
        Me.lblGuid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tpSettings
        '
        Me.tpSettings.Controls.Add(Me.lblCurrentLoadout)
        Me.tpSettings.Controls.Add(Me.btnSetUserFolderPath)
        Me.tpSettings.Controls.Add(Me.txbInfo)
        Me.tpSettings.Controls.Add(Me.btnLoadCsv)
        Me.tpSettings.Controls.Add(Me.btnExtractProperties)
        Me.tpSettings.Controls.Add(Me.txbCsvPath)
        Me.tpSettings.Location = New System.Drawing.Point(4, 25)
        Me.tpSettings.Name = "tpSettings"
        Me.tpSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSettings.Size = New System.Drawing.Size(242, 180)
        Me.tpSettings.TabIndex = 1
        Me.tpSettings.Text = "Settings"
        Me.tpSettings.UseVisualStyleBackColor = True
        '
        'lblCurrentLoadout
        '
        Me.lblCurrentLoadout.AutoSize = True
        Me.lblCurrentLoadout.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentLoadout.Location = New System.Drawing.Point(3, 104)
        Me.lblCurrentLoadout.Name = "lblCurrentLoadout"
        Me.lblCurrentLoadout.Size = New System.Drawing.Size(111, 17)
        Me.lblCurrentLoadout.TabIndex = 10
        Me.lblCurrentLoadout.Text = "Current Loadout"
        Me.lblCurrentLoadout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnSetUserFolderPath
        '
        Me.btnSetUserFolderPath.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnSetUserFolderPath.Location = New System.Drawing.Point(3, 63)
        Me.btnSetUserFolderPath.Margin = New System.Windows.Forms.Padding(8)
        Me.btnSetUserFolderPath.Name = "btnSetUserFolderPath"
        Me.btnSetUserFolderPath.Size = New System.Drawing.Size(236, 30)
        Me.btnSetUserFolderPath.TabIndex = 2
        Me.btnSetUserFolderPath.Text = "Set AppData Folder"
        Me.btnSetUserFolderPath.UseVisualStyleBackColor = True
        '
        'txbInfo
        '
        Me.txbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txbInfo.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.txbInfo.Location = New System.Drawing.Point(6, 112)
        Me.txbInfo.Multiline = True
        Me.txbInfo.Name = "txbInfo"
        Me.txbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txbInfo.Size = New System.Drawing.Size(13, 10)
        Me.txbInfo.TabIndex = 9
        Me.txbInfo.TabStop = False
        Me.txbInfo.WordWrap = False
        '
        'btnLoadCsv
        '
        Me.btnLoadCsv.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnLoadCsv.Location = New System.Drawing.Point(3, 33)
        Me.btnLoadCsv.Margin = New System.Windows.Forms.Padding(8)
        Me.btnLoadCsv.Name = "btnLoadCsv"
        Me.btnLoadCsv.Size = New System.Drawing.Size(236, 30)
        Me.btnLoadCsv.TabIndex = 3
        Me.btnLoadCsv.Text = "Load Verified Output"
        Me.btnLoadCsv.UseVisualStyleBackColor = True
        '
        'btnExtractProperties
        '
        Me.btnExtractProperties.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnExtractProperties.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnExtractProperties.Location = New System.Drawing.Point(3, 3)
        Me.btnExtractProperties.Margin = New System.Windows.Forms.Padding(8)
        Me.btnExtractProperties.Name = "btnExtractProperties"
        Me.btnExtractProperties.Size = New System.Drawing.Size(236, 30)
        Me.btnExtractProperties.TabIndex = 1
        Me.btnExtractProperties.Text = "Run Verifyer"
        Me.btnExtractProperties.UseVisualStyleBackColor = True
        '
        'txbCsvPath
        '
        Me.txbCsvPath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txbCsvPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txbCsvPath.Enabled = False
        Me.txbCsvPath.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.txbCsvPath.Location = New System.Drawing.Point(3, 121)
        Me.txbCsvPath.Multiline = True
        Me.txbCsvPath.Name = "txbCsvPath"
        Me.txbCsvPath.Size = New System.Drawing.Size(236, 53)
        Me.txbCsvPath.TabIndex = 3
        Me.txbCsvPath.TabStop = False
        '
        'UnisaControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.Controls.Add(Me.tabPlugin)
        Me.MinimumSize = New System.Drawing.Size(250, 200)
        Me.Name = "UnisaControl"
        Me.Size = New System.Drawing.Size(250, 209)
        Me.tabPlugin.ResumeLayout(False)
        Me.tpInfo.ResumeLayout(False)
        Me.tpInfo.PerformLayout()
        Me.tpSettings.ResumeLayout(False)
        Me.tpSettings.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabPlugin As Windows.Forms.TabControl
    Friend WithEvents tpInfo As Windows.Forms.TabPage
    Friend WithEvents tpSettings As Windows.Forms.TabPage
    Friend WithEvents lblGuid As Windows.Forms.Label
    Friend WithEvents lblStatus As Windows.Forms.Label
    Friend WithEvents txbMissingProperties As Windows.Forms.TextBox
    Friend WithEvents lblMissingProperpties As Windows.Forms.Label
    Friend WithEvents txbGuid As Windows.Forms.TextBox
    Friend WithEvents lblLoD As Windows.Forms.Label
    Friend WithEvents btnLoadCsv As Windows.Forms.Button
    Friend WithEvents btnExtractProperties As Windows.Forms.Button
    Friend WithEvents txbCsvPath As Windows.Forms.TextBox
    Friend WithEvents txbInfo As Windows.Forms.TextBox
    Friend WithEvents cmbStatus As Windows.Forms.ComboBox
    Friend WithEvents cmbLoD As Windows.Forms.ComboBox
    Friend WithEvents btnSetUserFolderPath As Windows.Forms.Button
    Friend WithEvents btnSave As Windows.Forms.Button
    Friend WithEvents lblCurrentLoadout As Windows.Forms.Label
End Class
