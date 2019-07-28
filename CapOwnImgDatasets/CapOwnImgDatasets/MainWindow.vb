Imports System.IO.Ports
Imports System.Windows.Forms
Imports OpenCvSharp

Public Class MainWindow
#Region "Member"
    ''' <summary>コンソール</summary>
    Declare Function AllocConsole Lib "kernel32" () As Int32

    ''' <summary>video cap</summary>
    Private _cap As VideoCapture = Nothing

    ''' <summary>thread video cap</summary>
    Private _thread As System.Threading.Thread = Nothing

    ''' <summary>スレッド同期(CS)</summary>
    Private objlock = New Object()

    ''' <summary>クリック位置</summary>
    Private _clickedPos As New Point(0, 0)

    ''' <summary>画像保存Utility</summary>
    Private _saveImgUtil As New SaveImageUtili()

    ''' <summary>画像保存</summary>
    Private _ip As New ImageProcesser()

    ''' <summary>Raw キャプチャ画像幅</summary>
    Private _rawWidth As Integer = 0

    ''' <summary>Raw キャプチャ画像高さ</summary>
    Private _rawHeight As Integer = 0

    ''' <summary>拡大率(diplay to rawimg)</summary>
    Private _zoomRatio As Double = 0.0

    Private _sumMat As New List(Of Mat)
    Private _avgCount As Byte = 0

    Private CLIP_SIZE_EX As Integer = 0
    Private CLIP_SIZE As Integer = 0

    Private _imgMat As Mat = Nothing
    Private _imgExMat As Mat = Nothing

    ''' <summary>
    ''' クリップ画像サイズ
    ''' </summary>
    Public Enum EnumClipImageSize
        ClipSize200x200 = 200
        ClipSize250x250 = 250
        ClipSize300x300 = 300
        ClipSize350x350 = 350
        ClipSize400x400 = 400
    End Enum

    ''' <summary>
    ''' 出力画像サイズ
    ''' </summary>
    Public Enum EnumOutpuImageSize
        ImageSize64x64 = 64
        ImageSize128x128 = 128
        ImageSize256x256 = 256
    End Enum

    ''' <summary>
    ''' カメラサイズ
    ''' </summary>
    Public Enum EnumCameraImgSize
        Size640x480 = 0
        Size800x600
        Size1280x720
        Size1280x960
        Size1280x1024
        Size1600x1200
        Size1920x1080
        Size1920x1200
        Size2048x1536
    End Enum

    ''' <summary>To Send Arduino data</summary>
    Private _sendData As New List(Of Byte)
    Private oSerialPort As SerialPort = Nothing

#End Region

