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

            Return result
        Catch ex As Exception
            MsgBox(ex.Message)
            Return New List(Of Dashboard.Produk)()
        Finally
            conn.Close()
        End Try
    End Function

    Public Function Delete(kodeBarang As String) As Boolean
        Dim query As String = "DELETE FROM barang WHERE kodebarang = @kode"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@kode", kodeBarang)

        Try
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            Return rowsAffected > 0
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
            Return False
        Finally
            conn.Close()
        End Try
    End Function

    Public Function Insert(data As Dashboard.Produk) As Boolean
        Dim query As String = "INSERT INTO barang (namabarang, kategori, harga, jumlah, img_asset) VALUES (@nama, @kategori, @harga, @jumlah, @img)"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@nama", data.nama_barang)
        cmd.Parameters.AddWithValue("@kategori", data.kategori)
        cmd.Parameters.AddWithValue("@harga", data.harga)
        cmd.Parameters.AddWithValue("@jumlah", data.qty)
        cmd.Parameters.AddWithValue("@img", data.img)

        Try
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            If (rowsAffected > 0) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
            Return False
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetDataByKodeBarang(kodeBarang As String) As Dashboard.Produk
        Dim query As String = "SELECT * FROM barang WHERE kodebarang = @kode"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)
        Dim produk As New Dashboard.Produk()

        cmd.Parameters.AddWithValue("@kode", kodeBarang)
        Try
            Dim reader As NpgsqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                produk.kode_barang = reader("kodebarang").ToString()
                produk.nama_barang = reader("namabarang").ToString()
                produk.kategori = reader("kategori").ToString()
                produk.harga = Convert.ToInt32(reader("harga"))
                produk.qty = Convert.ToInt32(reader("jumlah"))
                produk.img = reader("img_asset").ToString()
            End If

            Return produk
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        Finally
            conn.Close()
        End Try
    End Function

    Public Function UpdateProduct(EditProduct As Dashboard.Produk, kodeBarang As String) As Boolean
        Dim query As String = "UPDATE barang SET namabarang = @nama, kategori = @kategori, harga = @harga, jumlah = @jumlah WHERE kodebarang = @kode"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)

        cmd.Parameters.AddWithValue("@nama", EditProduct.nama_barang)
        cmd.Parameters.AddWithValue("@kategori", EditProduct.kategori)
        cmd.Parameters.AddWithValue("@harga", EditProduct.harga)
        cmd.Parameters.AddWithValue("@jumlah", EditProduct.qty)
        cmd.Parameters.AddWithValue("@kode", kodeBarang)
        Try
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            If (rowsAffected > 0) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
            Return False
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GetAllBarang()
        Dim query As String = "SELECT * FROM barang WHERE jumlah > 0"
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

            Return result
        Catch ex As Exception
            MsgBox(ex.Message)
            Return New List(Of Dashboard.Produk)()
        Finally
            conn.Close()
        End Try
    End Function

    Public Function AddTransaction(id_transaksi As Integer, products As List(Of CashierAdd.AddProduct)) As Boolean
        Dim conn As NpgsqlConnection = Connect()
        Dim transaction As NpgsqlTransaction = conn.BeginTransaction()

        Try
            ' Menyimpan data ke tabel transaksi
            Dim insertTransaksiQuery As String = "INSERT INTO transaksi (id_transaksi) VALUES (@id_transaksi)"
            Using cmd As New NpgsqlCommand(insertTransaksiQuery, conn)
                cmd.Transaction = transaction
                cmd.Parameters.AddWithValue("@id_transaksi", id_transaksi)
                cmd.ExecuteNonQuery()
            End Using

            ' Menyimpan data ke tabel transaksi_detail
            Dim insertDetailQuery As String = "INSERT INTO transaksi_detail (id_transaksi, kodebarang, jumlah, total_harga) VALUES (@id_transaksi, @kodebarang, @jumlah, @total_harga)"
            Using cmd As New NpgsqlCommand(insertDetailQuery, conn)
                cmd.Transaction = transaction

                For Each product As CashierAdd.AddProduct In products
                    Dim total_harga As Integer = product.qty * GetProductPrice(product.kode_barang) ' Menghitung total harga

                    cmd.Parameters.Clear() ' Menghapus parameter sebelumnya
                    cmd.Parameters.AddWithValue("@id_transaksi", id_transaksi)
                    cmd.Parameters.AddWithValue("@kodebarang", product.kode_barang)
                    cmd.Parameters.AddWithValue("@jumlah", product.qty)
                    cmd.Parameters.AddWithValue("@total_harga", total_harga)

                    cmd.ExecuteNonQuery()
                Next
            End Using

            transaction.Commit() ' Commit transaksi jika semua berhasil
            Return True
        Catch ex As Exception
            transaction.Rollback() ' Rollback jika terjadi kesalahan
            MsgBox(ex.Message)
            Return False
        Finally
            conn.Close()
        End Try
    End Function

    Private Function GetProductPrice(kode_barang As String) As Integer
        Dim query As String = "SELECT harga FROM barang WHERE kodebarang = @kodebarang"
        Dim conn As NpgsqlConnection = Connect()
        Dim cmd As New NpgsqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@kodebarang", kode_barang)

        Try
            Dim result As Object = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                Return Convert.ToInt32(result)
            Else
                Return 0
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return 0
        Finally
            conn.Close()
        End Try
    End Function

    Public Function GenerateTransactionId() As Integer
        Dim conn As NpgsqlConnection = Connect()
        Dim query As String = "SELECT COALESCE(MAX(id_transaksi), 0) + 1 FROM transaksi"
        Dim cmd As New NpgsqlCommand(query, conn)

        Try
            Dim result As Object = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                Return Convert.ToInt32(result)
            Else
                Return 1 ' Jika tidak ada transaksi, mulai dari 1
            End If
        Catch ex As Exception
            MsgBox("Terjadi kesalahan saat mengambil ID transaksi: " & ex.Message)
            Return 1 ' Kembali ke 1 jika terjadi kesalahan
        Finally
            conn.Close()
        End Try
    End Function

End Class
