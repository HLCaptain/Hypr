using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;
using Windows.UI.Xaml.Input;

namespace HyprWinUI3.Commands.EditorInteractions {
	public class AddFieldCommand : CommandBase {
		public ClassElement Element { get; set; }
		public Stack<FieldVariable> Variables { get; set; } = new Stack<FieldVariable>();
		public AddFieldCommand(ClassElement element = null) {
			Element = element;
			Initialize();
		}
		// Adding commands
		private void Initialize() {
			var executeCommand = new StandardUICommand(StandardUICommandKind.None);
			executeCommand.ExecuteRequested += AddField;
			ExecuteCommand = executeCommand;

			var unExecuteCommand = new StandardUICommand(StandardUICommandKind.Delete);
			unExecuteCommand.ExecuteRequested += RemoveField;
			UnExecuteCommand = unExecuteCommand;
		}

		private void RemoveField(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			Element.Fields.Remove(Variables.Pop());
		}

		private async void AddField(XamlUICommand sender, ExecuteRequestedEventArgs args) {
			// create actor
			var field = new FieldVariable();
			field.Name = "Field name";
			field.IsConst = false;
			field.Type = "object";
			field.Visibility = Visibility.Public;
			await FilesystemService.CreateElementFile(field);

			// todo refresh list automatically
			// add actor to places
			Element.Fields.Add(field);
		}
	}
}
