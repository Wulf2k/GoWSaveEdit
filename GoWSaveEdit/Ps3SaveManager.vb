Imports System.Collections.Generic
Imports System.Drawing
Imports System.IO
Imports System.Linq

Namespace PS3FileSystem
    Public Class Ps3SaveManager
        Public Shared GameConfigList As SecureFileInfo()

        Public Sub New(savedir As String, securefileid As Byte())
            If Not Directory.Exists(savedir) Then
                Throw New Exception("No such directory exist!")
            End If
            If Not File.Exists(savedir & "\PARAM.PFD") Then
                Throw New Exception("Rootdirectory does not contain any PARAM.PFD, Please load a valid directory")
            End If
            If Not File.Exists(savedir & "\PARAM.SFO") Then
                Throw New Exception("Rootdirectory does not contain any PARAM.SFO, Please load a valid directory")
            End If
            Param_PFD = New Param_PFD(savedir & "\PARAM.PFD")
            Param_SFO = New PARAM_SFO(savedir & "\PARAM.SFO")
            If securefileid IsNot Nothing Then
                Param_PFD.SecureFileID = securefileid
            End If
            RootPath = savedir
            If File.Exists(savedir & "\ICON0.PNG") Then
                'prevent file lock,reading to memory instead.
                SaveImage = Image.FromStream(New MemoryStream(File.ReadAllBytes(savedir & "\ICON0.PNG")))
            End If

            Files = (From ent In Param_PFD.Entries Let x = New FileInfo(savedir & "\" & Convert.ToString(ent.file_name)) Where x.Extension.ToUpper() <> ".PFD" AndAlso x.Extension.ToUpper() <> ".SFO" Select New Ps3File(savedir & "\" & Convert.ToString(ent.file_name), ent, Me)).ToArray()
        End Sub

        Public Property RootPath() As String
            Get
                Return m_RootPath
            End Get
            Private Set(value As String)
                m_RootPath = Value
            End Set
        End Property
        Private m_RootPath As String

        Public Property Param_PFD() As Param_PFD
            Get
                Return m_Param_PFD
            End Get
            Private Set(value As Param_PFD)
                m_Param_PFD = Value
            End Set
        End Property
        Private m_Param_PFD As Param_PFD

        Public Property Param_SFO() As PARAM_SFO
            Get
                Return m_Param_SFO
            End Get
            Private Set(value As PARAM_SFO)
                m_Param_SFO = Value
            End Set
        End Property
        Private m_Param_SFO As PARAM_SFO

        Public Property Files() As Ps3File()
            Get
                Return m_Files
            End Get
            Private Set(value As Ps3File())
                m_Files = Value
            End Set
        End Property
        Private m_Files As Ps3File()

        Public Property SaveImage() As Image
            Get
                Return m_SaveImage
            End Get
            Private Set(value As Image)
                m_SaveImage = Value
            End Set
        End Property
        Private m_SaveImage As Image

        Public Function DecryptAllFiles() As Integer
            Try
                If Param_PFD Is Nothing OrElse Not Directory.Exists(RootPath) Then
                    Return -1
                End If
                Return Param_PFD.DecryptAllFiles(RootPath)
            Catch
                Return -1
            End Try
        End Function

        Public Function EncryptAllFiles() As Integer
            Try
                If Param_PFD Is Nothing OrElse Not Directory.Exists(RootPath) Then
                    Return -1
                End If
                Dim x As Integer = Param_PFD.EncryptAllFiles(RootPath)
                If x > 0 Then
                    If Param_PFD.RebuilParamPFD(RootPath, False) Then
                        Return x
                    End If
                End If
                Return -1
            Catch
                Return -1
            End Try
        End Function


        Public Function ReBuildChanges() As Boolean
            Return Param_PFD.RebuilParamPFD(RootPath, False)
        End Function

        Public Function ReBuildChanges(encryptfiles As Boolean) As Boolean
            Return Param_PFD.RebuilParamPFD(RootPath, encryptfiles)
        End Function

        Public Function LoadGameConfigFile(filepath As String) As Integer
            Try
                Dim text As String = ""
                Using sr = New StreamReader(New FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    text = sr.ReadToEnd()
                    sr.Close()
                End Using
                Return (InlineAssignHelper(GameConfigList, Functions.ReadConfigFromtext(text))).Length
            Catch
                Return -1
            End Try
        End Function

        Private Function GetSecureFileIdFromConfigFile(titleid As String) As Byte()
            If GameConfigList Is Nothing OrElse GameConfigList.Length = 0 Then
                Return Nothing
            End If
            Return (From i In GameConfigList From s In i.GameIDs Where s.ToLower() = titleid.ToLower() Where i.SecureFileID IsNot Nothing AndAlso i.SecureFileID.Length = 32 Select i.SecureFileID.StringToByteArray()).FirstOrDefault()
        End Function
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace