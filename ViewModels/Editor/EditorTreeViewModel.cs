using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Helpers;
using HyprWinUI3.Models.Data;
using HyprWinUI3.Services;
using HyprWinUI3.Views.CustomControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.ViewModels.Editor {
	public class EditorTreeViewModel {
		public ObservableCollection<TreeItem> TreeItems { get; set; } = new ObservableCollection<TreeItem>();
		public EditorTreeViewModel() {
			ProjectService.RootFolderChangedEvent += InitializeTreeView;

			InitializeTreeView();
		}

		/// <summary>
		/// Loads in the first Children of nodes onto the TreeView if a root folder is loaded.
		/// </summary>
		private void InitializeTreeView() {
			TreeItems.Clear();
			if (ProjectService.RootFolder == null) {
				TreeItems.Add(new TreeItem {
					Content = "No project"
				});
				TreeItems[0].Children.Add(new TreeItem {
					Content = "Child1",
				});
				TreeItems[0].Children.Add(new TreeItem {
					Content = "Child",
				});
				TreeItems[0].Children.Add(new TreeItem {
					Content = "Child",
				});
				TreeItems[0].Children[0].Children.Add(new TreeItem {
					Content = "Child2",
				});
				TreeItems[0].Children[0].Children.Add(new TreeItem {
					Content = "Child2",
				});
			} else {
				var newRoot = new TreeItem() {
					Content = ProjectService.RootFolder
				};
				TreeItems.Add(newRoot);
				FillTreeNode(newRoot);
			}
		}

		// has current data to work with, just fills in 1 layer
		public async Task FillTreeNode(TreeItem root) {
			if (root == null) {
				return;
			}

			// Checking if the rootNode is empty right now, matters when loading in the first node.
			bool wasEmpty = root.Children.Count == 0;

			StorageFolder rootFolder;
			if (root.Content is StorageFolder) {
				rootFolder = root.Content as StorageFolder;
			} else {
				return;
			}

			// Separating the files and folders for convenience.
			var fileList = await rootFolder.GetFilesAsync();
			var folderList = await rootFolder.GetFoldersAsync();

			// List of nodes to be in the final node's Children list.
			var presentNodes = new List<TreeItem>();

			foreach (var folder in folderList) {
				// todo rework predicate, because it is kinda wacky
				//var sameFolders = rootNode.Children.Where(node => (node.Content as IStorageItem).Name == folder.Name);
				List<TreeItem> sameFolders = new List<TreeItem>(root.Children.Where((node) => (node.Content as IStorageItem).Name.Equals(folder.Name)));
				// add the list of present same files so we know, we dont have to delete these nodes later on
				presentNodes.AddRange(sameFolders);

				// if the folder is not present in the node, add it
				if (!sameFolders.Any()) {
					// adding new folder
					var newNode = new TreeItem() {
						Content = folder,
					};
					InsertItem(root, newNode);
					presentNodes.Add(newNode);
				}
			}

			foreach (var file in fileList) {
				// todo rework predicate, because it is kinda wacky
				List<TreeItem> sameFiles = new List<TreeItem>(root.Children.Where((node) => (node.Content as IStorageItem).Name.Equals(file.Name)));
				sameFiles.ForEach((node) => { node.Children.Clear(); });

				// add the list of present same files so we know, we dont have to delete these later on
				presentNodes.AddRange(sameFiles);

				// if the file is not present in the node, add it
				if (!sameFiles.Any()) {
					// adding new file
					var newNode = new TreeItem() {
						Content = file
					};
					InsertItem(root, newNode);
					presentNodes.Add(newNode);
				}
			}

			if (!wasEmpty) {
				// filter outdated nodes
				var outdatedNodes = new List<TreeItem>();
				foreach (var item in root.Children) {
					if (!presentNodes.Contains(item)) {
						outdatedNodes.Add(item);
					}
				}
				// delete outdated nodes
				foreach (var item in outdatedNodes) {
					root.Children.Remove(item);
				}
			}
		}
		public async Task<TreeItem> RenameItem(TreeItem item) {
			item.Content = await FilesystemService.RenameItem(item.Content as IStorageItem);
			return item;
		}
		public void OpenFile(TreeItem item) {
			try {
				var file = item.Content as StorageFile;
				// todo generalize it
				ProjectService.OpenEditor(file);
			} catch (Exception exception) {
				InfoService.DisplayInfoBar(exception.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
			}
		}
		public async Task DeleteItem(TreeItem item) {
			await FilesystemService.DeleteItem(item.Content as IStorageItem);
		}

		// sorting nodes based on content type and alphabetical order
		public void InsertItem(TreeItem root, TreeItem node) {
			var comparer = new StorageItemComparer<object>();
			for (int i = 0; i < root.Children.Count; i++) {
				if (comparer.Compare(root.Children[i].Content, node.Content) > 0) {
					root.Children.Insert(i, node);
					return;
				}
			}
			root.Children.Add(node);
		}
	}
}
