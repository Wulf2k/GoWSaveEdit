Namespace PS3FileSystem

    Public Class Delegates

        Public Enum MessageType
            Info
            Warning
            [Error]
        End Enum

        Public Delegate Sub PS3Action(sender As Object, message As String, type As MessageType)

    End Class
End Namespace
