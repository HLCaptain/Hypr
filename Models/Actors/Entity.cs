using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HyprWinUI3.Models.Actors {
    public class Entity {
        public string Uid { get; set; }
        public string Name { get; set; }

        public Entity() {
            Uid = Guid.NewGuid().ToString();
        }

        protected virtual void Save() {
            string obj = JsonSerializer.Serialize(this);
            Console.WriteLine(obj);
        }
    }
}
