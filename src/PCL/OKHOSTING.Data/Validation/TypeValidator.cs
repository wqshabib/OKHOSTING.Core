using System;
using System.Reflection;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Validates that a MemberExpression of type TypeMap contains a TypeMap 
	/// that is a specific TypeMap or a subclass of it.
	/// </summary>
	/// <example>
	/// Use this attribute on a DataMembers od type TypeMap where you want the TypeMap to be a subclass of a specific TypeMap only.
	/// PE: you have a MemberExpression Product.ProductInstanceType where you want the selected TypeMap to be ProductInstance or a subclass of ProductInstance only
	/// </example>
	/// <remarks>
	/// Applies only DataMembers of type TypeMap
	/// </remarks>
	public class TypeValidator : ValidatorBase
	{
		/// <summary>
		/// The TypeMap (or a subclass of it) that must be selected as a value of the MemberExpression
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
		/// Type (or any subclass of it) that can be selected as the Value of the MemberExpression
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

			//Getting the current value of the associated MemberExpression
			Type val = (Type) obj;

			//Do not validate null values
			if (val == null) return null;

			//Verifying if the value is equal to the Parent TypeMap or is a subclass of it
			if (!val.Equals(Type) || !val.GetTypeInfo().IsSubclassOf(Type))
			{
				error = new ValidationError(this, string.Format(Resources.Strings.OKHOSTING_Data_Validation_TypeValidator_Error, val, Type));
			}

			//Returning the applicable error or null
			return error;
		}
	}
}