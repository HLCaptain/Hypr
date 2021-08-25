using System;
using System.Threading.Tasks;
using HyprWinUI3.Constants;
using HyprWinUI3.Helpers;

using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace HyprWinUI3.Services {
	/// <summary>
	/// Helps handling the current theme of the app.
	/// </summary>
	public static class ThemeSelectorService {
		private const string SettingsKeyTheme = "AppBackgroundRequestedTheme";
		private const string SettingsKeyContrast = "AppBackgroundRequestedContrast";
		private const string SettingsKeySize = "AppBackgroundRequestedSize";

		// todo: use the current there in elements
		/// <summary>
		/// Current theme of the application.
		/// </summary>
		public static ElementTheme Theme { get; set; } = ElementTheme.Default;
		public static ElementSize Size { get; set; } = ElementSize.Normal;
		/// <summary>
		/// Does the theme needs to be high contrast?
		/// </summary>
		public static bool IsHighContrast { get; set; } = false;
		/// <summary>
		/// Fires when the Theme or the Contrast has been changed.
		/// </summary>
		public static event Action ThemeChanged;
		public static event Action SizeChanged;
		public static event Action ContrastChanged;

		public static async Task InitializeAsync() {
			Theme = await LoadThemeFromSettingsAsync();
			IsHighContrast = await LoadContrastFromSettingsAsync();
			Size = await LoadSizeFromSettingsAsync();
		}

		/// <summary>
		/// Sets a particular theme.
		/// </summary>
		/// <param name="theme">New theme.</param>
		/// <returns></returns>
		public static async Task SetThemeAsync(ElementTheme theme) {
			Theme = theme;

			await SetRequestedThemeAsync();
			await SaveThemeInSettingsAsync(Theme);
		}
		/// <summary>
		/// Sets the contrast of the current theme.
		/// </summary>
		/// <param name="isHighContrast">New contrast value.</param>
		/// <returns></returns>
		public static async Task SetContrastAsync(bool isHighContrast) {
			IsHighContrast = isHighContrast;

			ContrastChanged?.Invoke();
			await SaveContrastInSettingsAsync(isHighContrast);
		}

		public static async Task SetRequestedThemeAsync() {
			if (ThemeChanged != null) {
				ThemeChanged();
			}
			foreach (var view in CoreApplication.Views) {
				await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
					if (Window.Current.Content is FrameworkElement frameworkElement) {
						frameworkElement.RequestedTheme = Theme;
					}
				});
			}
		}
		public static async Task SetSizeAsync(ElementSize size) {
			Size = size;

			SizeChanged?.Invoke();
			await SaveSizeInSettingsAsync(size);
		}

		private static async Task<ElementTheme> LoadThemeFromSettingsAsync() {
			ElementTheme cacheTheme = ElementTheme.Default;
			string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKeyTheme);

			if (!string.IsNullOrEmpty(themeName)) {
				Enum.TryParse(themeName, out cacheTheme);
			}

			return cacheTheme;
		}

		private static async Task<bool> LoadContrastFromSettingsAsync() {
			bool isHighContrast = false;
			string contrastValue = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKeyContrast);

			if (!string.IsNullOrEmpty(contrastValue)) {
				bool.TryParse(contrastValue, out isHighContrast);
			}

			return isHighContrast;
		}

		private static async Task<ElementSize> LoadSizeFromSettingsAsync() {
			ElementSize size = ElementSize.Normal;
			string sizeValue = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKeySize);

			if (!string.IsNullOrEmpty(sizeValue)) {
				Enum.TryParse(sizeValue, out size);
			}

			return size;
		}

		private static async Task SaveThemeInSettingsAsync(ElementTheme theme) {
			await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKeyTheme, theme.ToString());
		}
		private static async Task SaveContrastInSettingsAsync(bool isHighContrast) {
			await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKeyContrast, isHighContrast.ToString());
		}
		private static async Task SaveSizeInSettingsAsync(ElementSize size) {
			await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKeySize, size.ToString());
		}
	}
}
