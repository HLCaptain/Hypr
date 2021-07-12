using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Models.Diagrams {
    public class Project : Entity {
        public List<Diagram> Diagrams { get; set; } = new List<Diagram>();

        /// <summary>
        /// Relative paths to diagrams which contains the Elements.
        /// </summary>
        public List<Element> Elements { get; set; } = new List<Element>();
    }
}
