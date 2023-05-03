Imports GoWEditor.PS3FileSystem
Imports GoWEditor.GoWFuncs

Public Class GoW1


    REM  Well damn
    REM Never look at your own old code
    REM Some of this is really, really bad.
    REM But it works, so whatever.
    Public Class checkpoint
        Public Property Category As String
        Public Property Name As String
        Public Property Wad1 As String
        Public Property Wad1Active As Boolean
        Public Property Wad2 As String
        Public Property Wad2Active As Boolean
        Public Property CurrCam As String
        Public Property CamWAD As String

        Public Property xPos As Single
        Public Property yPos As Single
        Public Property zPos As Single
        Public Property stance As UInt32

        Public Property worldIdle As Boolean

        Public Sub New(Category As String, Name As String, Wad1 As String, Wad1Active As Boolean, Wad2 As String,
                       Wad2Active As Boolean, CurrCam As String, camwad As String, xpos As Single, ypos As Single, zpos As Single,
                       stance As UInt32, Optional worldIdle As Boolean = False)
            Me.Category = Category
            Me.Name = Name
            Me.Wad1 = Wad1
            Me.Wad1Active = Wad1Active
            Me.Wad2 = Wad2
            Me.Wad2Active = Wad2Active
            Me.CurrCam = CurrCam
            Me.CamWAD = camwad
            Me.xPos = xpos
            Me.yPos = ypos
            Me.zPos = zpos
            Me.stance = stance
            Me.worldIdle = worldIdle
        End Sub
    End Class

    Public Shared checkpoints As New List(Of checkpoint)


    Public Shared bytes() As Byte
    Public Shared slotnum = "t"
    Public Shared modified = False



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









    Private Sub G1openSave()

        SecureID = GoW1SecureID

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



        If IO.File.Exists(folder + "\PARAM.PFD") Then
            encrypted = True
            manager = New Ps3SaveManager(txtG1Folder.Text, SecureID)
        Else
            encrypted = False
        End If





        If slotnum = "t" Then
            btnG1Master.BackColor = Color.Black
            btnG1Master.ForeColor = Color.White


            tctlG1Data.Visible = False
            gbG1Master.Visible = True

            filename = "MASTER.BIN"
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
                    MsgBox($"This file's opening bytes are not GoW-standard.  Decryption has failed or save slot #1 is empty.  Unless you've manually modified this value, this program will probably crash now. {bytes(4)} {bytes(7)}")
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
            chkG1Wad1Active.Checked = bytes(&H12)

            For i = 19 To 26
                txtG1Wad2.Text = txtG1Wad2.Text + Chr(bytes(i))
            Next
            chkG1Wad2Active.Checked = bytes(&H1D)

            txtG1Xpos.Text = RSingle(bytes, &H4E)
            txtG1YPos.Text = RSingle(bytes, &H52)
            txtG1Zpos.Text = RSingle(bytes, &H56)

            If bytes(&H60) > 0 And bytes(&H60) < 4 Then
                chkG1Swim.Checked = True
            Else
                chkG1Swim.Checked = False
            End If


            txtG1SecsPlayed.Text = RSingle(bytes, &H6A)

            txtG1Health.Text = RSingle(bytes, &H76)
            txtG1Magic.Text = RSingle(bytes, 122)
            txtG1Rage.Text = RSingle(bytes, 126)
            txtG1MagicRegen.Text = RSingle(bytes, 130)
            txtG1RedOrbs.Text = (bytes(134) * (16 ^ 2)) + bytes(135)


            txtG1HealthExt.Text = bytes(148)
            txtG1MagicExt.Text = bytes(149)

            txtG1PR.Text = bytes(&H96) + 1
            txtG1MG.Text = bytes(151) + 1
            txtG1ZF.Text = bytes(152) + 1
            txtG1AoH.Text = bytes(153) + 1
            txtG1BoA.Text = bytes(154) + 1
            txtG1BoC.Text = bytes(155) + 1


            chkG1PRSel.Checked = (bytes(&H9C) = 3)
            chkG1MGSel.Checked = (bytes(&H9C) = 4)
            chkG1AoHSel.Checked = (bytes(&H9C) = 5)
            chkG1ZFSel.Checked = (bytes(&H9C) = 6)

            txtG1GorgonEyes.Text = bytes(157)
            txtG1PhoenixFeathers.Text = bytes(158)
            txtG1MuseKeys.Text = bytes(159)

            chkG1PT.Checked = (bytes(&HA0) And 1)


            chkG1PR.Checked = bytes(&HA1) And &H4
            chkG1MG.Checked = bytes(&HA1) And &H8
            chkG1ZF.Checked = bytes(&HA1) And &H10
            chkG1AoH.Checked = bytes(&HA1) And &H20
            chkG1BoA.Checked = bytes(&HA1) And &H40


            txtG1Camera.Text = ""
            txtG1CamWad.Text = ""

            For i = &HAE To &HC5
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

            chkG1WorldIdle.Checked = bytes(&H41E)

            txtG1MusVol.Text = bytes(&H41F)
            txtG1SoundVol.Text = bytes(&H420)

            chkG1Vib.Checked = bytes(&H421)
            chkG1Wide.Checked = bytes(&H422)
            chkG1DeFlicker.Checked = bytes(&H423)

            txtG1SoundMode.Text = CSByte(bytes(&H424))

            chkG1PUFT.Checked = bytes(&H425)
            chkG1EndGame.Checked = bytes(&H428)


            If bytes(&H426) = 0 Then rdbG1Kratos.Checked = True
            If bytes(&H426) = 1 Then rdbG1Chef.Checked = True
            If bytes(&H426) = 2 Then rdbG1Bubbles.Checked = True
            If bytes(&H426) = 3 Then rdbG1Tycoonius.Checked = True
            If bytes(&H426) = 4 Then rdbG1Dairy.Checked = True
            If bytes(&H426) = 5 Then rdbG1Ares.Checked = True

            If bytes(&H427) = 0 Then rdbG1Easy.Checked = True
            If bytes(&H427) = 1 Then rdbG1Normal.Checked = True
            If bytes(&H427) = 2 Then rdbG1Hard.Checked = True
            If bytes(&H427) = 3 Then rdbG1VeryHard.Checked = True


            Dim checksum As ULong
            Dim power As ULong
            Dim csum As String

            For i = 0 To (bytes.Length - 5)
                power = 16 ^ (2 * (3 - (i Mod 4)))
                checksum = checksum + bytes(i) * power
            Next

            csum = Microsoft.VisualBasic.Right(Hex(checksum).ToString, 8)

            txtG1Checksum.Text = csum

            WUInt32(bytes, bytes.Length - 5, UInteger.Parse(txtG1Checksum.Text, System.Globalization.NumberStyles.HexNumber))

            REM For i = 1 To 7 Step 2
            REM bytes((bytes.Length - 5) + (i + 1) / 2) = Integer.Parse(Mid(csum, i, 2), System.Globalization.NumberStyles.HexNumber)
            REM Next

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

                bytes(&H12) = (chkG1Wad1Active.Checked And 1)

                For i = 0 To 8
                    If (i < txtG1Wad2.TextLength) Then
                        bytes(i + 19) = Asc(txtG1Wad2.Text(i))
                    Else
                        bytes(i + 19) = 0
                    End If
                Next

                bytes(&H1D) = (chkG1Wad2Active.Checked And 1)


                REM TODO:  What the hell, pastWulf?  This was what you did?
                REM For shame....

                WSingle(bytes, &H4E, Convert.ToSingle(txtG1Xpos.Text))
                WSingle(bytes, &H52, Convert.ToSingle(txtG1YPos.Text))
                WSingle(bytes, &H56, Convert.ToSingle(txtG1Zpos.Text))

                If chkG1Swim.Checked = True Then bytes(96) = 2 Else bytes(96) = 0

                WSingle(bytes, &H6A, Convert.ToSingle(txtG1SecsPlayed.Text))
                WSingle(bytes, &H76, Convert.ToSingle(txtG1Health.Text))
                WSingle(bytes, &H7A, Convert.ToSingle(txtG1Magic.Text))
                WSingle(bytes, &H7E, Convert.ToSingle(txtG1Rage.Text))
                WSingle(bytes, &H82, Convert.ToSingle(txtG1MagicRegen.Text))
                WUInt16(bytes, &H86, Convert.ToUInt16(txtG1RedOrbs.Text))




                bytes(&H94) = txtG1HealthExt.Text
                bytes(&H95) = txtG1MagicExt.Text

                bytes(&H96) = txtG1PR.Text - 1
                bytes(&H97) = txtG1MG.Text - 1
                bytes(&H98) = txtG1ZF.Text - 1
                bytes(153) = txtG1AoH.Text - 1
                bytes(154) = txtG1BoA.Text - 1
                bytes(&H9B) = txtG1BoC.Text - 1

                bytes(&H9C) = 0
                If chkG1PRSel.Checked = True Then bytes(156) = 3
                If chkG1MGSel.Checked = True Then bytes(156) = 4
                If chkG1AoHSel.Checked = True Then bytes(156) = 5
                If chkG1ZFSel.Checked = True Then bytes(156) = 6


                bytes(&H9D) = txtG1GorgonEyes.Text
                bytes(158) = txtG1PhoenixFeathers.Text
                bytes(159) = txtG1MuseKeys.Text


                bytes(&HA0) = (bytes(&HA0) And &HFE) Or (chkG1PT.Checked And 1)


                bytes(&HA1) = (chkG1PR.Checked And &H4)
                bytes(&HA1) = bytes(&HA1) + (chkG1MG.Checked And &H8)
                bytes(&HA1) = bytes(&HA1) + (chkG1ZF.Checked And &H10)
                bytes(&HA1) = bytes(&HA1) + (chkG1AoH.Checked And &H20)
                bytes(&HA1) = bytes(&HA1) + (chkG1BoA.Checked And &H40)


                For i = 0 To 23
                    If (i < txtG1Camera.TextLength) Then
                        bytes(i + &HAE) = Asc(txtG1Camera.Text(i))
                    Else
                        bytes(i + &HAE) = 0
                    End If
                Next

                For i = 0 To 23
                    If (i < txtG1CamWad.TextLength) Then
                        bytes(i + &HC6) = Asc(txtG1CamWad.Text(i))
                    Else
                        bytes(i + &HC6) = 0
                    End If
                Next

                If chkG1WorldIdle.Checked = True Then
                    bytes(&H41E) = 1
                Else
                    bytes(&H41E) = 0
                End If

                bytes(&H41F) = CSByte(txtG1MusVol.Text)
                bytes(&H420) = CSByte(txtG1SoundVol.Text)

                bytes(&H421) = chkG1Vib.Checked And 1
                bytes(&H422) = chkG1Wide.Checked And 1
                bytes(&H423) = chkG1DeFlicker.Checked And 1

                bytes(&H424) = CSByte(txtG1SoundMode.Text)

                bytes(&H425) = chkG1PUFT.Checked And 1
                bytes(&H428) = chkG1EndGame.Checked And 1


                If rdbG1Kratos.Checked = True Then
                    bytes(&H426) = 0
                    bytes(7) = 0
                End If
                If rdbG1Chef.Checked = True Then
                    bytes(&H426) = 1
                    bytes(7) = 1
                End If
                If rdbG1Bubbles.Checked = True Then
                    bytes(&H426) = 2
                    bytes(7) = 2
                End If
                If rdbG1Tycoonius.Checked = True Then
                    bytes(&H426) = 3
                    bytes(7) = 3
                End If
                If rdbG1Dairy.Checked = True Then
                    bytes(&H426) = 4
                    bytes(7) = 4
                End If
                If rdbG1Ares.Checked = True Then
                    bytes(&H426) = 5
                    bytes(7) = 5
                End If




                If rdbG1Easy.Checked = True Then bytes(&H427) = 0
                If rdbG1Normal.Checked = True Then bytes(&H427) = 1
                If rdbG1Hard.Checked = True Then bytes(&H427) = 2
                If rdbG1VeryHard.Checked = True Then bytes(&H427) = 3


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

                bytesmast(4 + 16 * Val(slotnum)) = &HCA
                bytesmast(5 + 16 * Val(slotnum)) = &HFE
                bytesmast(6 + 16 * Val(slotnum)) = &HBA
                bytesmast(7 + 16 * Val(slotnum)) = &HD1

                WUInt32(bytesmast, 4 + 16 * Val(slotnum), &HCAFEBAD1&)

                bytesmast(8 + 16 * Val(slotnum)) = bytes(&H6A)
                bytesmast(9 + 16 * Val(slotnum)) = bytes(&H6B)
                bytesmast(10 + 16 * Val(slotnum)) = bytes(&H6C)
                bytesmast(11 + 16 * Val(slotnum)) = bytes(&H6D)

                If bytesmast(13 + 16 * Val(slotnum)) = 0 Then bytesmast(13 + 16 * Val(slotnum)) = 1
                bytesmast(14 + 16 * Val(slotnum)) = bytes(&H427)

                BytesToFile("MASTER.BIN", bytesmast)
            End If

            BytesToFile(filename, bytes)


            MsgBox("Save Completed")
        Catch ex As Exception
            MsgBox("Save failed, no specific reason.  Either you or I did something dumb. " & ex.Message)
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

    Private Sub txtG1Height_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG1Zpos.TextChanged
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

    Private Sub GoW1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        cmbG1checkpointCat.Items.Add("Aegean Sea")
        cmbG1checkpointCat.Items.Add("Port of Athens")
        cmbG1checkpointCat.Items.Add("Road to Athens")
        cmbG1checkpointCat.Items.Add("Athens Town Square")
        cmbG1checkpointCat.Items.Add("Temple of the Oracle")
        cmbG1checkpointCat.Items.Add("Sewers")
        cmbG1checkpointCat.Items.Add("Road to Athens (Return)")
        cmbG1checkpointCat.Items.Add("Desert")
        cmbG1checkpointCat.Items.Add("Pandora's Temple")
        cmbG1checkpointCat.Items.Add("Rings of Pandora")
        cmbG1checkpointCat.Items.Add("Challenge of Atlas")
        cmbG1checkpointCat.Items.Add("Challenge of Poseidon")
        cmbG1checkpointCat.Items.Add("Challenge of Hades")
        cmbG1checkpointCat.Items.Add("Cliffs of Madness")

        cmbG1checkpointCat.Items.Add("Hades")
        cmbG1checkpointCat.Items.Add("Return to the Temple")

        cmbG1checkpointCat.Items.Add("Ares")
        cmbG1checkpointCat.Items.Add("Challenge of the Gods")
        cmbG1checkpointCat.Items.Add("Misc")
        REM cmbG1checkpointCat.Items.Add("Speedrun Practice")

        checkpoints.Add(New checkpoint("Aegean Sea", "Start of Game", "", False, "Athn01A", True, "IdleCam", "WAD_Athn01A", 1536, 20.48, 1956.63, &H80000001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "First Hydra", "Athn01B", True, "Athn01C", True, "HydCmbtCam1", "WAD_Athn01B", 1517.19, -156.1, 624.888, &H80000001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "Deck Hydra", "Athn01B", True, "Athn01C", True, "Boat2Cam4", "WAD_Athn01C", 235.32, -64, 211.97, &H80000001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "Box Kicking", "Athn01D", True, "Athn01C", True, "Boat3Cam18", "WAD_Athn01D", -1126.66, 56, -444.65, &H80000001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "Boat 3 Mast", "Athn01D", True, "Athn01E", True, "Boat3Cam16", "WAD_Athn01D", -993.6, 1120, -1797.69, &H80000001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "Poseidon's Rage", "Athn01D", True, "Athn01E", True, "BossCam3", "WAD_Athn01E", -2702.24, 592, 132.6, &H80040001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "King Hydra", "Athn01EE", True, "Athn01E", True, "DeckCam1", "WAD_Athn01E", -2560.22, 832, -28.4, &H80040001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "Captain's Door", "Athn01D", True, "Athn01E", True, "Boat3Cam21", "WAD_Athn01D", -1016.3, 64, -2161.13, &H80040001&))
        checkpoints.Add(New checkpoint("Aegean Sea", "Final Door", "Athn01D", True, "Athn01F", False, "ExitCam2", "WAD_Athn01D", -1220.37, -32, -3274.07, &H80040001&))

        checkpoints.Add(New checkpoint("Port of Athens", "Ship's Bed", "Athn02A", False, "Athn01F", True, "AniRvlCam", "WAD_Athn01F", 531.2, 88, -555.6, &H80040001&, True))
        checkpoints.Add(New checkpoint("Port of Athens", "Ship's Deck", "Athn02A", True, "Athn01F", False, "DocCam20", "WAD_Athn02A", 672.66, -623.69, 3610.69, &H80040001&))
        checkpoints.Add(New checkpoint("Port of Athens", "Minotaur's Scaffold", "Athn02A", True, "Athn01F", False, "ScafCam1", "WAD_Athn02A", -130.24, 162, 2866.68, &H80040001&))
        checkpoints.Add(New checkpoint("Port of Athens", "Cyclops Courtyard", "Athn02A", True, "Athn02B", True, "CrtYrdCam4", "WAD_Athn02B", 597.83, 952, 2598.45, &H80040001&))
        checkpoints.Add(New checkpoint("Port of Athens", "Medusa", "Athn02C", True, "Athn02D", True, "Room2Cam9", "WAD_Athn02C", -1240.2, 1096, 2139.76, &H80040001&))

        checkpoints.Add(New checkpoint("Road to Athens", "View of Ares", "Athn03A", True, "Athn03AA", True, "rightCam", "WAD_Athn03A", 155.47, 1120, -388.07, &H80040001&))
        checkpoints.Add(New checkpoint("Road to Athens", "Minotaur Room", "Athn03A", True, "Athn03AA", True, "minoRailCamON2", "WAD_Athn03A", -515.6, 1023, -279, &H80040001&))

        checkpoints.Add(New checkpoint("Athens Town Square", "Town Square", "Athn04A", True, "Athn03C", False, "RblCam1", "WAD_Athn04A", 1450.73, -85.51, 331.9, &H80040001&))

        checkpoints.Add(New checkpoint("Temple of the Oracle", "Bridge", "Athn06A", True, "Athn04D", True, "camOpen", "WAD_Athn06A", 3694.04, 656, 1283.74, &H80040001&))
        checkpoints.Add(New checkpoint("Temple of the Oracle", "Entrance", "Athn06A", False, "Athn06B", True, "EntryCam1", "WAD_Athn06B", -2408.42, 768, -1516.75, &H80000001&))
        checkpoints.Add(New checkpoint("Temple of the Oracle", "Second Floor", "Athn06A", False, "Athn06B", True, "TopCam6", "WAD_Athn06B", -2916.61, 992, -1216.51, &H80000001&))
        checkpoints.Add(New checkpoint("Temple of the Oracle", "Rescue", "Athn06C", True, "Athn06B", False, "LwrCam10", "WAD_Athn06C", -2916.61, 992, -1216.51, &H80000001&))

        checkpoints.Add(New checkpoint("Sewers", "Entrace", "sewr01", True, "Athn07", False, "camCmbt1", "WAD_sewr01", -1869.3, -27.63, 254.51, &H80000001&))
        checkpoints.Add(New checkpoint("Sewers", "Ladder", "Athn03D", False, "sewr02", True, "camin2Door", "WAD_sewr02", -5255.6, 164, -4628.01, &H80000001&))
        checkpoints.Add(New checkpoint("Sewers", "Stairs", "Athn03D", True, "sewr02", False, "camTunnel0A4", "WAD_Athn03D", 155.22, 504, 1110.8, &H80400001&))

        checkpoints.Add(New checkpoint("Road to Athens (Return)", "View", "Athn03D", True, "Athn03E", True, "rightCam", "WAD_Athn03E", 160.24, 1120, -396.74, &H80400001&))

        checkpoints.Add(New checkpoint("Desert", "Gate", "Dest00", True, "Athn03E", True, "camlGates2", "WAD_Athn03E", 2446.2, 976, -895.65, &H80400001&))
        checkpoints.Add(New checkpoint("Desert", "Statue", "Dest00", True, "Dest01", True, "MIMECam", "WAD_Dest01", 2366.88, 509.24, 1501.96, &H80400001&))
        checkpoints.Add(New checkpoint("Desert", "Through Door", "Dest02", True, "Dest01", True, "Cam46", "WAD_Dest02", 2449.03, 512, 6735.07, &H80400001&))
        checkpoints.Add(New checkpoint("Desert", "Door 2", "Dest02", True, "Dest03", True, "Cam30", "WAD_Dest02", 3291.3, 384, 6700, &H80400001&))
        checkpoints.Add(New checkpoint("Desert", "Horn", "Pand00A", False, "Dest03", True, "StrmCam6", "WAD_Dest03", 3678.04, 549.12, 3159.3, &H80400001&))

        checkpoints.Add(New checkpoint("Pandora's Temple", "Vista", "Pand00A", True, "Pand00B", False, "VistaCam2", "WAD_Pand00A", -721.03, 192, -1078.54, &H80400001&))
        checkpoints.Add(New checkpoint("Pandora's Temple", "Bottom of Stairs", "Pand00A", False, "Pand00B", True, "StartCam12", "WAD_Pand00B", 3105.45, -954.92, 1223.56, &H80400001&))
        checkpoints.Add(New checkpoint("Pandora's Temple", "Temple Entrance, Crank", "Pand00C", True, "Pand00B", True, "StartCam22", "WAD_Pand00B", 1520.32, 2, 1220.04, &H80000001&))
        checkpoints.Add(New checkpoint("Pandora's Temple", "Temple Entrance, Inside", "Pand00C", True, "Pand00B", True, "CombatCam1", "WAD_Pand00B", 1124.11, -4.76, 1232.71, &H80400001&))

        checkpoints.Add(New checkpoint("Rings of Pandora", "Rings Entrance", "Pand00C", True, "Pand00D", True, "Ring1Cam1", "WAD_Pand00C", 470.38, 0, 1238.9, &H80400001&))
        checkpoints.Add(New checkpoint("Rings of Pandora", "Eye", "Pand00C", True, "Pand00D", True, "CrushCam14", "WAD_Pand00D", -232.6, 0, 3592.32, &H80000001&))
        checkpoints.Add(New checkpoint("Rings of Pandora", "Muse Door", "Pand00C", True, "Pand00D", True, "GldShwrCam1", "WAD_Pand00D", -1505.17, 80, 1232.8, &H80000001&))
        checkpoints.Add(New checkpoint("Rings of Pandora", "Muse Room", "Pand00C", True, "Pand00D", True, "GldShwrCam3", "WAD_Pand00D", -1780.47, 84, 1231.69, &H80000001&))
        checkpoints.Add(New checkpoint("Rings of Pandora", "Middle Ring", "Pand00C", True, "Pand00D", True, "Ring3Cam3", "WAD_Pand00C", -570.4, 4.1, 1313.54, &H80400001&))
        checkpoints.Add(New checkpoint("Rings of Pandora", "Center", "Pand00D", True, "Pand00C", True, "Crank4Cam", "WAD_Pand00C", -252.75, 0.08, 1278, &H80400001&))

        checkpoints.Add(New checkpoint("Challenge of Atlas", "Blade of Artemis", "Pand00C", True, "Pand01A", True, "camera238", "WAD_Pand01A", -242.52, 3.37, 162.68, &H80000001&))
        checkpoints.Add(New checkpoint("Challenge of Atlas", "Challenge of Atlas", "Pand00C", True, "Pand01A", True, "camera11", "WAD_Pand01A", -590.73, -3.62, -662.2, &H80000001&))
        checkpoints.Add(New checkpoint("Challenge of Atlas", "Zeus Shield", "Pand01B", True, "Pand01A", True, "AltUpCam1", "WAD_Pand01A", -1004.26, 144, -1727.24, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Atlas", "Hades Shield", "Pand01D", True, "Pand01A", True, "camera252", "WAD_Pand01D", 93.15, -351.84, -705.49, &H80020001&))
        checkpoints.Add(New checkpoint("Challenge of Atlas", "Saw Room", "Pand01B", True, "Pand01A", True, "SawCam6", "WAD_Pand01B", -1357, 1, -1596.67, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Atlas", "Atlas Room", "Pand01B", True, "Pand01BB", True, "LowCam1", "WAD_Pand01BB", -1381, -2.7, -680.1, &H80200001&))
        checkpoints.Add(New checkpoint("Challenge of Atlas", "Architect's Son", "Pand00D", False, "Pand01BB", True, "BrdigeCam2", "WAD_Pand01BB", -2734.06, 4, -657.7, &H80000001&))

        checkpoints.Add(New checkpoint("Challenge of Poseidon", "Cerberus", "Pand02A", True, "Pand00C", False, "camBrdg1", "WAD_Pand02A", 349.56, -392, -306.12, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Poseidon", "Spinning Room", "Pand02A", True, "Pand03AA", False, "camSPNroom", "WAD_Pand02AA", -2330.27, -240, -1001.48, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Poseidon", "Prisoner's Cage", "Pand02B", False, "Pand02C", True, "cam2Cages", "WAD_Pand02C", -2089.43, 368, -3217.7, &H80100001&))
        checkpoints.Add(New checkpoint("Challenge of Poseidon", "Poseidon's Trident", "Pand02D", True, "Pand02C", True, "camMain2", "WAD_Pand02D", 1534.79, 256.48, -3009.78, &H80100001&))

        checkpoints.Add(New checkpoint("Challenge of Hades", "Hall of Hades", "Pand03A", True, "Pand03J", True, "Hallcam8", "WAD_Pand03J", 655.76, -96, 1312.82, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Hades", "Statue Pool", "Pand03A", True, "Pand03J", True, "Ocam2", "WAD_Pand03A", 0, -2.84, 147.45, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Hades", "Balls", "Pand03A", True, "Pand03C", True, "BlderJumpCam", "WAD_Pand03A", 0, 256, -940.54, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Hades", "Ceiling", "Pand03D", True, "Pand03E", True, "BBCam57", "WAD_Pand03E", -98.45, 455, -596.94, &H80020001&))
        checkpoints.Add(New checkpoint("Challenge of Hades", "Door Wheel", "Pand03F", True, "Pand03G", True, "WheelCam", "WAD_Pand03F", 191.48, 364, 1288.6, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Hades", "Mino Room", "Pand03F", True, "Pand03G", True, "camera4", "WAD_Pand03G", 677.27, -96, 1675.84, &H80400001&))
        checkpoints.Add(New checkpoint("Challenge of Hades", "Architect's Son", "Pand03H", True, "Pand03I", True, "TombCam2", "WAD_Pand03H", 224.44, 688, 1912.26, &H80400001&))

        checkpoints.Add(New checkpoint("Cliffs of Madness", "Hades Entrance", "Pand04AA", True, "Pand00C", False, "camEntrance", "WAD_Pand04AA", 28.32, -61.5, 1332.08, &H80400001&))
        checkpoints.Add(New checkpoint("Cliffs of Madness", "Elevator Save Point", "", False, "Pand04C", True, "Lyr1Cam4", "WAD_Pand04C", -282.33, 753.25, -1429.76, &H80020001&))
        checkpoints.Add(New checkpoint("Cliffs of Madness", "Extending Bridge", "", False, "Pand04E", True, "ZipCam18", "WAD_Pand04E", 774.35, 1491, -1870.91, &H80000001&))
        checkpoints.Add(New checkpoint("Cliffs of Madness", "Shape Puzzle", "Pand04EE", True, "Pand04E", True, "TetrisCam1", "WAD_Pand04EE", 754.23, 880, -4400, &H80040001&))
        checkpoints.Add(New checkpoint("Cliffs of Madness", "Architect's Tomb", "Pand04F", True, "Pand04G", True, "PlatCam1", "WAD_Pand04G", -172, 2715, -2679.57, &H80000001&))
        checkpoints.Add(New checkpoint("Cliffs of Madness", "Inside Tomb", "Pand04H", True, "Pand04G", True, "InterCam1", "WAD_Pand04G", -101, 2688, -4471.71, &H80010001&))
        checkpoints.Add(New checkpoint("Cliffs of Madness", "Conveyor Belt", "Pand04J", True, "Pand05A", True, "cam2Crushers", "WAD_Pand04J", 748.9, -1376, -5121.04, &H80010001&))
        checkpoints.Add(New checkpoint("Cliffs of Madness", "Pandora's Box", "", False, "Pand05A", True, "camCenterRail", "WAD_Pand05A", 2738.27, -1280, -5119.96, &H80010001&))

        checkpoints.Add(New checkpoint("Hades", "Entrance", "hdes01a", True, "", False, "PillarCam1", "WAD_Hdes01a", -8436.89, -4634.38, 512, &H80000001&))
        checkpoints.Add(New checkpoint("Hades", "Blade Pillar", "HDES01C", True, "", False, "HopCam6", "WAD_HDES01C", -1595.14, -3664, 414.35, &H80000001&))
        checkpoints.Add(New checkpoint("Hades", "Satyrs", "HDES01E", True, "HDES01D", True, "Dcam15", "WAD_HDES01D", -130.15, -1059.64, 990.4, &H80000001&))

        checkpoints.Add(New checkpoint("Return to the Temple", "Grave", "HDES01E", False, "AtRet01A", True, "CamCenL1", "WAD_ARet01A", 5070.11, 653.76, 1081.45, &H80010001&))
        checkpoints.Add(New checkpoint("Return to the Temple", "Final Battle Save Point", "AtRet01B", False, "AtRet01C", True, "LwrCam9", "WAD_ARet01C", -82.6, -19, -1501.65, &H80010001&))

        checkpoints.Add(New checkpoint("Ares", "Ares 1", "Athn08B", False, "Athn08a", True, "AresCamera1", "WAD_Athn08a", -9.96, -6.61, -149.23, &H80000001&))
        checkpoints.Add(New checkpoint("Ares", "Clone Fight", "Athn08B", True, "Athn08C", False, "WholeCam1", "WAD_Athn08b", 26.53, -175.81, -49.82, &H80000001&))
        checkpoints.Add(New checkpoint("Ares", "Ares 2", "Athn08B", False, "Athn08C", True, "AresCamera1", "WAD_Athn08c", -9.96, -6.61, -149.23, &H80000001&))
        checkpoints.Add(New checkpoint("Ares", "After Ares", "Athn07b", True, "Athn08C", True, "camMagical4", "WAD_Athn07b", 1118.65, 722.02, 806.92, &H80000001&))
        checkpoints.Add(New checkpoint("Ares", "Throne", "Olymp02", True, "olymp01", False, "HallCam5", "WAD_olymp02", -4.3, 50.13, -295.04, &H80000001&))

        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 1", "Arena10", True, "Arena08", False, "camera1", "WAD_Arena10", 0, 20.62, 245.75, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 2", "Arena06", False, "Arena08", True, "camera1", "WAD_Arena08", 0, 16.51, -3.14, &H80402001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 3", "Arena06", True, "Arena09", False, "camera1", "WAD_Arena06", -12.8, 20.6, 241.6, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 4", "Arena12", False, "Arena09", True, "camera1", "WAD_Arena09", -17.6, 20.6, 241.6, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 5", "Arena12", True, "Arena14", False, "camera1", "WAD_Arena12", 1.6, 27.71, 57.6, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 6", "Arena26", False, "Arena14", True, "camera1", "WAD_Arena14", 7.07, 14.75, -1.59, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 7", "Arena26", True, "Arena20", False, "camera1", "WAD_Arena26", -16, 20.62, 242.91, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 8", "Arena19", False, "Arena20", True, "camera1", "WAD_Arena20", -3.2, 20.62, 8.59, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 9", "Arena19", True, "Arena27", False, "camera1", "WAD_Arena19", 0, 20.62, 0, &H80002001&))
        checkpoints.Add(New checkpoint("Challenge of the Gods", "Challenge 10", "Arena19", False, "Arena27", True, "camera1", "WAD_Arena27", 0, 7.04, 246.01, &H80002001&))

        checkpoints.Add(New checkpoint("Misc", "Character Graveyard", "Olymp04", True, "", False, "camCombat1", "WAD_Olymp04", -16, -383.62, -304, &H80000001&))

        checkpoints.Add(New checkpoint("Speedrun Practice", "PoA, Edgehang", "Athn02A", True, "Athn01F", False, "LadderCam", "WAD_Athn02A", 656.2, -633.9, 3652.17, &H80000010&))


        For Each cp In checkpoints
            cmbG1Checkpoints.Items.Add(cp.Name)
        Next
    End Sub

    Private Sub cmbG1Checkpoints_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbG1Checkpoints.SelectedIndexChanged
        Dim cp As checkpoint = Nothing
        For Each check In checkpoints
            If check.Name = cmbG1Checkpoints.Text Then cp = check
        Next

        If cp IsNot Nothing Then
            txtG1Wad1.Text = cp.Wad1
            chkG1Wad1Active.Checked = cp.Wad1Active
            txtG1Wad2.Text = cp.Wad2
            chkG1Wad2Active.Checked = cp.Wad2Active

            txtG1Camera.Text = cp.CurrCam
            txtG1CamWad.Text = cp.CamWAD

            txtG1Xpos.Text = cp.xPos
            txtG1YPos.Text = cp.yPos
            txtG1Zpos.Text = cp.zPos

            chkG1Swim.Checked = (cp.stance And &H200)
            chkG1WorldIdle.Checked = cp.worldIdle

        End If
    End Sub

    Private Sub cmbG1checkpointCat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbG1checkpointCat.SelectedIndexChanged
        cmbG1Checkpoints.Items.Clear()
        For Each cp In checkpoints
            If cp.Category = cmbG1checkpointCat.Text Then cmbG1Checkpoints.Items.Add(cp.Name)
        Next
    End Sub
End Class
