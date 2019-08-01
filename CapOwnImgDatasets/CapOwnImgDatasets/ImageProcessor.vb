Imports OpenCvSharp

Public Class ImageProcesser
    ''' <summary></summary>
    Public Property InputMat As Mat = Nothing

    ''' <summary></summary>
    Public Property IsRotation As Boolean = False

    ''' <summary></summary>
    Public Property RotationStep As Integer = 0

    ''' <summary></summary>
    Public Property IsMove As Boolean = False

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

    End Sub

    ''' <summary>
    ''' 指定した条件でMatを取得
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMats() As List(Of Mat)
        If InputMat Is Nothing Then
            Return Nothing
        End If

        'Mats
        Dim retMats As New List(Of Mat)

        '切り抜きサイズ
        Dim diffSize = CInt(InputMat.Width - Me.ClipSize)
        Dim diffSizeHalf = CInt(InputMat.Width - Me.ClipSize) / 2.0
        Dim clipRect = New Rect(New Point(diffSizeHalf, diffSizeHalf), New Size(Me.ClipSize, Me.ClipSize))

        '1枚
        retMats.Add(ResizeAndColor(InputMat(clipRect)))

        '----------------------------------------
        'Rotation
        '----------------------------------------
        If Me.IsRotation Then
            Dim numRotate As Integer = (359 / Me.RotationStep - 0.5)
            For i As Integer = 1 To numRotate - 1
                Using dst As New Mat()
                    Dim stepAngle = i * Me.RotationStep
                    Dim center As New Point2f(InputMat.Width / 2.0, InputMat.Height / 2.0)
                    Dim rotationMat = Cv2.GetRotationMatrix2D(center, stepAngle, 1.0)
                    Cv2.WarpAffine(InputMat, dst, rotationMat, InputMat.Size())
                    Dim resizedMat = Me.ResizeAndColor(dst(clipRect))
                    retMats.Add(resizedMat)
                End Using
            Next
        End If

        '----------------------------------------
        'Move
        '----------------------------------------
        If IsMove = True Then
            Dim rnd As New Random(System.DateTime.Now.Millisecond) '一様分布でランダム移動
            For i As Integer = 0 To Me.NumOfMove - 1
                'ランダムに平行移動
                Dim tempX = rnd.Next(0, diffSize)
                Dim tempY = rnd.Next(0, diffSize)
                Dim tempRect = New Rect(New Point(tempX, tempY), New Size(Me.ClipSize, Me.ClipSize))
                Dim resizedMat = Me.ResizeAndColor(InputMat(tempRect))
                retMats.Add(resizedMat)
            Next
        End If

        '----------------------------------------
        'Flip
        '----------------------------------------
        If IsFlip = True Then
            Dim tempMats = GetFlipMats(InputMat(clipRect))
            retMats.AddRange(tempMats)
        End If
        If IsFlip = True AndAlso IsMove = True Then
            Dim startIdx = retMats.Count - Me.NumOfMove - 3
            For i As Integer = 0 To Me.NumOfMove - 1
                Dim idx = startIdx + i
                Dim tempMats = GetFlipMats(retMats(idx))
                retMats.AddRange(tempMats)
            Next
        End If

        Return retMats
    End Function

    Private Function GetFlipMats(ByRef tempMat As Mat) As List(Of Mat)
        Dim retMats As New List(Of Mat)
        Dim flipMat = New Mat()
        Cv2.Flip(tempMat, flipMat, FlipMode.X)
        retMats.Add(flipMat)
        Cv2.Flip(tempMat, flipMat, FlipMode.Y)
        retMats.Add(flipMat)
        Cv2.Flip(tempMat, flipMat, FlipMode.XY)
        retMats.Add(flipMat)

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
                   interpolation:=InterpolationFlags.Cubic)

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
