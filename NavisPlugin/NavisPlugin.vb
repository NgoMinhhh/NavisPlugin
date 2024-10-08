Imports System.Windows.Forms
Imports Autodesk.Navisworks.Api.Plugins

Namespace UnisaDockPaneAddin
    <Plugin("UnisaDockPanePlugin", "CAPS", DisplayName:="UnisaDockPanePlugin", ToolTip:="UniSA Docking Pane Plugin")>
    <DockPanePlugin(300, 230, AutoScroll:=False, MinimumHeight:=230, MinimumWidth:=300)>
    Public Class UnisaDockPanePlugin
        Inherits DockPanePlugin

        Public Overrides Function CreateControlPane() As Control
            Dim unisaControl As New UnisaControl()
            unisaControl.CreateControl()
            Return unisaControl
        End Function

        Public Overrides Sub DestroyControlPane(pane As Control)
            pane.Dispose()
        End Sub

    End Class

    <Plugin("UnisaDockPaneAddin", "CAPS",
        DisplayName:="UNISA LoD Plugin",
        ToolTip:="UNISA LoD Checklist")>
    Public Class BasicDockPaneAddin
        Inherits AddInPlugin
        Public Overrides Function Execute(ParamArray parameters() As String) As Integer
            If Autodesk.Navisworks.Api.Application.IsAutomated Then
                Throw New InvalidOperationException("Invalid when running using Automation")
            End If

            'Find the plugin
            Dim pr As PluginRecord = Autodesk.Navisworks.Api.Application.Plugins.FindPlugin("UnisaDockPanePlugin.CAPS")

            If pr IsNot Nothing AndAlso TypeOf pr Is DockPanePluginRecord AndAlso pr.IsEnabled Then
                'check if it needs loading
                If pr.LoadedPlugin Is Nothing Then
                    pr.LoadPlugin()
                End If

                Dim dpp As DockPanePlugin = TryCast(pr.LoadedPlugin, DockPanePlugin)
                dpp.ActivatePane()
                '    Dim dpp As DockPanePlugin = TryCast(pr.LoadedPlugin, DockPanePlugin)
                '    If dpp IsNot Nothing Then
                '        'switch the Visible flag
                '        dpp.Visible = Not dpp.Visible
                '    End If
            End If

            Return 0
        End Function
    End Class
End Namespace
