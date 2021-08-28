using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Strategies.ExtentionFiller;
using HyprWinUI3.Models.Data;
using HyprWinUI3.Models.Diagrams;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.EditorApps;
using Windows.Storage.Provider;
using Newtonsoft.Json;
using System.IO;

namespace HyprWinUI3.Services {
	/// <summary>
	/// Helps interact with the filesystem.
	/// </summary>
	public static class FilesystemService {
		/// <summary>
		/// Fires when a new Editor is created. arg1 is the new Editor.
		/// </summary>
		public static event Action<EditorApp> EditorCreated;
		public static event Action<Element> ElementCreated;
		public static event Action<IStorageItem> ItemChanged;

		/// <summary>
		/// Event when fires when a StorageItem is renamed. arg1 is the old file's name, arg2 is the new file.
		/// </summary>
		public static event Action<string, IStorageItem> ItemRenamed;

		/// <summary>
		/// Asks user for the new file's name and extention, then creates the file to specific location.
		/// </summary>
		/// <param name="folder">The folder the file is created in.</param>
		public static async Task<EditorApp> CreateActor(StorageFolder folder, IExtentionFiller filler) {
			// ini content of the content dialog
			StackPanel content = new StackPanel() {
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch
			};
			TextBox textBox = new TextBox() {
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				Margin = new Thickness(4),
				PlaceholderText = "File name"
			};
			content.Children.Add(textBox);
			ComboBox fileTypes = new ComboBox() {
				HorizontalAlignment = HorizontalAlignment.Right,
				VerticalAlignment = VerticalAlignment.Stretch,
				Margin = new Thickness(4)
			};

			// todo use strategy pattern when choosing file type to create an appropriate file
			filler.FillWithExtentions(fileTypes.Items);
			fileTypes.SelectedIndex = 0;
			content.Children.Add(fileTypes);

			// ini contentdialog
			// todo use a custom content dialog (kind of like a usercontrol) instead
			ContentDialog dialog = new ContentDialog();
			dialog.Title = "Create new file";
			dialog.CloseButtonText = "Cancel";
			dialog.PrimaryButtonText = "Create file";
			dialog.Content = content;

			var result = await dialog.ShowAsync();

			// saving file
			if (result == ContentDialogResult.Primary) {
				try {
					var actor = (Actor)Activator.CreateInstance(GetActorTypeFromExtention((string)fileTypes.SelectedItem));
					actor.Name = textBox.Text == "" ? actor.Uid : textBox.Text;
					var file = await folder.CreateFileAsync(actor.Name + (string)fileTypes.SelectedItem, CreationCollisionOption.FailIfExists);
					AddRelativePathToList(ProjectService.RootFolder, file, ProjectService.CurrentProject.Documents);
					await SaveJsonFile(file, actor);
					var editor = await EditorAppFactory.CreateEditorFromFile(file);
					InfoService.DisplayInfoBar($"{file.Name} created!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					editor.Model.File = file;
					ItemChanged?.Invoke(editor.Model.File);
					EditorCreated?.Invoke(editor);
					return editor;
				} catch (Exception e) {
					InfoService.DisplayError(e.Message);
				}
			} else {
				InfoService.DisplayError("Operation cancelled.");
			}
			return null;
		}

		// todo make this method with strategy pattern in mind.
		/// <summary>
		/// Can create any given files.
		/// </summary>
		public static async Task<EditorApp> CreateActor() {
			var folderPicker = new FolderPicker();
			folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			folderPicker.CommitButtonText = "Save file here";
			folderPicker.FileTypeFilter.Add("*");

			var folder = await folderPicker.PickSingleFolderAsync();

			if (folder != null) {
				// todo if in subfolder of the project's rootfolder
				if (folder.Path.StartsWith(ProjectService.RootFolder.Path)) {
					return await CreateActor(folder, new EditorExtentionFiller());
				} else {
					InfoService.DisplayError("File is not in project folder");
				}
			}
			return null;
		}

		public static async Task<IStorageItem> RenameItem(IStorageItem item, string newName) {
			try {
				string oldName = item.Name;
				string oldPath = Path.GetRelativePath(ProjectService.RootFolder.Path, item.Path);
				var nameParts = item.Name.Split('.').ToList();
				string extention = nameParts[nameParts.Count - 1];
				await item.RenameAsync(newName + "." + extention);
				await ProjectService.FilePathChanged(oldPath, Path.GetRelativePath(ProjectService.RootFolder.Path, item.Path));
				ItemRenamed?.Invoke(oldName, item);
				ItemChanged?.Invoke(item);
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
			return item;
		}

		public static async Task<IStorageItem> RenameItem(IStorageItem item) {
			// ini content of the content dialog
			StackPanel content = new StackPanel() {
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch
			};
			TextBox textBox = new TextBox() {
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				Margin = new Thickness(4),
				PlaceholderText = "New name"
			};
			content.Children.Add(textBox);

			// ini contentdialog
			ContentDialog dialog = new ContentDialog();
			dialog.Title = $"Rename {item.Name}";
			dialog.CloseButtonText = "Cancel";
			dialog.PrimaryButtonText = "Rename item";
			dialog.Content = content;

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary) {
				try {
					string newName = textBox.Text == "" ? Guid.NewGuid().ToString() : textBox.Text;
					item = await RenameItem(item, textBox.Text);
					InfoService.DisplayInfoBar($"{item.Name} renamed!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
				} catch (Exception e) {
					InfoService.DisplayError(e.Message);
				}
			} else {
				InfoService.OperationCancelled();
			}
			return item;
		}

		public static async Task DeleteItem(IStorageItem item) {
			// ini content of the content dialog

			// ini contentdialog
			ContentDialog dialog = new ContentDialog();
			dialog.Title = $"Delete {item.Name}";
			dialog.CloseButtonText = "Cancel";
			dialog.PrimaryButtonText = "Delete item";
			dialog.Content = $"Are you sure, you want to delete {item.Name}?";

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary) {
				try {
					await item.DeleteAsync();
					InfoService.DisplayInfoBar($"{item.Name} deleted!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
				} catch (Exception e) {
					InfoService.DisplayError(e.Message);
				}
			} else {
				InfoService.OperationCancelled();
			}
		}

		private static string GetActorExtention(Type type) {
			// find extention for the file
			string extention = null;
			foreach (var item in Constants.Extentions.ExtentionDiagramTypes.Keys) {
				if (Constants.Extentions.ExtentionDiagramTypes[item] == type) {
					extention = item;
					break;
				}
			}
			if (extention != null) {
				return extention;
			}
			foreach (var item in Constants.Extentions.ExtentionEdgeTypes.Keys) {
				if (Constants.Extentions.ExtentionEdgeTypes[item] == type) {
					extention = item;
					break;
				}
			}
			if (extention != null) {
				return extention;
			}
			foreach (var item in Constants.Extentions.ExtentionVertexTypes.Keys) {
				if (Constants.Extentions.ExtentionVertexTypes[item] == type) {
					extention = item;
					break;
				}
			}
			return extention;
		}

		private static Type GetActorTypeFromExtention(string extention) {
			Type type = null;
			if (Constants.Extentions.ExtentionDiagramTypes.Keys.Contains(extention)) {
				type = Constants.Extentions.ExtentionDiagramTypes[extention];
			}
			if (type != null) {
				return type;
			}
			if (Constants.Extentions.ExtentionEdgeTypes.Keys.Contains(extention)) {
				type = Constants.Extentions.ExtentionEdgeTypes[extention];
			}
			if (type != null) {
				return type;
			}
			if (Constants.Extentions.ExtentionVertexTypes.Keys.Contains(extention)) {
				type = Constants.Extentions.ExtentionVertexTypes[extention];
			}
			return type;
		}

		public static async Task CreateActorFile(Actor actor) {
			string extention = GetActorExtention(actor.GetType());
			try {
				if (actor.File == null) {
					actor.Name = actor.Name + " - " + actor.Uid;
					var file = await ProjectService.RootFolder.CreateFileAsync(
						actor.Name +
						extention ?? ".txt");
					actor.File = file;
					ItemChanged?.Invoke(actor.File);
				}
				await SaveActorFile(actor);
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
		}
		public static async Task<StorageFile> CreateElementFile(Element element, IList<string> elements) {
			element.File = await CreateElementFile(element);
			AddRelativePathToList(ProjectService.RootFolder, element.File, elements);
			return element.File;
		}

		public static async Task<StorageFile> CreateElementFile(Element element) {
			await CreateActorFile(element);
			ElementCreated?.Invoke(element);
			return element.File;
		}

		public static async Task SaveActorFile(Actor actor) {
			// rename actor if needed
			if (actor.File.DisplayName != actor.Name) {
				await RenameItem(actor.File, actor.Name);
			}
			// write new actor data
			await SaveJsonFile(actor.File, actor);
		}

		public static async Task<FileUpdateStatus> SaveFile(IStorageFile file, string data) {
			CachedFileManager.DeferUpdates(file);
			await FileIO.WriteTextAsync(file, data);
			return await CachedFileManager.CompleteUpdatesAsync(file);
		}

		public static async Task<FileUpdateStatus> SaveJsonFile(IStorageFile file, object data) {
			return await SaveFile(file, JsonConvert.SerializeObject(data, Formatting.Indented));
		}

		// todo renaming paths in project file's pathlist when folder is renamed

		/// <summary>
		/// Adds a reference path to a list.
		/// </summary>
		/// <param name="file">We add the relative path of this file. Has to be in the same folder or in subfolders as the Project.</param>
		/// <param name="list">The list related to the project to add the relative path to.</param>
		public static void AddRelativePathToList(StorageFolder root, IStorageItem file, IList<string> list) {
			try {
				if (!list.Contains(Path.GetRelativePath(root.Path, file.Path))) {
					if (file.Path.StartsWith(root.Path)) {
						list.Add(Path.GetRelativePath(root.Path, file.Path));
					} else {
						InfoService.DisplayError($"Cannot add {file.Name} to the list, because {file.Name} is not in the same folder as {root.Name}!");
					}
				} else {
					InfoService.DisplayError($"Cannot add {file.Name} to the list, because {file.Name} is already in it!");
				}
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
		}

		public static async Task<Actor> LoadActor(string path) {
			try {
				var absolutePath = Path.Combine(ProjectService.RootFolder.Path, path);
				var file = await StorageFile.GetFileFromPathAsync(absolutePath);
				return await LoadActor(file);
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
				throw;
			}
		}

		public static async Task<Actor> LoadActor(StorageFile file) {
			var type = GetActorTypeFromExtention(file.FileType);
			var data = await FileIO.ReadTextAsync(file);
			var actor = (Actor)JsonConvert.DeserializeObject(data, type);
			actor.File = file;
			return actor;
		}
	}
}
