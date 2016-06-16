using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace OKHOSTING.Core
{
	/// <summary>
	/// Extensions methods for System.Type
    /// <para xml:lang="es">
    /// Extension de metodos para System.Type
    /// </para>
	/// </summary>
	public static class TypeExtensions
	{
        /// <summary>
        /// Returns an instance of the type (using parameterless constructor)
        /// <para xml:lang="es">
        /// Devuelve una instancia del tipo (utilizando constructor sin parámetros)
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type instance that extends the method
        /// <para xml:lang="es">
        /// Tipo de instancia que se extiende el método
        /// </para>
        /// </param>
        /// <returns>
        /// Instance of type
        /// <para xml:lang="es">
        /// Instancia del tipo
        /// </para>
        /// </returns>
        public static object CreateInstance(this Type type)
		{
			return CreateInstance(type, null);
		}

        /// <summary>
        /// Returns an instance of the type using constructor with the specified parameters
        /// <para xml:lang="es">
        /// Devuelve una instancia del tipo que utiliza el constructor con los parámetros especificados
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type instance that extends the method
        /// <para xml:lang="es">
        /// Tipo de instancia que extiende el metodo
        /// </para>
        /// </param>
        /// <param name="args">
        /// Constructor arguments that will be used on object creation
        /// <para xml:lang="es">
        /// argumentos de constructor que se utilizarán en la creación de objetos
        /// </para>
        /// </param>
        /// <returns>
        /// Instance of type
        /// <para xml:lang="es">
        /// Instancia del tipo
        /// </para>
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
        /// <para xml:lang="es">
        /// Devuelve un valor booleano que indica si el tipo especificado
        /// es un valor entero
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// true if type is integer, otherwise false
        /// <para xml:lang="es">
        /// verdadero si el tipo es entero, de lo contrario falso
        /// </para>
        /// </returns>
        public static bool IsIntegral(this Type type)
		{
			Type[] integralTypes = new Type[] { typeof(Byte), typeof(SByte), typeof(Char), typeof(Int16), typeof(Int32), typeof(Int64), typeof(UInt16), typeof(UInt32), typeof(UInt64) };

			return !type.GetTypeInfo().IsEnum && integralTypes.Contains(type);
		}

        /// <summary>
        /// Indicates wether the Value is a numeric value, int, decimal, byte, etc.
        /// <para xml:lang="es">
        /// Indica si el valor es un valor numérico, en decimal, byte, etc.
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// true if type is numeric, otherwise false
        /// <para xml:lang="es">
        /// verdadero si el tipo es numérico, de lo contrario falso
        /// </para>
        /// </returns>
        public static bool IsNumeric(this Type type)
		{
			Type[] numericTypes = new Type[] { typeof(Byte), typeof(SByte), typeof(Char), typeof(Int16), typeof(Int32), typeof(Int64), typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(Single), typeof(Double), typeof(Decimal) };

			return !type.GetTypeInfo().IsEnum && numericTypes.Contains(type);
		}

        /// <summary>
        /// Returns a boolean value that indicates if the specified 
        /// type is an generic value
        /// <para xml:lang="es">
        /// Devuelve un valor booleano que indica si el tipo especificado
        /// es un valor generico
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// true if type is generic, otherwise false
        /// <para xml:lang="es">
        /// verdadero si el tipo es generico, de lo contrario falso
        /// </para>
        /// </returns>
		public static bool IsGeneric(this System.Type type)
		{
			return type.GetTypeInfo().IsGeneric();
		}

        /// <summary>
        /// Returns a boolean value that indicates if the specified 
        /// type is an generic value
        /// <para xml:lang="es">
        /// Devuelve un valor booleano que indica si el tipo especificado
        /// es un valor generico
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// true if type is generic, otherwise false
        /// <para xml:lang="es">
        /// verdadero si el tipo es generico, de lo contrario falso
        /// </para>
        /// </returns>
        public static bool IsGeneric(this TypeInfo type)
		{
			return type.IsGenericType || type.AsType().IsConstructedGenericType || type.ContainsGenericParameters || type.IsGenericTypeDefinition;
		}

        /// <summary>
        /// Returns all implemented interfaces on a type, including those inherited from parent types
        /// <para xml:lang="es">
        /// Devuelve todas las interfaces implementadas en un tipo, incluyendo las heredadas de tipos de padres
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// Returns all implemented interfaces on a type
        /// <para xml:lang="es">
        /// Devuelve todas las interfaces implementadas en un tipo
        /// </para>
        /// </returns>
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
        /// <para xml:lang="es">
        /// Devuelve un valor booleano que indica si un tipo (o cualquiera de sus tipos de padres) implementa IEnumerable
        /// </para>
        /// </summary>
        /// <param name="type">
        /// type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// true if type is a collection, otherwise false
        /// <para xml:lang="es">
        /// verdadero si el tipo es una coleccion, de lo contrario falso
        /// </para>
        /// </returns>
        public static bool IsCollection(this Type type)
		{
			return type.GetAllImplementedInterfaces().Where(i => i.Equals(typeof(System.Collections.IEnumerable))).Any();
		}

        /// <summary>
        /// Returns the type of elements that a collection can contain
        /// <para xml:lang="es">
        /// Devuelve el tipo de elementos que puede contener una colección
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Collection type, that implements IEnumerable
        /// <para xml:lang="es">
        /// tipo de colección, que implementa IEnumerable
        /// </para>
        /// </param>
        /// <returns>
        /// The type of the elements that this collection contanis
        /// <para xml:lang="es">
        /// El tipo de los elementos que contiene esta colección
        /// </para>
        /// </returns>
        /// <example>
        /// If type is string[], will return string.
        /// If type is IEnumerable<bool>, will return bool.
        /// If type is KeyValuePair<int, bool>, will return bool.
        /// <para xml:lang="es">
        /// Si el tipo es string[] devuelve string.
        /// Si el tipo es IEnumerable<bool>, devolvera bool.
        /// Si el tipo es KeyValuePair<int, bool>, devolvera bool
        /// </para>
        /// </example>
        public static Type GetCollectionItemType(this Type type)
		{
			if (!type.IsCollection())
			{
				throw new ArgumentOutOfRangeException("type", "Type is not a collection");
			}
			else if (type.IsArray)
			{
				return type.GetElementType();
			}
			else if (type.IsGeneric())
			{
				if (!type.IsConstructedGenericType)
				{
					throw new ArgumentOutOfRangeException("type", "Type is not a constructed generic type");
				}

				Type itemType = type.GetTypeInfo().GenericTypeArguments.Single();

				if (itemType.GetTypeInfo().IsSubclassOf(typeof(KeyValuePair<,>)))
				{
					return itemType.GetTypeInfo().GenericTypeArguments.Last();
				}
				else
				{
					return itemType;
				}
			}
			else
			{
				return typeof(object);
			}
		}

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="memberinfo">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		public static bool IsCompilerGenerated(this MemberInfo memberinfo)
		{
			return memberinfo.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);
		}

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="type">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <param name="methodSignature">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
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

        /// <summary>
        /// Returns a boolean value that indicates if the specified 
        /// type is an struct value
        /// <para xml:lang="es">
        /// Devuelve un valor booleano que indica si el tipo especificado
        /// es una estructura
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// true if type is struct, otherwise false
        /// <para xml:lang="es">
        /// verdadero si el tipo es un struct, de lo contrario false
        /// </para>
        /// </returns>
		public static bool IsStruct(this Type type)
		{
			return type.GetTypeInfo().IsValueType && !type.GetTypeInfo().IsEnum;
		}

        /// <summary>
        /// Get All Parents of type evaluated
        /// <para xml:lang="es">
        /// Obteniene todos los padres del tipo evaluado
        /// </para>
        /// </summary>
        /// <param name="type">
        /// type evaluated
        /// <para xml:lang="es">
        /// Tipo evaluado
        /// </para>
        /// </param>
        /// <returns>
        /// Return All Parents of collection
        /// <para xml:lang="es">
        /// Devuelve todos los padres de la coleccion
        /// </para>
        /// </returns>
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
        /// <para xml:lang="es">
        /// Devuelve una colección de todas las informaciones sobre miembros, incluidos los heredados por los tipos de padres
        /// </para>
        /// </summary>
        /// <param name="type">
        /// Type to validate
        /// <para xml:lang="es">
        /// Tipo a validar
        /// </para>
        /// </param>
        /// <returns>
        /// Returns a collection of all member infos
        /// <para xml:lang="es">
        /// Devuelve una colección de todas las informaciones sobre miembros
        /// </para>
        /// </returns>
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

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="type">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
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

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="type">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		public static string GetFriendlyFullName(this Type type)
		{
			return type.Namespace + "." + type.GetFriendlyName();
		}

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="member">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		public static string GetFriendlyName(this MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException(nameof(member));
			}

			if (member is MethodInfo)
			{
				return GetFriendlyName((MethodInfo) member);
			}
			else
			{
				return member.Name;
			}
		}

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="member">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		public static string GetFriendlyFullName(this MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException(nameof(member));
			}

			if (member is MethodInfo)
			{
				return GetFriendlyFullName((MethodInfo) member);
			}
			else
			{
				return member.Name;
			}
		}

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="method">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		public static string GetFriendlyName(this MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}

			//get full method signature
			string signature = method.ToString();

			//remove return type
			signature = signature.Substring(signature.IndexOf(' '));

			//remove blank spaces
			signature = signature.Replace(" ", "");

			return signature;
		}

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="method">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		public static string GetFriendlyFullName(this MethodInfo method)
		{
			return method.DeclaringType.GetFriendlyFullName() + "." + method.GetFriendlyName();
		}
	}
}