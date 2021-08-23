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
using HyprWinUI3.Core.Helpers;
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
					var editor = EditorAppFactory.CreateEditor((string)fileTypes.SelectedItem);
					editor.Model.Name = textBox.Text == "" ? editor.Model.Uid : textBox.Text;
					var file = await folder.CreateFileAsync(editor.Model.Name + (string)fileTypes.SelectedItem, CreationCollisionOption.FailIfExists);
					await SaveJsonFile(file, editor.Model);
					InfoService.DisplayInfoBar($"{file.Name} created!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					AddRelativePathToList(ProjectService.RootFolder, file, ProjectService.CurrentProject.Diagrams);
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
				var nameParts = item.Name.Split('.').ToList<string>();
				string extention = nameParts[nameParts.Count - 1];
				await item.RenameAsync(newName + "." + extention);
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
			foreach (var item in Constants.Extentions.ExtentionActorTypes.Keys) {
				if (Constants.Extentions.ExtentionActorTypes[item] == type) {
					extention = item;
					break;
				}
			}
			return extention;
		}

		public static async Task<StorageFile> CreateActorFile(Actor actor) {
			string extention = GetActorExtention(actor.GetType());
			try {
				var file = await ProjectService.RootFolder.CreateFileAsync(
					actor.Name +
					" - " +
					actor.Uid +
					extention ?? ".txt");
				actor.File = file;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
			return actor.File;
		}
		public static async Task<StorageFile> CreateElementFile(Element element, IList<string> elements) {
			await CreateActorFile(element);
			AddRelativePathToList(ProjectService.RootFolder, element.File, elements);
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

		private static async Task<FileUpdateStatus> SaveFile(IStorageFile file, string data) {
			CachedFileManager.DeferUpdates(file);
			await FileIO.WriteTextAsync(file, data);
			return await CachedFileManager.CompleteUpdatesAsync(file);
		}

		private static async Task<FileUpdateStatus> SaveJsonFile(IStorageFile file, object data) {
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
				if (!list.Contains(RelativePathFromFolder(root, file))) {
					if (file.Path.StartsWith(root.Path)) {
						list.Add(RelativePathFromFolder(root, file));
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

		public static string RelativePathFromFolder(StorageFolder root, IStorageItem item) {
			return Path.GetRelativePath(root.Path, item.Path);
		}
	}
}
