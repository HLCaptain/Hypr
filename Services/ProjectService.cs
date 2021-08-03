using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
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

		public static event Action<Diagram> OpenDiagramEvent;

		private static Project _currentProject;
		private static StorageFolder _rootFolder;
		private static StorageFile _projectFile;

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

		public static StorageFile ProjectFile {
			get => _projectFile;
			set {
				if (value != null) {
					_projectFile = value;
				}
			}
		}

		public static void AddDiagramFileToProject(Diagram diagram) {
			if (CurrentProject == null || diagram == null) {
				return;
			}
			AddFileToProjectList(diagram.File, CurrentProject.Diagrams);
		}
		public static void AddElementFileToProject(Element element) {
			if (CurrentProject == null) {
				return;
			}
			AddFileToProjectList(element.File, CurrentProject.Elements);
		}
		/// <summary>
		/// Adds a reference path to a project's list.
		/// </summary>
		/// <param name="file">We add the relative path of this file. Has to be in the same folder or in subfolders as the Project.</param>
		/// <param name="list">The list related to the project to add the relative path to.</param>
		private static void AddFileToProjectList(StorageFile file, IList<string> list) {
			try {
				if (!list.Contains(file.Path.Substring(RootFolder.Path.Length + 1)) && // is the file name already in the list?
					IsInProjectSubfolder(file)) {
					list.Add(file.Path.Substring(RootFolder.Path.Length + 1));
					SaveProject();
				} else {
					InfoService.DisplayError($"Cannot add {file.Name} to {ProjectFile.Name}, because {file.Name} is not in the same folder as {ProjectFile.Name}!");
				}
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
		}
		public static bool IsInProjectSubfolder(IStorageItem file) {
			//return file.Path.StartsWith(RootFolder.Path) && file.Path.Substring(file.Path.Length - file.Name.Length).Length > RootFolder.Path.Length;
			return file.Path.StartsWith(RootFolder.Path);
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
					InfoService.DisplayInfoBar("File " + file.Name + " was opened.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					// Updating project attributes.
					ProjectFile = file;
					RootFolder = await file.GetParentAsync();
				} else {
					InfoService.DisplayError("File " + file.Name + " couldn't be opened.");
				}
			} else {
				InfoService.DisplayError("Operation cancelled.");
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
					InfoService.DisplayInfoBar("File " + file.Name + " was saved.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					// if everything is alright, then we can set the root folder to be the folder the project is in
					RootFolder = await file.GetParentAsync();
					ProjectFile = file;
				} else {
					InfoService.DisplayInfoBar("File " + file.Name + " couldn't be saved.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
				}
			} else {
				InfoService.DisplayError("Operation cancelled.");
			}
		}
		public static async void SaveProject() {
			if (ProjectFile == null) {
				return;
			}
			// Saving the file

			// Prevent updates to the remote version of the file until
			// we finish making changes and call CompleteUpdatesAsync.
			CachedFileManager.DeferUpdates(ProjectFile);
			// write to file
			await FileIO.WriteTextAsync(ProjectFile, JsonSerializer.Serialize(CurrentProject));

			// Saving ends
			FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(ProjectFile);
			if (status == FileUpdateStatus.Complete) {
				InfoService.DisplayInfoBar(ProjectFile.Name + " saved.",
					Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
			} else {
				InfoService.DisplayError(ProjectFile.Name + " couldn't be saved.");
			}
		}

		public static void OpenDiagram(Diagram diagram) {
			OpenDiagramEvent?.Invoke(diagram);
		}
	}
}
