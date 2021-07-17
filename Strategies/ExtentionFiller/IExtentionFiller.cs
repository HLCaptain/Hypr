using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyprWinUI3.Strategies.ExtentionFiller {
	public interface IExtentionFiller {
		void FillWithExtentions(IList<object> list);
	}
}
