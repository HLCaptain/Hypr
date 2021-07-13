using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Services {
	public static class FilesystemService {
		public static async void CreateNewFile(StorageFolder folder) {
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
			fileTypes.Items.Add(".hyclass");
			fileTypes.Items.Add(".txt");
			fileTypes.Items.Add(".json");
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
					var file = await folder.CreateFileAsync(textBox.Text + (string)fileTypes.SelectedItem, CreationCollisionOption.FailIfExists);
					// todo use strategy when saving file data
					await FileIO.WriteTextAsync(file, "insert file data here");
					InfoService.DisplayInfoBar("Success", $"{file.Name} created!", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success);
				} catch (Exception e) {
					InfoService.DisplayInfoBar("Error", e.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
				}
			} else {
				InfoService.DisplayInfoBar("Error", "Operation cancelled.", Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
			}
		}
	}
}
