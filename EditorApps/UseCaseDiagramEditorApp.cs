using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Windows.UI.Xaml.Controls.Primitives;

namespace HyprWinUI3.EditorApps {
	class UseCaseDiagramEditorApp : DiagramEditorApp {
		public UseCaseDiagramEditorApp() {
			Model = new UseCaseDiagram();
		}
		public override bool LoadData(string data) {
			try {
				Model = JsonSerializer.Deserialize<UseCaseDiagram>(data);
				return true;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
				return false;
			}
		}
		public override void RefreshView() {
			base.RefreshView();
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new RepeatButton(), null));
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new RepeatButton(), null));
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new RepeatButton(), null));
		}
	}
}
