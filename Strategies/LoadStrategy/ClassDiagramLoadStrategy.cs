using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Strategies.LoadStrategy {
	public class ClassDiagramLoadStrategy : ILoadStrategy {
		public async Task LoadToCanvas(Canvas canvas, object entity) {
			// ini
			// view list needed for edge
			List<ElementView> views = new List<ElementView>();
			foreach (var view in canvas.Children) {
				views.Add((ElementView)view);
			}

			// is it a vertex?
			Element element = null;
			if (entity is Vertex) {
				element = (Element)await FilesystemService.LoadActor(((Vertex)entity).ElementReference);
				((Vertex)entity).Element = element;
			} else if (entity is Element) {
				element = (Element)entity;
			}

			// decide what to create
			if (element is Note) {
				canvas.Children.Add(new ElementView() {
					Vertex = (Vertex)entity,
					View = new NoteView((Note)element)
				});
			} else if (element is Edge) {
				// todo view
			} else if (element is ClassElement) {
				// todo view
			}
		}
	}
}
