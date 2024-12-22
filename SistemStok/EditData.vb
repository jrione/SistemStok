Imports System.Xml
Imports SistemStok.Dashboard

Public Class EditDataForm
    Inherits Form

    Private kodeBarang As String
    Private dbClient As Koneksi

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
    Private editButton As Button
    Private imagePath As String
    Private loadingLabel As Label

    Private EditProduct As Dashboard.Produk
    Private dashboardForm As Dashboard

    Public Sub New(kode As String, dbc As Koneksi, dashboard As Dashboard)
        kodeBarang = kode
        dbClient = dbc
        dashboardForm = dashboard
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        loadingLabel = New Label()
        loadingLabel.Text = "Mohon tunggu..."
        loadingLabel.Location = New Point(20, 300)
        loadingLabel.AutoSize = True
        loadingLabel.Visible = False

        nameLabel = New Label()
        nameLabel.Text = "Nama Barang:"
        nameLabel.Location = New Point(20, 20)

        nameTextBox = New TextBox()
        nameTextBox.Location = New Point(120, 20)
        nameTextBox.Width = 200

        priceLabel = New Label()
        priceLabel.Text = "Harga:"
        priceLabel.Location = New Point(20, 60)

        priceTextBox = New TextBox()
        priceTextBox.Location = New Point(120, 60)
        priceTextBox.Width = 200

        qtyLabel = New Label()
        qtyLabel.Text = "Jumlah:"
        qtyLabel.Location = New Point(20, 100)

        qtyTextBox = New TextBox()
        qtyTextBox.Location = New Point(120, 100)
        qtyTextBox.Width = 200

        categoryLabel = New Label()
        categoryLabel.Text = "Kategori:"
        categoryLabel.Location = New Point(20, 140)

        categoryTextBox = New TextBox()
        categoryTextBox.Location = New Point(120, 140)
        categoryTextBox.Width = 200

        editButton = New Button()
        editButton.Text = "Tambah Data"
        editButton.Location = New Point(120, 300)
        editButton.Width = 200
        AddHandler editButton.Click, AddressOf EditButton_Click

        Me.Controls.Add(nameLabel)
        Me.Controls.Add(nameTextBox)
        Me.Controls.Add(priceLabel)
        Me.Controls.Add(priceTextBox)
        Me.Controls.Add(qtyLabel)
        Me.Controls.Add(qtyTextBox)
        Me.Controls.Add(categoryLabel)
        Me.Controls.Add(categoryTextBox)
        Me.Controls.Add(editButton)

        Me.Text = "Form Edit Data"
        Me.Size = New Size(400, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Private Async Sub EditDataForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            loadingLabel.Visible = True
            Dim data As Produk = Await Task.Run(Function() dbClient.GetDataByKodeBarang(kodeBarang))

            nameTextBox.Text = data.nama_barang
            priceTextBox.Text = data.harga.ToString()
            qtyTextBox.Text = data.qty.ToString()
            categoryTextBox.Text = data.kategori
        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            loadingLabel.Visible = False
        End Try
    End Sub

    Private Sub EditButton_Click(sender As Object, e As EventArgs)
        With EditProduct
            .nama_barang = nameTextBox.Text
            .harga = priceTextBox.Text
            .qty = qtyTextBox.Text
            .kategori = categoryTextBox.Text
        End With

        If String.IsNullOrEmpty(EditProduct.nama_barang) OrElse String.IsNullOrEmpty(EditProduct.harga) OrElse String.IsNullOrEmpty(EditProduct.qty) OrElse String.IsNullOrEmpty(EditProduct.kategori) Then
            MessageBox.Show("Semua field harus diisi, termasuk gambar.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim parsedPrice As Integer
        Dim parsedQty As Integer
        If Not Integer.TryParse(EditProduct.harga, parsedPrice) OrElse Not Integer.TryParse(EditProduct.qty, parsedQty) Then
            MessageBox.Show("Harga dan Jumlah harus berupa angka.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim isEdited As Boolean = dbClient.UpdateProduct(EditProduct, kodeBarang)

        If isEdited Then
            MessageBox.Show("Barang berhasil diedit.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            If dashboardForm IsNot Nothing Then
                dashboardForm.ReloadData()
            End If
            Me.Close()
        Else
            MessageBox.Show("Gagal nemabah barang.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

End Class