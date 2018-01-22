Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Net
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class GroupService
    Inherits DOMOServiceBase

    Public Shared Function GroupsCreate(auth As DOMOAuthorization, isDefault As Boolean, Name As String) As DataTable
        Dim req As WebRequest = WebRequest.Create("https://api.domo.com/v1/groups")
        req.Method = "POST"
        AddCommonHeaders(auth, req)

        Dim groupToAdd As New DOMONewGroup()
        groupToAdd.isDefault = isDefault
        groupToAdd.name = Name
        AddObjectToRequestContent(req, groupToAdd)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim responseString As String = sr.ReadToEnd()

                    Dim retGroup As DOMOExistingGroup = JsonConvert.DeserializeObject(Of DOMOExistingGroup)(responseString)
                    Dim retList As New List(Of DOMOExistingGroup)()
                    retList.Add(retGroup)

                    Return DataTableUtils.ConvertToDataTable(retList)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function GroupsGet(auth As DOMOAuthorization, GroupId As Integer) As DataTable
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/groups/{0}", GroupId))
        req.Method = "GET"
        AddCommonHeaders(auth, req)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim retGroup As DOMOExistingGroup = JsonConvert.DeserializeObject(Of DOMOExistingGroup)(sr.ReadToEnd())
                    Dim retList As New List(Of DOMOExistingGroup)()
                    retList.Add(retGroup)

                    Return DataTableUtils.ConvertToDataTable(retList)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function
    Public Shared Function GroupsList(auth As DOMOAuthorization, Limit As Integer, Offset As Integer) As DataTable
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/groups?sort=name&fields=all&limit={0}&offset={1}", Limit, Offset))
        req.Method = "GET"
        AddCommonHeaders(auth, req)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim users As List(Of DOMOExistingGroup) = JsonConvert.DeserializeObject(Of List(Of DOMOExistingGroup))(sr.ReadToEnd())

                    Return DataTableUtils.ConvertToDataTable(users)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function
    Public Shared Function GroupsListMembers(auth As DOMOAuthorization, GroupId As Integer, Limit As Integer, Offset As Integer) As DataTable
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/groups/{0}/users", GroupId))
        req.Method = "GET"
        AddCommonHeaders(auth, req)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim responseAsString As String = sr.ReadToEnd()

                    Dim userIDs As IEnumerable(Of Integer) = JArray.Parse(responseAsString).Values(Of Integer)()
                    Dim dt As New DataTable()

                    dt.Columns.Add("GroupId", GetType(Integer))
                    dt.Columns.Add("UserId", GetType(Integer))

                    For Each userId As Integer In userIDs
                        Dim row As DataRow = dt.NewRow()

                        row.Item(0) = GroupId
                        row.Item(1) = userId

                        dt.Rows.Add(row)
                    Next

                    Return dt
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Sub GroupsUpdate(auth As DOMOAuthorization, GroupId As Integer, active As Boolean, isDefault As Boolean, Name As String)
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/groups/{0}", GroupId))
        req.Method = "PUT"
        AddCommonHeaders(auth, req)

        Dim groupToAdd As New DOMOUpdateGroup()
        groupToAdd.isDefault = isDefault
        groupToAdd.name = Name
        groupToAdd.active = active
        AddObjectToRequestContent(req, groupToAdd)
        'SqlContext.Pipe.Send(JsonConvert.SerializeObject(groupToAdd))

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
    Public Shared Sub GroupsDelete(auth As DOMOAuthorization, GroupId As Integer)
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/groups/{0}", GroupId))
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
    Public Shared Sub GroupsAddMember(auth As DOMOAuthorization, GroupId As Integer, UserId As Integer)
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/groups/{0}/users/{1}", GroupId, UserId))
        req.Method = "PUT"
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
    Public Shared Sub GroupsRemoveMember(auth As DOMOAuthorization, GroupId As Integer, UserId As Integer)
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/groups/{0}/users/{1}", GroupId, UserId))
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
