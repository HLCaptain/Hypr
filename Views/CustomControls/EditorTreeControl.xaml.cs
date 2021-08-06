using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using HyprWinUI3.Models.Data;
using HyprWinUI3.Models.Diagrams;
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
		public EditorTreeViewModel ViewModel { get; set; } = new EditorTreeViewModel();
		public TabViewViewModel TabViewViewModel { get; set; }

		public EditorTreeControl() {
			this.InitializeComponent();

			FilesystemService.DiagramCreated += (diagram) => { RefreshTreeNode(treeView.RootNodes[0]); };

			// Needed when renaming an item can be done outside of the TreeView.s
			//FilesystemService.ItemRenamed += () => { RefreshTreeNode(treeView.RootNodes[0]); };
		}

		private async Task RefreshTreeNode(Microsoft.UI.Xaml.Controls.TreeViewNode root) {
			if (root == null) {
				return;
			}

			Stack<Microsoft.UI.Xaml.Controls.TreeViewNode> stack = new Stack<Microsoft.UI.Xaml.Controls.TreeViewNode>();
			stack.Push(root);

			// DFS Preorder
			while (stack.Any()) {
				var node = stack.Pop();
				await ViewModel.FillTreeNode(node.Content as TreeItem);
				// update it in treeview if needed
				foreach (var child in node.Children) {
					if (node.IsExpanded) {
						stack.Push(child);
					}
				}
			}
		}

		private Microsoft.UI.Xaml.Controls.TreeViewNode FindNode(TreeItem item) {
			if (treeView.RootNodes[0] == null) {
				return null;
			}

			Stack<Microsoft.UI.Xaml.Controls.TreeViewNode> stack = new Stack<Microsoft.UI.Xaml.Controls.TreeViewNode>();
			stack.Push(treeView.RootNodes[0]);

			// DFS Preorder
			while (stack.Any()) {
				var node = stack.Pop();
				if ((node.Content as TreeItem).Content == item.Content) {
					return node;
				}
				foreach (var child in node.Children) {
					stack.Push(child);
				}
			}
			return null;
		}

		/// <summary>
		/// If a args.Node has unrealized children, then load and fill them in.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private async void treeView_Expanding(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewExpandingEventArgs args) {
			if (ProjectService.CurrentProject != null) {
				await RefreshTreeNode(args.Node);
			}
		}
		/// <summary>
		/// Collapse a node.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void treeView_Collapsed(Microsoft.UI.Xaml.Controls.TreeView sender, Microsoft.UI.Xaml.Controls.TreeViewCollapsedEventArgs args) {
			// placeholder for now
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
			var item = (sender as MenuFlyoutItem).DataContext as TreeItem;
			var node = FindNode(item);
			// todo make this return async and make the whole method async
			if (ProjectService.IsInProjectSubfolder(item.Content as StorageFolder)) {
				var diagram = await FilesystemService.CreateDiagramHere(item.Content as StorageFolder, new DiagramExtentionFiller());
				ProjectService.AddFileToProjectList(diagram.File, ProjectService.CurrentProject.Diagrams);
				ProjectService.OpenDiagram(diagram);
				await RefreshTreeNode(node);
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
			var item = (sender as MenuFlyoutItem).DataContext as TreeItem;
			var node = FindNode(item);

			item = await ViewModel.RenameItem(item);
			// View refresh
			await RefreshTreeNode(node.Parent);
			if (node.Parent.IsExpanded) {
				node.Parent.IsExpanded = false;
				node.Parent.IsExpanded = true;
			}
			treeView.SelectedItem = item;
		}

		private void OpenFile(object sender, DoubleTappedRoutedEventArgs e) {
			var item = (sender as StackPanel).DataContext as TreeItem;

			ViewModel.OpenFile(item);
		}

		private async void DeleteItem(object sender, RoutedEventArgs e) {
			var item = (sender as MenuFlyoutItem).DataContext as TreeItem;
			var node = FindNode(item);

			await ViewModel.DeleteItem(item);

			// View refresh
			await RefreshTreeNode(node.Parent);
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
			var node = (TreeItem)item;

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
