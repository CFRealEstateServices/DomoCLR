

Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Text
Imports System

Namespace DOMO_CLR
	''' <summary>
	''' Convert a base data type to another base data type
	''' </summary>
	''' <remarks>
	''' Based on code from http://dotnetpulse.blogspot.com/2006/04/convert-net-type-to-sqldbtype-or.html
	''' </remarks>
	Public NotInheritable Class TypeConverter

		Private Structure DbTypeMapEntry
			Public Type As Type
			Public DbType As DbType

			Public SqlDbType As SqlDbType
			Public Sub New(type As Type, dbType As DbType, sqlDbType As SqlDbType)
				Me.Type = type
				Me.DbType = dbType
				Me.SqlDbType = sqlDbType
			End Sub

		End Structure


		Private Shared ReadOnly _DbTypeList As New List(Of DbTypeMapEntry)()
		#Region "Constructors"

		Shared Sub New()
			Dim dbTypeMapEntry As New DbTypeMapEntry(GetType(Boolean), DbType.[Boolean], SqlDbType.Bit)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Byte), DbType.[Double], SqlDbType.TinyInt)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Byte()), DbType.Binary, SqlDbType.Image)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(DateTime), DbType.DateTime, SqlDbType.DateTime)
            _DbTypeList.Add(dbTypeMapEntry)

            dbTypeMapEntry = New DbTypeMapEntry(GetType(Date), DbType.Date, SqlDbType.Date)
            _DbTypeList.Add(dbTypeMapEntry)

            dbTypeMapEntry = New DbTypeMapEntry(GetType([Decimal]), DbType.[Decimal], SqlDbType.[Decimal])
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Double), DbType.[Double], SqlDbType.Float)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Guid), DbType.Guid, SqlDbType.UniqueIdentifier)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Int16), DbType.Int16, SqlDbType.SmallInt)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Int32), DbType.Int32, SqlDbType.Int)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Int64), DbType.Int64, SqlDbType.BigInt)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(Object), DbType.[Object], SqlDbType.[Variant])
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(String), DbType.[String], SqlDbType.VarChar)
			_DbTypeList.Add(dbTypeMapEntry)

			dbTypeMapEntry = New DbTypeMapEntry(GetType(String), DbType.[String], SqlDbType.NVarChar)
            _DbTypeList.Add(dbTypeMapEntry)

            dbTypeMapEntry = New DbTypeMapEntry(GetType(Integer()), DbType.[String], SqlDbType.VarChar)
            _DbTypeList.Add(dbTypeMapEntry)
        End Sub

		Private Sub New()
		End Sub

		#End Region

		#Region "Methods"

		''' <summary>
		''' Convert db type to .Net data type
		''' </summary>
		''' <param name="dbType"></param>
		''' <returns></returns>
		Public Shared Function ToNetType(dbType As DbType) As Type
			Dim entry As DbTypeMapEntry = Find(dbType)
			Return entry.Type
		End Function

		''' <summary>
		''' Convert TSQL type to .Net data type
		''' </summary>
		''' <param name="sqlDbType"></param>
		''' <returns></returns>
		Public Shared Function ToNetType(sqlDbType As SqlDbType) As Type
			Dim entry As DbTypeMapEntry = Find(sqlDbType)
			Return entry.Type
		End Function

		''' <summary>
		''' Convert .Net type to Db type
		''' </summary>
		''' <param name="type"></param>
		''' <returns></returns>
		Public Shared Function ToDbType(type As Type) As DbType
			Dim entry As DbTypeMapEntry = Find(type)
			Return entry.DbType
		End Function

		''' <summary>
		''' Convert TSQL data type to DbType
		''' </summary>
		''' <param name="sqlDbType"></param>
		''' <returns></returns>
		Public Shared Function ToDbType(sqlDbType As SqlDbType) As DbType
			Dim entry As DbTypeMapEntry = Find(sqlDbType)
			Return entry.DbType
		End Function

		''' <summary>
		''' Convert .Net type to TSQL data type
		''' </summary>
		''' <param name="type"></param>
		''' <returns></returns>
		Public Shared Function ToSqlDbType(type As Type) As SqlDbType
			Dim entry As DbTypeMapEntry = Find(type)
			Return entry.SqlDbType
		End Function

		''' <summary>
		''' Convert DbType type to TSQL data type
		''' </summary>
		''' <param name="dbType"></param>
		''' <returns></returns>
		Public Shared Function ToSqlDbType(dbType As DbType) As SqlDbType
			Dim entry As DbTypeMapEntry = Find(dbType)
			Return entry.SqlDbType
		End Function

		Private Shared Function Find(type As Type) As DbTypeMapEntry
			For Each entry As DbTypeMapEntry In _DbTypeList
				If Object.ReferenceEquals(entry.Type, type) Then
					Return entry
				End If
			Next

			Throw New ApplicationException("Referenced an unsupported Type")
		End Function

		Private Shared Function Find(dbType As DbType) As DbTypeMapEntry
			For Each entry As DbTypeMapEntry In _DbTypeList
				If entry.DbType = dbType Then
					Return entry
				End If
			Next

			Throw New ApplicationException("Referenced an unsupported DbType")
		End Function

		Private Shared Function Find(sqlDbType As SqlDbType) As DbTypeMapEntry
			For Each entry As DbTypeMapEntry In _DbTypeList
				If entry.SqlDbType = sqlDbType Then
					Return entry
				End If
			Next

			Throw New ApplicationException("Referenced an unsupported SqlDbType")
		End Function

		#End Region
	End Class

End Namespace