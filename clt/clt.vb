Imports System.Windows.Forms

Public Class clt

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Dim a As String
        a = inputFile.Text
        Dim File1 = IO.File.Open(a, IO.FileMode.Open, IO.FileAccess.Read)

        Dim bufferSize As Long = 100000
        Dim findwhat1 As Byte()
        findwhat1 = System.Text.Encoding.Unicode.GetBytes(findstart.Text)
        'findwhat1 = System.Text.Encoding.ASCII.GetBytes(findstart.Text)

        Dim findwhat2 As Byte()
        findwhat2 = System.Text.Encoding.Unicode.GetBytes(findend.Text)
        'findwhat2 = System.Text.Encoding.ASCII.GetBytes(findend.Text)


        Dim writeBuffer = New Byte(10000) {}
        Dim wp = 0

        Dim bytes = New Byte(bufferSize - 1) {}
        Dim p1 As Long = 0
        Dim pl1 As Long = findwhat1.Length

        Dim p2 As Long = 0
        Dim pl2 As Long = findwhat2.Length

        Dim offset As Long = 0
        Dim start As Long = -1
        Dim endpos As Long = -1
        Dim i As Long
        Dim sOffset As Long = Val(SearchOffset.Text)

        While File1.Read(bytes, 0, bufferSize) > 0

            If endpos = -1 Then
                Label6.Text = "Offset: " + offset.ToString
                System.Windows.Forms.Application.DoEvents()
            End If

            If offset >= sOffset Then

                For i = 0 To bufferSize - 1

                    If start = -1 Then
                        If bytes(i) = findwhat1(p1) Then
                            p1 = p1 + 1
                            If p1 = pl1 Then
                                'found
                                start = i + offset - pl1 + 1
                                Label5.Text = "Found start at: " + start.ToString
                                writeBuffer = findwhat1
                                wp = pl1 - 1
                                p1 = 0

                            End If
                            
                        Else
                            p1 = 0
                        End If
                    End If
                    If start > -1 Then
                        Dim k As Long = endpos - start
                        If endpos > -1 Then
                            If wp >= k Then
                                Exit While
                            End If
                        End If


                        If wp >= writeBuffer.Length Then
                            ReDim Preserve writeBuffer(writeBuffer.Length + bufferSize)
                        End If
                        writeBuffer(wp) = bytes(i) ' - pl1 + 1)
                        wp = wp + 1



                    End If

                    If start > -1 And endpos = -1 Then
                        If bytes(i) = findwhat2(p2) Then
                            p2 = p2 + 1
                            If p2 = pl2 Then
                                'found end
                                endpos = i + offset + 1
                                Label6.Text = "Found end at: " + endpos.ToString

                            End If

                            If p2 = pl2 Then
                                p2 = 0
                            End If
                        Else
                            p2 = 0
                        End If
                    Else


                    End If
                Next

            End If
            'If p = pl Then
            ' Exit While
            ' End If




            offset = offset + bufferSize
        End While



        File1.Close()

        If start > -1 And endpos > -1 Then
            'Save file


            If System.IO.File.Exists(outputFile.Text) = True Then

                System.IO.File.Delete(outputFile.Text)
            End If


            Dim File2 = IO.File.Open(outputFile.Text, IO.FileMode.CreateNew, IO.FileAccess.Write)

            File2.Write(writeBuffer, 0, wp)


            File2.Close()

        End If

    End Sub



    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
        inputFile.Text = OpenFileDialog1.FileName


    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        SaveFileDialog1.ShowDialog()
        outputFile.Text = SaveFileDialog1.FileName


    End Sub




    Private Sub clt_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles findstart.TextChanged, findend.TextChanged

    End Sub
End Class
