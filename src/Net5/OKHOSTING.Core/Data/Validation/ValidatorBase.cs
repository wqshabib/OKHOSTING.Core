using System;

namespace OKHOSTING.Core.Data.Validation
{
	/// <summary>
	/// Defines the behavior that must be have the validation classes
	/// </summary>
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public abstract class ValidatorBase: Attribute
	{
		public int Id { get; set; }

		/// <summary>
		/// Performs the validation
		/// </summary>
		/// <returns>
		/// ValidationError object with the error founded if the validation fails,
		/// otherwise returns null
		/// </returns>
		public abstract ValidationError Validate(object obj);
	}
}