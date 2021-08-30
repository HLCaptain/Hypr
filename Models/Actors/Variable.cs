using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace HyprWinUI3.Models.Actors {
	public abstract class Variable : Element {
		private bool isConst;
		public bool IsConst { get => isConst; set => SetProperty(ref isConst, value); }
		private string type;
		public string Type { get => type; set => SetProperty(ref type, value); }
		private Visibility visibility;
		public Visibility Visibility { get => visibility; set => SetProperty(ref visibility, value); }
	}
}
