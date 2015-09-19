using System;
using System.Reflection;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// Validates that a MemberMap of type TypeMap contains a TypeMap 
	/// that is a specific TypeMap or a subclass of it.
	/// </summary>
	/// <example>
	/// Use this attribute on a DataValues od type TypeMap where you want the TypeMap to be a subclass of a specific TypeMap only.
	/// PE: you have a MemberMap Product.ProductInstanceType where you want the selected TypeMap to be ProductInstance or a subclass of ProductInstance only
	/// </example>
	/// <remarks>
	/// Applies only DataValues of type TypeMap
	/// </remarks>
	public class TypeValidator : MemberValidator
	{
		/// <summary>
		/// The TypeMap (or a subclass of it) that must be selected as a value of the MemberMap
		/// </summary>
		public readonly Type Type;

		/// <summary>
		/// Construct the validator
		/// </summary>
		public TypeValidator()
		{
		}

		/// <summary>
		/// Construct the validator
		/// </summary>
		/// <param name="type">
		/// Type (or any subclass of it) that can be selected as the Value of the MemberMap
		/// </param>
		public TypeValidator(Type type)
		{
			Type = type;
		}

		/// <summary>
		/// Performs the validation
		/// </summary>
		/// <returns>
		/// ValidationError object with the error founded if the validation fails,
		/// otherwise returns null
		/// </returns>
		public override ValidationError Validate(object obj)
		{
			//Local Vars
			ValidationError error = null;

			//Getting the current value of the associated MemberMap
			Type val = (Type) Member.GetValue(obj);

			//Do not validate null values
			if (val == null) return null;

			//Verifying if the value is equal to the Parent TypeMap or is a subclass of it
			if (!val.Equals(Type) || !val.GetTypeInfo().IsSubclassOf(Type))
			{
				error = new ValidationError(this, "Type " + val + " is not equal or a subclass of " + Type);
			}

			//Returning the applicable error or null
			return error;
		}
	}
}