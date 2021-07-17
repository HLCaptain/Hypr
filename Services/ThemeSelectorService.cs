using System;
using System.Threading.Tasks;

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
		private const string SettingsKey = "AppBackgroundRequestedTheme";

		// todo: use the current there in elements
		/// <summary>
		/// Current theme of the application.
		/// </summary>
		public static ElementTheme Theme { get; set; } = ElementTheme.Default;
		/// <summary>
		/// Does the theme needs to be high contrast?
		/// </summary>
		public static bool IsHighContrast { get; set; } = false;
		/// <summary>
		/// Fires when the Theme or the Contrast has been changed.
		/// </summary>
		public static event Action ThemeChanged;

		public static async Task InitializeAsync()
		{
			Theme = await LoadThemeFromSettingsAsync();
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

			await SetRequestedThemeAsync();
			await SaveThemeInSettingsAsync(Theme);
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

		private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
		{
			ElementTheme cacheTheme = ElementTheme.Default;
			string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

			if (!string.IsNullOrEmpty(themeName))
			{
				Enum.TryParse(themeName, out cacheTheme);
			}

			return cacheTheme;
		}

		private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
		{
			await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
		}
	}
}
