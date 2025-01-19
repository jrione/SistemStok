Imports System.Xml

Public Class CashierAdd

    Public Structure AddProduct
        Public kode_barang As String
        Public product As String
        Public qty As Integer
    End Structure

    Dim dbClient As Koneksi
    Dim userData As Login.UserData
    Private productsList As New List(Of AddProduct)()

    Private productLabel As New Label()
    Private productComboBox As New ComboBox()

    Private qtyLabel As New Label()
    Private qtyTextBox As New TextBox()

    Private addButton As New Button()
    Private finishButton As New Button()
    Private productsListBox As New ListBox() ' ListBox untuk menampilkan daftar produk

    Public Sub New(DBC As Koneksi, LD As Login.UserData)
        dbClient = DBC
        userData = LD
        Me.Text = "Tambah Data"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        Init()
    End Sub

    Private Sub Init()
        With productLabel
            .Text = "Produk :"
            .Location = New Point(20, 20)
        End With
        Me.Controls.Add(productLabel)

        With productComboBox
            .Location = New Point(120, 20)
            .Size = New Size(200, 30)
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With
        Me.Controls.Add(productComboBox)

        With qtyLabel
            .Text = "Jumlah :"
            .Location = New Point(20, 50)
        End With
        Me.Controls.Add(qtyLabel)

        With qtyTextBox
            .Width = 200
            .Location = New Point(120, 50)
        End With
        Me.Controls.Add(qtyTextBox)

        With addButton
            .Text = "Tambah"
            .Location = New Point(20, 90)
        End With
        AddHandler addButton.Click, AddressOf AddButton_Click ' Menangani event Click untuk addButton
        Me.Controls.Add(addButton)

        With finishButton
            .Text = "Selesai"
            .Location = New Point(100, 90)
        End With
        AddHandler finishButton.Click, AddressOf FinishButton_Click
        Me.Controls.Add(finishButton)

        ' Inisialisasi ListBox
        With productsListBox
            .Location = New Point(20, 130)
            .Size = New Size(740, 400)
        End With
        Me.Controls.Add(productsListBox)
    End Sub

    Private Async Sub CashierAdd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim data As List(Of Dashboard.Produk) = Await Task.Run(Function() dbClient.GetAllBarang())

            productComboBox.Items.Clear()
            For Each produk As Dashboard.Produk In data
                productComboBox.Items.Add(produk.kode_barang & " - " & produk.nama_barang & " (" & produk.qty & ")")
            Next

            If productComboBox.Items.Count > 0 Then
                productComboBox.SelectedIndex = 0
            End If
        Catch ex As Exception
            MessageBox.Show("Terjadi kesalahan saat mengambil data: " & ex.Message)
        End Try
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs)
        Dim qty As Integer
        If Integer.TryParse(qtyTextBox.Text, qty) AndAlso qty > 0 Then
            Dim selectedProduct As String = productComboBox.SelectedItem.ToString()
            Dim kodeBarang As String = selectedProduct.Split("-"c)(0)

            Dim newProduct As New AddProduct()
            newProduct.kode_barang = kodeBarang.Trim()
            newProduct.product = selectedProduct
            newProduct.qty = qty

            productsList.Add(newProduct)

            qtyTextBox.Clear()

            UpdateProductsListBox()
        Else
            MessageBox.Show("Masukkan jumlah yang valid.")
        End If
    End Sub

    Private Sub UpdateProductsListBox()
        productsListBox.Items.Clear() ' Mengosongkan ListBox sebelum memperbarui
        For Each product As AddProduct In productsList
            productsListBox.Items.Add(product.kode_barang & " - Jumlah: " & product.qty)
        Next
    End Sub

    Private Sub FinishButton_Click(sender As Object, e As EventArgs)
        ' Misalkan kita menggunakan ID transaksi yang dihasilkan secara otomatis
        Dim id_transaksi As Integer = dbClient.GenerateTransactionId() ' Ganti dengan metode untuk menghasilkan ID transaksi

        If dbClient.AddTransaction(id_transaksi, productsList) Then
            MessageBox.Show("Transaksi Sukses!.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Dim CashierDashboard As New CashierSystem(dbClient, userData)
            Me.Hide()
            CashierDashboard.ShowDialog()
            Me.Close()
        Else
            MessageBox.Show("Terjadi kesalahan saat menyimpan transaksi.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

End Class