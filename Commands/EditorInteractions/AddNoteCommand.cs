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
			Model.Elements.Remove(element.Note);
		}

		private async void AddNote(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			// create actor
			var note = new Note();
			note.Name = "Note name - " + note.Uid;
			note.Text = "Note text";
			await FilesystemService.SaveActorFile(note);
			// from now on, every modification made to the actor, it saves
			note.PropertyChanged += async (sender2, args2) => {
				if (args2.PropertyName == "Name") {
					await FilesystemService.RenameItem(note.File, note.Name);
				} else {
					await FilesystemService.SaveActorFile(note);
				}
			};
			var view = new NoteView(Canvas.ForegroundCanvas, note);
			ToolTipService.SetToolTip(view, $"File\nName: {note.File?.Name}\nUid: {note.Uid}\nPath: {note.File?.Path}");

			// add actor to places
			Canvas.ForegroundCanvas.Children.Add(view);
			Elements.Push(view);
			Model.Elements.Add(note);

			// save
			await FilesystemService.SaveActorFile(Model);
		}
	}
}
