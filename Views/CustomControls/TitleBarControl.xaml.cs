using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
	public sealed partial class TitleBarControl : UserControl {
		public TitleBarViewModel ViewModel = new TitleBarViewModel();
		public TitleBarControl() {
			this.InitializeComponent();
		}
		private void settingsButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
			ViewModel.MenuFileSettingsCommand.Execute(null);
		}
	}
}
