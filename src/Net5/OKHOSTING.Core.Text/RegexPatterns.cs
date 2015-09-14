namespace OKHOSTING.Core.Text
{

	/// <summary>
	/// Contains a list of common regular expression patterns for general use
	/// </summary>
	/// <remarks>
	/// Some patterns downloaded from http://www.regexlib.com
	/// </remarks>
	public static class RegexPatterns
	{

		/// <summary>
		/// Internet email regex pattern
		/// </summary>
		public const string EmailAddress = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

		/// <summary>
		/// Internet email regex pattern. Used to search email addresses inside a string
		/// </summary>
		public const string FindEmailAddress = @"([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,10})";

		/// <summary>
		/// Url (internet address) regex pattern. 
		/// Allows absolute and relative urls, as well as omiting http://
		/// </summary>
		public const string Url = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

		/// <summary>
		/// Domain name regex pattern
		/// </summary>
		public const string DomainName = @"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

		/// <summary>
		/// Match an IP address v4
		/// </summary>
		public const string IPv4 = @"^([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$";

		/// <summary>
		/// • Match a credit card number to be entered as four sets of four digits separated with a space, -, or no character at all
		/// </summary>
		public const string CreditCard = @"^((\d{4}[- ]?){3}\d{4})$";
		
		/// <summary>
		/// Validates a name. Allows uppercase and lowercase characters and a few special characters that are common to some names
		/// </summary>
		public const string Name = @"^[a-zA-Z''-'\s]";
		
		/// <summary>
		/// Validates a strong password. It must be between 8 and 20 characters, contain at least one digit and one alphabetic character, and must not contain special characters.
		/// </summary>
		public const string Password = @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,20})$";

		/// <summary>
		/// Validates that the field contains an integer greater than zero.
		/// </summary>
		public const string NonNegativeInteger = @"^\d+$";
		
		/// <summary>
		/// Validates a positive currency amount. If there is a decimal point, it requires 2 numeric characters after the decimal point. 
		/// For example, 3.00 is valid but 3.1 is not.
		/// </summary>
		public const string NonNegativeCurrency = @"^([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$";

		/// <summary>
		/// Validates for a positive or negative currency amount. If there is a decimal point, it requires 2 numeric characters after the decimal point.
		/// </summary>
		public const string Currency = @"Validates for a positive or negative currency amount. If there is a decimal point, it requires 2 numeric characters after the decimal point.";

		/// <summary>
		/// Matches any HTML or XML tag within a string. Usefull to extract all tags from a html or xml string
		/// and create text-only representations of them
		/// </summary>
		public const string HtmlTag = @"<[^>]*>";

		/// <summary>
		/// Matches with a hexadecimal digit
		/// </summary>
		public const string HexadecimalDigit = "([A-F]|[a-f]|[0-9])";

		/// <summary>
		/// Matches with a hexadecimal number with exactly the specified 
		/// digits number
		/// </summary>
		/// <param name="Length">
		/// Length of Hexadecimal number
		/// </param>
		/// <returns>
		/// Regular expresion for hexadecimal number with exactly 
		/// the specified digits number
		/// </returns>
		public static string HexadecimalNumber(int Length)
		{ return HexadecimalDigit + "{" + Length + "}"; }


	}
}
