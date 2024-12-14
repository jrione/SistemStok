<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Login
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.WelcomeLabel = New System.Windows.Forms.Label()
        Me.Username = New System.Windows.Forms.TextBox()
        Me.Password = New System.Windows.Forms.TextBox()
        Me.LoginBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'WelcomeLabel
        '
        Me.WelcomeLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.WelcomeLabel.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WelcomeLabel.Location = New System.Drawing.Point(0, 0)
        Me.WelcomeLabel.Name = "WelcomeLabel"
        Me.WelcomeLabel.Padding = New System.Windows.Forms.Padding(10)
        Me.WelcomeLabel.Size = New System.Drawing.Size(578, 49)
        Me.WelcomeLabel.TabIndex = 1
        Me.WelcomeLabel.Text = "Silakan Login"
        Me.WelcomeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Username
        '
        Me.Username.Location = New System.Drawing.Point(189, 106)
        Me.Username.Name = "Username"
        Me.Username.Size = New System.Drawing.Size(200, 26)
        Me.Username.TabIndex = 2
        '
        'Password
        '
        Me.Password.Location = New System.Drawing.Point(189, 165)
        Me.Password.Name = "Password"
        Me.Password.Size = New System.Drawing.Size(200, 26)
        Me.Password.TabIndex = 3
        '
        'LoginBtn
        '
        Me.LoginBtn.Location = New System.Drawing.Point(243, 219)
        Me.LoginBtn.Name = "LoginBtn"
        Me.LoginBtn.Size = New System.Drawing.Size(75, 23)
        Me.LoginBtn.TabIndex = 4
        Me.LoginBtn.Text = "Button1"
        Me.LoginBtn.UseVisualStyleBackColor = True
        '
        'Login
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(578, 344)
        Me.Controls.Add(Me.LoginBtn)
        Me.Controls.Add(Me.Password)
        Me.Controls.Add(Me.Username)
        Me.Controls.Add(Me.WelcomeLabel)
        Me.Name = "Login"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Login"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents WelcomeLabel As Label
    Friend WithEvents Username As TextBox
    Friend WithEvents Password As TextBox
    Friend WithEvents LoginBtn As Button
End Class
