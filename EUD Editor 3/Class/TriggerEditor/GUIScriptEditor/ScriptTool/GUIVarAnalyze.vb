﻿Partial Public Class GUIScriptManager

    Private Function AddAble(cname As String, curtype As String, fname As String, ftype As String) As Boolean
        If fname.Trim <> "" Then
            If cname <> fname Then
                Return False
            End If
        End If
        If ftype.Trim <> "" Then
            If curtype <> ftype Then
                Return False
            End If
        End If


        Return True
    End Function

    '현재 위치로부터 변수들을 구하는 함수들을 만들것
    Public Function GetLocalVar(normalscr As ScriptBlock, Optional type As String = "", Optional varname As String = "") As List(Of ScriptBlock)
        Dim fname As String = varname
        Dim ftype As String = type

        Dim rscr As New List(Of ScriptBlock)
        If normalscr Is Nothing Then
            Return rscr
        End If

        If normalscr.isfolder Then
            If normalscr.child.Count >= 1 Then


                normalscr = normalscr.child.Last
            End If
        End If

        While normalscr.Parent IsNot Nothing
            Dim index As Integer = normalscr.Parent.child.IndexOf(normalscr)
            For i = index To 0 Step -1
                If normalscr.Parent.child(i).ScriptType = ScriptBlock.EBlockType.vardefine Then
                    Dim cname As String = normalscr.Parent.child(i).value
                    Dim curtype As String = normalscr.Parent.child(i).value2

                    If AddAble(cname, curtype, fname, ftype) Then
                        rscr.Add(normalscr.Parent.child(i))
                    End If

                End If
            Next
            normalscr = normalscr.Parent

            'If normalscr.ScriptType = ScriptBlock.EBlockType._for Then
            '    Dim values As String() = normalscr.ForvarName
            '    For i = 0 To values.Count - 1


            '        If varname.Trim = "" Then
            '            Dim tscr As ScriptBlock = New ScriptBlock(ScriptBlock.EBlockType.vardefine, "vardefine", False, False, values(i).Trim, Nothing)
            '            tscr.value2 = "var"
            '            rscr.Add(tscr)
            '        Else
            '            If values(i).Trim = varname Then
            '                Dim tscr As ScriptBlock = New ScriptBlock(ScriptBlock.EBlockType.vardefine, "vardefine", False, False, values(i).Trim, Nothing)
            '                tscr.value2 = "var"
            '                rscr.Add(tscr)
            '            End If
            '        End If
            '        'MsgBox(values(i))
            '    Next
            'End If
        End While

        '인덱스가 0이 될대까지 위로 올라감.
        '인덱스가 0이 되면

        Return rscr
    End Function
    Public Function GetGlobalVar(GUIEditor As GUIScriptEditor, Optional type As String = "", Optional varname As String = "") As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        Dim fname As String = varname
        Dim ftype As String = type
        For i = 0 To GUIEditor.ItemCount - 1
            If GUIEditor.GetItems(i).ScriptType = ScriptBlock.EBlockType.vardefine Then
                Dim cname As String = GUIEditor.GetItems(i).value
                Dim curtype As String = GUIEditor.GetItems(i).value2
                If AddAble(cname, curtype, fname, ftype) Then
                    rscr.Add(GUIEditor.GetItems(i))
                End If
            End If
        Next

        Return rscr
    End Function
    Public Function GetExternVar(GUIEditor As GUIScriptEditor, Optional type As String = "", Optional varname As String = "") As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        Dim fname As String = varname
        Dim ftype As String = type
        For i = 0 To GUIEditor.ExternFile.Count - 1
            For j = 0 To GUIEditor.ExternFile(i).Funcs.VariableCount - 1
                Dim vname As String = GUIEditor.ExternFile(i).Funcs.GetVariableNames(j)
                Dim vtype As String = GUIEditor.ExternFile(i).Funcs.GetVariableType(j)



                Dim scrtype As ScriptBlock.EBlockType
                Dim cname As String = GUIEditor.ExternFile(i).nameSpaceName & "." & vname
                Dim curtype As String 'value2


                If vtype.Trim = "" Then
                    '타입이 없음 = 일반 var
                    'ScriptBlock.EBlockType.vardefine
                    scrtype = ScriptBlock.EBlockType.vardefine
                    curtype = "var"
                Else
                    '타입이 있음 = const var
                    If IsNumeric(vtype) Then
                        '일반 값이 들어있는 상수변수
                        scrtype = ScriptBlock.EBlockType.vardefine
                        curtype = "const"
                    Else
                        '오브젝트
                        scrtype = ScriptBlock.EBlockType.vardefine
                        curtype = "object"


                    End If
                End If

                Dim scr As New ScriptBlock(scrtype, "vardefine", False, False, cname, GUIEditor)
                scr.value2 = curtype

                If AddAble(cname, curtype, fname, ftype) Then
                    rscr.Add(scr)
                End If

                'MsgBox(vname & "," & vtype)
            Next

        Next


        Return rscr
    End Function



    Public Function GetAllVar(normalscr As ScriptBlock, GUIEditor As GUIScriptEditor) As List(Of ScriptBlock)
        Dim rscr As New List(Of ScriptBlock)

        rscr.AddRange(GetLocalVar(normalscr))
        rscr.AddRange(GetGlobalVar(GUIEditor))
        rscr.AddRange(GetExternVar(GUIEditor))


        Return rscr
    End Function

End Class
