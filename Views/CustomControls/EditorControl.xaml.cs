using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
	public sealed partial class EditorControl : UserControl {
		const int delta = 48;
		int spaceBetweenDots = delta;
		

		public EditorControl() {
			this.InitializeComponent();
		}

		// need to redraw the session?
		private bool needToRedraw() {
			int newSpaceBetweenDots = (int)(delta * Math.Pow(2, Math.Round(1 / scrollViewer.ZoomFactor / 4)));
			if (spaceBetweenDots != newSpaceBetweenDots) {
				spaceBetweenDots = newSpaceBetweenDots;
				return true;
			} else {
				return false;
			}
		}

		private void canvas_RegionsInvalidated(Microsoft.Graphics.Canvas.UI.Xaml.CanvasVirtualControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasRegionsInvalidatedEventArgs args) {
			// ini size
			float dotSize = 2.4f * spaceBetweenDots / delta;

			foreach (var region in args.InvalidatedRegions) {
				using (var drawSession = sender.CreateDrawingSession(region)) {
					drawSession.Clear(Windows.UI.Color.FromArgb(0, 0, 0, 0));
					for (int i = (int)Math.Round(region.Left);
                        i < region.Right + spaceBetweenDots;
                        i += spaceBetweenDots) {
						for (int j = (int)Math.Round(region.Top);
							j < region.Bottom + spaceBetweenDots;
							j += spaceBetweenDots) {
							drawSession.FillCircle(
								new Vector2(
									(float)(i - (int)Math.Round(region.Left) % spaceBetweenDots),
									(float)(j - (int)Math.Round(region.Top) % spaceBetweenDots)),
								dotSize,
								Windows.UI.Color.FromArgb(100, 255, 255, 255));
						}
					}
				}
			}
		}
        double zoom = 0;
        bool zooming = false;
        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) {
            // todo: make canvas redraw itself loseless after zoom
            if (zoom == scrollViewer.ZoomFactor) {
                if (zooming) {
                    if (needToRedraw()) {
                        canvas.Invalidate();
                    }
                    zooming = false;
                }
            } else {
                zooming = true;
            }
            zoom = scrollViewer.ZoomFactor;
        }
    }
}
