using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels.Editor;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
	public sealed partial class EditorTreeControl : UserControl {
		public EditorTreeViewModel ViewModel { get; set; }

		public EditorTreeControl() {
			this.InitializeComponent();
			ViewModel = new EditorTreeViewModel(this);
			ProjectService.RootFolderChangedEvent += InitializeTreeView;

			InitializeTreeView();
		}

		private void InitializeTreeView() {
			treeView.RootNodes.Clear();
			if (ProjectService.RootFolder == null) {
				treeView.RootNodes.Add(new Microsoft.UI.Xaml.Controls.TreeViewNode {
					Content = "No project",
				});
			} else {
				var newRoot = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
					Content = ProjectService.RootFolder
				};
				treeView.RootNodes.Add(newRoot);
				newRoot.HasUnrealizedChildren = true;
				FillTreeNode(newRoot);
			}
		}

		private async void FillTreeNode(Microsoft.UI.Xaml.Controls.TreeViewNode rootNode) {
			// Get the contents of the folder represented by the current tree node.
			// Add each item as a new child node of the node that's being expanded.

			// Only process the node if it's a folder and has unrealized children.
			StorageFolder rootFolder = null;

			if (rootNode.Content is StorageFolder && rootNode.HasUnrealizedChildren == true) {
				rootFolder = rootNode.Content as StorageFolder;
			} else {
				// The node isn't a folder, or it's already been filled.
				return;
			}

			var fileList = await rootFolder.GetFilesAsync();
			var folderList = await rootFolder.GetFoldersAsync();

			if (fileList.Count == 0 && folderList.Count == 0) {
				return;
			}
			foreach (var folder in folderList) {
				var newNode = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
					Content = folder
				};
				newNode.HasUnrealizedChildren = true;
				rootNode.Children.Add(newNode);
			}
			foreach (var file in fileList) {
				var newNode = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
					Content = file
				};
				rootNode.Children.Add(newNode);
			}
			rootNode.HasUnrealizedChildren = false;
		}

		private void treeView_Expanding(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewExpandingEventArgs args) {
			if (args.Node.HasUnrealizedChildren) {
				FillTreeNode(args.Node);
			}
		}

		private void treeView_Collapsed(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewCollapsedEventArgs args) {
			args.Node.Children.Clear();
			args.Node.HasUnrealizedChildren = true;
		}

		private void treeView_ItemInvoked(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs args) {
			var node = args.InvokedItem as Microsoft.UI.Xaml.Controls.TreeViewNode;
			if (node.Content is IStorageItem item) {
				if (node.Content is StorageFolder) {
					node.IsExpanded = !node.IsExpanded;
				}
			}
		}

		private void AddFile(object sender, RoutedEventArgs e) {
			var treeNode = (Microsoft.UI.Xaml.Controls.TreeViewNode)((MenuFlyoutItem)sender).DataContext;

			// closing the treenode (forces refresh after adding new file)
			if (treeNode.IsExpanded) {
				treeNode.IsExpanded = false;
			}
			treeNode.Children.Clear();
			treeNode.HasUnrealizedChildren = true;

			// todo make this return async and make the whole method async
			FilesystemService.CreateNewFile((StorageFolder)treeNode.Content);

			// todo refresh treenode properly
		}

		private void RenameItem(object sender, RoutedEventArgs e) {
			var treeNode = (Microsoft.UI.Xaml.Controls.TreeViewNode)((MenuFlyoutItem)sender).DataContext;

			// may need to refresh the TreeView
			var nodeParent = treeNode.Parent;

			// closing the treenode (forces refresh after adding new file)
			if (treeNode.IsExpanded) {
				treeNode.IsExpanded = false;
			}
			treeNode.Children.Clear();
			treeNode.HasUnrealizedChildren = true;

			// todo call the FsService rename method with treeNode
			// todo refresh tree
		}
	}
	public class ExplorerItemTemplateSelector : DataTemplateSelector {
		public DataTemplate DefaultTemplate { get; set; }
		public DataTemplate FileTemplate { get; set; }
		public DataTemplate FolderTemplate { get; set; }
		public DataTemplate StringTemplate { get; set; }
		public DataTemplate NullTemplate { get; set; }

		protected override DataTemplate SelectTemplateCore(object item) {
			var node = (Microsoft.UI.Xaml.Controls.TreeViewNode)item;

			if (node.Content is StorageFolder) {
				return FolderTemplate;
			}
			if (node.Content is StorageFile) {
				return FileTemplate;
			}
			if (node.Content is string) {
				return StringTemplate;
			}
			if (node.Content is IStorageItem) {
				return DefaultTemplate;
			}
			return NullTemplate;
		}
	}
}
