
Imports Microsoft.SqlServer.Server
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System

Namespace DOMO_CLR
	Public Class SQLOutput
		Public Shared Sub SendDataTableOverPipe(tbl As DataTable)
			' Build our record schema 
			Dim OutputColumns As New List(Of SqlMetaData)(tbl.Columns.Count)
			For Each col As DataColumn In tbl.Columns
				Dim OutputColumn As SqlMetaData = Nothing

				Try
					OutputColumn = New SqlMetaData(col.ColumnName, TypeConverter.ToSqlDbType(col.DataType), col.MaxLength)
				Catch ex As Exception
					OutputColumn = New SqlMetaData(col.ColumnName, TypeConverter.ToSqlDbType(col.DataType))
				End Try

				OutputColumns.Add(OutputColumn)
			Next

			' Build our SqlDataRecord and start the results 
			Dim record As New SqlDataRecord(OutputColumns.ToArray())
			SqlContext.Pipe.SendResultsStart(record)

			' Now send all the rows 
			For Each row As DataRow In tbl.Rows
				For col As Integer = 0 To tbl.Columns.Count - 1
					record.SetValue(col, row.ItemArray(col))
				Next
				SqlContext.Pipe.SendResultsRow(record)
			Next

			' And complete the results 
			SqlContext.Pipe.SendResultsEnd()
		End Sub

        Public Shared Sub InsertDataTableIntoDBTable(DBTable As String, dt As DataTable, Connection As SqlConnection)
            Dim BaseSQL As String = String.Format("Insert Into dbo.{0} values", DBTable)
            Dim SQL As String = ""
            Dim setCounter As Integer = 0

            For RowIndex As Integer = 0 To dt.Rows.Count - 1
                If (setCounter = 0) Then
                    SQL = BaseSQL
                End If

                If (setCounter > 0) Then
                    SQL += ", "
                End If

                Dim row As DataRow = dt.Rows(RowIndex)
                SQL += String.Format("({0})", GetRowInsertSQL(row, dt.Columns.Count))

                setCounter += 1

                If (setCounter >= 999) Then
                    'SqlContext.Pipe.Send(SQL)
                    CLRConnection.ExecuteSQL(SQL, Connection)
                    setCounter = 0
                    SQL = ""
                End If
            Next

            If (Not String.IsNullOrEmpty(SQL)) Then
                'SqlContext.Pipe.Send(SQL)
                CLRConnection.ExecuteSQL(SQL, Connection)
            End If
        End Sub

        Public Shared Sub UpdateDataTableIntoDBTable(DBTable As String, dt As DataTable, PKName As String, PKValue As Integer, Connection As SqlConnection)
            Dim BaseSQL As String = String.Format("update t set ", DBTable)
            Dim SuffixSQL As String = String.Format(" from {0} t where t.{1} = {2}", DBTable, PKName, PKValue)
            Dim SQL As String = ""
            Dim setCounter As Integer = 0

            For RowIndex As Integer = 0 To dt.Rows.Count - 1
                If (setCounter = 0) Then
                    SQL = BaseSQL
                End If

                If (setCounter > 0) Then
                    SQL += ", "
                End If

                Dim row As DataRow = dt.Rows(RowIndex)
                SQL += String.Format("{0}", GetRowUpdateSQL(dt, row, dt.Columns.Count))

                setCounter += 1
            Next

            If (Not String.IsNullOrEmpty(SQL) And dt.Rows.Count > 0) Then
                SQL += SuffixSQL
                'SqlContext.Pipe.Send(SQL)
                CLRConnection.ExecuteSQL(SQL, Connection)
            End If
        End Sub

        Public Shared Function InsertDataTableIntoDBTableWithIdentity(DBTable As String, dt As DataTable, Connection As SqlConnection) As Integer
            Dim BaseSQL As String = String.Format("Insert Into dbo.{0} values", DBTable)
            Dim SQL As String = ""
            Dim setCounter As Integer = 0

            For RowIndex As Integer = 0 To dt.Rows.Count - 1
                If (setCounter = 0) Then
                    SQL = BaseSQL
                End If

                If (setCounter > 0) Then
                    SQL += ", "
                End If

                Dim row As DataRow = dt.Rows(RowIndex)
                SQL += String.Format("({0})", GetRowInsertSQL(row, dt.Columns.Count))

                setCounter += 1
            Next

            If (Not String.IsNullOrEmpty(SQL)) Then
                SQL += "; SELECT SCOPE_IDENTITY();"
                'SqlContext.Pipe.Send(SQL)
                Return Integer.Parse(CLRConnection.ExecuteScalar(SQL, Connection).ToString())
            Else
                Return 0
            End If
        End Function

        Public Shared Sub LogError(Source As String, Message As String, Connection As SqlConnection)
            Dim retTable As New DataTable()

            retTable.Columns.Add("DateCreated", GetType(DateTime))
            retTable.Columns.Add("Source", GetType(String))
            retTable.Columns.Add("Message", GetType(String))

            retTable.Rows.Add(DateTime.Now, Source, Message)

            InsertDataTableIntoDBTable("TODO", retTable, Connection)
        End Sub

        Private Shared LastMessageTime As DateTime

        Public Shared LastMessageID As Integer = 0
        Public Shared Sub WriteMessageOutput(Message As String, Optional ShouldWrite As Boolean = False)
            If (ShouldWrite) Then
                If (LastMessageTime <> DateTime.MinValue) Then
                    Dim TimeGap As Double = DateTime.Now.Subtract(LastMessageTime).TotalMilliseconds

                    'If (TimeGap > 0.01) Then
                    'End If
                    SqlContext.Pipe.Send(String.Format("{0}; Time since last message: {1:N2}ms; MessageID: {2}", Message, TimeGap, LastMessageID))
                Else
                    SqlContext.Pipe.Send(String.Format("{0}; Time since last message: N/A; MessageID: {1}", Message, LastMessageID))
                End If

                LastMessageTime = DateTime.Now
                LastMessageID += 1
            End If
        End Sub

        Public Shared Sub LogError(ErrorString As String)
            SqlContext.Pipe.Send(ErrorString)
        End Sub

        Private Shared Function GetRowInsertSQL(row As DataRow, NumberOfColumns As Integer) As String
			Dim SQL As String = ""

			For ColIndex As Integer = 0 To NumberOfColumns - 1
				If (ColIndex > 0) Then
					SQL += ", "
				End If

				SQL += GetEscapedSQLForObject(row.ItemArray(ColIndex))
			Next

			Return SQL
		End Function


		Private Shared Function GetRowUpdateSQL(table As DataTable, row As DataRow, NumberOfColumns As Integer) As String
			Dim SQL As String = ""

			For ColIndex As Integer = 0 To NumberOfColumns - 1
				If (ColIndex > 0) Then
					SQL += ", "
				End If

				SQL += String.Format("{0} = {1}", table.Columns(ColIndex).ColumnName, GetEscapedSQLForObject(row.ItemArray(ColIndex)))
			Next

			Return SQL
		End Function

		Private Shared Function GetEscapedSQLForObject(obj As Object) As String
            Dim objType As Type = obj.[GetType]()

            If (obj Is Nothing) Then
				Return "NULL"
			ElseIf (Object.ReferenceEquals(objType, GetType(DateTime))) Then
				Return String.Format("'{0}'", obj.ToString())
			ElseIf (Object.ReferenceEquals(objType, GetType(Boolean))) Then
				Dim bObj As Boolean = Convert.ToBoolean(obj)
				If (bObj = True) Then
					Return "1"
				Else
					Return "0"
				End If
			ElseIf (Object.ReferenceEquals(objType, GetType(String))) Then
				Return String.Format("'{0}'", obj.ToString().Replace("'", "''"))
			Else
                Dim ret As String = obj.ToString()

                If (ret.Length = 0) Then
					Return "NULL"
				Else
					Return ret
				End If
			End If

		End Function
	End Class
End Namespace