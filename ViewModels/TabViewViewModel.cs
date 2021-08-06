using System;
using System.Collections.ObjectModel;
using System.Linq;

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

namespace HyprWinUI3.ViewModels
{
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
			var editor = new EditorControl() {
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch
			};
			var editorStart = editor.Grid.Children[0] as EditorStartControl;
			editorStart.OpenDiagramEvent += (diagram, start) => OpenDiagram(diagram, start);

			// Adding the empty editor.
			Tabs.Add(new TabViewItem() {
				Header = "New tab",
				Content = editor
			});
		}

		private void CloseTab(WinUI.TabViewTabCloseRequestedEventArgs args) {
			if (args.Item is TabViewItem item) {
				Tabs.Remove(item);
			}
		}

		public int OpenDiagram(Diagram diagram) {
			if (diagram == null) {
				return -1;
			}

			for (int i = 0; i < Tabs.Count; i++) {
				if (((EditorControl)Tabs[i].Content).CurrentDiagram == null) {
					continue;
				}
				if ((((EditorControl)Tabs[i].Content).CurrentDiagram.Uid ?? "") == diagram.Uid) {
					return i;
				}
			}
			Tabs.Add(new TabViewItem() {
				Header = diagram.Name ?? "Diagram",
				Content = new EditorControl(diagram) {
					VerticalAlignment = VerticalAlignment.Stretch,
					HorizontalAlignment = HorizontalAlignment.Stretch
				}
			});
			return Tabs.Count - 1;
		}

		public int OpenDiagram(Diagram diagram, int index) {
			if (diagram == null) {
				return -1;
			}
			Tabs[index].Header = diagram.Name ?? "Diagram";
			((EditorControl)Tabs[index].Content).LoadDiagram(diagram);
			return index;
		}

		public int OpenDiagram(Diagram diagram, EditorStartControl editorStart) {
			var startItems = Tabs.Where((item) => { return (item.Content as EditorControl).Grid.Children[0] as EditorStartControl != null; });
			foreach (var item in startItems) {
				if (editorStart == (item.Content as EditorControl).Grid.Children[0] as EditorStartControl) {
					return OpenDiagram(diagram, Tabs.IndexOf(item));
				}
			}
			return -1;
		}
	}
}
