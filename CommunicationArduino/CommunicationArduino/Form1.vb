Imports System.IO.Ports
Imports System.Threading

Public Class frmMain
    Declare Function AllocConsole Lib "kernel32" () As Int32
    Private oSerialPort As SerialPort = Nothing
    Private myThread As Thread = Nothing

    ''' <summary>To Send Arduino data</summary>
    Private _sendData As New List(Of Byte)

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AllocConsole()
        oSerialPort = New SerialPort()
        oSerialPort.BaudRate = 9600
        oSerialPort.StopBits = StopBits.One
        oSerialPort.RtsEnable = False
        oSerialPort.DataBits = 8
        oSerialPort.Parity = False

        Dim ports = System.IO.Ports.SerialPort.GetPortNames()
        For Each portName In ports
            Console.WriteLine("{0}", portName)
        Next
        If ports.Length <> 0 Then
            Me.tbxPort.Text = String.Format("{0}", ports(0))
        End If
    End Sub

    Private Sub frmMain_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        CloseProcess()
    End Sub

    Private _lock = New Object
    Private recvData As New List(Of Byte)
    Private Const ReadBufferSize As Integer = 32

    ''' <summary>
    ''' 受信用スレッド
    ''' </summary>
    Private Sub threadFunc()
        Dim recentReadCount As Integer = 10
        recvData.Clear()

        While (True)
            'バッファオーバーチェック
            If Me.oSerialPort.BytesToRead > (Me.oSerialPort.ReadBufferSize / 2) Then
                Exit While
            End If

            'データ読み込み
            SyncLock (_lock)
                Dim isRead As Boolean = True
                Dim temp() As Byte = Nothing
                Dim readSize As Integer = 0

                '受信サイズ計算
                Dim nowReadBufferSize As Integer = Me.oSerialPort.BytesToRead
                If nowReadBufferSize = 0 Then
                    isRead = False
                ElseIf nowReadBufferSize > ReadBufferSize Then
                    '指定したサイズ分をバッファから読み込む
                    ReDim temp(ReadBufferSize - 1)
                    readSize = ReadBufferSize
                ElseIf recentReadCount > 10 Then
                    'バッファサイズに変更がない＝バッファをすべて空にする
                    recentReadCount = 0
                    ReDim temp(nowReadBufferSize - 1)
                    readSize = nowReadBufferSize
                Else
                    recentReadCount += 1
                    isRead = False
                End If

                '受信
                If isRead = True Then
                    'Console.WriteLine(Me.oSerialPort.BytesToRead)

                    'read
                    For i As Integer = 0 To readSize - 1
                        i += oSerialPort.Read(temp, 0, readSize - i)
                    Next

                    'Console.WriteLine(Me.oSerialPort.BytesToRead)
                    For Each value In temp
                        recvData.Add(value)
                    Next
                    Console.WriteLine("ReadSize:{0}", recvData.Count)
                    Console.Write("ReadData:")
                    For Each tempValue In recvData
                        Console.Write("{0} ", tempValue)
                    Next
                    Console.WriteLine("")
                    recvData.Clear()
                End If
            End SyncLock

            Thread.Sleep(50)
        End While
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        If oSerialPort.IsOpen = True Then
            oSerialPort.Close()
        End If
    End Sub

    ''' <summary>
    ''' データ送信
    ''' 先頭 符号なし2byte分のチェックサムを付与
    ''' </summary>
    Private Sub SendArduinoWithCheckSum()
        If oSerialPort.IsOpen = False Then
            Return
        End If

        'check sum
        Dim sumSize As UInt16 = 0
        For i As Integer = 0 To _sendData.Count - 1
            sumSize += _sendData(i)
        Next
        Dim chkSumByte = BitConverter.GetBytes(sumSize)

        'add checksum
        Dim allSendByte As New List(Of Byte)
        allSendByte.Add(chkSumByte(0))
        allSendByte.Add(chkSumByte(1))
        For Each temp In _sendData
            allSendByte.Add(temp) 'data
        Next

        'debug
        Console.WriteLine("SendSize:{0}", allSendByte.Count)
        Console.WriteLine("CheckSum :{0}", sumSize)
        Console.WriteLine("CheckSum & 0xFF:{0}", sumSize And &HFF)
        For i As Integer = 0 To allSendByte.Count - 1
            Console.Write("{0} ", allSendByte(i))
        Next
        Console.WriteLine("")

        'write
        oSerialPort.Write(allSendByte.ToArray, 0, allSendByte.Count)
        System.Threading.Thread.Sleep(50)
    End Sub

    Private Sub CloseProcess()
        If oSerialPort.IsOpen Then
            'thread stop
            myThread.Abort()

            'close
            oSerialPort.Close()
        End If
    End Sub

    Private Sub btnOpenClose_Click(sender As Object, e As EventArgs) Handles btnOpenClose.Click
        If oSerialPort.IsOpen Then
            CloseProcess()
            Me.btnOpenClose.Text = "Open"
        Else
            Me.btnOpenClose.Text = "Close"
        End If

        Try
            Me.oSerialPort.PortName = Me.tbxPort.Text
            Me.oSerialPort.Open()

            'thread start
            Me.myThread = New Thread(AddressOf threadFunc)
            Me.myThread.IsBackground = True
            Thread.Sleep(500)
            Me.myThread.Start()
            Thread.Sleep(500)

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        If oSerialPort.IsOpen = False Then
            Return
        End If

        'ascii 場合
        'Dim sendByte = System.Text.Encoding.ASCII.GetBytes(Me.tbxSendMessage.Text)

        'byte
        _sendData.Clear()
        Dim split = Me.tbxSendMessage.Text.Split(",")
        For Each temp In split
            _sendData.Add(Byte.Parse(temp))
        Next

        SendArduinoWithCheckSum()
    End Sub

    Private Sub btnColorPick_Click(sender As Object, e As EventArgs) Handles btnColorPick.Click
        Dim cd As New ColorDialog()
        If cd.ShowDialog() = DialogResult.OK Then
            'update trackbar
            trkR.Value = cd.Color.R
            trkG.Value = cd.Color.G
            trkB.Value = cd.Color.B

            'update sendData
            _sendData.Clear()
            _sendData.Add(Me.trkBar.Value)
            _sendData.Add(0) 'ch0
            _sendData.Add(cd.Color.R)
            _sendData.Add(cd.Color.G)
            _sendData.Add(cd.Color.B)
            _sendData.Add(1) 'ch1
            _sendData.Add(cd.Color.R)
            _sendData.Add(cd.Color.G)
            _sendData.Add(cd.Color.B)

            '文字変換
            Dim tempStr As String = String.Empty
            For i As Integer = 0 To _sendData.Count - 1
                tempStr += _sendData(i).ToString()
                If i = _sendData.Count - 1 Then
                Else
                    tempStr += ","
                End If
            Next
            Me.tbxSendMessage.Text = tempStr
        End If
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        For i As Integer = 0 To 50
            _sendData.Clear()
            Dim split = Me.tbxSendMessage.Text.Split(",")
            For Each temp In split
                _sendData.Add(Byte.Parse(temp))
            Next
            _sendData(0) = trkBar.Value

            SendArduinoWithCheckSum()
        Next
    End Sub

    Private Sub TarckBarCtrl(ByVal series As Integer, ByVal value As Integer)
        'Text -> _sendData
        _sendData.Clear()
        Dim split = Me.tbxSendMessage.Text.Split(",")
        For Each temp In split
            _sendData.Add(Byte.Parse(temp))
        Next

        'get trackbar value
        'BRIGHTNESS CH1 R1 G1 B1 CH2 R2 G2 B2
        If (series = 0) Then
            _sendData(0) = value
        ElseIf (series = 1) Then
            _sendData(2) = value
            _sendData(6) = value
        ElseIf (series = 2) Then
            _sendData(3) = value
            _sendData(7) = value
        ElseIf (series = 3) Then
            _sendData(4) = value
            _sendData(8) = value
        End If

        '_sendData -> Text
        Dim tempStr As String = String.Empty
        For i As Integer = 0 To _sendData.Count - 1
            tempStr += _sendData(i).ToString()
            If i = _sendData.Count - 1 Then
            Else
                tempStr += ","
            End If
        Next
        Me.tbxSendMessage.Text = tempStr

        SendArduinoWithCheckSum()
    End Sub

    Private Sub trkBar_Scroll(sender As Object, e As EventArgs) Handles trkBar.Scroll
        TarckBarCtrl(0, Me.trkBar.Value)
    End Sub

    Private Sub trkR_Scroll(sender As Object, e As EventArgs) Handles trkR.Scroll
        TarckBarCtrl(1, Me.trkR.Value)
    End Sub

    Private Sub trkG_Scroll(sender As Object, e As EventArgs) Handles trkG.Scroll
        TarckBarCtrl(2, Me.trkG.Value)
    End Sub

    Private Sub trkB_Scroll(sender As Object, e As EventArgs) Handles trkB.Scroll
        TarckBarCtrl(3, Me.trkB.Value)
    End Sub

End Class
