using System;
using System.Linq;

namespace OKHOSTING.Data.Validation
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
		public static uint GetMaxLenght(System.Reflection.MemberInfo member)
		{
			//Validating if the MemberInfo is null
			if (member == null) throw new ArgumentNullException("member");

			//Recovering the attributes of type DataMemberAttribute declared in the MemberInfo
			var stringLengthValidator = member.CustomAttributes.Where(att => att.AttributeType == typeof(StringLengthValidator)).SingleOrDefault();

			if (stringLengthValidator != null)
			{
				int argNumber = stringLengthValidator.ConstructorArguments.Count();

				if (argNumber == 0)
				{
					//no constructor arguments where given so we look in namedarguments
					var max = stringLengthValidator.NamedArguments.Where(na => na.MemberName == "maxLength").SingleOrDefault();

					if (max.MemberName == "maxLength")
					{
						return (uint) max.TypedValue.Value;
					}
				}
				else if (argNumber == 1)
				{
					return (uint) stringLengthValidator.ConstructorArguments.First().Value;
				}
				else if (argNumber == 2)
				{
					return (uint) stringLengthValidator.ConstructorArguments.Last().Value;
				}
			}

			//if no attribute was found, return the default 0
			return 0;
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
			var stringLengthValidator = member.CustomAttributes.Where(att => att.AttributeType == typeof(StringLengthValidator)).SingleOrDefault();

			if (stringLengthValidator != null)
			{
				int argNumber = stringLengthValidator.ConstructorArguments.Count();

				if (argNumber == 0)
				{
					//no constructor arguments where given so we look in namedarguments
					var min = stringLengthValidator.NamedArguments.Where(na => na.MemberName == "minLength").SingleOrDefault();

					if (min.MemberName == "minLength")
					{
						return (uint) min.TypedValue.Value;
					}
				}
				else if (argNumber == 1 || argNumber == 2)
				{
					return (uint)stringLengthValidator.ConstructorArguments.First().Value;
				}
			}

			//if no attribute was found, return the default 0
			return 0;
		}

		/// <summary>
		/// Use this value (zero) to specify no lenght limit
		/// </summary>
		public const uint Unlimited = 0;
	}
}