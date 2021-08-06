using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Strategies.LoadStrategy;
using HyprWinUI3.ViewModels;
using HyprWinUI3.ViewModels.Editor;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Strategies.LoadStrategy {
	class ClassDiagramLoader : DiagramLoader {
		public override void LoadToEditor(VariableSizedWrapGrid grid) {
			base.LoadToEditor(grid);
			grid.Children.Add(InteractButtonFactory.MakeInteractToggleButton(null));
			grid.Children.Add(InteractButtonFactory.MakeInteractToggleButton(null));
			grid.Children.Add(InteractButtonFactory.MakeInteractToggleButton(null));
		}
	}
}
