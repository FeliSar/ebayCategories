Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Data.SQLite


Module MainModule

    Sub Main(ByVal cmdArgs() As String)

        If cmdArgs.Count <> 0 Then

            If (String.Compare(cmdArgs(0), "--rebuild") = 0) Then
                Console.WriteLine("Regenerating Data Base")
                Dim categoriesArray As ebay.CategoryType()
                categoriesArray = MakeApiRequest()

                Dim dataBaseManager = New DataBaseManager()

                dataBaseManager.DataBaseConnection()
                dataBaseManager.UpdateDataBase(categoriesArray)


            ElseIf (String.Compare(cmdArgs(0), "--render") = 0) Then
                Dim htmlBuilder As New HTMLBuilder
                htmlBuilder.BuildTreeByID(CInt(cmdArgs(1)))


            Else
                Console.WriteLine("Please Select a Valid Option (--rebuild or --render <id>)")

            End If
        End if

    End Sub


  

    Public Function MakeApiRequest() As ebay.CategoryType()
        ' Dim endpoint As String =http://open.api.ebay.com/Shopping 
        Dim endpoint As String = "https://api.sandbox.ebay.com/wsapi"
        Dim callName As String = "GetCategories"
        Dim appId As String = "EchoBay62-5538-466c-b43b-662768d6841"    ' PolymathVent Id
        Dim devId As String = "16a26b1b-26cf-442d-906d-597b60c41c19"    ' PolymathVent Id
        Dim certId As String = "00dd08ab-2082-4e3c-9518-5f4298f296d"    ' PolymathVent Id
        Dim version As String = "911"
        Dim authToken As String = "AgAAAA**AQAAAA**aAAAAA**t2XTUQ**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4GhCpaCpQWdj6x9nY+seQ**L0MCAA**AAMAAA**pZOn+3Cb/dnuil4E90EEeGpHlaBVP0VpLebK58TPQ210Sn33HEvjGYoC9UYVqfbhxte6wp8/fPL795uVh9/4X00HC3wAfzcV+wobN2NfReqWAXFdfuj4CbTHEzIHVLJ8tApLPlI8Nxq6oCa5KsZf5L+An85i2BnohCfscJtl9OcZYnyWnD0oA4R3zdnH3dQeKRTxws/SbVCTgWcMXBqL9TUr4CrnOFyt0BdYp4lxg0HbMv1akuz+U7wQ3aLxJeFoUow20kUtVoTIDhnpfZ40Jcl/1a2ui0ha3rl9D3oA66PUhHSnHJTznwtp1pFLANWn9I49l9rrYbzzobB6SGf18LK/5cqSwse3AWMXJkFVbgFM7e5DZBv59S1sCRdEjzw8GciKYSxGDqu8UJQHgL/QPiTFhtj2Ad/vjZ/6PLBVA9rhOxJnlhCvLXmWZIf1NNcckN8uEEIqK7Wn0DdDi8p44ozIWNaIQ319HjYYOBp4a5FLUjwXCamoqfSjYli5ikqe0jwn+LxnOWblY47TFhruRQpYaBAro4VbgirwNYT7RlEGz+u7ol9A873dnqEZgdXWfrWkyxyKGeXHnHjiynfL/JDCdl2U2s+s5iOd8hp6QklHevPOlOtZgW+K/eFIv53UATQi4vMptUKEeD6QxFzvxP7wRAkKIQZUq+LKB8lZBP/Epjni47HXKpwQdgbTWbyfHpSF3A52u8koUY9chiBk1FCpqjBM/BT5tjhIlrQUVeWUUyGeQ49sJJvaeVnaavo9"

        ' call Arguments

        Dim siteID As String = "0" 'USA site
        Dim viewAllNodes As Boolean = True
        Dim detailLevel() As ebay.DetailLevelCodeType = {ebay.DetailLevelCodeType.ReturnAll, 0}
        'Build The URL request
        Dim requestURL = endpoint _
                         + "?callname=" + callName _
                         + "&siteid=" + siteID _
                         + "&appid=" + appId _
                         + "&version=" + version _
                         + "&routing=default"
        'create the service
        Dim service As ebay.eBayAPIInterfaceService = New ebay.eBayAPIInterfaceService()

        'Assign the request URL to the service Locator
        service.Url = requestURL

        'Set the credentials
        service.RequesterCredentials = New ebay.CustomSecurityHeaderType()
        service.RequesterCredentials.eBayAuthToken = authToken
        service.RequesterCredentials.Credentials = New ebay.UserIdPasswordType()
        service.RequesterCredentials.Credentials.AppId = appId
        service.RequesterCredentials.Credentials.DevId = devId
        service.RequesterCredentials.Credentials.AuthCert = certId

        'Make the call to GetCategories
        Dim request As ebay.GetCategoriesRequestType = New ebay.GetCategoriesRequestType()

        'specify the request arguments
        request.Version = version
        request.CategorySiteID = siteID
        request.DetailLevel = detailLevel
        request.ViewAllNodes = viewAllNodes
        'Perform the call and receive response
        Dim response As ebay.GetCategoriesResponseType = service.GetCategories(request)

        Dim DataBaseManager = New DataBaseManager()

        DataBaseManager.UpdateDabaseInfo(response.Timestamp, response.Build, endpoint)
        MakeApiRequest = response.CategoryArray

    End Function

End Module
