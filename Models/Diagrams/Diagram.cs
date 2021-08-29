using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Diagrams {
	public abstract class Diagram : Document {
		public ObservableCollection<Vertex> Vertices { get; set; } = new ObservableCollection<Vertex>();
		public ObservableCollection<string> Edges { get; set; } = new ObservableCollection<string>();

		public Diagram() {
			References.CollectionChanged += async (sender, args) => {
				if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace) {
					foreach (var oldItem in args.OldItems) {
						foreach (var newItem in args.NewItems) {
							// change old reference to new
							foreach (var vertex in Vertices) {
								if (vertex.ElementReference == (string)oldItem) {
									vertex.ElementReference = (string)newItem;
								}
							}
							for (int i = 0; i < Edges.Count; i++) {
								if (Edges[i] == (string)oldItem) {
									Edges[i] = (string)newItem;
								}
							}
						}
					}
				}
			};
			Vertices.CollectionChanged += async (sender, args) => {
				foreach (var item in args.NewItems) {
					((Vertex)item).PropertyChanged += VertexChanged;
					References.Add(((Vertex)item).ElementReference);
				}
				if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
					foreach (var item in args.OldItems) {
						References.Remove(((Vertex)item).ElementReference);
					}
				}
			};
			Edges.CollectionChanged += async (sender, args) => {
				foreach (var item in args.NewItems) {
					References.Add((string)item);
				}
				if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) {
					foreach (var item in args.OldItems) {
						References.Remove((string)item);
					}
				}
			};
		}

		private async void VertexChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (File != null) {

				// references are changing globally, so dont save on every property change
				if (e.PropertyName != "ElementReference") {
					await FilesystemService.SaveActorFile(this);
				}
			}
		}
	}
}
