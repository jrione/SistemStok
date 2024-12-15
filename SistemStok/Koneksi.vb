Imports System.Text
Imports Npgsql
Public Class Koneksi
    Public Function Connect() As NpgsqlConnection
        Dim connString As String = Environment.GetEnvironmentVariable("CONNSTRING")
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

    Public Function GetAllData() As List(Of Dashboard.Produk)
        Dim query As String = "SELECT * FROM barang"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)

        Dim result As New List(Of Dashboard.Produk)()

        Try
            Dim reader As NpgsqlDataReader = cmd.ExecuteReader()

            While reader.Read()
                Dim produk As New Dashboard.Produk() With {
                .kode_barang = reader("kodebarang").ToString(),
                .nama_barang = reader("namabarang").ToString(),
                .kategori = reader("kategori").ToString(),
                .harga = reader("harga"),
                .qty = reader("jumlah")
            }
                result.Add(produk)
            End While

            ' Optionally, you can return the result as a string
            Return result
        Catch ex As Exception
            MsgBox(ex.Message)
            Return New List(Of Dashboard.Produk)()
        Finally
            conn.Close() ' Ensure the connection is closed
        End Try
    End Function
End Class
