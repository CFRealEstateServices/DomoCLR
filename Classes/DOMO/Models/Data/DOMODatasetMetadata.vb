Public Class DOMODatasetMetadata
    Public Property id As String
    Public Property name As String
    Public Property rows As Integer
    Public Property columns As Integer
    Public Property schema As DOMOColumns
End Class

Public Class DOMOColumns
    Public Property columns As DOMOColumn()
End Class

Public Class DOMOColumn
    Public Property type As String
    Public Property name As String
End Class
