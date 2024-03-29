﻿Imports Newtonsoft.Json

Public Class SettingFile
    Private Shared _settings As New SettingFile()

    Public Property IsAverage As Boolean = True
    Public Property NumAve As Integer = 2
    Public Property IsLEDCtrl As Boolean = False
    Public Property IsRotation As Boolean = True
    Public Property RotationStep As Integer = 30
    Public Property IsMove As Boolean = False
    Public Property NumMoveImgs As Integer = 10
    Public Property IsFlip As Boolean = False
    Public Property IsConvertGrayScale As Boolean = False

    Private Const SETTINGPATH As String = "CaptureSetting.txt"

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Private Sub New()
        'nothing
    End Sub

    ''' <summary>
    ''' get instance(singleton)
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetInstance() As SettingFile
        Return _settings
    End Function

    ''' <summary>
    ''' update settings
    ''' </summary>
    Public Sub Update()
        Using sw As New System.IO.StreamWriter(SETTINGPATH, False, System.Text.Encoding.GetEncoding("shift_jis"))
            Dim setting = SettingFile.GetInstance()
            Dim output As String = JsonConvert.SerializeObject(setting, Formatting.Indented)
            sw.Write(output)
        End Using
    End Sub

    ''' <summary>
    ''' load settings
    ''' </summary>
    Public Sub Load()
        If System.IO.File.Exists(SETTINGPATH) = False Then
            Return
        End If

        Dim readJson = New System.Text.StringBuilder()
        Using sw As New System.IO.StreamReader(SETTINGPATH, System.Text.Encoding.GetEncoding("shift_jis"))
            While (sw.EndOfStream = False)
                Dim temp = sw.ReadLine()
                readJson.Append(temp)
            End While
        End Using
        Dim tempJson = readJson.ToString()
        SettingFile._settings = JsonConvert.DeserializeObject(Of SettingFile)(tempJson)
    End Sub
End Class
