using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HyprWinUI3.Services;
using HyprWinUI3.Strategies;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace HyprWinUI3.Models.Actors {
	/// <summary>
	/// Actors can be saved and can be represented in the tree view as an item (always leaf).
	/// </summary>
	public abstract class Actor : Entity {
		[JsonIgnore]
		public StorageFile File { get; set; } = null;
		public Actor() {
			PropertyChanged += async (sender2, args2) => {
				if (File == null) {
					return;
				}
				if (args2.PropertyName == "Name") {
					await FilesystemService.RenameItem(File, Name);
				} else if (args2.PropertyName == "File") {
					if (File == null) {

					}
				} else {
					await FilesystemService.SaveActorFile(this);
				}
			};
		}
	}
}
