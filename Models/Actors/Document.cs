using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyprWinUI3.Models.Actors {
	public class Document : Actor {
		public ObservableCollection<string> References { get; set; } = new ObservableCollection<string>();
	}
}
