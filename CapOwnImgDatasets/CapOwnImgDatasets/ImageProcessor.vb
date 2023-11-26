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

        ' 戻りMat
        Dim retMats As New List(Of Mat)

        ' 切り抜きサイズ
        Dim diffSize = CInt(InputMat.Width - Me.ClipSize)
        Dim diffSizeHalf = CInt(InputMat.Width - Me.ClipSize) / 2.0
        Dim clipRect = New Rect(New Point(diffSizeHalf, diffSizeHalf), New Size(Me.ClipSize, Me.ClipSize))

        ' 最初 画像
        retMats.Add(Me.ResizeAndColor(InputMat(clipRect)))

        ' 回転
        If Me.IsRotation = True Then
            Dim rMats = Me.DoRotation(Me.InputMat, Me.RotationStep, clipRect)
            retMats.AddRange(rMats)
        End If

        ' ランダムムーブ
        If Me.IsMove = True Then
            Dim rMats = Me.DoRandomMove(Me.InputMat, Me.NumOfMove, clipRect, diffSize)
            retMats.AddRange(rMats)
        End If

        ' Flip
        If Me.IsFlip = True Then
            Dim rMats = Me.DoFlip(Me.InputMat)
            retMats.AddRange(rMats)
        End If

        ''----------------------------------------
        '' Rotation, Random Move 組み合わせ
        ''----------------------------------------
        'If IsRotation = True And IsMove = True Then
        '    'Rotation & Move
        '    Dim numRotate As Integer = 360 / Me.RotationStep
        '    For rotate As Integer = 1 To numRotate - 1
        '        Using dst As New Mat()
        '            Dim stepAngle = rotate * Me.RotationStep
        '            '回転に対して少しのランダム性を加える
        '            'stepAngle += Util.XorShiftSingleton.GetInstance().Next(0, 2)
        '            Dim center As New Point2f(InputMat.Width / 2.0, InputMat.Height / 2.0)
        '            Dim rotationMat = Cv2.GetRotationMatrix2D(center, stepAngle, 1.0)
        '            Cv2.WarpAffine(InputMat, dst, rotationMat, InputMat.Size())

        '            For randMove As Integer = 0 To Me.NumOfMove - 1
        '                'ランダムに平行移動
        '                Dim tempX = Util.XorShiftSingleton.GetInstance().Next(0, diffSize)
        '                Dim tempY = Util.XorShiftSingleton.GetInstance().Next(0, diffSize)
        '                Dim tempRect = New Rect(New Point(tempX, tempY), New Size(Me.ClipSize, Me.ClipSize))
        '                retMats.Add(Me.ResizeAndColor(dst(tempRect)))
        '            Next
        '        End Using
        '    Next
        'ElseIf IsRotation = True And IsMove = False Then
        '    'Rotation
        '    Dim numRotate As Integer = 360 / Me.RotationStep
        '    For rotate As Integer = 1 To numRotate - 1
        '        Using dst As New Mat()
        '            Dim stepAngle = rotate * Me.RotationStep
        '            '回転に対して少しのランダム性を加える
        '            'stepAngle += Util.XorShiftSingleton.GetInstance().Next(0, 2)
        '            Dim center As New Point2f(InputMat.Width / 2.0, InputMat.Height / 2.0)
        '            Dim rotationMat = Cv2.GetRotationMatrix2D(center, stepAngle, 1.0)
        '            Cv2.WarpAffine(InputMat, dst, rotationMat, InputMat.Size())
        '            retMats.Add(Me.ResizeAndColor(dst(clipRect)))
        '        End Using
        '    Next
        'ElseIf IsRotation = False And IsMove = True Then
        '    'Move
        '    For randMove As Integer = 0 To Me.NumOfMove - 1
        '        'ランダムに平行移動
        '        Dim tempX = Util.XorShiftSingleton.GetInstance().Next(0, diffSize)
        '        Dim tempY = Util.XorShiftSingleton.GetInstance().Next(0, diffSize)
        '        Dim tempRect = New Rect(New Point(tempX, tempY), New Size(Me.ClipSize, Me.ClipSize))
        '        retMats.Add(Me.ResizeAndColor(InputMat(tempRect)))
        '    Next
        'End If

        ''----------------------------------------
        ''Flip
        ''----------------------------------------
        'If IsFlip = True Then
        '    Dim tempMats = GetFlipMats(InputMat(clipRect))
        '    retMats.AddRange(tempMats)
        'End If
        'If IsFlip = True AndAlso IsMove = True Then
        '    Dim startIdx = retMats.Count - Me.NumOfMove - 3
        '    For i As Integer = 0 To Me.NumOfMove - 1
        '        Dim idx = startIdx + i
        '        Dim tempMats = GetFlipMats(retMats(idx))
        '        retMats.AddRange(tempMats)
        '    Next
        'End If

        Return retMats
    End Function

    ''' <summary>
    ''' 回転
    ''' </summary>
    ''' <param name="inputMat"></param>
    ''' <param name="rotationStep"></param>
    ''' <param name="clipRect"></param>
    ''' <returns></returns>
    Public Function DoRotation(ByVal inputMat As Mat, ByVal rotationStep As Double,
                                    ByVal clipRect As Rect) As List(Of Mat)
        Dim rotatedMats As New List(Of Mat)
        Dim numRotate As Double = 360.0 / rotationStep

        For rotate As Integer = 1 To numRotate - 1
            Using dst As New Mat()
                Dim stepAngle = rotate * rotationStep
                ' 入力画像の中心で回転
                Dim center As New Point2f(inputMat.Width / 2.0, inputMat.Height / 2.0)
                Dim rotationMat = Cv2.GetRotationMatrix2D(center, stepAngle, 1.0)
                Cv2.WarpAffine(inputMat, dst, rotationMat, inputMat.Size())
                rotatedMats.Add(Me.ResizeAndColor(dst(clipRect)))
            End Using
        Next

        Return rotatedMats
    End Function

    ''' <summary>
    ''' フリップ
    ''' </summary>
    ''' <param name="inputMat"></param>
    ''' <returns></returns>
    Public Function DoFlip(ByRef inputMat As Mat) As List(Of Mat)
        Dim retMats As New List(Of Mat)

        Dim flipMatX = New Mat()
        Cv2.Flip(inputMat, flipMatX, FlipMode.X)
        retMats.Add(Me.ResizeAndColor(flipMatX))

        Dim flipMatY = New Mat()
        Cv2.Flip(inputMat, flipMatY, FlipMode.Y)
        retMats.Add(Me.ResizeAndColor(flipMatY))

        Dim flipMatXY = New Mat()
        Cv2.Flip(inputMat, flipMatXY, FlipMode.X)
        retMats.Add(Me.ResizeAndColor(flipMatXY))

        Return retMats
    End Function

    ''' <summary>
    ''' ランダムムーブ
    ''' </summary>
    ''' <param name="tMat"></param>
    ''' <param name="numOfMove"></param>
    ''' <param name="clipRect"></param>
    ''' <param name="diffSize"></param>
    ''' <returns></returns>
    Public Function DoRandomMove(ByRef tMat As Mat, ByVal numOfMove As Integer,
                                      ByRef clipRect As Rect, ByVal diffSize As Integer) As List(Of Mat)
        Dim retMats As New List(Of Mat)

        For i As Integer = 0 To numOfMove - 1
            Dim tempX = Util.XorShiftSingleton.GetInstance().Next(0, diffSize)
            Dim tempY = Util.XorShiftSingleton.GetInstance().Next(0, diffSize)
            Dim tempRect = New Rect(New Point(tempX, tempY), New Size(clipRect.Width, clipRect.Height))
            retMats.Add(Me.ResizeAndColor(InputMat(tempRect)))
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
