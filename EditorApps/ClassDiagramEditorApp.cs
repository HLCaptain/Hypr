using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Strategies.LoadStrategy;
using HyprWinUI3.Views.CustomControls;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Xaml.Controls.Primitives;

namespace HyprWinUI3.EditorApps {
	public class ClassDiagramEditorApp : DiagramEditorApp {
		public override async void RefreshView() {
			// Initialize EditorDiagramControl
			base.RefreshView();
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));

			// load data onto view
			if (Model == null) {
				return;
			}
			var loadStrategy = new ClassDiagramLoadStrategy();
			foreach (var item in ((ClassDiagram)Model).Vertices) {
				await loadStrategy.LoadToCanvas(
					((EditorDiagramControl)View).Canvas.ForegroundCanvas,
					item
					);
			}

			foreach (var item in ((ClassDiagram)Model).Edges) {
				await loadStrategy.LoadToCanvas(
					((EditorDiagramControl)View).Canvas.ForegroundCanvas,
					(Edge)await FilesystemService.LoadActor(item)
					);
			}
		}
	}
}
