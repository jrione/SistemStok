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
                .qty = reader("jumlah"),
                .img = reader("img_asset")
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

    Public Function Delete(kodeBarang As String) As Boolean
        Dim query As String = "DELETE FROM barang WHERE kodebarang = @kode"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)

        ' Tambahkan parameter untuk kode_barang
        cmd.Parameters.AddWithValue("@kode", kodeBarang)

        Try
            ' Eksekusi perintah DELETE
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

            ' Jika baris terpengaruh lebih dari 0, data berhasil dihapus
            Return rowsAffected > 0
        Catch ex As Exception
            ' Tampilkan pesan error jika terjadi kesalahan
            MsgBox("Error: " & ex.Message)
            Return False
        Finally
            ' Pastikan koneksi selalu ditutup
            conn.Close()
        End Try
    End Function

    Public Function GetDataByKodeBarang(kodeBarang As String) As Dashboard.Produk
        Dim query As String = "SELECT * FROM barang WHERE kodebarang = @kode"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)

        ' Mengikat parameter
        cmd.Parameters.AddWithValue("@kode", kodeBarang)

        Dim produk As New Dashboard.Produk()

        Try
            Dim reader As NpgsqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                produk.kode_barang = reader("kodebarang").ToString()
                produk.nama_barang = reader("namabarang").ToString()
                produk.kategori = reader("kategori").ToString()
                produk.harga = Convert.ToInt32(reader("harga")) ' Pastikan konversi tipe data sesuai
                produk.qty = Convert.ToInt32(reader("jumlah")) ' Pastikan konversi tipe data sesuai
                ' Jika ada kolom img_asset, pastikan Anda menambahkannya di struktur Produk
                produk.img = reader("img_asset").ToString() ' Pastikan kolom ini ada
            End If

            Return produk
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing ' Kembalikan Nothing jika terjadi kesalahan
        Finally
            conn.Close() ' Pastikan koneksi ditutup
        End Try
    End Function

End Class
