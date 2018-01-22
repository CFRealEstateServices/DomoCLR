Imports System
Imports System.Data
Imports System.IO
Imports System.Net
Imports DOMO_CLR.DOMO_CLR
Imports Microsoft.VisualBasic.FileIO
Imports Newtonsoft.Json

Public Class DataSetService
    Inherits DOMOServiceBase

    Public Shared Function GetDatasetMetadata(auth As DOMOAuthorization, datasetID As String) As String
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/datasets/{0}", datasetID))
        req.Method = "GET"
        AddCommonHeaders(auth, req)

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    Return sr.ReadToEnd()
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
            Return Nothing
        End Try
    End Function

    Public Shared Sub DownloadDataSet(auth As DOMOAuthorization, datasetID As String, dt As DataTable)
        Dim req As WebRequest = WebRequest.Create(String.Format("https://api.domo.com/v1/datasets/{0}/data?includeHeader=false&fileName=dump.csv", datasetID))
        req.Method = "GET"
        AddCommonHeaders(auth, req, "text/csv")

        Try
            Using response As WebResponse = req.GetResponse()
                Using sr As StreamReader = New StreamReader(response.GetResponseStream())
                    PopulateDataTableFromCSV(sr, dt)
                End Using
            End Using
        Catch ex As WebException
            HandleWebException(ex)
        End Try
    End Sub

    Private Shared Sub PopulateDataTableFromCSV(sr As StreamReader, dt As DataTable)
        Dim CommaChar As Char = Char.Parse(",")
        Dim RowNum As Integer = 0

        Using tfp As TextFieldParser = New Microsoft.VisualBasic.FileIO.TextFieldParser(sr)
            tfp.Delimiters = New String() {","}
            tfp.TextFieldType = FieldType.Delimited

            While Not tfp.EndOfData
                Dim Data() As String = tfp.ReadFields

                Try
                    Dim dr As DataRow = dt.NewRow()

                    For i As Integer = 0 To dt.Columns.Count - 1
                        If (Data(i).Equals("\N") Or Data(i).Equals("")) Then
                            dr.Item(i) = DBNull.Value
                        Else
                            dr.Item(i) = Data(i)
                        End If
                    Next

                    dt.Rows.Add(dr)
                Catch ex As Exception
                    SQLOutput.LogError(String.Format("Current Row Number: {0}", RowNum))
                    SQLOutput.LogError(String.Format("Current Row Data: {0}", JsonConvert.SerializeObject(Data)))
                    Throw ex
                End Try

                RowNum += 1
            End While
        End Using
    End Sub
End Class
