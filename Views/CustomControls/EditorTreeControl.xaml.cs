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
        public EditorTreeViewModel ViewModel { get; } = new EditorTreeViewModel();

        public EditorTreeControl() {
            this.InitializeComponent();

            ProjectService.RootFolderChangedEvent += InitializeTreeView;

            // ini with dummy data
            InitializeTreeView();
        }

        private void InitializeTreeView() {
            treeView.RootNodes.Clear();
            if (ProjectService.RootFolder == null) {
                StackPanel content = new StackPanel();
                content.Children.Add(new SymbolIcon(Symbol.Cancel));
                content.Children.Add(new TextBlock() { Text = "No project" });
                treeView.RootNodes.Add(new Microsoft.UI.Xaml.Controls.TreeViewNode {
                    Content = "No project",
                });
            } else {
                StackPanel content = new StackPanel();
                content.Children.Add(new SymbolIcon(Symbol.Document));
                content.Children.Add(new TextBlock() { Text = ProjectService.RootFolder.Name });
                var newRoot = new Microsoft.UI.Xaml.Controls.TreeViewNode() {
                    Content = ProjectService.RootFolder,
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
            StorageFolder folder = null;

            if (rootNode.Content is StorageFolder && rootNode.HasUnrealizedChildren == true) {
                folder = rootNode.Content as StorageFolder;
            } else {
                // The node isn't a folder, or it's already been filled.
                return;
            }

            IReadOnlyList<IStorageItem> itemsList = await folder.GetItemsAsync();

            if (itemsList.Count == 0) {
                // The item is a folder, but it's empty. Leave HasUnrealizedChildren = true so
                // that the chevron appears, but don't try to process children that aren't there.
                return;
            }

            foreach (var item in itemsList) {
                var newNode = new Microsoft.UI.Xaml.Controls.TreeViewNode();
                newNode.Content = item;

                if (item is StorageFolder) {
                    // If the item is a folder, set HasUnrealizedChildren to true.
                    // This makes the collapsed chevron show up.
                    newNode.HasUnrealizedChildren = true;
                } else {
                    // Item is StorageFile. No processing needed for this scenario.
                }

                rootNode.Children.Add(newNode);
            }

            // Children were just added to this node, so set HasUnrealizedChildren to false.
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
                //FileNameTextBlock.Text = item.Name;
                //FilePathTextBlock.Text = item.Path;
                //TreeDepthTextBlock.Text = node.Depth.ToString();

                if (node.Content is StorageFolder) {
                    node.IsExpanded = !node.IsExpanded;
                }
            }
        }
    }
}
