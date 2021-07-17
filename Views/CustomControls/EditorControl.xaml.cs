using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.ViewModels;
using HyprWinUI3.ViewModels.Editor;
using Microsoft.UI.Xaml.Controls;
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
    public sealed partial class EditorControl : UserControl {
        public EditorViewModel ViewModel { get; set; }
        public Diagram CurrentDiagram { get; set; }
        public Grid Grid { get => grid; }
        public TabView TabView { get; set; }
        public TabViewViewModel TabViewViewModel { get; set; }
        public EditorControl(TabViewViewModel tabViewVM) {
            this.InitializeComponent();
            TabView = tabViewVM.TabView;
            TabViewViewModel = tabViewVM;
            ViewModel = new EditorViewModel(this);
        }

        public EditorControl(TabViewViewModel tabViewVM, Diagram diagram) {
            this.InitializeComponent();
            TabView = tabViewVM.TabView;
            TabViewViewModel = tabViewVM;
            ViewModel = new EditorViewModel(this);
            CurrentDiagram = diagram;
            ViewModel.LoadDiagram(diagram);
        }
    }
}
