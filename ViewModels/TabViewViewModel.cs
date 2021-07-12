using System;
using System.Collections.ObjectModel;
using System.Linq;

using HyprWinUI3.Helpers;
using HyprWinUI3.Models;
using HyprWinUI3.Views;
using HyprWinUI3.Views.CustomControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace HyprWinUI3.ViewModels
{
    public class TabViewViewModel : ObservableObject
    {
        private RelayCommand _addTabCommand;
        private RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> _closeTabCommand;

        public RelayCommand AddTabCommand => _addTabCommand ?? (_addTabCommand = new RelayCommand(AddTab));

        public RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs> CloseTabCommand => _closeTabCommand ?? (_closeTabCommand = new RelayCommand<WinUI.TabViewTabCloseRequestedEventArgs>(CloseTab));

        public ObservableCollection<TabViewItem> Tabs { get; } = new ObservableCollection<TabViewItem>()
        {
            new TabViewItem()
            {
                Header = "New tab",
                //// In this sample the content shown in the Tab is a string, set the content to the model you want to show
                Content = new EditorControl() {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                }
            }
        };
        public HorizontalAlignment HorizontalAlignment { get; private set; }

        public TabViewViewModel()
        {
        }

        private void AddTab()
        {
            Tabs.Add(new TabViewItem()
            {
                Header = "New tab",
                Content = new EditorControl() {
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                }
            });
        }

        private void CloseTab(WinUI.TabViewTabCloseRequestedEventArgs args)
        {
            if (args.Item is TabViewItem item)
            {
                Tabs.Remove(item);
            }
        }

        private void OpenTab(UserControl content) {
            content.VerticalAlignment = VerticalAlignment.Stretch;
            content.HorizontalAlignment = HorizontalAlignment.Stretch;
            Tabs.Add(new TabViewItem() {
                Header = "New tab",
                Content = content
            });
        }
    }
}
