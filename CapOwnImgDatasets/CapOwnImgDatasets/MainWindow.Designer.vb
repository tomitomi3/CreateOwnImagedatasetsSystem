﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbxCorrectName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbImgSize = New System.Windows.Forms.ComboBox()
        Me.btnOpenFolder = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbxSlide = New System.Windows.Forms.TextBox()
        Me.tbxRotation = New System.Windows.Forms.TextBox()
        Me.cbxSlide = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tbxAverage = New System.Windows.Forms.TextBox()
        Me.cmbClipSize = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.btnSaveWithSettings = New System.Windows.Forms.Button()
        Me.cbxFlip = New System.Windows.Forms.CheckBox()
        Me.lblExDiff = New System.Windows.Forms.Label()
        Me.btnLightDemo = New System.Windows.Forms.Button()
        Me.cbxPort = New System.Windows.Forms.ComboBox()
        Me.btnOpenClose = New System.Windows.Forms.Button()
        Me.btnDemoR = New System.Windows.Forms.Button()
        Me.btnDemoG = New System.Windows.Forms.Button()
        Me.btnDemoB = New System.Windows.Forms.Button()
        Me.btnDemoW = New System.Windows.Forms.Button()
        Me.gbxCollect = New System.Windows.Forms.GroupBox()
        Me.cmbImageFormat = New System.Windows.Forms.ComboBox()
        Me.cbxColorOrGrayscale = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbCamImgSize = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbxRGBDemo = New System.Windows.Forms.TextBox()
        Me.btnSetRGB = New System.Windows.Forms.Button()
        Me.gbxLED = New System.Windows.Forms.GroupBox()
        Me.lblRGBFromROI = New System.Windows.Forms.Label()
        Me.btnRGBValueSave = New System.Windows.Forms.Button()
        Me.btnDemoOFF = New System.Windows.Forms.Button()
        Me.trbR = New System.Windows.Forms.TrackBar()
        Me.trbG = New System.Windows.Forms.TrackBar()
        Me.trbB = New System.Windows.Forms.TrackBar()
        Me.lblB = New System.Windows.Forms.Label()
        Me.lblG = New System.Windows.Forms.Label()
        Me.lblR = New System.Windows.Forms.Label()
        Me.cmbLEDCH = New System.Windows.Forms.ComboBox()
        Me.trbBrightness = New System.Windows.Forms.TrackBar()
        Me.lblBrightness = New System.Windows.Forms.Label()
        CType(Me.pbxMainRaw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbxProcessed, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.gbxCollect.SuspendLayout()
        Me.gbxLED.SuspendLayout()
        CType(Me.trbR, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trbG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trbB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trbBrightness, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbCamID
        '
        Me.cmbCamID.FormattingEnabled = True
        Me.cmbCamID.Location = New System.Drawing.Point(13, 35)
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
        Me.btnCamOpen.Location = New System.Drawing.Point(96, 33)
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
        Me.Label1.Location = New System.Drawing.Point(9, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(111, 12)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "-CameraDeviceOpen"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.pbxMainRaw)
        Me.GroupBox2.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(509, 388)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Source"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.pbxProcessed)
        Me.GroupBox3.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(527, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(508, 388)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Cliped Image(ROI)"
        '
        'cbxAveraging
        '
        Me.cbxAveraging.AutoSize = True
        Me.cbxAveraging.Checked = True
        Me.cbxAveraging.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxAveraging.Location = New System.Drawing.Point(13, 103)
        Me.cbxAveraging.Name = "cbxAveraging"
        Me.cbxAveraging.Size = New System.Drawing.Size(75, 16)
        Me.cbxAveraging.TabIndex = 9
        Me.cbxAveraging.Text = "Averaging"
        Me.cbxAveraging.UseVisualStyleBackColor = True
        '
        'cbxRotation
        '
        Me.cbxRotation.AutoSize = True
        Me.cbxRotation.Checked = True
        Me.cbxRotation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxRotation.Location = New System.Drawing.Point(13, 131)
        Me.cbxRotation.Name = "cbxRotation"
        Me.cbxRotation.Size = New System.Drawing.Size(67, 16)
        Me.cbxRotation.TabIndex = 9
        Me.cbxRotation.Text = "Rotation"
        Me.cbxRotation.UseVisualStyleBackColor = True
        '
        'cbxLightCtrl
        '
        Me.cbxLightCtrl.AutoSize = True
        Me.cbxLightCtrl.Checked = True
        Me.cbxLightCtrl.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxLightCtrl.Location = New System.Drawing.Point(196, 104)
        Me.cbxLightCtrl.Name = "cbxLightCtrl"
        Me.cbxLightCtrl.Size = New System.Drawing.Size(68, 16)
        Me.cbxLightCtrl.TabIndex = 9
        Me.cbxLightCtrl.Text = "LigthCtrl"
        Me.cbxLightCtrl.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(13, 319)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(92, 23)
        Me.btnSave.TabIndex = 10
        Me.btnSave.Text = "OneShotSave"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tbxFolderPath
        '
        Me.tbxFolderPath.Location = New System.Drawing.Point(13, 251)
        Me.tbxFolderPath.Name = "tbxFolderPath"
        Me.tbxFolderPath.Size = New System.Drawing.Size(457, 19)
        Me.tbxFolderPath.TabIndex = 11
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 12)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "-Settings"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 236)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 12)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Path:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(9, 183)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 12)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "-Save"
        '
        'tbxCorrectName
        '
        Me.tbxCorrectName.Location = New System.Drawing.Point(13, 294)
        Me.tbxCorrectName.Name = "tbxCorrectName"
        Me.tbxCorrectName.Size = New System.Drawing.Size(188, 19)
        Me.tbxCorrectName.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 279)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(177, 12)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Correct Name(using folder name):"
        '
        'cmbImgSize
        '
        Me.cmbImgSize.FormattingEnabled = True
        Me.cmbImgSize.Location = New System.Drawing.Point(74, 201)
        Me.cmbImgSize.Name = "cmbImgSize"
        Me.cmbImgSize.Size = New System.Drawing.Size(121, 20)
        Me.cmbImgSize.TabIndex = 16
        '
        'btnOpenFolder
        '
        Me.btnOpenFolder.Location = New System.Drawing.Point(395, 225)
        Me.btnOpenFolder.Name = "btnOpenFolder"
        Me.btnOpenFolder.Size = New System.Drawing.Size(75, 23)
        Me.btnOpenFolder.TabIndex = 10
        Me.btnOpenFolder.Text = "Open"
        Me.btnOpenFolder.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(14, 204)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 12)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "ImageSize:"
        '
        'tbxSlide
        '
        Me.tbxSlide.Location = New System.Drawing.Point(281, 127)
        Me.tbxSlide.Name = "tbxSlide"
        Me.tbxSlide.Size = New System.Drawing.Size(36, 19)
        Me.tbxSlide.TabIndex = 18
        '
        'tbxRotation
        '
        Me.tbxRotation.Location = New System.Drawing.Point(87, 128)
        Me.tbxRotation.Name = "tbxRotation"
        Me.tbxRotation.Size = New System.Drawing.Size(37, 19)
        Me.tbxRotation.TabIndex = 19
        Me.tbxRotation.Text = "10"
        '
        'cbxSlide
        '
        Me.cbxSlide.AutoSize = True
        Me.cbxSlide.Location = New System.Drawing.Point(196, 130)
        Me.cbxSlide.Name = "cbxSlide"
        Me.cbxSlide.Size = New System.Drawing.Size(84, 16)
        Me.cbxSlide.TabIndex = 20
        Me.cbxSlide.Text = "Move(Slide)"
        Me.cbxSlide.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(130, 131)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(51, 12)
        Me.Label11.TabIndex = 6
        Me.Label11.Text = "deg/step"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(324, 130)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(57, 12)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "pixel/step"
        '
        'tbxAverage
        '
        Me.tbxAverage.Location = New System.Drawing.Point(88, 101)
        Me.tbxAverage.Name = "tbxAverage"
        Me.tbxAverage.Size = New System.Drawing.Size(36, 19)
        Me.tbxAverage.TabIndex = 18
        Me.tbxAverage.Text = "2"
        '
        'cmbClipSize
        '
        Me.cmbClipSize.FormattingEnabled = True
        Me.cmbClipSize.Location = New System.Drawing.Point(253, 35)
        Me.cmbClipSize.Name = "cmbClipSize"
        Me.cmbClipSize.Size = New System.Drawing.Size(121, 20)
        Me.cmbClipSize.TabIndex = 21
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(184, 38)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(48, 12)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "ClipSize:"
        '
        'btnSaveWithSettings
        '
        Me.btnSaveWithSettings.Location = New System.Drawing.Point(111, 319)
        Me.btnSaveWithSettings.Name = "btnSaveWithSettings"
        Me.btnSaveWithSettings.Size = New System.Drawing.Size(114, 23)
        Me.btnSaveWithSettings.TabIndex = 10
        Me.btnSaveWithSettings.Text = "SaveWithSettings"
        Me.btnSaveWithSettings.UseVisualStyleBackColor = True
        '
        'cbxFlip
        '
        Me.cbxFlip.AutoSize = True
        Me.cbxFlip.Location = New System.Drawing.Point(402, 129)
        Me.cbxFlip.Name = "cbxFlip"
        Me.cbxFlip.Size = New System.Drawing.Size(43, 16)
        Me.cbxFlip.TabIndex = 22
        Me.cbxFlip.Text = "Flip"
        Me.cbxFlip.UseVisualStyleBackColor = True
        '
        'lblExDiff
        '
        Me.lblExDiff.AutoSize = True
        Me.lblExDiff.Location = New System.Drawing.Point(385, 38)
        Me.lblExDiff.Name = "lblExDiff"
        Me.lblExDiff.Size = New System.Drawing.Size(72, 12)
        Me.lblExDiff.TabIndex = 6
        Me.lblExDiff.Text = "Diff:xxx piexl"
        '
        'btnLightDemo
        '
        Me.btnLightDemo.Location = New System.Drawing.Point(414, 26)
        Me.btnLightDemo.Name = "btnLightDemo"
        Me.btnLightDemo.Size = New System.Drawing.Size(75, 23)
        Me.btnLightDemo.TabIndex = 23
        Me.btnLightDemo.Text = "DemoLight"
        Me.btnLightDemo.UseVisualStyleBackColor = True
        '
        'cbxPort
        '
        Me.cbxPort.FormattingEnabled = True
        Me.cbxPort.Location = New System.Drawing.Point(281, 101)
        Me.cbxPort.Name = "cbxPort"
        Me.cbxPort.Size = New System.Drawing.Size(82, 20)
        Me.cbxPort.TabIndex = 24
        '
        'btnOpenClose
        '
        Me.btnOpenClose.Location = New System.Drawing.Point(372, 100)
        Me.btnOpenClose.Name = "btnOpenClose"
        Me.btnOpenClose.Size = New System.Drawing.Size(75, 23)
        Me.btnOpenClose.TabIndex = 25
        Me.btnOpenClose.Text = "Open"
        Me.btnOpenClose.UseVisualStyleBackColor = True
        '
        'btnDemoR
        '
        Me.btnDemoR.Location = New System.Drawing.Point(250, 26)
        Me.btnDemoR.Name = "btnDemoR"
        Me.btnDemoR.Size = New System.Drawing.Size(35, 23)
        Me.btnDemoR.TabIndex = 26
        Me.btnDemoR.Text = "R"
        Me.btnDemoR.UseVisualStyleBackColor = True
        '
        'btnDemoG
        '
        Me.btnDemoG.Location = New System.Drawing.Point(291, 26)
        Me.btnDemoG.Name = "btnDemoG"
        Me.btnDemoG.Size = New System.Drawing.Size(35, 23)
        Me.btnDemoG.TabIndex = 26
        Me.btnDemoG.Text = "G"
        Me.btnDemoG.UseVisualStyleBackColor = True
        '
        'btnDemoB
        '
        Me.btnDemoB.Location = New System.Drawing.Point(332, 26)
        Me.btnDemoB.Name = "btnDemoB"
        Me.btnDemoB.Size = New System.Drawing.Size(35, 23)
        Me.btnDemoB.TabIndex = 26
        Me.btnDemoB.Text = "B"
        Me.btnDemoB.UseVisualStyleBackColor = True
        '
        'btnDemoW
        '
        Me.btnDemoW.Location = New System.Drawing.Point(373, 26)
        Me.btnDemoW.Name = "btnDemoW"
        Me.btnDemoW.Size = New System.Drawing.Size(35, 23)
        Me.btnDemoW.TabIndex = 26
        Me.btnDemoW.Text = "W"
        Me.btnDemoW.UseVisualStyleBackColor = True
        '
        'gbxCollect
        '
        Me.gbxCollect.Controls.Add(Me.cmbImageFormat)
        Me.gbxCollect.Controls.Add(Me.cbxColorOrGrayscale)
        Me.gbxCollect.Controls.Add(Me.Label8)
        Me.gbxCollect.Controls.Add(Me.cmbCamImgSize)
        Me.gbxCollect.Controls.Add(Me.Label1)
        Me.gbxCollect.Controls.Add(Me.Label11)
        Me.gbxCollect.Controls.Add(Me.Label7)
        Me.gbxCollect.Controls.Add(Me.cbxSlide)
        Me.gbxCollect.Controls.Add(Me.cbxLightCtrl)
        Me.gbxCollect.Controls.Add(Me.cmbImgSize)
        Me.gbxCollect.Controls.Add(Me.Label6)
        Me.gbxCollect.Controls.Add(Me.cmbCamID)
        Me.gbxCollect.Controls.Add(Me.tbxFolderPath)
        Me.gbxCollect.Controls.Add(Me.btnOpenFolder)
        Me.gbxCollect.Controls.Add(Me.btnSave)
        Me.gbxCollect.Controls.Add(Me.Label3)
        Me.gbxCollect.Controls.Add(Me.Label9)
        Me.gbxCollect.Controls.Add(Me.lblExDiff)
        Me.gbxCollect.Controls.Add(Me.tbxAverage)
        Me.gbxCollect.Controls.Add(Me.cbxPort)
        Me.gbxCollect.Controls.Add(Me.cbxAveraging)
        Me.gbxCollect.Controls.Add(Me.cbxFlip)
        Me.gbxCollect.Controls.Add(Me.Label4)
        Me.gbxCollect.Controls.Add(Me.tbxCorrectName)
        Me.gbxCollect.Controls.Add(Me.cbxRotation)
        Me.gbxCollect.Controls.Add(Me.tbxSlide)
        Me.gbxCollect.Controls.Add(Me.cmbClipSize)
        Me.gbxCollect.Controls.Add(Me.btnCamOpen)
        Me.gbxCollect.Controls.Add(Me.btnOpenClose)
        Me.gbxCollect.Controls.Add(Me.tbxRotation)
        Me.gbxCollect.Controls.Add(Me.btnSaveWithSettings)
        Me.gbxCollect.Controls.Add(Me.Label12)
        Me.gbxCollect.Controls.Add(Me.Label2)
        Me.gbxCollect.Controls.Add(Me.Label5)
        Me.gbxCollect.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.gbxCollect.Location = New System.Drawing.Point(12, 406)
        Me.gbxCollect.Name = "gbxCollect"
        Me.gbxCollect.Size = New System.Drawing.Size(509, 366)
        Me.gbxCollect.TabIndex = 27
        Me.gbxCollect.TabStop = False
        Me.gbxCollect.Text = "Settings for Collect"
        '
        'cmbImageFormat
        '
        Me.cmbImageFormat.FormattingEnabled = True
        Me.cmbImageFormat.Location = New System.Drawing.Point(253, 201)
        Me.cmbImageFormat.Name = "cmbImageFormat"
        Me.cmbImageFormat.Size = New System.Drawing.Size(86, 20)
        Me.cmbImageFormat.TabIndex = 33
        '
        'cbxColorOrGrayscale
        '
        Me.cbxColorOrGrayscale.AutoSize = True
        Me.cbxColorOrGrayscale.Location = New System.Drawing.Point(13, 159)
        Me.cbxColorOrGrayscale.Name = "cbxColorOrGrayscale"
        Me.cbxColorOrGrayscale.Size = New System.Drawing.Size(75, 16)
        Me.cbxColorOrGrayscale.TabIndex = 32
        Me.cbxColorOrGrayscale.Text = "Grayscale"
        Me.cbxColorOrGrayscale.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(206, 204)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 12)
        Me.Label8.TabIndex = 31
        Me.Label8.Text = "Format:"
        '
        'cmbCamImgSize
        '
        Me.cmbCamImgSize.FormattingEnabled = True
        Me.cmbCamImgSize.Location = New System.Drawing.Point(253, 59)
        Me.cmbCamImgSize.Name = "cmbCamImgSize"
        Me.cmbCamImgSize.Size = New System.Drawing.Size(121, 20)
        Me.cmbCamImgSize.TabIndex = 30
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(184, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 12)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "CameraSize:"
        '
        'tbxRGBDemo
        '
        Me.tbxRGBDemo.Location = New System.Drawing.Point(15, 28)
        Me.tbxRGBDemo.Name = "tbxRGBDemo"
        Me.tbxRGBDemo.Size = New System.Drawing.Size(100, 19)
        Me.tbxRGBDemo.TabIndex = 28
        Me.tbxRGBDemo.Text = "0,0,0"
        '
        'btnSetRGB
        '
        Me.btnSetRGB.Location = New System.Drawing.Point(121, 26)
        Me.btnSetRGB.Name = "btnSetRGB"
        Me.btnSetRGB.Size = New System.Drawing.Size(75, 23)
        Me.btnSetRGB.TabIndex = 29
        Me.btnSetRGB.Text = "Set"
        Me.btnSetRGB.UseVisualStyleBackColor = True
        '
        'gbxLED
        '
        Me.gbxLED.Controls.Add(Me.lblBrightness)
        Me.gbxLED.Controls.Add(Me.trbBrightness)
        Me.gbxLED.Controls.Add(Me.lblRGBFromROI)
        Me.gbxLED.Controls.Add(Me.btnRGBValueSave)
        Me.gbxLED.Controls.Add(Me.btnDemoOFF)
        Me.gbxLED.Controls.Add(Me.tbxRGBDemo)
        Me.gbxLED.Controls.Add(Me.trbR)
        Me.gbxLED.Controls.Add(Me.trbG)
        Me.gbxLED.Controls.Add(Me.btnSetRGB)
        Me.gbxLED.Controls.Add(Me.trbB)
        Me.gbxLED.Controls.Add(Me.btnDemoW)
        Me.gbxLED.Controls.Add(Me.lblB)
        Me.gbxLED.Controls.Add(Me.btnDemoB)
        Me.gbxLED.Controls.Add(Me.lblG)
        Me.gbxLED.Controls.Add(Me.btnDemoG)
        Me.gbxLED.Controls.Add(Me.lblR)
        Me.gbxLED.Controls.Add(Me.btnDemoR)
        Me.gbxLED.Controls.Add(Me.cmbLEDCH)
        Me.gbxLED.Controls.Add(Me.btnLightDemo)
        Me.gbxLED.Location = New System.Drawing.Point(527, 406)
        Me.gbxLED.Name = "gbxLED"
        Me.gbxLED.Size = New System.Drawing.Size(508, 330)
        Me.gbxLED.TabIndex = 32
        Me.gbxLED.TabStop = False
        Me.gbxLED.Text = "LED Manual Control"
        '
        'lblRGBFromROI
        '
        Me.lblRGBFromROI.AutoSize = True
        Me.lblRGBFromROI.Location = New System.Drawing.Point(256, 176)
        Me.lblRGBFromROI.Name = "lblRGBFromROI"
        Me.lblRGBFromROI.Size = New System.Drawing.Size(53, 12)
        Me.lblRGBFromROI.TabIndex = 34
        Me.lblRGBFromROI.Text = "RGB,x,x,x"
        '
        'btnRGBValueSave
        '
        Me.btnRGBValueSave.Location = New System.Drawing.Point(258, 142)
        Me.btnRGBValueSave.Name = "btnRGBValueSave"
        Me.btnRGBValueSave.Size = New System.Drawing.Size(114, 23)
        Me.btnRGBValueSave.TabIndex = 33
        Me.btnRGBValueSave.Text = "Save Center RGB"
        Me.btnRGBValueSave.UseVisualStyleBackColor = True
        '
        'btnDemoOFF
        '
        Me.btnDemoOFF.Location = New System.Drawing.Point(209, 26)
        Me.btnDemoOFF.Name = "btnDemoOFF"
        Me.btnDemoOFF.Size = New System.Drawing.Size(35, 23)
        Me.btnDemoOFF.TabIndex = 32
        Me.btnDemoOFF.Text = "OFF"
        Me.btnDemoOFF.UseVisualStyleBackColor = True
        '
        'trbR
        '
        Me.trbR.Location = New System.Drawing.Point(20, 143)
        Me.trbR.Maximum = 255
        Me.trbR.Minimum = 1
        Me.trbR.Name = "trbR"
        Me.trbR.Size = New System.Drawing.Size(205, 45)
        Me.trbR.TabIndex = 1
        Me.trbR.Value = 1
        '
        'trbG
        '
        Me.trbG.Location = New System.Drawing.Point(20, 194)
        Me.trbG.Maximum = 255
        Me.trbG.Minimum = 1
        Me.trbG.Name = "trbG"
        Me.trbG.Size = New System.Drawing.Size(205, 45)
        Me.trbG.TabIndex = 6
        Me.trbG.Value = 1
        '
        'trbB
        '
        Me.trbB.Location = New System.Drawing.Point(20, 245)
        Me.trbB.Maximum = 255
        Me.trbB.Minimum = 1
        Me.trbB.Name = "trbB"
        Me.trbB.Size = New System.Drawing.Size(205, 45)
        Me.trbB.TabIndex = 5
        Me.trbB.Value = 1
        '
        'lblB
        '
        Me.lblB.AutoSize = True
        Me.lblB.Location = New System.Drawing.Point(231, 249)
        Me.lblB.Name = "lblB"
        Me.lblB.Size = New System.Drawing.Size(11, 12)
        Me.lblB.TabIndex = 4
        Me.lblB.Text = "1"
        '
        'lblG
        '
        Me.lblG.AutoSize = True
        Me.lblG.Location = New System.Drawing.Point(231, 198)
        Me.lblG.Name = "lblG"
        Me.lblG.Size = New System.Drawing.Size(11, 12)
        Me.lblG.TabIndex = 3
        Me.lblG.Text = "1"
        '
        'lblR
        '
        Me.lblR.AutoSize = True
        Me.lblR.Location = New System.Drawing.Point(231, 147)
        Me.lblR.Name = "lblR"
        Me.lblR.Size = New System.Drawing.Size(11, 12)
        Me.lblR.TabIndex = 2
        Me.lblR.Text = "1"
        '
        'cmbLEDCH
        '
        Me.cmbLEDCH.FormattingEnabled = True
        Me.cmbLEDCH.Location = New System.Drawing.Point(15, 59)
        Me.cmbLEDCH.Name = "cmbLEDCH"
        Me.cmbLEDCH.Size = New System.Drawing.Size(100, 20)
        Me.cmbLEDCH.TabIndex = 0
        '
        'trbBrightness
        '
        Me.trbBrightness.Location = New System.Drawing.Point(20, 92)
        Me.trbBrightness.Maximum = 255
        Me.trbBrightness.Minimum = 1
        Me.trbBrightness.Name = "trbBrightness"
        Me.trbBrightness.Size = New System.Drawing.Size(205, 45)
        Me.trbBrightness.TabIndex = 35
        Me.trbBrightness.Value = 1
        '
        'lblBrightness
        '
        Me.lblBrightness.AutoSize = True
        Me.lblBrightness.Location = New System.Drawing.Point(231, 104)
        Me.lblBrightness.Name = "lblBrightness"
        Me.lblBrightness.Size = New System.Drawing.Size(11, 12)
        Me.lblBrightness.TabIndex = 36
        Me.lblBrightness.Text = "1"
        '
        'MainWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1046, 816)
        Me.Controls.Add(Me.gbxCollect)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.gbxLED)
        Me.Name = "MainWindow"
        Me.Text = "Collecting My Image dataset"
        CType(Me.pbxMainRaw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbxProcessed, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.gbxCollect.ResumeLayout(False)
        Me.gbxCollect.PerformLayout()
        Me.gbxLED.ResumeLayout(False)
        Me.gbxLED.PerformLayout()
        CType(Me.trbR, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trbG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trbB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trbBrightness, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

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
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents tbxCorrectName As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents cmbImgSize As ComboBox
    Friend WithEvents btnOpenFolder As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents tbxSlide As TextBox
    Friend WithEvents tbxRotation As TextBox
    Friend WithEvents cbxSlide As CheckBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents tbxAverage As TextBox
    Friend WithEvents cmbClipSize As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents btnSaveWithSettings As Button
    Friend WithEvents cbxFlip As CheckBox
    Friend WithEvents lblExDiff As Label
    Friend WithEvents btnLightDemo As Button
    Friend WithEvents cbxPort As ComboBox
    Friend WithEvents btnOpenClose As Button
    Friend WithEvents btnDemoR As Button
    Friend WithEvents btnDemoG As Button
    Friend WithEvents btnDemoB As Button
    Friend WithEvents btnDemoW As Button
    Friend WithEvents gbxCollect As GroupBox
    Friend WithEvents tbxRGBDemo As TextBox
    Friend WithEvents btnSetRGB As Button
    Friend WithEvents cmbCamImgSize As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents gbxLED As GroupBox
    Friend WithEvents cmbLEDCH As ComboBox
    Friend WithEvents trbG As TrackBar
    Friend WithEvents trbB As TrackBar
    Friend WithEvents lblB As Label
    Friend WithEvents lblG As Label
    Friend WithEvents lblR As Label
    Friend WithEvents trbR As TrackBar
    Friend WithEvents btnDemoOFF As Button
    Friend WithEvents btnRGBValueSave As Button
    Friend WithEvents lblRGBFromROI As Label
    Friend WithEvents cmbImageFormat As ComboBox
    Friend WithEvents cbxColorOrGrayscale As CheckBox
    Friend WithEvents Label8 As Label
    Friend WithEvents trbBrightness As TrackBar
    Friend WithEvents lblBrightness As Label
End Class
