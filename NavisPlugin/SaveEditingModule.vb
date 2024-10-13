Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.FileIO

Module SaveEditingModule
    Public Sub WriteUpdatedLoDtoCSV(ingestedElements As List(Of IngestedElement), oldCsvFilepath As String, guidIndex As Integer, lodIndex As Integer)

        ' Write the content to memory first then overwrite the file
        Dim content As New List(Of String())()
        Using reader As New TextFieldParser(oldCsvFilepath)
            reader.TextFieldType = FieldType.Delimited
            reader.Delimiters = New String() {","}
            reader.HasFieldsEnclosedInQuotes = True
            Dim line As String()

            ' Read each line until the end of the file
            While Not reader.EndOfData
                line = reader.ReadFields()
                For Each element In ingestedElements
                    If line(guidIndex) = element.GUID Then
                        line(lodIndex) = element.LOD
                        Exit For
                    End If
                Next
                content.Add(line)
            End While
        End Using

        Using writer As New StreamWriter(oldCsvFilepath, False, Encoding.UTF8)
            For Each row In content
                writer.WriteLine(CreateCsvLine(row))
            Next

        End Using
    End Sub

    Private Function CreateCsvLine(row As String()) As String
        Dim csvBuilder As New StringBuilder()

        For i As Integer = 0 To row.Length - 1
            Dim value As String = row(i)
            Dim escapedValue As String = value

            ' Escape double quotes by replacing each " with ""
            If value.Contains("""") Then
                escapedValue = value.Replace("""", """""")
            End If

            ' Wrap value in double quotes if it contains a comma or double quotes
            If value.Contains(",") OrElse value.Contains("""") Then
                escapedValue = $"""{escapedValue}"""
            End If

            ' Append the value to the StringBuilder
            csvBuilder.Append(escapedValue)

            ' Add a comma after the value if it is not the last element
            If i < row.Length - 1 Then
                csvBuilder.Append(",")
            End If
        Next

        Return csvBuilder.ToString()
    End Function
End Module
