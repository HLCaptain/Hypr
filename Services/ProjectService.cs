using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Models.Data;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Proxy;
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

		public static event Action<EditorApp> OpenEditorEvent;
		public static event Action<StorageFile> OpenEditorFileEvent;

		public static event Action<string> SavingStarted;
		public static event Action<string> SavingEnded;

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
					DocumentProxies.Clear();
					foreach (var documentPath in value.Documents) {
						DocumentProxies.Add(new DocumentProxy(documentPath));
					}
				}
			}
		}

		public static ObservableCollection<DocumentProxy> DocumentProxies { get; set; } = new ObservableCollection<DocumentProxy>();

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
				var project = JsonSerializer.Deserialize<Project>(projectData);
				// Let Windows know that we're finished changing the file so
				// the other app can update the remote version of the file.
				// Completing updates may require Windows to ask for user input.
				FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
				if (status == FileUpdateStatus.Complete) {
					InfoService.DisplayInfoBar("File " + file.Name + " was opened.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					// Updating project attributes.
					RootFolder = await file.GetParentAsync();
					project.File = file;
					CurrentProject = project;
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
				var project = new Project() { Name = file.DisplayName };
				var status = await FilesystemService.SaveJsonFile(file, project);
				if (status == FileUpdateStatus.Complete) {
					InfoService.DisplayInfoBar("File " + file.Name + " was saved.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					// if everything is alright, then we can set the root folder to be the folder the project is in
					RootFolder = await file.GetParentAsync();
					project.File = file;
					CurrentProject = project;
				} else {
					InfoService.DisplayInfoBar("File " + file.Name + " couldn't be saved.",
						Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
				}
			} else {
				InfoService.DisplayError("Operation cancelled.");
			}
		}
		public static async Task SaveProjectFile() {
			if (CurrentProject?.File == null) {
				return;
			}
			var status = await FilesystemService.SaveJsonFile(CurrentProject?.File, CurrentProject);
			if (status == FileUpdateStatus.Complete) {
				InfoService.DisplayInfoBar(CurrentProject.File.Name + " saved.",
					Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
			} else {
				InfoService.DisplayError(CurrentProject.File.Name + " couldn't be saved.");
			}
		}

		public static void OpenEditor(EditorApp editor) {
			OpenEditorEvent?.Invoke(editor);
		}

		public static void OpenEditor(StorageFile file) {
			OpenEditorFileEvent?.Invoke(file);
		}

		private static async Task SaveFiles() {
			SavingStarted?.Invoke("Saving files!");
			foreach (var document in DocumentProxies) {
				await document.SaveDocument();
			}
			SavingEnded?.Invoke("Files saved!");
		}

		public static async Task FilePathChanged(string oldPath, string newPath) {
			foreach (var proxies in DocumentProxies) {
				await proxies.ChangeReference(oldPath, newPath);
			}
			if (CurrentProject.Documents.Contains(oldPath)) {
				CurrentProject.Documents[CurrentProject.Documents.IndexOf(oldPath)] = newPath;
			}
		}
	}
}
