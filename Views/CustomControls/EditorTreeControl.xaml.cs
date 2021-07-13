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
	/// <summary>
	/// Handles the TreeView which displays content in the filesystem according to the Current Project.
	/// </summary>
	public sealed partial class EditorTreeControl : UserControl {
		public EditorTreeViewModel ViewModel { get; set; }

		public EditorTreeControl() {
			this.InitializeComponent();
			ViewModel = new EditorTreeViewModel(this);
			ProjectService.RootFolderChangedEvent += InitializeTreeView;

			InitializeTreeView();
		}

		/// <summary>
		/// Loads in the first Children of nodes onto the TreeView if a root folder is loaded.
		/// </summary>
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
		/// <summary>
		/// Loads and fills in a node with its children if there is any.
		/// </summary>
		/// <param name="rootNode"></param>
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

			// Separating the files and folders for convenience.
			var fileList = await rootFolder.GetFilesAsync();
			var folderList = await rootFolder.GetFoldersAsync();

			// If there is none, return.
			if (fileList.Count == 0 && folderList.Count == 0) {
				return;
			}
			// adding folders first
			foreach (var folder in folderList) {
				var newNode = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
					Content = folder
				};
				// folders may contain children
				newNode.HasUnrealizedChildren = true;
				rootNode.Children.Add(newNode);
			}
			// adding files
			foreach (var file in fileList) {
				var newNode = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
					Content = file
				};
				rootNode.Children.Add(newNode);
			}
			// this folder no longer has any unrealized children
			rootNode.HasUnrealizedChildren = false;
		}
		/// <summary>
		/// If a args.Node has unrealized children, then load and fill them in.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void treeView_Expanding(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewExpandingEventArgs args) {
			if (args.Node.HasUnrealizedChildren) {
				FillTreeNode(args.Node);
			}
		}
		/// <summary>
		/// Collapse a node.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void treeView_Collapsed(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewCollapsedEventArgs args) {
			args.Node.Children.Clear();
			args.Node.HasUnrealizedChildren = true;
		}
		/// <summary>
		/// Expands or Collapses a node with StorageFolder content.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void treeView_ItemInvoked(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs args) {
			var node = args.InvokedItem as Microsoft.UI.Xaml.Controls.TreeViewNode;
			if (node.Content is IStorageItem item) {
				if (node.Content is StorageFolder) {
					node.IsExpanded = !node.IsExpanded;
				}
			}
		}

		/// <summary>
		/// Adds a file to a folder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
		/// <summary>
		/// Renames a node item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
	/// <summary>
	/// Templates to define items in TreeView.
	/// </summary>
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
