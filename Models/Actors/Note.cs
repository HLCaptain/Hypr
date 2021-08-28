using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Actors {
	public class Note : Element {
		[JsonIgnore]
		private string text = "";
		public string Text { get => text; set => SetProperty(ref text, value); }

		public Note() {
			PropertyChanged += async (sender, args) => {
				if (File == null) {
					return;
				}
				if (args.PropertyName == "Text") {
					await FilesystemService.SaveActorFile(this);
				}
			};
		}
	}
}
