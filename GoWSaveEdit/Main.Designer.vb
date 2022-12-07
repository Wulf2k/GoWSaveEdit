<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
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
        Me.btnGoW1 = New System.Windows.Forms.Button()
        Me.btnGoW2 = New System.Windows.Forms.Button()
        Me.btnGoWA = New System.Windows.Forms.Button()
        Me.btnGoW3 = New System.Windows.Forms.Button()
        Me.btnSendMoney = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnGoW1
        '
        Me.btnGoW1.Location = New System.Drawing.Point(12, 12)
        Me.btnGoW1.Name = "btnGoW1"
        Me.btnGoW1.Size = New System.Drawing.Size(106, 23)
        Me.btnGoW1.TabIndex = 0
        Me.btnGoW1.Text = "God of War 1"
        Me.btnGoW1.UseVisualStyleBackColor = True
        '
        'btnGoW2
        '
        Me.btnGoW2.Location = New System.Drawing.Point(152, 12)
        Me.btnGoW2.Name = "btnGoW2"
        Me.btnGoW2.Size = New System.Drawing.Size(106, 23)
        Me.btnGoW2.TabIndex = 1
        Me.btnGoW2.Text = "God of War 2"
        Me.btnGoW2.UseVisualStyleBackColor = True
        '
        'btnGoWA
        '
        Me.btnGoWA.Location = New System.Drawing.Point(152, 41)
        Me.btnGoWA.Name = "btnGoWA"
        Me.btnGoWA.Size = New System.Drawing.Size(106, 35)
        Me.btnGoWA.TabIndex = 2
        Me.btnGoWA.Text = "God of War Ascension"
        Me.btnGoWA.UseVisualStyleBackColor = True
        '
        'btnGoW3
        '
        Me.btnGoW3.Enabled = False
        Me.btnGoW3.Location = New System.Drawing.Point(12, 41)
        Me.btnGoW3.Name = "btnGoW3"
        Me.btnGoW3.Size = New System.Drawing.Size(106, 35)
        Me.btnGoW3.TabIndex = 3
        Me.btnGoW3.Text = "God of War 3"
        Me.btnGoW3.UseVisualStyleBackColor = True
        '
        'btnSendMoney
        '
        Me.btnSendMoney.Location = New System.Drawing.Point(71, 122)
        Me.btnSendMoney.Name = "btnSendMoney"
        Me.btnSendMoney.Size = New System.Drawing.Size(126, 42)
        Me.btnSendMoney.TabIndex = 4
        Me.btnSendMoney.Text = "Send Wulf Stripper Money via Paypal"
        Me.btnSendMoney.UseVisualStyleBackColor = True
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(270, 176)
        Me.Controls.Add(Me.btnSendMoney)
        Me.Controls.Add(Me.btnGoW3)
        Me.Controls.Add(Me.btnGoWA)
        Me.Controls.Add(Me.btnGoW2)
        Me.Controls.Add(Me.btnGoW1)
        Me.Name = "Main"
        Me.Text = "Wulf's God of War Editor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnGoW1 As System.Windows.Forms.Button
    Friend WithEvents btnGoW2 As System.Windows.Forms.Button

    Private Sub btnGoW1_Click(sender As System.Object, e As System.EventArgs) Handles btnGoW1.Click
        Dim G1 As GoW1
        G1 = New GoW1
        G1.Show()
        Me.Hide()
    End Sub

    Private Sub btnGoW2_Click(sender As System.Object, e As System.EventArgs) Handles btnGoW2.Click
        Dim G2 As GoW2
        G2 = New GoW2
        G2.Show()
        Me.Hide()
    End Sub
    Private Sub btnGoW3_Click(sender As Object, e As EventArgs) Handles btnGoW3.Click
        Dim G3 As GoW3
        G3 = New GoW3
        G3.Show()
        Me.Hide()
    End Sub
    Private Sub btnGoWA_Click(sender As Object, e As EventArgs) Handles btnGoWA.Click
        Dim GA As GoWA
        GA = New GoWA
        GA.Show()
        Me.Hide()
    End Sub
    Friend WithEvents btnGoWA As System.Windows.Forms.Button
    Friend WithEvents btnGoW3 As System.Windows.Forms.Button
    Friend WithEvents btnSendMoney As System.Windows.Forms.Button
End Class
