Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Xml.Linq
Imports DOMO_CLR.DOMO_CLR
Imports Newtonsoft.Json

Public Class DataTableUtils
    Public Shared Function ParseJSONToDataTable(inJSON As String) As DataTable
        Dim dt As DataTable = New DataTable()

        Dim metadata As DOMODatasetMetadata = JsonConvert.DeserializeObject(Of DOMODatasetMetadata)(inJSON)

        For Each column As DOMOColumn In metadata.schema.columns
            dt.Columns.Add(CreateDataColumnFromDefinition(column))
        Next

        Return dt
    End Function
    Public Shared Function ConvertToDataTable(Of t)(
                                                  ByVal list As IList(Of t)
                                               ) As DataTable
        Dim table As New DataTable()
        If Not list.Any Then
            'don't know schema ....
            Return table
        End If
        Dim fields() As Reflection.PropertyInfo = list.First.GetType.GetProperties
        For Each field As Reflection.PropertyInfo In fields
            table.Columns.Add(field.Name, field.PropertyType)
        Next
        For Each item As t In list
            Dim row As DataRow = table.NewRow()
            For Each field As Reflection.PropertyInfo In fields
                Dim p As Reflection.PropertyInfo = item.GetType.GetProperty(field.Name)
                row(field.Name) = p.GetValue(item, Nothing)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function

    Private Shared Function CreateDataColumnFromDefinition(columnDefinition As DOMOColumn) As DataColumn
        Dim dc As New DataColumn()

        Dim Name As String = columnDefinition.name
        Dim definitionTypeAsString As String = columnDefinition.type
        Dim definitionTypeAsType As Type = Nothing

        Select Case definitionTypeAsString
            Case "STRING"
                definitionTypeAsType = GetType(String)
            Case "DOUBLE"
                definitionTypeAsType = GetType(Double)
            Case "DATETIME"
                definitionTypeAsType = GetType(DateTime)
            Case "DATE"
                definitionTypeAsType = GetType(Date)
            Case "BOOLEAN"
                definitionTypeAsType = GetType(Boolean)
            Case "INTEGER"
                definitionTypeAsType = GetType(Integer)
            Case "LONG"
                definitionTypeAsType = GetType(Long)
            Case "DECIMAL"
                definitionTypeAsType = GetType(Decimal)
            Case Else
                SQLOutput.LogError("definitionTypeAsString is: " & definitionTypeAsString)
        End Select

        dc.ColumnName = Name
        dc.DataType = definitionTypeAsType
        dc.AllowDBNull = True

        Return dc
    End Function
End Class
