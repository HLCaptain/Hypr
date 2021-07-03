using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;

namespace HyprWinUI3.Models.Diagrams {
    class Project : Entity {
        public List<Actors.Actor> Actors { get; set; }
    }
}
