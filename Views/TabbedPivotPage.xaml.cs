using System;

using HyprWinUI3.ViewModels;

using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Views
{
    public sealed partial class TabbedPivotPage : Page
    {
        public TabbedPivotViewModel ViewModel { get; } = new TabbedPivotViewModel();

        public TabbedPivotPage()
        {
            InitializeComponent();
        }
    }
}
