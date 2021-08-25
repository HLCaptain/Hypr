using System;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Views
{
	// For more info about the TabView Control see
	// https://docs.microsoft.com/uwp/api/microsoft.ui.xaml.controls.tabview?view=winui-2.2
	// For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
	public sealed partial class TabViewPage : Page {
		public TabViewViewModel ViewModel { get; set; }
		public Grid Grid { get => grid; }
		public VariableSizedWrapGrid InfoBarGrid { get => infoBarGrid; }

		public TabViewPage() {
			InitializeComponent();
			InfoService.InfoBarGrid = InfoBarGrid;
			ViewModel = new TabViewViewModel();
			treeView.TabViewViewModel = ViewModel;
			ProjectService.OpenEditorEvent += (editor) => OpenEditor(editor);
			ProjectService.OpenEditorFileEvent += (file) => OpenEditor(file);
		}

		private void OpenEditor(EditorApp editor) {
			int index = ViewModel.OpenEditor(editor);
			try {
				tabView.SelectedIndex = index;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
		}
		private async void OpenEditor(StorageFile file) {
			int index = await ViewModel.OpenEditor(file);
			try {
				tabView.SelectedIndex = index;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
		}
	}
}
