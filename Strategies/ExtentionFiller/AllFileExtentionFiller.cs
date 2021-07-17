using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyprWinUI3.Strategies.ExtentionFiller {
	public class AllFileExtentionFiller : IExtentionFiller {
		public void FillWithExtentions(IList<object> list) {
			foreach (var extention in Constants.Extentions.DiagramExtentions) {
				list.Add(extention);
			};
			foreach (var extention in Constants.Extentions.ElementExtentions) {
				list.Add(extention);
			};
		}
	}
}
