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
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.txbSetUserFolderPath = New System.Windows.Forms.Button()
        Me.txbInfo = New System.Windows.Forms.TextBox()
        Me.btnLoadCsv = New System.Windows.Forms.Button()
        Me.btnExtractProperties = New System.Windows.Forms.Button()
        Me.txbCsvPath = New System.Windows.Forms.TextBox()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(250, 250)
        Me.TabControl1.TabIndex = 9
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ComboBox2)
        Me.TabPage1.Controls.Add(Me.ComboBox1)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.TextBox1)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.TextBox2)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(242, 221)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Info"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ComboBox2
        '
        Me.ComboBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(58, 44)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(178, 24)
        Me.ComboBox2.TabIndex = 10
        '
        'ComboBox1
        '
        Me.ComboBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(58, 74)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(178, 24)
        Me.ComboBox1.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 47)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 17)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Status"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(10, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 17)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "LoD"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextBox1
        '
        Me.TextBox1.AllowDrop = True
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(3, 133)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(236, 85)
        Me.TextBox1.TabIndex = 3
        Me.TextBox1.Text = "123-456-789"
        Me.TextBox1.WordWrap = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(10, 113)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Missing Properties:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextBox2
        '
        Me.TextBox2.AllowDrop = True
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(58, 15)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(178, 23)
        Me.TextBox2.TabIndex = 0
        Me.TextBox2.Text = "123-456-789"
        Me.TextBox2.WordWrap = False
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 17)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "GUID"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.txbSetUserFolderPath)
        Me.TabPage2.Controls.Add(Me.txbInfo)
        Me.TabPage2.Controls.Add(Me.btnLoadCsv)
        Me.TabPage2.Controls.Add(Me.btnExtractProperties)
        Me.TabPage2.Controls.Add(Me.txbCsvPath)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(242, 221)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'txbSetUserFolderPath
        '
        Me.txbSetUserFolderPath.Dock = System.Windows.Forms.DockStyle.Top
        Me.txbSetUserFolderPath.Location = New System.Drawing.Point(3, 63)
        Me.txbSetUserFolderPath.Margin = New System.Windows.Forms.Padding(8)
        Me.txbSetUserFolderPath.Name = "txbSetUserFolderPath"
        Me.txbSetUserFolderPath.Size = New System.Drawing.Size(236, 30)
        Me.txbSetUserFolderPath.TabIndex = 2
        Me.txbSetUserFolderPath.Text = "Set AppData Folder"
        Me.txbSetUserFolderPath.UseVisualStyleBackColor = True
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
        Me.btnLoadCsv.Text = "Load Algorithm Output"
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
        Me.btnExtractProperties.Text = "Extract Properties"
        Me.btnExtractProperties.UseVisualStyleBackColor = True
        '
        'txbCsvPath
        '
        Me.txbCsvPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txbCsvPath.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txbCsvPath.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.txbCsvPath.Location = New System.Drawing.Point(3, 98)
        Me.txbCsvPath.Multiline = True
        Me.txbCsvPath.Name = "txbCsvPath"
        Me.txbCsvPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txbCsvPath.Size = New System.Drawing.Size(236, 120)
        Me.txbCsvPath.TabIndex = 3
        Me.txbCsvPath.TabStop = False
        '
        'UnisaControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.Controls.Add(Me.TabControl1)
        Me.MinimumSize = New System.Drawing.Size(240, 250)
        Me.Name = "UnisaControl"
        Me.Size = New System.Drawing.Size(250, 250)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As Windows.Forms.TabControl
    Friend WithEvents TabPage1 As Windows.Forms.TabPage
    Friend WithEvents TabPage2 As Windows.Forms.TabPage
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents TextBox1 As Windows.Forms.TextBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents TextBox2 As Windows.Forms.TextBox
    Private WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents btnLoadCsv As Windows.Forms.Button
    Friend WithEvents btnExtractProperties As Windows.Forms.Button
    Friend WithEvents txbCsvPath As Windows.Forms.TextBox
    Friend WithEvents txbInfo As Windows.Forms.TextBox
    Friend WithEvents ComboBox2 As Windows.Forms.ComboBox
    Friend WithEvents ComboBox1 As Windows.Forms.ComboBox
    Friend WithEvents txbSetUserFolderPath As Windows.Forms.Button
End Class
