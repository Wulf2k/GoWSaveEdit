Imports GoWEditor.PS3FileSystem

Public Class GoW1


    REM  Well damn
    REM Never look at your own old code
    REM Some of this is really, really bad.
    REM But it works, so whatever.

    Public Shared encrypted As Boolean = False

    Public Shared bigendian As Boolean = True

    Public Shared bytes() As Byte
    Public Shared folder
    Public Shared filename
    Public Shared slotnum = "t"
    Public Shared modified = False
    Public Shared manager As Ps3SaveManager
    Public Shared file As Ps3File
    Public Shared SecureID() As Byte = {&H82, &H21, &H42, &HD2, &H27, &H74, &H97, &H6, &H62, &H25, &H46, &HE6, &HE7, &H20, &H6, &H27}


    Private Sub numRangeCheck(ByRef txtbox As TextBox, low As Integer, high As Integer)
        If IsNumeric(txtbox.Text) Then
            If Val(txtbox.Text) < low Or Val(txtbox.Text) > high Then
                txtbox.BackColor = Color.Red
            Else
                txtbox.BackColor = Color.White
            End If
        Else
            txtbox.BackColor = Color.Red
        End If

    End Sub

    Private Function RSingle(ByRef bytes, start) As Single
        Dim ba(4) As Byte
        Array.Copy(bytes, start, ba, 0, 4)
        If bigendian Then Array.Reverse(ba)

        REM TODO: ...Why is this not acting like it's zero-indexed?
        Return BitConverter.ToSingle(ba, 1)
    End Function

    Private Function SingleToHex(text As String, part As Integer)
        Return (Byte.Parse(Mid(BitConverter.ToString(BitConverter.GetBytes(System.Convert.ToSingle(text))), (13 - part * 3), 2), Globalization.NumberStyles.HexNumber))
    End Function


    Private Function HexToSingle(ByVal hexValue As String) As Single
        Dim iInputIndex As Integer = 0
        Dim iOutputIndex As Integer = 0
        Dim bArray(3) As Byte

        For iInputIndex = 0 To hexValue.Length - 1 Step 2
            bArray(iOutputIndex) = Byte.Parse(hexValue.Chars(iInputIndex) & hexValue.Chars(iInputIndex + 1), Globalization.NumberStyles.HexNumber)
            iOutputIndex += 1
        Next

        Array.Reverse(bArray)
        Return BitConverter.ToSingle(bArray, 0)

    End Function

    Private Function FileToBytes(name As String) As Byte()
        If encrypted Then
            file = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = name)
            Return file.DecryptToBytes
        Else
            Return IO.File.ReadAllBytes(folder + "\" + name)
        End If
    End Function
    Private Sub BytesToFile(name As String, b As Byte())
        If encrypted Then
            Dim f As Ps3File = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = "MASTER.BIN")
            f.Encrypt(b)
            manager.ReBuildChanges()
        Else
            IO.File.WriteAllBytes(folder + "\" + name, b)
        End If
    End Sub



    Private Sub G1openSave()
        btnG1Master.BackColor = Color.LightGray
        btnG1Master.ForeColor = Color.Black

        btnG1Slot1.BackColor = Color.LightGray
        btnG1Slot1.ForeColor = Color.Black

        btnG1Slot2.BackColor = Color.LightGray
        btnG1Slot2.ForeColor = Color.Black

        btnG1Slot3.BackColor = Color.LightGray
        btnG1Slot3.ForeColor = Color.Black

        btnG1Slot4.BackColor = Color.LightGray
        btnG1Slot4.ForeColor = Color.Black



        If IO.File.Exists(folder + "\PARAM.SFD") Then
            encrypted = True
            manager = New Ps3SaveManager(txtG1Folder.Text, SecureID)
        Else
            encrypted = False
        End If





        If slotnum = "t" Then
            btnG1Master.BackColor = Color.Black
            btnG1Master.ForeColor = Color.White

            filename = "MASTER.BIN"
            tctlG1Data.Visible = False
            gbG1Master.Visible = True

            bytes = FileToBytes(filename)


            If bytes(4) = 202 Then
                bigendian = True
                btnG1Browse.Visible = False
                btnG1Open.Visible = False
                btnG1Master.Visible = True
                btnG1Slot1.Visible = True
                btnG1Slot2.Visible = True
                btnG1Slot3.Visible = True
                btnG1Slot4.Visible = True
            Else
                If bytes(7) = 202 Then
                    bigendian = False
                    btnG1Browse.Visible = False
                    btnG1Open.Visible = False
                    btnG1Master.Visible = True
                    btnG1Slot1.Visible = True
                    btnG1Slot2.Visible = True
                    btnG1Slot3.Visible = True
                    btnG1Slot4.Visible = True
                Else
                    MsgBox("This file's opening bytes are not GoW-standard.  Decryption has failed or save slot #1 is empty.  Unless you've manually modified this value, this program will probably crash now.")
                End If
            End If


            If (bytes(&H43) And 2) = 2 Then chkG1BeatGameMaster.Checked = True Else chkG1BeatGameMaster.Checked = False
            If (bytes(&H43) And 4) = 4 Then chkG1BeatVHMaster.Checked = True Else chkG1BeatVHMaster.Checked = False
            If (bytes(&H43) And 8) = 8 Then chkG1BeatCotGMaster.Checked = True Else chkG1BeatCotGMaster.Checked = False
            If (bytes(&H43) And 16) = 16 Then chkG1StatuesMaster.Checked = True Else chkG1StatuesMaster.Checked = False

            modified = False

        Else
            filename = "DATA0" + slotnum + ".BIN"
            tctlG1Data.Visible = True
            gbG1Master.Visible = False

            bytes = FileToBytes(filename)

            If bytes(0) <> &HCA Then MsgBox("This file's opening byte is not GoW-standard.  It is likely still encrypted.  Unless you've manually modified this value, this program will probably crash now.")

            txtG1Wad1.Text = ""
            txtG1Wad2.Text = ""

            For i = 8 To 15
                txtG1Wad1.Text = txtG1Wad1.Text + Chr(bytes(i))
            Next

            For i = 19 To 26
                txtG1Wad2.Text = txtG1Wad2.Text + Chr(bytes(i))
            Next


            txtG1Xpos.Text = RSingle(bytes, &H4E)
            txtG1Height.Text = RSingle(bytes, &H52)
            txtG1YPos.Text = RSingle(bytes, &H56)

            If bytes(&H60) > 0 And bytes(&H60) < 4 Then
                chkG1Swim.Checked = True
            Else
                chkG1Swim.Checked = False
            End If


            txtG1SecsPlayed.Text = RSingle(bytes, &H6A)

            txtG1Health.Text = RSingle(bytes, 118)
            txtG1Magic.Text = RSingle(bytes, 122)
            txtG1Rage.Text = RSingle(bytes, 126)
            txtG1MagicRegen.Text = RSingle(bytes, 130)
            txtG1RedOrbs.Text = (bytes(134) * (16 ^ 2)) + bytes(135)


            txtG1HealthExt.Text = bytes(148)
            txtG1MagicExt.Text = bytes(149)

            txtG1PR.Text = bytes(150) + 1
            txtG1MG.Text = bytes(151) + 1
            txtG1ZF.Text = bytes(152) + 1
            txtG1AoH.Text = bytes(153) + 1
            txtG1BoA.Text = bytes(154) + 1
            txtG1BoC.Text = bytes(155) + 1

            chkG1PRSel.Checked = True
            chkG1PRSel.Checked = False
            If bytes(&H9C) = 3 Then chkG1PRSel.Checked = True
            If bytes(&H9C) = 4 Then chkG1MGSel.Checked = True
            If bytes(&H9C) = 5 Then chkG1AoHSel.Checked = True
            If bytes(&H9C) = 6 Then chkG1ZFSel.Checked = True



            txtG1GorgonEyes.Text = bytes(157)
            txtG1PhoenixFeathers.Text = bytes(158)
            txtG1MuseKeys.Text = bytes(159)

            If bytes(160) = 1 Then chkG1PT.Checked = True Else chkG1PT.Checked = False

            If ((bytes(161) And 64) = 64) Then chkG1PR.Checked = True Else chkG1PR.Checked = False
            If ((bytes(161) And 32) = 32) Then chkG1MG.Checked = True Else chkG1MG.Checked = False
            If ((bytes(161) And 16) = 16) Then chkG1ZF.Checked = True Else chkG1ZF.Checked = False
            If ((bytes(161) And 8) = 8) Then chkG1AoH.Checked = True Else chkG1AoH.Checked = False
            If ((bytes(161) And 4) = 4) Then chkG1BoA.Checked = True Else chkG1BoA.Checked = False

            txtG1Camera.Text = ""
            txtG1CamWad.Text = ""

            For i = 174 To 197
                If bytes(i) > 0 Then
                    txtG1Camera.Text = txtG1Camera.Text + Chr(bytes(i))
                Else
                    i = 197
                End If
            Next

            For i = 198 To 210
                If bytes(i) > 0 Then
                    txtG1CamWad.Text = txtG1CamWad.Text + Chr(bytes(i))
                Else
                    i = 210
                End If
            Next

            If bytes(&H426) = 0 Then rdbG1Kratos.Checked = True
            If bytes(1062) = 1 Then rdbG1Chef.Checked = True
            If bytes(1062) = 2 Then rdbG1Bubbles.Checked = True
            If bytes(1062) = 3 Then rdbG1Tycoonius.Checked = True
            If bytes(1062) = 4 Then rdbG1Dairy.Checked = True
            If bytes(1062) = 5 Then rdbG1Ares.Checked = True

            If bytes(1063) = 0 Then rdbG1Easy.Checked = True
            If bytes(1063) = 1 Then rdbG1Normal.Checked = True
            If bytes(1063) = 2 Then rdbG1Hard.Checked = True
            If bytes(1063) = 3 Then rdbG1VeryHard.Checked = True

            Dim checksum As ULong
            Dim power As ULong
            Dim csum As String

            For i = 0 To (bytes.Length - 5)
                power = 16 ^ (2 * (3 - (i Mod 4)))
                checksum = checksum + bytes(i) * power
            Next

            csum = Microsoft.VisualBasic.Right(Hex(checksum).ToString, 8)

            txtG1Checksum.Text = csum

            For i = 1 To 7 Step 2
                bytes((bytes.Length - 5) + (i + 1) / 2) = Integer.Parse(Mid(csum, i, 2), System.Globalization.NumberStyles.HexNumber)
            Next

            modified = False
        End If

    End Sub


    Private Sub btnG1Open_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Open.Click
        slotnum = "t"
        G1openSave()
    End Sub


    Private Sub chkG1AB_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1AB.CheckedChanged
        modified = True
        If chkG1AB.Checked = True Then
            txtG1BoC.Text = 6
            txtG1BoC.Enabled = False
        Else
            txtG1BoC.Text = 1
            txtG1BoC.Enabled = True
        End If
    End Sub

    Private Sub txtG1BoC_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1BoC.TextChanged
        modified = True
        If Val(txtG1BoC.Text) = 6 Then
            chkG1AB.Checked = True
        Else
            chkG1AB.Checked = False
        End If
        numRangeCheck(txtG1BoC, 0, 6)
    End Sub

    Private Sub txtG1Health_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Health.TextChanged
        modified = True
        numRangeCheck(txtG1Health, 1, 200)
    End Sub

    Private Sub txtG1Magic_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Magic.TextChanged
        modified = True
        numRangeCheck(txtG1Magic, 0, 200)
    End Sub

    Private Sub txtG1Rage_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Rage.TextChanged
        modified = True
        numRangeCheck(txtG1Rage, 0, 100)
    End Sub

    Private Sub txtG1RedOrbs_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1RedOrbs.TextChanged
        modified = True
        If Val(txtG1RedOrbs.Text) > 65535 Then txtG1RedOrbs.Text = 65535
        numRangeCheck(txtG1RedOrbs, 0, 60000)
    End Sub

    Private Sub txtG1HealthExt_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1HealthExt.TextChanged
        modified = True
        numRangeCheck(txtG1HealthExt, 0, 4)
    End Sub

    Private Sub txtG1MagicExt_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1MagicExt.TextChanged
        modified = True
        numRangeCheck(txtG1MagicExt, 0, 5)
    End Sub

    Private Sub txtG1GorgonEyes_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1GorgonEyes.TextChanged
        modified = True
        numRangeCheck(txtG1GorgonEyes, 0, 24)
    End Sub

    Private Sub txtG1PhoenixFeathers_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1PhoenixFeathers.TextChanged
        modified = True
        numRangeCheck(txtG1PhoenixFeathers, 0, 24)
    End Sub

    Private Sub chkG1PRSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1PRSel.CheckedChanged
        modified = True
        If chkG1PRSel.Checked = True Then
            chkG1MGSel.Checked = False
            chkG1AoHSel.Checked = False
            chkG1ZFSel.Checked = False
        End If
    End Sub

    Private Sub chkG1MGSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1MGSel.TextChanged
        modified = True
        If chkG1MGSel.Checked = True Then
            chkG1PRSel.Checked = False
            chkG1AoHSel.Checked = False
            chkG1ZFSel.Checked = False
        End If
    End Sub

    Private Sub chkG1ZFSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1ZFSel.CheckedChanged
        modified = True
        If chkG1ZFSel.Checked = True Then
            chkG1PRSel.Checked = False
            chkG1MGSel.Checked = False
            chkG1AoHSel.Checked = False
        End If
    End Sub

    Private Sub chkG1AoHSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1AoHSel.CheckedChanged
        modified = True
        If chkG1AoHSel.Checked = True Then
            chkG1PRSel.Checked = False
            chkG1MGSel.Checked = False
            chkG1ZFSel.Checked = False
        End If
    End Sub

    Private Sub txtG1PR_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1PR.TextChanged
        modified = True
        numRangeCheck(txtG1PR, 1, 3)
    End Sub

    Private Sub txtG1MG_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1MG.TextChanged
        modified = True
        numRangeCheck(txtG1MG, 1, 3)
    End Sub

    Private Sub txtG1ZF_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1ZF.TextChanged
        modified = True
        numRangeCheck(txtG1ZF, 1, 3)
    End Sub

    Private Sub txtG1AoH_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1AoH.TextChanged
        modified = True
        numRangeCheck(txtG1AoH, 1, 3)
    End Sub

    Private Sub txtG1BoA_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1BoA.TextChanged
        modified = True
        numRangeCheck(txtG1BoA, 1, 3)
    End Sub

    Private Sub txtG1MuseKeys_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1MuseKeys.TextChanged
        modified = True
        numRangeCheck(txtG1MuseKeys, 0, 3)
    End Sub

    Private Sub chkG1Swim_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1Swim.CheckedChanged
        modified = True
    End Sub

    Private Sub btnG1Save_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Save.Click
        bytes = FileToBytes(filename)

        modified = False

        Try
            If filename = "MASTER.BIN" Then
                bytes(&H43) = Math.Abs(chkG1BeatGameMaster.Checked * 2)
                bytes(67) = bytes(67) + Math.Abs(chkG1BeatVHMaster.Checked * 4)
                bytes(67) = bytes(67) + Math.Abs(chkG1BeatCotGMaster.Checked * 8)
                bytes(67) = bytes(67) + Math.Abs(chkG1StatuesMaster.Checked * 16)
            Else
                For i = 0 To 8
                    If (i < txtG1Wad1.TextLength) Then
                        bytes(i + 8) = Asc(txtG1Wad1.Text(i))
                    Else
                        bytes(i + 8) = 0
                    End If
                Next

                If txtG1Wad1.TextLength > 0 Then bytes(18) = 1 Else bytes(18) = 0

                For i = 0 To 8
                    If (i < txtG1Wad2.TextLength) Then
                        bytes(i + 19) = Asc(txtG1Wad2.Text(i))
                    Else
                        bytes(i + 19) = 0
                    End If
                Next

                If txtG1Wad2.TextLength > 0 Then bytes(29) = 1 Else bytes(29) = 0


                REM TODO:  What the hell, pastWulf?  This was what you did?
                REM For shame....
                bytes(78) = SingleToHex(txtG1Xpos.Text, 1)
                bytes(79) = SingleToHex(txtG1Xpos.Text, 2)
                bytes(80) = SingleToHex(txtG1Xpos.Text, 3)
                bytes(81) = SingleToHex(txtG1Xpos.Text, 4)

                bytes(82) = SingleToHex(txtG1Height.Text, 1)
                bytes(83) = SingleToHex(txtG1Height.Text, 2)
                bytes(84) = SingleToHex(txtG1Height.Text, 3)
                bytes(85) = SingleToHex(txtG1Height.Text, 4)

                bytes(86) = SingleToHex(txtG1YPos.Text, 1)
                bytes(87) = SingleToHex(txtG1YPos.Text, 2)
                bytes(88) = SingleToHex(txtG1YPos.Text, 3)
                bytes(89) = SingleToHex(txtG1YPos.Text, 4)

                If chkG1Swim.Checked = True Then bytes(96) = 2 Else bytes(96) = 0

                bytes(106) = SingleToHex(txtG1SecsPlayed.Text, 1)
                bytes(107) = SingleToHex(txtG1SecsPlayed.Text, 2)
                bytes(108) = SingleToHex(txtG1SecsPlayed.Text, 3)
                bytes(109) = SingleToHex(txtG1SecsPlayed.Text, 4)

                bytes(118) = SingleToHex(txtG1Health.Text, 1)
                bytes(119) = SingleToHex(txtG1Health.Text, 2)
                bytes(120) = SingleToHex(txtG1Health.Text, 3)
                bytes(121) = SingleToHex(txtG1Health.Text, 4)

                bytes(122) = SingleToHex(txtG1Magic.Text, 1)
                bytes(123) = SingleToHex(txtG1Magic.Text, 2)
                bytes(124) = SingleToHex(txtG1Magic.Text, 3)
                bytes(125) = SingleToHex(txtG1Magic.Text, 4)

                bytes(126) = SingleToHex(txtG1Rage.Text, 1)
                bytes(127) = SingleToHex(txtG1Rage.Text, 2)
                bytes(128) = SingleToHex(txtG1Rage.Text, 3)
                bytes(129) = SingleToHex(txtG1Rage.Text, 4)

                bytes(130) = SingleToHex(txtG1MagicRegen.Text, 1)
                bytes(131) = SingleToHex(txtG1MagicRegen.Text, 2)
                bytes(132) = SingleToHex(txtG1MagicRegen.Text, 3)
                bytes(133) = SingleToHex(txtG1MagicRegen.Text, 4)

                bytes(134) = Math.Floor(Val(txtG1RedOrbs.Text / (256)))
                bytes(135) = txtG1RedOrbs.Text Mod 256


                bytes(148) = txtG1HealthExt.Text
                bytes(149) = txtG1MagicExt.Text

                bytes(150) = txtG1PR.Text - 1
                bytes(151) = txtG1MG.Text - 1
                bytes(152) = txtG1ZF.Text - 1
                bytes(153) = txtG1AoH.Text - 1
                bytes(154) = txtG1BoA.Text - 1
                bytes(155) = txtG1BoC.Text - 1

                bytes(156) = 0
                If chkG1PRSel.Checked = True Then bytes(156) = 3
                If chkG1MGSel.Checked = True Then bytes(156) = 4
                If chkG1AoHSel.Checked = True Then bytes(156) = 5
                If chkG1ZFSel.Checked = True Then bytes(156) = 6


                bytes(157) = txtG1GorgonEyes.Text
                bytes(158) = txtG1PhoenixFeathers.Text
                bytes(159) = txtG1MuseKeys.Text

                If chkG1PT.Checked = True Then
                    bytes(160) = 1
                Else
                    bytes(160) = 0
                End If

                bytes(161) = System.Math.Abs((chkG1PR.Checked * 64) + (chkG1MG.Checked * 32) + (chkG1ZF.Checked * 16) + (chkG1AoH.Checked * 8) + (chkG1BoA.Checked * 4))


                For i = 0 To 23
                    If (i < txtG1Camera.TextLength) Then
                        bytes(i + 174) = Asc(txtG1Camera.Text(i))
                    Else
                        bytes(i + 174) = 0
                    End If
                Next

                For i = 0 To 23
                    If (i < txtG1CamWad.TextLength) Then
                        bytes(i + 198) = Asc(txtG1CamWad.Text(i))
                    Else
                        bytes(i + 198) = 0
                    End If
                Next


                If rdbG1Kratos.Checked = True Then
                    bytes(1062) = 0
                    bytes(7) = 0
                End If
                If rdbG1Chef.Checked = True Then
                    bytes(1062) = 1
                    bytes(7) = 1
                End If
                If rdbG1Bubbles.Checked = True Then
                    bytes(1062) = 2
                    bytes(7) = 2
                End If
                If rdbG1Tycoonius.Checked = True Then
                    bytes(1062) = 3
                    bytes(7) = 3
                End If
                If rdbG1Dairy.Checked = True Then
                    bytes(1062) = 4
                    bytes(7) = 4
                End If
                If rdbG1Ares.Checked = True Then
                    bytes(1062) = 5
                    bytes(7) = 5
                End If

                If rdbG1Easy.Checked = True Then bytes(1063) = 0
                If rdbG1Normal.Checked = True Then bytes(1063) = 1
                If rdbG1Hard.Checked = True Then bytes(1063) = 2
                If rdbG1VeryHard.Checked = True Then bytes(1063) = 3


                Dim checksum As ULong
                Dim power As ULong
                Dim csum As String


                For i = 0 To (bytes.Length - 5)
                    power = 16 ^ (2 * (3 - (i Mod 4)))
                    checksum = checksum + bytes(i) * power
                Next

                csum = Microsoft.VisualBasic.Right(Hex(checksum).ToString, 8)

                txtG1Checksum.Text = csum

                For i = 1 To 7 Step 2
                    bytes((bytes.Length - 5) + (i + 1) / 2) = Integer.Parse(Mid(csum, i, 2), System.Globalization.NumberStyles.HexNumber)
                Next


                Dim bytesmast = FileToBytes("MASTER.BIN")

                bytesmast(4 + 16 * Val(slotnum)) = 202
                bytesmast(5 + 16 * Val(slotnum)) = 254
                bytesmast(6 + 16 * Val(slotnum)) = 186
                bytesmast(7 + 16 * Val(slotnum)) = 209

                bytesmast(8 + 16 * Val(slotnum)) = bytes(106)
                bytesmast(9 + 16 * Val(slotnum)) = bytes(107)
                bytesmast(10 + 16 * Val(slotnum)) = bytes(108)
                bytesmast(11 + 16 * Val(slotnum)) = bytes(109)

                If bytesmast(13 + 16 * Val(slotnum)) = 0 Then bytesmast(13 + 16 * Val(slotnum)) = 1
                bytesmast(14 + 16 * Val(slotnum)) = bytes(1063)
            End If

            BytesToFile(filename, bytes)


            MsgBox("Save Completed")
        Catch ex As Exception
            MsgBox("Save failed, no specific reason.  Either you or I did something dumb.")
        End Try
    End Sub

    Private Sub txtG1Browse_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Browse.Click
        Dim openDlg As New OpenFileDialog()
        openDlg.Filter = "GoW Save File|MASTER.bin"
        openDlg.Title = "Open your MASTER save file"

        If openDlg.ShowDialog() = Windows.Forms.DialogResult.OK Then txtG1Folder.Text = Microsoft.VisualBasic.Left(openDlg.FileName, openDlg.FileName.Length - 10)
    End Sub

    Private Sub GoW1_close(sender As System.Object, e As System.EventArgs) Handles MyBase.FormClosed
        Main.Close()
    End Sub

    Private Sub txtG1File_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Folder.TextChanged
        folder = UCase(txtG1Folder.Text)
    End Sub

    Private Sub btnG1Master_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Master.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "t"
            G1openSave()
            btnG1Master.BackColor = Color.Black
            btnG1Master.ForeColor = Color.White
        End If
    End Sub

    Private Sub btnG1Slot1_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Slot1.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "0"
            G1openSave()

            btnG1Slot1.BackColor = Color.Black
            btnG1Slot1.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG1Slot2_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Slot2.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "1"
            G1openSave()

            btnG1Slot2.BackColor = Color.Black
            btnG1Slot2.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG1Slot3_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Slot3.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "2"
            G1openSave()

            btnG1Slot3.BackColor = Color.Black
            btnG1Slot3.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG1Slot4_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Slot4.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "3"
            G1openSave()

            btnG1Slot4.BackColor = Color.Black
            btnG1Slot4.ForeColor = Color.White

        End If
    End Sub


    Private Sub chkG1PT_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1PT.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG1BeatGameMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1BeatGameMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG1BeatVHMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1BeatVHMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG1BeatCotGMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1BeatCotGMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG1StatuesMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG1StatuesMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Easy_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Easy.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Normal_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Normal.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Hard_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Hard.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1VeryHard_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1VeryHard.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Kratos_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Kratos.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Chef_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Chef.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Bubbles_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Bubbles.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Tycoonius_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Tycoonius.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Dairy_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Dairy.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG1Ares_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG1Ares.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG1Xpos_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Xpos.TextChanged
        modified = True
    End Sub

    Private Sub txtG1YPos_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1YPos.TextChanged
        modified = True
    End Sub

    Private Sub txtG1Height_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Height.TextChanged
        modified = True
    End Sub

    Private Sub txtG1Wad1_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Wad1.TextChanged
        modified = True
    End Sub

    Private Sub txtG1Wad2_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Wad2.TextChanged
        modified = True
    End Sub

    Private Sub txtG1Camera_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Camera.TextChanged
        modified = True
    End Sub

    Private Sub txtCamWad_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1CamWad.TextChanged
        modified = True
    End Sub

    Private Sub txtG1SecsPlayed_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1SecsPlayed.TextChanged
        modified = True
    End Sub

    Private Sub btnG1Restart_Click(sender As System.Object, e As System.EventArgs) Handles btnG1Restart.Click
        If MsgBox("Revert save to start of game?", vbYesNo) = vbYes Then
            modified = False
            Dim blankbytes = My.Resources.G1BlankSave

            BytesToFile(filename, blankbytes)

            G1openSave()

            Select Case slotnum
                Case 0
                    btnG1Slot1.BackColor = Color.Black
                    btnG1Slot1.ForeColor = Color.White
                Case 1
                    btnG1Slot2.BackColor = Color.Black
                    btnG1Slot2.ForeColor = Color.White
                Case 2
                    btnG1Slot3.BackColor = Color.Black
                    btnG1Slot3.ForeColor = Color.White
                Case 3
                    btnG1Slot4.BackColor = Color.Black
                    btnG1Slot4.ForeColor = Color.White
            End Select


            Dim bytes = FileToBytes("MASTER.BIN")

            bytes(8 + 16 * Val(slotnum)) = 0
            bytes(9 + 16 * Val(slotnum)) = 0
            bytes(10 + 16 * Val(slotnum)) = 0
            bytes(11 + 16 * Val(slotnum)) = 0

            bytes(13 + 16 * Val(slotnum)) = 1

            BytesToFile("MASTER.BIN", bytes)

        End If
    End Sub
End Class
