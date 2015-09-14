using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// A Type that is configured to have validators
	/// </summary>
	public class DataType
	{
		public DataType()
		{
		}

		public DataType(Type innerType)
		{
			if (innerType == null)
			{
				throw new ArgumentNullException("innerType");
			}

			InnerType = innerType;
		}

		#region Properties

		public readonly List<ValidatorBase> Validators = new List<ValidatorBase>();

		/// <summary>
		/// System.Type in wich this TypeMap<T> is created from
		/// </summary>
		public virtual System.Type InnerType { get; set; }

		//read only properties

		public virtual DataType BaseDataType
		{
			get
			{
				Type current;

				//Get all types in ascendent order (from base to child)
				current = this.InnerType.BaseType;

				while (current != null)
				{
					//see if this type is mapped
					if (IsMapped(current))
					{
						return current;
					}

					//Getting the parent of the current object
					current = current.BaseType;
				}

				return null;
			}
		}

		/// <summary>
		/// Returns all DataMembers, including those inherited from base classes. 
		/// </summary>
		/// <remarks>
		/// Does not duplicate the primary key by omitting base classes primary keys
		/// </remarks>
		public virtual IEnumerable<ValidatorBase> AllValidators
		{
			get
			{
				foreach (DataType parent in GetBaseDataTypes())
				{
					foreach (ValidatorBase validator in parent.Validators)
					{
						yield return validator;
					}
				}
			}
		}

		#endregion

		#region Methods

		public virtual IEnumerable<ValidationError> Validate(object obj)
		{
			List<ValidationError> errors = new List<ValidationError>();

			foreach (var validator in Validators)
			{
				var error = validator.Validate(obj);

				if (error != null)
				{
					//errors.Add(error);
					yield return error;
				}
			}

			//if (errors.Count > 0)
			//{
			//	throw new ValidationException(errors, obj);
			//}
		}
 
		public virtual IEnumerable<DataType> GetBaseDataTypes()
		{
			Type current;

			//Get all types in ascendent order (from base to child)
			current = this.InnerType;

			while (current != null)
			{
				//see if this type is mapped
				if (IsMapped(current))
				{
					yield return current;
				}

				//Getting the parent of the current object
				current = current.BaseType;
			}
		}

		/// <summary>
		/// Searches for all TypeMaps inherited from this TypeMap 
		/// </summary>
		/// <returns>
		/// All TypeMap that directly inherit from the current TypeMap
		/// </returns>
		public virtual IEnumerable<DataType> GetSubDataTypes()
		{
			return DataTypes.Where(d => d.BaseDataType != null && d.BaseDataType.Equals(this));
		}

		/// <summary>
		/// Searches for all DataTypes inherited from this TypeMap in a recursive way
		/// </summary>
		/// <returns>
		/// All DataTypes that directly and indirectly inherit from the current TypeMap. 
		/// Returns the hole tree of subclasses.
		/// </returns>
		public virtual IEnumerable<DataType> GetSubDataTypesRecursive()
		{
			//Crossing all loaded DataTypes
			foreach (DataType subType in DataTypes.Where(d => d.BaseDataType != null && d.BaseDataType.Equals(this)))
			{
				yield return subType;

				foreach (var subType2 in subType.GetSubDataTypesRecursive())
				{
					yield return subType2;
				}
			}
		}

		/// <summary>
		/// Returns a string representation of this TypeMapping
		/// </summary>
		public override string ToString()
		{
			return InnerType.FullName;
		}

		#endregion

		#region Equality

		/// <summary>
		/// Compare this TypeMap<T>'s instance with another to see if they are the same
		/// </summary>
		public virtual bool Equals(DataType typeMap)
		{
			//Validating if the argument is null
			if (typeMap == null) throw new ArgumentNullException("typeMap");

			//Comparing the InnerType types 
			return this.InnerType.Equals(typeMap.InnerType);
		}

		/// <summary>
		/// Compare this Type's instance with another to see if they are the same
		/// </summary>
		public virtual bool Equals(Type type)
		{
			//Validating if the argument is null
			if (type == null) throw new ArgumentNullException("type");

			//Comparing the InnerType types 
			return this.InnerType.Equals(type);
		}

		/// <summary>
		/// Compare this Type's instance with another to see if they are the same
		/// </summary>
		public override bool Equals(object obj)
		{
			//Validating if the argument is null
			if (obj == null) throw new ArgumentNullException("obj");

			//Validating if the argument is a System.Type
			if (obj is Type)
			{
				return this.Equals((Type)obj);
			}
			//Validating if the argument is a DataType
			else if (obj is DataType)
			{
				return this.Equals((DataType)obj);
			}
			else
			{
				//The object is not a TypeMap<T> and is not a System.Type
				return false;
			}
		}

		/// <summary>
		/// Serves as a hash function for DataTypes
		/// </summary>
		/// <remarks>Returns the InnerType.GetHashCode() value</remarks>
		public override int GetHashCode()
		{
			return InnerType.GetHashCode();
		}

		/// <summary>
		/// Determines whether an instance of the current 
		/// TypeMap<T> is assignable from another TypeMap<T>
		/// </summary>
		public virtual bool IsAssignableFrom(DataType typeMap)
		{
			//Validating if the argument is null
			if (typeMap == null) throw new ArgumentNullException("typeMap");

			//Validating...
			return this.InnerType.IsAssignableFrom(typeMap.InnerType);
		}

		#endregion

		#region Static

		/// <summary>
		/// List of available type mappings, system-wide
		/// </summary>
		public static readonly List<DataType> DataTypes = new List<DataType>();

		public static bool IsMapped(Type type)
		{
			return DataTypes.Where(m => m.InnerType.Equals(type)).Count() > 0;
		}

		public static DataType GetMap(Type type)
		{
			return DataTypes.Where(m => m.InnerType.Equals(type)).Single();
		}

		public static implicit operator DataType(Type type)
		{
			return GetMap(type);
		}

		/// <summary>
		/// Returns a collection of members that are mappable, 
		/// meaning they are fields or properties, public, non read-only, and non-static
		/// </summary>
		protected static IEnumerable<System.Reflection.MemberInfo> GetMapableMembers(Type type)
		{
			foreach (var memberInfo in type.GetMembers(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				//ignore methods, events and other members
				if (!(memberInfo is System.Reflection.FieldInfo || memberInfo is System.Reflection.PropertyInfo))
				{
					continue;
				}

				//ignore readonly properties and fields
				if (DataMember.IsReadOnly(memberInfo))
				{
					continue;
				}

				yield return memberInfo;
			}
		}

		#endregion
	}

	/// <summary>
	/// A Type that is mapped to a database Table
	/// </summary>
	public class DataType<T> : DataType
	{
		public DataType(): base(typeof(T))
		{
		}

		public new readonly List<Validation.ValidatorBase<T>> Validators = new List<Validation.ValidatorBase<T>>();

		/// <summary>
		/// Creates a new DataMember using the provided expression
		/// </summary>
		public DataMember<T> this[System.Linq.Expressions.Expression<Func<T, object>> expression]
		{
			get
			{
				return new DataMember<T>(expression);
			}
		}

		public IEnumerable<ValidationError> Validate(T obj)
		{
			return base.Validate(obj);
		}

		#region Static

		public static DataType<T> ToGeneric(DataType dtype)
		{
			Type genericDataTypeType = typeof(DataType<>).MakeGenericType(dtype.InnerType);
			
			ConstructorInfo constructor = genericDataTypeType.GetConstructor(
			  System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Instance,
			  null,
			  null,
			  null
			);

			DataType<T> genericDataType = (DataType<T>)constructor.Invoke(null);

			DataTypes.Add(genericDataType);

			return genericDataType;
		}

		#endregion
	}
}