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


        Else
            tabUser.Enabled = False
            tabSave.Enabled = True
            tabSaves.SelectedTab = tabSave


        End If


    End Sub

    Private Sub btnG3Save_Click(sender As Object, e As EventArgs) Handles btnG3Save.Click

    End Sub

    Private Sub btnG3Browse_Click(sender As Object, e As EventArgs) Handles btnG3Browse.Click

    End Sub


End Class