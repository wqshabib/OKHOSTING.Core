namespace OKHOSTING.Core.Net4.Net.Mail
{
	/// <summary>
	/// Defines configuration values to send emails from an application
	/// </summary>
	public class Configuration: Core.Net4.ConfigurationBase
	{
		/// <summary>
		/// Whether sending mails is active or not
		/// </summary>
		public bool SendMail;

		/// <summary>
		/// Whether to use authentication or not
		/// </summary>
		public bool UseAutentication;

		/// <summary>
		/// Username to use if authentication is enabled
		/// </summary>
		public string UserName;

		/// <summary>
		/// Password to use if authentication is enabled
		/// </summary>
		public string Password;

		/// <summary>
		/// Comma separated addresses where all emails will be sent as To
		/// </summary>
		public string ToAddress;

		/// <summary>
		/// Comma separated addresses where all emails will be sent as CC
		/// </summary>
		public string CCAddress;

		/// <summary>
		/// Comma separated addresses where all emails will be sent as Bcc
		/// </summary>
		public string BccAddress;

		/// <summary>
		/// Smtp server to use
		/// </summary>
		public string SmtpServer;

		/// <summary>
		/// Smtp port server to use
		/// </summary>
		public int SmtpPort;

		/// <summary>
		/// Email address used as email sender for all emails
		/// </summary>
		public string FromAddress;

		/// <summary>
		/// Name used as email sender for all emails
		/// </summary>
		public string FromName;

		/// <summary>
		/// Email address used as ReplyTo field for all emails
		/// </summary>
		public string ReplyToAddress;

		/// <summary>
		/// Name used as ReplyTo field for all emails
		/// </summary>
		public string ReplyToName;

		/// <summary>
		/// Current configuration
		/// </summary>
		public static Configuration Current;

		/// <summary>
		/// Loads the current configuration
		/// </summary>
		static Configuration()
		{
			Current = (Configuration) OKHOSTING.Core.Net4.ConfigurationBase.Load(typeof(Configuration));
		}
	}
}
