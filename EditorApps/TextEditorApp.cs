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
		public override void InitializeView() {
			var richEditBox = new RichEditBox();
			richEditBox.TextDocument.SetText(Windows.UI.Text.TextSetOptions.None, ((Note)Model).Text);
			View = richEditBox;
		}

		public override bool LoadData(string data) {
			try {
				Model = JsonSerializer.Deserialize<Note>(data);
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
				return false;
			}
			return true;
		}

		public override bool SaveData(StorageFolder folder) {
			throw new NotImplementedException();
		}
	}
}
