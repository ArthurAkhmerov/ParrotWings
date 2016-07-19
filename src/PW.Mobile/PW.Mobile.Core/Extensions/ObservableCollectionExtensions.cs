using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PW.Mobile.Core.Extensions
{
	public static class ObservableCollectionExtensions
	{
		public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> items)
		{
			foreach (var item in items)
				source.Add(item);
		}
	}
}