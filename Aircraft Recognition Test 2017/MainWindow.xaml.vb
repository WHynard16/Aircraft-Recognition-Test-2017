Imports System.Environment, System.IO
Class MainWindow
    Dim MPlayer As New MediaPlayer
    Dim filepath As String
    Dim NumCorrect As Integer
    Dim ImgNum As Integer
    Dim UsedQ(29) As Integer
    Dim Limit As Integer
    Dim QNum As Integer
    Dim ImageIdentifier As String
    Dim WithEvents Timer1 As New Windows.Forms.Timer
    Dim Elapsed(1) As Single 'Elapsed(0) = Mins; Elapsed(1) = Secs;

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Elapsed(1) = Elapsed(1) + 0.1
        If Elapsed(1) >= 60 Then
            Elapsed(0) = Elapsed(0) + 1
            Elapsed(1) = 0
        End If
        If Elapsed(0) = 3 Then
            ElapsedTimeTxt.Foreground = New SolidColorBrush(Color.FromRgb(208, 0, 0)) 'Dark Red
        End If
        If Elapsed(0) = 3 And Elapsed(1) >= 10 Then
            Timer1.Enabled = False
            Finish(True)
        End If
        ElapsedTimeTxt.Text = Format(Elapsed(0), "00") & ":" & Format(Elapsed(1), "00.0")


    End Sub

    Private Sub StartBtn_Click(sender As Object, e As RoutedEventArgs) Handles StartBtn.Click


        filepath = GetFolderPath(SpecialFolder.Desktop)

        'copy Audio to Documents
        Dim ms As New MemoryStream
        My.Resources.BEETHOVEN.CopyTo(ms)
        Dim AudioFile() As Byte = ms.ToArray
        File.WriteAllBytes(filepath & "\BEETHOVEN.wav", AudioFile)
        ms.Close()

        MPlayer.Open(New Uri(filepath & "\BEETHOVEN.wav", UriKind.Relative))
        MPlayer.Volume = 0.3



        QNum = 1
        EnableOptnButtons(True)

        SelectImage()
        RefreshImage()

        StartBtn.IsEnabled = False

        StartTimer()
        MPlayer.Play()

    End Sub

    Private Sub QuitBtn_Click(sender As Object, e As RoutedEventArgs) Handles QuitBtn.Click
        If Timer1.Enabled = True Then
            ResetTest()
            MPlayer.Stop()
            MPlayer.Close()
            My.Computer.FileSystem.DeleteFile(filepath & "\BEETHOVEN.wav", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        Else
            Me.Close()
        End If
    End Sub
    Sub ResetTest()
        Timer1.Enabled = False
        StartBtn.IsEnabled = True
        QImage.Source = Nothing
        EnableOptnButtons(False)
        ElapsedDisplayGrid.Visibility = Windows.Visibility.Hidden
    End Sub
    Sub SelectImage()
        ImageIdentifier = ""
    End Sub

    Sub RefreshImage()
        If QNum <= 30 Then
            QImage.Source = New BitmapImage(New Uri("Resources/" & ImageIdentifier & ".jpg", UriKind.Relative))
            QImage.Visibility = Windows.Visibility.Visible
        Else
            QImage.Visibility = Windows.Visibility.Hidden
        End If
    End Sub
    Sub StartTimer()
        ElapsedDisplayGrid.Visibility = Windows.Visibility.Visible
        Elapsed(0) = 0
        Elapsed(1) = 0
        ElapsedTimeTxt.Text = "00:00.0"

        Timer1.Interval = 100
        Timer1.Enabled = "True"
    End Sub
    Sub ButtonCLicked(ByVal SenderButton As Integer)
        QNum = QNum + 1
        If QNum >= 31 Then
            Finish(False)
        Else
            SelectImage()
            RefreshImage()
        End If

    End Sub

    Private Sub OptnBtn1_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn1.Click
        ButtonCLicked(1)
    End Sub
    Private Sub OptnBtn2_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn2.Click
        ButtonCLicked(2)
    End Sub
    Private Sub OptnBtn3_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn3.Click
        ButtonCLicked(3)
    End Sub
    Private Sub OptnBtn4_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn4.Click
        ButtonCLicked(4)
    End Sub
    Private Sub OptnBtn5_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn5.Click
        ButtonCLicked(5)
    End Sub
    Private Sub OptnBtn6_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn6.Click
        ButtonCLicked(6)
    End Sub
    Private Sub OptnBtn7_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn7.Click
        ButtonCLicked(7)
    End Sub
    Private Sub OptnBtn8_Click(sender As Object, e As RoutedEventArgs) Handles OptnBtn8.Click
        ButtonCLicked(8)
    End Sub
    Sub Finish(ByVal OutOfTime As Boolean)
        ResetTest()

        If OutOfTime = True Then
            MsgBox("Time's Up!")
        End If

    End Sub
    Function EnableOptnButtons(ByVal Enable As Boolean)
        OptnBtn1.IsEnabled = Enable
        OptnBtn2.IsEnabled = Enable
        OptnBtn3.IsEnabled = Enable
        OptnBtn4.IsEnabled = Enable
        OptnBtn5.IsEnabled = Enable
        OptnBtn6.IsEnabled = Enable
        OptnBtn7.IsEnabled = Enable
        OptnBtn8.IsEnabled = Enable
        Return Nothing
    End Function
    Function IsQuestionUsed(ByVal QtoCheck As Integer) As Boolean
        If UsedQ.Contains(QtoCheck) = True Then
            IsQuestionUsed = True
        Else
            IsQuestionUsed = False
            UsedQ(QNum - 1) = ImgNum
        End If
    End Function
    Sub Show_Results()
        Dim FinalScore As Single
        Dim TimeOver As Single
        Dim TotalTime As Single

        NumCorrectTxtVari.Text = NumCorrect
        TimeTakenTxtVari.Text = Format(Elapsed(0), "00") & ":" & Format(Elapsed(1), "00.0")

        TotalTime = CInt((Elapsed(0) * 60) + Math.Truncate(Elapsed(1)))
        If TotalTime > 130 Then
            TimeOver = TotalTime - 130
            FinalScore = TimeOver * 0.1
        Else
            FinalScore = NumCorrect
        End If
        ScoreTxtVari.Text = FinalScore


        ViewNumCorrect.Visibility = Windows.Visibility.Visible
        ViewNumCorrectVari.Visibility = Windows.Visibility.Visible
        ViewTimeTaken.Visibility = Windows.Visibility.Visible
        ViewTimeTakenVari.Visibility = Windows.Visibility.Visible
        ViewScore.Visibility = Windows.Visibility.Visible
        ViewScoreVari.Visibility = Windows.Visibility.Visible
    End Sub

End Class
