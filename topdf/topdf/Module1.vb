Imports Pechkin.Synchronized
Imports Pechkin
Imports System.Text
Imports System.IO

Module Module1

    Sub Main()
        Dim pdfbuffer As Byte() = New SimplePechkin(New GlobalConfig()).Convert("<table border = 1><tr><td>iMa></td><td>Test</td></tr></table>")
        Dim dir = "D:\\"
        Dim fname = "test.pdf"
        Dim isconvert = ByteArraytoFile(dir + fname, pdfbuffer)
        If isconvert Then
            Console.Write("suksesss")
        Else
            Console.Write("gagal")
        End If
    End Sub

    Private Function ByteArraytoFile(v As String, pdfbuffer() As Byte) As Boolean
        Try
            Dim fs As FileStream = New FileStream(v, FileMode.Create, FileAccess.Write)
            fs.Write(pdfbuffer, 0, pdfbuffer.Length)
            fs.Close()
            Return True
        Catch ex As Exception
            Console.WriteLine("error : " & ex.Message)
        End Try
    End Function
End Module
