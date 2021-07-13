using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.ViewModels.Editor {
    /// <summary>
	/// Editor instance.
	/// </summary>
    public class EditorViewModel : ObservableObject {
        /// <summary>
		/// The grid to display content on.
		/// </summary>
        public Grid Grid { get; set; }
        public EditorViewModel(Grid grid) {
            Grid = grid;
            Initialize();
            ProjectService.ProjectChangedEvent += ProjectChanged;
        }
        /// <summary>
		/// Initializes with an EditorStartControl, because there are no files yet to open.
		/// </summary>
        private void Initialize() {
            Grid?.Children.Clear();
            Grid?.Children.Add(new EditorStartControl() {
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
            });
        }
        /// <summary>
		/// Loads up a diagram to display its content in an EditorDiagramControl.
		/// </summary>
		/// <param name="diagram">Diagram to display.</param>
        public void LoadContent(Diagram diagram) {
            Grid?.Children.Clear();
            Grid?.Children.Add(new EditorDiagramControl(diagram) {
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch
            });
        }

        // todo remove after heavy dummy testing
        // todo LoadContent implementation PROPERLY!!
        public void ProjectChanged() {
            LoadContent(new Diagram());
        }
    }
}
