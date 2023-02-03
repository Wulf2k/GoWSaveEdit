Imports GoWEditor.PS3FileSystem
Imports GoWEditor.GoWFuncs

Public Class GoWA
    Public Shared bytes() As Byte
    Public Shared filename
    Public Shared slotnum = "t"
    Public Shared modified = False
    Public Shared cllGAch() As Button
    Public Shared cllGAtxt() As TextBox
    Public Shared cllGAcmb() As ComboBox
    Public Shared cllGArdbDiff() As RadioButton

    Public Shared cllGAWepLvl() As TextBox
    Public Shared cllGAWepOrbs() As TextBox
    Public Shared cllGAWepUnl() As CheckBox
    Public Shared cllGAWepSel() As CheckBox

    Public Shared cllGAEleUnl() As CheckBox
    Public Shared cllEleSel() As CheckBox

    Public Shared cllWeps() As Byte = {&H0, &H4, &H5, &H6, &H7, &H91, &H93, &H95}
    Public Shared cllSubwep() As Byte = {&H1B, &H1C, &H1D, &H1E, &H20, &H21, &H22, &H24, &H25, &H26, &H28, &H29, &H2A, &H2C, &H2D, &H2E}
    Public Shared cllSubwepBase() As Byte = {&H1F, &H23, &H27, &H2B, &H2F}
    Public Shared cllTorso() As Byte = {&H3C, &H3D, &H40, &H42, &H44, &H46, &H48, &H4A, &H4C, &H4E, &H50}
    Public Shared cllPants() As Byte = {&H3E, &H3F, &H41, &H43, &H45, &H47, &H49, &H4B, &H4D, &H4F, &H51}

    Public Shared kptr As UInt16
    Public Shared kptr2 As UInt16
    Public Shared invcount As Integer



    Private Sub btnGABrowse_Click(sender As Object, e As EventArgs) Handles btnGABrowse.Click
        Dim openDlg As New OpenFileDialog()
        openDlg.Filter = "GoWA Save File|DATFEST"
        openDlg.Title = "Open your PROFILE save file"

        If openDlg.ShowDialog() = Windows.Forms.DialogResult.OK Then txtGAFolder.Text = Microsoft.VisualBasic.Left(openDlg.FileName, openDlg.FileName.Length - 7)
    End Sub
    Private Sub GoWA_close(sender As System.Object, e As System.EventArgs) Handles MyBase.FormClosed
        Main.Close()
    End Sub
    Private Sub btnGAOpen_Click(sender As Object, e As EventArgs) Handles btnGAOpen.Click
        slotnum = "1"
        GAopenSave()
    End Sub
    Private Sub txtGAFile_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtGAFolder.TextChanged
        folder = UCase(txtGAFolder.Text)
    End Sub
    Private Sub ClearValues()

        For i = 0 To cllGAtxt.Length - 1
            cllGAtxt(i).Text = ""
        Next

        For i = 0 To cllGAcmb.Length - 1
            cllGAcmb(i).SelectedIndex = 0
        Next

        chkSubwepGlitch.Checked = False
        chkSubwepGlitch.Enabled = False
        chkGASwim.Checked = False
        chkGAFrozen.Checked = False
        chkGANGPlus.Checked = False

        For i = 0 To cllGAWepLvl.Length - 1
            cllGAWepLvl(i).Text = 1
            cllGAWepOrbs(i).Text = 0
            cllGAWepUnl(i).Checked = False
            If i < 5 Then cllGAWepSel(i).Checked = False
        Next

        For i = 0 To clbInventory.Items.Count - 1
            clbInventory.SetItemChecked(i, False)
        Next
    End Sub
    Private Sub GAopenSave()
        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            Dim idx

            bigendian = True

            REM Try
            btnGABrowse.Visible = False
            btnGAOpen.Visible = False

            For Each ctrl In cllGAch
                ctrl.Visible = True
                ctrl.BackColor = Color.LightGray
                ctrl.ForeColor = Color.Black
            Next

            If Val(slotnum) > 0 Then
                cllGAch(Val(slotnum) - 1).BackColor = Color.Black
                cllGAch(Val(slotnum) - 1).ForeColor = Color.White
            End If

            tctlGAData.Visible = True




            filename = "DAT-" + slotnum
            If IO.File.Exists(folder + "\PARAM.SFD") Then
                encrypted = True
                manager = New Ps3SaveManager(txtGAFolder.Text, SecureID)
            Else
                encrypted = False
            End If
            bytes = FileToBytes(filename)

            ClearValues()

            If bytes(0) <> 192 Then
                MsgBox("This file's opening bytes are not GoW-standard.  Decryption has failed or save slot is empty.")
                Exit Sub
            Else
                txtGAFolder.Enabled = False
                btnGABrowse.Visible = False
                btnGAOpen.Visible = False
            End If

            Try
                kptr = IndexOf(bytes, {0, 0, 3, &H24})
            Catch ex As Exception
                MsgBox("Failed to locate kptr in the save file.  Your computer will now explode." & vbCr & ex.Message)
            End Try

            Try
                kptr2 = IndexOf(bytes, {&H4, &HFF, &H0, &H0}) - 4
            Catch ex As Exception
                MsgBox("Failed to locate kptr2 in the save file.  The end times are upon us.  Secure food and shelter before the zombies arrive." & vbCr & ex.Message)
            End Try


            txtGAHealth.Text = RSingle(bytes, kptr + &HC)
            txtGAHealthRegen.Text = RSingle(bytes, kptr + &H10)
            txtGAMagic.Text = RSingle(bytes, kptr + &H14)
            txtGAMagicRegen.Text = RSingle(bytes, kptr + &H18)
            txtGARage.Text = RSingle(bytes, kptr + &H24)
            txtGARageRegen.Text = RSingle(bytes, kptr + &H28)
            txtGARedOrbs.Text = RSingle(bytes, kptr + &H30)


            chkGASwim.Checked = (bytes(kptr + &H5E) = &H50)

            txtGAXpos.Text = Math.Round(RSingle(bytes, kptr + &H38), 5)
            txtGAHeight.Text = Math.Round(RSingle(bytes, kptr + &H3C), 5)
            txtGAYPos.Text = Math.Round(RSingle(bytes, kptr + &H40), 5)


            chkGAFrozen.Checked = (bytes(kptr + &HCC) = 1)

            invcount = bytes(kptr + &H2D7)

            For i = 0 To invcount - 1
                AddToInv(kptr + &HD7 + &H10 * i)
            Next i

            idx = cmbSubwep.SelectedIndex
            If idx > 1 Then
                idx = Math.Floor((idx - 2) / 3) * 4 + 31
                chkSubwepGlitch.Checked = Not clbInventory.GetItemChecked(idx)
            End If

            txtGAGorgonEyes.Text = bytes(kptr + &H303)
            txtGAPhoenixFeathers.Text = bytes(kptr + &H307)
            txtSubwepAmmo.Text = bytes(kptr + &H317)


            cllGArdbDiff(bytes(kptr2 + &H17)).Checked = True
            chkGANGPlus.Checked = bytes(kptr2 + &H1B)



            txtGARedOrbs.Text = Val(txtGARedOrbs.Text).ToString("n0")
            For i = 0 To cllGAWepOrbs.Length - 1
                cllGAWepOrbs(i).Text = Val(cllGAWepOrbs(i).Text).ToString("n0")
            Next


            modified = False

            Try
            Catch ex As Exception
                MsgBox("Failed to Open." & vbCr & ex.Message)
            End Try


        End If
    End Sub
    Private Sub AddToInv(loc As ULong)
        Dim id As Byte() = {bytes(loc)}
        Dim item As Int16

        clbInventory.SetItemChecked(id(0), True)

        item = IndexOf(cllWeps, id)
        If Not item = -1 Then
            cllGAWepLvl(item).Text = bytes(loc + &H4) + 1
            cllGAWepUnl(item).Checked = (bytes(loc + &H5))
            If item < 5 Then cllGAWepSel(item).Checked = (bytes(loc + &H6))
            cllGAWepOrbs(item).Text = RInt32(bytes, loc + &H9)
        End If


        item = IndexOf(cllSubwep, id)
        If Not item = -1 Then
            cmbSubwep.SelectedIndex = item + 1
            If item > 1 Then
                chkSubwepGlitch.Enabled = True
            Else
                chkSubwepGlitch.Enabled = False
            End If
        End If


        item = IndexOf(cllTorso, id)
        If Not item = -1 Then
            cmbTorso.SelectedIndex = item + 1
        End If


        item = IndexOf(cllPants, id)
        If Not item = -1 Then
            cmbPants.SelectedIndex = item + 1
        End If
    End Sub
    Public Shared Function IndexOf(arrayToSearchThrough As Byte(), patternToFind As Byte()) As Integer
        If patternToFind.Length > arrayToSearchThrough.Length Then
            Return -1
        End If

        For i As Integer = 0 To arrayToSearchThrough.Length - patternToFind.Length
            Dim found As Boolean = True
            For j As Integer = 0 To patternToFind.Length - 1
                If arrayToSearchThrough(i + j) <> patternToFind(j) Then
                    found = False
                    Exit For
                End If
            Next
            If found Then
                Return i
            End If
        Next
        Return -1
    End Function
    Private Sub btnGACh_Click(sender As Object, e As EventArgs)

        If modified = True Then
            If MsgBox("Save has been modified.  Lose unsaved changes?", vbYesNo) = vbYes Then modified = False
        End If

        If modified = False Then
            slotnum = Convert.ToString(Array.IndexOf(cllGAch, sender) + 1)
            GAopenSave()
        End If
    End Sub
    Private Sub chkEleSel_Click(sender As Object, e As EventArgs)
        For i = 0 To cllEleSel.Length - 1
            If i <> Array.IndexOf(cllEleSel, sender) Then cllEleSel(i).Checked = False
        Next
    End Sub
    Private Sub chkEleUnl_Click(sender As Object, e As EventArgs)
        Dim StartEle = True
        For i = 0 To cllGAEleUnl.Length - 1
            If cllGAEleUnl(i).Checked = True Then StartEle = False
        Next
        clbInventory.SetItemChecked(9, StartEle)
    End Sub
    Private Sub chkWepUnl_click(sender As Object, e As EventArgs)

        Dim idx = Array.IndexOf(cllGAWepUnl, sender)
        clbInventory.SetItemChecked(cllWeps(idx), cllGAWepUnl(idx).Checked)

        If idx < 5 Then
            If Not cllGAWepUnl(idx).Checked Then cllGAWepSel(idx).Checked = False
            If idx > 0 Then
                clbInventory.SetItemChecked(idx + 8 + (idx = 1), cllGAWepUnl(idx).Checked)
            End If
        End If

    End Sub
    Private Sub btnGASave_Click(sender As Object, e As EventArgs) Handles btnGASave.Click



        modified = False


        WSingle(bytes, kptr + &HC, txtGAHealth.Text)
        WSingle(bytes, kptr + &H10, txtGAHealthRegen.Text)
        WSingle(bytes, kptr + &H14, txtGAMagic.Text)
        WSingle(bytes, kptr + &H18, txtGAMagicRegen.Text)
        WSingle(bytes, kptr + &H24, txtGARage.Text)
        WSingle(bytes, kptr + &H28, txtGARageRegen.Text)
        WSingle(bytes, kptr + &H30, txtGARedOrbs.Text)

        WSingle(bytes, kptr + &H38, txtGAXpos.Text)
        WSingle(bytes, kptr + &H3C, txtGAHeight.Text)
        WSingle(bytes, kptr + &H40, txtGAYPos.Text)

        bytes(kptr + &H5E) = &H8 + (&H48 * chkGASwim.Checked * -1)
        bytes(kptr + &HCC) = chkGAFrozen.Checked * -1

        Dim j
        Dim NoEleSel = True
        invcount = 0
        For Each i In clbInventory.CheckedIndices
            bytes(kptr + invcount * 16 + &HD7) = i  'Item ID
            bytes(kptr + invcount * 16 + &HDC) = 1  'Enabled
            bytes(kptr + invcount * 16 + &HDD) = 1  'Selected by default

            j = Array.IndexOf({0, 4, 5, 6, 7, &H91, &H93, &H95}, i)
            If j > -1 Then   'Deal with elements
                bytes(kptr + invcount * 16 + &HDB) = cllGAWepLvl(j).Text - 1  'Item Level
                If j < 5 Then
                    bytes(kptr + invcount * 16 + &HDD) = cllGAWepSel(j).Checked * -1  'Use weapon checkboxes to see if selected
                Else
                    bytes(kptr + invcount * 16 + &HDD) = 0  'Don't select eyes/stone/amulet
                End If
                bytes(kptr + invcount * 16 + &HE2) = cllGAWepOrbs(j).Text \ 256  'Orbs spent, byte 1
                bytes(kptr + invcount * 16 + &HE3) = cllGAWepOrbs(j).Text Mod 256  'Orbs spent, byte 2
            End If

            j = Array.IndexOf({8, &HA, &HB, &HC}, i)
            If j > -1 Then   'Select appropriate rage to match selected element
                If cllGAWepSel(j + 1).Checked = False Then
                    bytes(kptr + invcount * 16 + &HDD) = 0
                Else
                    NoEleSel = False
                End If
            End If

            If i = 9 Then   'If Non-elemental rage is present, unselect it if other elements are selected

                If Not NoEleSel Then
                    bytes(kptr + invcount * 16 + &HDD) = 0
                End If
            End If

            invcount += 1
        Next



        bytes(kptr + &H2D7) = clbInventory.CheckedIndices.Count


        bytes(kptr + &H303) = txtGAGorgonEyes.Text
        bytes(kptr + &H307) = txtGAPhoenixFeathers.Text
        bytes(kptr + &H317) = txtSubwepAmmo.Text


        For i = 0 To 3
            If cllGArdbDiff(i).Checked = True Then bytes(kptr2 + &H17) = i
        Next
        bytes(kptr2 + &H1B) = chkGANGPlus.Checked * -1



        BytesToFile(filename, bytes)

        MsgBox("Save Completed")
    End Sub
    Private Sub GoWA_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        cllGAch = {btnGACh1, btnGACh2, btnGACh3, btnGACh4, btnGACh5, btnGACh6, btnGACh7, btnGACh8, btnGACh9, btnGACh10, btnGACh11, btnGACh12, btnGACh13, _
                        btnGACh14, btnGACh15, btnGACh16, btnGACh17, btnGACh18, btnGACh19, btnGACh20, btnGACh21, btnGACh22, btnGACh23, btnGACh24, btnGACh25, _
                        btnGACh26, btnGACh27, btnGACh28, btnGACh29, btnGACh30, btnGACh31}
        cllGArdbDiff = {rdbGAEasy, rdbGANormal, rdbGAHard, rdbGAVeryHard}
        cllGAtxt = {txtGAGorgonEyes, txtGAHealth, txtGAHealthRegen, txtGAHeight, txtGAMagic, txtGAMagicRegen, txtGAPhoenixFeathers, _
                    txtGARage, txtGARageRegen, txtGARedOrbs, txtGAXpos, txtGAYPos, txtSubwepAmmo}
        cllGAcmb = {cmbSubwep, cmbTorso, cmbPants}

        cllGAWepLvl = {txtBladesLvl, txtElecLvl, txtFireLvl, txtIceLvl, txtSoulLvl, txtAmuletLvl, txtOathLvl, txtEyesLvl}
        cllGAWepOrbs = {txtBladesOrbs, txtElecOrbs, txtFireOrbs, txtIceOrbs, txtSoulOrbs, txtAmuletOrbs, txtOathOrbs, txtEyesOrbs}
        cllGAWepUnl = {chkBladesUnl, chkElecUnl, chkFireUnl, chkIceUnl, chkSoulUnl, chkAmuletUnl, chkOathUnl, chkEyesUnl}
        cllGAWepSel = {chkBladesSel, chkElecSel, chkFireSel, chkIceSel, chkSoulSel}

        cllGAEleUnl = {chkElecUnl, chkFireUnl, chkIceUnl, chkSoulUnl}
        cllEleSel = {chkElecSel, chkFireSel, chkIceSel, chkSoulSel}

        For i = 0 To cllGAch.Length - 1
            AddHandler cllGAch(i).Click, AddressOf btnGACh_Click
        Next

        For i = 0 To cllGAEleUnl.Length - 1
            AddHandler cllGAEleUnl(i).Click, AddressOf chkEleUnl_Click
        Next

        For i = 0 To cllEleSel.Length - 1
            AddHandler cllEleSel(i).Click, AddressOf chkEleSel_Click
        Next

        For i = 0 To cllGAWepUnl.Length - 1
            AddHandler cllGAWepUnl(i).Click, AddressOf chkWepUnl_Click
        Next
    End Sub
    Private Sub cmbSubwep_Changed(sender As Object, e As EventArgs) Handles cmbSubwep.SelectionChangeCommitted

        REM  Fix up this convoluted piece of crap.

        Dim idx = cmbSubwep.SelectedIndex

        For i = 0 To 20
            clbInventory.SetItemChecked(&H1B + i, False)
        Next

        If idx = 1 Then clbInventory.SetItemChecked(&H1B, True)
        If idx < 2 Then
            chkSubwepGlitch.Enabled = False
            chkSubwepGlitch.Checked = False
        End If


        If idx > 1 Then
            chkSubwepGlitch.Enabled = True
            chkSubwepGlitch.Checked = False
            idx = idx - 1
            clbInventory.SetItemChecked(&H1B + idx + Math.Floor((idx - 1) / 3), True)
            clbInventory.SetItemChecked(Math.Floor((idx - 1) / 3) * 4 + 31, True)
        End If

    End Sub
    Private Sub cmbTorso_Changed(sender As Object, e As EventArgs) Handles cmbTorso.SelectionChangeCommitted
        For i = 1 To cllTorso.Length
            clbInventory.SetItemChecked(cllTorso(i - 1), Not (i <> cmbTorso.SelectedIndex))
        Next
    End Sub
    Private Sub cmbPants_ChangedChanged(sender As Object, e As EventArgs) Handles cmbPants.SelectionChangeCommitted
        For i = 1 To cllPants.Length
            clbInventory.SetItemChecked(cllPants(i - 1), Not (i <> cmbPants.SelectedIndex))
        Next
    End Sub
    Private Sub chkSubwepGlitch_CheckedChanged(sender As Object, e As EventArgs) Handles chkSubwepGlitch.CheckedChanged

        Dim idx As Int32
        Dim wepgrp As Int32

        idx = cmbSubwep.SelectedIndex - 2 'Weapon groups start at idx 2
        wepgrp = Math.Floor(idx / 3) 'Weapon groups are listed in the pulldown in groups of three

        idx = idx - wepgrp * 3 'Are we subwep level 1/2/3?
        idx = idx + wepgrp * 4 'Pad for subwepBase
        idx = idx + 28 'pad for start of weapon values


        clbInventory.SetItemChecked(idx, Not chkSubwepGlitch.Checked)

    End Sub
End Class