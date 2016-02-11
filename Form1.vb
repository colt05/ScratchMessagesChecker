Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = TextBox1.Text
        If username.Contains("@") Then
            If username.Replace("@", String.Empty) = String.Empty Then
                MsgBox("Invalid username.")
            Else
                username = username.Replace("@", String.Empty)
            End If
        End If
        Dim result As String = ApiInterface.GetMessages(username)
        If result = 1 Then
            MsgBox(String.Concat(username, " has ", result, " message."))
        Else
            MsgBox(String.Concat(username, " has ", result, " messages."))
        End If
    End Sub
End Class
