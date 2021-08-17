using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.EditorApps;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;
using Windows.Storage;

namespace HyprWinUI3.Factories {
	class EditorAppFactory {
		public static async Task<EditorApp> CreateEditorFromFile(StorageFile file) {
			if (!Constants.Extentions.EditorExtentions.Contains(file.FileType)) {
				return null;
			}
			foreach (var extention in Constants.Extentions.EditorExtentions) {
				if (extention == file.FileType) {
					try {
						string data = await FileIO.ReadTextAsync(file);
						var editor = CreateEditor(extention);
						editor.LoadData(data);
						return editor;
					} catch (Exception e) {
						InfoService.DisplayError(e.Message);
					}
				}
			}
			return null;
		}
		public static EditorApp CreateEditor(string extention) {
			if (!Constants.Extentions.EditorExtentions.Contains(extention)) {
				return null;
			}
			try {
				Type editorType = Constants.Extentions.ExtentionAppTypes[extention];
				var editor = (EditorApp)Activator.CreateInstance(editorType);
				return editor;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
			return null;
		}
	}
}
