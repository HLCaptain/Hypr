using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HyprWinUI3.Models.Actors {
    public class Entity : ObservableObject {
        public string Uid { get; set; }
        public string Name { get; set; }

        public Entity() {
            Uid = Guid.NewGuid().ToString();
        }
    }
}
