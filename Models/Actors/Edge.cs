using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Models.Actors {
    public abstract class Edge : Element {
        public Vertex Start { get; set; }
        public Vertex End { get; set; }
    }
}
