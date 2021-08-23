using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Foundation;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Models.Actors {
    public abstract class Vertex : Element {
        [JsonIgnore]
        private Point position;
        public Point Position { get => position; set => SetProperty<Point>(ref position, value); }
    }
}
