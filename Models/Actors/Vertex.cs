using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation;
using HyprWinUI3.Models.Actors;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.IO;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Actors {
	public class Vertex : ObservableObject {
		[JsonIgnore]
		private Point position;
		public Point Position { get => position; set => SetProperty(ref position, value); }
		[JsonIgnore]
		private string elementReference = "";

		public string ElementReference { get => elementReference; set => SetProperty(ref elementReference, value); }

		[JsonIgnore]
		private Element element;
		[JsonIgnore]
		public Element Element {
			get => element;
			set {
				value.PropertyChanged += (sender, args) => {
					if (args.PropertyName == "Name") {
						ElementReference = Path.GetRelativePath(ProjectService.RootFolder.Path, value.File?.Path);
					}
				};
				ElementReference = Path.GetRelativePath(ProjectService.RootFolder.Path, value.File?.Path);
				element = value;
			}
		}
	}
}
