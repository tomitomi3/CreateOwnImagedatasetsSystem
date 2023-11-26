Imports OpenCvSharp

Public Class ImageProcesser
    ''' <summary></summary>
    Public Property InputMat As Mat = Nothing

    ''' <summary></summary>
    Public Property IsRotation As Boolean = False

    ''' <summary></summary>
    Public Property RotationStep As Integer = 0

    ''' <summary></summary>
    Public Property IsRandomMove As Boolean = False

    ''' <summary></summary>
    Public Property NumOfMove As Integer = 0

    ''' <summary></summary>
    Public Property IsFlip As Boolean = False

    ''' <summary></summary>
    Public Property IsColor As Boolean = True

    ''' <summary></summary>
    Public Property ImageSize As Integer = 0

    ''' <summary></summary>
    Public Property ClipSize As Integer = 0

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    Public Sub New()
        ' 乱数seedをtimeから取得
        Util.XorShiftSingleton.GetInstance().SetSeed(System.DateTime.Now.Millisecond)

        ' 100回ほど回す
        For Each a In Enumerable.Range(0, 100)
            Util.XorShiftSingleton.GetInstance().Next()
        Next
    End Sub

    ''' <summary>
    ''' 指定した条件でMatを取得
    ''' </summary>
    ''' <returns></returns>
    Public Function GetImageProcessedMats() As List(Of Mat)
        If InputMat Is Nothing Then
            Return Nothing
        End If

        ' 戻りMat
        Dim retMats As New List(Of Mat)

        ' 切り抜きサイズ
        Dim diffSize = CInt(InputMat.Width - Me.ClipSize)
        Dim diffSizeHalf = CInt(InputMat.Width - Me.ClipSize) / 2.0
        Dim clipRect = New Rect(New Point(diffSizeHalf, diffSizeHalf), New Size(Me.ClipSize, Me.ClipSize))

        ' 1つめの画像
        Dim processedMats As New List(Of Mat) From {Me.InputMat}

        ' 回転
        If Me.IsRotation Then
            Dim tempRotationMats As New List(Of Mat)
            For Each tempMat In processedMats
                Dim numRotate As Double = 360.0 / Me.RotationStep
                For rotate As Integer = 1 To numRotate - 1
                    Using dst As New Mat()
                        Dim stepAngle = rotate * RotationStep
                        ' 入力画像の中心で回転
                        Dim center As New Point2f(tempMat.Width / 2.0, tempMat.Height / 2.0)
                        Dim rotationMat = Cv2.GetRotationMatrix2D(center, stepAngle, 1.0)
                        Cv2.WarpAffine(tempMat, dst, rotationMat, tempMat.Size())
                        tempRotationMats.Add(dst.Clone())
                    End Using
                Next
            Next
            processedMats.AddRange(tempRotationMats)
        End If

        ' ランダムムーブ
        If Me.IsRandomMove Then
            Dim tempMoveMats As New List(Of Mat)
            For Each tempMat In processedMats
                For i As Integer = 0 To NumOfMove - 1
                    Dim tempX = Util.XorShiftSingleton.GetInstance().Next(0, diffSize / 1.2)
                    Dim tempY = Util.XorShiftSingleton.GetInstance().Next(0, diffSize / 1.2)
                    Dim tempRect = New Rect(New Point(tempX, tempY), New Size(clipRect.Width, clipRect.Height))
                    tempMoveMats.Add(tempMat(tempRect))
                Next
            Next
            processedMats.AddRange(tempMoveMats)
        End If

        ' Flip
        If Me.IsFlip Then
            Dim tempFlipMats As New List(Of Mat)
            For Each tempMat In processedMats
                Dim flipMatX = New Mat()
                Cv2.Flip(tempMat.Clone(), flipMatX, FlipMode.Y)
                tempFlipMats.Add(flipMatX)

                Dim flipMatY = New Mat()
                Cv2.Flip(tempMat.Clone(), flipMatY, FlipMode.X)
                tempFlipMats.Add(flipMatY)

                Dim flipMatXY = New Mat()
                Cv2.Flip(tempMat.Clone(), flipMatXY, FlipMode.XY)
                tempFlipMats.Add(flipMatXY)
            Next
            processedMats.AddRange(tempFlipMats)
        End If

        ' resize and convert grayscale
        For Each tempMat In processedMats
            retMats.Add(Me.ResizeAndColor(tempMat))
        Next

        Return retMats
    End Function

    ''' <summary>
    ''' 指定サイズでの縮小と色変換
    ''' </summary>
    ''' <param name="tempMat"></param>
    ''' <returns></returns>
    Private Function ResizeAndColor(ByRef tempMat As Mat) As Mat
        Dim resizeMat As New Mat()
        Cv2.Resize(tempMat, resizeMat,
                   New OpenCvSharp.Size(Me.ImageSize, Me.ImageSize),
                   interpolation:=InterpolationFlags.Linear)

        If Me.IsColor = True Then
            Return resizeMat
        Else
            'Convert grayscale
            Dim grayMat = New Mat()
            Cv2.CvtColor(resizeMat, grayMat, ColorConversionCodes.BGRA2GRAY)
            resizeMat.Dispose()
            resizeMat = Nothing
            Return grayMat
        End If
    End Function
End Class
