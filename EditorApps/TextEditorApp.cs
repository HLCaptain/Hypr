using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.EditorApps {
	public class TextEditorApp : EditorApp {
		public TextEditorApp() {
			Model = new Note();
		}

		public override void RefreshView() {
			if (View is RichEditBox) {
				((RichEditBox)View)?.TextDocument.SetText(Windows.UI.Text.TextSetOptions.None, ((Note)Model).Text);
			} else {
				var richEditBox = new RichEditBox();
				richEditBox.TextDocument.SetText(Windows.UI.Text.TextSetOptions.None, ((Note)Model).Text);
				View = richEditBox;
			}
		}
	}
}
