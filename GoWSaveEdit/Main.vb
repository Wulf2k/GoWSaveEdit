Public Class Main


    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

  
  
    Private Sub btnSendMoney_Click(sender As Object, e As EventArgs) Handles btnSendMoney.Click
        System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=D7UD87LN43ERN")
    End Sub
End Class