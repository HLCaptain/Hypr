using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HyprWinUI3.Models.Actors {
	public abstract class Entity : ObservableObject {
		public string Uid { get; set; } = Guid.NewGuid().ToString();
		public string Name { get; set; } = "";
	}
}
