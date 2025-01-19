Imports System.Data.Common
Imports System.Net
Imports Npgsql

Public Class CashierSystem

    Public Structure Transaksi
        Public Property IDTransaksi As Integer
        Public Property NamaBarang As String
        Public Property JumlahBarang As Integer
        Public Property TotalHarga As Decimal
        Public Property TanggalTransaksi As String
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

        Dim scrollablePanel As New Panel()
        With scrollablePanel
            .Dock = DockStyle.Fill
            .AutoScroll = True
            .BackColor = Color.White
        End With
        Me.Controls.Add(scrollablePanel)

        Dim headerContentPanel As New FlowLayoutPanel()
        With headerContentPanel
            .Dock = DockStyle.Fill
            .FlowDirection = FlowDirection.TopDown
            .AutoSize = True
        End With

        Dim buttonPanel As New FlowLayoutPanel()
        With buttonPanel
            .FlowDirection = FlowDirection.LeftToRight
            .AutoSize = True
            .Margin = New Padding(0)
        End With

        Dim addButton As New Button()
        With addButton
            .Text = "Tambah Transaksi"
            .Font = New Font("Arial", 10, FontStyle.Bold)
            .AutoSize = True
            .BackColor = Color.LightGreen
            .Margin = New Padding(5, 15, 5, 5)
        End With
        AddHandler addButton.Click, AddressOf AddButton_Click

        Dim dashboardButton As New Button()
        With dashboardButton
            .Text = "Dashboard"
            .Font = New Font("Arial", 10, FontStyle.Bold)
            .AutoSize = True
            .BackColor = Color.LightGreen
            .Margin = New Padding(5, 15, 5, 5)
        End With
        AddHandler dashboardButton.Click, AddressOf DashboardButton_Click

        buttonPanel.Controls.Add(addButton)
        buttonPanel.Controls.Add(dashboardButton)
        headerContentPanel.Controls.Add(buttonPanel)


        dataGridView = New DataGridView With {
            .Dock = DockStyle.Fill, ' Mengisi seluruh form
            .AutoGenerateColumns = False,
            .RowHeadersVisible = False,
            .ReadOnly = True,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .Font = New Font("Arial", 10, FontStyle.Regular),
            .Height = Height - 100,
            .BorderStyle = BorderStyle.Fixed3D,
            .GridColor = Color.White,
            .DefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.White,
                .ForeColor = Color.Black,
                .SelectionBackColor = Color.LightBlue,
                .SelectionForeColor = Color.Black,
                .Font = New Font("Arial", 10, FontStyle.Regular)
            },
            .ColumnHeadersDefaultCellStyle = New DataGridViewCellStyle With {
                .BackColor = Color.Navy,
                .ForeColor = Color.White,
                .Font = New Font("Arial", 10) ' Font untuk header kolom
            }
        }

        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "IDTransaksi", .HeaderText = "ID Transaksi", .DataPropertyName = "IDTransaksi", .FillWeight = 10})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NamaBarang", .HeaderText = "Nama Barang", .DataPropertyName = "NamaBarang", .FillWeight = 25})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "JumlahBarang", .HeaderText = "Jumlah Barang", .DataPropertyName = "JumlahBarang", .FillWeight = 15})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TotalHarga", .HeaderText = "Total Harga", .DataPropertyName = "TotalHarga", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "Rp #,###", .Alignment = DataGridViewContentAlignment.MiddleRight}, .FillWeight = 25})
        dataGridView.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TanggalTransaksi", .HeaderText = "Tanggal Transaksi", .DataPropertyName = "TanggalTransaksi", .FillWeight = 25})

        Dim containerPanel As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(10)
        }
        containerPanel.Controls.Add(dataGridView)


        Dim contentTable As New TableLayoutPanel()
        With contentTable
            .Dock = DockStyle.Fill
            .AutoSize = True
            .RowCount = 2
            .ColumnCount = 1
            .RowStyles.Add(New RowStyle(SizeType.AutoSize))
            .RowStyles.Add(New RowStyle(SizeType.AutoSize))
        End With
        scrollablePanel.Controls.Add(contentTable)

        contentTable.Controls.Add(headerContentPanel)
        contentTable.Controls.Add(containerPanel)
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

    Private Sub DashboardButton_Click(sender As Object, e As EventArgs)
        Dim dashboardForm As New Dashboard(dbClient, userData)
        dashboardForm.ShowDialog()
    End Sub
    Private Sub AddButton_Click(sender As Object, e As EventArgs)
        Dim dashboardForm As New Dashboard(dbClient, userData)
        dashboardForm.ShowDialog()
    End Sub

End Class
