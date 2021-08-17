using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;
using HyprWinUI3.ViewModels.Editor;
using Microsoft.Toolkit.Mvvm.Input;
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
		private ICommand _createProjectCommand;
		private ICommand _openProjectCommand;
		private ICommand _createEditorCommand;

		public ICommand CreateProjectCommand => _createProjectCommand ?? (_createProjectCommand = new RelayCommand(ProjectService.CreateProject));
		public ICommand OpenProjectCommand => _openProjectCommand ?? (_openProjectCommand = new RelayCommand(ProjectService.OpenProject));
		// todo add a strategy dependency injection onto this method
		public ICommand CreateEditorCommand => _createEditorCommand ?? (_createEditorCommand = new RelayCommand(CreateEditor));

		public event Action<EditorApp, EditorControl> OpenEditorEvent;

		public EditorViewModel ViewModel { get; set; }
		public EditorApp CurrentEditor { get; set; }
		public VariableSizedWrapGrid Grid { get => grid; }
		public EditorControl() {
			this.InitializeComponent();
			ViewModel = new EditorViewModel();
			ProjectService.ProjectChangedEvent += RefreshItems;
			RefreshItems();
		}

		public EditorControl(EditorApps.EditorApp editor) {
			this.InitializeComponent();
			ViewModel = new EditorViewModel();
			CurrentEditor = editor;
			LoadEditor();
		}

		/// <summary>
		/// Loads up a diagram to display its content in an EditorDiagramControl.
		/// </summary>
		/// <param name="diagram">Diagram to display.</param>
		public void LoadEditor() {
			// An empty Grid is a sign of loaded Editor.
			Grid?.Children.Clear();
			CurrentEditor?.InitializeView();
			Content = CurrentEditor?.View;
		}

		public void LoadEditor(EditorApp editor) {
			CurrentEditor = editor;
			LoadEditor();
		}

		private void RefreshItems() {
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
			if (editor == null) {
				return;
			}
			// todo add different editors (like diagram editors) to project somehow somewhere

			// saying, HEY, I WANNA OPEN AN EDITOR
			OpenEditorEvent?.Invoke(editor, this);
		}
	}
}