#Region "Private my func"
    ''' <summary>
    ''' Initialize camera
    ''' </summary>
    Private Sub InitCam()
        If _cap Is Nothing Then
            Dim camId = 0
            Me.Invoke(
                        Sub()
                            camId = CInt(Me.cmbCamID.SelectedItem.ToString())
                        End Sub
                        )
            _cap = New OpenCvSharp.VideoCapture(camId)

            '-----------------------------------------------
            'ここはUSBカメラによって適宜設定する
            '-----------------------------------------------
            Dim selectCamImgIndex = -1
            Me.Invoke(
                        Sub()
                            selectCamImgIndex = Me.cmbCamImgSize.SelectedIndex
                        End Sub
                      )
            If EnumCameraImgSize.Size640x480 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 640)
                _cap.Set(CaptureProperty.FrameHeight, 480)
            ElseIf EnumCameraImgSize.Size800x600 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 800)
                _cap.Set(CaptureProperty.FrameHeight, 600)
            ElseIf EnumCameraImgSize.Size1280x720 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 1280)
                _cap.Set(CaptureProperty.FrameHeight, 720)
            ElseIf EnumCameraImgSize.Size1280x960 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 1280)
                _cap.Set(CaptureProperty.FrameHeight, 960)
            ElseIf EnumCameraImgSize.Size1280x1024 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 1280)
                _cap.Set(CaptureProperty.FrameHeight, 1024)
            ElseIf EnumCameraImgSize.Size1600x1200 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 1600)
                _cap.Set(CaptureProperty.FrameHeight, 1200)
            ElseIf EnumCameraImgSize.Size1920x1080 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 1920)
                _cap.Set(CaptureProperty.FrameHeight, 1080)
            ElseIf EnumCameraImgSize.Size1920x1200 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 1920)
                _cap.Set(CaptureProperty.FrameHeight, 1200)
            ElseIf EnumCameraImgSize.Size2048x1536 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                _cap.Set(CaptureProperty.FrameWidth, 2048)
                _cap.Set(CaptureProperty.FrameHeight, 1536)
            End If

            'CapuretPropery
            Console.WriteLine("Camera ID    :{0}", camId)
            Console.WriteLine(" Width       :{0}", _cap.Get(CaptureProperty.FrameWidth))
            Console.WriteLine(" Height      :{0}", _cap.Get(CaptureProperty.FrameHeight))
            Console.WriteLine(" Exposure    :{0}", _cap.Get(CaptureProperty.Exposure))
            Console.WriteLine(" AutoExposure:{0}", _cap.Get(CaptureProperty.AutoExposure))
            Console.WriteLine(" Exposure    :{0}", _cap.Get(CaptureProperty.Exposure))
            Console.WriteLine(" FPS         :{0}", _cap.Get(CaptureProperty.Fps))
            Console.WriteLine(" FrameCount  :{0}", _cap.Get(CaptureProperty.FrameCount))
            Console.WriteLine(" Gamma       :{0}", _cap.Get(CaptureProperty.Gamma))
            Console.WriteLine(" Gain        :{0}", _cap.Get(CaptureProperty.Gain))
            Console.WriteLine(" Temperature :{0}", _cap.Get(CaptureProperty.Temperature))
            Console.WriteLine(" XI_AutoWB   :{0}", _cap.Get(CaptureProperty.XI_AutoWB))

            '_cap.Set(CaptureProperty.AutoExposure, 1)
            '_cap.Set(CaptureProperty.AutoExposure, 1)
            '_cap.Set(CaptureProperty.Exposure, 2.0)
        End If
    End Sub

    ''' <summary>
    ''' worker thread
    ''' </summary>
    Private Sub Worker()
        Dim sw As New Stopwatch()
        Dim rnd As New System.Random()
        While (True)
            sw.Restart()
            Try
                'create VideoCapture instance
                Me.InitCam()

                'wait ゆらぎ
                'System.Threading.Thread.Sleep(rnd.Next(20, 100))

                'capture
                Using mat = New Mat()
                    If _cap.Read(mat) = False Then
                        Continue While
                    End If

                    'update
                    SyncLock objlock
                        UpdateMainRawImg(mat)
                    End SyncLock
                End Using
            Catch ex As Threading.ThreadAbortException
                Console.WriteLine("throw ThreadAbortException")
                _cap.Release()
                _cap = Nothing
                Exit While
            Catch ex As Exception
                Console.WriteLine("Ex")
                Console.WriteLine("{0}", ex.Message)
            Finally
                Dim saumMemory = GC.GetTotalMemory(True)
                Dim thd As Long = 1024 * 1024 * 128
                If saumMemory > thd Then
                    GC.Collect()
                End If
            End Try
            sw.Stop()
            'Console.WriteLine("{0}[fps]", 1000.0 / sw.ElapsedMilliseconds)
        End While

        'done
        Console.WriteLine("WorkerDone")
    End Sub

    ''' <summary>
    ''' update image
    ''' </summary>
    ''' <param name="rawMat">raw image mat</param>
    Private Sub UpdateMainRawImg(ByRef rawMat As Mat)
        '画像の更新
        _zoomRatio = rawMat.Width / pbxMainRaw.Width
        _rawWidth = rawMat.Width
        _rawHeight = rawMat.Height

        'クリップ
        Dim dispW = rawMat.Width / _zoomRatio
        Dim dispH = rawMat.Height / _zoomRatio
        Dim exHalf = CInt(((CLIP_SIZE_EX - CLIP_SIZE) / 2.0))
        Dim exHalfRatio = CInt(((CLIP_SIZE_EX - CLIP_SIZE) / 2.0) / _zoomRatio)

        '縮小して表示画像を表示
        Using dst As New Mat(dispW, dispH, MatType.CV_8UC3)
            '縮小
            Cv2.Resize(rawMat, dst, New OpenCvSharp.Size(dispW, dispH), interpolation:=InterpolationFlags.Linear)

            'クリップの枠を描画
            'CLIP_SIZE_EX
            Dim rectSize = Nothing
            rectSize = New Rect(Me._clickedPos, New Size(CLIP_SIZE_EX / _zoomRatio, CLIP_SIZE_EX / _zoomRatio))
            Cv2.Rectangle(dst, rectSize, New Scalar(0, 0, 255), 1)
            'CLIP_SIZE
            rectSize = New Rect(New Point(Me._clickedPos.X + exHalfRatio, Me._clickedPos.Y + exHalfRatio), New Size(CLIP_SIZE / _zoomRatio, CLIP_SIZE / _zoomRatio))
            Cv2.Rectangle(dst, rectSize, New Scalar(255, 0, 0), 1)

            'Update
            Me.pbxMainRaw.ImageIpl = dst
        End Using

        'Clip image from raw image
        Dim mousePos As New Point(_clickedPos.X * _zoomRatio, _clickedPos.Y * _zoomRatio)
        Dim clipExRect = New Rect(mousePos, New Size(CLIP_SIZE_EX, CLIP_SIZE_EX))
        Dim clipExMat = rawMat(clipExRect)

        'get average num
        Dim avgNum As Integer = 1
        If String.IsNullOrEmpty(Me.tbxAverage.Text) = False Then
            avgNum = Integer.Parse(Me.tbxAverage.Text)
        End If

        'average
        Dim tempMat = GetAvgMat(clipExMat, avgNum)

        'update
        Dim clipRect = New Rect(New Point(exHalf, exHalf), New Size(CLIP_SIZE, CLIP_SIZE))
        If cbxAveraging.Checked = True Then
            Me._imgMat = tempMat(clipRect)
            Me._imgExMat = tempMat
        Else
            Me._imgMat = clipExMat(clipRect)
            Me._imgExMat = clipExMat
        End If
        Me.pbxProcessed.ImageIpl = Me._imgMat
    End Sub

    ''' <summary>
    ''' 平均化
    ''' </summary>
    ''' <param name="mat"></param>
    ''' <param name="averageNum"></param>
    ''' <returns></returns>
    Private Function GetAvgMat(ByVal mat As Mat, ByVal averageNum As Integer) As Mat
        'init
        If _sumMat.Count <> averageNum Then
            _sumMat.Clear()
            For i As Integer = 0 To averageNum - 1
                _sumMat.Add(New Mat(mat.Width, mat.Height, MatType.CV_8UC3))
            Next
            _avgCount = 0
        End If
        If _sumMat.Count > 0 AndAlso (_sumMat(0).Width <> mat.Width OrElse _sumMat(0).Height <> mat.Height) Then
            _sumMat.Clear()
            For i As Integer = 0 To averageNum - 1
                _sumMat.Add(New Mat(mat.Width, mat.Height, MatType.CV_8UC3))
            Next
            _avgCount = 0
        End If

        'save
        Dim idx = _avgCount Mod averageNum
        _sumMat(idx) = mat

        'add index
        If _avgCount = Byte.MaxValue Then
            _avgCount = 0
        Else
            _avgCount += 1
        End If

        'sum img
        Dim sumImg = New Mat(mat.Width, mat.Height, MatType.CV_16UC3)
        Dim tempImg = New Mat(mat.Width, mat.Height, MatType.CV_16UC3)
        For i As Integer = 0 To averageNum - 1
            _sumMat(i).ConvertTo(tempImg, MatType.CV_16UC3)
            sumImg += tempImg
        Next

        'average
        Dim retImg = New Mat(mat.Width, mat.Height, MatType.CV_16UC3)
        sumImg.ConvertTo(retImg, MatType.CV_8UC3, 1.0 / averageNum)
        Return retImg
    End Function

    ''' <summary>
    ''' close comport
    ''' </summary>
    Private Sub CloseProcess()
        If oSerialPort.IsOpen Then
            oSerialPort.Close()
        End If
    End Sub

    ''' <summary>
    ''' Send data to Arduino
    ''' </summary>
    Private Sub SendArduinoWithCheckSum()
        If oSerialPort.IsOpen = False Then
            Return
        End If

        'calc checksum
        Dim sumSize As UInt16 = 0
        For i As Integer = 0 To _sendData.Count - 1
            sumSize += _sendData(i)
        Next
        sumSize = (Not sumSize) + 1
        Dim chkSumByte = BitConverter.GetBytes(sumSize)

        'add checksum
        Dim allSendByte As New List(Of Byte)
        allSendByte.Add(chkSumByte(0))
        allSendByte.Add(chkSumByte(1))
        For Each temp In _sendData
            allSendByte.Add(temp) 'data
        Next

        'debug
        Console.WriteLine("SendSize:{0} CheckSum :{1}", allSendByte.Count, sumSize)
        For i As Integer = 0 To allSendByte.Count - 1
            Console.Write("{0} ", allSendByte(i))
        Next
        Console.WriteLine("")

        'write
        oSerialPort.Write(allSendByte.ToArray, 0, allSendByte.Count)
        Dim waitMs = CInt((allSendByte.Count * 1000) / (Me.oSerialPort.BaudRate / 8) * 1.5) + 20
        System.Threading.Thread.Sleep(waitMs)
    End Sub

