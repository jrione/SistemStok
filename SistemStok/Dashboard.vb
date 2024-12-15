Imports Npgsql

Public Class Dashboard
    Dim dbClient = New Koneksi

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
        Public kode_barang As String
        Public nama_barang As String
        Public harga As Integer
        Public qty As Integer
        Public kategori
    End Structure

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim data As List(Of Produk) = dbClient.GetAllData()
        Dim yOffset As Integer = 10

        If data IsNot Nothing AndAlso data.Count > 0 Then
            For Each dt In data
                Dim panel As New Panel()
                panel.Size = New Drawing.Size(300, 120)
                panel.Location = New Drawing.Point(10, yOffset)

                Dim labelKode As New Label()
                labelKode.Text = $"Kode: {dt.kode_barang}"
                labelKode.AutoSize = True
                labelKode.Location = New Drawing.Point(10, 10)
                panel.Controls.Add(labelKode)

                Dim labelNama As New Label()
                labelNama.Text = $"Nama: {dt.nama_barang}"
                labelNama.AutoSize = True
                labelNama.Location = New Drawing.Point(10, 30)
                panel.Controls.Add(labelNama)

                Dim labelKategori As New Label()
                labelKategori.Text = $"Kategori: {dt.kategori}"
                labelKategori.AutoSize = True
                labelKategori.Location = New Drawing.Point(10, 50)
                panel.Controls.Add(labelKategori)

                Dim labelHarga As New Label()
                labelHarga.Text = $"Harga: {dt.harga}"
                labelHarga.AutoSize = True
                labelHarga.Location = New Drawing.Point(10, 70)
                panel.Controls.Add(labelHarga)

                Dim labelJumlah As New Label()
                labelJumlah.Text = $"Jumlah: {dt.qty}"
                labelJumlah.AutoSize = True
                labelJumlah.Location = New Drawing.Point(10, 90)
                panel.Controls.Add(labelJumlah)

                ' Add the panel to the form
                Me.Controls.Add(panel)

                ' Update the vertical position for the next panel
                yOffset += panel.Height + 10 ' Add some space between panels
            Next
        Else
            Console.WriteLine("No data found or an error occurred.")
        End If
    End Sub

End Class