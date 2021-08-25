using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Strategies.LoadStrategy {
	public interface ILoadStrategy {
		Task LoadToCanvas(Canvas canvas, object entity);
	}
}
