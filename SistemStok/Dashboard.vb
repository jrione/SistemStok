Imports System.Net
Imports Npgsql

Public Class Dashboard
    Dim dbClient As Koneksi
    Dim ObjClient As ObjectStorage
    Dim userData As Login.UserData
    Dim TinggiPanel As Integer = 800
    Dim PanjangPanel As Integer = 600

    Private WithEvents timer As New Timer()
    Private tgl As New Label()

    Public Structure Produk
        Public kode_barang As String
        Public nama_barang As String
        Public harga As Integer
        Public qty As Integer
        Public kategori
        Public img
    End Structure

    Dim loadingPanel As New Panel()
    Dim loadingLabel As New Label()

    Public Sub New(DBC As Koneksi, LD As Login.UserData)
        dbClient = DBC
        userData = LD
        ObjClient = New ObjectStorage()
        InitializeComponent()

        With Me
            .AutoSize = False
            .Text = "Dashboard - Aplikasi Manajemen Stok Digital"
            .Size = New Drawing.Size(TinggiPanel, PanjangPanel)
            .StartPosition = FormStartPosition.CenterScreen
            .MaximizeBox = True
            .FormBorderStyle = FormBorderStyle.Sizable
            .BackColor = Color.White
        End With

        loadingPanel.Dock = DockStyle.Fill
        loadingPanel.BackColor = Color.White

        loadingLabel.Text = "Mohon tunggu..."
        loadingLabel.Font = New Font("Arial", 14, FontStyle.Bold)
        loadingLabel.TextAlign = ContentAlignment.MiddleCenter
        loadingLabel.Dock = DockStyle.Fill

        loadingPanel.Controls.Add(loadingLabel)
        Me.Controls.Add(loadingPanel)

    End Sub

    Private Async Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadingPanel.Visible = True

        Dim data As List(Of Produk) = Await Task.Run(Function() dbClient.GetAllData())

        loadingPanel.Visible = False

        If data IsNot Nothing AndAlso data.Count > 0 Then
            DisplayData(data)
        Else
            MessageBox.Show("Data not found.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub DisplayData(data As List(Of Produk))
        Dim scrollablePanel As New Panel()
        With scrollablePanel
            .Dock = DockStyle.Fill
            .AutoScroll = True
            .BackColor = Color.White
        End With
        Me.Controls.Add(scrollablePanel)

        Dim contentTable As New TableLayoutPanel()
        With contentTable
            .Dock = DockStyle.Top
            .AutoSize = True
            .RowCount = 2
            .ColumnCount = 1
            .RowStyles.Add(New RowStyle(SizeType.AutoSize))
            .RowStyles.Add(New RowStyle(SizeType.AutoSize))
        End With
        scrollablePanel.Controls.Add(contentTable)

        Dim headerTable As New TableLayoutPanel()
        With headerTable
            .Dock = DockStyle.Top
            .RowCount = 1
            .ColumnCount = 2
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))
            .ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            .Padding = New Padding(10)
            .AutoSize = True
        End With

        Dim headerContentPanel As New FlowLayoutPanel()
        With headerContentPanel
            .Dock = DockStyle.Fill
            .FlowDirection = FlowDirection.TopDown
            .AutoSize = True
        End With

        Dim welcomeLabel As New Label()
        With welcomeLabel
            .Text = "Selamat datang di aplikasi, " & userData.Username & "!"
            .Font = New Font("Arial", 14, FontStyle.Bold)
            .AutoSize = True
            .TextAlign = ContentAlignment.MiddleLeft
            .Margin = New Padding(5, 20, 5, 5)
        End With
        headerContentPanel.Controls.Add(welcomeLabel)

        With tgl
            .Font = New Font("Arial", 10, FontStyle.Regular)
            .AutoSize = True
            .TextAlign = ContentAlignment.MiddleLeft
            .Margin = New Padding(5, 0, 5, 5)
        End With
        headerContentPanel.Controls.Add(tgl)

        ' Inisialisasi Timer
        timer.Interval = 1000 ' Set interval ke 1000 ms (1 detik)
        timer.Start() ' Mulai timer
        UpdateDateTime()

        Dim addButton As New Button()
        With addButton
            .Text = "Tambah Data Baru"
            .Font = New Font("Arial", 10, FontStyle.Bold)
            .AutoSize = True
            .BackColor = Color.LightGreen
            .Margin = New Padding(5, 15, 5, 5)
        End With
        AddHandler addButton.Click, AddressOf AddButton_Click

        Dim cashierButton As New Button()
        With cashierButton
            .Text = "Sistem Kasir"
            .Font = New Font("Arial", 10, FontStyle.Bold)
            .AutoSize = True
            .BackColor = Color.DarkOrange
            .Margin = New Padding(5, 15, 5, 5)
        End With
        AddHandler cashierButton.Click, AddressOf CashierSystem_Click

        Dim buttonPanel As New FlowLayoutPanel()
        With buttonPanel
            .FlowDirection = FlowDirection.LeftToRight
            .AutoSize = True
            .Margin = New Padding(0)
        End With

        buttonPanel.Controls.Add(addButton)
        buttonPanel.Controls.Add(cashierButton)
        headerContentPanel.Controls.Add(buttonPanel)


        headerTable.Controls.Add(headerContentPanel, 0, 0)

        Dim logoPictureBox As New PictureBox()
        With logoPictureBox
            .Image = My.Resources.AppLogo
            .SizeMode = PictureBoxSizeMode.Zoom
            .Width = 100
            .Height = 100
            .Dock = DockStyle.Right
            .Margin = New Padding(5, 10, 60, 5)
        End With
        headerTable.Controls.Add(logoPictureBox, 1, 0)

        contentTable.Controls.Add(headerTable, 0, 0)

        Dim flowPanel As New FlowLayoutPanel()
        With flowPanel
            .Dock = DockStyle.Top
            .AutoSize = True
            .FlowDirection = FlowDirection.TopDown
            .WrapContents = False
        End With

        If data IsNot Nothing AndAlso data.Count > 0 Then
            data = data.OrderBy(Function(x) x.kode_barang).ToList()
            For Each dt In data
                Dim imageUrl As String = "https://static.jri.one/assets/" + dt.img
                Dim webClient As New WebClient()

                Dim imageBytes As Byte() = webClient.DownloadData(imageUrl)

                Dim image As Image
                Using ms As New IO.MemoryStream(imageBytes)
                    image = Image.FromStream(ms)
                End Using

                Dim itemPanel As New FlowLayoutPanel()
                With itemPanel
                    .Height = 170
                    .Width = Me.ClientSize.Width - 40
                    .Margin = New Padding(10)
                    .FlowDirection = FlowDirection.LeftToRight
                    .BorderStyle = BorderStyle.FixedSingle
                End With

                Dim pictureBox As New PictureBox()
                pictureBox.Image = image
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage
                pictureBox.Width = 160
                pictureBox.Height = 160

                itemPanel.Controls.Add(pictureBox)

                Dim tablePanel As New TableLayoutPanel()
                With tablePanel
                    .BackColor = Color.White
                    .ColumnCount = 2
                    .RowCount = 5
                    .Height = 160
                    .Width = 350
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

                itemPanel.Controls.Add(tablePanel)

                Dim btnPanel As New FlowLayoutPanel()
                With btnPanel
                    .FlowDirection = FlowDirection.TopDown
                End With

                Dim btnEdit As New Button()
                With btnEdit
                    .Text = "Edit"
                    .Tag = dt.kode_barang
                End With

                AddHandler btnEdit.Click, AddressOf BtnEdit_Click
                btnPanel.Controls.Add(btnEdit)

                Dim btnDelete As New Button()
                With btnDelete
                    .Text = "Hapus"
                    .Tag = dt.kode_barang
                End With

                AddHandler btnDelete.Click, AddressOf BtnDelete_Click
                btnPanel.Controls.Add(btnDelete)

                itemPanel.Controls.Add(btnPanel)

                flowPanel.Controls.Add(itemPanel)
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
        contentTable.Controls.Add(flowPanel, 0, 1)
    End Sub

    Private Sub AddButton_Click(sender As Object, e As EventArgs)
        Dim addForm As New AddDataForm(dbClient, Me)
        addForm.ShowDialog()
    End Sub

    Private Sub CashierSystem_Click(sender As Object, e As EventArgs)
        'Dim cashierForm As New CashierSystem()
        'cashierForm.ShowDialog()
        Dim cashierForm As New CashierAdd(dbClient)
        cashierForm.ShowDialog()
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

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs)
        Dim button As Button = CType(sender, Button)
        Dim kodeBarang As String = button.Tag.ToString()
        Dim result = MessageBox.Show("Apakah Anda yakin ingin menghapus barang dengan kode: " & kodeBarang & "?",
                                     "Konfirmasi Hapus",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim isDeleted As Boolean = dbClient.Delete(kodeBarang)
            If isDeleted Then
                MessageBox.Show("Barang berhasil dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Controls.Clear()
                Dashboard_Load(Nothing, Nothing)
            Else
                MessageBox.Show("Gagal menghapus barang.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    Public Sub ReloadData()
        Me.Controls.Clear()
        Dashboard_Load(Nothing, Nothing)
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs)
        Dim button As Button = CType(sender, Button)
        Dim kodeBarang As String = button.Tag.ToString()
        Dim editForm As New EditDataForm(kodeBarang, dbClient, Me)
        editForm.ShowDialog()
    End Sub

    Private Sub UpdateDateTime()
        Dim tgld As DateTime = DateTime.Now
        tgl.Text = tgld.ToString("yyyy-MM-dd HH:mm:ss") ' Format tanggal dan waktu
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles timer.Tick
        UpdateDateTime() ' Perbarui tanggal dan waktu setiap detik
    End Sub
End Class
