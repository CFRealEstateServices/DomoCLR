
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server
Imports DOMO_CLR
Imports System
Imports DOMO_CLR.DOMO_CLR

Partial Public Class StoredProcedures
    Shared IsDebugMode As Boolean = False

    <Microsoft.SqlServer.Server.SqlProcedure>
    Public Shared Sub DownloadFromDOMO(DOMODomain As String, DataSetID As String)
        Dim auth As DOMOAuthorization = AuthService.GetDataAccessToken(DOMODomain)
        Dim tableMetadata As String = DataSetService.GetDatasetMetadata(auth, DataSetID)
        Dim dt As DataTable = DataTableUtils.ParseJSONToDataTable(tableMetadata)

        If (IsDebugMode) Then
            SqlContext.Pipe.Send("Number of dt columns: " & dt.Columns.Count)
            For Each col As DataColumn In dt.Columns
                SqlContext.Pipe.Send("DT Column: " & col.ColumnName)
            Next
            SqlContext.Pipe.Send(tableMetadata)
        End If

        DataSetService.DownloadDataSet(auth, DataSetID, dt)

        SQLOutput.SendDataTableOverPipe(dt)
    End Sub
End Class