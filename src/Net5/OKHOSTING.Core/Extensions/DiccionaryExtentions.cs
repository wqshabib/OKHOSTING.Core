using System.Collections.Generic;
using System.Linq;

namespace OKHOSTING.Core.Extensions
{
	public static class DiccionaryExtentions
	{
		/// <summary>
		/// Allos to perform a reverse lookup in a generic dictionary
		/// </summary>
		public static TKey Reverse<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
		{
			return dictionary.First(x => x.Value.Equals(value)).Key;
		}
	}
}