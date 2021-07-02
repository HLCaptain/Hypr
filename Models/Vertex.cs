using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace HyprWinUI3.Models {
    class Vertex : Entity {
        [JsonInclude]
        private Point Position;
    }
}
