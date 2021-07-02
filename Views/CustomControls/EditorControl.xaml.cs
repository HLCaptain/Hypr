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

		}

        // need to redraw the session?
        private bool needToRedraw() {
            int newSpaceBetweenDots = (int)(delta + delta * Math.Round(1 / scrollViewer.ZoomFactor / 4));
            if (spaceBetweenDots != newSpaceBetweenDots) {
                spaceBetweenDots = newSpaceBetweenDots;
                return true;
            } else {
                return false;
            }
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (needToRedraw()) {
                canvas.Invalidate();
            }
        }

        private void canvas_RegionsInvalidated(Microsoft.Graphics.Canvas.UI.Xaml.CanvasVirtualControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasRegionsInvalidatedEventArgs args) {
            // ini size
            float dotSize = 1.6f * spaceBetweenDots / delta;

            foreach (var region in args.InvalidatedRegions) {
                using (var drawSession = sender.CreateDrawingSession(region)) {
                    drawSession.Clear(Windows.UI.Color.FromArgb(0, 0, 0, 0));
                    for (int i = 0; i < canvas.Width; i += spaceBetweenDots) {
                        for (int j = 0; j < canvas.Height; j += spaceBetweenDots) {
                            if (region.Contains(new Point(i, j))) {
                                drawSession.FillCircle(new System.Numerics.Vector2(i, j), dotSize, Windows.UI.Color.FromArgb(100, 255, 255, 255));
                            }
                        }
                    }
                }
            }
        }
    }
}
