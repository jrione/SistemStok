Imports Npgsql

Public Class Dashboard
    Dim dbClient = New Koneksi
    Dim loginData As Login.UserData
    Dim TinggiPanel As Integer = 1080
    Dim PanjangPanel As Integer = 720
    Public Sub New(dbc As Koneksi, LD As Login.UserData)
        InitializeComponent()
        dbClient = dbc
        loginData = LD
    End Sub
    Enum Kategori
        Pakaian
        Buku
        Perabot
    End Enum
    Structure Produk
        Public kode_barang As String
        Public nama_barang As String
        Public harga As Integer
        Public qty As Integer
        Public kategori
    End Structure

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With Me
            .AutoSize = False
            .Text = loginData.Username
            .Size = New Drawing.Size(TinggiPanel, PanjangPanel)
            .StartPosition = FormStartPosition.CenterScreen
            .MaximizeBox = False
            .FormBorderStyle = FormBorderStyle.FixedDialog
            .BackColor = Color.White
        End With


        Dim tabControl As New TabControl()
        With tabControl
            .Size = New Size(Me.ClientSize.Width - 5, Me.ClientSize.Height - 5)
            .Location = New Point(5, 0)
            .BackColor = Color.LightGray
            .ItemSize = New Size(100, 30)
            .SizeMode = TabSizeMode.Fixed
        End With

        Dim scrollablePanel As New Panel()
        With scrollablePanel
            .Size = New Size(tabControl.ClientSize.Width - 10, tabControl.ClientSize.Height - 50)
            .Location = New Point(5, 5)
            .AutoScroll = True
        End With

        Dim AllStockPage As New TabPage("All Stock")
        AllStockPage.Controls.Add(scrollablePanel)


        Dim NewPage As New TabPage("NEWWWW")
        Dim scrollablePanel2 As New Panel()
        scrollablePanel2.Size = New Size(300, 200)
        scrollablePanel2.Location = New Point(10, 10)
        scrollablePanel2.AutoScroll = True
        For i As Integer = 1 To 20
            Dim label As New Label()
            label.Text = "Label " & i.ToString()
            label.Location = New Point(10, (i - 1) * 30)
            scrollablePanel2.Controls.Add(label)
        Next
        NewPage.Controls.Add(scrollablePanel2)

        With tabControl.TabPages
            .Add(AllStockPage)
            .Add(NewPage)
        End With

        'Memunculkan Data All Stock
        Dim data As List(Of Produk) = dbClient.GetAllData()
        Dim yOffset As Integer = 20
        If data IsNot Nothing AndAlso data.Count > 0 Then
            For Each dt In data
                Dim panel As New Panel()
                With panel
                    .Size = New Drawing.Size(tabControl.ClientSize.Width - 55, 120)
                    .Location = New Drawing.Point(10, yOffset)
                    .BackColor = Color.White
                    .BorderStyle = BorderStyle.FixedSingle
                    .Padding = New Padding(10)
                End With


                Dim labelKode As New Label()
                With labelKode
                    .Font = New Font("Arial", 12, FontStyle.Regular)
                    .Text = $"Kode: {dt.kode_barang}"
                    .AutoSize = True
                    .Location = New Drawing.Point(10, 10)
                End With

                Dim labelNama As New Label()
                With labelNama
                    .Font = New Font("Arial", 12, FontStyle.Regular)
                    .Text = $"Nama: {dt.nama_barang}"
                    .AutoSize = True
                    .Location = New Drawing.Point(10, 30)
                End With

                Dim labelKategori As New Label()
                With labelKategori
                    .Font = New Font("Arial", 12, FontStyle.Regular)
                    .Text = $"Kategori: {dt.kategori}"
                    .AutoSize = True
                    .Location = New Drawing.Point(10, 50)
                End With

                Dim labelHarga As New Label()
                With labelHarga
                    .Font = New Font("Arial", 12, FontStyle.Regular)
                    .Text = $"Harga: {dt.harga}"
                    .AutoSize = True
                    .Location = New Drawing.Point(10, 70)
                End With

                Dim labelJumlah As New Label()
                With labelJumlah
                    .Font = New Font("Arial", 12, FontStyle.Regular)
                    .Text = $"Jumlah: {dt.qty}"
                    .AutoSize = True
                    .Location = New Drawing.Point(10, 90)
                End With

                With panel.Controls
                    .Add(labelKode)
                    .Add(labelNama)
                    .Add(labelKategori)
                    .Add(labelHarga)
                    .Add(labelJumlah)
                End With

                yOffset += panel.Height + 10
                scrollablePanel.Controls.Add(panel)
            Next
        Else
            MsgBox("No data found or an error occurred.")
        End If

        Me.Controls.Add(tabControl)
    End Sub
End Class