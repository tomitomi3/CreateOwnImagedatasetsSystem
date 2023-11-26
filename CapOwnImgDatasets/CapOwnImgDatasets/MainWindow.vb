Imports System.IO
Imports System.IO.Ports
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports Newtonsoft.Json
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

    <Runtime.InteropServices.DllImport("kernel32.dll")>
    Private Shared Function AllocConsole() As Boolean
    End Function

    ''' <summary>video cap</summary>
    Private _cap As VideoCapture = Nothing

    ''' <summary>thread video cap</summary>
    Private _thread As System.Threading.Thread = Nothing

    ''' <summary>スレッド同期(CS)</summary>
    Private _capLock = New Object()

    ''' <summary>LED非同期制御</summary>
    Private _ledLock = New Object()

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

    ''' <summary>加算Mat</summary>
    Private _sumMat As New List(Of Mat)

    ''' <summary>加算平均回数</summary>
    Private _avgCount As Byte = 0

    Private _exRatio As Double = 1.5

    Private _clipSizeEx As Integer = 0

    Private _clipSize As Integer = 0

    Private _rawClipMat As Mat = Nothing

    Private _rawClipExMat As Mat = Nothing

    Private _isColor = True

    ''' <summary>LED Controler</summary>
    Private _ledPatternGen As GenLEDPattern = Nothing

    ''' <summary>elapsed time per 1 capture</summary>
    Private _elapsedTime As Double = 0.0

    ''' <summary>serial port class</summary>
    Private oSerialPort As SerialPort = Nothing

#End Region

#Region "Private my func"
    ''' <summary>
    ''' Initialize camera
    ''' </summary>
    Private Function InitCam() As Boolean
        Try
            If _cap IsNot Nothing Then
                Return True
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
                                _cap.Set(VideoCaptureProperties.FrameWidth, 640)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 480)
                            ElseIf EnumCameraImgSize.Size800x600 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 800)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 600)
                            ElseIf EnumCameraImgSize.Size1280x720 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 1280)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 720)
                            ElseIf EnumCameraImgSize.Size1280x960 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 1280)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 960)
                            ElseIf EnumCameraImgSize.Size1280x1024 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 1280)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 1024)
                            ElseIf EnumCameraImgSize.Size1600x1200 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 1600)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 1200)
                            ElseIf EnumCameraImgSize.Size1920x1080 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 1920)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 1080)
                            ElseIf EnumCameraImgSize.Size1920x1200 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 1920)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 1200)
                            ElseIf EnumCameraImgSize.Size2048x1536 = CType(selectCamImgIndex, EnumCameraImgSize) Then
                                _cap.Set(VideoCaptureProperties.FrameWidth, 2048)
                                _cap.Set(VideoCaptureProperties.FrameHeight, 1536)
                            End If

                            ' カメラ設定
                            If Me.cbxAutoWB.Checked = False Then
                                _cap.Set(VideoCaptureProperties.AutoWB, 0)
                            Else
                                _cap.Set(VideoCaptureProperties.AutoWB, 1)
                            End If

                            If Me.cbxAutoExposure.Checked = False Then
                                _cap.Set(VideoCaptureProperties.AutoExposure, 0)

                                ' Manual
                                Dim tempExposure = Int(Me.tbxExposure.Text)
                                _cap.Set(VideoCaptureProperties.Exposure, tempExposure)
                            Else
                                _cap.Set(VideoCaptureProperties.AutoExposure, 1)
                            End If

                            If Me.cbxAutoExposure.Checked = False Then
                                ' Manual
                                Dim tempExposure = Int(Me.tbxExposure.Text)
                                _cap.Set(VideoCaptureProperties.Exposure, tempExposure)
                            End If

                            'CapuretPropery
                            Console.WriteLine("Camera ID    :{0}", camId)
                            Console.WriteLine(" Width       :{0}", _cap.Get(VideoCaptureProperties.FrameWidth))
                            Console.WriteLine(" Height      :{0}", _cap.Get(VideoCaptureProperties.FrameHeight))
                            Console.WriteLine(" AutoWB      :{0}", _cap.Get(VideoCaptureProperties.AutoWB))
                            Console.WriteLine(" AutoExposure:{0}", _cap.Get(VideoCaptureProperties.AutoExposure))
                            Console.WriteLine(" Exposure    :{0}", _cap.Get(VideoCaptureProperties.Exposure))
                            Console.WriteLine(" FPS         :{0}", _cap.Get(VideoCaptureProperties.Fps))
                            Console.WriteLine(" FrameCount  :{0}", _cap.Get(VideoCaptureProperties.FrameCount))
                            Console.WriteLine(" Gamma       :{0}", _cap.Get(VideoCaptureProperties.Gamma))
                            Console.WriteLine(" Brightness  :{0}", _cap.Get(VideoCaptureProperties.Brightness))
                            Console.WriteLine(" Contrast    :{0}", _cap.Get(VideoCaptureProperties.Contrast))
                            Console.WriteLine(" Hue         :{0}", _cap.Get(VideoCaptureProperties.Hue))
                            Console.WriteLine(" Gamma       :{0}", _cap.Get(VideoCaptureProperties.Gamma))
                            Console.WriteLine(" Gain        :{0}", _cap.Get(VideoCaptureProperties.Gain))
                            Console.WriteLine(" Temperature :{0}", _cap.Get(VideoCaptureProperties.Temperature))

                            Me._zoomRatio = _cap.FrameWidth / Me.pbxMainRaw.Width
                            Me._rawWidth = _cap.FrameWidth
                            Me._rawHeight = _cap.FrameHeight
                        End Sub
                      )

            'check
            If Me._cap.Get(VideoCaptureProperties.FrameWidth) <= 0 Then
                '使用できないカメラ
                Me._cap.Release()
                Me._cap = Nothing
                Return False
            End If

            'click位置でセンタリングしたい

        Catch ex As Exception
            _cap = Nothing
        End Try

        Return True
    End Function

    ''' <summary>
    ''' worker thread
    ''' </summary>
    Private Sub Worker()
        Dim sw As New Stopwatch()
        While (True)
            sw.Restart()
            Try
                'create VideoCapture instance
                If Me.InitCam() = False Then
                    MessageBox.Show("Erro:init cam")
                    Return
                End If

                'capture
                Using mat = New Mat()
                    If Me._cap.Read(mat) = False Then
                        Threading.Thread.Sleep(10)
                        Continue While
                    End If

                    'update
                    'Get image
                    Dim clipEditMat As Mat = Nothing
                    SyncLock _capLock
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
                            Me.pbxMainRaw.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(clipEditMat)

                            'update clip
                            If Me._isColor = False Then
                                'Convert grayscale
                                Dim dst = New Mat()
                                Cv2.CvtColor(Me._rawClipMat, dst, ColorConversionCodes.BGRA2GRAY)
                                Cv2.CvtColor(dst, Me._rawClipMat, ColorConversionCodes.GRAY2BGR)
                                Me.pbxProcessed.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst)
                            Else
                                Me.pbxProcessed.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(Me._rawClipMat)
                            End If

                            'adjust
                            If Me.pbxProcessed.Image.Height > Me.pbxProcessed.Height Then
                                Dim ratio = Me.pbxProcessed.Height / Me.pbxProcessed.Image.Height
                                Dim w = CInt(Me.pbxProcessed.Image.Height * ratio - 0.5)
                                Using tempMat As New Mat()
                                    Dim pbxProcessedMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(Me.pbxProcessed.Image)
                                    Cv2.Resize(pbxProcessedMat, tempMat, New OpenCvSharp.Size(w, w), interpolation:=InterpolationFlags.Linear)
                                    Me.pbxProcessed.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(tempMat)
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
            'Console.WriteLine("{0}[fps]", 1000.0 / sw.ElapsedMilliseconds)
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
    Private Sub SendArduinoWithCheckSum(ByVal _sendData As List(Of Byte), Optional ByVal flg As Boolean = True)
        If oSerialPort.IsOpen = False Then
            Return
        End If

        If flg = False Then
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

        'wait
        Dim waitMs = Int(allSendByte.Count / (Me.oSerialPort.BaudRate / 8.0) * 1000 * 1.5) + 40
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
        Console.WriteLine("start")

        'gets videocapture
        Dim camIds As New List(Of Integer)
        'camIds.Add(0)
        'camIds.Add(1)
        'camIds.Add(2)
        For i As Integer = 0 To 10 - 1
            Dim temp As VideoCapture = Nothing
            Try
                temp = New OpenCvSharp.VideoCapture(i)
                If temp.IsOpened() = False Then
                    Continue For
                Else
                    Console.WriteLine("CAMERA ID:{0}", i)
                    Console.WriteLine(" {0} {1}", temp.Get(VideoCaptureProperties.FrameHeight), temp.Get(VideoCaptureProperties.FrameWidth))
                    camIds.Add(i)
                End If
            Finally
                temp.Release()
            End Try
        Next

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

        'LED Controlの初期化
        Me._ledPatternGen = New GenLEDPattern()
        Me._ledPatternGen.NUM_OF_LED = 5
        Me._ledPatternGen.Brightness = 64

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

        'restore
        Me.RestoreSettings()

        Me.lblCapStatus.BackColor = Color.AliceBlue
        Me.lblCapStatus.Text = String.Format("Status:Stop")
    End Sub

    ''' <summary>
    ''' 設定値復元
    ''' </summary>
    Private Sub RestoreSettings()
        SettingFile.GetInstance().Load()
        Me.cbxAveraging.Checked = SettingFile.GetInstance().IsAverage
        Me.tbxAverage.Text = SettingFile.GetInstance().NumAve.ToString()
        Me.cbxLightCtrl.Checked = SettingFile.GetInstance().IsLEDCtrl
        Me.cbxRotation.Checked = SettingFile.GetInstance().IsRotation
        Me.tbxRotation.Text = SettingFile.GetInstance().RotationStep.ToString()
        Me.cbxMove.Checked = SettingFile.GetInstance().IsMove
        Me.tbxNumOfMove.Text = SettingFile.GetInstance().NumMoveImgs.ToString()
        Me.cbxFlip.Checked = SettingFile.GetInstance().IsFlip
        Me.cbxGrayscale.Checked = SettingFile.GetInstance().IsConvertGrayScale
    End Sub

    ''' <summary>
    ''' 設定値保存
    ''' </summary>
    Private Sub SaveSettings()
        SettingFile.GetInstance().IsAverage = Me.cbxAveraging.Checked
        SettingFile.GetInstance().NumAve = Integer.Parse(Me.tbxAverage.Text)
        SettingFile.GetInstance().IsLEDCtrl = Me.cbxLightCtrl.Checked
        SettingFile.GetInstance().IsRotation = Me.cbxRotation.Checked
        SettingFile.GetInstance().RotationStep = Integer.Parse(Me.tbxRotation.Text)
        SettingFile.GetInstance().IsMove = Me.cbxMove.Checked
        SettingFile.GetInstance().NumMoveImgs = Integer.Parse(Me.tbxNumOfMove.Text)
        SettingFile.GetInstance().IsFlip = Me.cbxFlip.Checked
        SettingFile.GetInstance().IsConvertGrayScale = Me.cbxGrayscale.Checked
        SettingFile.GetInstance().Update()
    End Sub

    ''' <summary>
    ''' Form close
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MainWindow_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        'save recent setting
        Me.SaveSettings()

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

    Private Sub StopThread()
        _thread.Abort()
        While (_thread.ThreadState <> Threading.ThreadState.Aborted)
            Console.WriteLine("exit...")
            Threading.Thread.Sleep(50)
            _thread.Join()
        End While
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
            Me.StopThread()

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
        Me._clipSizeEx = CInt(Me._clipSize * Me._exRatio + 0.5)
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

    Private Sub cbxColorOrGrayscale_CheckedChanged(sender As Object, e As EventArgs) Handles cbxGrayscale.CheckedChanged
        Me._isColor = Not Me.cbxGrayscale.Checked
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
    Private Async Sub btnSaveWithSettings_Click(sender As Object, e As EventArgs) Handles btnSaveWithSettings.Click
        If Me._cap Is Nothing Then
            Return
        End If

        Try
            Me.lblCapStatus.BackColor = Color.OrangeRed
            Me.lblCapStatus.Text = String.Format("Status:Running...")

            'set image process
            Dim ip = New ImageProcesser()

            If Me.cbxRotation.Checked Then
                ip.IsRotation = True
                ip.RotationStep = Integer.Parse(tbxRotation.Text)
            End If

            If Me.cbxMove.Checked Then
                ip.IsRandomMove = True
                ip.NumOfMove = Integer.Parse(tbxNumOfMove.Text)
            End If

            If Me.cbxFlip.Checked Then
                ip.IsFlip = True
            End If

            If Me.cbxGrayscale.Checked Then
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

            If Me.cbxLightCtrl.Checked = True Then
                'Exist LED Light control
                LEDPatternSets.GetInstance().Load()
                Dim patterns = LEDPatternSets.GetInstance().Patterns

                ' sleep setting
                Dim sleepTime = Me._elapsedTime * 4
                If Me.cbxAveraging.Checked = True Then
                    sleepTime = sleepTime * Integer.Parse(Me.tbxAverage.Text) + 100
                End If

                Await Task.Run(Sub()
                                   SyncLock _capLock
                                       For Each p In patterns
                                           'LED pattern
                                           SendArduinoWithCheckSum(p)

                                           'sleep
                                           System.Threading.Thread.Sleep(sleepTime)

                                           ip.InputMat = Me._rawClipExMat.Clone()
                                           Dim saveMats = ip.GetImageProcessedMats()
                                           For Each saveMat In saveMats
                                               Dim saveBmp As Bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(saveMat)
                                               saveImgUtil.Save(imgFormat, Me.tbxCorrectName.Text, saveBmp)
                                           Next
                                       Next
                                   End SyncLock
                               End Sub)
                'LED OFF
                Dim tempByte = GenLEDPattern.GetInitSendData(Me._ledPatternGen.NUM_OF_LED, Me._ledPatternGen.Brightness)
                SendArduinoWithCheckSum(tempByte)
            Else
                'No LED control
                Await Task.Run(Sub()
                                   SyncLock _capLock
                                       ip.InputMat = Me._rawClipExMat.Clone()
                                       Dim saveMats = ip.GetImageProcessedMats()
                                       For Each saveMat In saveMats
                                           Dim saveBmp As Bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(saveMat)
                                           saveImgUtil.Save(imgFormat, Me.tbxCorrectName.Text, saveBmp)
                                       Next
                                   End SyncLock
                               End Sub)
            End If
        Finally
            Me.lblCapStatus.BackColor = Color.AliceBlue
            Me.lblCapStatus.Text = String.Format("Status:Stop")
        End Try
    End Sub

    Private Sub cbxLightCtrl_CheckedChanged(sender As Object, e As EventArgs) Handles cbxLightCtrl.CheckedChanged
        'If cbxLightCtrl.Checked Then
        '    Me.btnOpenClose.Enabled = True
        '    Me.cbxPort.Enabled = True
        'Else
        '    Me.btnOpenClose.Enabled = False
        '    Me.cbxPort.Enabled = False

        '    If oSerialPort.IsOpen Then
        '        CloseProcess()
        '        Me.btnOpenClose.Text = "Open"
        '        Me.btnOpenClose.BackColor = Color.AliceBlue
        '    End If
        'End If
    End Sub

    Public Function GenerateLEDPattern() As List(Of Byte)
        Dim tempBytes As New List(Of Byte)
        Me._ledPatternGen.Brightness = Me.trbBrightness.Value
        If Me.rdnSingle.Checked Then
            tempBytes = Me._ledPatternGen.GenSinglePattern(Me.cmbLEDCH.SelectedIndex, trbR.Value, trbG.Value, trbB.Value)
        ElseIf Me.rdnLink.Checked Then
            tempBytes = GenLEDPattern.GenSamePattern(Me._ledPatternGen.NUM_OF_LED, Me._ledPatternGen.Brightness,
                                                    trbR.Value, trbG.Value, trbB.Value)
        ElseIf Me.rdnUpper.Checked Then
            tempBytes = GenLEDPattern.GenSamePattern(Me._ledPatternGen.NUM_OF_LED, Me._ledPatternGen.Brightness,
                                                    trbR.Value, trbG.Value, trbB.Value)
            tempBytes(GenLEDPattern.IDX_CH4 + 1) = 0
            tempBytes(GenLEDPattern.IDX_CH4 + 2) = 0
            tempBytes(GenLEDPattern.IDX_CH4 + 3) = 0
        ElseIf Me.rdnTable.Checked Then
            tempBytes = GenLEDPattern.GenSamePattern(Me._ledPatternGen.NUM_OF_LED, Me._ledPatternGen.Brightness,
                                                    trbR.Value, trbG.Value, trbB.Value)
            For i As Integer = 0 To 4 - 1
                Dim idx = 1 + i * GenLEDPattern.SIZE_CH_COLOR
                tempBytes(idx + 1) = 0
                tempBytes(idx + 2) = 0
                tempBytes(idx + 3) = 0
            Next
        End If
        Return tempBytes
    End Function

    Private _count As Integer = 0

    ''' <summary>
    ''' LED制御
    ''' </summary>
    Private Sub SendLEDSignal()
        If Me._ledPatternGen Is Nothing Then
            Return
        End If

        Dim tempBytes = Me.GenerateLEDPattern()
        Me.SendArduinoWithCheckSum(tempBytes)

        ' Async削除
        'Await Task.Run(Sub()
        '                   SyncLock _ledLock
        '                       Me.SendArduinoWithCheckSum(tempBytes)
        '                   End SyncLock
        '               End Sub)
    End Sub

    Private Sub btnDemoOFF_Click(sender As Object, e As EventArgs) Handles btnDemoOFF.Click
        Me.trbR.Value = 0
        Me.trbG.Value = 0
        Me.trbB.Value = 0
    End Sub

    Private Sub btnDemoR_Click(sender As Object, e As EventArgs) Handles btnDemoR.Click
        Me.trbR.Value = 128
        Me.trbG.Value = 0
        Me.trbB.Value = 0
    End Sub

    Private Sub btnDemoG_Click(sender As Object, e As EventArgs) Handles btnDemoG.Click
        Me.trbR.Value = 0
        Me.trbG.Value = 128
        Me.trbB.Value = 0
    End Sub

    Private Sub btnDemoB_Click(sender As Object, e As EventArgs) Handles btnDemoB.Click
        Me.trbR.Value = 0
        Me.trbG.Value = 0
        Me.trbB.Value = 128
    End Sub

    Private Sub btnDemoW_Click(sender As Object, e As EventArgs) Handles btnDemoW.Click
        Me.trbR.Value = 128
        Me.trbG.Value = 160
        Me.trbB.Value = 130
    End Sub

    Private Sub trbBrightness_ValueChanged(sender As Object, e As EventArgs) Handles trbBrightness.ValueChanged
        If Me._ledPatternGen Is Nothing Then
            Return
        End If
        Me._ledPatternGen.Brightness = Me.trbBrightness.Value
        Me.lblBrightness.Text = trbBrightness.Value.ToString()
        Me.SendLEDSignal()
    End Sub

    Private Sub trbR_ValueChanged(sender As Object, e As EventArgs) Handles trbR.ValueChanged
        If Me._ledPatternGen Is Nothing Then
            Return
        End If
        Me.lblR.Text = trbR.Value.ToString()
        Me.lblBrightness.Text = trbBrightness.Value.ToString()
        Me.SendLEDSignal()
    End Sub

    Private Sub trbG_ValueChanged(sender As Object, e As EventArgs) Handles trbG.ValueChanged
        If Me._ledPatternGen Is Nothing Then
            Return
        End If
        Me.lblG.Text = trbG.Value.ToString()
        Me.lblBrightness.Text = trbBrightness.Value.ToString()
        Me.SendLEDSignal()
    End Sub

    Private Sub trbB_ValueChanged(sender As Object, e As EventArgs) Handles trbB.ValueChanged
        If Me._ledPatternGen Is Nothing Then
            Return
        End If
        Me.lblB.Text = trbB.Value.ToString()
        Me.lblBrightness.Text = trbBrightness.Value.ToString()
        Me.SendLEDSignal()
    End Sub

    ''' <summary>
    ''' debug clipの中心RGB値を保存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRGBValueSave_Click(sender As Object, e As EventArgs) Handles btnRGBValueSave.Click
        Dim temp = lblRGBFromROI.Text.Split(",")
        Using writer As StreamWriter = New StreamWriter("RGBVALUE_DEBUG.txt", append:=True, encoding:=Encoding.GetEncoding("Shift_JIS"))
            writer.WriteLine("{0},{1},{2},{3},{4},{5}", trbR.Value, trbG.Value, trbB.Value, temp(1), temp(2), temp(3))
        End Using
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        LEDPatternSets.GetInstance().Load()

        Dim tempBytes = GenerateLEDPattern()
        LEDPatternSets.GetInstance().Patterns.Add(tempBytes)
        LEDPatternSets.GetInstance().Update()
    End Sub

    Private Sub btnPatternTest_Click(sender As Object, e As EventArgs) Handles btnPatternTest.Click
        LEDPatternSets.GetInstance().Load()

        For Each p In LEDPatternSets.GetInstance().Patterns
            Me.SendArduinoWithCheckSum(p)
            System.Threading.Thread.Sleep(300)
        Next
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        LEDPatternSets.GetInstance().Load()

        If LEDPatternSets.GetInstance().Patterns.Count > 0 Then
            Dim lastIndex = LEDPatternSets.GetInstance().Patterns.Count
            LEDPatternSets.GetInstance().Patterns.RemoveAt(lastIndex - 1)
        End If

        LEDPatternSets.GetInstance().Update()
    End Sub
#End Region
End Class
