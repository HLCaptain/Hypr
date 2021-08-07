using System;
using HyprWinUI3.Constants;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HyprWinUI3.Views
{
	// TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
	public sealed partial class SettingsPage : Page {
		public SettingsViewModel ViewModel { get; } = new SettingsViewModel();

		public SettingsPage() {
			InitializeComponent();
			sizeComboBox.SelectedItem = ThemeSelectorService.Size;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e) {
			await ViewModel.InitializeAsync();
		}

		private void sizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			ThemeSelectorService.SetSizeAsync((ElementSize)e.AddedItems[0]);
		}
	}
}
