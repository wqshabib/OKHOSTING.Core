using System;
using System.Text.RegularExpressions;

namespace OKHOSTING.Core.Net4.Net
{
	/// <summary>
	/// Extension methods for System.String
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Formats a string to be correctly presented in a html page. Also replaces all system line breaks with html line breacks
		/// </summary>
		/// <param name="text">String to be formatted</param>
		/// <returns>String correctly formatted for html presentation</returns>
		public static string TextToHtml(this string text)
		{
			string html;

			html = System.Web.HttpContext.Current.Server.HtmlEncode(text);
			html = html.Replace(Environment.NewLine, "<br />");

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
			text = System.Web.HttpContext.Current.Server.HtmlDecode(text);

			//replace all <br />, </p> and </div> tags with new line chars
			regex = new Regex("<br>", RegexOptions.IgnoreCase);
			text = regex.Replace(text, Environment.NewLine);

			regex = new Regex("<br />", RegexOptions.IgnoreCase);
			text = regex.Replace(text, Environment.NewLine);

			regex = new Regex("</p>", RegexOptions.IgnoreCase);
			text = regex.Replace(text, Environment.NewLine);

			regex = new Regex("</div>", RegexOptions.IgnoreCase);
			text = regex.Replace(text, Environment.NewLine);

			//remove the rest of html tags
			regex = new Regex(OKHOSTING.Core.RegexPatterns.HtmlTag);
			text = regex.Replace(text, "");

			return html;
		}
	}
}
