using System.Text.RegularExpressions;

namespace OKHOSTING.Core.Text
{
	/// <summary>
	/// Extension methods for System.String
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Count all words in a given string
		/// </summary>
		/// <param name="input">string to begin with</param>
		/// <returns>int</returns>
		public static int WordCount(this string input)
		{
			var count = 0;
			try
			{
				// Exclude whitespaces, Tabs and line breaks
				var re = new Regex(@"[^\s]+");
				var matches = re.Matches(input);
				count = matches.Count;
			}
			catch
			{
			}
			return count;
		}

		/// <summary>
		/// Truncates the string to a specified length and replace the truncated to a ...
		/// </summary>
		/// <param name="text">string that will be truncated</param>
		/// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
		/// <returns>truncated string</returns>
		public static string Truncate(this string text, int maxLength)
		{
			// replaces the truncated string to a ...
			const string suffix = "...";
			string truncatedString = text;

			if (maxLength <= 0) return truncatedString;
			int strLength = maxLength - suffix.Length;

			if (strLength <= 0) return truncatedString;

			if (text == null || text.Length <= maxLength) return truncatedString;

			truncatedString = text.Substring(0, strLength);
			truncatedString = truncatedString.TrimEnd();
			truncatedString += suffix;
			return truncatedString;
		}

		///// <summary>
		///// Converts string to a title case.  
		///// </summary>
		//public static string ToProperCase(this string text)
		//{
		//	System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
		//	System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
		//	return textInfo.ToTitleCase(text);
		//}
	}
}