using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.ViewModels;
using HyprWinUI3.ViewModels.Editor;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
    public sealed partial class EditorControl : UserControl {
        public EditorViewModel ViewModel { get; set; }
        public Diagram CurrentDiagram { get; set; }
        public Grid Grid { get => grid; }
        public EditorControl() {
            this.InitializeComponent();
            ViewModel = new EditorViewModel();
            Initialize();
        }

        public EditorControl(Diagram diagram) {
            this.InitializeComponent();
            ViewModel = new EditorViewModel();
            CurrentDiagram = diagram;
            LoadDiagram(diagram);
        }

        /// <summary>
		/// Initializes with an EditorStartControl, because there are no files yet to open.
		/// </summary>
        private void Initialize() {
            Grid?.Children.Clear();
            Grid?.Children.Add(new EditorStartControl() {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            });
        }
        /// <summary>
		/// Loads up a diagram to display its content in an EditorDiagramControl.
		/// </summary>
		/// <param name="diagram">Diagram to display.</param>
        public void LoadDiagram(Diagram diagram) {
            if (diagram == null) {
                return;
            }
            Grid?.Children.Clear();
            var editor = new EditorDiagramControl(diagram) {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            CurrentDiagram = diagram;
            Grid?.Children.Add(editor);
        }
    }
}
