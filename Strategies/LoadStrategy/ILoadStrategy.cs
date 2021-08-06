using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyprWinUI3.Models.Actors;
using HyprWinUI3.ViewModels;
using HyprWinUI3.ViewModels.Editor;
using HyprWinUI3.Views.CustomControls;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace HyprWinUI3.Strategies.LoadStrategy {
	/// <summary>
	/// Loads up elements onto different UI elements.
	/// </summary>
	public interface ILoadStrategy {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="grid"></param>
		void LoadToEditor(VariableSizedWrapGrid grid);
	}
}
