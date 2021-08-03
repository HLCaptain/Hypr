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

		public TabView TabView { get; set; }
		public ObservableCollection<TabViewItem> Tabs { get; } = new ObservableCollection<TabViewItem>();
		public HorizontalAlignment HorizontalAlignment { get; private set; }

		public TabViewViewModel(TabView tabView) {
			TabView = tabView;
			Tabs.Add(new TabViewItem() {
				Header = "New tab",
				//// In this sample the content shown in the Tab is a string, set the content to the model you want to show
				Content = new EditorControl(this) {
					VerticalAlignment = VerticalAlignment.Stretch,
					HorizontalAlignment = HorizontalAlignment.Stretch
				}
			});
			ProjectService.OpenDiagramEvent += OpenDiagram;
		}

		private void AddTab() {
			Tabs.Add(new TabViewItem() {
				Header = "New tab",
				Content = new EditorControl(this) {
					VerticalAlignment = VerticalAlignment.Stretch,
					HorizontalAlignment = HorizontalAlignment.Stretch
				}
			});
			TabView.SelectedIndex = Tabs.Count - 1;
		}

		private void CloseTab(WinUI.TabViewTabCloseRequestedEventArgs args) {
			if (args.Item is TabViewItem item) {
				Tabs.Remove(item);
			}
		}

		public void OpenDiagram(Diagram diagram) {
			if (diagram == null) {
				return;
			}
			for (int i = 0; i < Tabs.Count; i++) {
				if (((EditorControl)Tabs[i].Content).CurrentDiagram == null) {
					continue;
				}
				if ((((EditorControl)Tabs[i].Content).CurrentDiagram.Uid ?? "") == diagram.Uid) {
					TabView.SelectedIndex = i;
					return;
				}
			}
			Tabs.Add(new TabViewItem() {
				Header = diagram.Name ?? "Diagram",
				Content = new EditorControl(this, diagram) {
					VerticalAlignment = VerticalAlignment.Stretch,
					HorizontalAlignment = HorizontalAlignment.Stretch
				}
			});
			TabView.SelectedIndex = Tabs.Count - 1;
		}

		public void OpenDiagram(Diagram diagram, int index) {
			if (diagram == null) {
				return;
			}
			if (index < Tabs.Count) {
				Tabs[index].Header = diagram.Name ?? "Diagram";
				((EditorControl)Tabs[index].Content).ViewModel.LoadDiagram(diagram);
			}
			TabView.SelectedIndex = index;
		}
	}
}
