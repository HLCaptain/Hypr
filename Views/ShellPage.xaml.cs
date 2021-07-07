using System;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;

using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Views
{
	// TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
	// You can show pages in different ways (update main view, navigate, right pane, new windows or dialog) using MenuNavigationHelper class.
	// Read more about MenuBar project type here:
	// https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/projectTypes/menubar.md
	public sealed partial class ShellPage : Page {
        public ShellViewModel ViewModel { get; } = new ShellViewModel();
        public Grid FrameGrid { get => frameGrid; }

        public ShellPage()
		{
			InitializeComponent();
			ViewModel.Initialize(shellFrame, splitView, rightFrame, KeyboardAccelerators);
		}

        private void settingsButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            ViewModel.MenuFileSettingsCommand.Execute(null);
        }
    }
}
