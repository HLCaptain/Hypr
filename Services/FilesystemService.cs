using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Factories;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Strategies.ExtentionFiller;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Services {
	/// <summary>
	/// Helps interact with the filesystem.
	/// </summary>
	public static class FilesystemService {
		/// <summary>
		/// Asks user for the new file's name and extention, then creates the file to specific location.
		/// </summary>
		/// <param name="folder">The folder the file is created in.</param>
		public static async Task<Diagram> CreateDiagramHere(StorageFolder folder, IExtentionFiller filler) {
			// ini content of the content dialog
			StackPanel content = new StackPanel() {
				Orientation = Orientation.Horizontal,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch
			};
			TextBox textBox = new TextBox() {
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				Margin = new Thickness(4)
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
					Diagram diagram = DiagramFactory.CreateDiagram((string)fileTypes.SelectedItem);
					diagram.Name = textBox.Text == "" ? diagram.Uid : textBox.Text;
					var file = await folder.CreateFileAsync(diagram.Name + (string)fileTypes.SelectedItem, CreationCollisionOption.FailIfExists);
					// todo use strategy when saving file data
					
					
					await FileIO.WriteTextAsync(file, JsonSerializer.Serialize(diagram));
					InfoService.DisplayInfoBar($"{file.Name} created!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
					diagram.File = file;
					return diagram;
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
		public static async Task<Diagram> CreateDiagram() {
			var folderPicker = new FolderPicker();
			folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
			folderPicker.CommitButtonText = "Save diagram here";
			folderPicker.FileTypeFilter.Add("*");

			var folder = await folderPicker.PickSingleFolderAsync();

			if (folder != null) {
				// todo if in subfolder of the project's rootfolder
				if (ProjectService.IsInProjectSubfolder(folder)) {
					return await CreateDiagramHere(folder, new DiagramExtentionFiller());
				} else {
					InfoService.DisplayError("File is not in project folder");
				}
			}
			return null;
		}
	}
}
