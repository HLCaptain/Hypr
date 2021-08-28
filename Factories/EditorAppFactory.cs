using System;
using System.Collections.Generic;
using System.IO;
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
				// is the file in the projectfile's pathlist?
				var references = ProjectService.DocumentProxies.Where((proxy) => {
					return proxy.ReferencePath == Path.GetRelativePath(ProjectService.RootFolder.Path, file.Path);
				}).ToList();
				ProjectService.CurrentProject.ToString();
				if (references.Any()) {
					editor.Model = await references[0].GetActor();
				} else {
					InfoService.DisplayInfoBar($"{file.Name} is not in project files!");
					editor.Model = await FilesystemService.LoadActor(Path.GetRelativePath(ProjectService.RootFolder.Path, file.Path));
				}
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
