﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Models.Actors {
    class Edge : Element {
        public Vertex Start { get; set; }
        public Vertex End { get; set; }
    }
}