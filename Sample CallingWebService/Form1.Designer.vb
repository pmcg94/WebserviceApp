<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Me.TransferQueries = New System.Windows.Forms.Button()
        Me.BtnUpdateUsers = New System.Windows.Forms.Button()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.DataGridView2 = New System.Windows.Forms.DataGridView()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TransferQueries
        '
        Me.TransferQueries.Location = New System.Drawing.Point(186, 25)
        Me.TransferQueries.Name = "TransferQueries"
        Me.TransferQueries.Size = New System.Drawing.Size(168, 61)
        Me.TransferQueries.TabIndex = 0
        Me.TransferQueries.Text = "Transfer Queries"
        Me.TransferQueries.UseVisualStyleBackColor = True
        '
        'BtnUpdateUsers
        '
        Me.BtnUpdateUsers.Location = New System.Drawing.Point(12, 25)
        Me.BtnUpdateUsers.Name = "BtnUpdateUsers"
        Me.BtnUpdateUsers.Size = New System.Drawing.Size(168, 61)
        Me.BtnUpdateUsers.TabIndex = 3
        Me.BtnUpdateUsers.Text = "Fill Users"
        Me.BtnUpdateUsers.UseVisualStyleBackColor = True
        '
        'BackgroundWorker1
        '
        '
        'Timer1
        '
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Location = New System.Drawing.Point(12, 92)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        Me.DataGridView2.Size = New System.Drawing.Size(972, 178)
        Me.DataGridView2.TabIndex = 4
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 650)
        Me.Controls.Add(Me.DataGridView2)
        Me.Controls.Add(Me.BtnUpdateUsers)
        Me.Controls.Add(Me.TransferQueries)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TransferQueries As Button
    Friend WithEvents BtnUpdateUsers As Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As Timer
    Friend WithEvents DataGridView2 As DataGridView
End Class
