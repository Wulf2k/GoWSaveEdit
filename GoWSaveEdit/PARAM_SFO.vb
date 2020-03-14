Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace PS3FileSystem
    Public Class PARAM_SFO
        Public Enum DataTypes As UInteger
            GameData = &H4744
            SaveData = &H5344
            AppPhoto = &H4150
            AppMusic = &H414D
            AppVideo = &H4156
            BroadCastVideo = &H4256
            AppleTV = 4154
            WebTV = 5754
            CellBE = &H4342
            Home = &H484D
            StoreFronted = &H5346
            HDDGame = &H4847
            DiscGame = &H4447
            AutoInstallRoot = &H4152
            DiscPackage = &H4450
            ExtraRoot = &H5852
            VideoRoot = &H5652
            ThemeRoot = &H5452
            DiscMovie = &H444D
            None
        End Enum

        Public Enum FMT As UShort
            UTF_8 = &H400
            ASCII = &H402
            UINT32 = &H404
        End Enum

        Public Sub New(filepath As String)
            Init(New FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
        End Sub

        Public Sub New(inputdata As Byte())
            Init(New MemoryStream(inputdata))
        End Sub

        Public Sub New(input As Stream)
            Init(input)
        End Sub

        Public Property Tables() As Table()
            Get
                Return m_Tables
            End Get
            Private Set(value As Table())
                m_Tables = Value
            End Set
        End Property
        Private m_Tables As Table()

        Public ReadOnly Property AccountID() As String
            Get
                If Tables Is Nothing Then
                    Return ""
                End If
                For Each t As Table In Tables
                    If t.Name.ToLower() = "account_id" Then
                        Return t.Value
                    End If
                Next
                Return ""
            End Get
        End Property


        Public ReadOnly Property DataType() As DataTypes
            Get
                If Tables Is Nothing Then
                    Return DataTypes.None
                End If
                For Each t As Table In Tables
                    If t.Name = "CATEGORY" Then
                        Return CType(BitConverter.ToUInt16(Encoding.UTF8.GetBytes(t.Value), 0), DataTypes)
                    End If
                Next
                Return DataTypes.None
            End Get
        End Property

        Public ReadOnly Property Detail() As String
            Get
                If Tables Is Nothing Then
                    Return ""
                End If
                For Each t As Table In Tables
                    If t.Name = "DETAIL" Then
                        Return t.Value
                    End If
                Next
                Return ""
            End Get
        End Property

        Public ReadOnly Property ParentalControl() As UInteger
            Get
                If Tables Is Nothing Then
                    Return 0
                End If
                For Each t As Table In Tables
                    If t.Name = "PARENTAL_LEVEL" Then
                        Return UInteger.Parse(t.Value)
                    End If
                Next
                Return 0
            End Get
        End Property

        Public ReadOnly Property DirectoryName() As String
            Get
                If Tables Is Nothing Then
                    Return ""
                End If
                For Each t As Table In Tables
                    If t.Name = "SAVEDATA_DIRECTORY" Then
                        Return t.Value
                    End If
                Next
                Return ""
            End Get
        End Property

        Public ReadOnly Property TitleID() As String
            Get
                Dim name As String = DirectoryName
                If name = "" Then
                    Return ""
                End If
                Return name.Split("-"c)(0)
            End Get
        End Property


        Public ReadOnly Property SubTitle() As String
            Get
                If Tables Is Nothing Then
                    Return ""
                End If
                For Each t As Table In Tables
                    If t.Name = "SUB_TITLE" Then
                        Return t.Value
                    End If
                Next
                Return ""
            End Get
        End Property

        Public ReadOnly Property Title() As String
            Get
                If Tables Is Nothing Then
                    Return ""
                End If
                For Each t As Table In Tables
                    If t.Name = "TITLE" Then
                        Return t.Value
                    End If
                Next
                Return ""
            End Get
        End Property

        Private Function ReadValue(br As BinaryReader, table As index_table) As String
            br.BaseStream.Position = ((Header.DataTableStart) + table.param_data_offset)
            Select Case table.param_data_fmt
                Case FMT.ASCII
                    Return Encoding.ASCII.GetString(br.ReadBytes(CInt(table.param_data_max_len))).Replace(vbNullChar, "")
                Case FMT.UINT32
                    Return br.ReadUInt32().ToString()
                Case FMT.UTF_8
                    Return Encoding.UTF8.GetString(br.ReadBytes(CInt(table.param_data_max_len))).Replace(vbNullChar, "")
                Case Else
                    Return Nothing
            End Select
        End Function

        Private Function ReadName(br As BinaryReader, table As index_table) As String
            br.BaseStream.Position = (Header.KeyTableStart + table.param_key_offset)
            Dim name As String = ""
            While CByte(br.PeekChar()) <> 0
                name += br.ReadChar()
            End While
            br.BaseStream.Position += 1
            Return name
        End Function


        Private Sub Init(input As Stream)
            Using br = New BinaryReader(input)
                Header.Read(br)
                If Not Functions.CompareBytes(Header.Magic, New Byte() {0, &H50, &H53, &H46}) Then
                    Throw New Exception("Invalid PARAM.SFO Header Magic")
                End If
                Dim tables__1 = New List(Of index_table)()
                For i As Integer = 0 To Header.IndexTableEntries - 1
                    Dim t = New index_table()
                    t.Read(br)
                    tables__1.Add(t)
                Next
                Dim xtables = New List(Of Table)()
                Dim count As Integer = 0
                For Each t As index_table In tables__1
                    Dim x = New Table()
                    x.index = count
                    x.Indextable = t
                    x.Name = ReadName(br, t)
                    x.Value = ReadValue(br, t)
                    count += 1
                    xtables.Add(x)
                Next
                Tables = xtables.ToArray()
                br.Close()
            End Using
        End Sub

        Public Structure Header
            Public Shared Magic As Byte() = {0, &H50, &H53, &H46}
            Public Shared version As Byte() = {1, 1, 0, 0}
            Public Shared KeyTableStart As UInteger = 0
            Public Shared DataTableStart As UInteger = 0
            Public Shared IndexTableEntries As UInteger = 0

            Private Shared ReadOnly Property Buffer() As Byte()
                Get
                    Dim header = New Byte(19) {}
                    Array.Copy(Magic, 0, header, 0, 4)
                    Array.Copy(version, 0, header, 4, 4)
                    Array.Copy(BitConverter.GetBytes(KeyTableStart), 0, header, 8, 4)
                    Array.Copy(BitConverter.GetBytes(DataTableStart), 0, header, 12, 4)
                    Array.Copy(BitConverter.GetBytes(IndexTableEntries), 0, header, 16, 4)
                    Return header
                End Get
            End Property

            Public Shared Sub Read(input As BinaryReader)
                input.BaseStream.Seek(0, SeekOrigin.Begin)
                input.Read(Magic, 0, 4)
                input.Read(version, 0, 4)
                KeyTableStart = input.ReadUInt32()
                DataTableStart = input.ReadUInt32()
                IndexTableEntries = input.ReadUInt32()
            End Sub
        End Structure

        Public Structure Table
            Public Indextable As index_table
            Public Name As String
            Public Value As String
            Public index As Integer

            Private ReadOnly Property NameBuffer() As Byte()
                Get
                    Dim buffer = New Byte(Name.Length) {}
                    Array.Copy(Encoding.UTF8.GetBytes(Name), 0, buffer, 0, Name.Length)
                    Return buffer
                End Get
            End Property

            Private ReadOnly Property ValueBuffer() As Byte()
                Get
                    Dim buffer As Byte()
                    Select Case Indextable.param_data_fmt
                        Case FMT.ASCII
                            buffer = New Byte(Indextable.param_data_max_len - 1) {}
                            Array.Copy(Encoding.ASCII.GetBytes(Value), 0, buffer, 0, Value.Length)
                            Return buffer
                        Case FMT.UINT32
                            Return BitConverter.GetBytes(UInteger.Parse(Value))
                        Case FMT.UTF_8
                            buffer = New Byte(Indextable.param_data_max_len - 1) {}
                            Array.Copy(Encoding.UTF8.GetBytes(Value), 0, buffer, 0, Value.Length)
                            Return buffer
                        Case Else
                            Return Nothing
                    End Select
                End Get
            End Property
        End Structure

        Public Structure index_table
            Public param_data_fmt As FMT
            ' param_data data type 
            Public param_data_len As UInteger
            ' param_data used bytes 
            Public param_data_max_len As UInteger
            ' param_data total reserved bytes 
            Public param_data_offset As UInteger
            ' param_data offset (relative to start offset of data_table) 
            Public param_key_offset As UShort
            ' param_key offset (relative to start offset of key_table) 

            Private ReadOnly Property Buffer() As Byte()
                Get
                    Dim data = New Byte(15) {}
                    Array.Copy(BitConverter.GetBytes(param_key_offset), 0, data, 0, 2)
                    Array.Copy(BitConverter.GetBytes(CUShort(param_data_fmt).SwapByteOrder()), 0, data, 2, 2)
                    Array.Copy(BitConverter.GetBytes(param_data_len), 0, data, 4, 4)
                    Array.Copy(BitConverter.GetBytes(param_data_max_len), 0, data, 8, 4)
                    Array.Copy(BitConverter.GetBytes(param_data_offset), 0, data, 12, 4)
                    Return data
                End Get
            End Property

            Public Sub Read(input As BinaryReader)
                param_key_offset = input.ReadUInt16()
                param_data_fmt = CType(input.ReadUInt16().SwapByteOrder(), FMT)
                param_data_len = input.ReadUInt32()
                param_data_max_len = input.ReadUInt32()
                param_data_offset = input.ReadUInt32()
            End Sub
        End Structure
    End Class
End Namespace