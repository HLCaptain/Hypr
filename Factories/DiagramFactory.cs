using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HyprWinUI3.Models.Diagrams;
using HyprWinUI3.Services;
using Windows.Storage;

namespace HyprWinUI3.Factories {
	class DiagramFactory {
		public static async Task<Diagram> MakeDiagramFromFile(StorageFile file) {
			if (!Constants.Extentions.DiagramExtentions.Contains(file.FileType)) {
				return null;
			}
			foreach (var extention in Constants.Extentions.DiagramExtentions) {
				if (extention == file.FileType) {
					try {
						Type diagramType = Constants.Extentions.ExtentionTypes[extention];
						string diagramData = await FileIO.ReadTextAsync(file);
						Diagram diagram = (Diagram)JsonSerializer.Deserialize(diagramData, diagramType);
						return diagram;
					} catch (Exception e) {
						InfoService.DisplayError(e.Message);
					}
				}
			}
			return null;
		}
		public static Diagram CreateDiagram(string extention) {
			if (!Constants.Extentions.DiagramExtentions.Contains(extention)) {
				return null;
			}
			try {
				Type diagramType = Constants.Extentions.ExtentionTypes[extention];
				Diagram diagram = (Diagram)Activator.CreateInstance(diagramType);
				return diagram;
			} catch (Exception e) {
				InfoService.DisplayError(e.Message);
			}
			return null;
		}
	}
}
