using System;
using System.Net.Mail;

namespace OKHOSTING.Net.Net4.Mail
{

	/// <summary>
	/// Thrown when an email could not be sent
	/// </summary>
	public class MailNotSentException: System.Net.Mail.SmtpException
	{

		#region Field and properties

		/// <summary>
		/// Mail Message related to exception
		/// </summary>
		private MailMessage mailMessage;

		/// <summary>
		/// Mail Message related to exception
		/// </summary>
		public MailMessage MailMessage
		{ get { return this.mailMessage; } }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs the MailNotSentException
		/// </summary>
		public MailNotSentException() : base() { }

		/// <summary>
		/// Constructs the MailNotSentException
		/// </summary>
		/// <param name="message">
		/// Error message
		/// </param>
		public MailNotSentException(string message) : base(message) { }

		/// <summary>
		/// Constructs the MailNotSentException
		/// </summary>
		/// <param name="message">
		/// Error message
		/// </param>
		/// <param name="innerException">
		/// Inner exception
		/// </param>
		public MailNotSentException(string message, Exception innerException) : base(message, innerException) { }

		/// <summary>
		/// Constructs the MailNotSentException
		/// </summary>
		/// <param name="message">
		/// Error message
		/// </param>
		/// <param name="innerException">
		/// Inner exception
		/// </param>
		/// <param name="mailMessage">
		/// Mail Message related to exception
		/// </param>
		public MailNotSentException(string message, Exception innerException, MailMessage mailMessage)
			: base(message, innerException)
		{ this.mailMessage = mailMessage; }

		#endregion

		#region Override Functions

		/// <summary>
		/// Overrides the default ToString() method behavior
		/// </summary>
		/// <returns>
		/// Customized string for MailNotSentException
		/// </returns>
		public override string ToString()
		{
			//Initializing message
			string outStr = base.ToString() + "\n\nMail message:\n\n";

			//Completing the message
			if (mailMessage != null)
				outStr +=
					"\nSubject: " + mailMessage.Subject +
					"\nFrom: " + mailMessage.From +
					"\nTo: " + mailMessage.To +
					"\nCc: " + mailMessage.CC +
					"\nBcc: " + mailMessage.Bcc;

			else
				outStr +=
					"\nMail message information is not available";
			
			//Returning message
			return outStr;
		}

		#endregion

	}
}