using System;
using OKHOSTING.Core.Data;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// Indicates if a Property of Field of type string, must have 
	/// an specific length
	/// </summary>
	/// <remarks>Applies only to string DataValues</remarks>
	public class StringLengthValidator: MemberValidator
	{
		public StringLengthValidator()
		{
		}

		public StringLengthValidator(uint maxLength)
		{
			MaxLength = maxLength;
		}

		public StringLengthValidator(uint minLength, uint maxLength)
		{
			MinLength = minLength;
			MaxLength = maxLength;
		}

		/// <summary>
		/// Specify the maximum length that a string can contain
		/// </summary>
		public uint MaxLength { get; set; }

		/// <summary>
		/// Specify the minimum length that a string can contain
		/// </summary>
		public uint MinLength { get; set; }

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

			//Getting the value of the MemberMap
			string currentValue = (string) Member.GetValue(obj);

			//if it's null, and we dont have a MinLenght, omit validation
			if (currentValue == null && MinLength > 0)
			{
				error = new ValidationError(this, "String can't be an empty string on field " + Member);
			}

			//Perform the applicable validation

			if (MaxLength != 0 && currentValue.Length > MaxLength)
				error = new ValidationError(this, "String length must not be greater than " + MaxLength + " on field " + Member);
				
			if (MinLength != 0 && currentValue.Length < MinLength)
				error = new ValidationError(this, "String length must be greater than " + MinLength + " on field " + Member);

			//Returning the error or null
			return error;
		}

		/// <summary>
		/// Gets the max lenght of a string DataValue
		/// </summary>
		/// <param name="dmember">String DataValue that has a StringLengthValidator attribute</param>
		/// <returns>Maximum lenght of the string DataValue. 0 if no max lenght is defined.</returns>
		public static uint	GetMaxLenght(System.Reflection.MemberInfo member)
		{
			//Validating if the MemberInfo is null
			if (member == null) throw new ArgumentNullException("member");

			//Recovering the attributes of type DataMemberAttribute declared in the MemberInfo
			object[] attributes = member.GetCustomAttributes(typeof(StringLengthValidator), false);

			foreach (StringLengthValidator validator in attributes)
			{
				return validator.MaxLength;
			}

			//try with StringLengthAttribute
			attributes = member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false);

			foreach (System.ComponentModel.DataAnnotations.StringLengthAttribute validator in attributes)
			{
				return (uint) validator.MaximumLength;
			}

			//if no attribute was found, return the default 255 lenght for varchar strings
			return 255;
		}

		/// <summary>
		/// Gets the min lenght of a string DataValue
		/// </summary>
		/// <param name="dmember">String DataValue that has a StringLengthValidator attribute</param>
		/// <returns>Maximum lenght of the string DataValue. Null if no max lenght is defined.</returns>
		public static uint GetMinLenght(System.Reflection.MemberInfo member)
		{
			//Validating if the MemberInfo is null
			if (member == null) throw new ArgumentNullException("member");

			//Recovering the attributes of type DataMemberAttribute declared in the MemberInfo
			object[] attributes = member.GetCustomAttributes(typeof(StringLengthValidator), false);

			foreach (StringLengthValidator validator in attributes)
			{
				return validator.MinLength;
			}

			//try with StringLengthAttribute
			attributes = member.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.StringLengthAttribute), false);

			foreach (System.ComponentModel.DataAnnotations.StringLengthAttribute validator in attributes)
			{
				return (uint) validator.MinimumLength;
			}

			//if operator is not one of the previous, return null
			return 0;
		}

		/// <summary>
		/// Use this value (zero) to specify no lenght limit
		/// </summary>
		public const uint Unlimited = 0;
	}
}