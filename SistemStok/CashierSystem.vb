Imports System.Net
Imports Npgsql

Public Class CashierSystem

    Public Structure Transaksi
        Public Property IDTransaksi As Integer
        Public Property NamaBarang As String
        Public Property JumlahBarang As Integer
        Public Property TotalHarga As Decimal
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
        Me.Text = "Sistem Kasir - Aplikasi Manajemen Stok Digital"
        Me.Size = New Drawing.Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen

        ' DataGridView untuk menampilkan transaksi
        dataGridView = New DataGridView With {
            .Dock = DockStyle.Fill,
            .AutoGenerateColumns = False,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        }

        ' Tambahkan kolom ke DataGridView
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "IDTransaksi", .HeaderText = "ID Transaksi", .DataPropertyName = "IDTransaksi"})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NamaBarang", .HeaderText = "Nama Barang", .DataPropertyName = "NamaBarang"})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "JumlahBarang", .HeaderText = "Jumlah Barang", .DataPropertyName = "JumlahBarang"})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TotalHarga", .HeaderText = "Total Harga", .DataPropertyName = "TotalHarga", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "C"}})

        Me.Controls.Add(dataGridView)
    End Sub

    Private Async Sub CashierSystem_Click(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim data = Await Task.Run(Function() dbClient.GetAllTransactions())
        If data IsNot Nothing Then
            Dim bindingList = New BindingSource(data, Nothing)
            dataGridView.DataSource = bindingList
        Else
            MessageBox.Show("Tidak ada transaksi yang ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

End Class