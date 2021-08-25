using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace HyprWinUI3.Helpers {
	public class StorageItemComparer<T> : Comparer<T> {
		public override int Compare(T x, T y) {
			if (!(x is IStorageItem) || !(y is IStorageItem)) {
				return 0;
			}
			var varX = (IStorageItem)x;
			var varY = (IStorageItem)y;
			if (varX is StorageFolder && varY is StorageFolder) {
				return varX.Name.CompareTo(varY.Name);
			}
			if (varX is StorageFile && varY is StorageFile) {
				return varX.Name.CompareTo(varY.Name);
			}
			if (varX is StorageFolder && varY is StorageFile) {
				return -1;
			}
			if (varX is StorageFile && varY is StorageFolder) {
				return 1;
			}
			return 0;
		}
	}
}
