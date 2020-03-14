Imports System.Collections.Generic
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports GoWEditor.PS3FileSystem.Delegates



Namespace PS3FileSystem
    Public Class Param_PFD
#Region "static variables"

        Friend Shared pfd_istropy As Boolean = False
        Friend Shared pfdheaderkey As Byte() = New Byte(15) {}
        Friend Shared pfdkey As Byte() = New Byte(15) {}
        Friend Shared pfdhashkey As Byte() = New Byte(19) {}
        Friend Shared pfdhash As Byte() = New Byte(19) {}
        Friend Shared entrykey As Byte() = New Byte(63) {}
        Friend Shared realkey As Byte() = New Byte(19) {}
        Friend Shared m_consoleid As Byte() = New Byte(31) {}
        Friend Shared m_userid As Byte() = {0, 0, 0, 0, 0, 0, _
            0, 1}
        Friend Shared m_securefileid As Byte()
        Friend Shared m_authenticationid As Byte() = {&H10, &H10, &H0, &H0, &H1, &H0, _
            &H0, &H3}

        Friend Shared m_dischashkey As Byte() = {&HD1, &HC1, &HE1, &HB, &H9C, &H54, _
            &H7E, &H68, &H9B, &H80, &H5D, &HCD, _
            &H97, &H10, &HCE, &H8D}

#End Region

