using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Actors {
	public abstract class Edge : Element {
		// Vertex's file path
		[JsonIgnore]
		private string end = "";
		public string End { get => end; set => SetProperty<string>(ref end, value); }
		// Vertex's file path
		[JsonIgnore]
		private string start = "";
		public string Start { get => start; set => SetProperty<string>(ref start, value); }

		public Edge() {
			PropertyChanged += async (sender, args) => {
				if (File == null) {
					return;
				}
				await FilesystemService.SaveActorFile(this);
			};
		}
	}
}
