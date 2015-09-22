using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKHOSTING.Data
{
	/// <summary>
	/// Provides a base for custom lists that allow you to transform the values when reading/writing. Usefull for data manipulation and synchronization among diverse datasources
	/// </summary>
	public abstract class ListBase<T> : IList<T>
	{
		#region Abstract 

		public abstract IEnumerator<T> GetEnumerator();

		public abstract bool Contains(T item);

		public abstract void Add(T item);

		public abstract bool IsReadOnly
		{
			get;
		}

		public abstract bool Remove(T item);
		
		#endregion

		#region Virtual

		public virtual int Id { get; set; }

		public virtual int IndexOf(T item)
		{
			return this.ToList().IndexOf(item);
		}

		public virtual void Insert(int index, T item)
		{
			Add(item);
		}

		public virtual void RemoveAt(int index)
		{
			Remove(this.ToList()[index]);
		}

		public virtual T this[int index]
		{
			get
			{
				return this.ToList()[index];
			}
			set
			{
				this.ToList()[index] = value;
			}
		}

		public virtual void Clear()
		{
			while(Count > 0)
			{
				RemoveAt(0);
			}
		}

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			int i = 0;

			foreach(T e in this)
			{
				array.SetValue(e, arrayIndex, i);
			}
		}

		public virtual int Count
		{
			get
			{
				return this.ToList().Count;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
		#endregion
	}
}