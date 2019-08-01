Imports System.IO
Imports System.IO.Ports
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports OpenCvSharp

Public Class MainWindow
#Region "Member"

    ''' <summary>
    ''' クリップ画像サイズ
    ''' </summary>
    Public Enum EnumClipImageSize
        ClipSize200x200 = 200
        ClipSize250x250 = 250
        ClipSize300x300 = 300
        ClipSize350x350 = 350
        ClipSize400x400 = 400
        ClipSize450x450 = 450
        ClipSize500x500 = 500
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

    ''' <summary>
    ''' 出力画像フォーマット
    ''' </summary>
    Public Enum EnumOutpuImageFormat
        BMP
        PNG
        JPEG
    End Enum

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

    Private _clipSizeEx As Integer = 0

    Private _clipSize As Integer = 0

    Private _rawClipMat As Mat = Nothing

    Private _rawClipExMat As Mat = Nothing

    Private _isColor = True

    ''' <summary>elapsed time per 1 capture</summary>
    Private _elapsedTime As Double = 0.0
    ''' <summary>To Send Arduino data</summary>
    Private _sendData As New List(Of Byte)

    ''' <summary>serial port class</summary>
    Private oSerialPort As SerialPort = Nothing

#End Region

#Region "Private my func"
    ''' <summary>
    ''' Initialize camera
    ''' </summary>
    Private Sub InitCam()
        If _cap IsNot Nothing Then
            Return
        End If

        'Camera IDからキャプチャ
        Dim camId = 0
        Me.Invoke(
                    Sub()
                        camId = CInt(Me.cmbCamID.SelectedItem.ToString())
                    End Sub
                    )
        Me._cap = New OpenCvSharp.VideoCapture(camId)

        'ここはUSBカメラによって適宜設定する
        Dim selectCamImgIndex = -1
        Me.Invoke(
                    Sub()
                        selectCamImgIndex = Me.cmbCamImgSize.SelectedIndex

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

                        '_cap.Set(CaptureProperty.AutoExposure, 1)
                        '_cap.Set(CaptureProperty.Exposure, 2.0)
                        '_cap.Set(CaptureProperty.Gain, 2.0)
                        '_cap.Set(CaptureProperty.Gamma, 0.5)

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

                        Me._zoomRatio = _cap.FrameWidth / Me.pbxMainRaw.Width
                        Me._rawWidth = _cap.FrameWidth
                        Me._rawHeight = _cap.FrameHeight

                    End Sub
                  )
    End Sub

    ''' <summary>
    ''' worker thread
    ''' </summary>
    Private Sub Worker()
        Dim sw As New Stopwatch()
        While (True)
            sw.Restart()
            Try
                'create VideoCapture instance
                Me.InitCam()

                'wait ゆらぎ
                'System.Threading.Thread.Sleep(rnd.Next(20, 100))

                'capture
                Using mat = New Mat()
                    If Me._cap.Read(mat) = False Then
                        Continue While
                    End If

                    'update
                    'Get image
                    Dim clipEditMat As Mat = Nothing
                    SyncLock objlock
                        Me.GetClipImageFromCameraImage(mat, clipEditMat, Me._rawClipMat, Me._rawClipExMat)
                    End SyncLock

                    'Debug get RGB Value from ROI
                    Dim g_width As Integer = Me._rawClipMat.Width / 2
                    Dim g_height As Integer = Me._rawClipMat.Height / 2
                    Dim g_data = Me._rawClipMat(g_width, g_width + 1, g_height, g_height + 1)
                    Dim prt = g_data.Data() 'B G Rの並び
                    Dim b = Marshal.ReadByte(prt)
                    Dim g = Marshal.ReadByte(prt + 1)
                    Dim r = Marshal.ReadByte(prt + 2)

                    'Update UI
                    Me.Invoke(
                        Sub()
                            'update edit
                            Me.pbxMainRaw.ImageIpl = clipEditMat

                            'update clip
                            If Me._isColor = False Then
                                'Convert grayscale
                                Dim dst = New Mat()
                                Cv2.CvtColor(Me._rawClipMat, dst, ColorConversionCodes.BGRA2GRAY)
                                Cv2.CvtColor(dst, Me._rawClipMat, ColorConversionCodes.GRAY2BGR)
                                Me.pbxProcessed.ImageIpl = dst
                            Else
                                Me.pbxProcessed.ImageIpl = Me._rawClipMat
                            End If

                            'adjust
                            If Me.pbxProcessed.ImageIpl.Height > Me.pbxProcessed.Height Then
                                Dim ratio = Me.pbxProcessed.Height / Me.pbxProcessed.ImageIpl.Height
                                Dim w = CInt(Me.pbxProcessed.ImageIpl.Height * ratio - 0.5)
                                Using tempMat As New Mat()
                                    Cv2.Resize(Me.pbxProcessed.ImageIpl, tempMat, New OpenCvSharp.Size(w, w), interpolation:=InterpolationFlags.Cubic)
                                    Me.pbxProcessed.ImageIpl = tempMat
                                End Using
                            End If

                            Me.lblRGBFromROI.Text = String.Format("RGB,{0},{1},{2}", r, g, b)
                        End Sub
                        )
                End Using
            Catch ex As Threading.ThreadAbortException
                Console.WriteLine("throw ThreadAbortException")
                Me._cap.Release()
                Me._cap = Nothing
                Exit While
            Catch ex As Exception
                Console.WriteLine("Catch Exception!")
                Console.WriteLine("{0}", ex.Message)
            Finally
                Dim sumUsingMemory = GC.GetTotalMemory(True)
                Dim thd As Long = 1024 * 1024 * 256
                If sumUsingMemory > thd Then
                    GC.Collect()
                End If
            End Try
            sw.Stop()
            Me._elapsedTime = sw.ElapsedMilliseconds
            Console.WriteLine("{0}[fps]", 1000.0 / sw.ElapsedMilliseconds)
        End While

        'done
        Console.WriteLine("WorkerDone")
    End Sub

    ''' <summary>
    ''' Clip image from Camera
    ''' </summary>
    ''' <param name="rawCameraMat">Camera</param>
    ''' <param name="editClipMat">out disp edit mat</param>
    ''' <param name="rawClipMat">out Clip mat</param>
    ''' <param name="rawClipExMat">out ClipEx mat</param>
    Private Sub GetClipImageFromCameraImage(ByRef rawCameraMat As Mat, ByRef editClipMat As Mat, ByRef rawClipMat As Mat, ByRef rawClipExMat As Mat)
        'clip pos
        Dim dispW = rawCameraMat.Width / _zoomRatio
        Dim dispH = rawCameraMat.Height / _zoomRatio
        Dim rawDiffHalf = (_clipSizeEx - _clipSize) / 2.0
        Dim dispDiffHalf = (_clipSizeEx - _clipSize) / 2.0 / _zoomRatio
        Dim dispClipHalf = _clipSizeEx / 2.0 / _zoomRatio

        '縮小して表示画像を表示
        Using dispEditClipMat As New Mat(dispW, dispH, MatType.CV_8UC3)
            '縮小
            Cv2.Resize(rawCameraMat, dispEditClipMat, New OpenCvSharp.Size(dispW, dispH), interpolation:=InterpolationFlags.Cubic)

            'CLIP_SIZE_EX枠
            Dim rectSizeDisp = Nothing
            rectSizeDisp = New Rect(Me._clickedPos, New Size(_clipSizeEx / _zoomRatio, _clipSizeEx / _zoomRatio))
            Cv2.Rectangle(dispEditClipMat, rectSizeDisp, New Scalar(0, 0, 255), 1)

            'CLIP_SIZE枠
            rectSizeDisp = New Rect(New Point(Me._clickedPos.X + dispDiffHalf, Me._clickedPos.Y + dispDiffHalf), New Size(_clipSize / _zoomRatio, _clipSize / _zoomRatio))
            Cv2.Rectangle(dispEditClipMat, rectSizeDisp, New Scalar(0, 255, 0), 1)

            'cross hair
            Dim ptCrossHairX1 = New Point(dispClipHalf + Me._clickedPos.X - 5, dispClipHalf + Me._clickedPos.Y)
            Dim ptCrossHairX2 = New Point(dispClipHalf + Me._clickedPos.X + 5, dispClipHalf + Me._clickedPos.Y)
            Cv2.Line(dispEditClipMat, ptCrossHairX1, ptCrossHairX2, New Scalar(0, 0, 0), 2)
            Dim ptCrossHairY1 = New Point(dispClipHalf + Me._clickedPos.X, dispClipHalf + Me._clickedPos.Y - 5)
            Dim ptCrossHairY2 = New Point(dispClipHalf + Me._clickedPos.X, dispClipHalf + Me._clickedPos.Y + 5)
            Cv2.Line(dispEditClipMat, ptCrossHairY1, ptCrossHairY2, New Scalar(0, 0, 0), 2)

            'Update
            editClipMat = dispEditClipMat.Clone()
        End Using

        'ClipEx image from raw image
        Dim mousePos As New Point(Me._clickedPos.X * _zoomRatio, Me._clickedPos.Y * _zoomRatio)
        Dim tempRawClipExMat = rawCameraMat(mousePos.Y, mousePos.Y + _clipSizeEx, mousePos.X, mousePos.X + _clipSizeEx)

        'get average num
        Dim avgNum As Integer = 1
        Dim isAverage = (Me.cbxAveraging.Checked = True) And (String.IsNullOrEmpty(Me.tbxAverage.Text) = False) And (Integer.Parse(Me.tbxAverage.Text) >= 1)
        Dim rawClipRect = New Rect(New Point(rawDiffHalf, rawDiffHalf), New Size(_clipSize, _clipSize))
        If isAverage = True Then
            'Do Average
            avgNum = Integer.Parse(Me.tbxAverage.Text)
            Dim averagedMat = Me.GetAvgMat(tempRawClipExMat, avgNum)
            rawClipMat = averagedMat(rawClipRect)
            rawClipExMat = averagedMat
        Else
            'not Average
            rawClipMat = tempRawClipExMat(rawClipRect)
            rawClipExMat = tempRawClipExMat
        End If
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

    ''' <summary>
    ''' 1CHのLED制御
    ''' </summary>
    ''' <param name="brightness"></param>
    ''' <param name="ch"></param>
    ''' <param name="r"></param>
    ''' <param name="g"></param>
    ''' <param name="b"></param>
    Private Sub SendLightLED(ByVal brightness As Integer, ByVal ch As Integer, ByVal r As Integer, ByVal g As Integer, ByVal b As Integer)
        _sendData.Clear()
        _sendData.Add(brightness)
        _sendData.Add(ch)
        _sendData.Add(r)
        _sendData.Add(g)
        _sendData.Add(b)
        SendArduinoWithCheckSum()
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
        Else
            cmbCamID.SelectedIndex = 0
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
        cmbCamImgSize.SelectedIndex = 3

        'cmb box image size
        cmbImgSize.DropDownStyle = ComboBoxStyle.DropDownList
        cmbImgSize.Items.Clear()
        For Each tempVal In [Enum].GetValues(GetType(EnumOutpuImageSize))
            Dim eName As String = [Enum].GetName(GetType(EnumOutpuImageSize), tempVal)
            cmbImgSize.Items.Add(eName)
        Next
        cmbImgSize.SelectedIndex = 1

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

        'LED Manual
        cmbLEDCH.DropDownStyle = ComboBoxStyle.DropDownList
        cmbLEDCH.Items.Clear()
        For i = 0 To 5 - 1
            cmbLEDCH.Items.Add(String.Format("CH{0}", i))
        Next
        cmbLEDCH.SelectedIndex = 1

        'UI
        Me.btnOpenClose.Enabled = True
        Me.cbxPort.Enabled = True

        'Image format
        cmbImageFormat.DropDownStyle = ComboBoxStyle.DropDownList
        cmbImageFormat.Items.Clear()
        For Each tempVal In [Enum].GetValues(GetType(EnumOutpuImageFormat))
            Dim eName As String = [Enum].GetName(GetType(EnumOutpuImageFormat), tempVal)
            cmbImageFormat.Items.Add(eName)
        Next
        cmbImageFormat.SelectedIndex = 1

        'save
        Dim initCorrectName = "MyImageDataset"
        Me.tbxFolderPath.Text = SaveImageUtili.GetFullPathWithCorrectName(SaveImageUtili.GetExePath(), "MyImageDataset")
        Me.tbxCorrectName.Text = "INPUT CORRECT"
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
            Me._clickedPos.X -= (_clipSizeEx / _zoomRatio) / 2.0
            Me._clickedPos.Y -= (_clipSizeEx / _zoomRatio) / 2.0

            '領域指定
            Dim dispW = _rawWidth / _zoomRatio
            Dim dispH = _rawHeight / _zoomRatio
            Dim maxDispW = CInt(0.5 + Me._clickedPos.X + _clipSizeEx / _zoomRatio)
            Dim maxDispH = CInt(0.5 + Me._clickedPos.Y + _clipSizeEx / _zoomRatio)
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
            _thread.Priority = System.Threading.ThreadPriority.Highest
            _thread.Name = "Cap Thread"
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
                Me._clipSize = CInt(tempVal)
            End If
        Next
        Me._clipSizeEx = CInt(Me._clipSize * 1.5 + 0.5)
        Dim diff As Integer = _clipSizeEx - _clipSize
        Me.lblExDiff.Text = String.Format("Diff: {0} [pixel]", diff)
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

    Private Sub cbxColorOrGrayscale_CheckedChanged(sender As Object, e As EventArgs) Handles cbxColorOrGrayscale.CheckedChanged
        Me._isColor = Not Me.cbxColorOrGrayscale.Checked
    End Sub

    ''' <summary>
    ''' open save folder
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnOpenFolder_Click(sender As Object, e As EventArgs) Handles btnOpenFolder.Click
        System.Diagnostics.Process.Start(Me.tbxFolderPath.Text)
    End Sub

    ''' <summary>
    ''' save with various settings
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSaveWithSettings_Click(sender As Object, e As EventArgs) Handles btnSaveWithSettings.Click
        If Me._cap Is Nothing Then
            Return
        End If
        'set image process
        Dim ip = New ImageProcesser()

        If Me.cbxRotation.Checked Then
            ip.IsRotation = True
            ip.RotationStep = Integer.Parse(tbxRotation.Text)
        End If

        If Me.cbxMove.Checked Then
            ip.IsMove = True
            ip.NumOfMove = Integer.Parse(tbxNumOfMove.Text)
        End If

        If Me.cbxFlip.Checked Then
            ip.IsFlip = True
        End If

        If Me.cbxColorOrGrayscale.Checked Then
            ip.IsColor = False
        End If

        For Each tempVal In [Enum].GetValues(GetType(EnumOutpuImageSize))
            Dim eName As String = [Enum].GetName(GetType(EnumOutpuImageSize), tempVal)
            If Me.cmbImgSize.SelectedItem.ToString() = eName Then
                ip.ImageSize = CInt(tempVal)
                Exit For
            End If
        Next

        ip.ClipSize = Me._clipSize

        'check save folder
        Dim saveImgUtil As New SaveImageUtili()
        saveImgUtil.Init(Me.tbxFolderPath.Text)

        'save
        Dim imgFormat = CType(Me.cmbImageFormat.SelectedIndex, EnumOutpuImageFormat)

        Dim sleepTime = Me._elapsedTime * 1.5
        If Me.cbxAveraging.Checked = True Then
            sleepTime = sleepTime * Integer.Parse(Me.tbxAverage.Text) + 300
        End If
        If Me.cbxLightCtrl.Checked = True Then
            'Exist LED Light control
            Dim patterns = GenP()
            For Each p In patterns
                'LED pattern
                _sendData = p.ToList()
                SendArduinoWithCheckSum()

                'sleep
                System.Threading.Thread.Sleep(2000)

                SyncLock objlock
                    ip.InputMat = Me._rawClipExMat.Clone()
                End SyncLock
                Dim saveMats = ip.GetMats()
                For Each saveMat In saveMats
                    Dim saveBmp As Bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(saveMat)
                    saveImgUtil.Save(imgFormat, Me.tbxCorrectName.Text, saveBmp)
                Next

                '砂時計防止
                Application.DoEvents()
            Next

            'LED OFF
            _sendData = Me.GetInitSendData().ToList()
            SendArduinoWithCheckSum()
        Else
            'No LED control
            SyncLock objlock
                ip.InputMat = Me._rawClipExMat.Clone()
            End SyncLock
            Dim saveMats = ip.GetMats()
            For Each saveMat In saveMats
                Dim saveBmp As Bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(saveMat)
                saveImgUtil.Save(imgFormat, Me.tbxCorrectName.Text, saveBmp)
            Next
        End If
    End Sub

    Private Function GetInitSendData() As Byte()
        Dim size = (1 + SIZE_CH_COLOR * NUM_OF_LED) - 1
        Dim tempBytes(size) As Byte
        tempBytes(IDX_BRIGHTNESS) = _LightBrightness
        tempBytes(IDX_CH0) = 0
        tempBytes(IDX_CH1) = 1
        tempBytes(IDX_CH2) = 2
        tempBytes(IDX_CH3) = 3
        tempBytes(IDX_CH4) = 4

        Return tempBytes
    End Function

    Private Function GenP() As List(Of Byte())
        Dim colorPattern = New List(Of Byte())
        Dim size = (1 + SIZE_CH_COLOR * NUM_OF_LED) - 1

        '何もなし
        With Nothing
            Dim tempBytes = GetInitSendData()
            colorPattern.Add(tempBytes)
        End With

        'table
        With Nothing
            Dim tempBytes = GetInitSendData()
            tempBytes(IDX_CH4 + 1) = 128
            tempBytes(IDX_CH4 + 2) = 128
            tempBytes(IDX_CH4 + 3) = 128
            colorPattern.Add(tempBytes)
        End With

        '上
        With Nothing
            Dim tempBytes = GetInitSendData()
            tempBytes(IDX_CH0 + 1) = 128
            tempBytes(IDX_CH0 + 2) = 0
            tempBytes(IDX_CH0 + 3) = 0
            tempBytes(IDX_CH1 + 1) = 128
            tempBytes(IDX_CH1 + 2) = 0
            tempBytes(IDX_CH1 + 3) = 0
            colorPattern.Add(tempBytes)
        End With

        With Nothing
            Dim tempBytes = GetInitSendData()
            tempBytes(IDX_CH0 + 1) = 0
            tempBytes(IDX_CH0 + 2) = 128
            tempBytes(IDX_CH0 + 3) = 0
            tempBytes(IDX_CH1 + 1) = 0
            tempBytes(IDX_CH1 + 2) = 128
            tempBytes(IDX_CH1 + 3) = 0
            colorPattern.Add(tempBytes)
        End With

        With Nothing
            Dim tempBytes = GetInitSendData()
            tempBytes(IDX_CH0 + 1) = 0
            tempBytes(IDX_CH0 + 2) = 0
            tempBytes(IDX_CH0 + 3) = 128
            tempBytes(IDX_CH1 + 1) = 0
            tempBytes(IDX_CH1 + 2) = 0
            tempBytes(IDX_CH1 + 3) = 128
            colorPattern.Add(tempBytes)
        End With

        Return colorPattern
    End Function

    Private Const NUM_OF_LED As Integer = 5
    Private _LightBrightness As Byte = 255
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
        For i As Integer = 0 To (1 + SIZE_CH_COLOR * NUM_OF_LED) - 1
            _sendData.Add(0)
        Next
    End Sub

    Private Sub btnLightDemo_Click(sender As Object, e As EventArgs) Handles btnLightDemo.Click
        Dim ret = GenLigthtPattern()

        'Color
        '             0   R  G  B  0   R  G  B
        '[Brightness][CH][R][G][B][CH][R][G][B]
        InitSendData()
        _sendData(IDX_BRIGHTNESS) = _LightBrightness
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
        _sendData(IDX_BRIGHTNESS) = _LightBrightness
        For i As Integer = 0 To NUM_OF_LED - 1
            Dim idx = IDX_CH0 + SIZE_CH_COLOR * i
            _sendData(idx) = i
            _sendData(idx + 1) = split(0)
            _sendData(idx + 2) = split(1)
            _sendData(idx + 3) = split(2)
        Next
        SendArduinoWithCheckSum()
    End Sub

    Private Sub btnDemoOFF_Click(sender As Object, e As EventArgs) Handles btnDemoOFF.Click
        Me.tbxRGBDemo.Text = "0,0,0"
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

    ''' <summary>
    ''' トラックバー 輝度
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub trbBrightness_Scroll(sender As Object, e As EventArgs) Handles trbBrightness.Scroll
        Me._LightBrightness = Integer.Parse(Me.trbBrightness.Value)
        Me.lblBrightness.Text = trbBrightness.Value.ToString()
        Me.SendLightLED(_LightBrightness, Me.cmbLEDCH.SelectedIndex, trbR.Value, trbG.Value, trbB.Value)
        Me.SendArduinoWithCheckSum()
    End Sub

    Private Sub tbrR_Scroll(sender As Object, e As EventArgs) Handles trbR.Scroll
        Me.lblR.Text = trbR.Value.ToString()
        Me.SendLightLED(_LightBrightness, Me.cmbLEDCH.SelectedIndex, trbR.Value, trbG.Value, trbB.Value)
        Me.SendArduinoWithCheckSum()
    End Sub

    Private Sub tbrG_Scroll(sender As Object, e As EventArgs) Handles trbG.Scroll
        Me.lblG.Text = trbG.Value.ToString()
        Me.SendLightLED(_LightBrightness, Me.cmbLEDCH.SelectedIndex, trbR.Value, trbG.Value, trbB.Value)
        Me.SendArduinoWithCheckSum()
    End Sub

    Private Sub tbgB_Scroll(sender As Object, e As EventArgs) Handles trbB.Scroll
        Me.lblB.Text = trbB.Value.ToString()
        Me.SendLightLED(_LightBrightness, Me.cmbLEDCH.SelectedIndex, trbR.Value, trbG.Value, trbB.Value)
        Me.SendArduinoWithCheckSum()
    End Sub

    Private Sub btnRGBValueSave_Click(sender As Object, e As EventArgs) Handles btnRGBValueSave.Click
        Dim temp = lblRGBFromROI.Text.Split(",")
        Using writer As StreamWriter = New StreamWriter("RGBVALUE_DEBUG.txt", append:=True, encoding:=Encoding.GetEncoding("Shift_JIS"))
            writer.WriteLine("{0},{1},{2},{3},{4},{5}", trbR.Value, trbG.Value, trbB.Value, temp(1), temp(2), temp(3))
        End Using
    End Sub


#End Region
End Class
