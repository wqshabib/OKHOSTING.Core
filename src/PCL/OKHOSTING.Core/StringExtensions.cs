using System.Text.RegularExpressions;

namespace OKHOSTING.Core
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

		/// <summary>
		/// Formats a string to be correctly presented in a html page. Also replaces all system line breaks with html line breacks
		/// </summary>
		/// <param name="text">String to be formatted</param>
		/// <returns>String correctly formatted for html presentation</returns>
		public static string TextToHtml(this string text)
		{
			string html;

			html = System.Net.WebUtility.HtmlEncode(text);
			html = html.Replace("\n", "<br />");

			return html;
		}

		/// <summary>
		/// Creates a text only version of a html string, removing all html formatting and replacing html <br /> tags with new lines
		/// </summary>
		/// <param name="text">Html string to be converted to text</param>
		/// <returns>Text version of the html string</returns>
		public static string HtmlToText(this string html)
		{
			Regex regex;
			string text = html;

			//decode
			text = System.Net.WebUtility.HtmlDecode(text);

			//replace all <br />, </p> and </div> tags with new line chars
			regex = new Regex("<br>", RegexOptions.IgnoreCase);
			text = regex.Replace(text, "\n");

			regex = new Regex("<br />", RegexOptions.IgnoreCase);
			text = regex.Replace(text, "\n");

			regex = new Regex("</p>", RegexOptions.IgnoreCase);
			text = regex.Replace(text, "\n");

			regex = new Regex("</div>", RegexOptions.IgnoreCase);
			text = regex.Replace(text, "\n");

			//remove the rest of html tags
			regex = new Regex(RegexPatterns.HtmlTag);
			text = regex.Replace(text, string.Empty);

			return html;
		}
	}
}