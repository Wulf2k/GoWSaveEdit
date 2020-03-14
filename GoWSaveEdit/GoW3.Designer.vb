<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GoW3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.btnG3Browse = New System.Windows.Forms.Button()
        Me.btnG3Save = New System.Windows.Forms.Button()
        Me.btnG3Open = New System.Windows.Forms.Button()
        Me.lblG1File = New System.Windows.Forms.Label()
        Me.txtG3File = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnG3Browse
        '
        Me.btnG3Browse.Location = New System.Drawing.Point(492, 10)
        Me.btnG3Browse.Name = "btnG3Browse"
        Me.btnG3Browse.Size = New System.Drawing.Size(75, 23)
        Me.btnG3Browse.TabIndex = 5
        Me.btnG3Browse.Text = "Browse"
        Me.btnG3Browse.UseVisualStyleBackColor = True
        '
        'btnG3Save
        '
        Me.btnG3Save.Location = New System.Drawing.Point(492, 46)
        Me.btnG3Save.Name = "btnG3Save"
        Me.btnG3Save.Size = New System.Drawing.Size(75, 23)
        Me.btnG3Save.TabIndex = 8
        Me.btnG3Save.Text = "Save"
        Me.btnG3Save.UseVisualStyleBackColor = True
        '
        'btnG3Open
        '
        Me.btnG3Open.Location = New System.Drawing.Point(414, 46)
        Me.btnG3Open.Name = "btnG3Open"
        Me.btnG3Open.Size = New System.Drawing.Size(75, 23)
        Me.btnG3Open.TabIndex = 7
        Me.btnG3Open.Text = "Open"
        Me.btnG3Open.UseVisualStyleBackColor = True
        '
        'lblG1File
        '
        Me.lblG1File.AutoSize = True
        Me.lblG1File.Location = New System.Drawing.Point(17, 15)
        Me.lblG1File.Name = "lblG1File"
        Me.lblG1File.Size = New System.Drawing.Size(26, 13)
        Me.lblG1File.TabIndex = 6
        Me.lblG1File.Text = "File:"
        '
        'txtG3File
        '
        Me.txtG3File.Location = New System.Drawing.Point(49, 12)
        Me.txtG3File.Name = "txtG3File"
        Me.txtG3File.Size = New System.Drawing.Size(440, 20)
        Me.txtG3File.TabIndex = 4
        Me.txtG3File.Text = "C:\Users\misc\Desktop\GoW\GoW3\misc-SAVEDATA\BCES00510-100328130044"
        '
        'GoW3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(978, 581)
        Me.Controls.Add(Me.btnG3Browse)
        Me.Controls.Add(Me.btnG3Save)
        Me.Controls.Add(Me.btnG3Open)
        Me.Controls.Add(Me.lblG1File)
        Me.Controls.Add(Me.txtG3File)
        Me.Name = "GoW3"
        Me.Text = "Wulf's God of War 3 Editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnG3Browse As System.Windows.Forms.Button
    Friend WithEvents btnG3Save As System.Windows.Forms.Button
    Friend WithEvents btnG3Open As System.Windows.Forms.Button
    Friend WithEvents lblG1File As System.Windows.Forms.Label
    Friend WithEvents txtG3File As System.Windows.Forms.TextBox
End Class
