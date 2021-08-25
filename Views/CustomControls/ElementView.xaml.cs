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
	public sealed partial class ElementView : UserControl {
		public Vertex Vertex { get; set; } = null;
		private UIElement _view;
		public UIElement View {
			get => _view;
			set {
				grid.Children.Clear();
				if (Vertex != null) {
					grid.Children.Add(new EditorElementBackground() {
						Model = Vertex,
						Element = this,
					});
					Canvas.SetLeft(this, Vertex.Position.X);
					Canvas.SetTop(this, Vertex.Position.Y);
				}
				_view = value;
				grid.Children.Add(value);
			}
		}
		public ElementView() {
			this.InitializeComponent();
		}
	}
}
