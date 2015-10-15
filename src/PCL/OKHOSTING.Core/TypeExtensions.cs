using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace OKHOSTING.Core
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
			return type.GetTypeInfo().IsGeneric();
		}

		public static bool IsGeneric(this TypeInfo type)
		{
			return type.IsGenericType || type.AsType().IsConstructedGenericType || type.ContainsGenericParameters || type.IsGenericTypeDefinition;
		}

		/// <summary>
		/// Returns all implemented interfaces on a type, including those inherited from parent types
		/// </summary>
		public static IEnumerable<Type> GetAllImplementedInterfaces(this Type type)
		{
			while (type != null)
			{
				foreach (Type i in type.GetTypeInfo().ImplementedInterfaces)
				{
					yield return i;
				}

				type = type.GetTypeInfo().BaseType;
			}
		}

		/// <summary>
		/// Returns a boolean indicating if a type (or any of it's parent types) implements IEnumerable
		/// </summary>
		public static bool IsCollection(this Type type)
		{
			return type.GetAllImplementedInterfaces().Where(i => i.Equals(typeof(System.Collections.IEnumerable))).Count() > 0;
		}

		/// <summary>
		/// Returns the type of elements that a collection can contain
		/// </summary>
		/// <param name="type">Collection type, that implements IEnumerable</param>
		/// <returns>The type of the elements that this collection contanis</returns>
		/// <example>
		/// If type is string[], will return string.
		/// If type is IEnumerable<bool>, will return bool.
		/// </example>
		public static Type GetCollectionItemType(this Type type)
		{
			if (!type.IsCollection())
			{
				throw new ArgumentOutOfRangeException("type", "Type is not a collection");
			}

			if (type.IsArray)
			{
				return type.GetElementType();
			}

			if (type.IsGeneric())
			{
				if (!type.IsConstructedGenericType)
				{
					throw new ArgumentOutOfRangeException("type", "Type is not a constructed generic type");
				}

				return type.GetTypeInfo().GenericTypeArguments.First();
			}
			else
			{
				return typeof(object);
			}
		}

		public static bool IsCompilerGenerated(this MemberInfo memberinfo)
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

		public static IEnumerable<Type> GetAllParents(this Type type)
		{
			Type parent = type;

			while (parent != null)
			{
				yield return parent;
				parent = parent.GetTypeInfo().BaseType;
			}
		}

		/// <summary>
		/// Returns a collection of all member infos, including those inherited by parent types
		/// </summary>
		public static IEnumerable<MemberInfo> GetAllMemberInfos(this Type type)
		{
			Type parent = type;

			while (parent != null)
			{
				foreach (MemberInfo member in parent.GetTypeInfo().DeclaredMembers)
				{
					yield return member;
				}

				parent = parent.GetTypeInfo().BaseType;
			}
		}

		public static string GetFriendlyName(this Type type)
		{
			if (type.IsGenericParameter)
			{
				return type.Name;
			}

			if (!type.IsGeneric())
			{
				return type.Name;
			}

			var builder = new System.Text.StringBuilder();
			var name = type.Name;
			var index = name.IndexOf("`");
			builder.Append(name.Substring(0, index));
			builder.Append('<');
			var first = true;

			foreach (Type arg in type.GetTypeInfo().GenericTypeArguments)
			{
				if (!first)
				{
					builder.Append(',');
				}

				builder.Append(arg.GetFriendlyName());
				first = false;
			}

			builder.Append('>');

			return builder.ToString();
		}

		public static string GetFriendlyFullName(this Type type)
		{
			return type.Namespace + "." + type.GetFriendlyName();
		}
	}
}