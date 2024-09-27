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
        Me.txbCsvPath = New System.Windows.Forms.TextBox()
        Me.btnLoadCsv = New System.Windows.Forms.Button()
        Me.btnSelectElements = New System.Windows.Forms.Button()
        Me.txbInfo = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txbCsvPath
        '
        Me.txbCsvPath.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txbCsvPath.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txbCsvPath.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.txbCsvPath.Location = New System.Drawing.Point(0, 174)
        Me.txbCsvPath.Multiline = True
        Me.txbCsvPath.Name = "txbCsvPath"
        Me.txbCsvPath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txbCsvPath.Size = New System.Drawing.Size(275, 52)
        Me.txbCsvPath.TabIndex = 0
        Me.txbCsvPath.TabStop = False
        '
        'btnLoadCsv
        '
        Me.btnLoadCsv.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnLoadCsv.Location = New System.Drawing.Point(0, 141)
        Me.btnLoadCsv.Name = "btnLoadCsv"
        Me.btnLoadCsv.Size = New System.Drawing.Size(275, 33)
        Me.btnLoadCsv.TabIndex = 1
        Me.btnLoadCsv.Text = "Load Algorithm Output"
        Me.btnLoadCsv.UseVisualStyleBackColor = True
        '
        'btnSelectElements
        '
        Me.btnSelectElements.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnSelectElements.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnSelectElements.Location = New System.Drawing.Point(0, 111)
        Me.btnSelectElements.MinimumSize = New System.Drawing.Size(250, 30)
        Me.btnSelectElements.Name = "btnSelectElements"
        Me.btnSelectElements.Size = New System.Drawing.Size(275, 30)
        Me.btnSelectElements.TabIndex = 2
        Me.btnSelectElements.Text = "Select Verifed Elements"
        Me.btnSelectElements.UseVisualStyleBackColor = True
        '
        'txbInfo
        '
        Me.txbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txbInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txbInfo.Location = New System.Drawing.Point(0, 0)
        Me.txbInfo.Multiline = True
        Me.txbInfo.Name = "txbInfo"
        Me.txbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txbInfo.Size = New System.Drawing.Size(275, 111)
        Me.txbInfo.TabIndex = 3
        Me.txbInfo.Text = "GUID  >" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Status > Is not Verified" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "LoD    > 300" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Missing Properties > " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " - Roof, " &
    "Door, Thickiness"
        Me.txbInfo.WordWrap = False
        '
        'UnisaControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.Controls.Add(Me.txbInfo)
        Me.Controls.Add(Me.btnSelectElements)
        Me.Controls.Add(Me.btnLoadCsv)
        Me.Controls.Add(Me.txbCsvPath)
        Me.MinimumSize = New System.Drawing.Size(250, 200)
        Me.Name = "UnisaControl"
        Me.Size = New System.Drawing.Size(275, 226)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txbCsvPath As Windows.Forms.TextBox
    Friend WithEvents btnLoadCsv As Windows.Forms.Button
    Friend WithEvents btnSelectElements As Windows.Forms.Button
    Friend WithEvents txbInfo As Windows.Forms.TextBox
End Class
