using System;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// Validate that the specified MemberMap be between 
	/// a Minimum and Maximum values
	/// </summary>
	public class RangeValidator: MemberValidator
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

			//Getting the current value of the associated MemberMap
			IComparable val = (IComparable) Member.GetValue(obj);

			//Comparing the value with the minimum and maximum value
			int resultMin = val.CompareTo(MinValue);
			int resultMax = val.CompareTo(MaxValue);

			//Verifying if the range is fulfilled
			if (resultMin < 0 || resultMax > 0)
			{
				error = new ValidationError(this, "DataProperty value must be between " + MinValue + " and " + MaxValue + " (inclusive)");
			}

			//Returning the applicable error or null
			return error;
		}
	}
}