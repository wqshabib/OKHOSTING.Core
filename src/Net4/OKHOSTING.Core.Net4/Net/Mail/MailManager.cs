using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text.RegularExpressions;
using OKHOSTING.Core.Net4;
using System.Net;

namespace OKHOSTING.Core.Net4.Net.Mail
{
	/// <summary>
	/// Provides basic email sending functionality
    /// <para xml:lang="es">
    /// Prove funcionalidades basicas para envio de correo electronico
    /// </para>
	/// </summary>
	public class MailManager
	{
        /// <summary>
        /// Sends an email
        /// <para xml:lang="es">
        /// Envia un correo
        /// </para>
        /// </summary>
        /// <param name="message">
        /// Email to be sent
        /// <para xml:lang="es">
        /// Correo a enviar
        /// </para>
        /// </param>
        public static void Send(MailMessage message)
		{ 
			Send(message, Configuration.Current); 
		}

		/// <summary>
        /// Sends an email
        /// <para xml:lang="es">
        /// Envia un email
        /// </para>
        /// </summary>
        /// <param name="message">
        /// Email to be sent
        /// <para xml:lang="es">
        /// Correo a enviar
        /// </para>
        /// </param>
        /// <param name="config">
        /// Configurate to use
        /// <para xml:lang="es">
        /// Configuracion a usar
        /// </para>
        /// </param>
		public static void Send(MailMessage message, Configuration config)
		{
			//is mail sending activated?
			if (config.SendMail)
			{
				//Establishing smtp server configuration	
				SmtpClient Smtp = new SmtpClient(config.SmtpServer, config.SmtpPort);

				//Establishing authentication settings
				if (config.UseAutentication)
				{
					Smtp.UseDefaultCredentials = false;
					Smtp.Credentials = new NetworkCredential(config.UserName, config.Password);
				}

				//Set default From addresses (if apply)
				if (message.From == null && !string.IsNullOrWhiteSpace(config.FromAddress) && !string.IsNullOrWhiteSpace(config.FromName))
				{
					message.From = new MailAddress(config.FromAddress, config.FromName);
				}
				else if (message.From == null && !string.IsNullOrWhiteSpace(config.FromAddress))
				{
					message.From = new MailAddress(config.FromAddress);
				}
				
				//Set default ReplyTo addresses (if apply)
				if (!string.IsNullOrWhiteSpace(config.ReplyToAddress) && !string.IsNullOrWhiteSpace(config.ReplyToName))
				{
					message.ReplyToList.Add(new MailAddress(config.ReplyToAddress, config.ReplyToName));
				}
				else if (!string.IsNullOrWhiteSpace(config.ReplyToAddress))
				{
					message.ReplyToList.Add(new MailAddress(config.ReplyToAddress));
				}

				//Establishing To, Cc and Bcc fields
				if (!string.IsNullOrWhiteSpace(config.ToAddress)) message.To.Add(MailManager.OptimizeAddressList(config.ToAddress));
				if (!string.IsNullOrWhiteSpace(config.CCAddress)) message.CC.Add(MailManager.OptimizeAddressList(config.CCAddress));
				if (!string.IsNullOrWhiteSpace(config.BccAddress)) message.Bcc.Add(MailManager.OptimizeAddressList(config.BccAddress));

				//Trying to send the message
				try 
				{ 
					Smtp.Send(message);
					
					Log.WriteDebug
						(
							typeof(MailManager).FullName + ".Send(MailMessage, Configuration)", 
							"Message sent\n" + 
							"\nFrom: " + message.From +
							"\nTo: " + message.To +
							"\nCc: " + message.CC +
							"\nBcc: " + message.Bcc +
							"\nSubject: " + message.Subject +
							"\nSMTP Server: " + Smtp.Host + ":" + Smtp.Port
						);
				}
				catch (Exception e)
				{ 
					throw new MailNotSentException(e.Message, e, message); 
				}
			}
		}

        /// <summary>
        /// Sends a mail template message. Replaces all tags before sending
        /// <para xml:lang="es">
        /// Envía un mensaje de plantilla de correo.Reemplaza todas las etiquetas antes de enviar
        /// </para>
        /// </summary>
        /// <param name="mailTemplate">
        /// Mail template to be sent
        /// <para xml:lang="es">
        /// Plantilla de correo que se enviará
        /// </para>
        /// </param>
        public static void Send(MailTemplate mailTemplate)
		{ 
			Send(mailTemplate, Configuration.Current); 
		}

