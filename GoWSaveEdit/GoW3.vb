﻿Imports GoWEditor.PS3FileSystem

Public Class GoW3

    Dim DecSave(147455) As Byte
    Dim SecureID(15) As Byte
    Dim manager As Ps3SaveManager
    Dim file As Ps3File




    Private Sub btnG3Open_Click(sender As Object, e As EventArgs) Handles btnG3Open.Click

        SecureID = {&HD6, &H48, &H5E, &H21, &HCF, &HB9, &H7, &H85, &H44, &HFB, &H1, &H83, &HE8, &H23, &H92, &H3E}

        manager = New Ps3SaveManager(txtG3File.Text, SecureID)

        file = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = "SAVEDATA")
        If Not file Is Nothing Then
            DecSave = file.DecryptToBytes


        End If

    End Sub
End Class