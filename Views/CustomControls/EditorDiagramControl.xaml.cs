using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Models.Diagrams;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
	public sealed partial class EditorDiagramControl : UserControl {
        public Diagram CurrentDiagram { get; set; }
		public EditorDiagramControl(Diagram diagram) {
            CurrentDiagram = diagram;
			this.InitializeComponent();
            Initialize();
        }

        private void Initialize() {
            // todo draw elements
            CurrentDiagram?.LoadStrategy.LoadToEditor(toolbar.Grid);
        }
	}
}
