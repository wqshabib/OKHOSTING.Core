namespace OKHOSTING.Core
{

    /// <summary>
    /// Contains a list of common regular expression patterns for general use
    /// <para xml:lang="es">
    /// Contiene una lista de patrones de expresiones regulares comunes para el uso general
    /// </para>
	/// </summary>
	/// <remarks>
	/// Some patterns downloaded from http://www.regexlib.com
    /// <para xml:lang="es">
    /// Algunos patrones descargados de http://www.regexlib.com
    /// </para>
	/// </remarks>
	public static class RegexPatterns
	{

        /// <summary>
        /// Internet email regex pattern
        /// <para xml:lang="es">
        /// Internet patrón de expresión de correo electrónico
        /// </para>
        /// </summary>
        public const string EmailAddress = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        /// Internet email regex pattern. Used to search email addresses inside a string
        /// <para xml:lang="es">
        /// Internet patrón de expresión de correo electrónico.Se utiliza para buscar direcciones de correo electrónico dentro de una cadena
        /// </para>
        /// </summary>
        public const string FindEmailAddress = @"([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,10})";

        /// <summary>
        /// Url (internet address) regex pattern. 
        /// Allows absolute and relative urls, as well as omiting http://
        /// <para xml:lang="es">
        /// URL (dirección de Internet) patrón de expresión. Permite direcciones
        /// URL absolutas y relativas, así como vómitos http: //
        /// </para>
        /// </summary>
        public const string Url = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$";

        /// <summary>
        /// Domain name regex pattern
        /// <para xml:lang="es">
        /// Dominio patrón de nombre de expresiones regulares
        /// </para>
        /// </summary>
        public const string DomainName = @"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";

        /// <summary>
        /// Match an IP address v4
        /// <para xml:lang="es">
        /// Coincida con una dirección IP v4
        /// </para>
        /// </summary>
        public const string IPv4 = @"^([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$";

        /// <summary>
        /// • Match a credit card number to be entered as four sets of four digits separated with a space, -, or no character at all
        /// <para xml:lang="es">
        /// Coinciden con un número de tarjeta de crédito para introducirse como cuatro grupos de cuatro dígitos separados por un espacio, -, o ningún carácter en todo
        /// </para>
        /// </summary>
        public const string CreditCard = @"^((\d{4}[- ]?){3}\d{4})$";

        /// <summary>
        /// Validates a name. Allows uppercase and lowercase characters and a few special characters that are common to some names
        /// <para xml:lang="es">
        /// Valida un nombre. Permite caracteres en mayúsculas y minúsculas y algunos caracteres especiales que son comunes a algunos nombres
        /// </para>
        /// </summary>
        public const string Name = @"^[a-zA-Z''-'\s]";

        /// <summary>
        /// Validates a strong password. It must be between 8 and 20 characters, contain at least one digit and one alphabetic character, and must not contain special characters.
        /// <para xml:lang="es">
        /// Valida una contraseña segura. Debe tener entre 8 y 20 caracteres, contener al menos un dígito y un carácter alfabético, y no debe contener caracteres especiales.
        /// </para>
        /// </summary>
        public const string Password = @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,20})$";

        /// <summary>
        /// Validates that the field contains an integer greater than zero.
        /// <para xml:lang="es">
        /// Valida que el campo contiene un número entero mayor que cero.
        /// </para>
        /// </summary>
        public const string NonNegativeInteger = @"^\d+$";

        /// <summary>
        /// Validates a positive currency amount. If there is a decimal point, it requires 2 numeric characters after the decimal point. 
        /// For example, 3.00 is valid but 3.1 is not.
        /// <para xml:lang="es">
        /// Valida una cantidad de moneda positivo. Si hay un punto decimal, se requiere de 2 caracteres numéricos después del punto decimal. 
        /// Por ejemplo, 3.00 es válida, pero no si es 3.1.
        /// </para>
        /// </summary>
        public const string NonNegativeCurrency = @"^([0-2]?[0-5]?[0-5]\.){3}[0-2]?[0-5]?[0-5]$";

        /// <summary>
        /// Validates for a positive or negative currency amount. If there is a decimal point, it requires 2 numeric characters after the decimal point.
        /// <para xml:lang="es">
        /// Validaciones para una cantidad de moneda positivo o negativo. Si hay un punto decimal, se requiere de 2 caracteres numéricos después del punto decimal.
        /// </para>
        /// </summary>
        public const string Currency = @"Validates for a positive or negative currency amount. If there is a decimal point, it requires 2 numeric characters after the decimal point.";

        /// <summary>
        /// Matches any HTML or XML tag within a string. Usefull to extract all tags from a html or xml string
        /// and create text-only representations of them
        /// <para xml:lang="es">
        /// Coincide con cualquier etiqueta HTML o XML dentro de una cadena. Útil para extraer todas las etiquetas
        /// de una cadena HTML o XML y crear representaciones de sólo texto de ellas
        /// </para>
        /// </summary>
        public const string HtmlTag = @"<[^>]*>";

        /// <summary>
        /// Matches with a hexadecimal digit
        /// <para xml:lang="es">
        /// Coincide con un dígito hexadecimal
        /// </para>
        /// </summary>
        public const string HexadecimalDigit = "([A-F]|[a-f]|[0-9])";

        /// <summary>
        /// Matches with a hexadecimal number with exactly the specified 
        /// digits number
        /// <para xml:lang="es">
        /// Partidos con un número hexadecimal con exactitud el número de
        /// dígitos especificado
        /// </para>
        /// </summary>
        /// <param name="Length">
        /// Length of Hexadecimal number
        /// <para xml:lang="es">
        /// Longitud del número hexadecimal
        /// </para>
        /// </param>
        /// <returns>
        /// Regular expresion for hexadecimal number with exactly 
        /// the specified digits number
        /// <para xml:lang="es">
        /// Expresión regular para el número hexadecimal con exactamente
        /// el número de dígitos especificado
        /// </para>
        /// </returns>
        public static string HexadecimalNumber(int Length)
		{ return HexadecimalDigit + "{" + Length + "}"; }


	}
}
