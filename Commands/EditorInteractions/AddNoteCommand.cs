using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace HyprWinUI3.Commands.EditorInteractions {
	public class AddNoteCommand : EditorInteractionBase {
		public Stack<ElementView> Elements { get; set; } = new Stack<ElementView>();
		public AddNoteCommand(EditorApp editor) {
			Editor = editor;
			Initialize();
		}

		// Adding commands
		private void Initialize() {
			var executeCommand = new StandardUICommand(StandardUICommandKind.None);
			executeCommand.ExecuteRequested += AddNote;
			ExecuteCommand = executeCommand;

			var unExecuteCommand = new StandardUICommand(StandardUICommandKind.Delete);
			unExecuteCommand.ExecuteRequested += RemoveNote;
			UnExecuteCommand = unExecuteCommand;
		}

		private void RemoveNote(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			var element = Elements.Pop();
			Canvas.ForegroundCanvas.Children.Remove(element);
			Model.Vertices.Remove(element.Vertex);
		}

		private async void AddNote(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			// create actor
			var note = new Note();
			note.Name = "Note name";
			note.Text = "Note text";
			await FilesystemService.CreateElementFile(note);
			Vertex vertex = new Vertex() { Element = note };
			Model.Vertices.Add(vertex);
			// todo refresh list automatically
			// add actor to places

			var view = new ElementView() {
				Vertex = vertex,
				View = new NoteView(note)
			};
			Canvas.ForegroundCanvas.Children.Add(view);
			Elements.Push(view);

			// save
			await FilesystemService.SaveActorFile(note);
		}
	}
}
