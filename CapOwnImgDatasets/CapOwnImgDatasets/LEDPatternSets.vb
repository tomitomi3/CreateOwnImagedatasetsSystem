Imports Newtonsoft.Json

Public Class LEDPatternSets
    Private Shared _pattern As New LEDPatternSets()
    Private Const SETTINGPATH As String = "LEDPatterns.txt"

    Public Property Patterns As New List(Of List(Of Byte))

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
    Public Shared Function GetInstance() As LEDPatternSets
        Return _pattern
    End Function

    ''' <summary>
    ''' update settings
    ''' </summary>
    Public Sub Update()
        Using sw As New System.IO.StreamWriter(SETTINGPATH, False, System.Text.Encoding.GetEncoding("shift_jis"))
            Dim setting = LEDPatternSets.GetInstance()
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
        LEDPatternSets._pattern = JsonConvert.DeserializeObject(Of LEDPatternSets)(tempJson)
    End Sub
End Class
