Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub UsersUpdate(DOMODomain As String, UserId As Integer, Email As String, Role As String, Name As String)
        Dim auth As DOMOAuthorization = AuthService.GetUserAccessToken(DOMODomain)
        UserService.UsersUpdate(auth, UserId, Email, Role, Name)
    End Sub
End Class
