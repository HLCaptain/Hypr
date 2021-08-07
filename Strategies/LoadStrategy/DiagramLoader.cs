using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace HyprWinUI3.Strategies.LoadStrategy {
	class DiagramLoader : ILoadStrategy {
		public virtual void LoadToEditor(VariableSizedWrapGrid grid) {
			// todo add real interact buttons
			grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
			grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
			grid.Children.Add(InteractButtonFactory.MakeInteractButton(null, new ToggleButton(), null));
		}
	}
}
