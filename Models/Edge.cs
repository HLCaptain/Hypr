using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HyprWinUI3.Models {
    class Edge : Entity {
        [JsonInclude]
        private Vertex Start { set; get; }
        [JsonInclude]
        private Vertex End { set; get; }
    }
}
