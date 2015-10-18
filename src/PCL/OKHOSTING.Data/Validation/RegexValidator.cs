using System;
using System.Text.RegularExpressions;

namespace OKHOSTING.Data.Validation
{
	
	/// <summary>
	/// Implements a validation of regular expressions
	/// </summary>
	/// <remarks>Applies only to string DataMembers</remarks>
	public class RegexValidator: MemberValidator
	{
		public RegexValidator()
		{
		}

		public RegexValidator(string pattern)
		{
			if (string.IsNullOrWhiteSpace(pattern))
			{
				throw new ArgumentNullException("pattern");
			}

			Pattern = pattern;
		}

		/// <summary>
		/// Regular expression used to validate
		/// </summary>
		public string Pattern { get; set; }

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

			//Recover the value of MemberMap associated
			string currentValue = (string) Member.GetValue(obj);

			//if null, exit
			if (string.IsNullOrWhiteSpace(currentValue)) return null;

			//Performing the validation
			Regex regEx = new Regex(Pattern);
			
			//if doesnt match..
			if (!regEx.IsMatch(currentValue)) 
				error = new ValidationError(this, "The MemberMap " + Member + " doesn't match with the regular expression " + Pattern);

			//Returning the error or null
			return error;
		}
	}
}