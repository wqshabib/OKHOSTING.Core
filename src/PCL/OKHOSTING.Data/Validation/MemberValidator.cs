using System;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Validates a specific member on the object, instead of the object as a hole. The actual validation is the one assigned to the Validation property
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class MemberValidator : ValidatorBase
	{
		public MemberValidator()
		{
		}

		public MemberValidator(MemberExpression member, ValidatorBase validator)
		{
			Member = member;
			Validator = validator;
		}

		/// <summary>
		/// Member expression to which the validations will be applied
		/// </summary>
		public MemberExpression Member { get; set; }

		/// <summary>
		/// This is the actual validation that will be performed on the member
		/// </summary>
		public ValidatorBase Validator { get; set; }

		/// <summary>
		/// Performs the validation
		/// </summary>
		/// <returns>
		/// ValidationError object with the error founded if the validation fails,
		/// otherwise returns null
		/// </returns>
		public override ValidationError Validate(object obj)
		{
			//Recover the value of MemberExpression associated
			string currentValue = (string) Member.GetValue(obj);

			//if null, exit
			if (string.IsNullOrWhiteSpace(currentValue)) return null;

			//Performing the validation
			return Validator.Validate(currentValue);
		}
	}
}