using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Strategies;
using HyprWinUI3.Strategies.LoadStrategy;

namespace HyprWinUI3.Models.Diagrams {
    public class ClassDiagram : Diagram {
        public ClassDiagram() : base() {
            LoadStrategy = new ClassDiagramLoader();
		}
    }
}
