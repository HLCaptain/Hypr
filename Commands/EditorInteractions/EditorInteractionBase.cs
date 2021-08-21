using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Views.CustomControls;

namespace HyprWinUI3.Commands.EditorInteractions {
	public abstract class EditorInteractionBase : CommandBase {
		public EditorCanvasControl Canvas { get; private set; }
		public EditorApps.EditorApp Editor {
			get { return Editor; }
			set {
				Canvas = ((EditorDiagramControl)value.View).Canvas;
				CommandProcessor = value.CommandProcessor;
				Model = (Diagram)value.Model;
			}
		}
		public CommandProcessor CommandProcessor { get; private set; }
		public Diagram Model { get; private set; }
	}
}
