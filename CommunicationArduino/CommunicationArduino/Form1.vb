Imports System.IO.Ports
Imports System.Threading

Public Class frmMain
    Declare Function AllocConsole Lib "kernel32" () As Int32
    Private oSerialPort As SerialPort = Nothing
    Private myThread As Thread = Nothing

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If oSerialPort.IsOpen Then
            'thread stop
            myThread.Abort()

            'close
            oSerialPort.Close()
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

    Private _lock = New Object
    Private recvData As New List(Of Byte)
    Private Const ReadBufferSize As Integer = 32

    Private Sub threadFunc()
        Dim recentReadCount As Integer = 10
        recvData.Clear()

        While (True)
            'バッファサイズのオーバー疑い
            If Me.oSerialPort.BytesToRead > (Me.oSerialPort.ReadBufferSize / 2) Then
                Exit While
            End If

            'データ読み込み
            SyncLock (_lock)
                Dim isRead As Boolean = True
                Dim temp() As Byte = Nothing
                Dim readSize As Integer = 0
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

                '受信データをデシリアライズ
                If isRead = True Then
                    Console.WriteLine(Me.oSerialPort.BytesToRead)

                    'read
                    For i As Integer = 0 To readSize - 1
                        i += oSerialPort.Read(temp, 0, readSize - i)
                    Next

                    Console.WriteLine(Me.oSerialPort.BytesToRead)
                    For Each value In temp
                        recvData.Add(value)
                    Next

                    Console.WriteLine("Size:{0}", recvData.Count)
                    For Each tempValue In recvData
                        'Console.WriteLine("{0}", tempValue)
                    Next
                    recvData.Clear()
                End If
            End SyncLock

            Thread.Sleep(50)
        End While
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If oSerialPort.IsOpen = False Then
            Return
        End If


    End Sub
End Class