        /// <summary>
        /// Sends a mail template message. Replaces all tags before sending
        /// <para xml:lang="es">
        /// Envía un mensaje de plantilla de correo.Reemplaza todas las etiquetas antes de enviar
        /// </para>
        /// </summary>
        /// <param name="mailTemplate">
        /// Mail template to be sent
        /// <para xml:lang="es">
        /// Plantilla de correo que se enviará
        /// </para>
        /// </param>
        /// <param name="config">
        /// Configuration to use
        /// <para xml:lang="es">
        /// Configuracion que se usara
        /// </para>
        /// </param>
        public static void Send(MailTemplate mailTemplate, Configuration config)
		{
			//Replacing all tags in the subject and body, and prepares the message to be sent
			mailTemplate.Init();

			//Sending mail...
			Send((MailMessage)mailTemplate, config);
		}

        /// <summary>
        /// Splits a comma separated string containing email addresses into individual different addresses
        /// <para xml:lang="es">
        /// Divide una cadena separada por comas que contiene direcciones de correo electrónico a direcciones individuales diferentes
        /// </para>
        /// </summary>
        /// <param name="addresses">
        /// Comma separated string containing email addresses
        /// <para xml:lang="es">
        /// Cadenas separadas por coma que contienen direcciones de correo
        /// </para>
        /// </param>
        /// <returns>
        /// List of individual email addresses
        /// <para xml:lang="es">
        /// Lista de direcciones de correo individuales
        /// </para>
        /// </returns>
        public static string[] SplitAddresses(string addresses)
		{
			//Local vars
			string[] result = new string[0];

			//Validating specified address
			if (addresses != null)
			{
				//Splitting addresses on result array
				result = addresses.Split(';', ',');
			}
			
			//Returning the result
			return result;
		}

        /// <summary>
        /// Determines wether a string is a valid email address or not
        /// based on a regular expression pattern
        /// <para xml:lang="es">
        /// Determina si una cadena es una dirección de correo electrónico
        /// válida o no en base a un patrón de expresión regular
        /// </para>
        /// </summary>
        /// <param name="emailAddress">
        /// Adress to validate
        /// <para xml:lang="es">
        /// Direccion a validar
        /// </para>
        /// </param>
        /// <returns>
        /// true if it's a valid email address, otherwise false
        /// <para xml:lang="es">
        /// True si es una direccion de correo valida, de lo contrario false
        /// </para>
        /// </returns>
        public static bool IsValidEmail(string emailAddress)
		{
			//Local vars 
			bool isValidEmail = false;

			//Validating if email address is null or empty
			if (!string.IsNullOrWhiteSpace(emailAddress))
				isValidEmail = Regex.IsMatch(emailAddress, Core.RegexPatterns.EmailAddress);

			//Returning result
			return isValidEmail;
		}

        /// <summary>
        /// Optimizes an email addresses list, discarding blank and repeated addresses
        /// <para xml:lang="es">
        /// Optimiza una lista de direcciones de correo, descartando direcciones en blanco y repetidas
        /// </para>
        /// </summary>
        /// <param name="addressList">
        /// Email addresses list to optimize
        /// <para xml:lang="es">
        /// Lista de direcciones de correo a optimizar
        /// </para>
        /// </param>
        /// <returns>
        /// string with non repeated and not blank addresses
        /// <para xml:lang="es">
        /// cadena con direcciones no repetidas y no en blanco
        /// </para>
		/// </returns>
		public static string OptimizeAddressList(string addressList)
		{
			string list = string.Empty;

			//Splitting address
			foreach(string s in OptimizeAddressList(SplitAddresses(addressList)))
			{
				list += s + ",";			
			}

			list = list.TrimEnd(',');

			return list;
		}

        /// <summary>
        /// Optimizes an email addresses list, discarding blank and repeated addresses
        /// <para xml:lang="es">
        /// Optimiza una lista de direcciones de correo, descartando direcciones en blanco y repetidas
        /// </para>
        /// </summary>
        /// <param name="addressList">
        /// Email addresses list to optimize
        /// <para xml:lang="es">
        /// Lista de direcciones de correo a optimizar
        /// </para>
        /// </param>
        /// <returns>
        /// Array with non repeated and not blank addresses
        /// <para xml:lang="es">
        /// Arreglo con direcciones no repetidas y no en blanco
        /// </para>
        /// </returns>
        public static string[] OptimizeAddressList(string[] addressList)
		{
			//Local vars
			List<string> tempAddressList = new List<string>();

			//Crossing the address list
			for(int counter = 0; counter < addressList.GetLength(0); counter++)
			{
				//Getting current element
				string currentAddress = addressList[counter].Trim().ToLower();

				//Validating if current address is nos an empty 
				//string or a previously added addres
				if (currentAddress != string.Empty && !tempAddressList.Contains(currentAddress))
					tempAddressList.Add(currentAddress);
			}

			//Returning result
			return tempAddressList.ToArray();
			
		}

	}
}