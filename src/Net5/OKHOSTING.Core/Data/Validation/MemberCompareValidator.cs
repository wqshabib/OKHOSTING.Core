using System;
using OKHOSTING.Core.Data;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// Defines a validation based on the comparison between
	/// two MemberMap's
	/// </summary>
	public class MemberCompareValidator : CompareValidator
	{
		public MemberCompareValidator()
		{
		}

		public MemberCompareValidator(MemberExpression member, MemberExpression memberToCompare): base(member)
		{
			MemberToCompare = memberToCompare;
		}

		/// <summary>
		/// MemberMap to compare with the MemberMap of the validator
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

			//Validating
			error = base.Validate(obj, memberValue);
			
			//Returning the applicable error or null...
			return error;
		}
	}
}