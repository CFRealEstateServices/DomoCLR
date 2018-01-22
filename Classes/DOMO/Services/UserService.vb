Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Net
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server
Imports Newtonsoft.Json

Public Class UserService
    Inherits DOMOServiceBase
    Public Shared Function UsersList(auth As DOMOAuthorization, Limit As Integer, Offset As Integer) As DataTable
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/users?sort=name&fields=all&limit={0}&offset={1}", Limit, Offset))
        req.Method = "GET"
        AddCommonHeaders(auth, req)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim users As List(Of DOMOExistingUser) = JsonConvert.DeserializeObject(Of List(Of DOMOExistingUser))(sr.ReadToEnd())

                    Return DataTableUtils.ConvertToDataTable(users)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function UsersCreate(auth As DOMOAuthorization, Email As String, Role As String, Name As String, SendInvite As Boolean) As DataTable
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/users?sendInvite={0}", If(SendInvite, "true", "false")))
        req.Method = "POST"
        AddCommonHeaders(auth, req)

        Dim userToAdd As New DOMONewUser()
        userToAdd.email = Email
        userToAdd.name = Name
        userToAdd.role = Role
        AddObjectToRequestContent(req, userToAdd)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim responseString As String = sr.ReadToEnd()

                    Dim retUser As DOMOExistingUser = JsonConvert.DeserializeObject(Of DOMOExistingUser)(responseString)
                    Dim retList As New List(Of DOMOExistingUser)()
                    retList.Add(retUser)

                    Return DataTableUtils.ConvertToDataTable(retList)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Sub UsersUpdate(auth As DOMOAuthorization, UserId As Integer, Email As String, Role As String, Name As String)
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/users/{0}", UserId))
        req.Method = "PUT"
        AddCommonHeaders(auth, req)

        Dim userToAdd As New DOMONewUser()
        userToAdd.email = Email
        userToAdd.name = Name
        userToAdd.role = Role
        AddObjectToRequestContent(req, userToAdd)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim responseString As String = sr.ReadToEnd()
                    SqlContext.Pipe.Send(responseString)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
        End Try
    End Sub
    Public Shared Function UsersGet(auth As DOMOAuthorization, UserId As Integer) As DataTable
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/users/{0}", UserId))
        req.Method = "GET"
        AddCommonHeaders(auth, req)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim retUser As DOMOExistingUser = JsonConvert.DeserializeObject(Of DOMOExistingUser)(sr.ReadToEnd())
                    Dim retList As New List(Of DOMOExistingUser)()
                    retList.Add(retUser)

                    Return DataTableUtils.ConvertToDataTable(retList)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub UsersDelete(auth As DOMOAuthorization, UserId As Integer)
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/users/{0}", UserId))
        req.Method = "DELETE"
        AddCommonHeaders(auth, req, "application/json", "")

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim responseString As String = sr.ReadToEnd()
                    SqlContext.Pipe.Send(responseString)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
        End Try
    End Sub
End Class
