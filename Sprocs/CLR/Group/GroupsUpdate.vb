Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub GroupsUpdate(DOMODomain As String, groupId As Integer, active As Boolean, isDefault As Boolean, Name As String)
        Dim auth As DOMOAuthorization = AuthService.GetUserAccessToken(DOMODomain)
        GroupService.GroupsUpdate(auth, groupId, active, isDefault, Name)
    End Sub
End Class
