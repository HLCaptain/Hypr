using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using HyprWinUI3.Helpers;
using HyprWinUI3.Services;
using HyprWinUI3.Views;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace HyprWinUI3.ViewModels
{
	public class ShellViewModel : ObservableObject
	{
		private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
		private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);
		private IList<KeyboardAccelerator> _keyboardAccelerators;

		private ICommand _loadedCommand;

		public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

		public ShellViewModel() {
		}

		public void Initialize(Frame shellFrame, SplitView splitView, Frame rightFrame, IList<KeyboardAccelerator> keyboardAccelerators) {
			NavigationService.Frame = shellFrame;
			MenuNavigationHelper.Initialize(splitView, rightFrame);
			_keyboardAccelerators = keyboardAccelerators;

			// hook statePanel onto projecthandling
		}

		public void InitializeSaveStates(TextBlock stateText, FontIcon xIcon, FontIcon tickIcon, Microsoft.UI.Xaml.Controls.ProgressRing progressRing) {
			stateText.Text = "No project loaded";
			xIcon.Visibility = Visibility.Visible;
			tickIcon.Visibility = Visibility.Collapsed;
			progressRing.Visibility = Visibility.Collapsed;

			ProjectService.ProjectChangedEvent += () => {
				if (ProjectService.CurrentProject.File == null) {
					stateText.Text = "No file loaded to project!";
				} else {
					stateText.Text = ProjectService.CurrentProject.File.Name;
				}
				xIcon.Visibility = Visibility.Collapsed;
				tickIcon.Visibility = Visibility.Visible;
				progressRing.Visibility = Visibility.Collapsed;
			};
			ProjectService.SavingStarted += (message) => {
				xIcon.Visibility = Visibility.Collapsed;
				tickIcon.Visibility = Visibility.Collapsed;
				progressRing.Visibility = Visibility.Visible;
				stateText.Text = message;
			};
			ProjectService.SavingEnded += (message) => {
				xIcon.Visibility = Visibility.Collapsed;
				tickIcon.Visibility = Visibility.Visible;
				progressRing.Visibility = Visibility.Collapsed;
				stateText.Text = message;
			};
		}

		private void OnLoaded() {
			// Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
			// More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
			_keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
			_keyboardAccelerators.Add(_backKeyboardAccelerator);
		}

		private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null) {
			var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
			if (modifiers.HasValue) {
				keyboardAccelerator.Modifiers = modifiers.Value;
			}

			keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
			return keyboardAccelerator;
		}

		private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args) {
			var result = NavigationService.GoBack();
			args.Handled = result;
		}
	}
}
