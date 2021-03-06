using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;

namespace HyprWinUI3.Constants {
	public static class Extentions {
		public static string ClassDiagramExtention { get; } = ".hycd"; // Class diagram
		public static string SequenceDiagramExtention { get; } = ".hysq"; // Sequential diagram
		public static string UseCaseDiagramExtention { get; } = ".hyuc"; // Use case diagram
		public static string TextExtention { get; } = ".txt"; // Text editor
		public static string NoteExtention { get; } = ".hynote";

		public static List<string> DiagramExtentions { get; } = new List<string>() {
			ClassDiagramExtention,
			SequenceDiagramExtention,
			UseCaseDiagramExtention,
		};

		public static List<string> EditorExtentions { get; } = new List<string>() {
			ClassDiagramExtention,
			SequenceDiagramExtention,
			UseCaseDiagramExtention,
			TextExtention,
			NoteExtention
		};

		public static string ClassElementExtention { get; } = ".hyclass"; // Class element
		public static string EdgeElementExtention { get; } = ".hyedge"; // Edge element
		public static List<string> ElementExtentions { get; } = new List<string>() {
			ClassElementExtention,
			EdgeElementExtention,
			NoteExtention
		};

		public static Dictionary<string, Type> ExtentionAppTypes { get; } = new Dictionary<string, Type>() {
			{ ClassDiagramExtention, typeof(ClassDiagramEditorApp) },
			{ SequenceDiagramExtention, typeof(SequenceDiagramEditorApp) }, // todo sequential diagram class
			{ UseCaseDiagramExtention, typeof(UseCaseDiagramEditorApp) },
			{ TextExtention, typeof(TextEditorApp) },
			{ NoteExtention, typeof(TextEditorApp) }
		};

		public static Dictionary<string, Type> ExtentionEdgeTypes { get; } = new Dictionary<string, Type>() {
			{ EdgeElementExtention, typeof(Edge) },
		};

		public static Dictionary<string, Type> ExtentionVertexTypes { get; } = new Dictionary<string, Type>() {
			{ ClassElementExtention, typeof(ClassElement) },
			{ NoteExtention, typeof(Note) },
		};

		public static Dictionary<string, Type> ExtentionDiagramTypes { get; } = new Dictionary<string, Type>() {
			{ ClassDiagramExtention, typeof(ClassDiagram) },
			{ SequenceDiagramExtention, typeof(SequenceDiagram) }, // todo sequential diagram class
			{ UseCaseDiagramExtention, typeof(UseCaseDiagram) },
		};
	}
}
