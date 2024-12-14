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
End Class
