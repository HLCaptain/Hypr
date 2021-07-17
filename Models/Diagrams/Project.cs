using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Models.Diagrams {
	public class Project : Entity {
		/// <summary>
		/// Relative paths to Diagrams.
		/// </summary>
		public List<string> Diagrams { get; set; } = new List<string>();

		/// <summary>
		/// Relative paths to Elements.
		/// </summary>
		public List<string> Elements { get; set; } = new List<string>();
	}
}
