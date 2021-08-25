using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Diagrams {
	public abstract class Diagram : Actor {
		private ObservableCollection<Vertex> vertices = new ObservableCollection<Vertex>();
		public ObservableCollection<Vertex> Vertices {
			get => vertices;
			set {
				foreach (var item in value) {
					item.PropertyChanged += VertexChanged;
				}
				vertices = value;
			}
		}
		public ObservableCollection<string> Edges { get; set; } = new ObservableCollection<string>();

		public Diagram() : base() {
			foreach (var item in Vertices) {
				item.PropertyChanged += VertexChanged;
			}
			Vertices.CollectionChanged += async (sender, args) => {
				foreach (var item in args.NewItems) {
					((Vertex)item).PropertyChanged += VertexChanged;
				}
				if (File == null) {
					return;
				}
				await FilesystemService.SaveActorFile(this);
			};
			Edges.CollectionChanged += async (sender, args) => {
				await FilesystemService.SaveActorFile(this);
			};
		}

		private async void VertexChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			if (File == null) {
				return;
			}
			await FilesystemService.SaveActorFile(this);
		}
	}
}
