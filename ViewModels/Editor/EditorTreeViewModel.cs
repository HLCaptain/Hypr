using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.ViewModels.Editor {
    public class EditorTreeViewModel : ObservableObject {
        public ObservableCollection<StackPanel> Items = new ObservableCollection<StackPanel>();

        public EditorTreeViewModel() {
            ProjectService.ProjectChangedEvent += RefreshTree;
            //RefreshTree();
        }

        private void RefreshTree() {
            //Items.Clear();
            //if (ProjectService.CurrentProject != null) {
            //    var projectLoadedPanel = new StackPanel();
            //    projectLoadedPanel.Children.Add(new SymbolIcon(Symbol.Like));
            //    projectLoadedPanel.Children.Add(new TextBlock() { Text = "Project Loaded" });
            //    Items.Add(projectLoadedPanel);
            //} else {
            //    var noProjectPanel = new StackPanel();
            //    noProjectPanel.Children.Add(new SymbolIcon(Symbol.Important));
            //    noProjectPanel.Children.Add(new TextBlock() { Text = "No Project" });
            //    Items.Add(noProjectPanel);
            //}
        }
    }
}
