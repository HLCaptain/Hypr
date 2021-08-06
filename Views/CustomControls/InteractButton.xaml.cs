using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using HyprWinUI3.Factories;
using Microsoft.Toolkit.Mvvm.Input;
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
	public sealed partial class InteractButton : UserControl {
		private ICommand _command;
		private ButtonBase _button;
		public ICommand Command {
			get => _command;
			set {
				if (value != null) {
					_command = value;
					Button.Command = value;
					Button.IsEnabled = true;
				}
			}
		}
		public ButtonBase Button {
			get => _button;
			set {
				if (value != null) {
					_button = value;
					if (Command != null) {
						_button.IsEnabled = true;
						_button.Command = Command;
					} else {
						_button.IsEnabled = false;
					}

					// todo set button to grid with proper properties
					grid.Children.Clear();
					grid.Children.Add(_button);
				}
			}
		}
		public InteractButton() {
			this.InitializeComponent();
			Button = InteractButtonFactory.MakeInteractToggleButton(null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="command"></param>
		/// <param name="button">If null, button will be InteractButtonFactory.MakeInteractToggleButton(null);</param>
		public InteractButton(RelayCommand command, ButtonBase button) {
			this.InitializeComponent();
			Button = button ?? InteractButtonFactory.MakeInteractToggleButton(null);
			Button.Command = command;
		}
	}
}
