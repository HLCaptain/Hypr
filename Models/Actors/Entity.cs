using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HyprWinUI3.Models.Actors {
	public abstract class Entity : ObservableObject {
		public string Uid { get; set; } = Guid.NewGuid().ToString();

		[JsonIgnore]
		private string name = "";
		public string Name { get => name; set => SetProperty<string>(ref name, value); }
	}
}