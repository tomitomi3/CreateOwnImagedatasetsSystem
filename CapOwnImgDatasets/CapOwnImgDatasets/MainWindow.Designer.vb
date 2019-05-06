<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainWindow
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cmbCamID = New System.Windows.Forms.ComboBox()
        Me.pbxMainRaw = New OpenCvSharp.UserInterface.PictureBoxIpl()
        Me.btnCamOpen = New System.Windows.Forms.Button()
        Me.pbxProcessed = New OpenCvSharp.UserInterface.PictureBoxIpl()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cbxAveraging = New System.Windows.Forms.CheckBox()
        Me.cbxRotation = New System.Windows.Forms.CheckBox()
        Me.cbxLightCtrl = New System.Windows.Forms.CheckBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tbxFolderPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbxCorrectName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbImgSize = New System.Windows.Forms.ComboBox()
        Me.btnOpenFolder = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        CType(Me.pbxMainRaw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbxProcessed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmbCamID
        '
        Me.cmbCamID.FormattingEnabled = True
        Me.cmbCamID.Location = New System.Drawing.Point(16, 463)
        Me.cmbCamID.Name = "cmbCamID"
        Me.cmbCamID.Size = New System.Drawing.Size(77, 20)
        Me.cmbCamID.TabIndex = 2
        '
        'pbxMainRaw
        '
        Me.pbxMainRaw.Location = New System.Drawing.Point(11, 18)
        Me.pbxMainRaw.Name = "pbxMainRaw"
        Me.pbxMainRaw.Size = New System.Drawing.Size(480, 360)
        Me.pbxMainRaw.TabIndex = 3
        Me.pbxMainRaw.TabStop = False
        '
        'btnCamOpen
        '
        Me.btnCamOpen.Location = New System.Drawing.Point(99, 461)
        Me.btnCamOpen.Name = "btnCamOpen"
        Me.btnCamOpen.Size = New System.Drawing.Size(75, 23)
        Me.btnCamOpen.TabIndex = 4
        Me.btnCamOpen.Text = "CamOpen"
        Me.btnCamOpen.UseVisualStyleBackColor = True
        '
        'pbxProcessed
        '
        Me.pbxProcessed.Location = New System.Drawing.Point(12, 18)
        Me.pbxProcessed.Name = "pbxProcessed"
        Me.pbxProcessed.Size = New System.Drawing.Size(480, 360)
        Me.pbxProcessed.TabIndex = 5
        Me.pbxProcessed.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 446)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 12)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "-CameraDeviceOpen"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbxMainRaw)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(509, 396)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Source"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.pbxProcessed)
        Me.GroupBox3.Location = New System.Drawing.Point(527, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(508, 396)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "ROI"
        '
        'cbxAveraging
        '
        Me.cbxAveraging.AutoSize = True
        Me.cbxAveraging.Location = New System.Drawing.Point(16, 539)
        Me.cbxAveraging.Name = "cbxAveraging"
        Me.cbxAveraging.Size = New System.Drawing.Size(75, 16)
        Me.cbxAveraging.TabIndex = 9
        Me.cbxAveraging.Text = "Averaging"
        Me.cbxAveraging.UseVisualStyleBackColor = True
        '
        'cbxRotation
        '
        Me.cbxRotation.AutoSize = True
        Me.cbxRotation.Location = New System.Drawing.Point(122, 539)
        Me.cbxRotation.Name = "cbxRotation"
        Me.cbxRotation.Size = New System.Drawing.Size(67, 16)
        Me.cbxRotation.TabIndex = 9
        Me.cbxRotation.Text = "Rotation"
        Me.cbxRotation.UseVisualStyleBackColor = True
        '
        'cbxLightCtrl
        '
        Me.cbxLightCtrl.AutoSize = True
        Me.cbxLightCtrl.Location = New System.Drawing.Point(222, 539)
        Me.cbxLightCtrl.Name = "cbxLightCtrl"
        Me.cbxLightCtrl.Size = New System.Drawing.Size(177, 16)
        Me.cbxLightCtrl.TabIndex = 9
        Me.cbxLightCtrl.Text = "LigthCtrl(under construction...)"
        Me.cbxLightCtrl.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(210, 667)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tbxFolderPath
        '
        Me.tbxFolderPath.Location = New System.Drawing.Point(16, 604)
        Me.tbxFolderPath.Name = "tbxFolderPath"
        Me.tbxFolderPath.Size = New System.Drawing.Size(472, 19)
        Me.tbxFolderPath.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 492)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "-Settings"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 568)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(82, 12)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "-Save Settings"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 589)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 12)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Path:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(12, 635)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 12)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "-Save"
        '
        'tbxCorrectName
        '
        Me.tbxCorrectName.Location = New System.Drawing.Point(16, 669)
        Me.tbxCorrectName.Name = "tbxCorrectName"
        Me.tbxCorrectName.Size = New System.Drawing.Size(188, 19)
        Me.tbxCorrectName.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 654)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(175, 12)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Correct Name(using folder name)"
        '
        'cmbImgSize
        '
        Me.cmbImgSize.FormattingEnabled = True
        Me.cmbImgSize.Location = New System.Drawing.Point(73, 510)
        Me.cmbImgSize.Name = "cmbImgSize"
        Me.cmbImgSize.Size = New System.Drawing.Size(121, 20)
        Me.cmbImgSize.TabIndex = 16
        '
        'btnOpenFolder
        '
        Me.btnOpenFolder.Location = New System.Drawing.Point(494, 602)
        Me.btnOpenFolder.Name = "btnOpenFolder"
        Me.btnOpenFolder.Size = New System.Drawing.Size(75, 23)
        Me.btnOpenFolder.TabIndex = 10
        Me.btnOpenFolder.Text = "Open"
        Me.btnOpenFolder.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(14, 513)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 12)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "ImageSize:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 420)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(131, 16)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "Collect Settings"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label9.Location = New System.Drawing.Point(606, 420)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(153, 16)
        Me.Label9.TabIndex = 17
        Me.Label9.Text = "Recognize Settings"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label10.Location = New System.Drawing.Point(626, 446)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(167, 16)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "Under construction..."
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1052, 714)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cmbImgSize)
        Me.Controls.Add(Me.tbxCorrectName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbxFolderPath)
        Me.Controls.Add(Me.btnOpenFolder)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.cbxLightCtrl)
        Me.Controls.Add(Me.cbxRotation)
        Me.Controls.Add(Me.cbxAveraging)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCamOpen)
        Me.Controls.Add(Me.cmbCamID)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Name = "MainWindow"
        Me.Text = "DEMO - Collect Image dataset And Recognizer"
        CType(Me.pbxMainRaw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbxProcessed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbCamID As ComboBox
    Friend WithEvents pbxMainRaw As OpenCvSharp.UserInterface.PictureBoxIpl
    Friend WithEvents btnCamOpen As Button
    Friend WithEvents pbxProcessed As OpenCvSharp.UserInterface.PictureBoxIpl
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents cbxAveraging As CheckBox
    Friend WithEvents cbxRotation As CheckBox
    Friend WithEvents cbxLightCtrl As CheckBox
    Friend WithEvents btnSave As Button
    Friend WithEvents tbxFolderPath As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents tbxCorrectName As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents cmbImgSize As ComboBox
    Friend WithEvents btnOpenFolder As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
End Class
