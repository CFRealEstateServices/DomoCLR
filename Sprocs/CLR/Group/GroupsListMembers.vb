Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub GroupsListMembers(DOMODomain As String, GroupId As Integer, Limit As Integer, Offset As Integer)
        Dim auth As DOMOAuthorization = AuthService.GetUserAccessToken(DOMODomain)
        Dim dt As DataTable = GroupService.GroupsListMembers(auth, GroupId, Limit, Offset)
        SQLOutput.SendDataTableOverPipe(dt)
    End Sub
End Class
