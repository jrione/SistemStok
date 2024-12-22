Imports System.IO
Imports System.Diagnostics


Public Class AddDataForm
    Inherits Form

    Private nameLabel As Label
    Private nameTextBox As TextBox
    Private priceLabel As Label
    Private priceTextBox As TextBox
    Private qtyLabel As Label
    Private qtyTextBox As TextBox
    Private categoryLabel As Label
    Private categoryTextBox As TextBox
    Private imageLabel As Label
    Private imagePictureBox As PictureBox
    Private browseButton As Button
    Private addButton As Button

    Private imagePath As String
    Private imageName As String
    Private extName As String

    Private dbClient As Koneksi
    Private S3Client As ObjectStorage

    Private NewProduct As Dashboard.Produk
    Private dashboardForm As Dashboard

    Public Sub New(DBC As Koneksi, dashboard As Dashboard)
        dbClient = DBC
        S3Client = New ObjectStorage
        dashboardForm = dashboard
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
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
            Dim uuidnya As Guid = Guid.NewGuid()
            Dim uuidString As String = uuidnya.ToString()

            imagePath = openFileDialog.FileName
            'imageName = uuidString & Path.GetFileName(imagePath)
            imageName = uuidString & Path.GetExtension(imagePath)
            imagePictureBox.Image = Image.FromFile(imagePath)
        End If
    End Sub

    ' Event handler untuk tombol Tambah Data
    Private Sub AddButton_Click(sender As Object, e As EventArgs)
        ' Mengambil data dari kontrol
        With NewProduct
            .nama_barang = nameTextBox.Text
            .harga = priceTextBox.Text
            .qty = qtyTextBox.Text
            .kategori = categoryTextBox.Text
            .img = imageName
        End With

        ' Validasi input

        If String.IsNullOrEmpty(NewProduct.nama_barang) OrElse String.IsNullOrEmpty(NewProduct.harga) OrElse String.IsNullOrEmpty(NewProduct.qty) OrElse String.IsNullOrEmpty(NewProduct.kategori) OrElse String.IsNullOrEmpty(NewProduct.img) Then
            MessageBox.Show("Semua field harus diisi, termasuk gambar.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Mengonversi harga dan jumlah menjadi integer
        Dim parsedPrice As Integer
        Dim parsedQty As Integer
        If Not Integer.TryParse(NewProduct.harga, parsedPrice) OrElse Not Integer.TryParse(NewProduct.qty, parsedQty) Then
            MessageBox.Show("Harga dan Jumlah harus berupa angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim isAdded As Boolean = dbClient.Insert(NewProduct)

        If isAdded Then
            S3Client.UploadObject(imagePath, imageName)
            MessageBox.Show("Barang berhasil ditambah.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)

            If dashboardForm IsNot Nothing Then
                dashboardForm.ReloadData()
            End If

            Me.Close()

        Else
            MessageBox.Show("Gagal nemabah barang.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class
