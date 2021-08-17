using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.EditorApps {
	public abstract class EditorApp {
		// model representation
		public Actor Model { get; protected set; }
		public UIElement View { get; set; } = new TextBox() { Text = "Editor not initialized" };
		public abstract void InitializeView();
		public abstract bool LoadData(string data);
		public abstract bool SaveData(StorageFolder folder);
	}
}
