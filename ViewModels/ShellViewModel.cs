﻿using System;
using System.Collections.Generic;
using System.Windows.Input;

using HyprWinUI3.Helpers;
using HyprWinUI3.Services;
using HyprWinUI3.Views;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace HyprWinUI3.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);
        private IList<KeyboardAccelerator> _keyboardAccelerators;

        private ICommand _loadedCommand;
        private ICommand _menuViewsMainCommand;
        private ICommand _menuViewsTabbedPivotCommand;
        private ICommand _menuViewsTabViewCommand;
        private ICommand _menuFilesSettingsCommand;
        private ICommand _menuFileExitCommand;

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand MenuViewsMainCommand => _menuViewsMainCommand ?? (_menuViewsMainCommand = new RelayCommand(OnMenuViewsMain));

        public ICommand MenuViewsTabbedPivotCommand => _menuViewsTabbedPivotCommand ?? (_menuViewsTabbedPivotCommand = new RelayCommand(OnMenuViewsTabbedPivot));

        public ICommand MenuViewsTabViewCommand => _menuViewsTabViewCommand ?? (_menuViewsTabViewCommand = new RelayCommand(OnMenuViewsTabView));

        public ICommand MenuFileSettingsCommand => _menuFilesSettingsCommand ?? (_menuFilesSettingsCommand = new RelayCommand(OnMenuFileSettings));

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public ShellViewModel()
        {
        }

        public void Initialize(Frame shellFrame, SplitView splitView, Frame rightFrame, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            NavigationService.Frame = shellFrame;
            MenuNavigationHelper.Initialize(splitView, rightFrame);
            _keyboardAccelerators = keyboardAccelerators;
        }

        private void OnLoaded()
        {
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            _keyboardAccelerators.Add(_backKeyboardAccelerator);
        }

        private void OnMenuViewsMain() => MenuNavigationHelper.UpdateView(typeof(MainPage));

        private void OnMenuViewsTabbedPivot() => MenuNavigationHelper.UpdateView(typeof(TabbedPivotPage));

        private void OnMenuViewsTabView() => MenuNavigationHelper.UpdateView(typeof(TabViewPage));

        private void OnMenuFileSettings() => MenuNavigationHelper.OpenInRightPane(typeof(SettingsPage));

        private void OnMenuFileExit()
        {
            Application.Current.Exit();
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }
    }
}
