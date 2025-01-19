Imports System.Data.Common
Imports System.Net
Imports Npgsql

Public Class CashierSystem

    Public Structure Transaksi
        Public Property IDTransaksi As Integer
        Public Property NamaBarang As String
        Public Property JumlahBarang As Integer
        Public Property TotalHarga As Decimal
        Public Property TanggalTransaksi As DateTime
    End Structure

    Dim dbClient As Koneksi
    Dim userData As Login.UserData
    Private dataGridView As DataGridView

    Public Sub New(DBC As Koneksi, LD As Login.UserData)
        dbClient = DBC
        userData = LD
        InitializeComponent()
        InitializeDashboard()
    End Sub

    Private Sub InitializeDashboard()
        Me.Text = "Dashboard Sistem Kasir - Aplikasi Manajemen Stok Digital"
        Me.Size = New Drawing.Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' Panel untuk tombol
        Dim buttonPanel As New Panel With {
            .Dock = DockStyle.Top, ' Panel ini akan berada di atas
            .Height = 50, ' Menentukan tinggi panel tombol
            .Padding = New Padding(10) ' Menambahkan padding di sekitar tombol
        }

        ' Tombol Tambah Transaksi
        Dim addButton As New Button()
        With addButton
            .Text = "Tambah Transaksi"
            .Font = New Font("Arial", 10, FontStyle.Bold)
            .AutoSize = True
            .BackColor = Color.LightGreen
            .Margin = New Padding(5, 15, 5, 5)
        End With
        AddHandler addButton.Click, AddressOf AddButton_Click

        ' Tombol Dashboard
        Dim dashboardButton As New Button()
        With dashboardButton
            .Text = "Dashboard"
            .Font = New Font("Arial", 10, FontStyle.Bold)
            .AutoSize = True
            .BackColor = Color.LightGreen
            .Margin = New Padding(5, 15, 5, 5)
        End With
        AddHandler dashboardButton.Click, AddressOf DashboardButton_Click

        ' Menambahkan tombol ke panel
        buttonPanel.Controls.Add(addButton)
        buttonPanel.Controls.Add(dashboardButton)

        ' Menambahkan panel ke form
        Me.Controls.Add(buttonPanel)

        ' DataGridView untuk menampilkan transaksi
        dataGridView = New DataGridView With {
            .Dock = DockStyle.Fill, ' Mengisi seluruh form
            .AutoGenerateColumns = False,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .Font = New Font("Arial", 10, FontStyle.Regular), ' Mengatur jenis dan ukuran font
            .BorderStyle = BorderStyle.Fixed3D, ' Mengatur jenis border
            .GridColor = Color.Gray, ' Mengatur warna grid
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.White,
                .ForeColor = Color.Black,
                .SelectionBackColor = Color.LightBlue,
                .SelectionForeColor = Color.Black,
                .Font = New Font("Arial", 10, FontStyle.Regular) ' Font untuk sel
            },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.Navy,
                .ForeColor = Color.White,
                .Font = New Font("Arial", 10) ' Font untuk header kolom
            }
        }

        ' Menambahkan kolom ke DataGridView
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "IDTransaksi", .HeaderText = "ID Transaksi", .DataPropertyName = "IDTransaksi", .FillWeight = 20})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NamaBarang", .HeaderText = "Nama Barang", .DataPropertyName = "NamaBarang", .FillWeight = 40})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "JumlahBarang", .HeaderText = "Jumlah Barang", .DataPropertyName = "JumlahBarang", .FillWeight = 20})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TotalHarga", .HeaderText = "Total Harga", .DataPropertyName = "TotalHarga", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "C"}, .FillWeight = 20})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TanggalTransaksi", .HeaderText = "Tanggal Transaksi", .DataPropertyName = "TanggalTransaksi", .FillWeight = 20})

        ' Panel untuk menampung DataGridView
        Dim containerPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(10) ' Menambahkan padding di semua sisi
        }

        ' Menambahkan DataGridView ke panel
        containerPanel.Controls.Add(dataGridView)

        ' Menambahkan panel ke form
        Me.Controls.Add(containerPanel)
    End Sub

    Private Async Sub CashierSystem_Click(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Mengambil data transaksi dan menampilkannya ke DataGridView
        Dim data = Await Task.Run(Function() dbClient.GetAllTransactions())
        If data IsNot Nothing Then
            Dim bindingList = New BindingSource(data, Nothing)
            dataGridView.DataSource = bindingList
        Else
            MessageBox.Show("Tidak ada transaksi yang ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub DashboardButton_Click(sender As Object, e As EventArgs)
        ' Membuka form Dashboard
        Dim dashboardForm As New Dashboard(dbClient, userData)
        dashboardForm.ShowDialog()
    End Sub

    ' Event untuk tombol Add Transaksi
    Private Sub AddButton_Click(sender As Object, e As EventArgs)
        ' Membuka form untuk menambahkan transaksi, bukan Dashboard
        Dim dashboardForm As New Dashboard(dbClient, userData)
        dashboardForm.ShowDialog()
    End Sub

End Class
