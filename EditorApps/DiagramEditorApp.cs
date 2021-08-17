using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Views.CustomControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.EditorApps {
	public abstract class DiagramEditorApp : EditorApp {
		public override void InitializeView() {
			var editorDiagram = new EditorDiagramControl(this);
			editorDiagram.Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new Button(), null));
			editorDiagram.Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new Button(), null));
			editorDiagram.Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new Button(), null));
			View = editorDiagram;
		}
	}
}
