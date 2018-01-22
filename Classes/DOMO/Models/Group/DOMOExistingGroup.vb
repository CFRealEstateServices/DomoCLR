Imports Newtonsoft.Json

Public Class DOMOExistingGroup
    Public Property creatorId As Integer
    Public Property id As Integer
    Public Property active As Boolean
    <JsonProperty(PropertyName:="default")>
    Public Property isDefault As Boolean
    Public Property memberCount As Integer
    Public Property name As String
End Class
