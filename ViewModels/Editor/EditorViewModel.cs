using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.ViewModels.Editor {
	/// <summary>
	/// Editor instance.
	/// </summary>
	public class EditorViewModel : ObservableObject {
		private ICommand _createProjectCommand;
		private ICommand _openProjectCommand;
		private ICommand _createEditorCommand;

		public ICommand CreateProjectCommand => _createProjectCommand ?? (_createProjectCommand = new RelayCommand(ProjectService.CreateProject));
		public ICommand OpenProjectCommand => _openProjectCommand ?? (_openProjectCommand = new RelayCommand(ProjectService.OpenProject));
		// todo add a strategy dependency injection onto this method
		public ICommand CreateEditorCommand => _createEditorCommand ?? (_createEditorCommand = new RelayCommand(CreateEditor));

		public EditorApp CurrentEditor { get; set; }
		public VariableSizedWrapGrid Grid { get; set; }
		public EditorControl View { get; set; }
		public TabViewItem TabViewItem { get; set; }

		/// <summary>
		/// Loads up a diagram to display its content in an EditorDiagramControl.
		/// </summary>
		/// <param name="diagram">Diagram to display.</param>
		public void LoadEditor() {
			// An empty Grid is a sign of loaded Editor.
			if (CurrentEditor == null) {
				throw new NullReferenceException("Editor cannot load in because it was null.");
			}
			Grid?.Children.Clear();
			CurrentEditor?.RefreshView();
			View.Content = CurrentEditor?.View;
			if (CurrentEditor?.Model?.File == null) {
				TabViewItem.Header = CurrentEditor?.Model?.Name ?? "Editor";
			} else {
				TabViewItem.Header = CurrentEditor?.Model?.File.Name ?? "Editor";
			}

		}

		public void LoadEditor(EditorApp editor) {
			if (editor == null) {
				throw new ArgumentNullException();
			}
			CurrentEditor = editor;
			LoadEditor();
		}

		public void RefreshItems() {
			if (CurrentEditor != null) {
				return;
			}
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
					Command = CreateEditorCommand
				});
			}
		}

		private async void CreateEditor() {
			var editor = await FilesystemService.CreateActor();
			if (editor != null) {
				LoadEditor(editor);
			}
		}
	}
}

