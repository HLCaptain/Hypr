using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HyprWinUI3.ViewModels {
    public class EditorStartControlViewModel : ObservableObject {
        public VariableSizedWrapGrid Grid;

        public EditorStartControlViewModel(VariableSizedWrapGrid grid) {
            Grid = grid;
            ProjectService.ProjectChangedEvent += RefreshItems;
            RefreshItems();
        }

        // todo: replace dummy items with real ones
        private void RefreshItems() {
            Grid.Children.Clear();
            if (ProjectService.CurrentProject == null) {
                Grid.Children.Add(new AppBarButton() {
                    Label = "New Project",
                    Icon = new SymbolIcon(Symbol.Add)
                });
            }
            Grid.Children.Add(new AppBarButton() {
                Label = "New Class Diagram",
                Icon = new SymbolIcon(Symbol.Add)
            });
        }
    }
}
