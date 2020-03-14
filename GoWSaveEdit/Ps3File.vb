Imports System.IO

Namespace PS3FileSystem
    Public Class Ps3File
        Public Sub New(filepath__1 As String, entry As Param_PFD.PFDEntry, manager__2 As Ps3SaveManager)
            FilePath = filepath__1
            PFDEntry = entry
            Manager = manager__2
        End Sub

        Public Property FilePath() As String
            Get
                Return m_FilePath
            End Get
            Private Set(value As String)
                m_FilePath = Value
            End Set
        End Property
        Private m_FilePath As String

        Public Property PFDEntry() As Param_PFD.PFDEntry
            Get
                Return m_PFDEntry
            End Get
            Private Set(value As Param_PFD.PFDEntry)
                m_PFDEntry = Value
            End Set
        End Property
        Private m_PFDEntry As Param_PFD.PFDEntry

        Public Property Manager() As Ps3SaveManager
            Get
                Return m_Manager
            End Get
            Private Set(value As Ps3SaveManager)
                m_Manager = Value
            End Set
        End Property
        Private m_Manager As Ps3SaveManager

        Public ReadOnly Property IsEncrypted() As Boolean
            Get
                Return Manager.Param_PFD.ValidEntryHash(FilePath, False)
            End Get
        End Property

        Public Function Decrypt() As Boolean
            Return Manager.Param_PFD.Decrypt(FilePath)
        End Function

        Public Function DecryptToBytes() As Byte()
            Return Manager.Param_PFD.DecryptToBytes(FilePath)
        End Function

        Public Function DecryptToStream() As Stream
            Return Manager.Param_PFD.DecryptToStream(FilePath)
        End Function

        Public Function Encrypt() As Boolean
            Return Manager.Param_PFD.Encrypt(FilePath)
        End Function

        Public Function Encrypt(data As Byte()) As Boolean
            Return Manager.Param_PFD.Encrypt(data, Me)
        End Function

        Public Function EncryptToBytes() As Byte()
            Return Manager.Param_PFD.EncryptToBytes(FilePath)
        End Function

        Public Function EncryptToStream() As Stream
            Return Manager.Param_PFD.EncryptToStream(FilePath)
        End Function
    End Class
End Namespace