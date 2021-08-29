using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;

namespace HyprWinUI3.Proxy {
	public class DocumentProxy : IActorProxy {
		public string ReferencePath { get; protected set; } = string.Empty;
		private Document Document { get; set; } = null;
		public async Task ChangeReference(string oldPath, string newPath) {
			if (((Document)await GetActor()).References.Contains(oldPath)) {
				((Document)await GetActor()).References[Document.References.IndexOf(oldPath)] = newPath;
				await SaveDocument();
			}
		}

		// get loaded document
		public async Task<Actor> GetActor() {
			if (Document == null) {
				await TryLoadDocument();
			}
			return Document;
		}

		public async Task SaveDocument() {
			if (Document != null) {
				await FilesystemService.SaveActorFile(Document);
			}
		}

		private async Task TryLoadDocument() {
			Document = (Document)await FilesystemService.LoadActor(ReferencePath);
		}

		public DocumentProxy(string referencePath) {
			ReferencePath = referencePath;
		}
	}
}
