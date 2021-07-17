using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HyprWinUI3.Models.Actors {
    /// <summary>
    /// Actors can be saved and can be represented in the tree view as an item (always leaf).
    /// </summary>
    public class Actor : Entity {
        public StorageFile File { get; set; }
    }
}
