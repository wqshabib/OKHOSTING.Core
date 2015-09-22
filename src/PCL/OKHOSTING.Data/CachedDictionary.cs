using System.Collections.Generic;

namespace OKHOSTING.Data
{
	/// <summary>
	/// Caches an entire dictionary. Could use a lot of memory for large data
	/// </summary>
	public class CachedDictionary<TKey, TValue>: ProxyDictionary<TKey, TValue>
	{
		protected readonly IDictionary<TKey, TValue> Cache = new Dictionary<TKey, TValue>();

		public override void Add(TKey key, TValue value)
		{
			Source.Add(key, value);
			Cache.Add(key, value);
		}

		public override bool ContainsKey(TKey key)
		{
			return Cache.ContainsKey(key);
		}

		public override ICollection<TKey> Keys
		{
			get 
			{ 
				return Cache.Keys; 
			}
		}

		public override bool Remove(TKey key)
		{
			bool result = Source.Remove(key);
			Cache.Remove(key);

			return result;
		}

		public override bool TryGetValue(TKey key, out TValue value)
		{
			return Cache.TryGetValue(key, out value);
		}

		public override ICollection<TValue> Values
		{
			get 
			{ 
				return Cache.Values; 
			}
		}

		public override TValue this[TKey key]
		{
			get
			{
				return Cache[key];
			}
			set
			{
				Source[key] = value;
				Cache[key] = value;
			}
		}

		public override void Clear()
		{
			Source.Clear();
			Cache.Clear();
		}

		public override bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return Cache.ContainsKey(item.Key);
		}

		public override void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>) Cache).CopyTo(array, arrayIndex);
		}

		public override int Count
		{
			get 
			{ 
				return Cache.Count; 
			}
		}

		public override bool Remove(KeyValuePair<TKey, TValue> item)
		{
			bool result = Source.Remove(item.Key);
			Cache.Remove(item.Key);

			return result;
		}

		public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return Cache.GetEnumerator();
		}
	}
}