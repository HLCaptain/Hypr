using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HyprWinUI3.Models {
    class Entity {
        [JsonInclude]
        private string Uid { set; get; }

        public Entity() {
            Uid = Guid.NewGuid().ToString();
        }
    }
}
