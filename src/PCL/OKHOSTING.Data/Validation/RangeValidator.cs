using System;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Validate that the specified MemberExpression be between 
	/// a Minimum and Maximum values
	/// </summary>
	public class RangeValidator: ValidatorBase
	{
		public RangeValidator()
		{
		}

		public RangeValidator(IComparable minValue, IComparable maxValue)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		/// <summary>
		/// Minimum value of the allowed range
		/// </summary>
		public IComparable MinValue { get; set; }
		
		/// <summary>
		/// Maximum value of the allowed range
		/// </summary>
		public IComparable MaxValue { get; set; }

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
			IComparable val = (IComparable) obj;

			//Comparing the value with the minimum and maximum value
			int resultMin = val.CompareTo(MinValue);
			int resultMax = val.CompareTo(MaxValue);

			//Verifying if the range is fulfilled
			if (resultMin < 0 || resultMax > 0)
			{
				error = new ValidationError(this, string.Format(Resources.Strings.OKHOSTING_Data_Validation_RangeValidator_Error, MinValue, MaxValue));
			}

			//Returning the applicable error or null
			return error;
		}
	}
}