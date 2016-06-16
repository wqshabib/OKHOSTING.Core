using System.Text.RegularExpressions;

namespace OKHOSTING.Core
{
    /// <summary>
    /// Extension methods for System.String
    /// <para xml:lang="es">
    /// Los métodos de extensión para System.String
    /// </para>
    /// </summary>
    public static class StringExtensions
	{
        /// <summary>
        /// Count all words in a given string
        /// <para xml:lang="es">
        /// Contar todas las palabras de una cadena dada
        /// </para>
        /// </summary>
        /// <param name="input">
        /// string to begin with
        /// <para xml:lang="es">
        /// cadena para empezar
        /// </para>
        /// </param>
        /// <returns>
        /// int
        /// <para xml:lang="es">
        /// entero
        /// </para>
        /// </returns>
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
        /// <para xml:lang="es">
        /// Trunca la cadena en una longitud especificada y vuelva a colocar el truncado a un ...
        /// </para>
        /// </summary>
        /// <param name="text">
        /// string that will be truncated
        /// <para xml:lang="es">
        /// Cadena que sera truncada
        /// </para>
        /// </param>
        /// <param name="maxLength">
        /// total length of characters to maintain before the truncate happens
        /// <para xml:lang="es">
        /// longitud total de caracteres para mantener antes de que ocurra el truncado
        /// </para>
        /// </param>
        /// <returns>
        /// truncated string
        /// <para xml:lang="es">
        /// Cadena truncada
        /// </para>
        /// </returns>
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
        /// <para xml:lang="es">
        /// Formato a una cadena que se presentarán correctamente en una página HTML. Sustituye también a todos los saltos de línea del sistema con saltos de línea HTML
        /// </para>
        /// </summary>
        /// <param name="text">
        /// String to be formatted
        /// <para xml:lang="es">
        /// Cadena a ser formateada
        /// </para>
        /// </param>
        /// <returns>
        /// String correctly formatted for html presentation
        /// <para xml:lang="es">
        /// Cadena formateada correctamente para presentacion html
        /// </para>
        /// </returns>
        public static string TextToHtml(this string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return string.Empty;
			}

			string html;

			html = System.Net.WebUtility.HtmlEncode(text);
			html = html.Replace("\n", "<br />");

			return html;
		}

        /// <summary>
        /// Creates a text only version of a html string, removing all html formatting and replacing html <br /> tags with new lines
        /// <para xml:lang="es">
        /// Crea una versión de sólo texto de una cadena html, eliminar todo el formato html y la sustitución de las etiquetas HTML /> <br con nuevas líneas
        /// </para>
        /// </summary>
        /// <param name="text">
        /// Html string to be converted to text
        /// <para xml:lang="es">
        /// cadena HTML que se convierte en texto
        /// </para>
        /// </param>
        /// <returns>
        /// Text version of the html string
        /// <para xml:lang="es">
        /// Versión texto de la cadena html
        /// </para>
        /// </returns>
        public static string HtmlToText(this string html)
		{
			if (string.IsNullOrWhiteSpace(html))
			{
				return string.Empty;
			}

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