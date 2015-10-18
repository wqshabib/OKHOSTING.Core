using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace OKHOSTING.Data.Validation
{
	/// <summary>
	/// Defines an exception caused by a validator
	/// </summary>
	public class ValidationException : Exception
	{
		/// <summary>
		/// Array of errors that causes the exception throw
		/// </summary>
		public readonly IEnumerable<ValidationError> ValidationErrors;

		/// <summary>
		/// Referece to Object that fails on it validation
		/// </summary>
		public readonly object ValidatedObject;


		/// <summary>
		/// Constructs the exception
		/// </summary>
		/// <param name="validationError">
		/// Error that causes the exception throw
		/// </param>
		/// <param name="validatedObject">
		/// Referece to object that fails on it validation
		/// </param>
		public ValidationException(ValidationError validationError, object validatedObject): this(new List<ValidationError> { validationError }, validatedObject, string.Empty)
		{ 
		}

		/// <summary>
		/// Constructs the exception
		/// </summary>
		/// <param name="validationError">
		/// Error that causes the exception throw
		/// </param>
		/// <param name="validatedObject">
		/// Referece to object that fails on it validation
		/// </param>
		/// <param name="message">
		/// Custom Error message
		/// </param>
		public ValidationException(ValidationError validationError, object validatedObject, string message): this(new List<ValidationError> { validationError }, validatedObject, message) 
		{ 
		}

		/// <summary>
		/// Constructs the exception
		/// </summary>
		/// <param name="validationErrors">
		/// Array of errors that causes the exception throw
		/// </param>
		/// <param name="validatedObject">
		/// Referece to object that fails on it validation
		/// </param>
		public ValidationException(IEnumerable<ValidationError> validationErrors, object validatedObject) : this(validationErrors, validatedObject, string.Empty) 
		{ 
		}
	
		/// <summary>
		/// Constructs the exception
		/// </summary>
		/// <param name="validationErrors">
		/// Array of errors that causes the exception throw
		/// </param>
		/// <param name="validatedObject">
		/// Referece to object that fails on it validation
		/// </param>
		/// <param name="message">
		/// Custom Error message
		/// </param>
		public ValidationException(IEnumerable<ValidationError> validationErrors, object validatedObject, string message): base(message)
		{
			this.ValidationErrors = validationErrors;
			this.ValidatedObject = validatedObject;
		}

		#region Exception Overrides

		/// <summary>
		/// Returns the exception represented as a string
		/// </summary>
		/// <returns>
		/// Equivalent to this.Message
		/// </returns>
		public override string ToString()
		{
			return this.Message;
		}

		/// <summary>
		/// Returns the complete error message of the exception 
		/// (from all Errors contained on ValidationErrors array)
		/// </summary>
		public override string Message
		{
			get
			{
				//Local Vars
				string msg;

				//Initializing error message
				msg = base.Message + "\n";

				//Crossing all the exceptions and completing the message
				foreach (ValidationError error in this.ValidationErrors)
				{
					msg += error.Description + "\n";
				}
				
				msg += "\n object:\n" + Convert.ToString((IXmlSerializable) this.ValidatedObject);

				//Returning the message
				return msg;
			}
		}

		#endregion
	}
}