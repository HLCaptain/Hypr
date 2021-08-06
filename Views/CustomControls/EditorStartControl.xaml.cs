using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;
using Microsoft.Toolkit.Mvvm.Input;
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
    public sealed partial class EditorStartControl : UserControl {
		private ICommand _createProjectCommand;
		private ICommand _openProjectCommand;
		private ICommand _createDiagramCommand;

		public ICommand CreateProjectCommand => _createProjectCommand ?? (_createProjectCommand = new RelayCommand(ProjectService.CreateProject));
		public ICommand OpenProjectCommand => _openProjectCommand ?? (_openProjectCommand = new RelayCommand(ProjectService.OpenProject));
		// todo add a strategy dependency injection onto this method
		public ICommand CreateDiagramCommand => _createDiagramCommand ?? (_createDiagramCommand = new RelayCommand(CreateDiagram));

		public event Action<Diagram, EditorStartControl> OpenDiagramEvent;

		public EditorStartControl() {
            this.InitializeComponent();
            ProjectService.ProjectChangedEvent += RefreshItems;
            RefreshItems();
        }
		private void RefreshItems() {
			grid.Children.Clear();
			if (ProjectService.CurrentProject == null) {
				grid.Children.Add(new AppBarButton() {
					Label = "New Project",
					Icon = new SymbolIcon(Symbol.Add),
					Command = CreateProjectCommand
				});
				grid.Children.Add(new AppBarButton() {
					Label = "Open Project",
					Icon = new SymbolIcon(Symbol.OpenFile),
					Command = OpenProjectCommand
				});
			} else {
				grid.Children.Add(new AppBarButton() {
					Label = "New Diagram",
					Icon = new SymbolIcon(Symbol.Add),
					Command = CreateDiagramCommand
				});
			}
		}

		private async void CreateDiagram() {
			var diagram = await FilesystemService.CreateDiagram();
			if (diagram == null) {
				return;
			}
			ProjectService.AddFileToProjectList(diagram.File, ProjectService.CurrentProject.Diagrams);

			// saying, HEY, I WANNA OPEN A DIAGRAM
			OpenDiagramEvent?.Invoke(diagram, this);
		}
	}
}