#Region "internal/Private methods"

        Friend Function GenerateHashkeyForSFO(hashindex As Integer) As Byte()
            If hashindex > 3 Then
                Return Nothing
            End If
            Select Case hashindex
                Case 0
                    Return Functions.GetStaticKey("savegame_param_sfo_key")
                Case 1
                    Return ConsoleID
                Case 2
                    Return DiscHashKey
                Case 3
                    Return AuthenticationID
                Case Else
                    Return Nothing
            End Select
        End Function

        Friend Function GenerateHashKeyForSecureFileID(secureid As Byte()) As Byte()
            If secureid.Length <> 16 Then
                Throw New Exception("SecureFileID must be 16 bytes in length")
            End If
            Dim key = New Byte(19) {}
            Array.Copy(secureid, 0, key, 0, 16)
            Dim i As Integer = 0, j As Integer = 0
            While i < key.Length
                Select Case i
                    Case 1
                        key(i) = 11
                        Exit Select
                    Case 2
                        key(i) = 15
                        Exit Select
                    Case 5
                        key(i) = 14
                        Exit Select
                    Case 8
                        key(i) = 10
                        Exit Select
                    Case Else
                        key(i) = secureid(j)
                        j = j + 1
                        Exit Select
                End Select
                i += 1
            End While
            Return key
        End Function

        Friend Function GetEntryHashKey(entry As PFDEntry, hashindex As Integer) As Byte()
            Select Case entry.file_name.ToLower()
                Case "param.sfo"
                    Return GenerateHashkeyForSFO(hashindex)
                Case "tropsys.dat"
                    Return Functions.GetStaticKey("tropsys_dat_key")
                Case "tropusr.dat"
                    Return Functions.GetStaticKey("tropusr_dat_key")
                Case "troptrns.dat"
                    Return Functions.GetStaticKey("troptrns_dat_key")
                Case "tropconf.sfm"
                    Return Functions.GetStaticKey("tropconf_sfm_key")
                Case Else
                    Return GenerateHashKeyForSecureFileID(SecureFileID)
            End Select
        End Function

        Friend Function GetEntryKey(entry As PFDEntry) As Byte()
            Dim key As Byte() = GetEntryHashKey(entry, 0)
            Return Functions.DecryptWithPortability(key, entry.key, entry.key.Length)
        End Function

        Friend Function GetTopHash() As Byte()
            Dim buffer As Byte() = PFDHashTable.Buffer
            buffer = Functions.GetHMACSHA1(realkey, buffer, 0, buffer.Length)
            Return buffer
        End Function

        Friend Function GetBottomHash() As Byte()
            Dim buffer As Byte() = PFDEntrySignatureTable.Buffer
            buffer = Functions.GetHMACSHA1(realkey, buffer, 0, buffer.Length)
            Return buffer
        End Function

        Friend Function GetDefaultHash() As Byte()
            Return New HMACSHA1(realkey).ComputeHash(New Byte() {})
        End Function

        Friend Function CalculateHashTableEntryIndex(name As String) As ULong
            Dim len As Integer = name.Length
            Dim hash As ULong = 0
            For i As Integer = 0 To len - 1
                hash = (hash << 5) - hash + CByte(AscW(name(i)))
            Next
            Return hash Mod PFDHashTable.capacity


        End Function

        Friend Sub Init(input As Stream)
            DoProgress("Initializing Param.PFD stream..", MessageType.Info)
            Using br = New BinaryReader(input)
                PFDHeader.magic = br.ReadUInt64().SwapByteOrder()
                If PFDHeader.magic <> &H50464442UL Then
                    DoProgress("Invalid PFD File!", MessageType.[Error])
                    Throw New Exception("Invalid PFD File!")
                End If
                PFDHeader.version = br.ReadUInt64().SwapByteOrder()
                If PFDHeader.version <> 3UL AndAlso PFDHeader.version <> 4UL Then
                    DoProgress("Unsupported PFD version!", MessageType.[Error])
                    Throw New Exception("Unsupported PFD version!")
                End If
                DoProgress("Allocating Header Data..", MessageType.Info)
                pfdheaderkey = br.ReadBytes(16)
                Dim header As Byte() = br.ReadBytes(64)
                header = Functions.DecryptWithPortability(pfdheaderkey, header, header.Length)
                PFDSignature.bottom_hash = New Byte(19) {}
                Array.Copy(header, 0, PFDSignature.bottom_hash, 0, 20)
                PFDSignature.top_hash = New Byte(19) {}
                Array.Copy(header, 20, PFDSignature.top_hash, 0, 20)
                PFDSignature.hash_key = New Byte(19) {}
                Array.Copy(header, 40, PFDSignature.hash_key, 0, 20)
                PFDSignature.padding = New Byte(3) {}
                Array.Copy(header, 60, PFDSignature.padding, 0, 4)
                header = Nothing

                If PFDHeader.version = 4UL Then
                    realkey = Functions.GetHMACSHA1(Functions.GetStaticKey("keygen_key"), PFDSignature.hash_key, 0, 20)
                Else
                    realkey = PFDSignature.hash_key
                End If
                DoProgress("Reading Entries..", MessageType.Info)
                PFDHashTable.capacity = br.ReadUInt64().SwapByteOrder()
                PFDHashTable.num_reserved = br.ReadUInt64().SwapByteOrder()
                PFDHashTable.num_used = br.ReadUInt64().SwapByteOrder()
                PFDHashTable.entries = New List(Of ULong)()
                DoProgress("Reading table capicity (" & PFDHashTable.capacity & " entries)..", MessageType.Info)
                For i As ULong = 0 To PFDHashTable.capacity - 1
                    PFDHashTable.entries.Add(br.ReadUInt64().SwapByteOrder())
                Next

                PFDEntries.entries = New List(Of PFDEntry)()
                DoProgress("Reading used tables (" & PFDHashTable.num_used & " entries)..", MessageType.Info)
                For i As ULong = 0 To PFDHashTable.num_used - 1
                    Dim x = New PFDEntry()
                    x.additional_index = br.ReadUInt64().SwapByteOrder()
                    x.file_name = Encoding.ASCII.GetString(br.ReadBytes(65)).Replace(vbNullChar, "")
                    x.__padding_0 = br.ReadBytes(7)
                    x.key = br.ReadBytes(64)
                    x.file_hashes = New List(Of Byte())()
                    For j As Integer = 0 To 3
                        x.file_hashes.Add(br.ReadBytes(20))
                    Next
                    x.__padding_1 = br.ReadBytes(40)
                    x.file_size = br.ReadUInt64().SwapByteOrder()
                    PFDEntries.entries.Add(x)
                Next
                Dim offset = CLng(CULng(br.BaseStream.Position) + (&H110 * (PFDHashTable.num_reserved - PFDHashTable.num_used)))
                br.BaseStream.Position = offset
                PFDEntrySignatureTable.hashes = New List(Of Byte())()
                DoProgress("Reading file table hashes (" & PFDHashTable.capacity & " entries)..", MessageType.Info)
                For i As ULong = 0 To PFDHashTable.capacity - 1
                    PFDEntrySignatureTable.hashes.Add(br.ReadBytes(20))
                Next
                br.Close()
            End Using
        End Sub

        Friend Shared Function AlignedSize(size As Integer) As Integer
            Return (size + 16 - 1) And Not (16 - 1)
        End Function

        Friend Function GetEntryHash(entryindex As Integer) As Byte()
            If entryindex >= PFDEntries.entries.Count Then
                Throw New Exception("entryindex is out of bounds")
            End If
            Dim ent As PFDEntry = PFDEntries.entries(entryindex)
            Dim tableindex As ULong = CalculateHashTableEntryIndex(ent.file_name)
            Dim currententryindex As ULong = PFDHashTable.entries(CInt(tableindex))

            If currententryindex < PFDHashTable.num_reserved Then
                Dim sha1 = New HMACSHA1(realkey)
                Dim hashdata = New List(Of Byte)()
                While currententryindex < PFDHashTable.num_reserved
                    ent = PFDEntries.entries(CInt(currententryindex))
                    hashdata.AddRange(ent.HashData)
                    currententryindex = ent.additional_index
                End While
                sha1.ComputeHash(hashdata.ToArray())
                hashdata.Clear()
                Return sha1.Hash
            End If
            Return Nothing
        End Function

        Private Sub DoProgress(message As String, type As MessageType)
            RaiseEvent ProgressChanged(Me, message, type)
        End Sub

        Friend Structure PFDEntries
            Friend Shared entries As List(Of PFDEntry)
        End Structure


        Public Class PFDEntry
            Friend __padding_0 As Byte()
            Friend __padding_1 As Byte()
            Friend additional_index As ULong
            Friend file_hashes As List(Of Byte())
            Friend file_name As String
            Friend file_size As ULong
            Friend key As Byte()

            Friend ReadOnly Property EntryData() As Byte()
                Get
                    Dim ms = New MemoryStream()
                    Using bw = New BinaryWriter(ms)
                        bw.Write(additional_index.SwapByteOrder())
                        Dim name = New Byte(64) {}
                        Array.Copy(Encoding.ASCII.GetBytes(file_name), 0, name, 0, file_name.Length)
                        bw.Write(name, 0, name.Length)
                        bw.Write(__padding_0, 0, __padding_0.Length)
                        bw.Write(key, 0, key.Length)
                        For i As Integer = 0 To file_hashes.Count - 1
                            bw.Write(file_hashes(i), 0, file_hashes(i).Length)
                        Next
                        bw.Write(__padding_1, 0, __padding_1.Length)
                        bw.Write(file_size.SwapByteOrder())
                        Return ms.ToArray()
                    End Using
                End Get
            End Property

            Friend ReadOnly Property HashData() As Byte()
                Get
                    Dim ms = New MemoryStream()
                    Using bw = New BinaryWriter(ms)
                        Dim name = New Byte(64) {}
                        Array.Copy(Encoding.ASCII.GetBytes(file_name), 0, name, 0, file_name.Length)
                        bw.Write(name, 0, name.Length)
                        bw.Write(key, 0, key.Length)
                        For i As Integer = 0 To file_hashes.Count - 1
                            bw.Write(file_hashes(i), 0, file_hashes(i).Length)
                        Next
                        bw.Write(__padding_1, 0, __padding_1.Length)
                        bw.Write(file_size.SwapByteOrder())
                        Return ms.ToArray()
                    End Using
                End Get
            End Property
        End Class

        Friend Structure PFDEntrySignatureTable
            Friend Shared hashes As New List(Of Byte())()

            Friend Shared ReadOnly Property Buffer() As Byte()
                Get
                    Dim buffer__1 = New Byte((hashes.Count * 20) - 1) {}
                    For i As Integer = 0 To hashes.Count - 1
                        Array.Copy(hashes(i), 0, buffer__1, (i * 20), 20)
                    Next
                    Return buffer__1
                End Get
            End Property
        End Structure

        Friend Structure PFDHashTable
            Public Shared capacity As ULong = 0
            Public Shared num_reserved As ULong = 0
            Public Shared num_used As ULong = 0
            Public Shared entries As New List(Of ULong)()

            Public Shared ReadOnly Property Buffer() As Byte()
                Get
                    Dim ms = New MemoryStream()
                    Using bw = New BinaryWriter(ms)
                        bw.Write(capacity.SwapByteOrder())
                        bw.Write(num_reserved.SwapByteOrder())
                        bw.Write(num_used.SwapByteOrder())
                        For Each value As ULong In entries
                            bw.Write(value.SwapByteOrder())
                        Next
                        Return ms.ToArray()
                    End Using
                End Get
            End Property
        End Structure

        Friend Structure PFDHeader
            Friend Shared magic As ULong
            Friend Shared version As ULong
        End Structure

        Friend Structure PFDSignature
            Friend Shared bottom_hash As Byte()
            Friend Shared top_hash As Byte()
            Friend Shared hash_key As Byte()
            Friend Shared padding As Byte()

            Friend Shared ReadOnly Property Buffer() As Byte()
                Get
                    Dim data = New Byte(63) {}
                    Array.Copy(bottom_hash, 0, data, 0, 20)
                    Array.Copy(top_hash, 0, data, 20, 20)
                    Array.Copy(hash_key, 0, data, 40, 20)
                    Array.Copy(padding, 0, data, 60, 4)
                    Return data
                End Get
            End Property
        End Structure

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub


        Public Sub New(filepath As String)
            Init(New FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
        End Sub


        Public Sub New(inputdata As Byte())
            Init(New MemoryStream(inputdata))
        End Sub


        Public Sub New(input As Stream)
            Init(input)
        End Sub

#End Region

#Region "public methods"

#Region "Validations"


        Public Function ValidTopHash(fix As Boolean) As Boolean
            Dim hash As Byte() = GetTopHash()
            If Not Functions.CompareBytes(hash, PFDSignature.top_hash) Then
                If fix Then
                    PFDSignature.top_hash = hash
                Else
                    Return False
                End If
            End If
            Return True
        End Function


        Public Function ValidBottomHash(fix As Boolean) As Boolean
            Dim hash As Byte() = GetBottomHash()
            If Not Functions.CompareBytes(hash, PFDSignature.bottom_hash) Then
                If fix Then
                    PFDSignature.bottom_hash = hash
                Else
                    Return False
                End If
            End If
            Return True
        End Function


        Public Function ValidFileCID(fix As Boolean) As Boolean
            Dim buffer As Byte() = GetDefaultHash()
            If buffer Is Nothing Then
                Return False
            End If
            Dim indexes = New List(Of Integer)()
            For Each ent As PFDEntry In PFDEntries.entries
                indexes.Add(CInt(CalculateHashTableEntryIndex(ent.file_name)))
            Next
            For i As Integer = 0 To PFDEntrySignatureTable.hashes.Count - 1
                If indexes.IndexOf(i) > -1 Then
                    Continue For
                End If
                If Not Functions.CompareBytes(buffer, PFDEntrySignatureTable.hashes(i)) Then
                    If fix Then
                        PFDEntrySignatureTable.hashes(i) = buffer
                    Else
                        Return False
                    End If
                End If
            Next
            Return True
        End Function


        Public Function ValidDHKCID2(fix As Boolean) As Boolean
            For i As Integer = 0 To PFDEntries.entries.Count - 1
                Dim hash As Byte() = GetEntryHash(i)
                Dim index = CInt(CalculateHashTableEntryIndex(PFDEntries.entries(i).file_name))
                If Not Functions.CompareBytes(hash, PFDEntrySignatureTable.hashes(index)) Then
                    If fix Then
                        PFDEntrySignatureTable.hashes(index) = hash
                    Else
                        Return False
                    End If
                End If
            Next
            Return True
        End Function


        Public Function ValidEntryHash(input As Stream, entryname As String, fix As Boolean) As Boolean
            If Not input.CanRead OrElse Not input.CanWrite Then
                Throw New Exception("Unable to Access stream")
            End If
            input.Position = 0
            For Each t As PFDEntry In PFDEntries.entries
                If entryname.ToLower() = t.file_name.ToLower() Then
                    If t.file_name.ToLower() = "param.sfo" Then
                        Console.WriteLine("Here!")
                    End If
                    For i As Integer = 0 To 3
                        If t.file_name.ToLower() = "param.sfo" AndAlso i <> 0 Then
                            Continue For
                        End If
                        If Not pfd_istropy AndAlso i > 0 Then
                            Continue For
                        End If
                        Dim key As Byte() = GetEntryHashKey(t, i)
                        key = Functions.CalculateFileHMACSha1(input, key)
                        If Not Functions.CompareBytes(key, t.file_hashes(i)) Then
                            If fix Then
                                t.file_hashes(i) = key
                            Else
                                Return False
                            End If
                        End If
                    Next

                    Return True
                End If
            Next
            Return False
        End Function


        Public Function ValidEntryHash(filepath As String, fix As Boolean) As Boolean
            If SecureFileID Is Nothing Then
                Return False
            End If
            If Not File.Exists(filepath) Then
                Throw New Exception(filepath & " does not exist!")
            End If
            Dim filename As String = New FileInfo(filepath).Name
            Dim x As Boolean = False
            Using fs = New FileStream(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read)
                x = ValidEntryHash(fs, filename, fix)
            End Using
            Return x
        End Function


        Public Function ValidAllEntryHashes(rootdirectory As String, fix As Boolean) As Boolean
            For i As Integer = 0 To PFDEntries.entries.Count - 1
                Dim filepath As String = rootdirectory & "\" & PFDEntries.entries(i).file_name
                If Not File.Exists(filepath) Then
                    Return False
                End If
                If Not ValidEntryHash(filepath, fix) Then
                    Return False
                End If
            Next
            Return True
        End Function


        Public Function ValidAllParamHashes(rootdirectory As String, fix As Boolean) As Boolean
            Return (ValidAllEntryHashes(rootdirectory, fix) AndAlso ValidDHKCID2(fix) AndAlso ValidFileCID(fix) AndAlso ValidTopHash(fix) AndAlso ValidBottomHash(fix))
        End Function

#End Region


        Public Sub INIT(filepath As String)
            Init(New FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
        End Sub


        Public Function RebuilParamPFD(rootdirectory As String, encryptfiles As Boolean) As Boolean
            Try
                If Not File.Exists(rootdirectory & "\PARAM.SFO") Then
                    Return False
                End If
                DoProgress("Rebuilding Param.PFD..", MessageType.Info)
                If encryptfiles Then
                    DoProgress("ReEncrypting Files..", MessageType.Info)
                    If EncryptAllFiles(rootdirectory) = -1 Then
                        Return False
                    End If
                End If
                DoProgress("Validating Param.PFD Hashes..", MessageType.Info)
                If Not ValidAllParamHashes(rootdirectory, True) Then
                    Return False
                End If
                DoProgress("Writing new Param.PFD..", MessageType.Info)
                File.WriteAllBytes(rootdirectory & "\PARAM.PFD", GetParamPFDCombinedData())
                DoProgress("Param.PFD Rebuilding complete! Rebuilded FilePath => " & rootdirectory & "\PARAM.PFD", MessageType.Info)
                Return True
            Catch ex As Exception
                DoProgress(ex.Message, MessageType.[Error])
                Return False
            End Try
        End Function


        Public Function GetParamPFDCombinedData() As Byte()
            Dim buffer As Byte() = Nothing
            DoProgress("Rebuilding new Param.PFD..", MessageType.Info)
            Using ms = New MemoryStream()
                Using bw = New BinaryWriter(ms)
                    bw.Write(PFDHeader.magic.SwapByteOrder())
                    bw.Write(PFDHeader.version.SwapByteOrder())
                    bw.Write(pfdheaderkey, 0, pfdheaderkey.Length)
                    buffer = PFDSignature.Buffer
                    buffer = Functions.EncryptWithPortability(pfdheaderkey, buffer, buffer.Length)
                    bw.Write(buffer, 0, buffer.Length)
                    buffer = PFDHashTable.Buffer
                    bw.Write(buffer, 0, buffer.Length)
                    For Each t As PFDEntry In PFDEntries.entries
                        buffer = t.EntryData
                        bw.Write(buffer, 0, buffer.Length)
                    Next
                    buffer = New Byte((&H110 * (PFDHashTable.num_reserved - PFDHashTable.num_used)) - 1) {}
                    bw.Write(buffer, 0, buffer.Length)
                    buffer = PFDEntrySignatureTable.Buffer
                    bw.Write(buffer, 0, buffer.Length)
                    buffer = New Byte(&H8000 - ms.Length - 1) {}
                    If buffer.Length > 0 Then
                        bw.Write(buffer, 0, buffer.Length)
                    End If
                    buffer = ms.ToArray()
                    bw.Close()
                End Using
            End Using
            DoProgress("Rebuild Completed!", MessageType.Info)
            Return buffer
        End Function

        Public Function EntryExists(name As String) As Boolean
            If PFDEntries.entries Is Nothing OrElse PFDEntries.entries.Count = 0 Then
                Return False
            End If
            For Each ent As PFDEntry In PFDEntries.entries
                If ent.file_name.ToLower() = name.ToLower() Then
                    Return True
                End If
            Next
            Return False
        End Function

        ''' <summary>
        '''     Progress Changed event, use this to recieve any progress made by this instance
        ''' </summary>
        Public Event ProgressChanged As PS3Action

#Region "Properties"

        Public Property ConsoleID() As Byte()
            Get
                Return m_consoleid
            End Get
            Set(value As Byte())
                If value.Length <> 32 Then
                    Throw New Exception("ConsoleID must be 32 bytes in length")
                End If
                m_consoleid = value
            End Set
        End Property

        Public Property DiscHashKey() As Byte()
            Get
                Return m_dischashkey
            End Get
            Set(value As Byte())
                If value.Length <> 16 Then
                    Throw New Exception("DiscHashKey must be 16 bytes in length")
                End If
                m_dischashkey = value
            End Set
        End Property

        Public Property AuthenticationID() As Byte()
            Get
                Return m_authenticationid
            End Get
            Set(value As Byte())
                If value.Length <> 8 Then
                    Throw New Exception("AuthenticationID must be 8 bytes in length")
                End If
                m_authenticationid = value
            End Set
        End Property

        Public Property SecureFileID() As Byte()
            Get
                Return m_securefileid
            End Get
            Set(value As Byte())
                If value.Length <> 16 Then
                    Throw New Exception("SecureFileID must nbe 16 bytes in length")
                End If
                m_securefileid = value
            End Set
        End Property

        Public Property UserID() As Byte()
            Get
                Return m_userid
            End Get
            Set(value As Byte())
                If value.Length <> 8 Then
                    Throw New Exception("UserID must be 8 bytes in length")
                End If
                m_userid = value
            End Set
        End Property

        Public ReadOnly Property Entries() As PFDEntry()
            Get
                Return PFDEntries.entries.ToArray()
            End Get
        End Property

#End Region

#Region "Decryption"

        ''' <summary>
        '''     Decrypts System.IO.Stream into a Byte Array
        ''' </summary>
        ''' <param name="stream">the input stream of the file that is located inside the Param.PFD Entries</param>
        ''' <param name="entryname">the name of the entry that the stream belongs to</param>
        ''' <returns>byte array of the decrypted file</returns>
        Public Function Decrypt(stream As Stream, entryname As String) As Byte()
            If SecureFileID Is Nothing OrElse SecureFileID.Length <> 16 Then
                DoProgress((If(SecureFileID Is Nothing, "SecureFileID needed to preform the encryption!", "SecureFileID is not valid! length must be 16 bytes long (128bit)")), MessageType.[Error])
                Return Nothing
            End If
            Dim ent = New PFDEntry()
            Dim found As Boolean = False
            For Each t As PFDEntry In PFDEntries.entries
                If t.file_name.ToLower() = entryname.ToLower() Then
                    ent = t
                    found = True
                    Exit For
                End If
            Next
            If Not found Then
                Throw New Exception("entryname does not exist inside the initialized Param.PFD")
            End If
            If Not stream.CanRead OrElse Not stream.CanWrite Then
                Throw New Exception("Unable to Access stream")
            End If

            If Not ValidEntryHash(stream, entryname, False) Then
                Throw New Exception("Encrypted data seems to be invalid, a validated file is required for this operation")
            End If

            Dim size As Integer = AlignedSize(CInt(stream.Length))
            DoProgress("Allocating memory (" & size & " bytes)..", MessageType.Info)
            Dim data = New Byte(size - 1) {}
            stream.Seek(0, SeekOrigin.Begin)
            stream.Read(data, 0, data.Length)
            DoProgress("Allocating decryption key..", MessageType.Info)
            Dim key As Byte() = GetEntryKey(ent)
            DoProgress("Decrypting data (" & size & " bytes)..", MessageType.Info)
            data = Functions.Decrypt(key, data, size)
            If data Is Nothing Then
                Throw New Exception("Unable to decrypt data")
            End If
            DoProgress("Free memory..", MessageType.Info)
            key = Nothing
            DoProgress("Resizing data to its original size..", MessageType.Info)
            Array.Resize(data, CInt(ent.file_size))
            Return data
        End Function

        ''' <summary>
        '''     decrypts input data
        ''' </summary>
        ''' <param name="inputdata">the data to decrypt</param>
        ''' <param name="entryname">the name of the entry that the data belongs to</param>
        ''' <returns>true if data is succesfully decrypted</returns>
        Public Function Decrypt(ByRef inputdata As Byte(), entryname As String) As Boolean
            Try
                Dim data As Byte() = Decrypt(New MemoryStream(inputdata), entryname)
                If data Is Nothing Then
                    Return False
                End If
                Array.Resize(inputdata, data.Length)
                Array.Copy(data, 0, inputdata, 0, data.Length)
                Return True
            Catch ex As Exception
                DoProgress(ex.Message, MessageType.[Error])
                Return False
            End Try
        End Function

        ''' <summary>
        '''     Decrypts specified filepath, file must be located inside the Param.PFD Entries
        ''' </summary>
        ''' <param name="filepath">the filepath that should be decrypted</param>
        ''' <returns>true if file is succesfully decrypted</returns>
        Public Function Decrypt(filepath As String) As Boolean
            Try
                If Not File.Exists(filepath) OrElse Not ValidEntryHash(filepath, False) Then
                    Return False
                End If
                Dim name As String = New FileInfo(filepath).Name
                Dim data As Byte() = Nothing
                Using fs As FileStream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                    data = Decrypt(fs, name)
                    fs.Close()
                End Using
                If data IsNot Nothing Then
                    File.WriteAllBytes(filepath, data)
                Else
                    Return False
                End If
                data = Nothing
                Return True
            Catch ex As Exception
                DoProgress(ex.Message, MessageType.[Error])
                Return False
            End Try
        End Function

        ''' <summary>
        '''     decrypts filepath to a new filepath destination
        ''' </summary>
        ''' <param name="filepath">the filepath of the file to decrypt, file must be located inside the Param.PFD Entries</param>
        ''' <param name="outpath">the new filepath that you wish to use for the decrypted file</param>
        ''' <returns>true if file is succesfully decrypted</returns>
        Public Function Decrypt(filepath As String, outpath As String) As Boolean
            Try
                If Not File.Exists(filepath) OrElse Not ValidEntryHash(filepath, False) Then
                    Return False
                End If
                Dim name As String = New FileInfo(filepath).Name
                Dim fs As FileStream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                Dim data As Byte() = Decrypt(fs, name)
                fs.Close()
                fs.Dispose()
                File.WriteAllBytes(outpath, data)
                Return True
            Catch ex As Exception
                DoProgress(ex.Message, MessageType.[Error])
                Return False
            End Try
        End Function


        Public Function DecryptAllFiles(root As String) As Integer
            Try
                Dim decrypted As Integer = 0
                For Each t As PFDEntry In PFDEntries.entries
                    If t.file_name.ToLower() = "param.sfo" Then
                        Continue For
                    End If
                    Dim filepath As String = root & "\" & t.file_name
                    If File.Exists(filepath) Then
                        If ValidEntryHash(filepath, False) Then
                            If Decrypt(filepath) Then
                                decrypted += 1
                            End If
                        End If
                    End If
                Next
                Return decrypted
            Catch ex As Exception
                DoProgress(ex.Message, MessageType.[Error])
                Return -1
            End Try
        End Function

        ''' <summary>
        '''     Decrypts specified filepath into a byte array
        ''' </summary>
        ''' <param name="filepath">the filepath to decrypt, file must be located inside the Param.PFD Entries</param>
        ''' <returns>byte array of the decrypted file</returns>
        Public Function DecryptToBytes(filepath As String) As Byte()
            If Not File.Exists(filepath) Then
                Return Nothing
            End If
            Dim name As String = New FileInfo(filepath).Name
            Dim fs As FileStream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            Dim data As Byte() = Decrypt(fs, name)
            fs.Close()
            fs.Dispose()
            Return data
        End Function

        ''' <summary>
        '''     Decrypts specified filepath into a System.IO.Stream
        ''' </summary>
        ''' <param name="filepath">the filepath to decrypt, file must be located inside the Param.PFD Entries</param>
        ''' <returns>System.IO.Stream of the decrypted file</returns>
        Public Function DecryptToStream(filepath As String) As Stream
            Dim data As Byte() = DecryptToBytes(filepath)
            If data Is Nothing Then
                Return Nothing
            End If
            Return New MemoryStream(data)
        End Function

#End Region

#Region "Encryption"

        ''' <summary>
        '''     Encrypt a specified System.IO.Stream to a byte array
        ''' </summary>
        ''' <param name="stream">Input System.IO.Stream of the file to encrypt</param>
        ''' <param name="entryname">the name of the entry inside the PARAM.PFD</param>
        ''' <returns>a byte array of theencrypted stream</returns>
        Public Function Encrypt(stream As Stream, entryname As String) As Byte()
            If SecureFileID Is Nothing OrElse SecureFileID.Length <> 16 Then
                DoProgress((If(SecureFileID Is Nothing, "SecureFileID needed to preform the encryption!", "SecureFileID is not valid! length must be 16 bytes long (128bit)")), MessageType.[Error])
                Return Nothing
            End If
            If ValidEntryHash(stream, entryname, False) Then
                DoProgress("File already valid, same data will be returned instead", MessageType.Info)
                Dim data = New Byte(stream.Length - 1) {}
                stream.Seek(0, SeekOrigin.Begin)
                stream.Read(data, 0, data.Length)
                Return data
            Else
                Dim ent = New PFDEntry()
                Dim found As Boolean = False
                For Each t As PFDEntry In PFDEntries.entries
                    If t.file_name.ToLower() = entryname.ToLower() Then
                        ent = t
                        found = True
                        Exit For
                    End If
                Next
                If Not found Then
                    Throw New Exception("entryname does not exist inside the initialized Param.PFD")
                End If
                If Not stream.CanRead OrElse Not stream.CanWrite Then
                    Throw New Exception("Unable to Access stream")
                End If
                ent.file_size = CULng(stream.Length)
                stream.Seek(0, SeekOrigin.Begin)
                Dim size As Integer = AlignedSize(CInt(stream.Length))
                DoProgress("Allocating memory (" & size & " bytes)..", MessageType.Info)
                Dim data = New Byte(size - 1) {}
                DoProgress("Reading stream into memory..", MessageType.Info)
                stream.Read(data, 0, CInt(stream.Length))
                DoProgress("Allocating encryption key..", MessageType.Info)
                Dim key As Byte() = GetEntryKey(ent)
                DoProgress("Encrypting Data (" & size & "bytes)..", MessageType.Info)
                data = Functions.Encrypt(key, data, data.Length)
                If data Is Nothing Then
                    Throw New Exception("Unable to decrypt data")
                End If
                DoProgress("Free allocated memory..", MessageType.Info)
                key = Nothing
                Return data
            End If
        End Function

        ''' <summary>
        '''     Encrypts inputdata
        ''' </summary>
        ''' <param name="inputdata">the byte array of the file to encrypt</param>
        ''' <param name="entry">the the entry inside the PARAM.PFD</param>
        ''' <returns>true if succesfully encrypted</returns>
        Public Function Encrypt(inputdata As Byte(), entry As Ps3File) As Boolean
            Try
                entry.PFDEntry.file_size = CULng(inputdata.Length)
                Dim data As Byte() = Encrypt(New MemoryStream(inputdata), entry.PFDEntry.file_name)
                If data Is Nothing Then
                    Return False
                End If
                File.WriteAllBytes(entry.FilePath, data)
                data = Nothing
                Return True
            Catch
                Return False
            End Try
        End Function

        ''' <summary>
        '''     Encrypts specified filepath
        ''' </summary>
        ''' <param name="filepath">the filepath to encrypt, file must be in the same directory as the PARAM.PFD</param>
        ''' <returns>true if file is succesfully encrypted</returns>
        Public Function Encrypt(filepath As String) As Boolean
            Try
                If Not File.Exists(filepath) Then
                    DoProgress(filepath & " Does not exist!", MessageType.[Error])
                    Return False
                End If
                Dim name As String = New FileInfo(filepath).Name
                If Not EntryExists(name) Then
                    DoProgress("There is no """ & name & """ inside the PARAM.PFD Entries!", MessageType.[Error])
                    Return False
                End If
                DoProgress("Initializing file stream..", MessageType.Info)
                Dim data As Byte() = Nothing
                Using fs As FileStream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                    For i As Integer = 0 To PFDEntries.entries.Count - 1
                        If PFDEntries.entries(i).file_name.ToLower() = name.ToLower() Then
                            Dim t As PFDEntry = PFDEntries.entries(i)
                            t.file_size = CULng(fs.Length)
                            PFDEntries.entries(i) = t
                            Exit For
                        End If
                    Next

                    data = Encrypt(fs, name)
                    'DoProgress("Rehashing PARAM.PFD..", MessageType.Info);
                    'bool x = ValidEntryHash(fs, name, true) && ValidFileCID(true) && ValidDHKCID2(true) && ValidBottomHash(true);
                    fs.Dispose()
                End Using
                If data Is Nothing Then
                    Return False
                End If
                DoProgress("Writing Encrypted data to : " & filepath, MessageType.Info)
                File.WriteAllBytes(filepath, data)
                DoProgress(name & " is succesfully encrypted", MessageType.Info)
                Return True
            Catch ex As Exception
                DoProgress(ex.Message, MessageType.[Error])
                Return False
            End Try
        End Function

        ''' <summary>
        '''     Encrypts specified filepath into a new filepath
        ''' </summary>
        ''' <param name="filepath">the filepath of the file to encrypt, path must be in the same location as the PARAM.PFD</param>
        ''' <param name="newfilepath">the new filepath to encrypt the file to</param>
        ''' <returns>true if file is succesfully encrypted</returns>
        Public Function Encrypt(filepath As String, newfilepath As String) As Boolean
            Try
                If Not File.Exists(filepath) Then
                    Return False
                End If
                Dim name As String = New FileInfo(filepath).Name
                DoProgress("Encrypting " & name & "..", MessageType.Info)
                Dim fs As FileStream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                Dim data As Byte() = Encrypt(fs, name)
                fs.Close()
                fs.Dispose()
                DoProgress("Encrypting " & name & "..", MessageType.Info)
                File.WriteAllBytes(newfilepath, data)
                If filepath = newfilepath Then
                    Return ValidEntryHash(filepath, True)
                End If
                Return True
            Catch
                Return False
            End Try
        End Function

        Public Function EncryptAllFiles(root As String) As Integer
            Try
                Dim encrypted As Integer = 0
                For i As Integer = 0 To PFDEntries.entries.Count - 1
                    Dim t As PFDEntry = PFDEntries.entries(i)
                    If t.file_name.ToLower() = "param.sfo" Then
                        Continue For
                    End If
                    Dim filepath As String = root & "\" & t.file_name
                    If File.Exists(filepath) Then
                        If Not ValidEntryHash(filepath, False) Then
                            If Encrypt(filepath) Then
                                If Not ValidEntryHash(filepath, True) Then
                                    Return -1
                                End If
                                encrypted += 1
                            End If
                        End If
                    End If
                Next
                Return encrypted
            Catch
                Return -1
            End Try
        End Function

        Public Function EncryptToBytes(filepath As String) As Byte()
            If Not File.Exists(filepath) Then
                Return Nothing
            End If
            Dim name As String = New FileInfo(filepath).Name
            Dim fs As FileStream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            Dim data As Byte() = Encrypt(fs, name)
            fs.Close()
            fs.Dispose()
            Return data
        End Function

        Public Function EncryptToStream(filepath As String) As Stream
            Dim data As Byte() = EncryptToBytes(filepath)
            If data Is Nothing Then
                Return Nothing
            End If
            Return New MemoryStream(data)
        End Function

#End Region

#End Region
    End Class
End Namespace