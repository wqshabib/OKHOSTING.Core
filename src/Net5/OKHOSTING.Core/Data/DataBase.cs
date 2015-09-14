using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKHOSTING.Core.Data
{
	/// <summary>
	/// Represents a database, that can be memory based or sql based, as an abstraction for every project to use as datasource
	/// </summary>
	public class DataBase
	{
		public IEnumerable<T> Default<T>()
		{
			foreach (var item in Default(typeof(T)))
			{
				yield return (T) item;
			}
		}

		public IEnumerable Default(Type type)
		{
			string sessionName = type.FullName;
			IEnumerable dataSource = null;

			if (Session.Current.ContainsKey(sessionName))
			{
				dataSource = (IEnumerable) Session.Current[sessionName];
			}
			else
			{
				dataSource = new List<object>();
				Session.Current.Add(sessionName, dataSource);
			}

			return dataSource;
		}
	}
}