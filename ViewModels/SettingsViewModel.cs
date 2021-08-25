using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using HyprWinUI3.Constants;
using HyprWinUI3.Helpers;
using HyprWinUI3.Services;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace HyprWinUI3.ViewModels
{
	// TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/pages/settings.md
	public class SettingsViewModel : ObservableObject {
		private ElementTheme _elementTheme = ThemeSelectorService.Theme;

		public ElementTheme ElementTheme {
			get => _elementTheme;

			set { SetProperty(ref _elementTheme, value); }
		}

		private string _versionDescription;

		public string VersionDescription {
			get => _versionDescription;

			set { SetProperty(ref _versionDescription, value); }
		}

		private ICommand _switchThemeCommand;

		public ICommand SwitchThemeCommand {
			get {
				if (_switchThemeCommand == null) {
					_switchThemeCommand = new RelayCommand<ElementTheme>(
						async (param) => {
							ElementTheme = param;
							await ThemeSelectorService.SetThemeAsync(param);
						});
				}

				return _switchThemeCommand;
			}
		}

		public ObservableCollection<ElementSize> Sizes { get; set; } = new ObservableCollection<ElementSize>() { ElementSize.Small, ElementSize.Normal, ElementSize.Big, ElementSize.Huge };

		private bool _isHighContrast;

		public bool IsHighContrast {
			get => _isHighContrast;
			set { SetProperty(ref _isHighContrast, value); }
		}

		private ElementSize _size;
		public ElementSize Size {
			get => _size;
			set { SetProperty(ref _size, value); }
		}

		private ICommand _switchSizeCommand;
		public ICommand SwitchSizeCommand {
			get {
				if (_switchSizeCommand == null) {
					_switchSizeCommand = new RelayCommand<ElementSize>(
						async (param) => {
							Size = param;
							await ThemeSelectorService.SetSizeAsync(param);
						});
				}
				return _switchSizeCommand;
			}
		}


		private ICommand _switchContrastCommand;
		public ICommand SwitchContrastCommand {
			get {
				if (_switchContrastCommand == null) {
					_switchContrastCommand = new RelayCommand<bool>(
						async (param) => {
							IsHighContrast = param;
							await ThemeSelectorService.SetContrastAsync(param);
						});
				}

				return _switchContrastCommand;
			}
		}

		public SettingsViewModel() { }

		public async Task InitializeAsync() {
			VersionDescription = GetVersionDescription();
			await Task.CompletedTask;
		}

		private string GetVersionDescription() {
			var appName = "AppDisplayName".GetLocalized();
			var package = Package.Current;
			var packageId = package.Id;
			var version = packageId.Version;

			return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
		}
	}
}
