using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyprWinUI3.Models.Actors {
    public class Variable : Actor {
        public bool IsConst { get; set; }
        public string Type { get; set; }
        public Visibility Visibility { get; set; }
    }
}
