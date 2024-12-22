Imports Npgsql

Public Class Dashboard
    Dim dbClient As Koneksi
    Dim userData As Login.UserData
    Dim TinggiPanel As Integer = 800
    Dim PanjangPanel As Integer = 600

    Structure Produk
        Public kode_barang As String
        Public nama_barang As String
        Public harga As Integer
        Public qty As Integer
        Public kategori
    End Structure

    Public Sub New(DBC As Koneksi, LD As Login.UserData)
        dbClient = DBC
        userData = LD
        InitializeComponent()

        With Me
            .AutoSize = False
            .Text = userData.Username
            .Size = New Drawing.Size(TinggiPanel, PanjangPanel)
            .StartPosition = FormStartPosition.CenterScreen
            .MaximizeBox = True ' Memungkinkan pengguna untuk mengubah ukuran jendela
            .FormBorderStyle = FormBorderStyle.Sizable
            .BackColor = Color.White
        End With
    End Sub

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Panel utama yang dapat discroll
        Dim scrollablePanel As New Panel()
        With scrollablePanel
            .Dock = DockStyle.Fill
            .AutoScroll = True
            .BackColor = Color.White
        End With
        Me.Controls.Add(scrollablePanel)

        ' TableLayoutPanel untuk seluruh konten
        Dim contentTable As New TableLayoutPanel()
        With contentTable
            .Dock = DockStyle.Top
            .AutoSize = True
            .RowCount = 2
            .ColumnCount = 1
            .RowStyles.Add(New RowStyle(SizeType.AutoSize)) ' Untuk header
            .RowStyles.Add(New RowStyle(SizeType.AutoSize)) ' Untuk data tabel
        End With
        scrollablePanel.Controls.Add(contentTable)

        ' TableLayoutPanel untuk header
        Dim headerTable As New TableLayoutPanel()
        With headerTable
            .Dock = DockStyle.Top
            .RowCount = 1
            .ColumnCount = 2
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60)) ' Kolom untuk sambutan dan tombol
            .ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize)) ' Kolom untuk logo
            .Padding = New Padding(10)
            .AutoSize = True
        End With

        ' Panel untuk sambutan dan tombol
        Dim headerContentPanel As New FlowLayoutPanel()
        With headerContentPanel
            .Dock = DockStyle.Fill
            .FlowDirection = FlowDirection.TopDown
            .AutoSize = True
        End With

        ' Label sambutan
        Dim welcomeLabel As New Label()
        With welcomeLabel
            .Text = "Selamat datang di aplikasi, " & userData.Username & "!"
            .Font = New Font("Arial", 14, FontStyle.Bold)
            .AutoSize = True
            .TextAlign = ContentAlignment.MiddleLeft
            .Margin = New Padding(5, 20, 5, 5)
        End With
        headerContentPanel.Controls.Add(welcomeLabel)

        ' Tombol tambah data baru
        Dim addButton As New Button()
        With addButton
            .Text = "Tambah Data Baru"
            .Font = New Font("Arial", 10, FontStyle.Bold)
            .AutoSize = True
            .BackColor = Color.LightGreen
            .Margin = New Padding(5, 15, 5, 5)
        End With
        AddHandler addButton.Click, AddressOf AddButton_Click
        headerContentPanel.Controls.Add(addButton)

        ' Tambahkan headerContentPanel ke headerTable
        headerTable.Controls.Add(headerContentPanel, 0, 0)

        ' PictureBox untuk logo
        Dim logoPictureBox As New PictureBox()
        With logoPictureBox
            .Image = My.Resources.AppLogo ' Ganti dengan nama gambar di Resources Anda
            .SizeMode = PictureBoxSizeMode.Zoom
            .Width = 100
            .Height = 100
            .Dock = DockStyle.Right
            .Margin = New Padding(5, 10, 60, 5)
        End With
        headerTable.Controls.Add(logoPictureBox, 1, 0)

        ' Tambahkan headerTable ke contentTable
        contentTable.Controls.Add(headerTable, 0, 0)

        ' Panel untuk daftar data produk
        Dim flowPanel As New FlowLayoutPanel()
        With flowPanel
            .Dock = DockStyle.Top
            .AutoSize = True
            .FlowDirection = FlowDirection.TopDown
            .WrapContents = False
        End With

        ' Mendapatkan data produk
        Dim data As List(Of Produk) = dbClient.GetAllData()

        If data IsNot Nothing AndAlso data.Count > 0 Then
            data = data.OrderBy(Function(x) x.kode_barang).ToList()
            For Each dt In data
                Dim tablePanel As New TableLayoutPanel()
                With tablePanel
                    .AutoSizeMode = AutoSizeMode.GrowAndShrink
                    .BackColor = Color.White
                    .BorderStyle = BorderStyle.FixedSingle
                    .ColumnCount = 2
                    .RowCount = 5
                    .Width = Me.ClientSize.Width - 40
                    .Height = 150
                    .Margin = New Padding(10)
                    .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0F))
                    .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70.0F))
                    For i As Integer = 0 To .RowCount - 1
                        .RowStyles.Add(New RowStyle(SizeType.Absolute, 30.0F))
                    Next
                End With

                AddLabelToTable(tablePanel, "Kode", dt.kode_barang, 0)
                AddLabelToTable(tablePanel, "Nama", dt.nama_barang, 1)
                AddLabelToTable(tablePanel, "Kategori", dt.kategori.ToString(), 2)
                AddLabelToTable(tablePanel, "Harga", "Rp. " & dt.harga.ToString("N0"), 3)
                AddLabelToTable(tablePanel, "Jumlah", dt.qty.ToString(), 4)

                flowPanel.Controls.Add(tablePanel)
            Next
        Else
            Dim noDataLabel As New Label()
            With noDataLabel
                .Text = "Data tidak ditemukan."
                .Font = New Font("Arial", 12, FontStyle.Italic)
                .ForeColor = Color.Gray
                .TextAlign = ContentAlignment.MiddleCenter
                .Dock = DockStyle.Fill
            End With
            flowPanel.Controls.Add(noDataLabel)
        End If

        ' Tambahkan flowPanel ke contentTable
        contentTable.Controls.Add(flowPanel, 0, 1)
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs)
        Dim addForm As New AddDataForm()
        addForm.ShowDialog() ' Menampilkan form sebagai dialog
    End Sub

    Private Sub AddLabelToTable(tablePanel As TableLayoutPanel, key As String, value As String, row As Integer)
        Dim labelKey As New Label() With {
            .Text = key & ":",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft
        }

        Dim labelValue As New Label() With {
            .Text = ": " & value,
            .Font = New Font("Arial", 10, FontStyle.Regular),
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft
        }
        tablePanel.Controls.Add(labelKey, 0, row)
        tablePanel.Controls.Add(labelValue, 1, row)
    End Sub

End Class
