Imports Autodesk.Navisworks.Api
Imports Autodesk.Navisworks.Api.Plugins
Imports Autodesk.Navisworks.Api.Application
Imports Autodesk.Navisworks.Api.DocumentParts
Imports System.Text
Imports System.IO

Namespace BasicPlugIn
    <Plugin("NavisPlugin",                         ' Plugin name
            "ADSK",                                             ' 4 character Developer ID or GUID
            ToolTip:="BasicPlugIn.ABasicPlugin tool tip",       ' The tooltip for the item in the ribbon
            DisplayName:="Extract Properties Plugin")>                 ' Display name for the Plugin in the Ribbon
    Public Class ABasicPlugin
        Inherits AddInPlugin                                    ' Derives from AddInPlugin
        ' Shared method equivalent to static in C#

        Public Function ExtractDataProperties() As List(Of String)
            ' Create a StringBuilder to store the category names
            Dim output As New List(Of String)()

            ' Write Header name
            output.Add("ElementName,CategoryDisplayName,CategoryName,PropertyDisplayName,PropertyName,Value")

            ' Iterate through the ModelItems in the current selection
            For Each item As ModelItem In Application.ActiveDocument.CurrentSelection.SelectedItems
                ' Iterate through the item's PropertyCategory entries
                For Each category As PropertyCategory In item.PropertyCategories
                    ' Iterate through the DataProperties
                    For Each dataProperty As DataProperty In category.Properties
                        ' Add content for display
                        Dim line As String = String.Format("""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}""",' Escape quotes
                                                           item.DisplayName, 'Element's name
                                                           category.DisplayName.Replace("""", """"""),
                                                           category.Name.Replace("""", """"""),
                                                           dataProperty.DisplayName.Replace("""", """"""),
                                                           dataProperty.Name.Replace("""", """"""),
                                                           If(dataProperty.Value Is Nothing, "", dataProperty.Value.ToString().Replace("""", """""")))
                        output.Add(line)
                    Next
                Next
            Next

            Return output
        End Function



        Public Overrides Function Execute(ParamArray parameters() As String) As Integer
            'Extract properties from selected elements
            Dim lines As List(Of String) = ExtractDataProperties()

            ' Define the output CSV file path
            Dim csvFilePath As String = "C:\Users\ngonh\Projects\NavisPlugin\sample.csv"

            ' Write all lines to the CSV file at once
            File.WriteAllLines(csvFilePath, lines, Encoding.UTF8)

            Return 0
        End Function
    End Class
End Namespace