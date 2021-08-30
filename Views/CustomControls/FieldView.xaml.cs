using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.ViewModels.Diagrams;
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
	public sealed partial class FieldView : UserControl {
		public ObservableCollection<Models.Actors.Visibility> Visibilities = new ObservableCollection<Models.Actors.Visibility>() { Models.Actors.Visibility.Default, Models.Actors.Visibility.Public, Models.Actors.Visibility.Private, Models.Actors.Visibility.Protected };

		public Models.Actors.Visibility FieldVisibility {
			get { return (Models.Actors.Visibility)GetValue(visibilityProperty); }
			set { SetValue(visibilityProperty, value); }
		}

		// Using a DependencyProperty as the backing store for visibility.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty visibilityProperty =
			DependencyProperty.Register("FieldVisibility", typeof(Models.Actors.Visibility), typeof(FieldVariable), new PropertyMetadata(Models.Actors.Visibility.Default));

		public string FieldType {
			get { return (string)GetValue(FieldTypeProperty); }
			set { SetValue(FieldTypeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FieldTypeProperty =
			DependencyProperty.Register("FieldType", typeof(string), typeof(FieldVariable), new PropertyMetadata("int"));

		public string FieldName {
			get { return (string)GetValue(FieldNameProperty); }
			set { SetValue(FieldNameProperty, value); }
		}

		// Using a DependencyProperty as the backing store for FieldName.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FieldNameProperty =
			DependencyProperty.Register("FieldName", typeof(string), typeof(FieldVariable), new PropertyMetadata("Name"));

		public bool IsFieldConst {
			get { return (bool)GetValue(IsFieldConstProperty); }
			set { SetValue(IsFieldConstProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsFieldConst.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsFieldConstProperty =
			DependencyProperty.Register("IsFieldConst", typeof(bool), typeof(FieldVariable), new PropertyMetadata(false));

		public bool IsFieldStatic {
			get { return (bool)GetValue(IsFieldStaticProperty); }
			set { SetValue(IsFieldStaticProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IsFieldStatic.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IsFieldStaticProperty =
			DependencyProperty.Register("IsFieldStatic", typeof(bool), typeof(FieldVariable), new PropertyMetadata(false));

		public FieldView() {
			this.InitializeComponent();
		}

		private void visibilityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			if (e.AddedItems.Any()) {
				FieldVisibility = (Models.Actors.Visibility)e.AddedItems[0];
			}
		}

		private void FieldType_TextUpdatedEvent(object sender, string text) {
			FieldType = text;
		}

		private void FieldName_TextUpdatedEvent(object sender, string text) {
			FieldName = text;
		}

		private void ConstantChanged_Click(object sender, RoutedEventArgs e) {
			IsFieldConst = ((ToggleMenuFlyoutItem)e.OriginalSource).IsChecked;
		}

		private void StaticChanged_Click(object sender, RoutedEventArgs e) {
			IsFieldStatic = ((ToggleMenuFlyoutItem)e.OriginalSource).IsChecked;
		}
	}
}
