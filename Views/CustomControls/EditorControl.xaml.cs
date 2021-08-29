using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using HyprWinUI3.ViewModels;
using HyprWinUI3.ViewModels.Editor;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
	public sealed partial class EditorControl : UserControl {
		

		public EditorViewModel ViewModel { get; set; } = new EditorViewModel();
		
		public EditorControl(TabViewItem tabViewItem = null) {
			this.InitializeComponent();
			ViewModel.Grid = grid;
			ViewModel.TabViewItem = tabViewItem;
			ViewModel.View = this;
			ProjectService.ProjectChangedEvent += ViewModel.RefreshItems;
			ViewModel.RefreshItems();
		}

		public EditorControl(EditorApp editor, TabViewItem tabViewItem) {
			this.InitializeComponent();
			ViewModel.TabViewItem = tabViewItem;
			ViewModel.View = this;
			ViewModel.LoadEditor(editor);
		}
	}
}
