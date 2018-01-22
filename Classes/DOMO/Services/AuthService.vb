Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server
Imports Newtonsoft.Json

Public Class AuthService
    Inherits DOMOServiceBase
    Public Shared Function GetDataAccessToken(DOMODomain As String) As DOMOAuthorization
        Return GetAccessToken(DOMODomain, True)
    End Function

    Public Shared Function GetUserAccessToken(DOMODomain As String) As DOMOAuthorization
        Return GetAccessToken(DOMODomain, False)
    End Function

    Private Shared Function GetAccessToken(DOMODomain As String, IsDataScope As Boolean) As DOMOAuthorization
        Dim clientID As String
        Dim clientSecret As String
        Dim scopeString As String = If(IsDataScope, "data", "user")
        Dim ret As DOMOAuthorization = New DOMOAuthorization()

        Using conn As SqlConnection = CLRConnection.GetConnection()
            Dim accessTokenSQL As String = String.Format("select AccessToken from _DOMOAccessTokens where DOMODomain = '{0}' and Scope = '{1}' and ExpiresAt >= getdate()", DOMODomain, scopeString)
            'SqlContext.Pipe.Send(accessTokenSQL)
            Dim accessTokenDT As DataTable = CLRConnection.ExecuteReader(accessTokenSQL, conn)

            If (accessTokenDT.Rows.Count > 0) Then
                ret.OAuthAccessToken = accessTokenDT.Rows(0)("AccessToken").ToString()
            Else
                Dim dt As DataTable = CLRConnection.ExecuteReader(String.Format("select ClientID, ClientSecret from _DOMOSettings where DOMODomain = '{0}' and Scope = '{1}'", DOMODomain, scopeString), conn)

                If (dt.Rows.Count > 0) Then
                    clientID = dt.Rows(0)("ClientID").ToString()
                    clientSecret = dt.Rows(0)("ClientSecret").ToString()
                Else
                    Throw New Exception("It looks like you are missing a ClientID and ClientSecret in the dbo._DOMOSettings table.  Go to developer.domo.com to get credentials.")
                End If

                Dim req As WebRequest = WebRequest.Create("https://api.domo.com/oauth/token?grant_type=client_credentials&scope=" & scopeString)
                req.Method = "GET"
                req.Headers("Authorization") = String.Format("Basic {0}", Base64Encode(clientID & ":" & clientSecret))

                Try
                    Using response As WebResponse = req.GetResponse()
                        Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                            Dim JSONObj As Dictionary(Of String, String) = JsonConvert.DeserializeObject(Of Dictionary(Of String, String))(sr.ReadToEnd())

                            ret.OAuthAccessToken = JSONObj("access_token")
                            Dim expiresInSeconds As Integer = Integer.Parse(JSONObj("expires_in").ToString())

                            Dim accessTokenSaveSQL As String = String.Format("insert into _DOMOAccessTokens values ('{0}', '{1}', '{2}', '{3}', '{4}')", ret.OAuthAccessToken, DateTime.Now, DateTime.Now.AddSeconds(expiresInSeconds - 5), DOMODomain, scopeString)
                            'SqlContext.Pipe.Send(accessTokenSaveSQL)
                            CLRConnection.ExecuteSQL(accessTokenSaveSQL, conn)
                        End Using
                    End Using
                Catch ex As WebException
                    HandleWebException(ex)
                    Return Nothing
                End Try
            End If

            conn.Close()
        End Using

        Return ret
    End Function

    Private Shared Function Base64Encode(plainText As String) As String
        Dim plainTextBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(plainText)
        Return System.Convert.ToBase64String(plainTextBytes)
    End Function
End Class
