using System;
using System.Collections.Generic;
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
		public Stack<NoteView> Elements { get; set; } = new Stack<NoteView>();
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
			string relativePath = FilesystemService.RelativePathFromFolder(ProjectService.RootFolder, element.Note.File);
			Model.Elements.Remove(relativePath);
		}

		private async void AddNote(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			// create actor
			var note = new Note();
			note.Name = "Note name - " + note.Uid;
			note.Text = "Note text";
			await FilesystemService.CreateElementFile(note, Model.Elements);
			var view = new NoteView(Canvas.ForegroundCanvas, note);
			
			// add actor to places
			Canvas.ForegroundCanvas.Children.Add(view);
			Elements.Push(view);

			// save
			await FilesystemService.SaveActorFile(note);
		}
	}
}
