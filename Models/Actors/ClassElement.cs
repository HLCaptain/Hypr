using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyprWinUI3.Models.Actors {
    public class ClassElement : Vertex {
        public bool Specification { get; set; }
        public bool IsAbstract { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Method> Methods { get; set; }
    }
}
