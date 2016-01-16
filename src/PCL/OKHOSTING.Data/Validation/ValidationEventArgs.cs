using System;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Argument for custom validation events
	/// </summary>
	public class ValidationEventArgs : EventArgs
	{
		/// <summary>
		/// true if validation is successfully evaluated, otherwise false
		/// </summary>
		public bool IsValid { get; set; }

		/// <summary>
		/// Error message to show if the validation fails
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Stores the value of the MemberExpression to validate
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Constructs the argument
		/// </summary>
		/// <param name="errorMessage">
		/// Error message to show if the validation fails
		/// </param>
		/// <param name="value">
		/// Stores the value of the MemberExpression to validate
		/// </param>
		public ValidationEventArgs(string errorMessage, object value)
		{
			IsValid = true;
			ErrorMessage = errorMessage;
			Value = value;
		}
	}
}