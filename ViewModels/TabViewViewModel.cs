using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Factories;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace HyprWinUI3.ViewModels {
	public class TabViewViewModel : ObservableObject {
		private RelayCommand _addTabCommand;
		private RelayCommand<TabViewTabCloseRequestedEventArgs> _closeTabCommand;

		public RelayCommand AddTabCommand => _addTabCommand ?? (_addTabCommand = new RelayCommand(AddTab));

		public RelayCommand<TabViewTabCloseRequestedEventArgs> CloseTabCommand => _closeTabCommand ?? (_closeTabCommand = new RelayCommand<TabViewTabCloseRequestedEventArgs>(CloseTab));

		public ObservableCollection<TabViewItem> Tabs { get; } = new ObservableCollection<TabViewItem>();
		public HorizontalAlignment HorizontalAlignment { get; private set; }

		public TabViewViewModel() {
			AddTab();
		}

		private void AddTab() {
			// Creating empty editor, then subbing to Open Diagram input.
			// todo change var to explicit type. Future you will thank you, because it is more readable.

			// Adding the empty editor.
			

			var tabViewItem = new TabViewItem() {
				Header = "New tab",
			};
			var control = new EditorControl(tabViewItem) {
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch,
			};
			tabViewItem.Content = control;
			Tabs.Add(tabViewItem);
		}

		private void CloseTab(TabViewTabCloseRequestedEventArgs args) {
			if (args.Item is TabViewItem item) {
				Tabs.Remove(item);
			}
		}

		public int OpenEditor(EditorApp editor) {
			if (editor == null) {
				return -1;
			}

			for (int i = 0; i < Tabs.Count; i++) {
				if (((EditorControl)Tabs[i].Content).ViewModel.CurrentEditor?.Model?.Uid.Equals(editor.Model.Uid) ?? false) {
					return i;
				}
			}
			var tabViewItem = new TabViewItem() {
				Header = editor.Model.Name ?? "Editor",
			};
			var control = new EditorControl(editor, tabViewItem) {
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch,
			};
			tabViewItem.Content = control;
			Tabs.Add(tabViewItem);
			return Tabs.Count - 1;
		}

		public int OpenEditor(EditorApp editor, int index) {
			if (editor == null) {
				return -1;
			}
			Tabs[index].Header = editor.Model.Name ?? "Editor";
			((EditorControl)Tabs[index].Content).ViewModel.LoadEditor(editor);
			return index;
		}

		public int OpenEditor(EditorApp editor, EditorControl control) {
			foreach (var item in Tabs) {
				if (control == (item.Content as EditorControl)) {
					return OpenEditor(editor, Tabs.IndexOf(item));
				}
			}
			return -1;
		}

		public async Task<int> OpenEditor(StorageFile file) {
			var editor = await EditorAppFactory.CreateEditorFromFile(file);
			foreach (var item in Tabs) {
				var itemEditor = ((EditorControl)item.Content).ViewModel.CurrentEditor;
				if (itemEditor?.Model == editor.Model) {
					return OpenEditor(itemEditor);
				}
			}
			return OpenEditor(editor);
		}
	}
}
