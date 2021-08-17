using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace HyprWinUI3.EditorApps {
	public class ClassDiagramEditorApp : DiagramEditorApp {
		public ClassDiagramEditorApp() {
			Model = new ClassDiagram();
		}
		public override void InitializeView() {
			// Initialize EditorDiagramControl
			base.InitializeView();

			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
		}

		public override bool LoadData(string data) {
			try {
				Model = JsonSerializer.Deserialize<ClassDiagram>(data);
				return true;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
			return false;
		}

		public override bool SaveData(StorageFolder folder) {
			throw new NotImplementedException();
		}
	}
}
