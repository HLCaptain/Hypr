using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Diagrams {
	public class Project : Actor {
		/// <summary>
		/// Relative paths to Diagrams.
		/// </summary>
		public ObservableCollection<string> Documents { get; set; } = new ObservableCollection<string>();

		public Project() {
			Documents.CollectionChanged += (sender, args) => {
				if (ProjectService.CurrentProject == this) {
					foreach (var item in args.NewItems) {
						ProjectService.DocumentProxies.Add(new Proxy.DocumentProxy((string)item));
					}
				}
			};
		}
	}
}
