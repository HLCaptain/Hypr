using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.Strategies.LoadStrategy;

namespace HyprWinUI3.Models.Diagrams {
    // composite design pattern
    public class Diagram : Actor {
        public List<Element> Elements { get; set; }

        public Diagram() {
            LoadStrategy = new DiagramLoader();
        }
    }
}
