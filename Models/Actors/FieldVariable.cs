using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Actors {
	public class FieldVariable : Variable {
		private bool isStatic;
		public bool IsStatic { get => isStatic; set => SetProperty(ref isStatic, value); }

		public FieldVariable() {
			PropertyChanged += async (sender, args) => {
				if (File != null) {
					await FilesystemService.SaveActorFile(this);
				}
			};
		}
	}
}
