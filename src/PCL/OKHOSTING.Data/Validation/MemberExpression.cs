using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static OKHOSTING.Core.TypeExtensions;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Represents a member of aclass, but that can also spport nested members, pe: "Address" or "Address.Country.Name".
	/// Usefull to validate or map members in a more flexible way than just using memberinfos
	/// </summary>
	public class MemberExpression
	{
		public MemberExpression()
		{
		}

		public MemberExpression(Type type, string expression)
		{
			Type = type;
			Expression = expression;

			//validate expression
			try
			{
				MemberInfos.ToList();
			}
			catch
			{
				throw new ArgumentOutOfRangeException("expression", "Member expression is not valid for " + type.FullName);
			}
		}

		public int Id { get; set; }

		/// <summary>
		/// The type that contains this member
		/// </summary>
		public Type Type { get; set; }

		/// <summary>
		/// String representing the member (property or field) that is being mapped
		/// </summary>
		public string Expression { get; set; }

		//read only properties

		public System.Type ReturnType
		{
			get
			{
				return GetReturnType(FinalMemberInfo);
			}
		}

		/// <summary>
		/// Returns a list of members represented in the member string
		/// </summary>
		/// <param name="memberPath"></param>
		/// <returns></returns>
		/// <remarks>
		/// http://www.java2s.com/Code/CSharp/Reflection/GetPropertyfromPropertypath.htm
		/// </remarks>
		public IEnumerable<MemberInfo> MemberInfos
		{
			get
			{
				return GetMemberInfos(Type, Expression);
			}
		}

		public MemberInfo FinalMemberInfo
		{
			get
			{
				return MemberInfos.Last();
			}
		}

		//methods

		public IEnumerable<object> GetValues(object obj)
		{
			object result = obj;

			foreach (MemberInfo memberInfo in MemberInfos)
			{
				if (result != null)
				{
					result = GetValue(memberInfo, result);
				}

				yield return result;
			}
		}

		public object GetValue(object obj)
		{
			return GetValues(obj).Last();
		}

		public void SetValue(object obj, object value)
		{
			var allMembers = MemberInfos.ToList();
			var allValues = GetValues(obj).ToList();

			//ensure all nested members are not null, except the lastone, that can be null
			for (int i = 0; i < allValues.Count - 1; i++)
			{
				object val = allValues[i];
				MemberInfo member = allMembers[i];

				if (val == null)
				{
					object container = (i == 0) ? obj : allValues[i - 1];
					object newValue = Activator.CreateInstance(GetReturnType(member));
					allValues[i] = newValue;

					SetValue(member, container, newValue);
				}
			}

			object finalContainer = (allValues.Count > 1) ? allValues[allValues.Count - 2] : obj;
			SetValue(FinalMemberInfo, finalContainer, value);
		}

		public override string ToString()
		{
			return Expression;
		}

		#region Static

		public static Type GetReturnType(MemberInfo memberInfo)
		{
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).FieldType;
			}
			else
			{
				return ((PropertyInfo)memberInfo).PropertyType;
			}
		}

		/// <summary>
		/// Returns the value of this DataMember
		/// </summary>
		/// <param name="obj">
		/// Object that will be examined
		/// </param>
		/// <returns>
		/// The value of the current DataMember in the specified Object
		/// </returns>
		public static object GetValue(MemberInfo memberInfo, object obj)
		{
			if (memberInfo is FieldInfo)
			{
				return ((FieldInfo)memberInfo).GetValue(obj);
			}
			else
			{
				return ((PropertyInfo)memberInfo).GetValue(obj);
			}
		}

		/// <summary>
		/// Sets the value for this DataMember
		/// </summary>
		/// <param name="obj">
		/// Object that will be changed
		/// </param>
		/// <param name="value">
		/// The value that will be set to the DataMember
		/// </param>
		public static void SetValue(MemberInfo memberInfo, object obj, object value)
		{
			value = OKHOSTING.Data.Convert.ChangeType(value, GetReturnType(memberInfo));

			if (memberInfo is FieldInfo)
			{
				((FieldInfo)memberInfo).SetValue(obj, value);
			}
			else
			{
				((PropertyInfo)memberInfo).SetValue(obj, value);
			}
		}

		public static void SetValue(string memberExpression, object obj, object value)
		{
			Type type = obj.GetType();
			var allMembers = GetMemberInfos(type, memberExpression).ToList();
			var allValues = new List<object>();
			object result = obj;

			foreach (MemberInfo memberInfo in allMembers)
			{
				if (result != null)
				{
					result = GetValue(memberInfo, result);
				}

				allValues.Add(result);
			}

			//ensure all nested members are not null, except the lastone, that can be null
			for (int i = 0; i < allValues.Count - 1; i++)
			{
				object val = allValues[i];
				MemberInfo member = allMembers[i];

				if (val == null)
				{
					object container = (i == 0) ? obj : allValues[i - 1];
					object newValue = Activator.CreateInstance(GetReturnType(member));
					allValues[i] = newValue;

					SetValue(member, container, newValue);
				}
			}

			object finalContainer = (allValues.Count > 1) ? allValues[allValues.Count - 2] : obj;
			SetValue(allMembers.Last(), finalContainer, value);
		}

		public static bool IsReadOnly(MemberInfo memberInfo)
		{
			//ignore readonly properties
			if (memberInfo is PropertyInfo && ((PropertyInfo) memberInfo).SetMethod == null)
			{
				return true;
			}

			//ignore readonly fields
			if (memberInfo is FieldInfo && (((FieldInfo) memberInfo).IsInitOnly || ((FieldInfo) memberInfo).IsLiteral))
			{
				return true;
			}

			return false;
		}

		public static bool IsCollection(MemberInfo memberInfo)
		{
			return OKHOSTING.Core.TypeExtensions.IsCollection(GetReturnType(memberInfo));
		}

		public static bool IsIndexer(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetIndexParameters().Length > 0;
		}

		/// <summary>
		/// Returns an enumeration of MemberInfos that are interpreted from a string
		/// </summary>
		/// <param name="type">Type declaring the member expression</param>
		/// <param name="memberExpression">A member expression, pe: Address.Country.Name</param>
		public static IEnumerable<MemberInfo> GetMemberInfos(Type type, string memberExpression)
		{
			string[] splittedMembers = memberExpression.Split(new[] { '.' }, StringSplitOptions.None);

			Type memberType = type;

			for (int x = 0; x < splittedMembers.Length; x++)
			{
				MemberInfo memberInfo = memberType.GetAllMemberInfos().Where(m => m.Name == splittedMembers[x].Trim() && (m is PropertyInfo || m is FieldInfo)).FirstOrDefault();

				if (memberInfo == null)
				{
					throw new ArgumentOutOfRangeException("Members", splittedMembers[x], "Type " + memberType + " does not contain a member with that name");
				}

				memberType = GetReturnType(memberInfo);

				yield return memberInfo;
			}
		}

		#endregion
	}

	public class MemberExpression<T>: MemberExpression
	{
		public MemberExpression()
		{
			Type = typeof(T);
		}
	
		public MemberExpression(System.Linq.Expressions.Expression<Func<T, object>> linqExpression)
		{
			Type = typeof(T);
			Expression = GetMemberString(linqExpression);
		}

		//static

		public static string GetMemberString(System.Linq.Expressions.Expression<Func<T, object>> member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}

			var propertyRefExpr = member.Body;
			var memberExpr = propertyRefExpr as System.Linq.Expressions.MemberExpression;

			if (memberExpr == null)
			{
				var unaryExpr = propertyRefExpr as System.Linq.Expressions.UnaryExpression;

				if (unaryExpr != null && unaryExpr.NodeType == System.Linq.Expressions.ExpressionType.Convert)
				{
					memberExpr = unaryExpr.Operand as System.Linq.Expressions.MemberExpression;

					if(memberExpr != null)
					{
						return memberExpr.Member.Name;
					}
				}
			}
			else
			{
				//gets something line "m.Field1.Field2.Field3", from here we just remove the prefix "m."
				string body = member.Body.ToString();
				return body.Substring(body.IndexOf('.') + 1);
			}

			throw new ArgumentException("No property reference expression was found.", "member");
		}

		public static MemberExpression<T> ToGeneric(MemberExpression memberExpression)
		{
			Type genericDataTypeType = typeof(MemberExpression<>).MakeGenericType(memberExpression.Type);
			ConstructorInfo constructor = genericDataTypeType.GetTypeInfo().DeclaredConstructors.Where(c => c.GetParameters().Count() == 0).Single();
			MemberExpression<T> genericMember = (MemberExpression<T>) constructor.Invoke(null);

			genericMember.Expression = memberExpression.Expression;

			return genericMember;
		}
	}
}