Imports GoWEditor.PS3FileSystem
Imports GoWEditor.GoWFuncs

Public Class GoW3

    Dim DecSave(147455) As Byte
    Dim SecureID(15) As Byte
    Dim manager As Ps3SaveManager
    Dim file As Ps3File




    Private Sub btnG3Open_Click(sender As Object, e As EventArgs) Handles btnG3Open.Click

        SecureID = GoW3SecureID

        manager = New Ps3SaveManager(txtG3File.Text, SecureID)

        file = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = "SAVEDATA")
        If Not file Is Nothing Then
            DecSave = file.DecryptToBytes


        End If

    End Sub

    Private Sub btnG3Save_Click(sender As Object, e As EventArgs) Handles btnG3Save.Click

    End Sub

    Private Sub btnG3Browse_Click(sender As Object, e As EventArgs) Handles btnG3Browse.Click

    End Sub
End Class