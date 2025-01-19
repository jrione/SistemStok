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

    Public Function GetAllTransactions() As List(Of CashierSystem.Transaksi)
        Dim transactions As New List(Of CashierSystem.Transaksi)
        ' Implementasikan query database untuk mendapatkan data transaksi.
        Return transactions
    End Function


End Class
