using System;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Defines a validation based on the comparison between
	/// an absolute value and a DataMember
	/// </summary>
	public class ValueCompareValidator : CompareValidator
	{
		public ValueCompareValidator()
		{
		}

		public ValueCompareValidator(IComparable valueToCompare)
		{
			ValueToCompare = valueToCompare;
		}

		/// <summary>
		/// Value used on the comparison
		/// </summary>
		public IComparable ValueToCompare { get; set; }

		/// <summary>
		/// Performs the validation
		/// </summary>
		/// <returns>
		/// ValidationError object with the error founded if the validation fails,
		/// otherwise returns null
		/// </returns>
		public override ValidationError Validate(object obj)
		{
			//Validating
			return base.Validate((IComparable) obj, ValueToCompare);
		}
	}
}