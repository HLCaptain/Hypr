using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HyprWinUI3.Constants;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace HyprWinUI3.Factories {
	public static class InteractButtonFactory {
		public static void FormatInteractButton(InteractButton interactButton) {
			var icon = (FontIcon)interactButton.Button.Content;
			// todo make size a global variable to autoformat things with observer pattern
			switch(ThemeSelectorService.Size) {
				case ElementSize.Small: {
						interactButton.Width = 24;
						interactButton.Height = 24;
						interactButton.CornerRadius = new Windows.UI.Xaml.CornerRadius(2);
						icon.Width = 24;
						icon.Height = 24;
						break;
					}
				case ElementSize.Normal: {
						interactButton.Width = 36;
						interactButton.Height = 36;
						interactButton.CornerRadius = new Windows.UI.Xaml.CornerRadius(2);
						icon.Width = 36;
						icon.Height = 36;
						break;
					}
				case ElementSize.Big: {
						interactButton.Width = 48;
						interactButton.Height = 48;
						interactButton.CornerRadius = new Windows.UI.Xaml.CornerRadius(4);
						icon.Width = 48;
						icon.Height = 48;
						break;
					}
				case ElementSize.Huge: {
						interactButton.Width = 64;
						interactButton.Height = 64;
						interactButton.CornerRadius = new Windows.UI.Xaml.CornerRadius(6);
						icon.Width = 64;
						icon.Height = 64;
						break;
					}
				default: {
						break;
					}
			}
			icon.FontSize = icon.Width / 2;
			icon.Margin = new Windows.UI.Xaml.Thickness(-4);
			interactButton.Margin = new Windows.UI.Xaml.Thickness(2);
			interactButton.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
			interactButton.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
			icon.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
			icon.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;

			// Normal button size template
			/*
			<ToggleButton
					Width = "36"
					Height = "36"
					Margin = "2,2,2,2"
					HorizontalAlignment = "Center"
					VerticalAlignment = "Center"
					CornerRadius = "4,4,4,4"
					IsEnabled = "False" >
						< FontIcon
							Width = "24"
							Height = "24"
							Margin = "-4,-4,-4,-4"
							HorizontalAlignment = "Center"
							VerticalAlignment = "Center"
							FontFamily = "Segoe Fluent Icons"
							Glyph = "&#xECCD;" />
				</ ToggleButton >
				*/
		}
		public static InteractButton MakeInteractButton(FontIcon icon, ButtonBase button, ICommand command) {
			button.Content = icon ?? new FontIcon() {
				FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"),
				Glyph = "\xECCD"
			};
			var interactButton = new InteractButton() {
				Button = button,
				Command = command
			};
			FormatInteractButton(interactButton);
			return interactButton;
		}
	}
}
