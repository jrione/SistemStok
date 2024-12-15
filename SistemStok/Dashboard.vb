Imports Npgsql

Public Class Dashboard
    Public Sub New(dbClient As Koneksi, loginData As Login.UserData)
        InitializeComponent()
        Me.Text = loginData.Username
        Me.Size = New Drawing.Size(1080, 900)
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub
    Enum Kategori
        Pakaian
        Buku
        Perabot
    End Enum
    Structure Produk
        Public kode_barang As Integer
        Public nama_barang As String
        Public harga As Integer
        Public qty As Integer
        Public kategori As Kategori
    End Structure

End Class