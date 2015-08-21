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
		/// <summary>
		/// Specify the maximum length that a string can contain
		/// </summary>
		public int MaxLength { get; set; }

		/// <summary>
		/// Specify the minimum length that a string can contain
		/// </summary>
		public int MinLength { get; set; }

		/// <summary>
		/// Defines if an string.Empty value is valid
		/// </summary>
		public bool AllowEmpty { get; set; }

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

			//if it's null, omit validation
			if (currentValue == null) return null;

			//Perform the applicable validation

			if (MaxLength != 0 && currentValue.Length > MaxLength)
				error = new ValidationError(this, "String length must not be greater than " + MaxLength + " on field " + Member);
				
			if (MinLength != 0 && currentValue.Length < MinLength)
				error = new ValidationError(this, "String length must be greater than " + MinLength + " on field " + Member);
					

			//Validating if the string.Empty value is a valid value (only if dont exists errors))
			if (error == null && !this.AllowEmpty)
			{
				if (currentValue.Trim() == string.Empty)
					error = new ValidationError(this, "String can't be an empty string on field " + Member);
			}

			//Returning the error or null
			return error;
		}

		/// <summary>
		/// Gets the max lenght of a string DataValue
		/// </summary>
		/// <param name="dmember">String DataValue that has a StringLengthValidator attribute</param>
		/// <returns>Maximum lenght of the string DataValue. 0 if no max lenght is defined.</returns>
		public static int	GetMaxLenght(System.Reflection.MemberInfo member)
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
				return validator.MaximumLength;
			}

			//if no attribute was found, return the default 255 lenght for varchar strings
			return 255;
		}

		/// <summary>
		/// Gets the min lenght of a string DataValue
		/// </summary>
		/// <param name="dmember">String DataValue that has a StringLengthValidator attribute</param>
		/// <returns>Maximum lenght of the string DataValue. Null if no max lenght is defined.</returns>
		public static int GetMinLenght(System.Reflection.MemberInfo member)
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
				return validator.MinimumLength;
			}

			//if operator is not one of the previous, return null
			return 0;
		}
	}
}