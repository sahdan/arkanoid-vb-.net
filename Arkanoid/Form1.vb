Public Class Form1

    Dim VSpeed As Single
    Dim HSpeed As Single
    Dim Rows As Integer = 7
    Dim Cols As Integer = 10
    Dim TopRow As Single = 0.15
    Dim RowHeight As Single = 0.05
    Dim Score As Integer = 0
    Dim arrBtn As List(Of Button) = New List(Of Button)
    Dim arrRectBtn As List(Of Rectangle) = New List(Of Rectangle)

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Check top of Screen
        If Ball.Top < MenuStrip1.Height + Label1.Height Then
            VSpeed = -VSpeed
        End If

        'Check left of Screen
        If Ball.Left < 0 Then
            HSpeed = -HSpeed
        End If

        'Check bottom of Screen
        If Ball.Bottom > Me.ClientRectangle.Height Then
            Timer1.Enabled = False
            Label1.Text = "Game Over"
        End If

        'Check rightof Screen
        If Ball.Right > Me.ClientRectangle.Width Then
            HSpeed = -HSpeed
        End If

        Dim C As Single = Ball.Left + Ball.Width / 2
        'Check for a Paddle hit
        If (C > Paddle.Left And C < Paddle.Right And
            VSpeed > 0 And Ball.Bottom > Paddle.Top _
            And Ball.Top < Paddle.Top) Then
            VSpeed = -VSpeed

            'Curve the paddle
            Dim Offset As Single = (Ball.Left + Ball.Width / 2) - (Paddle.Left + Paddle.Width / 2)
            Dim Ratio As Single = Offset / (Paddle.Width / 2)
            If Score > 30 Then
                HSpeed = 10 * Ratio
            Else
                HSpeed = 5 * Ratio
            End If
        End If


        'Dim lokasibola As New Rectangle(Ball.Location, Ball.Size)  'For Checking Using Rectangle Intersect Library

        If Ball.Top < Me.ClientRectangle.Height * 0.5 Then 'To check only if the ball is within Brick area, instead of everytime it moves
            'Check Using Rectangle Intersect Library
            'For index = 0 To arrRectBtn.Count - 1
            '    If (lokasibola.IntersectsWith(arrRectBtn(index))) And arrBtn(index).Visible = True Then
            '        VSpeed = -VSpeed

            '        If (arrBtn(index).Text.Equals("1")) Then
            '            arrBtn(index).Visible = False
            '            Score += arrBtn(index).Tag
            '            Label1.Text = Score.ToString
            '        Else
            '            Dim Point As Integer = arrBtn(index).Text
            '            Point -= 1
            '            arrBtn(index).Text = Point.ToString
            '            If (arrBtn(index).Text.Equals("2")) Then
            '                arrBtn(index).BackColor = Color.Yellow
            '            Else
            '                arrBtn(index).BackColor = Color.Red
            '            End If
            '        End If
            '    End If

            'Next

            For Each Cnt As Control In Me.Controls
                If Cnt.Name = "Brick" Then
                    CheckBrick(Cnt, Ball)
                End If
            Next
        End If
        Ball.Left += HSpeed
        Ball.Top += VSpeed
    End Sub

    Private Sub CheckBrick(ByVal Brick As Button, ByVal Ball As Button)

        If Brick.Visible = True Then
            Dim Hit As Boolean = False

            Dim C As Single = Ball.Left + Ball.Width / 2

            'Check Bottom of the Brick
            If (VSpeed < 0 And C > Brick.Left And C < Brick.Right And
                Ball.Top < Brick.Bottom And Ball.Bottom > Brick.Bottom) Then
                VSpeed = -VSpeed
                Hit = True
            End If

            'Check Top of the Brick
            If (VSpeed > 0 And C > Brick.Left And C < Brick.Right And
                Ball.Top < Brick.Top And Ball.Bottom > Brick.Top) Then
                VSpeed = -VSpeed
                Hit = True
            End If

            C = Ball.Top + Ball.Height / 2

            'Check Left of the Brick
            If (HSpeed > 0 And C > Brick.Top And C < Brick.Bottom And
                Ball.Left < Brick.Left And Ball.Right > Brick.Left) Then
                HSpeed = -HSpeed
                Hit = True
            End If

            'Check Right of the Brick
            If (HSpeed < 0 And C > Brick.Top And C < Brick.Bottom And
                Ball.Left < Brick.Right And Ball.Right > Brick.Right) Then
                HSpeed = -HSpeed
                Hit = True
            End If

            'Remove the brick when applicable
            If Hit Then
                If (Brick.Text.Equals("1")) Then
                    Brick.Visible = False
                    Score += Brick.Tag
                    Label1.Text = Score.ToString
                Else
                    'Change the color of brick
                    Dim Point As Integer = Brick.Text
                    Point -= 1
                    Brick.Text = Point.ToString
                    If (Brick.Text.Equals("3")) Then
                        Brick.BackColor = Color.Green
                    ElseIf (Brick.Text.Equals("2"))
                        Brick.BackColor = Color.Yellow
                    ElseIf (Brick.Text.Equals("1")) Then
                        Brick.BackColor = Color.Red
                    End If
                End If
                'Increasing Ball Speed after getting score above 30
                If (Score = 190) Then
                    Timer1.Enabled = False
                    Label1.Text = "Congratulation, You Win!"
                ElseIf Score > 90 Then
                    If VSpeed < 0 Then
                        VSpeed = -16
                    Else
                        VSpeed = 16
                    End If
                ElseIf Score > 60 Then
                    If VSpeed < 0 Then
                        VSpeed = -12
                    Else
                        VSpeed = 12
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        Dim C As Single = 0.15 * Me.ClientRectangle.Width
        If (e.X > C And e.X < Me.ClientRectangle.Width - C) Then
            Paddle.Left = e.X - C
        End If
    End Sub

    Private Sub MakeBricks()


        'Clear old Bricks
        For i As Integer = Me.Controls.Count - 1 To 0 Step -1
            If Me.Controls(i).Name = "Brick" Then
                Me.Controls.RemoveAt(i)
            End If
        Next

        arrRectBtn.Clear()
        arrBtn.Clear()

        'Setting Paddle
        Paddle.Width = 0.3 * Me.ClientRectangle.Width
        Paddle.Height = 0.05 * Me.ClientRectangle.Height
        Paddle.Top = 0.95 * Me.ClientRectangle.Height

        For Row As Integer = 0 To Rows - 1
            For Col As Integer = 0 To Cols - 1
                Dim B As New Button
                Me.Controls.Add(B)
                B.Visible = True
                B.Name = "Brick"
                B.Text = 3
                B.Tag = Rows - Row
                B.Width = Me.ClientRectangle.Width / Cols
                B.Height = Me.ClientRectangle.Height * RowHeight
                B.Left = Col * B.Width
                B.Top = Me.ClientRectangle.Height * (TopRow + Row * RowHeight)
                arrRectBtn.Add(New Rectangle(B.Location, B.Size))
                arrBtn.Add(B)
                If (Row < 2) Then
                    B.BackColor = Color.Gray
                    B.Tag = 5
                    B.Text = 5

                ElseIf (Row < 4) Then
                    B.BackColor = Color.Green
                    B.Tag = 3
                    B.Text = 3
                Else
                    B.BackColor = Color.Red
                    B.Tag = 1
                    B.Text = 1
                End If
            Next
        Next

        With Ball
            .Left = Me.ClientRectangle.Width / 2
            .Top = Me.ClientRectangle.Height * 0.9 + 20
            VSpeed = -8
            HSpeed = 8
        End With

        Score = 0
        Label1.Text = 0
        Timer1.Enabled = True

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        MakeBricks()
    End Sub

    Private Sub RestartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartToolStripMenuItem.Click
        MakeBricks()
    End Sub

    Private Sub Form1_Click(sender As Object, e As EventArgs) Handles Me.Click
        If (Timer1.Enabled = False) Then
            Timer1.Enabled = True
        Else
            Timer1.Enabled = False
        End If
    End Sub
End Class
