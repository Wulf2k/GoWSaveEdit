Namespace PS3FileSystem
    Public Class SecureFileInfo
        Public Sub New(name__1 As String, id As String, securefileid__2 As String, dischashkey__3 As String, isprotected As Boolean)
            Name = name__1
            GameIDs = id.Trim({"["c, "]"c}).Split("/"c)
            SecureFileID = securefileid__2
            DiscHashKey = dischashkey__3
            [Protected] = isprotected
        End Sub

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = Value
            End Set
        End Property
        Private m_Name As String
        Public Property GameIDs() As String()
            Get
                Return m_GameIDs
            End Get
            Set(value As String())
                m_GameIDs = Value
            End Set
        End Property
        Private m_GameIDs As String()
        Public Property SecureFileID() As String
            Get
                Return m_SecureFileID
            End Get
            Set(value As String)
                m_SecureFileID = Value
            End Set
        End Property
        Private m_SecureFileID As String
        Public Property DiscHashKey() As String
            Get
                Return m_DiscHashKey
            End Get
            Set(value As String)
                m_DiscHashKey = Value
            End Set
        End Property
        Private m_DiscHashKey As String

        Public Property [Protected]() As Boolean
            Get
                Return m_Protected
            End Get
            Set(value As Boolean)
                m_Protected = Value
            End Set
        End Property
        Private m_Protected As Boolean
    End Class
End Namespace