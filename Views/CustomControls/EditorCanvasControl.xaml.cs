using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Services;
using Microsoft.Toolkit.Uwp.UI.Helpers;
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
		private const int delta = 48;

		/// <summary>
		/// The space between dots in pixels. This can change overtime, with the user zooming in and out.
		/// </summary>
		private int spaceBetweenDots = delta;

		Color dotColor = Color.FromArgb(255, 0, 0, 0);
		Color backGroundColor = Color.FromArgb(255, 240, 240, 240);

		public EditorCanvasControl() {
			this.InitializeComponent();
			ThemeSelectorService.ThemeChanged += ThemeSelectorService_ThemeChanged;
			UpdateColors();
		}

		/// <summary>
		/// Invalidates the whole canvas and updates the drawing colors to match the new theme.
		/// </summary>
		private void ThemeSelectorService_ThemeChanged() {
			UpdateColors();
			canvas.Invalidate();
		}

		/// <summary>
		/// Decides to redraw the whole shown canvas according to the ZoomFactor of the ScrollViewer.
		/// Canvas needed to redrawn if user scrolls too close or far from the current size of the dots.
		/// </summary>
		/// <returns>True if canvas needed to be invalidated, otherwise false.</returns>
		private bool NeedToRedraw() {
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

		/// <summary>
		/// Redraws a region of the canvas.
		/// </summary>
		/// <param name="sender">We are able to draw on this.</param>
		/// <param name="args">Has the invalidated regions, which need to be redrawn.</param>
		private void Canvas_RegionsInvalidated(Microsoft.Graphics.Canvas.UI.Xaml.CanvasVirtualControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasRegionsInvalidatedEventArgs args) {
			// Ini current size of the dots.
			float dotSize = 2.4f * spaceBetweenDots / delta;

			// Redrawing each region.
			foreach (var region in args.InvalidatedRegions) {
				using (var drawSession = sender.CreateDrawingSession(region)) {
					// Clearing every region first.
					drawSession.Clear(backGroundColor);

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
							//    dotColor);

							// Drawing the dots as rectangles
							drawSession.FillRectangle(
								new Rect(
									new Point(
										i - (int)region.Left % spaceBetweenDots,
										j - (int)region.Top % spaceBetweenDots),
									new Size(dotSize * 1.2, dotSize * 1.2)),
								dotColor);
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
				if (zooming && NeedToRedraw()) {
					canvas.Invalidate();
				}
				zooming = false;
			} else {
				zooming = true;
			}
			zoom = scrollViewer.ZoomFactor;
		}
		/// <summary>
		/// Invalidates the whole canvas and updates the drawing colors to match the new theme.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void UserControl_ActualThemeChanged(FrameworkElement sender, object args) {
			UpdateColors();
			canvas.Invalidate();
		}

		/// <summary>
		/// Updates colors to match the new theme.
		/// </summary>
		private void UpdateColors() {
			// changing color based on current theme
			switch (ThemeSelectorService.Theme) {
				case ElementTheme.Default:
					if (ThemeSelectorService.IsHighContrast) {
						dotColor = Color.FromArgb(255, 255, 255, 255);
						backGroundColor = Color.FromArgb(255, 0, 0, 0);
					} else {
						dotColor = Color.FromArgb(255, 200, 200, 200);
						backGroundColor = Color.FromArgb(255, 20, 20, 20);
					}
					break;
				case ElementTheme.Light:
					if (ThemeSelectorService.IsHighContrast) {
						dotColor = Color.FromArgb(255, 0, 0, 0);
						backGroundColor = Color.FromArgb(255, 255, 255, 255);
					} else {
						dotColor = Color.FromArgb(255, 40, 40, 40);
						backGroundColor = Color.FromArgb(255, 240, 240, 240);
					}
					break;
				case ElementTheme.Dark:
					if (ThemeSelectorService.IsHighContrast) {
						dotColor = Color.FromArgb(255, 255, 255, 255);
						backGroundColor = Color.FromArgb(255, 0, 0, 0);
					} else {
						dotColor = Color.FromArgb(255, 200, 200, 200);
						backGroundColor = Color.FromArgb(255, 20, 20, 20);
					}
					break;
				default:
					break;
			}
		}
	}
}
