Imports System.Xml
Imports SistemStok.Dashboard

Public Class EditDataForm
    Inherits Form

    Private kodeBarang As String
    Private dbClient As Koneksi

    ' Deklarasi kontrol
    Private nameLabel As Label
    Private nameTextBox As TextBox
    Private priceLabel As Label
    Private priceTextBox As TextBox
    Private qtyLabel As Label
    Private qtyTextBox As TextBox
    Private categoryLabel As Label
    Private categoryTextBox As TextBox
    Private imageLabel As Label
    Private imagePictureBox As PictureBox ' PictureBox untuk menampilkan gambar
    Private browseButton As Button ' Tombol untuk memilih gambar
    Private addButton As Button
    Private imagePath As String ' Untuk menyimpan path gambar yang dipilih
    Private loadingLabel As Label

    Public Sub New(kode As String, dbc As Koneksi)
        kodeBarang = kode
        dbClient = dbc
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        loadingLabel = New Label()
        loadingLabel.Text = "Mohon tunggu..."
        loadingLabel.Location = New Point(20, 300)
        loadingLabel.AutoSize = True
        loadingLabel.Visible = False

        ' Membuat Label dan TextBox untuk Nama Barang
        nameLabel = New Label()
        nameLabel.Text = "Nama Barang:"
        nameLabel.Location = New Point(20, 20)

        nameTextBox = New TextBox()
        nameTextBox.Location = New Point(120, 20)
        nameTextBox.Width = 200

        ' Membuat Label dan TextBox untuk Harga
        priceLabel = New Label()
        priceLabel.Text = "Harga:"
        priceLabel.Location = New Point(20, 60)

        priceTextBox = New TextBox()
        priceTextBox.Location = New Point(120, 60)
        priceTextBox.Width = 200

        ' Membuat Label dan TextBox untuk Jumlah
        qtyLabel = New Label()
        qtyLabel.Text = "Jumlah:"
        qtyLabel.Location = New Point(20, 100)

        qtyTextBox = New TextBox()
        qtyTextBox.Location = New Point(120, 100)
        qtyTextBox.Width = 200

        ' Membuat Label dan TextBox untuk Kategori
        categoryLabel = New Label()
        categoryLabel.Text = "Kategori:"
        categoryLabel.Location = New Point(20, 140)

        categoryTextBox = New TextBox()
        categoryTextBox.Location = New Point(120, 140)
        categoryTextBox.Width = 200

        ' Membuat Label dan PictureBox untuk Gambar
        imageLabel = New Label()
        imageLabel.Text = "Pilih Gambar:"
        imageLabel.Location = New Point(20, 180)

        imagePictureBox = New PictureBox()
        imagePictureBox.Location = New Point(120, 180)
        imagePictureBox.Size = New Size(100, 100)
        imagePictureBox.BorderStyle = BorderStyle.FixedSingle
        imagePictureBox.SizeMode = PictureBoxSizeMode.Zoom

        ' Tombol untuk memilih gambar
        'browseButton = New Button()
        'browseButton.Text = "Browse"
        'browseButton.Location = New Point(230, 200)
        'browseButton.Width = 90
        'AddHandler browseButton.Click, AddressOf BrowseButton_Click

        ' Membuat Tombol untuk Menambah Data
        'addButton = New Button()
        'addButton.Text = "Tambah Data"
        'addButton.Location = New Point(120, 300)
        'addButton.Width = 200
        'AddHandler addButton.Click, AddressOf AddButton_Click

        ' Menambahkan kontrol ke form
        Me.Controls.Add(nameLabel)
        Me.Controls.Add(nameTextBox)
        Me.Controls.Add(priceLabel)
        Me.Controls.Add(priceTextBox)
        Me.Controls.Add(qtyLabel)
        Me.Controls.Add(qtyTextBox)
        Me.Controls.Add(categoryLabel)
        Me.Controls.Add(categoryTextBox)
        Me.Controls.Add(imageLabel)
        Me.Controls.Add(imagePictureBox)
        'Me.Controls.Add(browseButton)
        'Me.Controls.Add(addButton)

        ' Pengaturan Form
        Me.Text = "Form Tambah Data"
        Me.Size = New Size(400, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Async Sub EditDataForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Tampilkan label "Mohon tunggu..."
            loadingLabel.Visible = True

            ' Ambil data produk secara asinkron
            Dim data As Produk = Await Task.Run(Function() dbClient.GetDataByKodeBarang(kodeBarang))

            ' Periksa apakah data tidak null
            'If data IsNot Nothing Then
            ' Isi kontrol dengan data produk
            nameTextBox.Text = data.nama_barang
            priceTextBox.Text = data.harga.ToString()
            qtyTextBox.Text = data.qty.ToString()
            categoryTextBox.Text = data.kategori
            'Else
            '    MessageBox.Show("Data tidak ditemukan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'End If
        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Sembunyikan label "Mohon tunggu..."
            loadingLabel.Visible = False
        End Try
    End Sub

End Class