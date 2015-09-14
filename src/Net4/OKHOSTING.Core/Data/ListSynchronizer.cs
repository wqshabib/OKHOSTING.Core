using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKHOSTING.Core.Data
{
	public class ListSynchronizer<T>
	{
		public void AddOrUpdate(IList<T> origin, IList<T> destination)
		{
			foreach (T item in origin)
			{
				if (destination.Contains(item))
				{
					int index = destination.IndexOf(item);
					destination[index] = item;
				}
				else
				{
					destination.Add(item);
				}
			}
		}
	}
}