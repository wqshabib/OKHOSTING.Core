using System;
using System.Collections.Generic;

namespace OKHOSTING.Data
{
	/// <summary>
	/// A dictionary that just wraps another dictionary. Child classes can overwrite any method to customize behaviour over another dictionary.
	/// So theoretically you could stack a proxy above another and it should allow you to add complex behaviours like caching or encrypting
	/// </summary>
	public abstract class ProxyDictionary<TKey, TValue>: IDictionary<TKey, TValue>
	{
		protected IDictionary<TKey, TValue> Source { get; set; }

		public virtual void Add(KeyValuePair<TKey, TValue> item)
		{
			Source.Add(item.Key, item.Value);
		}

		public virtual void Add(TKey key, TValue value)
		{
			Source.Add(key, value);
		}

		public virtual bool ContainsKey(TKey key)
		{
			return Source.ContainsKey(key);
		}

		public virtual ICollection<TKey> Keys
		{
			get 
			{
				return Source.Keys; 
			}
		}

		public virtual bool Remove(TKey key)
		{
			return Source.Remove(key);
		}

		public virtual bool TryGetValue(TKey key, out TValue value)
		{
			return Source.TryGetValue(key, out value);
		}

		public virtual ICollection<TValue> Values
		{
			get 
			{ 
				return Source.Values; 
			}
		}

		public virtual TValue this[TKey key]
		{
			get
			{
				return Source[key];
			}
			set
			{
				Source[key] = value;
			}
		}

		
		public virtual void Clear()
		{
			Source.Clear();
		}

		public virtual bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return Source.ContainsKey(item.Key);
		}

		public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)Source).CopyTo(array, arrayIndex);
		}

		public virtual bool IsReadOnly
		{
			get 
			{ 
				return ((ICollection<KeyValuePair<TKey, TValue>>) Source).IsReadOnly;
			}
		}

		public virtual int Count
		{
			get 
			{ 
				return Source.Count; 
			}
		}


		public virtual bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return Source.Remove(item.Key);
		}

		public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return Source.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>) Source).GetEnumerator();
		}
	}
}