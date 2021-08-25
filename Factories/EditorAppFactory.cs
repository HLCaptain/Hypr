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
			try {
				var editor = CreateEditor(file.FileType);
				await editor.LoadData(file);
				return editor;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
				return new TextEditorApp();
			}
		}
		public static EditorApp CreateEditor(string extention) {
			try {
				Type editorType = Constants.Extentions.ExtentionAppTypes[extention];
				var editor = (EditorApp)Activator.CreateInstance(editorType);
				return editor;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message + $"\nEditor cannot find proper Editor Application to open the file with the extention \"{extention}\".\nDefaulting to Text Editor.");
				return new TextEditorApp();
			}
		}
	}
}
