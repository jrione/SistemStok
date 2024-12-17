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
            .MaximizeBox = False
            .FormBorderStyle = FormBorderStyle.FixedDialog
            .BackColor = Color.White
        End With
    End Sub

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim data As List(Of Produk) = dbClient.GetAllData()

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

            Me.Controls.Add(flowPanel)
            flowPanel.Dock = DockStyle.Fill
        Else
            MessageBox.Show("Data  not found.", "Ingpo", MessageBoxButtons.OK, MessageBoxIcon.Information)
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

End Class