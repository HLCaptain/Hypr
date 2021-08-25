using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyprWinUI3.Commands {
	public class CommandProcessor {
		public event Action<CommandBase> CommandExecutedEvent;
		public event Action<CommandBase> CommandUnExecutedEvent;

		private Stack<CommandBase> UndoBuffer = new Stack<CommandBase>();
		private Stack<CommandBase> RedoBuffer = new Stack<CommandBase>();

		public void Execute(CommandBase command, object parameter = null) {
			RedoBuffer.Clear();
			command.ExecuteCommand.Execute(parameter);
			CommandExecutedEvent?.Invoke(command);
			UndoBuffer.Push(command);
		}
		public void UnExecute(object parameter = null) {
			var command = UndoBuffer.Pop();
			command.UnExecuteCommand.Execute(parameter);
			CommandUnExecutedEvent?.Invoke(command);
			RedoBuffer.Push(command);
		}
	}
}
