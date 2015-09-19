using System;
using System.Linq;
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
			Type[] integralTypes = new Type[] { typeof(Byte), typeof(SByte), typeof(Char), typeof(Int16), typeof(Int32), typeof(Int64), typeof(UInt16), typeof(UInt32), typeof(UInt64) };

			return !type.GetTypeInfo().IsEnum && integralTypes.Contains(type);
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
			Type[] numericTypes = new Type[] { typeof(Byte), typeof(SByte), typeof(Char), typeof(Int16), typeof(Int32), typeof(Int64), typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(Single), typeof(Double), typeof(Decimal) };

			return !type.GetTypeInfo().IsEnum && numericTypes.Contains(type);
		}

		public static bool IsGeneric(this System.Type type)
		{
			return type.GetTypeInfo().IsGenericType || type.IsConstructedGenericType || type.GetTypeInfo().ContainsGenericParameters || type.GetTypeInfo().IsGenericTypeDefinition;
		}

		public static bool IsCollection(this Type type)
		{
			return type.GetTypeInfo().ImplementedInterfaces.Where(i => i.Equals(typeof(System.Collections.IEnumerable))).Count() > 0 && !type.Equals(typeof(System.String));
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

			//find method by comparing method name and parameter types
			var methods = type.GetTypeInfo().GetDeclaredMethods(methodSignature).ToArray();

			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo method = methods[i];
				bool isMatch = true;

				foreach (var paramInfo in method.GetParameters())
				{
					if (paramInfo.ParameterType != parameterTypes[i])
					{
						isMatch = false;
						break;
					}
				}

				if (isMatch)
				{
					return method;
				}
			}

			//return null if no match was found
			return null;
		}

		public static bool IsStruct(this Type type)
		{
			return type.GetTypeInfo().IsValueType && !type.GetTypeInfo().IsEnum;
		}

		public static List<Type> GetAllParents(this Type type)
		{
			Type parent = type;
			List<Type> types = new List<Type>();

			while (parent != null)
			{
				types.Add(parent);
				parent = parent.GetTypeInfo().BaseType;
			}

			return types;
		}
	}
}