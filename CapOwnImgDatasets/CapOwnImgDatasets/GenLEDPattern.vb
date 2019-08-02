''' <summary>
''' LED Pattern
''' </summary>
Public Class GenLEDPattern
    Public Property Brightness As Integer = 0
    Public Property NUM_OF_LED As Integer = 5

    Public Sub New()
        'nop
    End Sub

    Public Const IDX_BRIGHTNESS As Integer = 0
    Public Const SIZE_CH_COLOR As Integer = 4
    Public Const IDX_CH0 As Integer = 1
    Public Const IDX_CH1 As Integer = IDX_CH0 + SIZE_CH_COLOR
    Public Const IDX_CH2 As Integer = IDX_CH1 + SIZE_CH_COLOR
    Public Const IDX_CH3 As Integer = IDX_CH2 + SIZE_CH_COLOR
    Public Const IDX_CH4 As Integer = IDX_CH3 + SIZE_CH_COLOR

    ''' <summary>
    ''' 初期データ生成
    ''' </summary>
    ''' <param name="numOfLed"></param>
    ''' <param name="brightness"></param>
    ''' <returns></returns>
    Public Shared Function GetInitSendData(ByVal numOfLed As Integer, ByVal brightness As Integer) As List(Of Byte)
        Dim ret = New List(Of Byte)
        'Brightness
        ret.Add(brightness)

        'CH
        Dim ch As Integer = 0
        For i As Integer = 0 To (numOfLed * SIZE_CH_COLOR) - 1
            If (i Mod SIZE_CH_COLOR) = 0 Then
                ret.Add(ch)
                ch += 1
            Else
                ret.Add(0)
            End If
        Next

        Return ret
    End Function

    ''' <summary>
    ''' Pattern
    ''' </summary>
    ''' <returns></returns>
    Public Function GetPattern() As List(Of List(Of Byte))
        Dim colorPattern = New List(Of List(Of Byte))

        '何もなし
        With Nothing
            Dim tempBytes = GetInitSendData(NUM_OF_LED, Brightness)
            colorPattern.Add(tempBytes)
        End With

        'table
        With Nothing
            Dim tempBytes = GetInitSendData(NUM_OF_LED, Brightness)
            tempBytes(IDX_CH4 + 1) = 64
            tempBytes(IDX_CH4 + 2) = 128
            tempBytes(IDX_CH4 + 3) = 96
            colorPattern.Add(tempBytes)
        End With

        '上
        With Nothing
            Dim tempBytes = GetInitSendData(NUM_OF_LED, Brightness)
            tempBytes(IDX_CH0 + 1) = 128
            tempBytes(IDX_CH0 + 2) = 0
            tempBytes(IDX_CH0 + 3) = 0
            tempBytes(IDX_CH1 + 1) = 128
            tempBytes(IDX_CH1 + 2) = 0
            tempBytes(IDX_CH1 + 3) = 0
            tempBytes(IDX_CH4 + 1) = 64
            tempBytes(IDX_CH4 + 2) = 128
            tempBytes(IDX_CH4 + 3) = 96
            colorPattern.Add(tempBytes)
        End With

        With Nothing
            Dim tempBytes = GetInitSendData(NUM_OF_LED, Brightness)
            tempBytes(IDX_CH0 + 1) = 0
            tempBytes(IDX_CH0 + 2) = 128
            tempBytes(IDX_CH0 + 3) = 0
            tempBytes(IDX_CH1 + 1) = 0
            tempBytes(IDX_CH1 + 2) = 128
            tempBytes(IDX_CH1 + 3) = 0
            tempBytes(IDX_CH4 + 1) = 64
            tempBytes(IDX_CH4 + 2) = 128
            tempBytes(IDX_CH4 + 3) = 96
            colorPattern.Add(tempBytes)
        End With

        With Nothing
            Dim tempBytes = GetInitSendData(NUM_OF_LED, Brightness)
            tempBytes(IDX_CH0 + 1) = 0
            tempBytes(IDX_CH0 + 2) = 0
            tempBytes(IDX_CH0 + 3) = 128
            tempBytes(IDX_CH1 + 1) = 0
            tempBytes(IDX_CH1 + 2) = 0
            tempBytes(IDX_CH1 + 3) = 128
            tempBytes(IDX_CH4 + 1) = 64
            tempBytes(IDX_CH4 + 2) = 128
            tempBytes(IDX_CH4 + 3) = 96
            colorPattern.Add(tempBytes)
        End With

        Return colorPattern
    End Function

    ''' <summary>
    ''' 1CHのLED制御
    ''' </summary>
    ''' <param name="ch"></param>
    ''' <param name="r"></param>
    ''' <param name="g"></param>
    ''' <param name="b"></param>
    Public Function GenSinglePattern(ByVal ch As Integer,
                                 ByVal r As Integer, ByVal g As Integer, ByVal b As Integer) As List(Of Byte)
        Dim p = GenLEDPattern.GetInitSendData(1, Brightness)
        p.Add(Me.Brightness)
        p.Add(ch)
        p.Add(r)
        p.Add(g)
        p.Add(b)
        Return p
    End Function

    ''' <summary>
    ''' 同じパターンを生成
    ''' </summary>
    ''' <param name="numOfLed"></param>
    ''' <param name="brightness"></param>
    ''' <param name="r"></param>
    ''' <param name="g"></param>
    ''' <param name="b"></param>
    ''' <returns></returns>
    Public Shared Function GenSamePattern(numOfLed As Integer, brightness As Integer, r As Integer, g As Integer, b As Integer) As List(Of Byte)
        Dim tempBytes = GenLEDPattern.GetInitSendData(numOfLed, brightness)
        'Protocol
        '             0   R  G  B  1   R  G  B
        '[Brightness][CH][R][G][B][CH][R][G][B]
        For i As Integer = 0 To numOfLed - 1
            Dim idx = SIZE_CH_COLOR * i + IDX_CH0
            tempBytes(idx + 1) = r
            tempBytes(idx + 2) = g
            tempBytes(idx + 3) = b
        Next

        Return tempBytes
    End Function
End Class
