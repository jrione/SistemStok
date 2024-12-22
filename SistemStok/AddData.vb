﻿Public Class AddDataForm
    Inherits Form

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

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
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
        browseButton = New Button()
        browseButton.Text = "Browse"
        browseButton.Location = New Point(230, 200)
        browseButton.Width = 90
        AddHandler browseButton.Click, AddressOf BrowseButton_Click

        ' Membuat Tombol untuk Menambah Data
        addButton = New Button()
        addButton.Text = "Tambah Data"
        addButton.Location = New Point(120, 300)
        addButton.Width = 200
        AddHandler addButton.Click, AddressOf AddButton_Click

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
        Me.Controls.Add(browseButton)
        Me.Controls.Add(addButton)

        ' Pengaturan Form
        Me.Text = "Form Tambah Data"
        Me.Size = New Size(400, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    ' Event handler untuk tombol Browse
    Private Sub BrowseButton_Click(sender As Object, e As EventArgs)
        ' Membuka dialog untuk memilih gambar
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp" ' Filter hanya gambar
        If openFileDialog.ShowDialog() = DialogResult.OK Then
            ' Menampilkan gambar yang dipilih di PictureBox
            imagePath = openFileDialog.FileName
            imagePictureBox.Image = Image.FromFile(imagePath)
        End If
    End Sub

    ' Event handler untuk tombol Tambah Data
    Private Sub AddButton_Click(sender As Object, e As EventArgs)
        ' Mengambil data dari kontrol
        Dim name As String = nameTextBox.Text
        Dim price As String = priceTextBox.Text
        Dim qty As String = qtyTextBox.Text
        Dim category As String = categoryTextBox.Text
        Dim image As String = imagePath ' Path gambar yang dipilih

        ' Validasi input
        If String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(price) OrElse String.IsNullOrEmpty(qty) OrElse String.IsNullOrEmpty(category) OrElse String.IsNullOrEmpty(image) Then
            MessageBox.Show("Semua field harus diisi, termasuk gambar.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Mengonversi harga dan jumlah menjadi integer
        Dim parsedPrice As Integer
        Dim parsedQty As Integer
        If Not Integer.TryParse(price, parsedPrice) OrElse Not Integer.TryParse(qty, parsedQty) Then
            MessageBox.Show("Harga dan Jumlah harus berupa angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Simulasi penambahan data
        MessageBox.Show($"Data berhasil ditambahkan: {name}, Harga: Rp. {parsedPrice}, Jumlah: {parsedQty}, Kategori: {category}, Gambar: {image}", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
