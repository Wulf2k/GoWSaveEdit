Imports GoWEditor.PS3FileSystem

Public Class GoW2
    Public Shared bytes() As Byte
    Public Shared folder
    Public Shared filename
    Public Shared slotnum = "t"
    Public Shared modified = False
    Public Shared manager As Ps3SaveManager
    Public Shared file As Ps3File
    Public Shared SecureID() As Byte = {&H82, &H21, &H42, &HD2, &H27, &H74, &H97, &H6, &H62, &H25, &H46, &HE6, &HE7, &H20, &H6, &H27}
    Public Shared costume
    Public Shared difficulty
    Public Shared Unlocks1
    Public Shared Unlocks2
    Public Shared Unlocks3


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

    Private Function FourByteFloat(ByRef bytes, start) As String
        Return HexToSingle(Hex(bytes(start)).PadLeft(2, "0"c).ToString & Hex(bytes(start + 1)).PadLeft(2, "0"c).ToString & Hex(bytes(start + 2)).PadLeft(2, "0"c).ToString & Hex(bytes(start + 3)).PadLeft(2, "0"c).ToString)
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

    Private Sub G2openSave()

        btnG2Master.BackColor = Color.LightGray
        btnG2Master.ForeColor = Color.Black

        btnG2Slot1.BackColor = Color.LightGray
        btnG2Slot1.ForeColor = Color.Black

        btnG2Slot2.BackColor = Color.LightGray
        btnG2Slot2.ForeColor = Color.Black

        btnG2Slot3.BackColor = Color.LightGray
        btnG2Slot3.ForeColor = Color.Black

        btnG2Slot4.BackColor = Color.LightGray
        btnG2Slot4.ForeColor = Color.Black

        btnG2Slot5.BackColor = Color.LightGray
        btnG2Slot5.ForeColor = Color.Black

        btnG2Slot6.BackColor = Color.LightGray
        btnG2Slot6.ForeColor = Color.Black

        btnG2Slot7.BackColor = Color.LightGray
        btnG2Slot7.ForeColor = Color.Black

        btnG2Slot8.BackColor = Color.LightGray
        btnG2Slot8.ForeColor = Color.Black

        btnG2Slot9.BackColor = Color.LightGray
        btnG2Slot9.ForeColor = Color.Black

        btnG2Slot10.BackColor = Color.LightGray
        btnG2Slot10.ForeColor = Color.Black

        manager = New Ps3SaveManager(txtG2Folder.Text, SecureID)


        If slotnum = "t" Then
            filename = "MASTER.BIN"
            tctlG2Data.Visible = False
            gbG2Master.Visible = True

            btnG2Master.BackColor = Color.Black
            btnG2Master.ForeColor = Color.White

            file = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = filename)
            bytes = file.DecryptToBytes

            If bytes(4) <> 202 Then
                MsgBox("This file's opening bytes are not GoW-standard.  It is likely still encrypted, or save slot #1 is empty.  Unless you've manually modified this value, this program will probably crash now.")
            Else
                btnG2Open.Visible = False
                btnG2Browse.Visible = False
                btnG2Master.Visible = True
                btnG2Slot1.Visible = True
                btnG2Slot2.Visible = True
                btnG2Slot3.Visible = True
                btnG2Slot4.Visible = True
                btnG2Slot5.Visible = True
                btnG2Slot6.Visible = True
                btnG2Slot7.Visible = True
                btnG2Slot8.Visible = True
                btnG2Slot9.Visible = True
                btnG2Slot10.Visible = True
            End If

            If (bytes(162) And 1) = 1 Then chkG2CyclopsEyesMaster.Checked = True Else chkG2CyclopsEyesMaster.Checked = False
            If (bytes(163) And 1) = 1 Then chkG2BeatEMaster.Checked = True Else chkG2BeatEMaster.Checked = False
            If (bytes(163) And 2) = 2 Then chkG2BeatNMaster.Checked = True Else chkG2BeatNMaster.Checked = False
            If (bytes(163) And 4) = 4 Then chkG2BeatHMaster.Checked = True Else chkG2BeatHMaster.Checked = False
            If (bytes(163) And 8) = 8 Then chkG2BeatVHMaster.Checked = True Else chkG2BeatVHMaster.Checked = False
            If (bytes(163) And 16) = 16 Then chkG2MortalRankMaster.Checked = True Else chkG2MortalRankMaster.Checked = False
            If (bytes(163) And 32) = 32 Then chkG2SpartanRankMaster.Checked = True Else chkG2SpartanRankMaster.Checked = False
            If (bytes(163) And 64) = 64 Then chkG2GodRankMaster.Checked = True Else chkG2GodRankMaster.Checked = False
            If (bytes(163) And 128) = 128 Then chkG2TitanRankMaster.Checked = True Else chkG2TitanRankMaster.Checked = False

            modified = False

        Else
            filename = "DATA0" + slotnum + ".BIN"
            tctlG2Data.Visible = True
            gbG2Master.Visible = False

            file = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = filename)
            bytes = file.DecryptToBytes

            If bytes(0) <> 202 Then MsgBox("This file's opening byte is not GoW-standard.  It likely failed decryption.  Unless you've manually modified this value, this program will probably crash now.")

            costume = bytes(7)

            Select Case costume
                Case 0
                    rdbG2Kratos.Checked = True
                Case 1
                    rdbG2KratosGodArmor.Checked = True
                Case 2
                    rdbG2KratosInjured.Checked = True
                Case 10
                    rdbG2Cod.Checked = True
                Case 11
                    rdbG2Hydra.Checked = True
                Case 12
                    rdbG2DarkOdyssey.Checked = True
                Case 13
                    rdbG2Athena.Checked = True
                Case 14
                    rdbG2Hercules.Checked = True
                Case 15
                    rdbG2GeneralKratos.Checked = True
                Case 16
                    rdbG2GodOfWar.Checked = True
                Case Else
                    rdbG2Kratos.Checked = True
            End Select

            txtG2Wad1.Text = ""
            txtG2Wad2.Text = ""

            For i = 8 To 27
                txtG2Wad1.Text = txtG2Wad1.Text + Chr(bytes(i))
            Next

            For i = 29 To 48
                txtG2Wad2.Text = txtG2Wad2.Text + Chr(bytes(i))
            Next


            txtG2Xpos.Text = FourByteFloat(bytes, 118)
            txtG2Height.Text = FourByteFloat(bytes, 122)
            txtG2YPos.Text = FourByteFloat(bytes, 126)

            If bytes(144) > 0 And bytes(144) < 4 Then
                chkG2Swim.Checked = True
            Else
                chkG2Swim.Checked = False
            End If


            Unlocks1 = bytes(155)
            If (Unlocks1 And 128) = 128 Then chkG2UrnPoseidonAcq.Checked = True Else chkG2UrnPoseidonAcq.Checked = False
            If (Unlocks1 And 64) = 64 Then chkG2UrnFatesAcq.Checked = True Else chkG2UrnFatesAcq.Checked = False
            If (Unlocks1 And 32) = 32 Then chkG2UrnPrometheusAcq.Checked = True Else chkG2UrnPrometheusAcq.Checked = False
            If (Unlocks1 And 16) = 16 Then chkG2UrnOlympusAcq.Checked = True Else chkG2UrnOlympusAcq.Checked = False
            If (Unlocks1 And 8) = 8 Then chkG2UrnGorgonAcq.Checked = True Else chkG2UrnGorgonAcq.Checked = False
            If (Unlocks1 And 4) = 4 Then chkG2UrnGaiaAcq.Checked = True Else chkG2UrnGaiaAcq.Checked = False
            If (Unlocks1 And 2) = 2 Then chkG2UnkUnl1_02.Checked = True Else chkG2UnkUnl1_02.Checked = False
            If (Unlocks1 And 1) = 1 Then chkG2BoOUnl.Checked = True Else chkG2BoOUnl.Checked = False

            Unlocks2 = bytes(156)
            If (Unlocks2 And 128) = 128 Then chkG2EHAcq.Checked = True Else chkG2EHAcq.Checked = False
            If (Unlocks2 And 64) = 64 Then chkG2BHUnl.Checked = True Else chkG2BHUnl.Checked = False
            If (Unlocks2 And 32) = 32 Then chkG2SoDUnl.Checked = True Else chkG2SoDUnl.Checked = False
            If (Unlocks2 And 16) = 16 Then chkG2UnkUnl2_10.Checked = True Else chkG2UnkUnl2_10.Checked = False
            If (Unlocks2 And 8) = 8 Then chkG2UnkUnl2_08.Checked = True Else chkG2UnkUnl2_08.Checked = False
            If (Unlocks2 And 4) = 4 Then chkG2AmuletAcq.Checked = True Else chkG2AmuletAcq.Checked = False
            If (Unlocks2 And 2) = 2 Then chkG2RageAcq.Checked = True Else chkG2RageAcq.Checked = False
            If (Unlocks2 And 1) = 1 Then chkG2UnkUnl2_01.Checked = True Else chkG2UnkUnl2_01.Checked = False

            Unlocks3 = bytes(157)
            If (Unlocks3 And 128) = 128 Then chkG2UnkUnl3_80.Checked = True Else chkG2UnkUnl3_80.Checked = False
            If (Unlocks3 And 64) = 64 Then chkG2WingsAcq.Checked = True Else chkG2WingsAcq.Checked = False
            If (Unlocks3 And 32) = 32 Then chkG2AQAcq.Checked = True Else chkG2AQAcq.Checked = False
            If (Unlocks3 And 16) = 16 Then chkG2FleeceAcq.Checked = True Else chkG2FleeceAcq.Checked = False
            If (Unlocks3 And 8) = 8 Then chkG2UnkUnl3_08.Checked = True Else chkG2UnkUnl3_08.Checked = False
            If (Unlocks3 And 4) = 4 Then chkG2TBAcq.Checked = True Else chkG2TBAcq.Checked = False
            If (Unlocks3 And 2) = 2 Then chkG2CRAcq.Checked = True Else chkG2CRAcq.Checked = False
            If (Unlocks3 And 1) = 1 Then chkG2PRAcq.Checked = True Else chkG2PRAcq.Checked = False

            txtG2Health.Text = FourByteFloat(bytes, 162)
            txtG2Magic.Text = FourByteFloat(bytes, 166)
            txtG2Item.Text = FourByteFloat(bytes, 170)
            txtG2Rage.Text = FourByteFloat(bytes, 174)
            txtG2MagicRegen.Text = FourByteFloat(bytes, 178)

            If bytes(182) < 128 Then
                txtG2RedOrbs.Text = (bytes(182) * 16 ^ 6) + (bytes(183) * 16 ^ 4) + (bytes(184) * 16 ^ 2) + bytes(185)
            Else
                txtG2RedOrbs.Text = (((bytes(182) - 128) * 16 ^ 6) + (bytes(183) * 16 ^ 4) + (bytes(184) * 16 ^ 2) + bytes(185)) - 2147483648
            End If

            txtG2HealthExt.Text = bytes(204)
            txtG2MagicExt.Text = bytes(205)
            txtG2ItemExt.Text = bytes(206)
            txtG2BoAlvl.Text = bytes(207) + 1

            txtG2AQlvl.Text = bytes(209) + 1
            txtG2CRlvl.Text = bytes(210) + 1
            txtG2PRlvl.Text = bytes(211) + 1
            txtG2TBlvl.Text = bytes(212) + 1
            txtG2SoDlvl.Text = bytes(213) + 1
            txtG2BHlvl.Text = bytes(214) + 1
            txtG2EHlvl.Text = bytes(215) + 1
            txtG2BoOlvl.Text = bytes(216) + 1

            If bytes(217) = 1 Then rdbG2PRSel.Checked = True Else rdbG2PRSel.Checked = False
            If bytes(217) = 2 Then rdbG2CRSel.Checked = True Else rdbG2CRSel.Checked = False
            If bytes(217) = 3 Then rdbG2TBSel.Checked = True Else rdbG2TBSel.Checked = False
            If bytes(217) = 6 Then rdbG2AQSel.Checked = True Else rdbG2AQSel.Checked = False
            If bytes(217) = 16 Then rdbG2EHSel.Checked = True Else rdbG2EHSel.Checked = False

            If bytes(218) = 14 Then rdbG2SoDSel.Checked = True Else rdbG2SoDSel.Checked = False
            If bytes(218) = 15 Then rdbG2BHSel.Checked = True Else rdbG2BHSel.Checked = False
            If bytes(218) = 17 Then rdbG2BoOSel.Checked = True Else rdbG2BoOSel.Checked = False

            If bytes(219) = 1 Then chkG2ForcedSubweapon.Checked = False Else chkG2ForcedSubweapon.Checked = True

            txtG2GorgonEyes.Text = bytes(220)
            txtG2PhoenixFeathers.Text = bytes(221)

            txtG2CyclopsEyes.Text = bytes(226)

            txtG2Camera.Text = ""
            txtG2CamWad.Text = ""

            For i = 246 To 269
                If bytes(i) > 0 Then
                    txtG2Camera.Text = txtG2Camera.Text + Chr(bytes(i))
                Else
                    i = 269
                End If
            Next

            For i = 270 To 293
                If bytes(i) > 0 Then
                    txtG2CamWad.Text = txtG2CamWad.Text + Chr(bytes(i))
                Else
                    i = 293
                End If
            Next


            If bytes(2110) < 128 Then
                txtG2SecsPlayed.Text = (bytes(2110) * 16 ^ 6) + (bytes(2111) * 16 ^ 4) + (bytes(2112) * 16 ^ 2) + bytes(2113)
            Else
                txtG2SecsPlayed.Text = (((bytes(2110) - 128) * 16 ^ 6) + (bytes(2111) * 16 ^ 4) + (bytes(2112) * 16 ^ 2) + bytes(2113)) - 2147483648
            End If




            If bytes(2115) = 0 Then rdbG2Easy.Checked = True
            If bytes(2115) = 1 Then rdbG2Normal.Checked = True
            If bytes(2115) = 2 Then rdbG2Hard.Checked = True
            If bytes(2115) = 3 Then rdbG2VeryHard.Checked = True

            If bytes(2116) = 1 Then chkG2InfiniteHealth.Checked = True Else chkG2InfiniteHealth.Checked = False
            If bytes(2117) = 1 Then chkG2UrnOlympusActive.Checked = True Else chkG2UrnOlympusActive.Checked = False
            If bytes(2118) = 1 Then chkG2UrnPrometheusActive.Checked = True Else chkG2UrnPrometheusActive.Checked = False
            If bytes(2119) = 1 Then chkG2UrnGorgonsActive.Checked = True Else chkG2UrnGorgonsActive.Checked = False
            If bytes(2120) = 1 Then chkG2UrnPoseidonActive.Checked = True Else chkG2UrnPoseidonActive.Checked = False
            If bytes(2121) = 1 Then chkG2UrnGaiaActive.Checked = True Else chkG2UrnGaiaActive.Checked = False
            If bytes(2122) = 1 Then chkG2UrnFatesActive.Checked = True Else chkG2UrnFatesActive.Checked = False

            If bytes(2124) = 1 Then chkG2Bonus.Checked = True Else chkG2Bonus.Checked = False

            txtG2TimesSaved.Text = bytes(2197)



            Dim checksum As ULong
            Dim power As ULong
            Dim csum As String

            For i = 0 To (bytes.Length - 5)
                power = 16 ^ (2 * (3 - (i Mod 4)))
                checksum = checksum + bytes(i) * power
            Next

            csum = Microsoft.VisualBasic.Right(Hex(checksum).ToString, 8)

            txtG2Checksum.Text = csum

            For i = 1 To 7 Step 2
                bytes((bytes.Length - 5) + (i + 1) / 2) = Integer.Parse(Mid(csum, i, 2), System.Globalization.NumberStyles.HexNumber)
            Next

            modified = False
        End If

    End Sub

    Private Sub btnG2Open_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Open.Click

        slotnum = "t"
        G2openSave()


    End Sub

    Private Sub txtG2Health_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Health.TextChanged
        modified = True
        numRangeCheck(txtG2Health, 1, 200)
    End Sub

    Private Sub txtG2Magic_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Magic.TextChanged
        modified = True
        numRangeCheck(txtG2Magic, 0, 200)
    End Sub

    Private Sub txtG2Rage_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Rage.TextChanged
        modified = True
        numRangeCheck(txtG2Rage, 0, 100)
    End Sub

    Private Sub txtG2RedOrbs_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2RedOrbs.TextChanged
        modified = True
    End Sub

    Private Sub txtG2HealthExt_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2HealthExt.TextChanged
        modified = True
        numRangeCheck(txtG2HealthExt, 0, 4)
    End Sub

    Private Sub txtG2MagicExt_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2MagicExt.TextChanged
        modified = True
        numRangeCheck(txtG2MagicExt, 0, 5)
    End Sub

    Private Sub txtG2GorgonEyes_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2GorgonEyes.TextChanged
        modified = True
        numRangeCheck(txtG2GorgonEyes, 0, 24)
    End Sub

    Private Sub txtG2PhoenixFeathers_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2PhoenixFeathers.TextChanged
        modified = True
        numRangeCheck(txtG2PhoenixFeathers, 0, 24)
    End Sub

    Private Sub txtG2BoO_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2BoOlvl.TextChanged
        modified = True
        numRangeCheck(txtG2BoOlvl, 1, 3)
    End Sub

    Private Sub txtG2SoD_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2SoDlvl.TextChanged
        modified = True
        numRangeCheck(txtG2SoDlvl, 1, 3)
    End Sub

    Private Sub txtG2BH_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2BHlvl.TextChanged
        modified = True
        numRangeCheck(txtG2BHlvl, 1, 3)
    End Sub

    Private Sub txtG2CyclopsEyes_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2CyclopsEyes.TextChanged
        modified = True
        numRangeCheck(txtG2CyclopsEyes, 0, 20)
    End Sub

    Private Sub chkG2Swim_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2Swim.CheckedChanged
        modified = True
    End Sub

    Private Sub btnG2Save_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Save.Click

        file = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = filename)
        bytes = file.DecryptToBytes

        modified = False


        If filename = "MASTER.BIN" Then
            bytes(162) = Math.Abs(chkG2CyclopsEyesMaster.Checked * 1)

            bytes(163) = Math.Abs(chkG2TitanRankMaster.Checked * 128)
            bytes(163) = bytes(163) + Math.Abs(chkG2GodRankMaster.Checked * 64)
            bytes(163) = bytes(163) + Math.Abs(chkG2SpartanRankMaster.Checked * 32)
            bytes(163) = bytes(163) + Math.Abs(chkG2MortalRankMaster.Checked * 16)
            bytes(163) = bytes(163) + Math.Abs(chkG2BeatVHMaster.Checked * 8)
            bytes(163) = bytes(163) + Math.Abs(chkG2BeatHMaster.Checked * 4)
            bytes(163) = bytes(163) + Math.Abs(chkG2BeatNMaster.Checked * 2)
            bytes(163) = bytes(163) + Math.Abs(chkG2BeatEMaster.Checked * 1)


        Else

            bytes(7) = costume

            For i = 0 To 13
                If (i < txtG2Wad1.TextLength) Then
                    bytes(i + 8) = Asc(txtG2Wad1.Text(i))
                Else
                    bytes(i + 8) = 0
                End If
            Next

            If txtG2Wad1.TextLength > 0 Then bytes(28) = 1 Else bytes(28) = 0

            For i = 0 To 13
                If (i < txtG2Wad2.TextLength) Then
                    bytes(i + 29) = Asc(txtG2Wad2.Text(i))
                Else
                    bytes(i + 29) = 0
                End If
            Next

            If txtG2Wad2.TextLength > 0 Then bytes(49) = 1 Else bytes(49) = 0

            bytes(118) = SingleToHex(txtG2Xpos.Text, 1)
            bytes(119) = SingleToHex(txtG2Xpos.Text, 2)
            bytes(120) = SingleToHex(txtG2Xpos.Text, 3)
            bytes(121) = SingleToHex(txtG2Xpos.Text, 4)

            bytes(122) = SingleToHex(txtG2Height.Text, 1)
            bytes(123) = SingleToHex(txtG2Height.Text, 2)
            bytes(124) = SingleToHex(txtG2Height.Text, 3)
            bytes(125) = SingleToHex(txtG2Height.Text, 4)

            bytes(126) = SingleToHex(txtG2YPos.Text, 1)
            bytes(127) = SingleToHex(txtG2YPos.Text, 2)
            bytes(128) = SingleToHex(txtG2YPos.Text, 3)
            bytes(129) = SingleToHex(txtG2YPos.Text, 4)

            If chkG2Swim.Checked = True Then bytes(144) = 2 Else bytes(144) = 0

            Unlocks1 = Math.Abs(chkG2BoOUnl.Checked * 1)
            Unlocks1 = Unlocks1 + Math.Abs(chkG2UnkUnl1_02.Checked * 2)
            Unlocks1 = Unlocks1 + Math.Abs(chkG2UrnGaiaAcq.Checked * 4)
            Unlocks1 = Unlocks1 + Math.Abs(chkG2UrnGorgonAcq.Checked * 8)
            Unlocks1 = Unlocks1 + Math.Abs(chkG2UrnOlympusAcq.Checked * 16)
            Unlocks1 = Unlocks1 + Math.Abs(chkG2UrnPrometheusAcq.Checked * 32)
            Unlocks1 = Unlocks1 + Math.Abs(chkG2UrnFatesAcq.Checked * 64)
            Unlocks1 = Unlocks1 + Math.Abs(chkG2UrnPoseidonAcq.Checked * 128)
            bytes(155) = Unlocks1

            Unlocks2 = Math.Abs(chkG2UnkUnl2_01.Checked * 1)
            Unlocks2 = Unlocks2 + Math.Abs(chkG2RageAcq.Checked * 2)
            Unlocks2 = Unlocks2 + Math.Abs(chkG2AmuletAcq.Checked * 4)
            Unlocks2 = Unlocks2 + Math.Abs(chkG2UnkUnl2_08.Checked * 8)
            Unlocks2 = Unlocks2 + Math.Abs(chkG2UnkUnl2_10.Checked * 16)
            Unlocks2 = Unlocks2 + Math.Abs(chkG2SoDUnl.Checked * 32)
            Unlocks2 = Unlocks2 + Math.Abs(chkG2BHUnl.Checked * 64)
            Unlocks2 = Unlocks2 + Math.Abs(chkG2EHAcq.Checked * 128)
            bytes(156) = Unlocks2

            Unlocks3 = Math.Abs(chkG2PRAcq.Checked * 1)
            Unlocks3 = Unlocks3 + Math.Abs(chkG2CRAcq.Checked * 2)
            Unlocks3 = Unlocks3 + Math.Abs(chkG2TBAcq.Checked * 4)
            Unlocks3 = Unlocks3 + Math.Abs(chkG2UnkUnl3_08.Checked * 8)
            Unlocks3 = Unlocks3 + Math.Abs(chkG2FleeceAcq.Checked * 16)
            Unlocks3 = Unlocks3 + Math.Abs(chkG2AQAcq.Checked * 32)
            Unlocks3 = Unlocks3 + Math.Abs(chkG2WingsAcq.Checked * 64)
            Unlocks3 = Unlocks3 + Math.Abs(chkG2UnkUnl3_80.Checked * 128)
            bytes(157) = Unlocks3

            bytes(162) = SingleToHex(txtG2Health.Text, 1)
            bytes(163) = SingleToHex(txtG2Health.Text, 2)
            bytes(164) = SingleToHex(txtG2Health.Text, 3)
            bytes(165) = SingleToHex(txtG2Health.Text, 4)

            bytes(166) = SingleToHex(txtG2Magic.Text, 1)
            bytes(167) = SingleToHex(txtG2Magic.Text, 2)
            bytes(168) = SingleToHex(txtG2Magic.Text, 3)
            bytes(169) = SingleToHex(txtG2Magic.Text, 4)

            bytes(170) = SingleToHex(txtG2Item.Text, 1)
            bytes(171) = SingleToHex(txtG2Item.Text, 2)
            bytes(172) = SingleToHex(txtG2Item.Text, 3)
            bytes(173) = SingleToHex(txtG2Item.Text, 4)

            bytes(174) = SingleToHex(txtG2Rage.Text, 1)
            bytes(175) = SingleToHex(txtG2Rage.Text, 2)
            bytes(176) = SingleToHex(txtG2Rage.Text, 3)
            bytes(177) = SingleToHex(txtG2Rage.Text, 4)

            bytes(178) = SingleToHex(txtG2MagicRegen.Text, 1)
            bytes(179) = SingleToHex(txtG2MagicRegen.Text, 2)
            bytes(180) = SingleToHex(txtG2MagicRegen.Text, 3)
            bytes(181) = SingleToHex(txtG2MagicRegen.Text, 4)

            If txtG2RedOrbs.Text >= 0 Then
                bytes(182) = Math.Floor(txtG2RedOrbs.Text / (256 ^ 3))
                bytes(183) = Math.Abs(Math.Floor(txtG2RedOrbs.Text / (256 ^ 2)) Mod 256)
                bytes(184) = Math.Abs(Math.Floor(txtG2RedOrbs.Text / 256) Mod 256)
                bytes(185) = Math.Abs(txtG2RedOrbs.Text Mod 256)
            Else
                Dim tempOrbs As Long
                tempOrbs = (2147483648 + txtG2RedOrbs.Text)
                bytes(182) = Math.Floor(tempOrbs / (256 ^ 3)) + 128
                bytes(183) = Math.Abs(Math.Floor(tempOrbs / (256 ^ 2)) Mod 256)
                bytes(184) = Math.Abs(Math.Floor(tempOrbs / 256) Mod 256)
                bytes(185) = Math.Abs(tempOrbs Mod 256)
            End If

            bytes(204) = txtG2HealthExt.Text
            bytes(205) = txtG2MagicExt.Text
            bytes(206) = txtG2ItemExt.Text
            bytes(207) = txtG2BoAlvl.Text - 1
            bytes(209) = txtG2AQlvl.Text - 1
            bytes(210) = txtG2CRlvl.Text - 1
            bytes(211) = txtG2PRlvl.Text - 1
            bytes(212) = txtG2TBlvl.Text - 1
            bytes(213) = txtG2SoDlvl.Text - 1
            bytes(214) = txtG2BHlvl.Text - 1
            bytes(215) = txtG2EHlvl.Text - 1
            bytes(216) = txtG2BoOlvl.Text - 1

            bytes(217) = 0
            If rdbG2PRSel.Checked = True Then bytes(217) = 1
            If rdbG2CRSel.Checked = True Then bytes(217) = 2
            If rdbG2TBSel.Checked = True Then bytes(217) = 3
            If rdbG2AQSel.Checked = True Then bytes(217) = 6
            If rdbG2EHSel.Checked = True Then bytes(217) = 16

            bytes(218) = 0
            If rdbG2SoDSel.Checked = True Then bytes(218) = 14
            If rdbG2BHSel.Checked = True Then bytes(218) = 15
            If rdbG2BoOSel.Checked = True Then bytes(218) = 17

            bytes(219) = chkG2ForcedSubweapon.Checked + 1

            bytes(220) = txtG2GorgonEyes.Text
            bytes(221) = txtG2PhoenixFeathers.Text

            bytes(226) = txtG2CyclopsEyes.Text

            bytes(230) = bytes(7)






            For i = 0 To 16
                If (i < txtG2Camera.TextLength) Then
                    bytes(i + 246) = Asc(txtG2Camera.Text(i))
                Else
                    bytes(i + 246) = 0
                End If
            Next

            For i = 0 To 16
                If (i < txtG2CamWad.TextLength) Then
                    bytes(i + 270) = Asc(txtG2CamWad.Text(i))
                Else
                    bytes(i + 270) = 0
                End If
            Next

            If txtG2SecsPlayed.Text >= 0 Then
                bytes(2110) = Math.Floor(txtG2SecsPlayed.Text / (256 ^ 3))
                bytes(2111) = Math.Abs(Math.Floor(txtG2SecsPlayed.Text / (256 ^ 2)) Mod 256)
                bytes(2112) = Math.Abs(Math.Floor(txtG2SecsPlayed.Text / 256) Mod 256)
                bytes(2113) = Math.Abs(txtG2SecsPlayed.Text Mod 256)
            Else
                Dim tempSecs As Long
                tempSecs = (2147483648 + txtG2SecsPlayed.Text)
                bytes(2110) = Math.Floor(tempSecs / (256 ^ 3)) + 128
                bytes(2111) = Math.Abs(Math.Floor(tempSecs / (256 ^ 2)) Mod 256)
                bytes(2112) = Math.Abs(Math.Floor(tempSecs / 256) Mod 256)
                bytes(2113) = Math.Abs(tempSecs Mod 256)
            End If


            bytes(2114) = Math.Abs(chkG2Frozen.Checked * 1)

            If rdbG2Easy.Checked = True Then bytes(2115) = 0
            If rdbG2Normal.Checked = True Then bytes(2115) = 1
            If rdbG2Hard.Checked = True Then bytes(2115) = 2
            If rdbG2VeryHard.Checked = True Then bytes(2115) = 3

            bytes(2116) = Math.Abs(chkG2InfiniteHealth.Checked * 1)
            bytes(2117) = Math.Abs(chkG2InfiniteMagic.Checked * 1)
            bytes(2118) = Math.Abs(chkG2InfiniteRage.Checked * 1)
            bytes(2119) = Math.Abs(chkG2UrnGorgonsActive.Checked * 1)
            bytes(2120) = Math.Abs(chkG2UrnPoseidonActive.Checked * 1)
            bytes(2121) = Math.Abs(chkG2UrnGaiaActive.Checked * 1)
            bytes(2122) = Math.Abs(chkG2UrnFatesActive.Checked * 1)

            bytes(2124) = Math.Abs(chkG2Bonus.Checked * 1)

            bytes(2197) = txtG2TimesSaved.Text



            Dim checksum As ULong
            Dim power As ULong
            Dim csum As String


            For i = 0 To (bytes.Length - 5)
                power = 16 ^ (2 * (3 - (i Mod 4)))
                checksum = checksum + bytes(i) * power
            Next

            csum = Microsoft.VisualBasic.Right(Hex(checksum).ToString, 8)

            txtG2Checksum.Text = csum

            For i = 1 To 7 Step 2
                bytes((bytes.Length - 5) + (i + 1) / 2) = Integer.Parse(Mid(csum, i, 2), System.Globalization.NumberStyles.HexNumber)
            Next

            Dim filemast As Ps3File = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = "MASTER.BIN")
            Dim bytesmast = filemast.DecryptToBytes

            bytesmast(4 + 16 * Val(slotnum)) = 202
            bytesmast(5 + 16 * Val(slotnum)) = 254
            bytesmast(6 + 16 * Val(slotnum)) = 186
            bytesmast(7 + 16 * Val(slotnum)) = 212

            bytesmast(8 + 16 * Val(slotnum)) = bytes(2110)
            bytesmast(9 + 16 * Val(slotnum)) = bytes(2111)
            bytesmast(10 + 16 * Val(slotnum)) = bytes(2112)
            bytesmast(11 + 16 * Val(slotnum)) = bytes(2113)

            If bytesmast(13 + 16 * Val(slotnum)) = 0 Then bytesmast(13 + 16 * Val(slotnum)) = 1
            bytesmast(14 + 16 * Val(slotnum)) = bytes(2115)
            bytesmast(15 + 16 * Val(slotnum)) = bytes(2124)

            filemast.Encrypt(bytesmast)

        End If


        file.Encrypt(bytes)
        manager.ReBuildChanges()

        MsgBox("Save Completed")
    End Sub

    Private Sub txtG2Browse_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Browse.Click

        Dim openDlg As New OpenFileDialog()
        openDlg.Filter = "GoW Save File|MASTER.bin"
        openDlg.Title = "Open your MASTER save file"

        If openDlg.ShowDialog() = Windows.Forms.DialogResult.OK Then txtG2Folder.Text = Microsoft.VisualBasic.Left(openDlg.FileName, openDlg.FileName.Length - 10)
    End Sub

    Private Sub GoW2_close(sender As System.Object, e As System.EventArgs) Handles MyBase.FormClosed
        Main.Close()
    End Sub

    Private Sub txtG2File_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Folder.TextChanged
        filename = UCase(txtG2Folder.Text)
    End Sub

    Private Sub btnG2Treasures_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Master.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "t"
            G2openSave()
            btnG2Master.BackColor = Color.Black
            btnG2Master.ForeColor = Color.White
        End If
    End Sub

    Private Sub btnG2Slot1_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot1.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "0"
            G2openSave()

            btnG2Slot1.BackColor = Color.Black
            btnG2Slot1.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot2_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot2.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "1"
            G2openSave()

            btnG2Slot2.BackColor = Color.Black
            btnG2Slot2.ForeColor = Color.White
        End If
    End Sub

    Private Sub btnG2Slot3_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot3.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "2"
            G2openSave()

            btnG2Slot3.BackColor = Color.Black
            btnG2Slot3.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot4_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot4.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "3"
            G2openSave()

            btnG2Slot4.BackColor = Color.Black
            btnG2Slot4.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot5_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot5.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "4"
            G2openSave()

            btnG2Slot5.BackColor = Color.Black
            btnG2Slot5.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot6_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot6.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "5"
            G2openSave()

            btnG2Slot6.BackColor = Color.Black
            btnG2Slot6.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot7_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot7.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "6"
            G2openSave()

            btnG2Slot7.BackColor = Color.Black
            btnG2Slot7.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot8_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot8.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "7"
            G2openSave()

            btnG2Slot8.BackColor = Color.Black
            btnG2Slot8.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot9_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot9.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "8"
            G2openSave()

            btnG2Slot9.BackColor = Color.Black
            btnG2Slot9.ForeColor = Color.White

        End If
    End Sub

    Private Sub btnG2Slot10_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Slot10.Click
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = "9"
            G2openSave()

            btnG2Slot10.BackColor = Color.Black
            btnG2Slot10.ForeColor = Color.White

        End If
    End Sub

    Private Sub chkG2PT_CheckedChanged(sender As System.Object, e As System.EventArgs)
        modified = True
    End Sub

    Private Sub chkG2BeatGameMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2BeatEMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2BeatVHMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2BeatVHMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2BeatCotGMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2MortalRankMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2StatuesMaster_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2SpartanRankMaster.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG2Easy_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Easy.CheckedChanged
        modified = True
        If rdbG2Easy.Checked = True Then difficulty = 0
    End Sub

    Private Sub rdbG2Normal_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Normal.CheckedChanged
        modified = True
        If rdbG2Normal.Checked = True Then difficulty = 1
    End Sub

    Private Sub rdbG2Hard_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Hard.CheckedChanged
        modified = True
        If rdbG2Hard.Checked = True Then difficulty = 2
    End Sub

    Private Sub rdbG2VeryHard_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2VeryHard.CheckedChanged
        modified = True
        If rdbG2VeryHard.Checked = True Then difficulty = 3
    End Sub

    Private Sub rdbG2Kratos_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Kratos.CheckedChanged
        modified = True
        If rdbG2Kratos.Checked = True Then costume = 0
    End Sub

    Private Sub rdbG2KratosGodArmor_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2KratosGodArmor.CheckedChanged
        modified = True
        If rdbG2KratosGodArmor.Checked = True Then costume = 1
    End Sub

    Private Sub rdbG2KratosInjured_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2KratosInjured.CheckedChanged
        modified = True
        If rdbG2KratosInjured.Checked = True Then costume = 2
    End Sub

    Private Sub rdbG2Cod_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Cod.CheckedChanged
        modified = True
        If rdbG2Cod.Checked = True Then costume = 10
    End Sub

    Private Sub rdbG2Hydra_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Hydra.CheckedChanged
        modified = True
        If rdbG2Hydra.Checked = True Then costume = 11
    End Sub

    Private Sub rdbG2DarkOdyssey_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2DarkOdyssey.CheckedChanged
        modified = True
        If rdbG2DarkOdyssey.Checked = True Then costume = 12
    End Sub

    Private Sub rdbG2Athena_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Athena.CheckedChanged
        modified = True
        If rdbG2Athena.Checked = True Then costume = 13
    End Sub

    Private Sub rdbG2Hercules_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2Hercules.CheckedChanged
        modified = True
        If rdbG2Hercules.Checked = True Then costume = 14
    End Sub

    Private Sub rdbG2GeneralKratos_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2GeneralKratos.CheckedChanged
        modified = True
        If rdbG2GeneralKratos.Checked = True Then costume = 15
    End Sub

    Private Sub rdbG2GodOfWar_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2GodOfWar.CheckedChanged
        modified = True
        If rdbG2GodOfWar.Checked = True Then costume = 16
    End Sub

    Private Sub txtG2Xpos_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Xpos.TextChanged
        modified = True
    End Sub

    Private Sub txtG2YPos_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2YPos.TextChanged
        modified = True
    End Sub

    Private Sub txtG2Height_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Height.TextChanged
        modified = True
    End Sub

    Private Sub txtG2Wad1_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Wad1.TextChanged
        modified = True
    End Sub

    Private Sub txtG2Wad2_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Wad2.TextChanged
        modified = True
    End Sub

    Private Sub txtG2Camera_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Camera.TextChanged
        modified = True
    End Sub

    Private Sub txtG2CamWad_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2CamWad.TextChanged
        modified = True
    End Sub

    Private Sub txtG2SecsPlayed_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2SecsPlayed.TextChanged
        modified = True
    End Sub

    Private Sub btnG2Restart_Click(sender As System.Object, e As System.EventArgs) Handles btnG2Restart.Click
        If MsgBox("Revert save to start of game?", vbYesNo) = vbYes Then
            modified = False
            Dim oFileStream As System.IO.FileStream
            Dim blankbytes = My.Resources.G2BlankSave

            oFileStream = New System.IO.FileStream(filename, System.IO.FileMode.Create)
            oFileStream.Write(blankbytes, 0, blankbytes.Length)
            oFileStream.Close()

            G2openSave()

            Select Case slotnum
                Case 0
                    btnG2Slot1.BackColor = Color.Black
                    btnG2Slot1.ForeColor = Color.White
                Case 1
                    btnG2Slot2.BackColor = Color.Black
                    btnG2Slot2.ForeColor = Color.White
                Case 2
                    btnG2Slot3.BackColor = Color.Black
                    btnG2Slot3.ForeColor = Color.White
                Case 3
                    btnG2Slot4.BackColor = Color.Black
                    btnG2Slot4.ForeColor = Color.White
                Case 4
                    btnG2Slot5.BackColor = Color.Black
                    btnG2Slot5.ForeColor = Color.White
                Case 5
                    btnG2Slot6.BackColor = Color.Black
                    btnG2Slot6.ForeColor = Color.White
                Case 6
                    btnG2Slot7.BackColor = Color.Black
                    btnG2Slot7.ForeColor = Color.White
                Case 7
                    btnG2Slot8.BackColor = Color.Black
                    btnG2Slot8.ForeColor = Color.White
                Case 8
                    btnG2Slot9.BackColor = Color.Black
                    btnG2Slot9.ForeColor = Color.White
                Case 9
                    btnG2Slot10.BackColor = Color.Black
                    btnG2Slot10.ForeColor = Color.White
            End Select

            Dim bytes = My.Computer.FileSystem.ReadAllBytes("MASTER.BIN")

            bytes(8 + 16 * Val(slotnum)) = 0
            bytes(9 + 16 * Val(slotnum)) = 0
            bytes(10 + 16 * Val(slotnum)) = 0
            bytes(11 + 16 * Val(slotnum)) = 0

            bytes(13 + 16 * Val(slotnum)) = 1

            oFileStream = New System.IO.FileStream("MASTER.BIN", System.IO.FileMode.Create)
            oFileStream.Write(bytes, 0, bytes.Length)
            oFileStream.Close()

        End If
    End Sub

    Private Sub btnG2UrnHelp_Click(sender As System.Object, e As System.EventArgs) Handles btnG2UrnHelp.Click
        MsgBox("Gaia = 10x Red Orbs" & vbNewLine & "Gorgons = Weapons Petrify Enemies" & vbNewLine & _
               "Poseidon = Poseidon's Rage" & vbNewLine & "Olympus = Infinite Magic" & vbNewLine & _
               "Prometheus = Infinite Rage" & vbNewLine & "Fates = Extend Combo Time")
    End Sub

    Private Sub chkG2UrnOlympusActive_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnOlympusActive.CheckedChanged
        modified = True
        If chkG2UrnOlympusActive.Checked = True Then chkG2InfiniteMagic.Checked = True Else chkG2InfiniteMagic.Checked = False
    End Sub

    Private Sub chkG2UrnPrometheusActive_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnPrometheusActive.CheckedChanged
        modified = True
        If chkG2UrnPrometheusActive.Checked = True Then chkG2InfiniteRage.Checked = True Else chkG2InfiniteRage.Checked = False
    End Sub

    Private Sub chkG2InfiniteMagic_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2InfiniteMagic.CheckedChanged
        modified = True
        If chkG2InfiniteMagic.Checked = True Then chkG2UrnOlympusActive.Checked = True Else chkG2UrnOlympusActive.Checked = False
    End Sub

    Private Sub chkG2InfiniteRage_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2InfiniteRage.CheckedChanged
        modified = True
        If chkG2InfiniteRage.Checked = True Then chkG2UrnPrometheusActive.Checked = True Else chkG2UrnPrometheusActive.Checked = False
    End Sub

    Private Sub chkG2InfiniteHealth_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2InfiniteHealth.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2MagicRegen_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2MagicRegen.TextChanged
        modified = True
    End Sub

    Private Sub txtG2Item_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2Item.TextChanged
        modified = True
        numRangeCheck(txtG2Item, 0, 200)
    End Sub

    Private Sub txtG2ItemExt_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2ItemExt.TextChanged
        modified = True
        numRangeCheck(txtG2Item, 0, 5)
    End Sub

    Private Sub chkG2Bonus_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2Bonus.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2BoOUnl_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2BoOUnl.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG2BoOSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2BoOSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2RageAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2RageAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2SoDUnl_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2SoDUnl.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG2SoDSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2SoDSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2AmuletAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2AmuletAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2BHUnl_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2BHUnl.CheckedChanged
        modified = True
    End Sub

    Private Sub rdbG2BHSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2BHSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2FleeceAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2FleeceAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2BoAlvl_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2BoAlvl.TextChanged
        modified = True
        numRangeCheck(txtG2BoAlvl, 1, 6)
    End Sub

    Private Sub chkG2WingsAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2WingsAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2CRAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2CRAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2CRlvl_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2CRlvl.TextChanged
        modified = True
        numRangeCheck(txtG2CRlvl, 1, 3)
    End Sub

    Private Sub rdbG2CRSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2CRSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnGaiaAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnGaiaAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnGaiaActive_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnGaiaActive.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2PRAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2PRAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2PRlvl_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2PRlvl.TextChanged
        modified = True
        numRangeCheck(txtG2PRlvl, 1, 3)
    End Sub

    Private Sub rdbG2PRSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2PRSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnGorgonAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnGorgonAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnGorgonsActive_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnGorgonsActive.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2EHAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2EHAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2EHlvl_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2EHlvl.TextChanged
        modified = True
        numRangeCheck(txtG2EHlvl, 1, 3)
    End Sub

    Private Sub rdbG2EHSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2EHSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnPoseidonAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnPoseidonAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnPoseidonActive_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnPoseidonActive.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2TBAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2TBAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2TBlvl_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2TBlvl.TextChanged
        modified = True
        numRangeCheck(txtG2TBlvl, 1, 3)
    End Sub

    Private Sub rdbG2TBSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2TBSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnOlympusAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnOlympusAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2AQAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2AQAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2AQlvl_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2AQlvl.TextChanged
        modified = True
        numRangeCheck(txtG2AQlvl, 1, 3)
    End Sub

    Private Sub rdbG2AQSel_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles rdbG2AQSel.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnPrometheusAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnPrometheusAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnFatesAcq_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnFatesAcq.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UrnFatesActive_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UrnFatesActive.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UnkUnl1_02_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UnkUnl1_02.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UnkUnl2_01_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UnkUnl2_01.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UnkUnl2_08_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UnkUnl2_08.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UnkUnl2_10_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UnkUnl2_10.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UnkUnl3_08_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UnkUnl3_08.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2UnkUnl3_80_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2UnkUnl3_80.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2ForcedSubweapon_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2ForcedSubweapon.CheckedChanged
        modified = True
    End Sub

    Private Sub chkG2Frozen_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkG2Frozen.CheckedChanged
        modified = True
    End Sub

    Private Sub txtG2TimesSaved_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtG2TimesSaved.TextChanged
        modified = True
    End Sub

    Private Sub GoW2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
