using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Models.Diagrams;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Services {
	public static class ProjectService {
		public delegate void SomethingChangedDelegate();
		public static event SomethingChangedDelegate ProjectChangedEvent;
		public static event SomethingChangedDelegate RootFolderChangedEvent;

		private static Project _currentProject;
		private static StorageFolder _rootFolder;

		public static Project CurrentProject {
			get => _currentProject;
			// todo: rework set not to directly set project, but indirectly with methods
			set {
				if (value != null) {
					_currentProject = value;
					ProjectChangedEvent?.Invoke();
				}
			}
		}

		public static StorageFolder RootFolder {
			get => _rootFolder;
			set {
				if (value != null) {
					_rootFolder = value;
					RootFolderChangedEvent?.Invoke();
				}
			}
		}

		public static async void CreateDiagram(Diagram diagram) {
			if (CurrentProject == null) {
				return;
			}
			CurrentProject.Diagrams.Add(diagram);
			// todo save diagram
		}
		public static async void OpenProject() {
			var openPicker = new FileOpenPicker();
			openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			openPicker.FileTypeFilter.Add(".hyproj");

			StorageFile file = await openPicker.PickSingleFileAsync();
			if (file != null) {
				// Prevent updates to the remote version of the file until
				// we finish making changes and call CompleteUpdatesAsync.
				CachedFileManager.DeferUpdates(file);
				string projectData = await FileIO.ReadTextAsync(file);
				CurrentProject = JsonSerializer.Deserialize<Project>(projectData);
				// Let Windows know that we're finished changing the file so
				// the other app can update the remote version of the file.
				// Completing updates may require Windows to ask for user input.
				FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
				if (status == FileUpdateStatus.Complete) {
					InfoService.DisplayInfoBar("Success",
						"File " + file.Name + " was opened.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					RootFolder = await file.GetParentAsync();
				} else {
					InfoService.DisplayInfoBar("Failure",
						"File " + file.Name + " couldn't be opened." +
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
				}
			} else {
				InfoService.DisplayInfoBar("Error",
						"Operation cancelled.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
			}
		}
		public static async void CreateProject() {
			var savePicker = new FileSavePicker();
			savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			// Dropdown of file types the user can save the file as
			savePicker.FileTypeChoices.Add("Hypr project file", new List<string>() { ".hyproj" });
			// Default file name if the user does not type one in or select a file to replace
			savePicker.SuggestedFileName = "New Project";

			StorageFile file = await savePicker.PickSaveFileAsync();
			if (file != null) {
				CurrentProject = new Project() { Name = file.DisplayName };
				// Saving the file

				// Prevent updates to the remote version of the file until
				// we finish making changes and call CompleteUpdatesAsync.
				CachedFileManager.DeferUpdates(file);
				// write to file
				await FileIO.WriteTextAsync(file, JsonSerializer.Serialize(CurrentProject));
				// Let Windows know that we're finished changing the file so
				// the other app can update the remote version of the file.
				// Completing updates may require Windows to ask for user input.

				// Saving ends
				FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
				if (status == FileUpdateStatus.Complete) {
					InfoService.DisplayInfoBar("Success",
						"File " + file.Name + " was saved.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					RootFolder = await file.GetParentAsync();
				} else {
					InfoService.DisplayInfoBar("Success",
						"File " + file.Name + " couldn't be saved.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
				}
			} else {
				InfoService.DisplayInfoBar("Error",
						"Operation cancelled.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
			}
		}
	}
}
