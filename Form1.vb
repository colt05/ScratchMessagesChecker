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
        Dim result As String = 0
        Try
            result = ApiInterface.GetMessages(username, True)
            If result = "ERR_Scratch_Servers_Down" Then
                MsgBox("The Scratch API is not working. Try again later.")
            End If
        Catch ex As ScratchServersDownException
            MsgBox("The Scratch API is not working. Try again later.")
        Catch ex As Exception
            MsgBox(String.Concat("Error: ", ex.Message))
        End Try
        If result = 1 Then
            MsgBox(String.Concat(username, " has ", result, " message."))
        Else
            MsgBox(String.Concat(username, " has ", result, " messages."))
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim result As String = ApiInterface.GetRequest("https://scratch.mit.edu/")
            If result = "ERR_Scratch_Servers_Down" Then
                Label2.Visible = True
                Button1.Enabled = False
            End If
        Catch
        End Try
    End Sub
End Class
