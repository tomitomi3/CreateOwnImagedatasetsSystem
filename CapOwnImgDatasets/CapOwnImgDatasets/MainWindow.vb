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
            _cap.Set(CaptureProperty.FrameWidth, 1280) '1280
            _cap.Set(CaptureProperty.FrameHeight, 960) '960
        End If
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

                sw.Stop()
                'Console.WriteLine("{0}[fps]", 1000.0 / sw.ElapsedMilliseconds)
            End Try
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

        'debug
        cmbCamID.SelectedIndex = 1
        btnCamOpen.PerformClick()
    End Sub

    ''' <summary>
    ''' Close
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
    End Sub

    ''' <summary>
    ''' キャプチャ画像クリック
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
    End Sub

    ''' <summary>
    ''' 保存先を開く
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnOpenFolder_Click(sender As Object, e As EventArgs) Handles btnOpenFolder.Click
        System.Diagnostics.Process.Start(_saveImgUtil.GetSaveFolder())
    End Sub

    ''' <summary>
    ''' 保存
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
    ''' 保存 with Settings
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




            _saveImgUtil.Save(tbxCorrectName.Text, Me.pbxProcessed.Image)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Lightのパターン生成
    ''' </summary>
    Private Sub GenLigthtPattern()

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
