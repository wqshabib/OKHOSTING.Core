using System;

namespace OKHOSTING.Core.Extensions
{
	/// <summary>
	/// Extension methods for System.Char
	/// </summary>
	public static class CharExtensions
	{
		/// <summary>
		/// Defines categories for Chars
		/// </summary>
		public enum CharCategory
		{
			Control,
			Digit,
			Letter,
			Number,
			Punctuation,
			Separator,
			Surrogate,
			Symbol,
			Whitespace,
			Unknown
		}

		/// <summary>
		/// Returns the category of a Char
		/// </summary>
		public static CharCategory Category(this char c)
		{
			//Local vars
			CharCategory category = CharCategory.Unknown;

			//Evaluating char category
			if (Char.IsControl(c))
				category = CharCategory.Control;

			else if (Char.IsDigit(c))
				category = CharCategory.Digit;

			else if (Char.IsLetter(c))
				category = CharCategory.Letter;

			else if (Char.IsNumber(c))
				category = CharCategory.Number;

			else if (Char.IsPunctuation(c))
				category = CharCategory.Punctuation;

			else if (Char.IsSeparator(c))
				category = CharCategory.Separator;

			else if (Char.IsSurrogate(c))
				category = CharCategory.Surrogate;

			else if (Char.IsSymbol(c))
				category = CharCategory.Symbol;

			else if (Char.IsWhiteSpace(c))
				category = CharCategory.Whitespace;

			//Returning char category
			return category;
		}
	}
}