#End Region

#Region "Public event"
    ''' <summary>
    ''' Main
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllocConsole()

        'gets videocapture
        Dim camIds As New List(Of Integer)
        camIds.Add(0)
        camIds.Add(1)
        camIds.Add(2)

        'For i As Integer = 0 To 10 - 1
        '    Dim temp As VideoCapture = Nothing
        '    Try
        '        temp = New OpenCvSharp.VideoCapture(i)
        '        If temp.IsOpened() = False Then
        '            Continue For
        '        Else
        '            Console.WriteLine("CAMERA ID:{0}", i)
        '            Console.WriteLine(" {0} {1}", temp.Get(CaptureProperty.FrameHeight), temp.Get(CaptureProperty.FrameWidth))
        '            camIds.Add(i)
        '        End If
        '    Finally
        '        temp.Release()
        '    End Try
        'Next

        'cmb box camera ID
        cmbCamID.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCamID.Items.Clear()
        For Each camId In camIds
            cmbCamID.Items.Add(camId.ToString())
        Next
        If cmbCamID.Items.Count = 0 Then
            'do nothing
        ElseIf cmbCamID.Items.Count = 1 Then
            cmbCamID.SelectedIndex = 0
        ElseIf cmbCamID.Items.Count = 2 Then
            cmbCamID.SelectedIndex = 1
        End If

        'cmb box clip size
        cmbClipSize.DropDownStyle = ComboBoxStyle.DropDownList
        cmbClipSize.Items.Clear()
        For Each tempVal In [Enum].GetValues(GetType(EnumClipImageSize))
            Dim eName As String = [Enum].GetName(GetType(EnumClipImageSize), tempVal)
            cmbClipSize.Items.Add(eName)
        Next
        cmbClipSize.SelectedIndex = 2

        'cmb box clip size
        cmbCamImgSize.DropDownStyle = ComboBoxStyle.DropDownList
        cmbCamImgSize.Items.Clear()
        For Each tempVal In [Enum].GetValues(GetType(EnumCameraImgSize))
            Dim eName As String = [Enum].GetName(GetType(EnumCameraImgSize), tempVal)
            cmbCamImgSize.Items.Add(eName)
        Next
        cmbCamImgSize.SelectedIndex = 2

        'cmb box image size
        cmbImgSize.DropDownStyle = ComboBoxStyle.DropDownList
        cmbImgSize.Items.Clear()
        For Each tempVal In [Enum].GetValues(GetType(EnumOutpuImageSize))
            Dim eName As String = [Enum].GetName(GetType(EnumOutpuImageSize), tempVal)
            cmbImgSize.Items.Add(eName)
        Next
        cmbImgSize.SelectedIndex = 1

        'save
        _saveImgUtil.Init(SaveImageUtili.GetExePath(), "MyImageDataset")
        Me.tbxFolderPath.Text = _saveImgUtil.GetSaveFolder()

        'UART
        oSerialPort = New SerialPort()
        oSerialPort.BaudRate = 9600 '9600
        oSerialPort.StopBits = StopBits.One
        oSerialPort.RtsEnable = False
        oSerialPort.DataBits = 8
        oSerialPort.Parity = False

        Dim ports = System.IO.Ports.SerialPort.GetPortNames()
        For Each portName In ports
            Console.WriteLine("{0}", portName)
        Next
        If ports.Length <> 0 Then
            For Each p In ports
                Me.cbxPort.Items.Add(String.Format("{0}", p))
            Next
            Me.cbxPort.SelectedIndex = 0
        End If

        'UI
        Me.btnOpenClose.Enabled = False
        Me.cbxPort.Enabled = False

        'debug
        'cmbCamID.SelectedIndex = 1
        'btnCamOpen.PerformClick()
    End Sub

    ''' <summary>
    ''' Form close
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MainWindow_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If _thread Is Nothing Then
            Return
        End If
        If _thread.IsAlive = True Then
            _thread.Abort()
        End If

        CloseProcess()
    End Sub

    ''' <summary>
    ''' click capture image
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub pbxMainRaw_MouseDown(sender As Object, e As MouseEventArgs) Handles pbxMainRaw.MouseDown
        Me._clickedPos.X = e.X
        Me._clickedPos.Y = e.Y
        Console.WriteLine("X={0},Y={1}", e.X, e.Y)

        '表示画像内にROIを限定
        If _cap IsNot Nothing Then
            'センタリング
            Me._clickedPos.X -= (CLIP_SIZE_EX / _zoomRatio) / 2.0
            Me._clickedPos.Y -= (CLIP_SIZE_EX / _zoomRatio) / 2.0

            '領域指定
            Dim dispW = _rawWidth / _zoomRatio
            Dim dispH = _rawHeight / _zoomRatio
            Dim maxDispW = CInt(0.5 + Me._clickedPos.X + CLIP_SIZE_EX / _zoomRatio)
            Dim maxDispH = CInt(0.5 + Me._clickedPos.Y + CLIP_SIZE_EX / _zoomRatio)
            If Me._clickedPos.X < 0 Then
                Me._clickedPos.X = 0
            End If
            If Me._clickedPos.Y < 0 Then
                Me._clickedPos.Y = 0
            End If
            If maxDispW > dispW Then
                Me._clickedPos.X -= maxDispW - dispW
            End If
            If maxDispH > dispH Then
                Me._clickedPos.Y -= maxDispH - dispH
            End If
        End If
    End Sub

    ''' <summary>
    ''' cap start
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCamOpen_Click(sender As Object, e As EventArgs) Handles btnCamOpen.Click
        'open
        If _cap Is Nothing Then
            'open cap
            _thread = New Threading.Thread(AddressOf Worker)
            _thread.Start()

            btnCamOpen.Text = "CamClose"
            btnCamOpen.BackColor = Color.Aqua
        Else
            'close cap
            _thread.Abort()
            While (_thread.ThreadState <> Threading.ThreadState.Aborted)
                Console.WriteLine("exit...")
                Threading.Thread.Sleep(50)
                _thread.Join()
            End While

            btnCamOpen.Text = "CamOpen"
            btnCamOpen.BackColor = Color.AliceBlue
        End If
    End Sub

    ''' <summary>
    ''' change clip image
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmbClipSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClipSize.SelectedIndexChanged
        For Each tempVal In [Enum].GetValues(GetType(EnumClipImageSize))
            Dim eName As String = [Enum].GetName(GetType(EnumClipImageSize), tempVal)
            If Me.cmbClipSize.SelectedItem.ToString() = eName Then
                Me.CLIP_SIZE = CInt(tempVal)
            End If
        Next
        Me.CLIP_SIZE_EX = CInt(CLIP_SIZE * 1.4143 + 0.5)
        Dim diff As Integer = CLIP_SIZE_EX - CLIP_SIZE
        Me.lblExDiff.Text = String.Format("Diff: {0} [pixel]", diff)

        'slide
        Me.tbxSlide.Text = CInt(diff / 2)
    End Sub

    ''' <summary>
    ''' Open/Close Com port
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCom_Click(sender As Object, e As EventArgs) Handles btnOpenClose.Click
        If oSerialPort.IsOpen Then
            CloseProcess()
            Me.btnOpenClose.Text = "Open"
            Me.btnOpenClose.BackColor = Color.AliceBlue
        Else
            Me.btnOpenClose.Text = "Close"
            Me.btnOpenClose.BackColor = Color.Aqua
            Try
                Me.oSerialPort.PortName = Me.cbxPort.SelectedItem.ToString
                Me.oSerialPort.Open()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' open save folder
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnOpenFolder_Click(sender As Object, e As EventArgs) Handles btnOpenFolder.Click
        System.Diagnostics.Process.Start(_saveImgUtil.GetSaveFolder())
    End Sub

    ''' <summary>
    ''' save(one shot)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SyncLock objlock
            Dim strCorrect = tbxCorrectName.Text
            _saveImgUtil.Save(strCorrect, Me.pbxProcessed.Image)
        End SyncLock
    End Sub

    ''' <summary>
    ''' save with various settings
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSaveWithSettings_Click(sender As Object, e As EventArgs) Handles btnSaveWithSettings.Click
        Dim ip = New ImageProcesser(Me._imgExMat)
        If Me.cbxRotation.Checked Then
            ip.RotationStep = Integer.Parse(tbxRotation.Text)
        End If
        If Me.cbxSlide.Checked Then
            ip.Slide = Integer.Parse(tbxSlide.Text)
            ip.SlideDiff = CLIP_SIZE_EX - CLIP_SIZE
        End If

        SyncLock objlock
            'flip x軸
            'rotation 18

            _saveImgUtil.Save(tbxCorrectName.Text, Me.pbxProcessed.Image)
        End SyncLock
    End Sub

    Private Const NUM_OF_LED As Integer = 5
    Private Const LightBrightness As Byte = 64
    Private Const LightStep As Byte = 32
    Private Const SIZE_CH_COLOR As Integer = 4
    Private Const IDX_BRIGHTNESS As Integer = 0
    Private Const IDX_CH0 As Integer = 1
    Private Const IDX_CH1 As Integer = IDX_CH0 + SIZE_CH_COLOR
    Private Const IDX_CH2 As Integer = IDX_CH1 + SIZE_CH_COLOR
    Private Const IDX_CH3 As Integer = IDX_CH2 + SIZE_CH_COLOR
    Private Const IDX_CH4 As Integer = IDX_CH3 + SIZE_CH_COLOR

    ''' <summary>
    ''' NeoPixelパターン生成
    ''' </summary>
    Private Function GenLigthtPattern() As List(Of Byte())
        Dim colorPattern = New List(Of Byte())
        Dim roundNum As Integer = ((Byte.MaxValue / LightStep) + 0.5)
        For r As Integer = 0 To roundNum - 1
            For g As Integer = 0 To roundNum - 1
                For b As Integer = 0 To roundNum - 1
                    Dim singleRGB As New List(Of Byte)
                    singleRGB.Add(r * LightStep)
                    singleRGB.Add(g * LightStep)
                    singleRGB.Add(b * LightStep)
                    colorPattern.Add(singleRGB.ToArray())
                Next
            Next
        Next
        Return colorPattern
    End Function

    Private Sub InitSendData()
        '初期化
        _sendData.Clear()
        If _sendData.Count <> (1 + SIZE_CH_COLOR * NUM_OF_LED) Then
            For i As Integer = 0 To (1 + SIZE_CH_COLOR * NUM_OF_LED) - 1
                _sendData.Add(0)
            Next
        End If
    End Sub

    Private Sub btnLightDemo_Click(sender As Object, e As EventArgs) Handles btnLightDemo.Click
        Dim ret = GenLigthtPattern()

        'Color
        '             0   R  G  B  0   R  G  B
        '[Brightness][CH][R][G][B][CH][R][G][B]
        InitSendData()
        _sendData(IDX_BRIGHTNESS) = LightBrightness
        For Each c In ret
            For i As Integer = 0 To NUM_OF_LED - 1
                Dim idx = IDX_CH0 + SIZE_CH_COLOR * i
                _sendData(idx) = i
                _sendData(idx + 1) = c(0)
                _sendData(idx + 2) = c(1)
                _sendData(idx + 3) = c(2)
            Next
            SendArduinoWithCheckSum()
            Application.DoEvents()
        Next
    End Sub

    Private Sub cbxLightCtrl_CheckedChanged(sender As Object, e As EventArgs) Handles cbxLightCtrl.CheckedChanged
        If cbxLightCtrl.Checked Then
            Me.btnOpenClose.Enabled = True
            Me.cbxPort.Enabled = True
        Else
            Me.btnOpenClose.Enabled = False
            Me.cbxPort.Enabled = False

            If oSerialPort.IsOpen Then
                CloseProcess()
                Me.btnOpenClose.Text = "Open"
                Me.btnOpenClose.BackColor = Color.AliceBlue
            End If
        End If
    End Sub

    Private Sub btnSetRGB_Click(sender As Object, e As EventArgs) Handles btnSetRGB.Click
        'byte
        Dim split = Me.tbxRGBDemo.Text.Split(",")
        If split.Count <> 3 Then
            Return
        End If

        InitSendData()
        _sendData(IDX_BRIGHTNESS) = LightBrightness
        For i As Integer = 0 To NUM_OF_LED - 1
            Dim idx = IDX_CH0 + SIZE_CH_COLOR * i
            _sendData(idx) = i
            _sendData(idx + 1) = split(0)
            _sendData(idx + 2) = split(1)
            _sendData(idx + 3) = split(2)
        Next
        SendArduinoWithCheckSum()
    End Sub

    Private Sub btnDemoR_Click(sender As Object, e As EventArgs) Handles btnDemoR.Click
        Me.tbxRGBDemo.Text = "255,0,0"
    End Sub

    Private Sub btnDemoG_Click(sender As Object, e As EventArgs) Handles btnDemoG.Click
        Me.tbxRGBDemo.Text = "0,255,0"
    End Sub

    Private Sub btnDemoB_Click(sender As Object, e As EventArgs) Handles btnDemoB.Click
        Me.tbxRGBDemo.Text = "0,0,255"
    End Sub

    Private Sub btnDemoW_Click(sender As Object, e As EventArgs) Handles btnDemoW.Click
        Me.tbxRGBDemo.Text = "255,255,255"
    End Sub

    Private Sub btnSingleLED_Click(sender As Object, e As EventArgs) Handles btnSingleLED.Click
        'byte
        Dim split = Me.tbxChRGB.Text.Split(",")
        If split.Count <> 4 Then
            Return
        End If

        _sendData.Clear()
        _sendData.Add(LightBrightness)
        _sendData.Add(Byte.Parse(split(0)))
        _sendData.Add(Byte.Parse(split(1)))
        _sendData.Add(Byte.Parse(split(2)))
        _sendData.Add(Byte.Parse(split(3)))
        SendArduinoWithCheckSum()
    End Sub

#End Region
End Class

Public Class ImageProcesser
    Private clipExMat As Mat

    Public Property RotationStep As Integer = 0
    Public Property Slide As Integer = 0
    Public Property SlideDiff As Integer = 0

    Public Sub New()

    End Sub

    Public Sub New(clipExMat As Mat)
        Me.clipExMat = clipExMat
    End Sub

    Public Function GetRotationMats() As Mat

    End Function

    Public Function GetMat() As Mat
        ''画像処理
        'Using dst As New Mat(clipExMat.Width, clipExMat.Height, MatType.CV_8UC3)
        '    Dim stepAngle = Integer.Parse(Me.tbxRotation.Text)

        '    Dim numAngle = CInt(360 / stepAngle)

        '    Dim center As New Point2f(clipExMat.Width / 2.0, clipExMat.Height / 2.0)
        '    Dim rotationMat = Cv2.GetRotationMatrix2D(center, stepAngle, 1.0)
        '    Cv2.WarpAffine(clipExMat, dst, rotationMat, clipExMat.Size())
        '    Dim clipRect = New Rect(New Point(exHalf, exHalf), New Size(CLIP_SIZE, CLIP_SIZE))
        '    Dim dst2 = dst(clipRect)
        '    Me.pbxProcessed.ImageIpl = dst2
        'End Using
    End Function
End Class
