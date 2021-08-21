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
	public sealed partial class EditorElementBackground : UserControl {
		private bool isDragged = false;
		private Point grabPosition = new Point(0, 0);

		public UIElement Element {
			get { return (UIElement)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Element.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ElementProperty =
			DependencyProperty.Register("Element", typeof(UIElement), typeof(UIElement), new PropertyMetadata(null));

		public Vertex Model {
			get { return (Vertex)GetValue(ModelProperty); }
			set { SetValue(ModelProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Model.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ModelProperty =
			DependencyProperty.Register("Model", typeof(Vertex), typeof(object), new PropertyMetadata(null));

		public EditorElementBackground() {
			this.InitializeComponent();
		}

		private void Element_PointerPressed(object sender, PointerRoutedEventArgs e) {
			isDragged = true;
			grabPosition = e.GetCurrentPoint(Element).Position;
		}

		private void Element_PointerReleased(object sender, PointerRoutedEventArgs e) {
			if (isDragged) {
				Canvas.SetLeft(Element, Canvas.GetLeft(Element) + e.GetCurrentPoint(Element).Position.X - grabPosition.X);
				Canvas.SetTop(Element, Canvas.GetTop(Element) + e.GetCurrentPoint(Element).Position.Y - grabPosition.Y);
			}
			if (Model != null) {
				Model.Position = e.GetCurrentPoint(Element).Position;
			}
			isDragged = false;
		}

		private void Element_PointerMoved(object sender, PointerRoutedEventArgs e) {
			if (isDragged) {
				Canvas.SetLeft(Element, Canvas.GetLeft(Element) + e.GetCurrentPoint(Element).Position.X - grabPosition.X);
				Canvas.SetTop(Element, Canvas.GetTop(Element) + e.GetCurrentPoint(Element).Position.Y - grabPosition.Y);
			}
		}

		private void Element_PointerExited(object sender, PointerRoutedEventArgs e) {
			if (isDragged) {
				Canvas.SetLeft(Element, Canvas.GetLeft(Element) + e.GetCurrentPoint(Element).Position.X - grabPosition.X);
				Canvas.SetTop(Element, Canvas.GetTop(Element) + e.GetCurrentPoint(Element).Position.Y - grabPosition.Y);
			}
		}
	}
}
