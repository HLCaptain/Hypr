using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace HyprWinUI3.Views.CustomControls {
    public sealed partial class EditorCanvasControl : UserControl {
        /// <summary>
        /// This many pixels are 1 model unit.
        /// </summary>
        const int delta = 48;

        /// <summary>
        /// The space between dots in pixels. This can change overtime, with the user zooming in and out.
        /// </summary>
        int spaceBetweenDots = delta;

        public EditorCanvasControl() {
            this.InitializeComponent();
        }

        /// <summary>
        /// Decides to redraw the whole shown canvas according to the ZoomFactor of the ScrollViewer.
        /// Canvas needed to redrawn if user scrolls too close or far from the current size of the dots.
        /// </summary>
        /// <returns>True if canvas needed to be invalidated, otherwise false.</returns>
        private bool needToRedraw() {
            // Zooming evenly and independently from the ZoomFactor.
            int newSpaceBetweenDots = (int)(delta * Math.Pow(2, Math.Round(Math.Log(1 / scrollViewer.ZoomFactor, 2))));
            // If the zoom has changed, then redraw the scene.
            if (spaceBetweenDots != newSpaceBetweenDots) {
                spaceBetweenDots = newSpaceBetweenDots;
                return true;
            } else {
                return false;
            }
        }

        private void canvas_RegionsInvalidated(Microsoft.Graphics.Canvas.UI.Xaml.CanvasVirtualControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasRegionsInvalidatedEventArgs args) {
            // Ini current size of the dots.
            float dotSize = 2.4f * spaceBetweenDots / delta;

            // Redrawing each region.
            foreach (var region in args.InvalidatedRegions) {
                using (var drawSession = sender.CreateDrawingSession(region)) {
                    // Clearing every region first.
                    drawSession.Clear(Color.FromArgb(0, 0, 0, 0));

                    // Drawing the dots onto the region.
                    for (int i = (int)region.Left;
                        i < region.Right + spaceBetweenDots;
                        i += spaceBetweenDots) {
                        for (int j = (int)region.Top;
                            j < region.Bottom + spaceBetweenDots;
                            j += spaceBetweenDots) {
                            // Drawing the dots as circles.
                            //drawSession.FillCircle(
                            //    new System.Numerics.Vector2(
                            //        i - (int)region.Left % spaceBetweenDots,
                            //        j - (int)region.Top % spaceBetweenDots),
                            //    dotSize,
                            //    Color.FromArgb(100, 255, 255, 255));

                            // Drawing the dots as rectangles
                            drawSession.FillRectangle(
                                new Rect(
                                    new Point(
                                        i - (int)region.Left % spaceBetweenDots,
                                        j - (int)region.Top % spaceBetweenDots),
                                    new Size(dotSize * 1.2, dotSize * 1.2)),
                                Color.FromArgb(100, 255, 255, 255));
                        }
                    }
                }
            }
        }

        // todo: make canvas redraw itself loseless after zoom
        /// <summary>
        /// Stores the previous value of the ZoomFactor of the ScrollView.
        /// Needed to check when the zooming action ended.
        /// </summary>
        double zoom = 0;
        /// <summary>
        /// Is the user zooming in right now?
        /// </summary>
        bool zooming = false;
        /// <summary>
        /// Event called when the ScrollViewer's View has been changed. (Panning and zooming counts.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e) {
            // Is the zoom of the view chaning?
            if (zoom == scrollViewer.ZoomFactor) {
                // If the zooming stopped right now, redraw the scene if needed.
                if (zooming && needToRedraw()) {
                    canvas.Invalidate();
                }
                zooming = false;
            } else {
                zooming = true;
            }
            zoom = scrollViewer.ZoomFactor;
        }
    }
}
