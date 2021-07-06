using System;

using HyprWinUI3.ViewModels;

using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
