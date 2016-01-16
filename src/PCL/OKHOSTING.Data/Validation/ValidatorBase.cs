using System;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Parent class for all validations
	/// </summary>
	/// <remarks>
	/// Usable as well as an attribute
	/// </remarks>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public abstract class ValidatorBase: Attribute
	{
		/// <summary>
		/// Performs the validation on the supplied object
		/// </summary>
		/// <returns>
		/// ValidationError object with the error founded if the validation fails,
		/// otherwise returns null
		/// </returns>
		public abstract ValidationError Validate(object obj);
	}
}