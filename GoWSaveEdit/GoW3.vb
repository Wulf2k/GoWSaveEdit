Imports GoWEditor.PS3FileSystem
Imports GoWEditor.GoWFuncs
Imports System.Data.SqlTypes

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

            Dim sfobytes = FileToBytes("PARAM.SFO")

            txtSaveDesc.Text = RStrAscii(sfobytes, &H1B0, 68)
            txtSaveDesc.Text = txtSaveDesc.Text.Replace(Chr(10), Environment.NewLine)

            txtSaveArea.Text = RStrAscii(sfobytes, &H9FC, &H7F)
            txtSaveArea.Text = txtSaveArea.Text.Replace(Chr(10), Environment.NewLine)

            Dim Sec1Size = RInt16(bytes, &H4)
            Dim CamSize = RInt16(bytes, &H6 + Sec1Size)

            Dim PlayerLoc = RInt16(bytes, &H46)


            txtXPos.Text = RSingle(bytes, PlayerLoc + &H7C)
            txtYPos.Text = RSingle(bytes, PlayerLoc + &H80)
            txtZPos.Text = RSingle(bytes, PlayerLoc + &H84)


            REM &HA0
            chkAphroditesGarter.Checked = bytes(PlayerLoc + &HA0) And 1
            chkHephaestusRing.Checked = bytes(PlayerLoc + &HA0) And 2
            chkHerasChalice.Checked = bytes(PlayerLoc + &HA0) And 4
            chkZeusEagle.Checked = bytes(PlayerLoc + &HA0) And 8
            chkDaedalusSchematics.Checked = bytes(PlayerLoc + &HA0) And &H10
            REM And &H20, ?
            REM And &H40, ?
            REM And &H80, ?

            REM &HA1
            chkWhipUnlock.Checked = bytes(PlayerLoc + &HA1) And 1
            chkBladesUnlock.Checked = bytes(PlayerLoc + &HA1) And 2
            chkClawsUnlock.Checked = bytes(PlayerLoc + &HA1) And 4
            chkHadesHelm.Checked = bytes(PlayerLoc + &HA1) And 8
            chkHeliosShield.Checked = bytes(PlayerLoc + &HA1) And &H10
            chkHermesCoin.Checked = bytes(PlayerLoc + &HA1) And &H20
            chkHerculesShouldGuard.Checked = bytes(PlayerLoc + &HA1) And &H40
            chkPoseidonsConchShell.Checked = bytes(PlayerLoc + &HA1) And &H80

            REM &HA2
            REM And 1, ?
            chkBladeOfOlympusRage.Checked = bytes(PlayerLoc + &HA2) And 2
            chkBootsUnlock.Checked = bytes(PlayerLoc + &HA2) And 4
            chkPoseidonsTrident.Checked = bytes(PlayerLoc + &HA2) And 8
            chkBladeOfOlympus.Checked = bytes(PlayerLoc + &HA2) And &H10
            REM And &H20, ?
            REM And &H40, ?
            chkCestusUnlock.Checked = bytes(PlayerLoc + &HA2) And &H80

            REM &HA3
            chkBowUnlock.Checked = bytes(PlayerLoc + &HA3) And 1
            REM And 2, ?
            chkHeadUnlock.Checked = bytes(PlayerLoc + &HA3) And 4
            chkBoreasIcestorm.Checked = bytes(PlayerLoc + &HA3) And 8
            chkGoldenFleece.Checked = bytes(PlayerLoc + &HA3) And &H10
            chkIcarusWings.Checked = bytes(PlayerLoc + &HA3) And &H20
            REM And &H40, ?
            REM And &H80, ?


            txtCurrHealth.Text = RSingle(bytes, PlayerLoc + &HA8)
            txtCurrMagic.Text = RSingle(bytes, PlayerLoc + &HAC)
            txtCurrItem.Text = RSingle(bytes, PlayerLoc + &HB0)

            txtRedOrbs.Text = RSingle(bytes, PlayerLoc + &HC4)

            txtHealthExt.Text = bytes(PlayerLoc + &HDA)
            txtMagicExt.Text = bytes(PlayerLoc + &HDB)
            txtItemExt.Text = bytes(PlayerLoc + &HDC)


            txtBowLevel.Text = bytes(PlayerLoc + &HDD)
            txtBootsLevel.Text = bytes(PlayerLoc + &HDE)
            txtHeadLevel.Text = bytes(PlayerLoc + &HDF)
            txtCestusLevel.Text = bytes(PlayerLoc + &HE0)
            txtWhipLevel.Text = bytes(PlayerLoc + &HE1)
            txtBladesLevel.Text = bytes(PlayerLoc + &HE2)
            txtClawsLevel.Text = bytes(PlayerLoc + &HE3)


            rbCurrDiff0.Checked = (bytes(PlayerLoc + &H48A) = 0)
            rbCurrDiff1.Checked = (bytes(PlayerLoc + &H48A) = 1)
            rbCurrDiff2.Checked = (bytes(PlayerLoc + &H48A) = 2)
            rbCurrDiff3.Checked = (bytes(PlayerLoc + &H48A) = 3)

            rbCurrCost0.Checked = (bytes(PlayerLoc + &H48B) = 0)
            rbCurrCost1.Checked = (bytes(PlayerLoc + &H48B) = 1)
            rbCurrCost2.Checked = (bytes(PlayerLoc + &H48B) = 2)
            rbCurrCost3.Checked = (bytes(PlayerLoc + &H48B) = 3)
            rbCurrCost4.Checked = (bytes(PlayerLoc + &H48B) = 4)
            rbCurrCost5.Checked = (bytes(PlayerLoc + &H48B) = 5)
            rbCurrCost6.Checked = (bytes(PlayerLoc + &H48B) = 6)
            rbCurrCost7.Checked = (bytes(PlayerLoc + &H48B) = 7)

            chkHealthInfinite.Checked = bytes(PlayerLoc + &H48C)
            chkMagicInfinite.Checked = bytes(PlayerLoc + &H493)
            chkItemInfinite.Checked = bytes(PlayerLoc + &H496)
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
            Dim Sec1Size = RInt16(bytes, &H4)
            Dim CamSize = RInt16(bytes, &H6 + Sec1Size)


            Dim sfobytes = FileToBytes("PARAM.SFO")
            Dim desc As String
            Dim area As String

            desc = txtSaveDesc.Text
            desc = desc.Replace(Environment.NewLine, Chr(10))

            area = txtSaveArea.Text
            area = area.Replace(Environment.NewLine, Chr(10))


            WStrAscii(sfobytes, &H1B0, desc + Chr(0))
            WStrAscii(sfobytes, &H9FC, area + Chr(0))

            BytesToFile("PARAM.SFO", sfobytes)

            Dim PlayerLoc = RInt16(bytes, &H46)
            WSingle(bytes, PlayerLoc + &H7C, Convert.ToSingle(txtXPos.Text))
            WSingle(bytes, PlayerLoc + &H80, Convert.ToSingle(txtYPos.Text))
            WSingle(bytes, PlayerLoc + &H84, Convert.ToSingle(txtZPos.Text))


            Dim b As Byte

            REM &HA0
            b = bytes(PlayerLoc + &HA0)
            b = b And &HE0 'Leave unknown values alone

            b = b + (chkAphroditesGarter.Checked And 1)
            b = b + (chkHephaestusRing.Checked And 2)
            b = b + (chkHerasChalice.Checked And 4)
            b = b + (chkZeusEagle.Checked And 8)
            b = b + (chkDaedalusSchematics.Checked And &H10)
            REM And &H20, ?
            REM And &H40, ?
            REM And &H80, ?
            bytes(PlayerLoc + &HA0) = b

            REM &HA1
            b = 0
            b = b + (chkWhipUnlock.Checked And 1)
            b = b + (chkBladesUnlock.Checked And 2)
            b = b + (chkClawsUnlock.Checked And 4)
            b = b + (chkHadesHelm.Checked And 8)
            b = b + (chkHeliosShield.Checked And &H10)
            b = b + (chkHermesCoin.Checked And &H20)
            b = b + (chkHerculesShouldGuard.Checked And &H40)
            b = b + (chkPoseidonsConchShell.Checked And &H80)
            bytes(PlayerLoc + &HA1) = b

            REM &HA2
            b = bytes(PlayerLoc + &HA2)
            b = b And &H61 'Leave unknown values alone

            REM And 1, ?
            b = b + (chkBladeOfOlympusRage.Checked And 2)
            b = b + (chkBootsUnlock.Checked And 4)
            b = b + (chkPoseidonsTrident.Checked And 8)
            b = b + (chkBladeOfOlympus.Checked And &H10)
            REM And &H20, ?
            REM And &H40, ?
            b = b + (chkCestusUnlock.Checked And &H80)
            bytes(PlayerLoc + &HA2) = b

            REM &HA3
            b = bytes(PlayerLoc + &HA3)
            b = b And &HC2 'Leave unknown values alone

            b = b + (chkBowUnlock.Checked And 1)
            REM And 2, ?
            b = b + (chkHeadUnlock.Checked And 4)
            b = b + (chkBoreasIcestorm.Checked And 8)
            b = b + (chkGoldenFleece.Checked And &H10)
            b = b + (chkIcarusWings.Checked And &H20)
            REM And &H40, ?
            REM And &H80, ?
            bytes(PlayerLoc + &HA3) = b

            WSingle(bytes, PlayerLoc + &HA8, txtCurrHealth.Text)
            WSingle(bytes, PlayerLoc + &HAC, txtCurrMagic.Text)
            WSingle(bytes, PlayerLoc + &HB0, txtCurrItem.Text)

            WSingle(bytes, PlayerLoc + &HC4, txtRedOrbs.Text)

            bytes(PlayerLoc + &HDA) = CSByte(txtHealthExt.Text)
            bytes(PlayerLoc + &HDB) = CSByte(txtMagicExt.Text)
            bytes(PlayerLoc + &HDC) = CSByte(txtItemExt.Text)
            bytes(PlayerLoc + &HDD) = CSByte(txtBowLevel.Text)
            bytes(PlayerLoc + &HDE) = CSByte(txtBootsLevel.Text)
            bytes(PlayerLoc + &HDF) = CSByte(txtHeadLevel.Text)
            bytes(PlayerLoc + &HE0) = CSByte(txtCestusLevel.Text)
            bytes(PlayerLoc + &HE1) = CSByte(txtWhipLevel.Text)
            bytes(PlayerLoc + &HE2) = CSByte(txtBladesLevel.Text)
            bytes(PlayerLoc + &HE3) = CSByte(txtClawsLevel.Text)

            If rbCurrDiff0.Checked Then bytes(PlayerLoc + &H48A) = 0
            If rbCurrDiff1.Checked Then bytes(PlayerLoc + &H48A) = 1
            If rbCurrDiff2.Checked Then bytes(PlayerLoc + &H48A) = 2
            If rbCurrDiff3.Checked Then bytes(PlayerLoc + &H48A) = 3

            If rbCurrCost0.Checked Then bytes(PlayerLoc + &H48B) = 0
            If rbCurrCost1.Checked Then bytes(PlayerLoc + &H48B) = 1
            If rbCurrCost2.Checked Then bytes(PlayerLoc + &H48B) = 2
            If rbCurrCost3.Checked Then bytes(PlayerLoc + &H48B) = 3
            If rbCurrCost4.Checked Then bytes(PlayerLoc + &H48B) = 4
            If rbCurrCost5.Checked Then bytes(PlayerLoc + &H48B) = 5
            If rbCurrCost6.Checked Then bytes(PlayerLoc + &H48B) = 6
            If rbCurrCost7.Checked Then bytes(PlayerLoc + &H48B) = 7

            bytes(PlayerLoc + &H48C) = chkHealthInfinite.Checked And 1
            bytes(PlayerLoc + &H493) = chkMagicInfinite.Checked And 1
            bytes(PlayerLoc + &H496) = chkItemInfinite.Checked And 1

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