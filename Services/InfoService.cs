﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Views;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Composition;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace HyprWinUI3.Services {
    /// <summary>
	/// Helps displaying information bars to the user.
	/// </summary>
	public static class InfoService {
        /// <summary>
		/// The UI element, the InfoBar is displayed on.
		/// </summary>
        public static VariableSizedWrapGrid InfoBarGrid { get; set; }

        /// <summary>
		/// Displays an InfoBar on the InfoBarGrid for 3 seconds.
		/// </summary>
		/// <param name="title">Title of the InfoBar</param>
		/// <param name="message">Message of the InfoBar</param>
		/// <param name="severity">Severity of the InfoBar</param>
		public static void DisplayInfoBar(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational) {
            var infoBar = new InfoBar() {
                Title = title,
                Message = message,
                Severity = severity,
                IsOpen = true,
                HorizontalAlignment = HorizontalAlignment.Center,
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(4),
			};

            InfoBarGrid?.Children.Add(infoBar);

            // timer removes infobar after 3 seconds
            var timer = new Timer(CloseInfoBar, infoBar, 3000, Timeout.Infinite);
        }

        /// <summary>
		/// Method is called when timer needs to close the InfoBar.
		/// </summary>
		/// <param name="state">The InfoBar that needs to be closed</param>
        private static async void CloseInfoBar(object state) {
            InfoBar infoBar = (InfoBar)state;
            // egy kurva zseni vagyok
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (infoBar != null) {
                    if (infoBar.IsOpen) {
                        InfoBarGrid?.Children.Remove(infoBar);
                    }
                }
            });
        }
	}
}
