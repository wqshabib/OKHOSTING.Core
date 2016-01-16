using System;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Defines a validation based on the comparison between
	/// two MemberExpression's
	/// </summary>
	public class MemberCompareValidator : CompareValidator
	{
		public MemberCompareValidator()
		{
		}

		public MemberCompareValidator(MemberExpression member, MemberExpression memberToCompare)
		{
			Member = member;
			MemberToCompare = memberToCompare;
		}

		/// <summary>
		/// MemberExpression to compare with the MemberToCompare
		/// </summary>
		public MemberExpression Member { get; set; }

		/// <summary>
		/// MemberExpression to compare with the MemberExpression of the validator
		/// </summary>
		public MemberExpression MemberToCompare { get; set; }

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

			//Converting the value to an IComparable interface
			IComparable memberValue = (IComparable) Member.GetValue(obj);
			IComparable memberToCompareValue = (IComparable) MemberToCompare.GetValue(obj);

			//Validating
			error = base.Validate(memberValue, memberToCompareValue);
			
			//Returning the applicable error or null...
			return error;
		}
	}
}