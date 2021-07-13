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
	/// <summary>
	/// Helps manage the Project. One project can be allocated per application.
	/// </summary>
	public static class ProjectService {
		/// <summary>
		/// Fires when the Project file has been changed.
		/// </summary>
		public static event Action ProjectChangedEvent;
		/// <summary>
		/// Fires when the Root folder has been changed.
		/// </summary>
		public static event Action RootFolderChangedEvent;

		private static Project _currentProject;
		private static StorageFolder _rootFolder;

		/// <summary>
		/// Current Project loaded. Cannot be null after given a proper value.
		/// </summary>
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

		/// <summary>
		/// Root Folder loaded. Cannot be null after given a proper value.
		/// </summary>
		public static StorageFolder RootFolder {
			get => _rootFolder;
			set {
				if (value != null) {
					_rootFolder = value;
					RootFolderChangedEvent?.Invoke();
				}
			}
		}

		/// <summary>
		/// Creates a diagram and adds it to the project.
		/// </summary>
		/// <param name="diagram">The diagram to handle.</param>
		public static async void CreateDiagram(Diagram diagram) {
			if (CurrentProject == null) {
				return;
			}
			CurrentProject.Diagrams.Add(diagram);
			// todo save diagram
		}
		/// <summary>
		/// Opens and loads a project by a FileOpenPicker.
		/// </summary>
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
		/// <summary>
		/// Creates and loads a project by a FileSavePicker.
		/// </summary>
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
					// if everything is alright, then we can set the root folder to be the folder the project is in
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
