using System;
using System.Collections.ObjectModel;
using System.Linq;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Helpers;
using HyprWinUI3.Models;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.Views;
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
		private RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> _closeTabCommand;

		public RelayCommand AddTabCommand => _addTabCommand ?? (_addTabCommand = new RelayCommand(AddTab));

		public RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> CloseTabCommand => _closeTabCommand ?? (_closeTabCommand = new RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs>(CloseTab));

		public ObservableCollection<TabViewItem> Tabs { get; } = new ObservableCollection<TabViewItem>();
		public HorizontalAlignment HorizontalAlignment { get; private set; }

		public TabViewViewModel() {
			AddTab();
		}

		private void AddTab() {
			// Creating empty editor, then subbing to Open Diagram input.
			// todo change var to explicit type. Future you will thank you, because it is more readable.
			var control = new EditorControl() {
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			control.OpenEditorEvent += (editor, editorControl) => OpenEditor(editor, editorControl);

			// Adding the empty editor.
			Tabs.Add(new TabViewItem() {
				Header = "New tab",
				Content = control
			});
		}

		private void CloseTab(WinUI.TabViewTabCloseRequestedEventArgs args) {
			if (args.Item is TabViewItem item) {
				Tabs.Remove(item);
			}
		}

		public int OpenEditor(EditorApp editor) {
			if (editor == null) {
				return -1;
			}

			for (int i = 0; i < Tabs.Count; i++) {
				if (((EditorControl)Tabs[i].Content).CurrentEditor == null) {
					continue;
				}
				if ((((EditorControl)Tabs[i].Content).CurrentEditor.Model.Uid ?? "") == editor.Model.Uid) {
					return i;
				}
			}
			Tabs.Add(new TabViewItem() {
				Header = editor.Model.Name ?? "Editor",
				Content = new EditorControl(editor) {
					VerticalAlignment = VerticalAlignment.Stretch,
					HorizontalAlignment = HorizontalAlignment.Stretch
				}
			});
			return Tabs.Count - 1;
		}

		public int OpenEditor(EditorApp editor, int index) {
			if (editor == null) {
				return -1;
			}
			Tabs[index].Header = editor.Model.Name ?? "Editor";
			((EditorControl)Tabs[index].Content).LoadEditor(editor);
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
	}
}
