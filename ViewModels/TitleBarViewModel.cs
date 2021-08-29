using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HyprWinUI3.Helpers;
using HyprWinUI3.Services;
using HyprWinUI3.Views;
using Microsoft.Toolkit.Mvvm.Input;
using Windows.UI.Xaml;

namespace HyprWinUI3.ViewModels {
	public class TitleBarViewModel {
		private ICommand _menuViewsTabViewCommand;
		private ICommand _menuFilesSettingsCommand;
		private ICommand _menuFileExitCommand;
		private ICommand _createProjectCommand;
		private ICommand _openProjectCommand;
		private ICommand _saveProjectCommand;

		public ICommand MenuViewsTabViewCommand => _menuViewsTabViewCommand ?? (_menuViewsTabViewCommand = new RelayCommand(OnMenuViewsTabView));
		private void OnMenuViewsTabView() => MenuNavigationHelper.UpdateView(typeof(TabViewPage));

		public ICommand MenuFileSettingsCommand => _menuFilesSettingsCommand ?? (_menuFilesSettingsCommand = new RelayCommand(OnMenuFileSettings));

		private void OnMenuFileSettings() => MenuNavigationHelper.OpenInRightPane(typeof(SettingsPage));

		public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

		private void OnMenuFileExit() {
			Application.Current.Exit();
		}

		public ICommand CreateProjectCommand => _createProjectCommand ?? (_createProjectCommand = new RelayCommand(ProjectService.CreateProject));
		public ICommand OpenProjectCommand => _openProjectCommand ?? (_openProjectCommand = new RelayCommand(ProjectService.OpenProject));

		public ICommand SaveProjectCommand => _saveProjectCommand ?? (_saveProjectCommand = new AsyncRelayCommand(ProjectService.SaveAll));
	}
}
