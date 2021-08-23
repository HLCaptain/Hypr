using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace HyprWinUI3.Models.Actors {
	public class Note : Vertex {
		[JsonIgnore]
		private string text = "";
		public string Text { get => text; set => SetProperty<string>(ref text, value); }
	}
}
