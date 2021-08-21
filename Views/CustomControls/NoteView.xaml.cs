using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using HyprWinUI3.Models.Actors;
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
	public sealed partial class NoteView : UserControl {
		public Canvas Canvas { get; set; }
		public Note Note { get; set; }
		public UserControl This { get => this; }
		public NoteView(Canvas canvas, Note note) {
			Canvas = canvas;
			Note = note;
			this.InitializeComponent();
			Canvas.SetTop(this, 400);
			Canvas.SetLeft(this, 400);
			tbName.TextBlock.Text = note.Name;
			tbText.TextBlock.Text = note.Text;
			tbName.TextBox.IsEnabledChanged += (sender, args) => {
				if ((bool)args.NewValue == false) {
					note.Name = tbName.TextBox.Text;
				}
			};
			tbText.TextBox.IsEnabledChanged += (sender, args) => {
				if ((bool)args.NewValue == false) {
					note.Text = tbText.TextBox.Text;
				}
			};
			tbName.TextBox.TextAlignment = TextAlignment.Center;
			tbName.TextBlock.TextAlignment = TextAlignment.Center;
			tbText.TextBox.AcceptsReturn = true;
		}
	}
}
