Imports System.IO
Imports System.Net
Public Class ApiInterface
    ''' <summary>
    ''' Checks if the user exists
    ''' </summary>
    ''' <param name="username">Username</param>
    ''' <returns>True if user exists or False if user does not exist</returns>
    Public Shared Function UserExists(ByVal username As String) As Boolean
        Try
            Dim responseString As String = GetRequest(String.Concat("https://api.scratch.mit.edu/users/", username))
            If responseString = "{""code"":""NotFoundError"",""message"":""}" Then
                Return False
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
    ''' <returns>Message count</returns>
    Public Shared Function GetMessages(ByVal username As String) As Integer
        If UserExists(username) Then
            Dim response As String = GetRequest(String.Concat("https://api.scratch.mit.edu/proxy/users/", username, "/activity/count"))
            If response = String.Concat("{""code"":""ResourceNotFound"",""message"":""/proxy/users/}", username, "/activity/count does not exist""") Then
                Return 0
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
    ''' <returns>Response</returns>
    Public Shared Function GetRequest(ByVal URL As String) As String
        Dim cookies As New CookieContainer
        Dim request As HttpWebRequest = DirectCast(HttpWebRequest.Create(URL), HttpWebRequest)
        request.Method = "GET"
        request.KeepAlive = True
        request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; ru; rv:1.9.2.3) Gecko/20100401 Firefox/4.0 (.NET CLR 3.5.30729"
        request.ContentType = "application/x-www-form-urlencoded"
        request.Referer = "https://api.scratch.mit.edu"
        Dim response As WebResponse = request.GetResponse()
        Dim responseStream As Stream = response.GetResponseStream()
        Dim reader As StreamReader = New StreamReader(responseStream)
        Dim responseString As String = reader.ReadToEnd()
        responseStream.Close()
        Return responseString
    End Function
End Class
