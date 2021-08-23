using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Services;

namespace HyprWinUI3.Models.Diagrams {
	public class Project : Actor {
		/// <summary>
		/// Relative paths to Diagrams.
		/// </summary>
		public ObservableCollection<string> Diagrams { get; set; } = new ObservableCollection<string>();

		/// <summary>
		/// Relative paths to Elements.
		/// </summary>
		public ObservableCollection<string> Elements { get; set; } = new ObservableCollection<string>();

		public Project() {
			Elements.CollectionChanged += async (sender, args) => {
				if (File != null || ProjectService.CurrentProject?.File == File) {
					await ProjectService.SaveProject();
				}
			};
			Diagrams.CollectionChanged += async (sender, args) => {
				if (File != null || ProjectService.CurrentProject?.File == File) {
					await ProjectService.SaveProject();
				}
			};
			PropertyChanged += async (sender, args) => {
				if (File != null || ProjectService.CurrentProject?.File == File) {
					await ProjectService.SaveProject();
				}
			};
		}
	}
}
