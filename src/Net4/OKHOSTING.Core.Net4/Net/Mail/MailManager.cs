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
	/// </summary>
	public class MailManager
	{
		/// <summary>
		/// Sends an email
		/// </summary>
		/// <param name="message">Email to be sent</param>
		public static void Send(MailMessage message)
		{ 
			Send(message, Configuration.Current); 
		}

		/// <summary>
		/// Sends an email
		/// </summary>
		/// <param name="message">
		/// Email to be sent
		/// </param>
		/// <param name="config">
		/// Configuration to use
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
		/// </summary>
		/// <param name="mailTemplate">
		/// Mail template to be sent
		/// </param>
		public static void Send(MailTemplate mailTemplate)
		{ 
			Send(mailTemplate, Configuration.Current); 
		}

		/// <summary>
		/// Sends a mail template message. Replaces all tags before sending
		/// </summary>
		/// <param name="mailTemplate">
		/// Mail template to be sent
		/// </param>
		/// <param name="config">
		/// Configuration to use
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
		/// </summary>
		/// <param name="addresses">Comma separated string containing email addresses</param>
		/// <returns>List of individual email addresses</returns>
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
		/// </summary>
		/// <param name="emailAddress">
		/// Adress to validate
		/// </param>
		/// <returns>
		/// true if it's a valid email address, otherwise false
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
		/// </summary>
		/// <param name="addressList">
		/// Email addresses list to optimize
		/// </param>
		/// <returns>
		/// string with non repeated and not blank addresses
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
		/// </summary>
		/// <param name="addressList">
		/// Email addresses list to optimize
		/// </param>
		/// <returns>
		/// Array with non repeated and not blank addresses
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