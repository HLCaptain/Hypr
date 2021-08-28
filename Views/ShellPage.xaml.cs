using System;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;
using HyprWinUI3.Views.CustomControls;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HyprWinUI3.Views {
	// TODO WTS: You can edit the text for the menu in String/en-US/Resources.resw
	// You can show pages in different ways (update main view, navigate, right pane, new windows or dialog) using MenuNavigationHelper class.
	// Read more about MenuBar project type here:
	// https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/projectTypes/menubar.md
	public sealed partial class ShellPage : Page {
		public ShellViewModel ViewModel { get; } = new ShellViewModel();
		public Grid FrameGrid { get => frameGrid; }

		public ShellPage() {
			InitializeComponent();
			ViewModel.Initialize(shellFrame, splitView, rightFrame, KeyboardAccelerators);
			ViewModel.InitializeSaveStates(stateText, xIcon, tickIcon, progressRing);

			// Hide default title bar.
			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
			Window.Current.SetTitleBar(titleBarBackground);
			CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += TitleBar_LayoutMetricsChanged;
			ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Color.FromArgb(0, 255, 255, 255);
		}

		private void TitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) {
			titleBarBackground.Height = sender.Height;
			titleBarControl.Height = sender.Height;
		}
	}
}
