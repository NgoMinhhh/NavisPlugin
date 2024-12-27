' ====================================================================================
' File Name     : NavusPlugin.vb
' Description   : This file is the main entry point
'                 + UnisaDockPanePlugin: Initialise the docking pane 
'                 + BasicDockPaneAddin: the button on the Tool Add-in to activate the
'					the custom dock pane 
' Authors       : Nhat Minh Ngo, ngonhatminh9898@gmail.com
'				: Yongzen Guan, guanyongzhen@gmail.com
'				: Grahi Brahmbhatt, grahibrahmbhatt0202@gmail.com
'		        : Hemel Mahfuzul Islam, hkau0127@gmail.com
' Date Created  : October 2024
' =====================================================================================
'12345
Imports System.Windows.Forms
Imports Autodesk.Navisworks.Api.Plugins

Namespace UnisaDockPaneAddin
	<Plugin("UnisaDockPanePlugin", "MINH", DisplayName:="Unisa Plugin", ToolTip:="UniSA Docking Pane Plugin For LoD Verification")>
	<DockPanePlugin(250, 230, AutoScroll:=False, MinimumHeight:=230, MinimumWidth:=250)>
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

	<Plugin("UnisaDockPaneAddin", "MINH",
		DisplayName:="UniSA LoD Plugin",
		ToolTip:="UniSA Docking Pane Plugin For LoD Verification")>
	Public Class BasicDockPaneAddin
		Inherits AddInPlugin
		Public Overrides Function Execute(ParamArray parameters() As String) As Integer
			If Autodesk.Navisworks.Api.Application.IsAutomated Then
				Throw New InvalidOperationException("Invalid when running using Automation")
			End If

			'Find the plugin
			Dim pr As PluginRecord = Autodesk.Navisworks.Api.Application.Plugins.FindPlugin("UnisaDockPanePlugin.MINH")

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
