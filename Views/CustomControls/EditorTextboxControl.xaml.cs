using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
	public sealed partial class EditorTextboxControl : UserControl {
		// textBLOCK is not editable
		// textBOX is editable
		public TextBox TextBox { get => textBox; }
		public TextBlock TextBlock { get => textBlock; }
		public event Action<string> TextChanged;
		public EditorTextboxControl() {
			this.InitializeComponent();
			textBox.IsEnabled = false;
			textBox.Visibility = Visibility.Collapsed;
		}

		private void textBlock_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) {
			textBox.IsEnabled = true;
			textBox.Visibility = Visibility.Visible;
			textBox.Text = textBlock.Text;
			textBox.Focus(FocusState.Keyboard);
			textBox.SelectAll();
			textBlock.Visibility = Visibility.Collapsed;
		}

		private void textBox_LostFocus(object sender, RoutedEventArgs e) {
			textBox.IsEnabled = false;
			textBox.Visibility = Visibility.Collapsed;
			textBlock.Text = textBox.Text;
			textBlock.Visibility = Visibility.Visible;
		}
	}
}
