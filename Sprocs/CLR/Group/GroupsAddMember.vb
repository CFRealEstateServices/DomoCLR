Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub GroupsAddMember(DOMODomain As String, groupId As Integer, userId As Integer)
        Dim auth As DOMOAuthorization = AuthService.GetUserAccessToken(DOMODomain)
        GroupService.GroupsAddMember(auth, groupId, userId)
    End Sub
End Class
