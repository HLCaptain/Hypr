using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HyprWinUI3.Commands {
	public class CommandBase {
		public ICommand ExecuteCommand { get; set; }
		public ICommand UnExecuteCommand { get; set; }
	}
}
