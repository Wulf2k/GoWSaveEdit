Imports GoWEditor.PS3FileSystem
Imports GoWEditor.GoWFuncs

Public Class GoW3

    Dim SecureID(15) As Byte
    Public Shared bytes() As Byte




    Private Sub btnG3Open_Click(sender As Object, e As EventArgs) Handles btnG3Open.Click

        SecureID = GoW3SecureID
        folder = UCase(txtG3Folder.Text)


        If IO.File.Exists(folder + "\PARAM.PFD") Then
            encrypted = True
            manager = New Ps3SaveManager(txtG3Folder.Text, SecureID)
        Else
            encrypted = False
        End If

        filename = "SAVEDATA"
        bytes = FileToBytes(filename)


        If txtG3Folder.Text.ToLower.Contains("-userdata") Then
            tabUser.Enabled = True
            tabSave.Enabled = False
            tabSaves.SelectedTab = tabUser

            chkCh5.Checked = bytes(&H6) And &H1
            chkCh6.Checked = bytes(&H6) And &H2
            chkCh7.Checked = bytes(&H6) And &H4

            chkCh1.Checked = bytes(&H7) And &H10
            chkCh2.Checked = bytes(&H7) And &H20
            chkCh3.Checked = bytes(&H7) And &H40
            chkCh4.Checked = bytes(&H7) And &H80

            chkBeatEasy.Checked = bytes(&H7) And &H1
            chkBeatNormal.Checked = bytes(&H7) And &H2
            chkBeatHard.Checked = bytes(&H7) And &H4
            chkBeatVeryHard.Checked = bytes(&H7) And &H8

            txtMusVol.Text = bytes(&HE)
            txtSpeechVol.Text = bytes(&HF)
            txtSfxVol.Text = bytes(&H10)
            txtCineVol.Text = bytes(&H11)
            txtSoundMode.Text = bytes(&H12)
            txtBrightness.Text = bytes(&H13)
            txtScreenScale.Text = bytes(&H14)
            txtScreenLocX.Text = bytes(&H15)
            txtScreenLocY.Text = bytes(&H16)
            txtTutDisplay.Text = bytes(&H17)
            chkShowSubs.Checked = (bytes(&H18) = 1)
            chkInvertFlight.Checked = (bytes(&H1A) = 1)
            chkInvertFreeCam.Checked = (bytes(&H1B) = 1)

            rbCost1.Checked = (bytes(&H1D) = 0)
            rbCost2.Checked = (bytes(&H1D) = 1)
            rbCost3.Checked = (bytes(&H1D) = 2)
            rbCost4.Checked = (bytes(&H1D) = 3)
            rbCost5.Checked = (bytes(&H1D) = 4)
            rbCost6.Checked = (bytes(&H1D) = 5)
            rbCost7.Checked = (bytes(&H1D) = 6)
            rbCost8.Checked = (bytes(&H1D) = 7)

        Else
            tabUser.Enabled = False
            tabSave.Enabled = True
            tabSaves.SelectedTab = tabSave



        End If


    End Sub

    Private Sub btnG3Save_Click(sender As Object, e As EventArgs) Handles btnG3Save.Click
        bytes = FileToBytes(filename)

        If txtG3Folder.Text.ToLower.Contains("-userdata") Then

            bytes(&H6) = chkCh5.Checked And &H1
            bytes(&H6) = bytes(&H6) + (chkCh6.Checked And &H2)
            bytes(&H6) = bytes(&H6) + (chkCh7.Checked And &H4)

            bytes(&H7) = chkCh1.Checked And &H10
            bytes(&H7) = bytes(&H7) + (chkCh2.Checked And &H20)
            bytes(&H7) = bytes(&H7) + (chkCh3.Checked And &H40)
            bytes(&H7) = bytes(&H7) + (chkCh4.Checked And &H80)

            bytes(&H7) = bytes(&H7) + (chkBeatEasy.Checked And &H1)
            bytes(&H7) = bytes(&H7) + (chkBeatNormal.Checked And &H2)
            bytes(&H7) = bytes(&H7) + (chkBeatHard.Checked And &H4)
            bytes(&H7) = bytes(&H7) + (chkBeatVeryHard.Checked And &H8)




            bytes(&HE) = CSByte(txtMusVol.Text)
            bytes(&HF) = CSByte(txtSpeechVol.Text)
            bytes(&H10) = CSByte(txtSfxVol.Text)
            bytes(&H11) = CSByte(txtCineVol.Text)
            bytes(&H12) = CSByte(txtSoundMode.Text)
            bytes(&H13) = CSByte(txtBrightness.Text)
            bytes(&H14) = CSByte(txtScreenScale.Text)
            bytes(&H15) = CSByte(txtScreenLocX.Text)
            bytes(&H16) = CSByte(txtScreenLocY.Text)
            bytes(&H17) = CSByte(txtTutDisplay.Text)
            bytes(&H18) = chkShowSubs.Checked And 1
            bytes(&H1A) = chkInvertFlight.Checked And 1
            bytes(&H1B) = chkInvertFreeCam.Checked And 1


            txtBrightness.Text = bytes(&H13)
            txtScreenScale.Text = bytes(&H14)
            txtScreenLocX.Text = bytes(&H15)
            txtScreenLocY.Text = bytes(&H16)
            txtTutDisplay.Text = bytes(&H17)
            chkShowSubs.Checked = (bytes(&H18) = 1)
            chkInvertFlight.Checked = (bytes(&H1A) = 1)
            chkInvertFreeCam.Checked = (bytes(&H1B) = 1)


            If rbCost1.Checked Then bytes(&H1D) = 0
            If rbCost2.Checked Then bytes(&H1D) = 1
            If rbCost3.Checked Then bytes(&H1D) = 2
            If rbCost4.Checked Then bytes(&H1D) = 3
            If rbCost5.Checked Then bytes(&H1D) = 4
            If rbCost6.Checked Then bytes(&H1D) = 5
            If rbCost7.Checked Then bytes(&H1D) = 6
            If rbCost8.Checked Then bytes(&H1D) = 7

        Else



        End If


        BytesToFile(filename, bytes)
    End Sub

    Private Sub btnG3Browse_Click(sender As Object, e As EventArgs) Handles btnG3Browse.Click
        Dim openDlg As New OpenFileDialog()
        openDlg.Filter = "GoW3 Save File|SAVEDATA"
        openDlg.Title = "Open your SAVEDATA  file"

        If openDlg.ShowDialog() = Windows.Forms.DialogResult.OK Then txtG3Folder.Text = Microsoft.VisualBasic.Left(openDlg.FileName, openDlg.FileName.Length - 8)
    End Sub

    Private Sub GoW3_close(sender As System.Object, e As System.EventArgs) Handles MyBase.FormClosed
        Main.Close()
    End Sub

End Class