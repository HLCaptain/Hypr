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
	public static class InfoService {
		public static void DisplayInfoBar(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational) {
			var infoBar = new InfoBar() {
				Title = title,
				Message = message,
				Severity = severity,
				IsOpen = true,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(24),
			};

            // todo: rework how infobar displays
            ((TabViewPage)NavigationService.Frame.Content).Grid.Children.Add(infoBar);

            var timer = new Timer(CloseInfoBar, infoBar, 3000, Timeout.Infinite);
        }

        private static async void CloseInfoBar(object state) {
            InfoBar infoBar = (InfoBar)state;
            // egy kurva zseni vagyok
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (infoBar != null) {
                    if (infoBar.IsOpen) {
                        ((TabViewPage)NavigationService.Frame.Content).Grid.Children.Remove(infoBar);
                    }
                }
            });
        }
	}
}
