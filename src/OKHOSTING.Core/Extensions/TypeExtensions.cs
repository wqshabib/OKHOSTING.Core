using System;
using System.Collections.Generic;
using System.Reflection;

namespace OKHOSTING.Core.Extensions
{
	/// <summary>
	/// Extensions methods for System.Type
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Returns an instance of the type (using parameterless constructor)
		/// </summary>
		/// <param name="type">
		/// Type instance that extends the method
		/// </param>
		/// <returns>
		/// Instance of type
		/// </returns>
		public static object CreateInstance(this Type type)
		{
			return CreateInstance(type, null);
		}

		/// <summary>
		/// Returns an instance of the type using constructor with the specified parameters
		/// </summary>
		/// <param name="type">
		/// Type instance that extends the method
		/// </param>
		/// <param name="args">
		/// Constructor arguments that will be used on object creation
		/// </param>
		/// <returns>
		/// Instance of type
		/// </returns>
		public static object CreateInstance(this Type type, params object[] args)
		{
			if (type.Equals(typeof(string)))
			{
				return string.Empty;
			}
		
			return Activator.CreateInstance(type, args);
		}

		/// <summary>
		/// Returns a boolean value that indicates if the specified 
		/// type is an integer value 
		/// </summary>
		/// <param name="type">
		/// Type to validate
		/// </param>
		/// <returns>
		/// true if type is integer, otherwise false
		/// </returns>
		public static bool IsIntegral(this Type type)
		{
			//Local var
			bool isIntegral = false;

			//Validating if is not a enum
			if (!type.IsEnum)
			{
				//Validating if is numeric
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Byte:
					case TypeCode.Char:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.SByte:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64:
						isIntegral = true;
						break;
				}
			}

			//Returning value
			return isIntegral;
		}

		/// <summary>
		/// Indicates wether the Value is a numeric value, int, decimal, byte, etc.
		/// </summary>
		/// <param name="type">
		/// Type to validate
		/// </param>
		/// <returns>
		/// true if type is numeric, otherwise false
		/// </returns>
		public static bool IsNumeric(this Type type)
		{
			//Local var
			bool isNumeric = false;

			//Validating if is not a enum
			if (!type.IsEnum)
			{
				//Validating if is numeric
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Byte:
					case TypeCode.Char:
					case TypeCode.Decimal:
					case TypeCode.Double:
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.SByte:
					case TypeCode.Single:
					case TypeCode.UInt16:
					case TypeCode.UInt32:
					case TypeCode.UInt64:
						isNumeric = true;
						break;
				}
			}

			//Returning value
			return isNumeric;
		}

		public static bool IsGeneric(this System.Type type)
		{
			return type.IsGenericType || type.IsConstructedGenericType || type.ContainsGenericParameters || type.IsGenericTypeDefinition;
		}

		public static bool IsPersistent(this System.Type type)
		{
			var memberFilters = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

			foreach (var prop in type.GetProperties(memberFilters))
			{
				if (prop.IsDefined(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), false))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsCollection(this Type type)
		{
			return type.GetInterface("IEnumerable") != null && !type.Equals(typeof(string));
		}


		public static PropertyInfo GetKey(this System.Type type)
		{
			var memberFilters = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

			foreach (var prop in type.GetProperties(memberFilters))
			{
				if (prop.IsDefined(typeof(System.ComponentModel.DataAnnotations.KeyAttribute), false))
				{
					return prop;
				}
			}

			return null;
		}

		public static object GetKeyValue(this System.Object obj)
		{
			System.Type type = obj.GetType();

			if (!type.IsPersistent())
			{
				return null;
			}

			var key = type.GetKey();

			return key.GetValue(obj);
		}

		public static bool IsCompilerGenerated(this System.Reflection.MemberInfo memberinfo)
		{
			return memberinfo.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);
		}

		public static MethodInfo GetMethodFromSignature(this Type type, string methodSignature)
		{
			//extract arguments from methodSignature name
			List<Type> parameterTypes = new System.Collections.Generic.List<Type>();

			if (methodSignature.Contains("(") && methodSignature.Contains(")"))
			{
				string _args = methodSignature.Substring(methodSignature.IndexOf('(') + 1);
				_args = _args.TrimEnd(')').Trim();

				if (!string.IsNullOrWhiteSpace(_args))
				{
					string[] _argsList = _args.Split(',');

					foreach (string s in _argsList)
					{
						parameterTypes.Add(Type.GetType(s.Replace(" ", string.Empty).Replace("[", ", ").Replace("]", string.Empty), true));
					}

					//remove arguments from methodSignature name
					methodSignature = methodSignature.Substring(0, methodSignature.IndexOf('('));
				}
				else
				{
					methodSignature = methodSignature.Replace("(", null).Replace(")", null);
				}
			}

			//find methodSignature info
			return type.GetMethod(methodSignature, parameterTypes.ToArray());
		}

		public static bool IsStruct(this Type type)
		{
			return type.IsValueType && !type.IsEnum;
		}

		public static List<Type> GetAllParents(this Type type)
		{
			Type parent = type;
			List<Type> types = new List<Type>();

			while (parent != null)
			{
				types.Add(parent);
				parent = parent.BaseType;
			}

			return types;
		}
	}
}