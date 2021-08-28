using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;
using Newtonsoft.Json;

namespace HyprWinUI3.Models.Diagrams {
	public class Project : Actor {
		/// <summary>
		/// Relative paths to Diagrams.
		/// </summary>
		[JsonIgnore]
		private ObservableCollection<string> documents = new ObservableCollection<string>();
		public ObservableCollection<string> Documents { get => documents; set => SetProperty(ref documents, value); }

		public Project() {
			Documents.CollectionChanged += (sender, args) => {
				if (ProjectService.CurrentProject == this) {
					foreach (var item in args.NewItems) {
						ProjectService.DocumentProxies.Add(new Proxy.DocumentProxy((string)item));
					}
				}
			};
			PropertyChanged += (sender, args) => {
				if (args.PropertyName == "Documents") {
					Documents.CollectionChanged += (sender2, args2) => {
						if (ProjectService.CurrentProject == this) {
							foreach (var item in args2.NewItems) {
								ProjectService.DocumentProxies.Add(new Proxy.DocumentProxy((string)item));
							}
						}
					};
				}
			};
		}
	}
}
