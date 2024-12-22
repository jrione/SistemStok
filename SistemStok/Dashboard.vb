Imports System.Net
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
        Public img
    End Structure

    ' Panel untuk loading
    Dim loadingPanel As New Panel()
    Dim loadingLabel As New Label()

    Public Sub New(DBC As Koneksi, LD As Login.UserData)
        dbClient = DBC
        userData = LD
        InitializeComponent()

        With Me
            .AutoSize = False
            .Text = userData.Username
            .Size = New Drawing.Size(TinggiPanel, PanjangPanel)
            .StartPosition = FormStartPosition.CenterScreen
            .MaximizeBox = False
            .FormBorderStyle = FormBorderStyle.FixedDialog
            .BackColor = Color.White
        End With

        ' Konfigurasi loading panel
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
        ' Tampilkan panel loading
        loadingPanel.Visible = True

        ' Muat data secara asinkron
        Dim data As List(Of Produk) = Await Task.Run(Function() dbClient.GetAllData())

        ' Sembunyikan panel loading
        loadingPanel.Visible = False

        ' Tampilkan data
        If data IsNot Nothing AndAlso data.Count > 0 Then
            DisplayData(data)
        Else
            MessageBox.Show("Data not found.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub DisplayData(data As List(Of Produk))
        Dim flowPanel As New FlowLayoutPanel()
        With flowPanel
            .Dock = DockStyle.Fill
            .AutoScroll = True
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

                ' Buat panel untuk setiap item
                Dim itemPanel As New FlowLayoutPanel()
                With itemPanel
                    .Height = 170
                    .Width = Me.ClientSize.Width - 40
                    .Margin = New Padding(10)
                    .FlowDirection = FlowDirection.LeftToRight
                    .BorderStyle = BorderStyle.FixedSingle
                End With

                ' Tambahkan PictureBox untuk gambar
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

                ' Tambahkan label ke TableLayoutPanel
                AddLabelToTable(tablePanel, "Kode", dt.kode_barang, 0)
                AddLabelToTable(tablePanel, "Nama", dt.nama_barang, 1)
                AddLabelToTable(tablePanel, "Kategori", dt.kategori.ToString(), 2)
                AddLabelToTable(tablePanel, "Harga", "Rp. " & dt.harga.ToString("N0"), 3)
                AddLabelToTable(tablePanel, "Jumlah", dt.qty.ToString(), 4)

                ' Tambahkan TableLayoutPanel ke itemPanel
                itemPanel.Controls.Add(tablePanel)

                ' Button Panel
                Dim btnPanel As New FlowLayoutPanel()
                With btnPanel
                    .FlowDirection = FlowDirection.TopDown
                End With

                Dim btnEdit As New Button()
                With btnEdit
                    .Text = "Edit"
                End With

                Dim btnDelete As New Button()
                With btnDelete
                    .Text = "Hapus"
                    .Tag = dt.kode_barang
                End With

                btnPanel.Controls.Add(btnEdit)

                AddHandler btnDelete.Click, AddressOf BtnDelete_Click
                btnPanel.Controls.Add(btnDelete)

                itemPanel.Controls.Add(btnPanel)

                ' Tambahkan itemPanel ke flowPanel
                flowPanel.Controls.Add(itemPanel)
            Next

            Me.Controls.Add(flowPanel)
            flowPanel.Dock = DockStyle.Fill
        Else
            MessageBox.Show("Data not found.", "Ingpo", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
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
        ' Ambil tombol yang diklik
        Dim button As Button = CType(sender, Button)

        ' Ambil kode_barang dari Tag tombol
        Dim kodeBarang As String = button.Tag.ToString()

        ' Konfirmasi penghapusan
        Dim result = MessageBox.Show("Apakah Anda yakin ingin menghapus barang dengan kode: " & kodeBarang & "?",
                                     "Konfirmasi Hapus",
                                     MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' Logika penghapusan dari database
            Dim isDeleted As Boolean = dbClient.Delete(kodeBarang) ' Implementasikan logika Delete di Koneksi
            If isDeleted Then
                MessageBox.Show("Barang berhasil dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' Muat ulang data untuk memperbarui tampilan
                Me.Controls.Clear()
                Dashboard_Load(Nothing, Nothing)
            Else
                MessageBox.Show("Gagal menghapus barang.", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

End Class