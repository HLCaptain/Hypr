using System;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Views
{
	// For more info about the TabView Control see
	// https://docs.microsoft.com/uwp/api/microsoft.ui.xaml.controls.tabview?view=winui-2.2
	// For other samples, get the XAML Controls Gallery app http://aka.ms/XamlControlsGallery
	public sealed partial class TabViewPage : Page {
		public TabViewViewModel ViewModel { get; } = new TabViewViewModel();
        public Grid Grid { get => grid; }
        public VariableSizedWrapGrid InfoBarGrid { get => infoBarGrid; }

        public TabViewPage() {
			InitializeComponent();
            InfoService.InfoBarGrid = InfoBarGrid;
        }
	}
}
