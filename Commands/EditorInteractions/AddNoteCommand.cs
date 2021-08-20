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
using Windows.UI.Xaml.Input;

namespace HyprWinUI3.Commands.EditorInteractions {
	public class AddNoteCommand : EditorInteractionBase {
		public Stack<UIElement> Elements { get; set; } = new Stack<UIElement>();
		public AddNoteCommand(EditorCanvasControl canvas, CommandProcessor commandProcessor) {
			Canvas = canvas;
			CommandProcessor = commandProcessor;
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
			Canvas.ForegroundCanvas.Children.Remove(Elements.Pop());
		}

		private async void AddNote(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			var note = new Note();
			await FilesystemService.SaveActorFile(note);
			var view = new NoteView(Canvas.ForegroundCanvas, note);
			ToolTipService.SetToolTip(view, $"File\nName: {note.File?.Name}\nUid: {note.Uid}\nPath: {note.File?.Path}");
			Canvas.ForegroundCanvas.Children.Add(view);
			Elements.Push(view);
		}
	}
}
