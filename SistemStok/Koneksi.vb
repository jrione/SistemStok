Imports Npgsql
Public Class Koneksi
    Private connString As String
    Public Function Connect() As NpgsqlConnection
        connString = "Server=127.0.0.1:5432;Database=stokdb;Userid=postgres;Password='admin'"
        Dim conn As New NpgsqlConnection(connString)
        Try
            conn.Open()
            Return conn
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetPasswordHash(username As String) As String
        Dim query As String = "SELECT password FROM users WHERE username = @username"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@username", username)

        Dim passwordHash As String = Nothing
        Try
            Dim reader As NpgsqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                passwordHash = reader.GetString(0)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try

        Return passwordHash
    End Function
End Class
