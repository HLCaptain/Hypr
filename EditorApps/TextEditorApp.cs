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

		public override bool LoadData(string data) {
			try {
				var dataModel = JsonSerializer.Deserialize<Note>(data);
				if (!IsNote(data)) {
					((Note)Model).Text = data;
				} else {
					Model = dataModel;
				}
				Model.Uid = dataModel.Uid;

				// If view is loaded, update text
				RefreshView();
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
				return false;
			}
			return true;
		}

		// flawed, because it is based on string comparing
		// compares the json parsed data serialized again with the original data
		private bool IsNote(string data) {
			var dataModel = JsonSerializer.Deserialize<Note>(data);
			return Newtonsoft.Json.JsonConvert.SerializeObject(dataModel, Newtonsoft.Json.Formatting.Indented).Equals(data);
		}
	}
}
