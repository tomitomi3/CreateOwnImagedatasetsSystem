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

    Private _clickedPos As New Point(0, 0)

    Private _saveImgUtil As New SaveImageUtili()

    Private _ip As New ImageProcesser()

    Private _zoomRatio As Double = 0.0

    ''' <summary>
    ''' 出力画像サイズ
    ''' </summary>
    Public Enum EnumImageSize
        ImageSize64x64 = 64
        ImageSize128x128 = 128
        ImageSize256x256 = 256
    End Enum

#End Region

    ''' <summary>
    ''' Main
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllocConsole()

        'gets videocapture
        Dim camIds As New List(Of Integer)
        For i As Integer = 0 To 10 - 1
            Dim temp As VideoCapture = Nothing
            Try
                temp = New OpenCvSharp.VideoCapture(i)
                If temp.IsOpened() = False Then
                    Continue For
                Else
                    Console.WriteLine("CAMERA ID:{0}", i)
                    Console.WriteLine(" {0} {1}", temp.Get(CaptureProperty.FrameHeight), temp.Get(CaptureProperty.FrameWidth))
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
        ElseIf cmbCamID.Items.Count = 1 Then
            cmbCamID.SelectedIndex = 0
        ElseIf cmbCamID.Items.Count = 2 Then
            cmbCamID.SelectedIndex = 1
        End If

        'cmb box image size
        cmbImgSize.DropDownStyle = ComboBoxStyle.DropDownList
        cmbImgSize.Items.Clear()
        For Each tempVal In [Enum].GetValues(GetType(EnumImageSize))
            Dim eName As String = [Enum].GetName(GetType(EnumImageSize), tempVal)
            cmbImgSize.Items.Add(eName)
        Next
        cmbImgSize.SelectedIndex = 1

        'save
        _saveImgUtil.Init(SaveImageUtili.GetExePath(), "MyImageDataset")
        Me.tbxFolderPath.Text = _saveImgUtil.GetSaveFolder()
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
    ''' initialize camera
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
            _cap.Set(CaptureProperty.FrameWidth, 800) '1280
            _cap.Set(CaptureProperty.FrameHeight, 600) '960
        End If
    End Sub

    ''' <summary>
    ''' update image
    ''' </summary>
    ''' <param name="mat"></param>
    Private Sub UpdateMainRawImg(ByRef mat As Mat)
        '比率
        _zoomRatio = mat.Width / pbxMainRaw.Width
        Dim w = mat.Width / _zoomRatio
        Dim h = mat.Height / _zoomRatio
        Dim CLIP_SIZE = 300
        Dim CLIP_SIZE_EX = CInt(300.0 * 1.4143)

        'update raw image
        Using dst As New Mat(w, h, MatType.CV_8UC3)
            'resize for display
            Cv2.Resize(mat, dst, New OpenCvSharp.Size(w, h), interpolation:=InterpolationFlags.Linear)

            'get
            Dim rectSize = New Rect(Me._clickedPos, New Size(CLIP_SIZE / _zoomRatio, CLIP_SIZE / _zoomRatio))
            Cv2.Rectangle(dst, rectSize, New Scalar(255, 0, 0), 1)

            'Update
            Me.pbxMainRaw.ImageIpl = dst
        End Using

        'update ROI image
        Dim roiMousePos As New Point(_clickedPos.X * _zoomRatio, _clickedPos.Y * _zoomRatio)
        Dim roiRect = New Rect(roiMousePos, New Size(CLIP_SIZE, CLIP_SIZE))
        Dim temp = mat.Clone()(roiRect)
        'update
        Me.pbxProcessed.ImageIpl = temp

    End Sub

    ''' <summary>
    ''' worker thread
    ''' </summary>
    Private Sub Worker()
        Dim sw As New Stopwatch()
        While (True)
            sw.Restart()
            Threading.Thread.Sleep(1) 'window sleep precition is 16ms...
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
                If GC.GetTotalMemory(False) > 1024 * 1024 * 128 Then
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
    ''' 画像上の右クリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub pbxMainRaw_MouseDown(sender As Object, e As MouseEventArgs) Handles pbxMainRaw.MouseDown
        Me._clickedPos.X = e.X
        Me._clickedPos.Y = e.Y
        Console.WriteLine("X={0},Y={1}", e.X, e.Y)

        'クリック位置→カメラ画像の位置に変換
        Dim camClickedPosition As New Point(_clickedPos.X * _zoomRatio, _clickedPos.Y * _zoomRatio)
        Console.WriteLine("camera X={0},camera Y={1}", camClickedPosition.X, camClickedPosition.Y)
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
            _saveImgUtil.Save(strCorrect, pbxProcessed.Image)
        End SyncLock
    End Sub

    ''' <summary>
    ''' 加算平均画像
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cbxAveraging_CheckedChanged(sender As Object, e As EventArgs) Handles cbxAveraging.CheckedChanged
        '正規分布に従うノイズであれば1/√Nでへるが
        '蛍光灯などの電源周波数由来は減らせないか？　素数のfpsにしてみるとか？
    End Sub

    ''' <summary>
    ''' 回転
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cbxRotation_CheckedChanged(sender As Object, e As EventArgs) Handles cbxRotation.CheckedChanged
        '画像の回転 data augmentationに相当
    End Sub

    ''' <summary>
    ''' 光の制御
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cbxLightCtrl_CheckedChanged(sender As Object, e As EventArgs) Handles cbxLightCtrl.CheckedChanged
        'シリアルポートの制御
        'ライトの方向（左右）と色（R,G,B）の制御
        'data augmentationに相当
    End Sub
End Class

Public Class ImageProcesser
    Public Sub New()

    End Sub

    Public Sub Add(ByVal mat As Mat)

    End Sub

    Public Function GetMat() As Mat

    End Function

End Class
