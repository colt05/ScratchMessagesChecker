Imports System.IO
Imports System.Net
Public Class ApiInterface
    ''' <summary>
    ''' Checks if the user exists.
    ''' </summary>
    ''' <param name="username">Username</param>
    ''' <param name="CanReturnError">Can return "ERR_Scratch_Servers_Down" if the Scratch API is not working.</param>
    ''' <returns>True if user exists or False if user does not exist. If the Scratch API is not working, returns false by default.</returns>
    Public Shared Function UserExists(ByVal username As String, Optional ByVal CanReturnError As Boolean = False) As Object
        Try
            Dim responseString As String = GetRequest(String.Concat("https://api.scratch.mit.edu/users/", username))
            If responseString = "{""code"":""NotFoundError"",""message"":""}" Then
                If CanReturnError = True Then
                    Return "ERR_Scratch_Servers_Down"
                Else
                    Return False
                End If
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Gets amount of messages
    ''' </summary>
    ''' <param name="username">Username</param>
    ''' <param name="CanReturnError">Can return "ERR_Scratch_Servers_Down" if the Scratch API is not working.</param>
    ''' <returns>Message count</returns>
    Public Shared Function GetMessages(ByVal username As String, Optional ByVal CanReturnError As Boolean = False) As Integer
        If UserExists(username) Then
            Dim response As String = GetRequest(String.Concat("https://api.scratch.mit.edu/proxy/users/", username, "/activity/count"), False)
            If response = String.Concat("{""code"":""ResourceNotFound"",""message"":""/proxy/users/}", username, "/activity/count does not exist""") Then
                Return 0
            ElseIf response = "ERR_Scratch_Servers_Down" Then
                If CanReturnError = False Then
                    Return 0
                Else
                    Return "ERR_Scratch_Servers_Down"
                End If
            Else
                    response = response.Replace("{""msg_count"":", String.Empty)
                response = response.Replace("}", String.Empty)
                Return response
            End If
        Else
            Return 0
        End If
    End Function
    ''' <summary>
    ''' GET request
    ''' </summary>
    ''' <param name="URL">URL to request</param>
    ''' <param name="CanThrowException">If true, will throw ScratchServersDownException() if an error occurs. If false, will return "ERR_Scratch_Servers_Down".</param>
    ''' <returns>Response</returns>
    Public Shared Function GetRequest(ByVal URL As String, Optional ByVal CanThrowException As Boolean = False) As String
        Dim cookies As New CookieContainer
        Dim request As HttpWebRequest = DirectCast(HttpWebRequest.Create(URL), HttpWebRequest)
        request.Method = "GET"
        request.KeepAlive = True
        request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729"
        request.ContentType = "application/x-www-form-urlencoded"
        request.Referer = "https://api.scratch.mit.edu"
        Dim response As WebResponse
        Try
            response = request.GetResponse()
        Catch ex As System.Net.WebException
            If CanThrowException = True Then
                Throw New ScratchServersDownException()
            Else
                Return "ERR_Scratch_Servers_Down"
            End If
        End Try
        Dim responseStream As Stream = response.GetResponseStream()
        Dim reader As StreamReader = New StreamReader(responseStream)
        Dim responseString As String = reader.ReadToEnd()
        responseStream.Close()
        Return responseString
    End Function
    ''' <summary>
    ''' Checks if the Scratch Website is broken.
    ''' </summary>
    ''' <returns>True if the Scratch Servers are broken, false if they are working.</returns>
    Public Shared Function ScratchServersAreBroken() As Boolean
        Try
            Dim result As String = ApiInterface.GetRequest("https://scratch.mit.edu/")
            If result = "ERR_Scratch_Servers_Down" Then
                Return True
            Else
                Return False
            End If
        Catch
        End Try
    End Function
End Class
''' <summary>
''' ScratchServersDownException means the Scratch API returned a 503 error (Scratch website down for making changes to the website) or (possibly) 404.
''' </summary>
Class ScratchServersDownException
    Inherits Exception
End Class
