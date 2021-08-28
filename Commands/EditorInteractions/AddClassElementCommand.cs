using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Windows.UI.Xaml.Input;

namespace HyprWinUI3.Commands.EditorInteractions {
	public class AddClassElementCommand : EditorInteractionBase {
		public Stack<ElementView> Elements { get; set; } = new Stack<ElementView>();
		public AddClassElementCommand(EditorApp editor) : base(editor) {
			Initialize();
		}

		// Adding commands
		private void Initialize() {
			var executeCommand = new StandardUICommand(StandardUICommandKind.None);
			executeCommand.ExecuteRequested += AddClassElement;
			ExecuteCommand = executeCommand;

			var unExecuteCommand = new StandardUICommand(StandardUICommandKind.Delete);
			unExecuteCommand.ExecuteRequested += RemoteClassElement;
			UnExecuteCommand = unExecuteCommand;
		}

		private void RemoteClassElement(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			var element = Elements.Pop();
			Canvas.ForegroundCanvas.Children.Remove(element);
			Model.Vertices.Remove(element.Vertex);
		}

		private async void AddClassElement(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			// create actor
			var element = new ClassElement();
			element.Name = "Class name";
			await FilesystemService.CreateElementFile(element);
			Vertex vertex = new Vertex() { Element = element };
			Model.Vertices.Add(vertex);
			// todo refresh list automatically
			// add actor to places

			var view = new ElementView() {
				Vertex = vertex,
				View = new ClassElementView(element)
			};
			Canvas.ForegroundCanvas.Children.Add(view);
			Elements.Push(view);
		}
	}
}
