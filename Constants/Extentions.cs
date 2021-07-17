using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;

namespace HyprWinUI3.Constants {
	public static class Extentions {
		public static string ClassDiagramExtention { get; } = ".hycd"; // Class diagram
		public static string SequentialDiagramExtention { get; } = ".hysq"; // Sequential diagram
		public static string UseCaseDiagramExtention { get; } = ".hyuc"; // Use case diagram

		public static List<string> DiagramExtentions { get; } = new List<string>() {
			ClassDiagramExtention,
			SequentialDiagramExtention,
			UseCaseDiagramExtention, 
		};

		public static string ClassElementExtention { get; } = ".hyclass"; // Class element
		public static string EdgeElementExtention { get; } = ".hyedge"; // Edge element
		public static List<string> ElementExtentions { get; } = new List<string>() {
			ClassElementExtention,
			EdgeElementExtention,
		};

		public static Dictionary<string, Type> ExtentionTypes { get; } = new Dictionary<string, Type>() {
			{ ClassDiagramExtention, typeof(ClassDiagram) },
			{ SequentialDiagramExtention, typeof(Diagram) }, // todo sequential diagram class
			{ UseCaseDiagramExtention, typeof(Diagram) }, // todo use case diagram class
			{ ClassElementExtention, typeof(ClassElement) },
			{ EdgeElementExtention, typeof(Edge) },
		};
	}
}
