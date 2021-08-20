using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HyprWinUI3.Views.CustomControls;

namespace HyprWinUI3.Commands.EditorInteractions {
	public abstract class EditorInteractionBase : CommandBase {
		public EditorCanvasControl Canvas { get; set; }
		public CommandProcessor CommandProcessor { get; set; }
	}
}
