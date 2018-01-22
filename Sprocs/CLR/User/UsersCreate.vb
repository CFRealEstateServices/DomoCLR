Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.SqlServer.Server


Partial Public Class StoredProcedures
    <Microsoft.SqlServer.Server.SqlProcedure()>
    Public Shared Sub UsersCreate(DOMODomain As String, Email As String, Role As String, Name As String, SendInvite As Boolean)
        Dim auth As DOMOAuthorization = AuthService.GetUserAccessToken(DOMODomain)
        Dim dt As DataTable = UserService.UsersCreate(auth, Email, Role, Name, SendInvite)
        SQLOutput.SendDataTableOverPipe(dt)
    End Sub
End Class
