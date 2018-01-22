Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub GroupsDelete(DOMODomain As String, GroupId As Integer)
        Dim auth As DOMOAuthorization = AuthService.GetUserAccessToken(DOMODomain)
        GroupService.GroupsDelete(auth, GroupId)
    End Sub
End Class
