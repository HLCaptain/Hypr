using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Models.Diagrams {
	public abstract class Diagram : Actor {
		public List<string> Elements { get; set; } = new List<string>();

		public Diagram() {
		}
	}
}
