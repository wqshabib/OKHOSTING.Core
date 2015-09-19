using System;
using System.Linq;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// Validate if a Property of Field on a Class can be null
	/// </summary>
	public class RequiredValidator : MemberValidator
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

			//Recover the value of MemberMap associated
			object currentValue = Member.GetValue(obj);

			//Validating if the value is null
			if (currentValue == null)
			{
				error = new ValidationError(this, Member + " cannot be null");
			}
			else if (OKHOSTING.Core.TypeExtensions.IsNumeric(Member.ReturnType))
			{
				if (System.Convert.ToInt64(currentValue) == 0)
				{
					error = new ValidationError(this, Member + " cannot be zero");
				}
			}
			//if this is a string, do not allow null nor empty values
			else if (Member.ReturnType.Equals(typeof(string)))
			{
				if (string.IsNullOrWhiteSpace((string)currentValue))
				{
					error = new ValidationError(this, Member + " cannot be empty");
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
			
			//else if (OKHOSTING.Core.Extensions.TypeExtensions.IsNumeric(value.GetType()))
			//{
			//	if (Convert.ToInt64(value) == 0)
			//	{
			//		return false;
			//	}
			//}

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