﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tbxPort = New System.Windows.Forms.TextBox()
        Me.tbxSendMessage = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(118, 10)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Open"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'tbxPort
        '
        Me.tbxPort.Location = New System.Drawing.Point(12, 12)
        Me.tbxPort.Name = "tbxPort"
        Me.tbxPort.Size = New System.Drawing.Size(100, 19)
        Me.tbxPort.TabIndex = 1
        '
        'tbxSendMessage
        '
        Me.tbxSendMessage.Location = New System.Drawing.Point(12, 49)
        Me.tbxSendMessage.Name = "tbxSendMessage"
        Me.tbxSendMessage.Size = New System.Drawing.Size(100, 19)
        Me.tbxSendMessage.TabIndex = 2
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(118, 47)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Send"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(319, 167)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.tbxSendMessage)
        Me.Controls.Add(Me.tbxPort)
        Me.Controls.Add(Me.Button1)
        Me.Name = "frmMain"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents tbxPort As TextBox
    Friend WithEvents tbxSendMessage As TextBox
    Friend WithEvents Button2 As Button
End Class
