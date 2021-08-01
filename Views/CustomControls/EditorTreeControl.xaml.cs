using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using HyprWinUI3.Services;
using HyprWinUI3.Strategies.ExtentionFiller;
using HyprWinUI3.ViewModels;
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
		public TabViewViewModel TabViewViewModel { get; set; }

		public EditorTreeControl() {
			this.InitializeComponent();
			ViewModel = new EditorTreeViewModel(this);
			ProjectService.RootFolderChangedEvent += InitializeTreeView;

			InitializeTreeView();
			FilesystemService.DiagramCreated += () => { RefreshTreeNode(treeView.RootNodes[0]); };

			// Needed when renaming an item can be done outside of the TreeView.s
			//FilesystemService.ItemRenamed += () => { RefreshTreeNode(treeView.RootNodes[0]); };
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
				treeView.RootNodes[0].Children.Add(new Microsoft.UI.Xaml.Controls.TreeViewNode {
					Content = "Child",
				});
				treeView.RootNodes[0].Children.Add(new Microsoft.UI.Xaml.Controls.TreeViewNode {
					Content = "Child",
				});
				treeView.RootNodes[0].Children.Add(new Microsoft.UI.Xaml.Controls.TreeViewNode {
					Content = "Child",
				});
				treeView.RootNodes[0].Children[0].Children.Add(new Microsoft.UI.Xaml.Controls.TreeViewNode {
					Content = "Child",
				});
				treeView.RootNodes[0].Children[0].Children.Add(new Microsoft.UI.Xaml.Controls.TreeViewNode {
					Content = "Child",
				});
			} else {
				var newRoot = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
					Content = ProjectService.RootFolder
				};
				newRoot.HasUnrealizedChildren = true;
				treeView.RootNodes.Add(newRoot);
			}
		}

		private async Task RefreshTreeNode(Microsoft.UI.Xaml.Controls.TreeViewNode rootNode) {
			if (rootNode == null) {
				return;
			}

			Stack<Microsoft.UI.Xaml.Controls.TreeViewNode> nodeStack = new Stack<Microsoft.UI.Xaml.Controls.TreeViewNode>();
			nodeStack.Push(rootNode);

			// DFS Preorder
			while (nodeStack.Any()) {
				Microsoft.UI.Xaml.Controls.TreeViewNode node = nodeStack.Pop();
				await FillTreeNode(node);
				// update it in treeview if needed
				foreach (var nodeChild in node.Children) {
					if (node.IsExpanded) {
						nodeStack.Push(nodeChild);
					}
				}
			}
		}

		// has current data to work with, just fills in 1 layer
		private async Task FillTreeNode(Microsoft.UI.Xaml.Controls.TreeViewNode rootNode) {
			if (rootNode == null) {
				return;
			}

			// Checking if the rootNode is empty right now, matters when loading in the first node.
			bool wasEmpty = rootNode.Children.Count == 0;

			StorageFolder rootFolder;
			if (rootNode.Content is StorageFolder) {
				rootFolder = rootNode.Content as StorageFolder;
			} else {
				return;
			}

			// Separating the files and folders for convenience.
			var fileList = await rootFolder.GetFilesAsync();
			var folderList = await rootFolder.GetFoldersAsync();

			// List of nodes to be in the final node's Children list.
			var presentNodes = new List<Microsoft.UI.Xaml.Controls.TreeViewNode>();

			foreach (var folder in folderList) {
				// todo rework predicate, because it is kinda wacky
				//var sameFolders = rootNode.Children.Where(node => (node.Content as IStorageItem).Name == folder.Name);
				List<Microsoft.UI.Xaml.Controls.TreeViewNode> sameFolders = new List<Microsoft.UI.Xaml.Controls.TreeViewNode>(rootNode.Children.Where((node) => (node.Content as IStorageItem).Name.Equals(folder.Name)));
				// add the list of present same files so we know, we dont have to delete these nodes later on
				presentNodes.AddRange(sameFolders);

				// if the folder is not present in the node, add it
				if (!sameFolders.Any()) {
					// adding new folder
					var newNode = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
						Content = folder,
						HasUnrealizedChildren = true
					};
					rootNode.Children.Add(newNode);
					presentNodes.Add(newNode);
				}
			}

			foreach (var file in fileList) {
				// todo rework predicate, because it is kinda wacky
				//var sameFiles = rootNode.Children.Where(node => (node.Content as IStorageItem).Name == file.Name);
				List<Microsoft.UI.Xaml.Controls.TreeViewNode> sameFiles = new List<Microsoft.UI.Xaml.Controls.TreeViewNode>(rootNode.Children.Where((node) => (node.Content as IStorageItem).Name.Equals(file.Name)));
				sameFiles.ForEach((node) => { node.HasUnrealizedChildren = false; });

				// add the list of present same files so we know, we dont have to delete these later on
				presentNodes.AddRange(sameFiles);

				// if the file is not present in the node, add it
				if (!sameFiles.Any()) {
					// adding new file
					var newNode = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
						Content = file,
						HasUnrealizedChildren = false
					};
					rootNode.Children.Add(newNode);
					presentNodes.Add(newNode);
				}
			}

			if (!wasEmpty) {
				// filter outdated nodes
				var outdatedNodes = new List<Microsoft.UI.Xaml.Controls.TreeViewNode>();
				foreach (var item in rootNode.Children) {
					if (!presentNodes.Contains(item)) {
						outdatedNodes.Add(item);
					}
				}
				// delete outdated nodes
				foreach (var item in outdatedNodes) {
					rootNode.Children.Remove(item);
				}
			}

			// already filled this node up
			rootNode.HasUnrealizedChildren = false;

			// todo sort
			// sorting nodes based on content type and alphabetical order

		}

		private volatile bool manualRefresh = false;
		/// <summary>
		/// If a args.Node has unrealized children, then load and fill them in.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private async void treeView_Expanding(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewExpandingEventArgs args) {
			if (args.Node.HasUnrealizedChildren && !manualRefresh && ProjectService.CurrentProject != null) {
				await RefreshTreeNode(args.Node);

				// need to expand it after filling the node, without this, folders with items wouldnt expend
				args.Node.IsExpanded = true;
			}
		}
		/// <summary>
		/// Collapse a node.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void treeView_Collapsed(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewCollapsedEventArgs args) {
			args.Node.HasUnrealizedChildren = true;
		}
		/// <summary>
		/// Expands or Collapses a node with StorageFolder content.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void treeView_ItemInvoked(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs args) {
			// placeholder for now
		}

		/// <summary>
		/// Adds a file to a folder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void AddDiagram(object sender, RoutedEventArgs e) {
			var treeNode = (Microsoft.UI.Xaml.Controls.TreeViewNode)((MenuFlyoutItem)sender).DataContext;
			// todo make this return async and make the whole method async
			if (ProjectService.IsInProjectSubfolder((StorageFolder)treeNode.Content)) {
				var diagram = await FilesystemService.CreateDiagramHere((StorageFolder)treeNode.Content, new DiagramExtentionFiller());
				ProjectService.AddDiagramFileToProject(diagram);
				TabViewViewModel?.OpenDiagram(diagram);
				treeNode.HasUnrealizedChildren = true;
				await RefreshTreeNode(treeNode);
			} else {
				InfoService.DisplayError("File is not in project folder");
			}
		}

		/// <summary>
		/// Renames a node item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void RenameItem(object sender, RoutedEventArgs e) {
			var treeNode = (Microsoft.UI.Xaml.Controls.TreeViewNode)((MenuFlyoutItem)sender).DataContext;

			await FilesystemService.RenameItem(treeNode.Content as IStorageItem);

			// View refresh
			var nodeParent = treeNode.Parent;
			await RefreshTreeNode(treeNode.Parent);
			if (nodeParent.IsExpanded) {
				nodeParent.IsExpanded = false;
				nodeParent.IsExpanded = true;
			}
		}

		private async void OpenFile(object sender, DoubleTappedRoutedEventArgs e) {
			var treeNode = (Microsoft.UI.Xaml.Controls.TreeViewNode)((StackPanel)sender).DataContext;
			try {
				var file = (StorageFile)treeNode.Content;
				// todo generalize it
				TabViewViewModel.OpenDiagram(await Factories.DiagramFactory.MakeDiagramFromFile(file));
			} catch (Exception exception) {
				InfoService.DisplayInfoBar(exception.Message, Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error);
				throw;
			}
		}

		private async void DeleteItem(object sender, RoutedEventArgs e) {
			var treeNode = (Microsoft.UI.Xaml.Controls.TreeViewNode)((MenuFlyoutItem)sender).DataContext;

			await FilesystemService.DeleteItem(treeNode.Content as IStorageItem);

			// View refresh
			var nodeParent = treeNode.Parent;
			await RefreshTreeNode(treeNode.Parent);
			if (nodeParent.IsExpanded) {
				nodeParent.IsExpanded = false;
				nodeParent.IsExpanded = true;
			}
		}
	}

	// todo make each file type a template and open method, so no type check is needed nowhere?
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
