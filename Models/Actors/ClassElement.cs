using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Actors {
	public class ClassElement : Element {
		public string Specification { get; set; }
		public bool IsAbstract { get; set; }
		public ObservableCollection<FieldVariable> Fields { get; set; } = new ObservableCollection<FieldVariable>();
		public ObservableCollection<Method> Methods { get; set; } = new ObservableCollection<Method>();
		public ClassElement() {
			Fields.CollectionChanged += async (sender, args) => {
				if (File != null) {
					await FilesystemService.SaveActorFile(this);
				}
			};
			Methods.CollectionChanged += async (sender, args) => {
				if (File != null) {
					await FilesystemService.SaveActorFile(this);
				}
			};
			PropertyChanged += async (sender, args) => {
				if (File != null) {
					await FilesystemService.SaveActorFile(this);
				}
			};
		}
	}
}
