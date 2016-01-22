using System;
using System.Linq;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Validate if a Property of Field on a Class can be null
	/// </summary>
	public class RequiredValidator : ValidatorBase
	{
		/// <summary>
		/// Constructs the attribute
		/// </summary>
		public RequiredValidator()
		{
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

			//Recover the value of MemberExpression associated
			object currentValue = obj;

			//Validating if the value is null
			if (currentValue == null)
			{
				error = new ValidationError(this, Resources.Strings.OKHOSTING_Data_Validation_RequiredValidator_Errors_Object);
			}
			else if (OKHOSTING.Core.TypeExtensions.IsNumeric(obj.GetType()))
			{
				if (System.Convert.ToInt64(currentValue) == 0)
				{
					error = new ValidationError(this, Resources.Strings.OKHOSTING_Data_Validation_RequiredValidator_Errors_Numeric);
				}
			}
			//if this is a string, do not allow null nor empty values
			else if (obj is string)
			{
				if (string.IsNullOrWhiteSpace((string) currentValue))
				{
					error = new ValidationError(this, Resources.Strings.OKHOSTING_Data_Validation_RequiredValidator_Errors_String);
				}
			}

			//Returning the error or null
			return error;
		}

		public static bool IsRequired(System.Reflection.MemberInfo member)
		{
			//Validating if the MemberInfo is null
			if (member == null) throw new ArgumentNullException("member");

			//Recovering the attributes of type DataMemberAttribute declared in the MemberInfo
			return member.CustomAttributes.Where(att => att.AttributeType.Name.StartsWith("Required")).Count() > 0;
		}

		public static bool HasValue(object value)
		{
			//Validating if the value is null
			if (value == null)
			{
				return false;
			}

			else if (OKHOSTING.Core.TypeExtensions.IsNumeric(value.GetType()))
			{
				if (System.Convert.ToInt64(value) == 0)
				{
					return false;
				}
			}

			//if this is a string, do not allow null nor empty values
			else if (value.GetType().Equals(typeof(string)))
			{
				if (string.IsNullOrWhiteSpace((string) value))
				{
					return false;
				}
			}

			return true;
		}
	}
}