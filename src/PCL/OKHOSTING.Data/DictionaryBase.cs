using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OKHOSTING.Data
{
	/// <summary>
	/// Provides base methods for generic custom dictionaries.
	/// Allows you to transform the values when reading/writing. Usefull for data manipulation and synchronization among diverse datasources
	/// </summary>
	/// <typeparam name="TKey">Type of keys that will be used as unique identifiers</typeparam>
	/// <typeparam name="TValue">Type of the actual value object</typeparam>
	public abstract class DictionaryBase<TKey, TValue> : IDictionary<TKey, TValue>
	{
		#region Abstract

		public abstract void Add(KeyValuePair<TKey, TValue> item);

		public abstract bool ContainsKey(TKey key);

		public abstract ICollection<TKey> Keys { get; }

		public abstract bool Remove(TKey key);

		public abstract TValue this[TKey key] { get; set; }

		public abstract bool IsReadOnly { get; }

		public abstract IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator();

		#endregion

		#region Virtual

		public virtual int Id { get; set; }

		public virtual int Count
		{
			get
			{
				return Keys.Count;
			}
		}

		public virtual ICollection<TValue> Values
		{
			get
			{
				List<TValue> values = new List<TValue>();

				foreach (KeyValuePair<TKey, TValue> item in this)
				{
					values.Add(item.Value);
				}

				return values;
			}
		}

		public virtual void Add(TKey key, TValue item)
		{
			Add(new KeyValuePair<TKey, TValue>(key, item));
		}

		public virtual System.Collections.IEnumerable EnumerateKeys()
		{
			foreach (TKey key in Keys)
			{
				yield return key;
			}
		}

		public virtual void Clear()
		{
			while (Count > 0)
			{
				Remove(this.Keys.First());
			}
		}

		public virtual bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return ContainsKey(item.Key);
		}

		public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			int i = 0;

			foreach (KeyValuePair<TKey, TValue> e in this)
			{
				array.SetValue(e, arrayIndex, i);
			}
		}

		public virtual bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return Remove(item.Key);
		}

		public virtual bool TryGetValue(TKey key, out TValue value)
		{
			try
			{
				value = this[key];
			}
			catch { }

			value = default(TValue);
			return false;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}