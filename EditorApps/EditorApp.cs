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
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.EditorApps {
	public abstract class EditorApp {
		// model representation
		public Actor Model { get; protected set; }
		public CommandProcessor CommandProcessor { get; } = new CommandProcessor();
		public UIElement View { get; protected set; } = new TextBox() { Text = "Editor not initialized" };
		public abstract void RefreshView();
		public abstract bool LoadData(string data);
		public virtual async Task<bool> LoadData(StorageFile file) {
			try {
				string data = await FileIO.ReadTextAsync(file);
				LoadData(data);
				Model.File = file;
				Model.Name = file.DisplayName;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
				return false;
			}
			return true;
		}
		public virtual async Task<bool> SaveData(StorageFolder folder) {
			try {
				if (Model.File == null) {
					// save model as a new file somewhere
				} else {
					// save model's info to its file
					await FilesystemService.SaveActorFile(Model);
				}
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
				return false;
			}
			return true;
		}
	}
}
