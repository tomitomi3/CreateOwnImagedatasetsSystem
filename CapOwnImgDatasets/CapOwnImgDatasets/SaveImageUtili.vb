Imports System.IO

Public Class SaveImageUtili
    ''' <summary>対応表（正解名:ファイルNo）</summary>
    Private _correctVsNo As New Dictionary(Of String, ULong)

    Public Property SaveDirectory As String = String.Empty

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' 初期化
    ''' </summary>
    ''' <param name="saveFolderPath"></param>
    Public Function Init(ByVal saveFolderPath As String) As Boolean
        If System.IO.Directory.Exists(saveFolderPath) = False Then
            Return False
        End If
        Me.SaveDirectory = saveFolderPath

        '初期化
        Me._correctVsNo.Clear()

        'フォルダ名を探索
        Dim allDir = System.IO.Directory.GetDirectories(SaveDirectory, "*", SearchOption.TopDirectoryOnly)

        'フォルダなし
        If allDir.Length = 0 Then
            Return False
        End If

        'フォルダあり
        For i As Integer = 0 To allDir.Length - 1
            Dim allFile = Directory.GetFiles(allDir(i), "*", SearchOption.AllDirectories)
            Dim strKey = allDir(i).Replace(SaveDirectory + "\", "")

            Me.Add(strKey, 0)
            If allFile.Count = 0 Then
                'ファイルがあった
            Else
                Array.Sort(allFile)
                Dim recentFileName = IO.Path.GetFileNameWithoutExtension(allFile(allFile.Count - 1))
                Dim nextNo As ULong = 0
                If ULong.TryParse(recentFileName, nextNo) = False Then
                    '文字列が混ざっている
                    Me.Add(strKey, 0)
                Else
                    Me.Add(strKey, nextNo + 1)
                End If
            End If
        Next

        Return True
    End Function

    ''' <summary>
    ''' 保存
    ''' </summary>
    ''' <param name="imgFormat"></param>
    ''' <param name="correctFolderName"></param>
    ''' <param name="tempBmp"></param>
    Public Sub Save(ByVal imgFormat As MainWindow.EnumOutpuImageFormat, ByVal correctFolderName As String, tempBmp As Bitmap)
        'get file no
        Dim nextNo As ULong = 0
        If IsInclude(correctFolderName) = True Then
            'exit table
            nextNo = _correctVsNo(correctFolderName)
        Else
            'not exit table
            Me.Add(correctFolderName, 0)
        End If

        'パスの生成と保存
        Dim saveDirPath As String = String.Format("{0}\{1}", SaveDirectory, correctFolderName)
        If System.IO.Directory.Exists(saveDirPath) = False Then
            System.IO.Directory.CreateDirectory(saveDirPath)
        End If

        Dim savePath As String = String.Empty
        If imgFormat = MainWindow.EnumOutpuImageFormat.PNG Then
            savePath = String.Format("{0}\{1:D6}.png", saveDirPath, nextNo)
            tempBmp.Save(savePath, Imaging.ImageFormat.Png)
        ElseIf imgFormat = MainWindow.EnumOutpuImageFormat.JPEG Then
            savePath = String.Format("{0}\{1:D6}.jpg", saveDirPath, nextNo)
            tempBmp.Save(savePath, Imaging.ImageFormat.Jpeg)
        Else
            savePath = String.Format("{0}\{1:D6}.bmp", saveDirPath, nextNo)
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

#Region "Public Shared"
    ''' <summary>
    ''' Exeのパスを取得
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetExePath() As String
        Return IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
    End Function

    ''' <summary>
    ''' 指定ディレクトリに指定フォルダを作成する
    ''' </summary>
    ''' <param name="rootFolderPath"></param>
    ''' <param name="correctFolderName"></param>
    ''' <returns></returns>
    Public Shared Function GetFullPathWithCorrectName(ByVal rootFolderPath As String, ByVal correctFolderName As String) As String
        If System.IO.Directory.Exists(rootFolderPath) = False Then
            Return ""
        End If

        '末尾のバックスラッシュを削除
        If (rootFolderPath.Length - 1) = (rootFolderPath.LastIndexOf("\")) Then
            rootFolderPath = rootFolderPath.Remove(rootFolderPath.LastIndexOf("\"), 1)
        End If
        Dim tempPath = String.Format("{0}\{1}", rootFolderPath, correctFolderName)
        If System.IO.Directory.Exists(tempPath) = False Then
            System.IO.Directory.CreateDirectory(tempPath)
        End If

        Return tempPath
    End Function
#End Region
End Class
