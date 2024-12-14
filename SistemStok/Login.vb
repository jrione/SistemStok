Imports BCrypt.Net
Imports Npgsql
Public Class Login
    Dim dbClient = New Koneksi
    Public Structure UserData
        Public Username As String
        Public Password As String
    End Structure
    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dbClient.Connect()
        Me.MaximizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.Text = "Login Aplikasi Manajemen Stok Digital"
        Me.Size = New Drawing.Size(500, 300)
        Me.StartPosition = FormStartPosition.CenterScreen


        With WelcomeLabel
            .Text = "Silakan Login"
            .Padding = New Padding(10)
            .Font = New Font("Arial", 12, FontStyle.Bold)
            .TextAlign = ContentAlignment.TopCenter
            .Dock = DockStyle.Fill
            .AutoSize = False
        End With

        With Username
            .Text = "Username"
            .Padding = New Padding(10, 0, 10, 10)
            .Font = New Font("Arial", 12)
            .Width = 200
            .Height = 30
            .Location = New Point((Me.ClientSize.Width - .Width) \ 2, (Me.ClientSize.Height - .Height) \ 2 - 40)
            .ForeColor = Color.Gray
        End With

        With Password
            .Text = "Password"
            .Padding = New Padding(10, 0, 10, 10)
            .Font = New Font("Arial", 12)
            .Width = 200
            .Height = 30
            .Location = New Point((Me.ClientSize.Width - .Width) \ 2, (Me.ClientSize.Height - .Height) \ 2)
            .PasswordChar = "*"c
            .ForeColor = Color.Gray
        End With

        With LoginBtn
            .Text = "Login"
            .Padding = New Padding(5)
            .Font = New Font("Arial", 12)
            .Width = 90
            .Height = 40
            .Location = New Point((Me.ClientSize.Width - .Width) \ 2, Password.Bottom + 10)
        End With

        AddHandler Username.GotFocus, AddressOf TextBox_Focus
        AddHandler Username.LostFocus, AddressOf TextBox_Lost

        AddHandler Password.GotFocus, AddressOf TextBox_Focus
        AddHandler Password.LostFocus, AddressOf TextBox_Lost

    End Sub

    Private Sub TextBox_Focus(sender As Object, e As EventArgs)
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        If textBox.ForeColor = Color.Gray Then
            textBox.Text = ""
            textBox.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBox_Lost(sender As Object, e As EventArgs)
        Dim textBox As TextBox = DirectCast(sender, TextBox)
        If String.IsNullOrWhiteSpace(textBox.Text) Then
            If textBox Is Username Then
                textBox.Text = "Username"
            ElseIf textBox Is Password Then
                textBox.Text = "Password"
            End If
            textBox.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub LoginBtn_Click(sender As Object, e As EventArgs) Handles LoginBtn.Click
        Dim loginData As UserData
        loginData.Username = Username.Text
        loginData.Password = Password.Text

        Dim passwordHash As String = dbClient.GetPasswordHash(loginData.Username)
        If passwordHash Is Nothing Then
            MessageBox.Show("Invalid Username/Password!")
            Return
        End If

        If BCrypt.Net.BCrypt.Verify(loginData.Password, passwordHash) Then
            Dim Dashboard As New Dashboard(dbClient, loginData)
            Me.Hide()
            Dashboard.ShowDialog()
            Me.Close()
        Else
            MessageBox.Show("Invalid Username/Password!")
        End If
    End Sub
    Public Function HashPassword(password As String) As String
        Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(password)
        Return hashedPassword
    End Function
End Class
