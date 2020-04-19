﻿Public Class Tokenizer
    Private _str As String
    Private index As Long

    Public ReadOnly Property IsEnd As Boolean
        Get
          return  _str.Length <= index
        End Get
    End Property


    Public Sub New(str As String)
        _str = str
        index = 0
    End Sub

    Public Sub Back()
        index -= 1
    End Sub
    Public Function NextChar() As String
        If _str.Length > index Then
            Dim rstr As String = _str(index)
            index += 1
            Return rstr
        End If
        Throw New Exception("문자열의 끝에 도달했습니다.")

        Return ""
    End Function

    Public Function NextBlock() As String
        Dim c As String = NextChar()
        Dim r As String = ""

        While (Lexical_Analyzer.IsLetter(c) And Not IsEnd)
            r = r & c

            c = NextChar()
        End While
        index -= 1
        Return r
    End Function
    Public Function NextDigt() As String
        Dim c As String = NextChar()
        Dim r As String = ""

        While (IsNumeric(c) And Not IsEnd)
            r = r & c

            c = NextChar()
        End While
        index -= 1
        Return r
    End Function
    Public Function NextLine() As String
        Dim c As String = NextChar()
        Dim r As String = ""

        While (c <> vbCr And c <> vbLf And Not IsEnd)
            r = r & c

            c = NextChar()
        End While
        index -= 1
        Return r
    End Function
    Public Function NextHex() As String
        Dim c As String = NextChar()
        Dim r As String = ""

        While (Lexical_Analyzer.IsHexLetter(c) And Not IsEnd)
            r = r & c

            c = NextChar()
        End While
        index -= 1
        Return r
    End Function
End Class
