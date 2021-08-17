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

namespace HyprWinUI3.Services {
	/// <summary>
	/// Helps interact with the filesystem.
	/// </summary>
	public static class FilesystemService {
		/// <summary>
		/// Fires when a new Editor is created. arg1 is the new Editor.
		/// </summary>
		public static event Action<EditorApp> EditorCreated;

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
					await FileIO.WriteTextAsync(file, JsonSerializer.Serialize(editor.Model));
					InfoService.DisplayInfoBar($"{file.Name} created!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					EditorCreated?.Invoke(editor);
					editor.Model.File = file;
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
				if (ProjectService.IsInProjectSubfolder(folder)) {
					return await CreateActor(folder, new EditorExtentionFiller());
				} else {
					InfoService.DisplayError("File is not in project folder");
				}
			}
			return null;
		}

		public static async Task<IStorageItem> RenameItem(IStorageItem storageItem, string newName) {
			string oldName = storageItem.Name;
			await storageItem.RenameAsync(newName);
			ItemRenamed?.Invoke(oldName, storageItem);
			return storageItem;
		}

		public static async Task<IStorageItem> RenameItem(IStorageItem storageItem) {
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
			dialog.Title = $"Rename {storageItem.Name}";
			dialog.CloseButtonText = "Cancel";
			dialog.PrimaryButtonText = "Rename item";
			dialog.Content = content;

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary) {
				try {
					string newName = textBox.Text == "" ? Guid.NewGuid().ToString() : textBox.Text;
					storageItem = await RenameItem(storageItem, textBox.Text);
					InfoService.DisplayInfoBar($"{storageItem.Name} renamed!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
				} catch (Exception e) {
					InfoService.DisplayError(e.Message);
				}
			} else {
				InfoService.OperationCancelled();
			}
			return storageItem;
		}

		public static async Task DeleteItem(IStorageItem storageItem) {
			// ini content of the content dialog

			// ini contentdialog
			ContentDialog dialog = new ContentDialog();
			dialog.Title = $"Delete {storageItem.Name}";
			dialog.CloseButtonText = "Cancel";
			dialog.PrimaryButtonText = "Delete item";
			dialog.Content = $"Are you sure, you want to delete {storageItem.Name}?";

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary) {
				try {
					await storageItem.DeleteAsync();
					InfoService.DisplayInfoBar($"{storageItem.Name} deleted!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
				} catch (Exception e) {
					InfoService.DisplayError(e.Message);
				}
			} else {
				InfoService.OperationCancelled();
			}
		}
	}
}
