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
			Documents.CollectionChanged += Documents_CollectionChanged;
			PropertyChanged += (sender, args) => {
				if (args.PropertyName == "Documents") {
					Documents.CollectionChanged += Documents_CollectionChanged;
				}
			};
		}

		private async void Documents_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			if (ProjectService.CurrentProject == this) {
				if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) {
					foreach (var item in e.NewItems) {
						ProjectService.DocumentProxies.Add(new Proxy.DocumentProxy((string)item));
					}
				}
				if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
					foreach (var item in e.NewItems) {
						var oldProxies = ProjectService.DocumentProxies.Where((proxy) => {
							return proxy.ReferencePath == (string)item;
						});
						foreach (var OldProxy in oldProxies) {
							ProjectService.DocumentProxies.Remove(OldProxy);
						}
					}
				}
			}
			await ProjectService.SaveProjectFile();
		}
	}
}
