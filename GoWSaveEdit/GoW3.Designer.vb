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
        Me.tabSaves = New System.Windows.Forms.TabControl()
        Me.tabUser = New System.Windows.Forms.TabPage()
        Me.gpDefCostume = New System.Windows.Forms.GroupBox()
        Me.rbCost1 = New System.Windows.Forms.RadioButton()
        Me.gpChallsBeaten = New System.Windows.Forms.GroupBox()
        Me.CheckBox20 = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.CheckBox21 = New System.Windows.Forms.CheckBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.CheckBox22 = New System.Windows.Forms.CheckBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.CheckBox18 = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.CheckBox19 = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CheckBox17 = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CheckBox16 = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.gpDiffsBeaten = New System.Windows.Forms.GroupBox()
        Me.CheckBox14 = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.CheckBox15 = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CheckBox13 = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CheckBox12 = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tabSave = New System.Windows.Forms.TabPage()
        Me.rbCost2 = New System.Windows.Forms.RadioButton()
        Me.rbCost3 = New System.Windows.Forms.RadioButton()
        Me.rbCost4 = New System.Windows.Forms.RadioButton()
        Me.rbCost5 = New System.Windows.Forms.RadioButton()
        Me.rbCost6 = New System.Windows.Forms.RadioButton()
        Me.rbCost7 = New System.Windows.Forms.RadioButton()
        Me.rbCost8 = New System.Windows.Forms.RadioButton()
        Me.tabSaves.SuspendLayout()
        Me.tabUser.SuspendLayout()
        Me.gpDefCostume.SuspendLayout()
        Me.gpChallsBeaten.SuspendLayout()
        Me.gpDiffsBeaten.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnG3Browse
        '
        Me.btnG3Browse.Location = New System.Drawing.Point(492, 10)
        Me.btnG3Browse.Name = "btnG3Browse"
        Me.btnG3Browse.Size = New System.Drawing.Size(75, 23)
        Me.btnG3Browse.TabIndex = 1
        Me.btnG3Browse.Text = "Browse"
        Me.btnG3Browse.UseVisualStyleBackColor = True
        '
        'btnG3Save
        '
        Me.btnG3Save.Location = New System.Drawing.Point(492, 46)
        Me.btnG3Save.Name = "btnG3Save"
        Me.btnG3Save.Size = New System.Drawing.Size(75, 23)
        Me.btnG3Save.TabIndex = 3
        Me.btnG3Save.Text = "Save"
        Me.btnG3Save.UseVisualStyleBackColor = True
        '
        'btnG3Open
        '
        Me.btnG3Open.Location = New System.Drawing.Point(414, 46)
        Me.btnG3Open.Name = "btnG3Open"
        Me.btnG3Open.Size = New System.Drawing.Size(75, 23)
        Me.btnG3Open.TabIndex = 2
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
        Me.txtG3File.TabIndex = 0
        Me.txtG3File.Text = "D:\emus\ps3\dev_hdd0\home\00000001\savedata\BCUS98111-USERDATA\Param.sfo"
        '
        'tabSaves
        '
        Me.tabSaves.Controls.Add(Me.tabUser)
        Me.tabSaves.Controls.Add(Me.tabSave)
        Me.tabSaves.Location = New System.Drawing.Point(12, 75)
        Me.tabSaves.Name = "tabSaves"
        Me.tabSaves.SelectedIndex = 0
        Me.tabSaves.Size = New System.Drawing.Size(1053, 785)
        Me.tabSaves.TabIndex = 9
        '
        'tabUser
        '
        Me.tabUser.Controls.Add(Me.gpDefCostume)
        Me.tabUser.Controls.Add(Me.gpChallsBeaten)
        Me.tabUser.Controls.Add(Me.gpDiffsBeaten)
        Me.tabUser.Location = New System.Drawing.Point(4, 22)
        Me.tabUser.Name = "tabUser"
        Me.tabUser.Padding = New System.Windows.Forms.Padding(3)
        Me.tabUser.Size = New System.Drawing.Size(1045, 759)
        Me.tabUser.TabIndex = 0
        Me.tabUser.Text = "UserData"
        Me.tabUser.UseVisualStyleBackColor = True
        '
        'gpDefCostume
        '
        Me.gpDefCostume.Controls.Add(Me.rbCost8)
        Me.gpDefCostume.Controls.Add(Me.rbCost7)
        Me.gpDefCostume.Controls.Add(Me.rbCost6)
        Me.gpDefCostume.Controls.Add(Me.rbCost5)
        Me.gpDefCostume.Controls.Add(Me.rbCost4)
        Me.gpDefCostume.Controls.Add(Me.rbCost3)
        Me.gpDefCostume.Controls.Add(Me.rbCost2)
        Me.gpDefCostume.Controls.Add(Me.rbCost1)
        Me.gpDefCostume.Location = New System.Drawing.Point(130, 11)
        Me.gpDefCostume.Name = "gpDefCostume"
        Me.gpDefCostume.Size = New System.Drawing.Size(145, 215)
        Me.gpDefCostume.TabIndex = 37
        Me.gpDefCostume.TabStop = False
        Me.gpDefCostume.Text = "Default Costume"
        '
        'rbCost1
        '
        Me.rbCost1.AutoSize = True
        Me.rbCost1.Location = New System.Drawing.Point(6, 19)
        Me.rbCost1.Name = "rbCost1"
        Me.rbCost1.Size = New System.Drawing.Size(55, 17)
        Me.rbCost1.TabIndex = 0
        Me.rbCost1.TabStop = True
        Me.rbCost1.Text = "Kratos"
        Me.rbCost1.UseVisualStyleBackColor = True
        '
        'gpChallsBeaten
        '
        Me.gpChallsBeaten.Controls.Add(Me.CheckBox20)
        Me.gpChallsBeaten.Controls.Add(Me.Label12)
        Me.gpChallsBeaten.Controls.Add(Me.CheckBox21)
        Me.gpChallsBeaten.Controls.Add(Me.Label13)
        Me.gpChallsBeaten.Controls.Add(Me.CheckBox22)
        Me.gpChallsBeaten.Controls.Add(Me.Label14)
        Me.gpChallsBeaten.Controls.Add(Me.CheckBox18)
        Me.gpChallsBeaten.Controls.Add(Me.Label10)
        Me.gpChallsBeaten.Controls.Add(Me.CheckBox19)
        Me.gpChallsBeaten.Controls.Add(Me.Label11)
        Me.gpChallsBeaten.Controls.Add(Me.CheckBox17)
        Me.gpChallsBeaten.Controls.Add(Me.Label9)
        Me.gpChallsBeaten.Controls.Add(Me.CheckBox16)
        Me.gpChallsBeaten.Controls.Add(Me.Label8)
        Me.gpChallsBeaten.Location = New System.Drawing.Point(6, 117)
        Me.gpChallsBeaten.Name = "gpChallsBeaten"
        Me.gpChallsBeaten.Size = New System.Drawing.Size(118, 168)
        Me.gpChallsBeaten.TabIndex = 36
        Me.gpChallsBeaten.TabStop = False
        Me.gpChallsBeaten.Text = "Challenges Beaten"
        '
        'CheckBox20
        '
        Me.CheckBox20.AutoSize = True
        Me.CheckBox20.Location = New System.Drawing.Point(75, 136)
        Me.CheckBox20.Name = "CheckBox20"
        Me.CheckBox20.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox20.TabIndex = 41
        Me.CheckBox20.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox20.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 136)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(63, 13)
        Me.Label12.TabIndex = 48
        Me.Label12.Text = "Challenge 7"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox21
        '
        Me.CheckBox21.AutoSize = True
        Me.CheckBox21.Location = New System.Drawing.Point(75, 116)
        Me.CheckBox21.Name = "CheckBox21"
        Me.CheckBox21.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox21.TabIndex = 40
        Me.CheckBox21.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox21.UseVisualStyleBackColor = True
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(6, 116)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(63, 13)
        Me.Label13.TabIndex = 47
        Me.Label13.Text = "Challenge 6"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox22
        '
        Me.CheckBox22.AutoSize = True
        Me.CheckBox22.Location = New System.Drawing.Point(75, 96)
        Me.CheckBox22.Name = "CheckBox22"
        Me.CheckBox22.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox22.TabIndex = 39
        Me.CheckBox22.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox22.UseVisualStyleBackColor = True
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(6, 96)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(63, 13)
        Me.Label14.TabIndex = 46
        Me.Label14.Text = "Challenge 5"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox18
        '
        Me.CheckBox18.AutoSize = True
        Me.CheckBox18.Location = New System.Drawing.Point(75, 76)
        Me.CheckBox18.Name = "CheckBox18"
        Me.CheckBox18.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox18.TabIndex = 38
        Me.CheckBox18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox18.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 76)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(63, 13)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Challenge 4"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox19
        '
        Me.CheckBox19.AutoSize = True
        Me.CheckBox19.Location = New System.Drawing.Point(75, 56)
        Me.CheckBox19.Name = "CheckBox19"
        Me.CheckBox19.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox19.TabIndex = 37
        Me.CheckBox19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox19.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(6, 56)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(63, 13)
        Me.Label11.TabIndex = 44
        Me.Label11.Text = "Challenge 3"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox17
        '
        Me.CheckBox17.AutoSize = True
        Me.CheckBox17.Location = New System.Drawing.Point(75, 36)
        Me.CheckBox17.Name = "CheckBox17"
        Me.CheckBox17.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox17.TabIndex = 36
        Me.CheckBox17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox17.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(6, 36)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(63, 13)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "Challenge 2"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox16
        '
        Me.CheckBox16.AutoSize = True
        Me.CheckBox16.Location = New System.Drawing.Point(75, 16)
        Me.CheckBox16.Name = "CheckBox16"
        Me.CheckBox16.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox16.TabIndex = 35
        Me.CheckBox16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox16.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(6, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(63, 13)
        Me.Label8.TabIndex = 42
        Me.Label8.Text = "Challenge 1"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'gpDiffsBeaten
        '
        Me.gpDiffsBeaten.Controls.Add(Me.CheckBox14)
        Me.gpDiffsBeaten.Controls.Add(Me.Label6)
        Me.gpDiffsBeaten.Controls.Add(Me.CheckBox15)
        Me.gpDiffsBeaten.Controls.Add(Me.Label7)
        Me.gpDiffsBeaten.Controls.Add(Me.CheckBox13)
        Me.gpDiffsBeaten.Controls.Add(Me.Label5)
        Me.gpDiffsBeaten.Controls.Add(Me.CheckBox12)
        Me.gpDiffsBeaten.Controls.Add(Me.Label4)
        Me.gpDiffsBeaten.Location = New System.Drawing.Point(6, 6)
        Me.gpDiffsBeaten.Name = "gpDiffsBeaten"
        Me.gpDiffsBeaten.Size = New System.Drawing.Size(113, 105)
        Me.gpDiffsBeaten.TabIndex = 35
        Me.gpDiffsBeaten.TabStop = False
        Me.gpDiffsBeaten.Text = "Difficulties Beaten"
        '
        'CheckBox14
        '
        Me.CheckBox14.AutoSize = True
        Me.CheckBox14.Location = New System.Drawing.Point(83, 76)
        Me.CheckBox14.Name = "CheckBox14"
        Me.CheckBox14.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox14.TabIndex = 24
        Me.CheckBox14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox14.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(23, 76)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 13)
        Me.Label6.TabIndex = 28
        Me.Label6.Text = "Very Hard"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox15
        '
        Me.CheckBox15.AutoSize = True
        Me.CheckBox15.Location = New System.Drawing.Point(83, 56)
        Me.CheckBox15.Name = "CheckBox15"
        Me.CheckBox15.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox15.TabIndex = 23
        Me.CheckBox15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox15.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(47, 56)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(30, 13)
        Me.Label7.TabIndex = 27
        Me.Label7.Text = "Hard"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox13
        '
        Me.CheckBox13.AutoSize = True
        Me.CheckBox13.Location = New System.Drawing.Point(83, 36)
        Me.CheckBox13.Name = "CheckBox13"
        Me.CheckBox13.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox13.TabIndex = 22
        Me.CheckBox13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox13.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(37, 37)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "Normal"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CheckBox12
        '
        Me.CheckBox12.AutoSize = True
        Me.CheckBox12.Location = New System.Drawing.Point(83, 16)
        Me.CheckBox12.Name = "CheckBox12"
        Me.CheckBox12.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox12.TabIndex = 21
        Me.CheckBox12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckBox12.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(47, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 13)
        Me.Label4.TabIndex = 25
        Me.Label4.Text = "Easy"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tabSave
        '
        Me.tabSave.Location = New System.Drawing.Point(4, 22)
        Me.tabSave.Name = "tabSave"
        Me.tabSave.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSave.Size = New System.Drawing.Size(1045, 759)
        Me.tabSave.TabIndex = 1
        Me.tabSave.Text = "SaveData"
        Me.tabSave.UseVisualStyleBackColor = True
        '
        'rbCost2
        '
        Me.rbCost2.AutoSize = True
        Me.rbCost2.Location = New System.Drawing.Point(7, 43)
        Me.rbCost2.Name = "rbCost2"
        Me.rbCost2.Size = New System.Drawing.Size(79, 17)
        Me.rbCost2.TabIndex = 1
        Me.rbCost2.TabStop = True
        Me.rbCost2.Text = "Fear Kratos"
        Me.rbCost2.UseVisualStyleBackColor = True
        '
        'rbCost3
        '
        Me.rbCost3.AutoSize = True
        Me.rbCost3.Location = New System.Drawing.Point(7, 66)
        Me.rbCost3.Name = "rbCost3"
        Me.rbCost3.Size = New System.Drawing.Size(54, 17)
        Me.rbCost3.TabIndex = 2
        Me.rbCost3.TabStop = True
        Me.rbCost3.Text = "Apollo"
        Me.rbCost3.UseVisualStyleBackColor = True
        '
        'rbCost4
        '
        Me.rbCost4.AutoSize = True
        Me.rbCost4.Location = New System.Drawing.Point(7, 90)
        Me.rbCost4.Name = "rbCost4"
        Me.rbCost4.Size = New System.Drawing.Size(72, 17)
        Me.rbCost4.TabIndex = 3
        Me.rbCost4.TabStop = True
        Me.rbCost4.Text = "Morpheus"
        Me.rbCost4.UseVisualStyleBackColor = True
        '
        'rbCost5
        '
        Me.rbCost5.AutoSize = True
        Me.rbCost5.Location = New System.Drawing.Point(7, 114)
        Me.rbCost5.Name = "rbCost5"
        Me.rbCost5.Size = New System.Drawing.Size(112, 17)
        Me.rbCost5.TabIndex = 4
        Me.rbCost5.TabStop = True
        Me.rbCost5.Text = "Phantom of Chaos"
        Me.rbCost5.UseVisualStyleBackColor = True
        '
        'rbCost6
        '
        Me.rbCost6.AutoSize = True
        Me.rbCost6.Location = New System.Drawing.Point(7, 137)
        Me.rbCost6.Name = "rbCost6"
        Me.rbCost6.Size = New System.Drawing.Size(107, 17)
        Me.rbCost6.TabIndex = 5
        Me.rbCost6.TabStop = True
        Me.rbCost6.Text = "Forgotten Warrior"
        Me.rbCost6.UseVisualStyleBackColor = True
        '
        'rbCost7
        '
        Me.rbCost7.AutoSize = True
        Me.rbCost7.Location = New System.Drawing.Point(6, 161)
        Me.rbCost7.Name = "rbCost7"
        Me.rbCost7.Size = New System.Drawing.Size(66, 17)
        Me.rbCost7.TabIndex = 6
        Me.rbCost7.TabStop = True
        Me.rbCost7.Text = "Dominus"
        Me.rbCost7.UseVisualStyleBackColor = True
        '
        'rbCost8
        '
        Me.rbCost8.AutoSize = True
        Me.rbCost8.Location = New System.Drawing.Point(6, 185)
        Me.rbCost8.Name = "rbCost8"
        Me.rbCost8.Size = New System.Drawing.Size(131, 17)
        Me.rbCost8.TabIndex = 7
        Me.rbCost8.TabStop = True
        Me.rbCost8.Text = "Demios (DLC required)"
        Me.rbCost8.UseVisualStyleBackColor = True
        '
        'GoW3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1077, 872)
        Me.Controls.Add(Me.tabSaves)
        Me.Controls.Add(Me.btnG3Browse)
        Me.Controls.Add(Me.btnG3Save)
        Me.Controls.Add(Me.btnG3Open)
        Me.Controls.Add(Me.lblG1File)
        Me.Controls.Add(Me.txtG3File)
        Me.Name = "GoW3"
        Me.Text = "Wulf's God of War 3 Editor"
        Me.tabSaves.ResumeLayout(False)
        Me.tabUser.ResumeLayout(False)
        Me.gpDefCostume.ResumeLayout(False)
        Me.gpDefCostume.PerformLayout()
        Me.gpChallsBeaten.ResumeLayout(False)
        Me.gpChallsBeaten.PerformLayout()
        Me.gpDiffsBeaten.ResumeLayout(False)
        Me.gpDiffsBeaten.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnG3Browse As System.Windows.Forms.Button
    Friend WithEvents btnG3Save As System.Windows.Forms.Button
    Friend WithEvents btnG3Open As System.Windows.Forms.Button
    Friend WithEvents lblG1File As System.Windows.Forms.Label
    Friend WithEvents txtG3File As System.Windows.Forms.TextBox
    Friend WithEvents tabSaves As TabControl
    Friend WithEvents tabUser As TabPage
    Friend WithEvents tabSave As TabPage
    Friend WithEvents gpDefCostume As GroupBox
    Friend WithEvents rbCost1 As RadioButton
    Friend WithEvents gpChallsBeaten As GroupBox
    Friend WithEvents CheckBox20 As CheckBox
    Friend WithEvents Label12 As Label
    Friend WithEvents CheckBox21 As CheckBox
    Friend WithEvents Label13 As Label
    Friend WithEvents CheckBox22 As CheckBox
    Friend WithEvents Label14 As Label
    Friend WithEvents CheckBox18 As CheckBox
    Friend WithEvents Label10 As Label
    Friend WithEvents CheckBox19 As CheckBox
    Friend WithEvents Label11 As Label
    Friend WithEvents CheckBox17 As CheckBox
    Friend WithEvents Label9 As Label
    Friend WithEvents CheckBox16 As CheckBox
    Friend WithEvents Label8 As Label
    Friend WithEvents gpDiffsBeaten As GroupBox
    Friend WithEvents CheckBox14 As CheckBox
    Friend WithEvents Label6 As Label
    Friend WithEvents CheckBox15 As CheckBox
    Friend WithEvents Label7 As Label
    Friend WithEvents CheckBox13 As CheckBox
    Friend WithEvents Label5 As Label
    Friend WithEvents CheckBox12 As CheckBox
    Friend WithEvents Label4 As Label
    Friend WithEvents rbCost8 As RadioButton
    Friend WithEvents rbCost7 As RadioButton
    Friend WithEvents rbCost6 As RadioButton
    Friend WithEvents rbCost5 As RadioButton
    Friend WithEvents rbCost4 As RadioButton
    Friend WithEvents rbCost3 As RadioButton
    Friend WithEvents rbCost2 As RadioButton
End Class
