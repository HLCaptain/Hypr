using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Models.Actors;
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
	public sealed partial class NoteView : UserControl {
		private bool isDragged = false;
		private Point grabPosition = new Point(0, 0);
		public Canvas Canvas { get; set; }
		public Note Note { get; set; }
		public NoteView(Canvas canvas, Note note) {
			Canvas = canvas;
			Note = note;
			this.InitializeComponent();
			Canvas.SetTop(this, 400);
			Canvas.SetLeft(this, 400);
			tbText.TextBox.TextAlignment = TextAlignment.Left;
			tbText.TextBlock.TextAlignment = TextAlignment.Left;
		}

		private void EditorTextboxControl_PointerPressed(object sender, PointerRoutedEventArgs e) {
			isDragged = true;
			grabPosition = e.GetCurrentPoint(this).Position;
		}

		private void EditorTextboxControl_PointerReleased(object sender, PointerRoutedEventArgs e) {
			if (isDragged) {
				Canvas.SetLeft(this, Canvas.GetLeft(this) + e.GetCurrentPoint(this).Position.X - grabPosition.X);
				Canvas.SetTop(this, Canvas.GetTop(this) + e.GetCurrentPoint(this).Position.Y - grabPosition.Y);
			}
			isDragged = false;
		}

		private void Grid_PointerMoved(object sender, PointerRoutedEventArgs e) {
			if (isDragged) {
				Canvas.SetLeft(this, Canvas.GetLeft(this) + e.GetCurrentPoint(this).Position.X - grabPosition.X);
				Canvas.SetTop(this, Canvas.GetTop(this) + e.GetCurrentPoint(this).Position.Y - grabPosition.Y);
			}
		}

		private void EditorTextboxControl_PointerExited(object sender, PointerRoutedEventArgs e) {
			isDragged = false;
		}
	}
}
