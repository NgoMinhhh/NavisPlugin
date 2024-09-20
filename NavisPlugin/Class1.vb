Imports Autodesk.Navisworks.Api
Imports Autodesk.Navisworks.Api.Plugins
Imports Autodesk.Navisworks.Api.Application
Imports Autodesk.Navisworks.Api.DocumentParts
Imports System.Text

Namespace BasicPlugIn
    <Plugin("NavisPlugin",                         ' Plugin name
            "ADSK",                                             ' 4 character Developer ID or GUID
            ToolTip:="BasicPlugIn.ABasicPlugin tool tip",       ' The tooltip for the item in the ribbon
            DisplayName:="Extract Properties Plugin")>                 ' Display name for the Plugin in the Ribbon
    Public Class ABasicPlugin
        Inherits AddInPlugin                                    ' Derives from AddInPlugin
        ' Shared method equivalent to static in C#
        Public Function IteratePropertyCategoryDataProperties()
            ' Create a StringBuilder to store the category names
            Dim output As New StringBuilder()
            ' Iterate through the ModelItems in the current selection
            For Each item As ModelItem In Application.ActiveDocument.CurrentSelection.SelectedItems
                    ' Iterate through the item's PropertyCategory entries
                    For Each category As PropertyCategory In item.PropertyCategories
                        ' Iterate through the DataProperties
                        For Each dataProperty As DataProperty In category.Properties
                            ' Add content for display
                            output.AppendFormat(
                                "PropertyCategory: ({0} / {1})   DataProperty: CombinedName='{2}', Value='{3}'{4}",
                                category.DisplayName,
                                category.Name,
                                dataProperty.CombinedName,
                                If(dataProperty.Value Is Nothing, "", dataProperty.Value.ToString()),
                                Environment.NewLine) ' Adds a new line after each entry
                        Next
                    Next
                Next

            Return output
        End Function


        Public Overrides Function Execute(ParamArray parameters() As String) As Integer
            Debug.Print(IteratePropertyCategoryDataProperties)
            Return 0
        End Function
    End Class
End Namespace