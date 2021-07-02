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
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
	public sealed partial class EditorControl : UserControl {
		const int delta = 48;
		int spaceBetweenDots = delta;
        

        public EditorControl() {
			this.InitializeComponent();
		}

		private void canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args) {

			// ini delta

            // ini size
            float dotSize = 1.6f * spaceBetweenDots / delta;


            //args.DrawingSession.DrawText("y: " + scrollViewer.VerticalOffset.ToString(), new System.Numerics.Vector2(100, 100), Windows.UI.Color.FromArgb(255, 255, 255, 255));
            //args.DrawingSession.DrawText("x: " + scrollViewer.HorizontalOffset.ToString(), new System.Numerics.Vector2(100, 125), Windows.UI.Color.FromArgb(255, 255, 255, 255));

            // if something has changed, redraw
            args.DrawingSession.Clear(Windows.UI.Color.FromArgb(0, 0, 0, 0));
            for (int i = 0; i < canvas.Width / scrollViewer.ZoomFactor; i += spaceBetweenDots) {
                for (int j = 0; j < canvas.Height / scrollViewer.ZoomFactor; j += spaceBetweenDots) {
                    args.DrawingSession.FillCircle(new System.Numerics.Vector2(i, j), dotSize,Windows.UI.Color.FromArgb(100, 255, 255, 255));
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
			//canvas.Invalidate();
		}

        // need to redraw the dots?
        private bool needToRedraw() {
            int newSpaceBetweenDots = (int)(delta + delta * Math.Round(1 / scrollViewer.ZoomFactor / 4));
            if (spaceBetweenDots != newSpaceBetweenDots) {
                spaceBetweenDots = newSpaceBetweenDots;
                return true;
            } else {
                return false;
            }
        }
    }
}
