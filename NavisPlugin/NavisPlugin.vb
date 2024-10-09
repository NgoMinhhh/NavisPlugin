Imports System.Windows.Forms
Imports Autodesk.Navisworks.Api
Imports Autodesk.Navisworks.Api.Plugins
Imports Application = Autodesk.Navisworks.Api.Application

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

	<Plugin("TestPlugin", "MINH",
		DisplayName:="TestPlugin")>
	Public Class TestPlugin
		Inherits AddInPlugin
		Public Overrides Function Execute(ParamArray parameters() As String) As Integer
			If Autodesk.Navisworks.Api.Application.IsAutomated Then
				Throw New InvalidOperationException("Invalid when running using Automation")
			End If
			Dim activeDoc As Document = Application.ActiveDocument
			Try
				Dim selectionSet1 As New SelectionSet() With {
					.DisplayName = "TestChildren1"}
				Dim selectionSet2 As New SelectionSet() With {
					.DisplayName = "TestChildren2",
					.Guid = Guid.NewGuid()}
				Dim folderItem As New FolderItem() With {
					.DisplayName = "Test"
				}
				activeDoc.SelectionSets.AddCopy(folderItem)

				For Each item In activeDoc.SelectionSets.Value.ToList()
					If item.DisplayName = "Test" Then
						activeDoc.SelectionSets.AddCopy(item, selectionSet2)
						activeDoc.SelectionSets.AddCopy(item, selectionSet1)
					End If
				Next
			Catch ex As Exception
				Debug.Print(ex.Message)
			End Try

			Return 0
		End Function
	End Class
End Namespace
