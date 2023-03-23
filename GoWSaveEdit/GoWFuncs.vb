Imports GoWEditor.PS3FileSystem

Public Class GoWFuncs
    Public Shared bigendian As Boolean = True
    Public Shared encrypted As Boolean = False
    Public Shared manager As Ps3SaveManager
    Public Shared file As Ps3File
    Public Shared filename
    Public Shared folder

    Public Shared SecureID() As Byte = {}
    Public Shared GoW1SecureID() As Byte = {&H82, &H21, &H42, &HD2, &H27, &H74, &H97, &H6, &H62, &H25, &H46, &HE6, &HE7, &H20, &H6, &H27}
    Public Shared GoW2SecureID() As Byte = {&H82, &H21, &H42, &HD2, &H27, &H74, &H97, &H6, &H62, &H25, &H46, &HE6, &HE7, &H20, &H6, &H27}
    Public Shared GoW3SecureID() As Byte = {&HD6, &H48, &H5E, &H21, &HCF, &HB9, &H7, &H85, &H44, &HFB, &H1, &H83, &HE8, &H23, &H92, &H3E}
    Public Shared GoWASecureID() As Byte = {&HE1, &H19, &H7F, &H68, &HAC, &HA2, &H3B, &H45, &H9D, &HEC, &H80, &H62, &HFF, &H46, &H19, &H5A}

    Shared Function RInt32(ByRef bytes() As Byte, start As Int32) As Int32
        Dim ba(3) As Byte
        Array.Copy(bytes, start, ba, 0, 4)
        If bigendian Then Array.Reverse(ba)

        Return BitConverter.ToInt32(ba, 0)
    End Function
    Shared Function RSingle(ByRef bytes() As Byte, start As Int32) As Single
        Dim ba(3) As Byte
        Array.Copy(bytes, start, ba, 0, 4)
        If bigendian Then Array.Reverse(ba)

        Return BitConverter.ToSingle(ba, 0)
    End Function
    Shared Function RUInt32(ByRef bytes() As Byte, start As Int32) As UInt32
        Dim ba(3) As Byte
        Array.Copy(bytes, start, ba, 0, 4)
        If bigendian Then Array.Reverse(ba)

        Return BitConverter.ToUInt32(ba, 0)
    End Function

    Shared Sub WInt16(ByRef bytes() As Byte, start As Int32, val As Int16)
        Dim ba(1) As Byte
        ba = BitConverter.GetBytes(val)
        If bigendian Then Array.Reverse(ba)

        Array.Copy(ba, 0, bytes, start, 2)
    End Sub
    Shared Sub WInt32(ByRef bytes() As Byte, start As Int32, val As Int32)
        Dim ba(3) As Byte
        ba = BitConverter.GetBytes(val)
        If bigendian Then Array.Reverse(ba)

        Array.Copy(ba, 0, bytes, start, 4)
    End Sub
    Shared Sub WSingle(ByRef bytes() As Byte, start As Int32, val As Single)
        Dim ba(3) As Byte
        ba = BitConverter.GetBytes(val)
        If bigendian Then Array.Reverse(ba)

        Array.Copy(ba, 0, bytes, start, 4)
    End Sub
    Shared Sub WUInt16(ByRef bytes() As Byte, start As Int32, val As UInt16)
        Dim ba(1) As Byte
        ba = BitConverter.GetBytes(val)
        If bigendian Then Array.Reverse(ba)

        Array.Copy(ba, 0, bytes, start, 2)
    End Sub
    Shared Sub WUInt32(ByRef bytes() As Byte, start As Int32, val As UInt32)
        Dim ba(3) As Byte
        ba = BitConverter.GetBytes(val)
        If bigendian Then Array.Reverse(ba)

        Array.Copy(ba, 0, bytes, start, 4)
    End Sub


    Shared Function FileToBytes(name As String) As Byte()
        If encrypted Then
            file = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = name)
            Return file.DecryptToBytes
        Else
            Return IO.File.ReadAllBytes(folder + "\" + name)
        End If
    End Function
    Shared Sub BytesToFile(name As String, b As Byte())
        If encrypted Then
            Dim f As Ps3File = manager.Files.FirstOrDefault(Function(t) t.PFDEntry.file_name = name)
            f.Encrypt(b)
            manager.ReBuildChanges()
        Else
            IO.File.WriteAllBytes(folder + "\" + name, b)
        End If
    End Sub
End Class
