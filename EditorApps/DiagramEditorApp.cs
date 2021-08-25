using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Commands.EditorInteractions;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Views.CustomControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.EditorApps {
	public abstract class DiagramEditorApp : EditorApp {
		public override void RefreshView() {
			View = new EditorDiagramControl(this);

			var font = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons");

			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new Button(), null));

			// Add note button
			var button = new Button();
			ToolTipService.SetToolTip(button, "Add Note");
			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(
				InteractButtonFactory.MakeInteractButton(
				  new FontIcon() { Glyph = "\xE109" },
				  button,
				  new AddNoteCommand(this).ExecuteCommand)
				);

			((EditorDiagramControl)View).Toolbar.Grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new Button(), null));
		}
	}
}
