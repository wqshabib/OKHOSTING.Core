using System;

namespace OKHOSTING.Core
{
	/// <summary>
	/// Extension methods for System.Char
	/// <para xml:lang="es">
	/// Los métodos de extensión para System.Char
	/// </para>
	/// </summary>
	public static class CharExtensions
	{
		/// <summary>
		/// Defines categories for Chars
		/// <para xml:lang="es">
		/// Define las categorías de Caracteres
		/// </para>
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
		/// <para xml:lang="es">
		/// Devuelve la categoría de un Char
		/// </para>
		/// </summary>
		/// <param name="c">
		/// Which returns the character category
		/// <para xml:lang="es">
		/// Carácter del cual devuelve la categoría
		/// </para>
		/// </param>
		/// <returns>
		/// category of a Char
		/// <para xml:lang="es">
		/// categoria del caracter
		/// </para>
		/// </returns>
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
