using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HyprWinUI3.ViewModels {
	public class EditorStartViewModel : ObservableObject {
		public VariableSizedWrapGrid Grid;
		public EditorControl EditorControl;

        private ICommand _createProjectCommand;
        private ICommand _openProjectCommand;
		private ICommand _createDiagramCommand;

        public ICommand CreateProjectCommand => _createProjectCommand ?? (_createProjectCommand = new RelayCommand(ProjectService.CreateProject));
        public ICommand OpenProjectCommand => _openProjectCommand ?? (_openProjectCommand = new RelayCommand(ProjectService.OpenProject));
		// todo add a strategy dependency injection onto this method
		public ICommand CreateDiagramCommand => _createDiagramCommand ?? (_createDiagramCommand = new RelayCommand(CreateDiagram));
		/// <summary>
		/// Start screen of an editor tab.
		/// </summary>
		/// <param name="grid">Grid to display data on.</param>
        public EditorStartViewModel(EditorControl editor, VariableSizedWrapGrid grid) {
			Grid = grid;
			EditorControl = editor;
			ProjectService.ProjectChangedEvent += RefreshItems;
			RefreshItems();
		}

		// todo: replace dummy items with real ones
		private void RefreshItems() {
			Grid.Children.Clear();
			if (ProjectService.CurrentProject == null) {
				Grid.Children.Add(new AppBarButton() {
					Label = "New Project",
					Icon = new SymbolIcon(Symbol.Add),
                    Command = CreateProjectCommand
				});
                Grid.Children.Add(new AppBarButton() {
                    Label = "Open Project",
                    Icon = new SymbolIcon(Symbol.OpenFile),
                    Command = OpenProjectCommand
                });
            } else {
				Grid.Children.Add(new AppBarButton() {
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
			ProjectService.AddDiagramFileToProject(diagram);
			for (int i = 0; i < EditorControl.TabView.TabItems.Count; i++) {
				var editor = (EditorControl)((TabViewItem)EditorControl.TabView.TabItems[i]).Content;
				if (editor == EditorControl) {
					EditorControl.CurrentDiagram = diagram;
					EditorControl.TabViewViewModel.OpenDiagram(diagram, i);
				}
			}
		}
	}
}
