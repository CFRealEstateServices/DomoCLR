Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web
Imports DOMO_CLR.DOMO_CLR
Imports Newtonsoft.Json

Public Class DOMOServiceBase
    Private Const DOMO_HOST As String = "https://api.domo.com"

    Protected Shared Sub AddCommonHeaders(auth As DOMOAuthorization, req As WebRequest, Optional AcceptText As String = "application/json", Optional ContentType As String = "application/json")
        req.Headers("Authorization") = String.Format("bearer {0}", auth.OAuthAccessToken)
        DirectCast(req, HttpWebRequest).Accept = AcceptText
        If (ContentType <> "") Then
            req.ContentType = ContentType
        End If
    End Sub

    Protected Shared Sub AddObjectToRequestContent(req As WebRequest, obj As Object)
        Dim nvCollection As NameValueCollection = HttpUtility.ParseQueryString(String.Empty)

        Dim dataToPost As Byte() = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj))

        Dim ds As Stream = req.GetRequestStream()
        ds.Write(dataToPost, 0, dataToPost.Length)
    End Sub

    Protected Shared Sub HandleWebException(webException As WebException)
        If (webException.Response IsNot Nothing) Then
            Dim exceptionString As String = ""

            Using excReader As New StreamReader(webException.Response.GetResponseStream())
                exceptionString += String.Format("Response from server: {0}{1}", excReader.ReadToEnd(), System.Environment.NewLine)
            End Using

            Throw New System.Exception(exceptionString)
        Else
            Throw webException
        End If
    End Sub
End Class
