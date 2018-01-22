
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Data.SqlClient
Imports System

Namespace DOMO_CLR
	Public Class CLRConnection
		Private Const CONNECTION_STRING As String = "context connection=true;"
		Private Const REMOTE_CONNECTION_STRING_BASE As String = "Data Source={0};Initial Catalog={1};Integrated Security=True;Enlist=false;Max Pool Size=1000;Min Pool Size=50;"

		Private Const COMMAND_TIMEOUT As Integer = 3000
		Public Shared Function GetConnection() As SqlConnection
			Dim Connection As SqlConnection = Nothing
			If (Connection Is Nothing) Then
				Connection = New SqlConnection()
			End If

			If (String.IsNullOrEmpty(Connection.ConnectionString)) Then
				Connection.ConnectionString = CONNECTION_STRING
			End If

			If (Connection.State = ConnectionState.Closed) Then
				Connection.Open()
			End If

			Return Connection
		End Function

		Public Shared Function GetRemoteConnection(ServerName As String, DBName As String) As SqlConnection
			Dim Connection As SqlConnection = Nothing
			If (Connection Is Nothing) Then
				Connection = New SqlConnection()
			End If

			If (String.IsNullOrEmpty(Connection.ConnectionString)) Then
				Connection.ConnectionString = String.Format(REMOTE_CONNECTION_STRING_BASE, ServerName, DBName)
			End If

			If (Connection.State = ConnectionState.Closed) Then
				Connection.Open()
			End If

			Return Connection
		End Function

		Public Shared Function GetNewConnection() As SqlConnection
			'Dim conn = New SqlConnection()
			'conn.ConnectionString = CONNECTION_STRING
			'conn.Open()

			'Return conn
			Return GetConnection()
		End Function

		Public Shared Function GetDBInfo(conn As SqlConnection) As DBInfo
            Dim Sql As String = "SELECT SERVERPROPERTY('MachineName') as ServerName, DB_NAME() as DBName"
            Dim Command As SqlCommand = New SqlCommand(Sql, conn)
            Dim LineTable As New DataTable()
			LineTable.Load(Command.ExecuteReader())
			Return New DBInfo(LineTable.Rows(0)("ServerName").ToString(), LineTable.Rows(0)("DBName").ToString())
		End Function

		Public Class DBInfo
			Public ServerName As String

			Public DBName As String
			Public Sub New(ServerName As String, DBName As String)
				Me.ServerName = ServerName
				Me.DBName = DBName
			End Sub
		End Class

		Public Shared Sub ExecuteSQL(SQL As String, Connection As SqlConnection)
            Dim comm As SqlCommand = New SqlCommand(SQL, Connection)
            comm.CommandType = CommandType.Text
			comm.CommandTimeout = COMMAND_TIMEOUT
			comm.CommandText = SQL
			comm.ExecuteNonQuery()
			comm.Dispose()
		End Sub

		Public Shared Function ExecuteScalar(SQL As String, Connection As SqlConnection) As Object
            Dim comm As SqlCommand = New SqlCommand(SQL, Connection)
            comm.CommandTimeout = COMMAND_TIMEOUT
			comm.CommandType = CommandType.Text

            Dim ret As Object = Nothing
            Try
				ret = comm.ExecuteScalar()
			Catch ex As Exception
				ExecuteScalar(SQL, Connection)
			End Try
			ret = comm.ExecuteScalar()
			comm.Dispose()

			Return ret
		End Function

		Public Shared Function ExecuteScalarWithConnection(SQL As String, PassedConnection As SqlConnection) As Object
			Return ExecuteScalar(SQL, PassedConnection)
		End Function

		Public Shared Function ExecuteReader(SQL As String, Connection As SqlConnection) As DataTable
            Dim comm As SqlCommand = New SqlCommand(SQL, Connection)
            comm.CommandType = CommandType.Text
			comm.CommandTimeout = COMMAND_TIMEOUT

            Dim DT As DataTable = New DataTable()
            DT.Load(comm.ExecuteReader())
			comm.Dispose()

			Return DT
		End Function
	End Class
End Namespace