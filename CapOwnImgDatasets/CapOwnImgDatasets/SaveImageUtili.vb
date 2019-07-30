Imports System.IO

Public Class SaveImageUtili
    Private _saveFolderPath As String = String.Empty

    ''' <summary>対応表（正解名:ファイルNo）</summary>
    Private _correctVsNo As New Dictionary(Of String, ULong)

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Exeのパスを取得
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetExePath() As String
        Return IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    End Function

    ''' <summary>
    ''' 初期化
    ''' </summary>
    ''' <param name="rootFolderPath"></param>
    ''' <param name="correctFolderName"></param>
    Public Sub Init(ByVal rootFolderPath As String, ByVal correctFolderName As String)
        Dim tempPath = String.Format("{0}\{1}", rootFolderPath, correctFolderName)
        If System.IO.Directory.Exists(tempPath) = False Then
            System.IO.Directory.CreateDirectory(tempPath)
        End If
        _saveFolderPath = tempPath

        '初期化
        _correctVsNo.Clear()

        'フォルダ名を探索
        Dim allDir = System.IO.Directory.GetDirectories(_saveFolderPath, "*", SearchOption.TopDirectoryOnly)

        'フォルダなし
        If allDir.Length = 0 Then
            Return
        End If

        'フォルダあり
        For i As Integer = 0 To allDir.Length - 1
            Dim allFile = Directory.GetFiles(allDir(i), "*", SearchOption.AllDirectories)
            Dim strKey = allDir(i).Replace(tempPath + "\", "")

            Me.Add(strKey, 0)
            If allFile.Count = 0 Then
                'ファイルがあった
            Else
                Array.Sort(allFile)
                Dim recentFileName = IO.Path.GetFileNameWithoutExtension(allFile(allFile.Count - 1))
                Dim recentNo As ULong = 0
                If ULong.TryParse(recentFileName, recentNo) = False Then
                    '文字列が混ざっている
                    Me.Add(strKey, 0)
                Else
                    Me.Add(strKey, recentNo)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' 保存パスを取得
    ''' </summary>
    ''' <returns></returns>
    Public Function GetSaveFolder() As String
        Return _saveFolderPath
    End Function

    ''' <summary>
    ''' 保存
    ''' </summary>
    ''' <param name="imgFormat"></param>
    ''' <param name="correctFolderName"></param>
    ''' <param name="tempBmp"></param>
    Public Sub Save(ByVal imgFormat As MainWindow.EnumOutpuImageFormat, ByVal correctFolderName As String, tempBmp As Bitmap)
        'get recent file no
        Dim recentNo As ULong = 0
        If IsInclude(correctFolderName) = True Then
            'exit table
            recentNo = _correctVsNo(correctFolderName)
        Else
            'not exit table
            Me.Add(correctFolderName, 0)
        End If

        'パスの生成と保存
        Dim saveDirPath As String = String.Format("{0}\{1}", _saveFolderPath, correctFolderName)
        If System.IO.Directory.Exists(saveDirPath) = False Then
            System.IO.Directory.CreateDirectory(saveDirPath)
        End If

        Dim savePath As String = String.Empty
        If imgFormat = MainWindow.EnumOutpuImageFormat.PNG Then
            savePath = String.Format("{0}\{1:D6}.png", saveDirPath, recentNo)
            tempBmp.Save(savePath, Imaging.ImageFormat.Png)
        ElseIf imgFormat = MainWindow.EnumOutpuImageFormat.JPEG Then
            savePath = String.Format("{0}\{1:D6}.jpg", saveDirPath, recentNo)
            tempBmp.Save(savePath, Imaging.ImageFormat.Jpeg)
        Else
            savePath = String.Format("{0}\{1:D6}.bmp", saveDirPath, recentNo)
            tempBmp.Save(savePath, Imaging.ImageFormat.Bmp)
        End If
        _correctVsNo(correctFolderName) += 1 'increment no
    End Sub

    ''' <summary>
    ''' ヘルパー関数 正解名と番号の対応表に指定した正解名が含まれているか
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <returns></returns>
    Private Function IsInclude(ByVal strKey As String) As Boolean
        Dim val As ULong = 0
        If _correctVsNo.TryGetValue(strKey, val) = False Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' ヘルパー関数 正解名と番号の対応表に指定した正解名と値を追加
    ''' </summary>
    ''' <param name="strKey"></param>
    ''' <param name="val"></param>
    Private Sub Add(ByVal strKey As String, ByVal val As ULong)
        If IsInclude(strKey) = True Then
            _correctVsNo(strKey) = val
        Else
            _correctVsNo.Add(strKey, val)
        End If
    End Sub
End Class
