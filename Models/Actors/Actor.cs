using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HyprWinUI3.Strategies;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Storage;

namespace HyprWinUI3.Models.Actors {
	/// <summary>
	/// Actors can be saved and can be represented in the tree view as an item (always leaf).
	/// </summary>
	public abstract class Actor : Entity {
		[JsonIgnore]
		public StorageFile File { get; set; }
	}
}
