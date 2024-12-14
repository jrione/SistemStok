Imports MySql.Data.MySqlClient

Public Class Form1
    ' Koneksi database
    Dim connectionString As String = "Server=localhost;Database=StokDB;Uid=root;Pwd=mypass;"
    Dim connection As MySqlConnection

    ' Load Form
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connection = New MySqlConnection(connectionString)
        LoadData()
    End Sub

    ' Load data ke DataGridView
    Private Sub LoadData()
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
            Dim query As String = "SELECT * FROM Barang"
            Dim adapter As New MySqlDataAdapter(query, connection)
            Dim table As New DataTable()
            adapter.Fill(table)
            DataGridView1.DataSource = table
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    ' Tambah data
    Private Sub BtnTambah_Click(sender As Object, e As EventArgs) Handles BtnTambah.Click
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
            Dim query As String = "INSERT INTO Barang (KodeBarang, NamaBarang, Kategori, Jumlah) VALUES (@Kode, @Nama, @Kategori, @Jumlah)"
            Dim command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@Kode", TxtKode.Text)
            command.Parameters.AddWithValue("@Nama", TxtNama.Text)
            command.Parameters.AddWithValue("@Kategori", TxtKategori.Text)
            command.Parameters.AddWithValue("@Jumlah", TxtJumlah.Text)
            command.ExecuteNonQuery()
            MessageBox.Show("Data berhasil ditambahkan.")
            LoadData()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    ' Edit data
    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
            Dim query As String = "UPDATE Barang SET NamaBarang=@Nama, Kategori=@Kategori, Jumlah=@Jumlah WHERE KodeBarang=@Kode"
            Dim command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@Kode", TxtKode.Text)
            command.Parameters.AddWithValue("@Nama", TxtNama.Text)
            command.Parameters.AddWithValue("@Kategori", TxtKategori.Text)
            command.Parameters.AddWithValue("@Jumlah", TxtJumlah.Text)
            command.ExecuteNonQuery()
            MessageBox.Show("Data berhasil diubah.")
            LoadData()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    ' Hapus data
    Private Sub BtnHapus_Click(sender As Object, e As EventArgs) Handles BtnHapus.Click
        Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
            Dim query As String = "DELETE FROM Barang WHERE KodeBarang=@Kode"
            Dim command As New MySqlCommand(query, connection)
            command.Parameters.AddWithValue("@Kode", TxtKode.Text)
            command.ExecuteNonQuery()
            MessageBox.Show("Data berhasil dihapus.")
            LoadData()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            connection.Close()
        End Try
    End Sub

    ' Kosongkan form input
    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        TxtKode.Clear()
        TxtNama.Clear()
        TxtKategori.Clear()
        TxtJumlah.Clear()
    End Sub

End Class