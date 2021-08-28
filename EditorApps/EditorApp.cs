using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Commands;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Strategies.LoadStrategy;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.EditorApps {
	public abstract class EditorApp {
		// model representation
		public Actor Model { get; set; }
		public CommandProcessor CommandProcessor { get; } = new CommandProcessor();
		public UIElement View { get; protected set; } = new TextBox() { Text = "Editor not initialized" };
		public abstract void RefreshView();
	}
}
