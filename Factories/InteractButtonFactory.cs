using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace HyprWinUI3.Factories {
	public enum InteractButtonSize {
		Small, Normal, Big, Huge
	}
	public static class InteractButtonFactory {
		private static void FormatInteractButton(ButtonBase button, InteractButtonSize size = InteractButtonSize.Normal) {
			var icon = (FontIcon)button.Content;
			switch(size) {
				case InteractButtonSize.Small: {
						button.Width = 24;
						button.Height = 24;
						button.Margin = new Windows.UI.Xaml.Thickness(2);
						button.CornerRadius = new Windows.UI.Xaml.CornerRadius(2);
						icon.Width = 16;
						icon.Height = 16;
						icon.Margin = new Windows.UI.Xaml.Thickness(-4);
						break;
					}
				case InteractButtonSize.Normal: {
						button.Width = 36;
						button.Height = 36;
						button.Margin = new Windows.UI.Xaml.Thickness(2);
						button.CornerRadius = new Windows.UI.Xaml.CornerRadius(2);
						icon.Width = 24;
						icon.Height = 24;
						icon.Margin = new Windows.UI.Xaml.Thickness(-4);
						break;
					}
				case InteractButtonSize.Big: {
						button.Width = 48;
						button.Height = 48;
						button.Margin = new Windows.UI.Xaml.Thickness(4);
						button.CornerRadius = new Windows.UI.Xaml.CornerRadius(4);
						icon.Width = 36;
						icon.Height = 36;
						icon.Margin = new Windows.UI.Xaml.Thickness(-8);
						break;
					}
				case InteractButtonSize.Huge: {
						button.Width = 64;
						button.Height = 64;
						button.Margin = new Windows.UI.Xaml.Thickness(6);
						button.CornerRadius = new Windows.UI.Xaml.CornerRadius(6);
						icon.Width = 48;
						icon.Height = 48;
						icon.Margin = new Windows.UI.Xaml.Thickness(-12);
						break;
					}
				default: {
						break;
					}
			}
			button.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
			button.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
			button.IsEnabled = false;
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
		public static Button MakeInteractButton(FontIcon icon, InteractButtonSize size = InteractButtonSize.Normal) {
			var button = new Button() {
				Content = icon ?? new FontIcon() {
					FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"),
					Glyph = "\xECCD"
				},
			};
			FormatInteractButton(button, size);
			return button;
		}
		public static ToggleButton MakeInteractToggleButton(FontIcon icon, InteractButtonSize size = InteractButtonSize.Normal) {
			var toggleButton = new ToggleButton() {
				Content = icon ?? new FontIcon() {
					FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe Fluent Icons"),
					Glyph = "\xECCD"
				},
			};
			FormatInteractButton(toggleButton, size);
			return toggleButton;
		}
	}
}
