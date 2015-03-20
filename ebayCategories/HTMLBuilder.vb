Imports System.Xml
Public Class HTMLBuilder
    Function BuildTreeByID(referenceID As Integer) As Integer
        Dim resultValue As Integer = 0
        Dim FILE_NAME As String = referenceID.ToString + ".html"
        Dim htmlCode As String
        'Write the HTML tags and CSS up to the body start

        Dim objWriter As New System.IO.StreamWriter(FILE_NAME, True)
        Dim sb As New System.Text.StringBuilder()

        Dim dataBaseManager = New DataBaseManager()
        Dim levelresult As Integer

        levelresult = dataBaseManager.findCategoryLevelByID(referenceID)

        If levelresult <> 0 Then
            resultValue = 1
            Dim childList As New List(Of DataRow)
            childList = dataBaseManager.ReturnAllChildsByID(levelresult, referenceID)
            Dim referenceCategory As DataRow = dataBaseManager.ReturnDataByID(levelresult, referenceID)
            Dim index As Integer = 0
            htmlCode = "<!DOCTYPE html> <html> <head> <title>Lists, Tables and Forms</title> <style type=""text/css""> body { font-family: Arial, Verdana, sans-serif; font-size: 90%; color: #666; background-color: #f8f8f8;} li { list-style-image: url(""images/icon-plus.png""); line-height: 1.6em;} .title { float: left; width: 160px; clear: left;} .submit { width: 310px; text-align: right;} </style> </head> <body> <h1>Ebay categories Rooted at Category: " + referenceCategory.Item(1).ToString + "</h1> <p>This Information was taken from the database update on: " + dataBaseManager.ReturnDataBaseData + " </p> <p> If you have further enquires sen me an email to : felipe@fsarmiento.com </p>"
            htmlCode = sb.Append(htmlCode).Append("<ul><li> ID : ").Append(referenceID.ToString).Append(referenceCategory.Item(1).ToString).Append("</li>").ToString
            htmlCode = sb.Append("<ul>").ToString
            For Each child As DataRow In childList
                htmlCode = sb.Append("<li>").Append("ID : ").Append(childList(index).Item(0).ToString).Append(" : ").Append(childList(index).Item(1).ToString).Append("</li>").ToString
                Dim childList2 As New List(Of DataRow)
                If (levelresult + 1 <= 6) Then
                    childList2 = dataBaseManager.ReturnAllChildsByID(levelresult + 1, CInt(childList(index).Item(0).ToString))
                End If
                htmlCode = sb.Append("<ul>").ToString
                Dim index2 As Integer = 0
                For Each child2 As DataRow In childList2
                    htmlCode = sb.Append("<li>").Append("ID : ").Append(childList2(index2).Item(0).ToString).Append(" : ").Append(childList2(index2).Item(1).ToString).Append("</li>").ToString
                    Dim childList3 As New List(Of DataRow)
                    If (levelresult + 2 <= 6) Then
                        childList3 = dataBaseManager.ReturnAllChildsByID(levelresult + 2, CInt(childList2(index2).Item(0).ToString))
                    End If
                    htmlCode = sb.Append("<ul>").ToString
                    Dim index3 As Integer = 0
                    For Each child3 As DataRow In childList3
                        htmlCode = sb.Append("<li>").Append("ID : ").Append(childList3(index3).Item(0).ToString).Append(" -: ").Append(childList3(index3).Item(1).ToString).Append("</li>").ToString
                        Dim childList4 As New List(Of DataRow)
                        If (levelresult + 3 <= 6) Then
                            childList4 = dataBaseManager.ReturnAllChildsByID(levelresult + 3, CInt(childList3(index3).Item(0).ToString))
                        End If
                        htmlCode = sb.Append("<ul>").ToString
                        Dim index4 As Integer = 0
                        For Each child4 As DataRow In childList4
                            htmlCode = sb.Append("<li>").Append("ID : ").Append(childList4(index4).Item(0).ToString).Append(" - Cat: ").Append(childList4(index4).Item(1).ToString).Append("</li>").ToString
                            Dim childList5 As New List(Of DataRow)
                            If (levelresult + 4 <= 6) Then
                                childList5 = dataBaseManager.ReturnAllChildsByID(levelresult + 4, CInt(childList4(index4).Item(0).ToString))
                            End If
                            htmlCode = sb.Append("<ul>").ToString
                            Dim index5 As Integer = 0
                            For Each child5 As DataRow In childList5
                                htmlCode = sb.Append("<li>").Append("ID : ").Append(childList5(index5).Item(0).ToString).Append(" : ").Append(childList5(index5).Item(1).ToString).Append("</li>").ToString
                                index5 = index5 + 1
                            Next
                            htmlCode = sb.Append("</ul>").ToString
                            index4 = index4 + 1
                        Next
                        htmlCode = sb.Append("</ul>").ToString
                        index3 = index3 + 1
                    Next
                    htmlCode = sb.Append("</ul>").ToString
                    index2 = index2 + 1
                Next
                htmlCode = sb.Append("</ul>").ToString

                index = index + 1
            Next
            htmlCode = sb.Append("</ul>").ToString
            'Close The Body Section
            htmlCode = sb.Append("</ul></body> </html>").ToString()
            objWriter.WriteLine(htmlCode)
            objWriter.Close()
            BuildTreeByID = resultValue
            Console.WriteLine(FILE_NAME)
        Else
            Console.WriteLine("No category with ID: " + referenceID.ToString)
        End If



        BuildTreeByID = resultValue

    End Function


End Class
