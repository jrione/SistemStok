<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.TxtKode = New System.Windows.Forms.TextBox()
        Me.TxtNama = New System.Windows.Forms.TextBox()
        Me.TxtKategori = New System.Windows.Forms.TextBox()
        Me.TxtJumlah = New System.Windows.Forms.TextBox()
        Me.BtnTambah = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.BtnHapus = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(0, 336)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(544, 124)
        Me.DataGridView1.TabIndex = 0
        '
        'TxtKode
        '
        Me.TxtKode.Location = New System.Drawing.Point(33, 91)
        Me.TxtKode.Name = "TxtKode"
        Me.TxtKode.Size = New System.Drawing.Size(100, 20)
        Me.TxtKode.TabIndex = 1
        '
        'TxtNama
        '
        Me.TxtNama.Location = New System.Drawing.Point(167, 91)
        Me.TxtNama.Name = "TxtNama"
        Me.TxtNama.Size = New System.Drawing.Size(100, 20)
        Me.TxtNama.TabIndex = 2
        '
        'TxtKategori
        '
        Me.TxtKategori.Location = New System.Drawing.Point(286, 91)
        Me.TxtKategori.Name = "TxtKategori"
        Me.TxtKategori.Size = New System.Drawing.Size(100, 20)
        Me.TxtKategori.TabIndex = 3
        '
        'TxtJumlah
        '
        Me.TxtJumlah.Location = New System.Drawing.Point(417, 94)
        Me.TxtJumlah.Name = "TxtJumlah"
        Me.TxtJumlah.Size = New System.Drawing.Size(100, 20)
        Me.TxtJumlah.TabIndex = 4
        '
        'BtnTambah
        '
        Me.BtnTambah.Location = New System.Drawing.Point(43, 216)
        Me.BtnTambah.Name = "BtnTambah"
        Me.BtnTambah.Size = New System.Drawing.Size(75, 23)
        Me.BtnTambah.TabIndex = 5
        Me.BtnTambah.Text = "BtnTambah"
        Me.BtnTambah.UseVisualStyleBackColor = True
        '
        'BtnEdit
        '
        Me.BtnEdit.Location = New System.Drawing.Point(178, 216)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 6
        Me.BtnEdit.Text = "BtnEdit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'BtnHapus
        '
        Me.BtnHapus.Location = New System.Drawing.Point(286, 216)
        Me.BtnHapus.Name = "BtnHapus"
        Me.BtnHapus.Size = New System.Drawing.Size(75, 23)
        Me.BtnHapus.TabIndex = 7
        Me.BtnHapus.Text = "BtnHapus"
        Me.BtnHapus.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Location = New System.Drawing.Point(429, 216)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(75, 23)
        Me.BtnClear.TabIndex = 8
        Me.BtnClear.Text = "BtnClear"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(548, 467)
        Me.Controls.Add(Me.BtnClear)
        Me.Controls.Add(Me.BtnHapus)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.BtnTambah)
        Me.Controls.Add(Me.TxtJumlah)
        Me.Controls.Add(Me.TxtKategori)
        Me.Controls.Add(Me.TxtNama)
        Me.Controls.Add(Me.TxtKode)
        Me.Controls.Add(Me.DataGridView1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents TxtKode As TextBox
    Friend WithEvents TxtNama As TextBox
    Friend WithEvents TxtKategori As TextBox
    Friend WithEvents TxtJumlah As TextBox
    Friend WithEvents BtnTambah As Button
    Friend WithEvents BtnEdit As Button
    Friend WithEvents BtnHapus As Button
    Friend WithEvents BtnClear As Button
End Class
