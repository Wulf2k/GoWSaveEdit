Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Windows.Forms

Namespace PS3FileSystem
    Friend Module Functions
        Sub New()
        End Sub
        Public ReadOnly Property StaticKeys() As String()
            Get
                Return {"syscon_manager_key=D413B89663E1FE9F75143D3BB4565274", "keygen_key=6B1ACEA246B745FD8F93763B920594CD53483B82", "savegame_param_sfo_key=0C08000E090504040D010F000406020209060D03", "trophy_param_sfo_key=5D5B647917024E9BB8D330486B996E795D7F4392", "tropsys_dat_key=B080C40FF358643689281736A6BF15892CFEA436", "tropusr_dat_key=8711EFF406913F0937F115FAB23DE1A9897A789A", _
                    "troptrns_dat_key=91EE81555ACC1C4FB5AAE5462CFE1C62A4AF36A5", "tropconf_sfm_key=E2ED33C71C444EEBC1E23D635AD8E82F4ECA4E94", "fallback_disc_hash_key=D1C1E10B9C547E689B805DCD9710CE8D"}
            End Get
        End Property


        <System.Runtime.CompilerServices.Extension> _
        Public Function SwapByteOrder(value As UInt16) As UInt16
            Return CType((value And &HFFUI) << 8 Or (value And &HFF00UI) >> 8, UInt16)
        End Function

        <System.Runtime.CompilerServices.Extension> _
        Public Function SwapByteOrder(value As UInt32) As UInt32
            Return (value And &HFFUI) << 24 Or (value And &HFF00UI) << 8 Or (value And &HFF0000UI) >> 8 Or (value And &HFF000000UI) >> 24
        End Function

        <System.Runtime.CompilerServices.Extension> _
        Public Function SwapByteOrder(value As UInt64) As UInt64

            Return ((value And &HFF00000000000000UL) >> 56) Or ((value And &HFF000000000000UL) >> 40) Or ((value And &HFF0000000000UL) >> 24) Or ((value And &HFF00000000UL) >> 8) Or ((value And &HFF000000UL) << 8) Or ((value And &HFF0000UL) << 24) Or ((value And &HFF00UL) << 40) Or ((value And &HFFUL) << 56)
        End Function

        Public Function CompareBytes(a As Byte(), b As Byte()) As Boolean
            If a.Length <> b.Length Then
                Return False
            End If
            For i As Integer = 0 To a.Length - 1
                If a(i) <> b(i) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Function DecryptWithPortability(iv As Byte(), data As Byte(), data_size As Integer) As Byte()
            Dim x = New AesCryptoServiceProvider()
            x.Mode = CipherMode.CBC
            x.Padding = PaddingMode.Zeros
            Dim key As Byte() = GetStaticKey("syscon_manager_key")
            If iv.Length <> 16 Then
                Array.Resize(iv, 16)
            End If
            Return x.CreateDecryptor(key, iv).TransformFinalBlock(data, 0, data_size)
        End Function

        Public Function EncryptWithPortability(iv As Byte(), data As Byte(), data_size As Integer) As Byte()
            Dim x = New AesCryptoServiceProvider()
            x.Mode = CipherMode.CBC
            x.Padding = PaddingMode.Zeros
            Dim key As Byte() = GetStaticKey("syscon_manager_key")
            If iv.Length <> 16 Then
                Array.Resize(iv, 16)
            End If
            Return x.CreateEncryptor(key, iv).TransformFinalBlock(data, 0, data_size)
        End Function

        <System.Runtime.CompilerServices.Extension> _
        Public Function StringToByteArray(hex As String) As Byte()
            If (hex.Length Mod 2) <> 0 Then
                hex = hex.PadLeft(hex.Length + 1, "0"c)
            End If
            Return Enumerable.Range(0, hex.Length).Where(Function(x) x Mod 2 = 0).[Select](Function(x) Convert.ToByte(hex.Substring(x, 2), 16)).ToArray()
        End Function

        Public Function GetStaticKey(name As String) As Byte()
            For Each line As String In StaticKeys
                Dim x As String = line.Split("="c)(0)
                If x.ToLower() = name.ToLower() Then
                    Dim value As String = line.Split("="c)(1)
                    Return StringToByteArray(value)
                End If
            Next
            Return Nothing
        End Function

        Private Function xDownloadAldosGameConfig() As SecureFileInfo()
            Try
                Dim text As String = New WebClient().DownloadString("http://ps3tools.aldostools.org/games.conf")
                If text Is Nothing OrElse text.Length < 100 Then
                    Return New SecureFileInfo() {}
                End If
                Return ReadConfigFromtext(text)
            Catch
                Return New SecureFileInfo() {}
            End Try
        End Function

        Public Function ReadConfigFromtext(inputtext As String) As SecureFileInfo()
            Dim files = New List(Of SecureFileInfo)()
            Using sr = New StringReader(inputtext)
                Dim line As String = ""
                Dim s As String = InlineAssignHelper(line, sr.ReadLine())
                While s IsNot Nothing AndAlso (sr.Peek() > -1 AndAlso Not s.Equals("; -- UNPROTECTED GAMES --"))
                    Application.DoEvents()
                End While

                Dim s1 As String = InlineAssignHelper(line, sr.ReadLine())
                While s1 IsNot Nothing AndAlso (sr.Peek() > -1 AndAlso s1.StartsWith(";"))
                    files.Add(New SecureFileInfo(line.Replace(";", ""), "", "", "", False))
                End While

                While sr.Peek() > -1
                    Dim s2 As String = InlineAssignHelper(line, sr.ReadLine())
                    While s2 IsNot Nothing AndAlso (sr.Peek() > -1 AndAlso s2.StartsWith(";"))
                        If line IsNot Nothing Then
                            Dim name As String = line.Replace(";", "")
                            Dim s3 As String = InlineAssignHelper(line, sr.ReadLine())
                            If s3 IsNot Nothing AndAlso s3.StartsWith("[") Then
                                Dim id As String = line
                                Dim readLine As String = sr.ReadLine()
                                If readLine IsNot Nothing Then
                                    Dim diskhashkey As String = readLine.Split("="c)(1)
                                    Dim secureid As String = readLine.Split("="c)(1)
                                    files.Add(New SecureFileInfo(name, id, secureid, diskhashkey, (Not String.IsNullOrEmpty(secureid) AndAlso secureid.Length = 32)))
                                End If
                            End If
                        End If
                    End While
                End While
                sr.Close()
            End Using
            Return files.ToArray()
        End Function

        Public Function DownloadAldosGameConfig() As SecureFileInfo()

            Return xDownloadAldosGameConfig()
        End Function

        Public Function GetHMACSHA1(key As Byte(), data As Byte(), start As Integer, length As Integer) As Byte()
            Return New HMACSHA1(key).ComputeHash(data, start, length)
        End Function

        Public Function CalculateFileHMACSha1(file As String, key As Byte()) As Byte()
            Dim hash As Byte()
            Using fs = New FileStream(file, FileMode.Open)
                hash = New HMACSHA1(key).ComputeHash(fs)
                fs.Close()
            End Using
            Return hash
        End Function

        Public Function CalculateFileHMACSha1(input As Stream, key As Byte()) As Byte()
            Return New HMACSHA1(key).ComputeHash(input)
        End Function

        Public Function Decrypt(key As Byte(), input As Byte(), length As Integer) As Byte()
            Array.Resize(key, 16)
            Dim x1 As Aes = Aes.Create()
            x1.Key = key
            x1.BlockSize = 128
            x1.Mode = CipherMode.ECB
            x1.Padding = PaddingMode.Zeros
            Dim x2 As Aes = Aes.Create()
            x2.Key = key
            x1.BlockSize = 128
            x2.Mode = CipherMode.ECB
            x2.Padding = PaddingMode.Zeros
            Dim nums As Integer = (length \ 16)
            Dim output = New Byte(length - 1) {}
            For i As Integer = 0 To nums - 1
                Dim blockdata = New Byte(15) {}
                Array.Copy(input, (i * 16), blockdata, 0, 16)
                Dim offset As Integer = (i * 16)
                Dim buffer = New Byte(15) {}
                Array.Copy(BitConverter.GetBytes(SwapByteOrder(CULng(i))), 0, buffer, 0, 8)
                buffer = x1.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length)
                blockdata = x2.CreateDecryptor().TransformFinalBlock(blockdata, 0, blockdata.Length)
                For j As Integer = 0 To 15
                    blockdata(j) = blockdata(j) Xor buffer(j)
                Next
                Array.Copy(blockdata, 0, output, (i * 16), 16)
            Next
            Return output
        End Function

        Public Function Encrypt(key As Byte(), input As Byte(), length As Integer) As Byte()
            Array.Resize(key, 16)
            Dim x1 As Aes = Aes.Create()
            x1.Key = key
            x1.BlockSize = 128
            x1.Mode = CipherMode.ECB
            x1.Padding = PaddingMode.Zeros
            Dim x2 As Aes = Aes.Create()
            x2.Key = key
            x1.BlockSize = 128
            x2.Mode = CipherMode.ECB
            x2.Padding = PaddingMode.Zeros
            Dim nums As Integer = (length \ 16)
            Dim output = New Byte(length - 1) {}
            For i As Integer = 0 To nums - 1
                Dim blockdata = New Byte(15) {}
                Array.Copy(input, (i * 16), blockdata, 0, 16)
                Dim offset As Integer = (i * 16)
                Dim buffer = New Byte(15) {}
                Array.Copy(BitConverter.GetBytes(SwapByteOrder(CULng(i))), 0, buffer, 0, 8)
                buffer = x1.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length)
                For j As Integer = 0 To 15
                    blockdata(j) = blockdata(j) Xor buffer(j)
                Next
                blockdata = x2.CreateEncryptor().TransformFinalBlock(blockdata, 0, blockdata.Length)
                Array.Copy(blockdata, 0, output, (i * 16), 16)
            Next
            Return output
        End Function
        Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Module
End Namespace