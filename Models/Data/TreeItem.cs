using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Models.Data {
	public class TreeItem {
		public ObservableCollection<TreeItem> Children { get; set; } = new ObservableCollection<TreeItem>();
		public TreeItem Parent { get; set; }
		public object Content { get; set; }
		public StackPanel StackPanel { get; set; }



		public void AddChild(TreeItem item) {
			item.Parent = this;
			Children.Add(item);
		}
	}
}
