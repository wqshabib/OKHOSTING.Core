using System;
using System.Net.Mail;

namespace OKHOSTING.Core.Net4.Net.Mail
{

    /// <summary>
    /// Thrown when an email could not be sent
    /// <para xml:lang="es">
    /// Se lanza cuando un correo electrónico no se pudo enviar
    /// </para>
    /// </summary>
    public class MailNotSentException: System.Net.Mail.SmtpException
	{

        #region Field and properties

        /// <summary>
        /// Mail Message related to exception
        /// <para xml:lang="es">
        /// Mensaje de correo relacionado con excepción
        /// </para>
        /// </summary>
        private MailMessage mailMessage;

        /// <summary>
        /// Mail Message related to exception
        /// <para xml.lang="es">
        /// Mensaje de correo relacionado con excepción
        /// </para>
        /// </summary>
        public MailMessage MailMessage
		{ get { return this.mailMessage; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs the MailNotSentException
        /// <para xml:lang="es">
        /// Construye la excepción de correo no enviada
        /// </para>
        /// </summary>
        public MailNotSentException() : base() { }

        /// <summary>
        /// Constructs the MailNotSentException
        /// <para xml:lang="es">
        /// Construye la excepción de correo no enviada
        /// </para>
        /// </summary>
        /// <param name="message">
        /// Error message
        /// </param>
        public MailNotSentException(string message) : base(message) { }

        /// <summary>
        /// Constructs the MailNotSentException
        /// <para xml:lang="es">
        /// Construye la excepción de correo no enviada
        /// </para>
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
        /// <para xml:lang="es">
        /// Construye la excepción de correo no enviada
        /// </para>
        /// </summary>
        /// <param name="message">
        /// Error message
        /// <para xml:lang="es">
        /// Mensaje de error
        /// </para>
        /// </param>
        /// <param name="innerException">
        /// Inner exception
        /// <para xml:lang="es">
        /// excepción interna
        /// </para>
        /// </param>
        /// <param name="mailMessage">
        /// Mail Message related to exception
        /// <para xml:lang="es">
        /// Mensaje de correo relacionada con excepción
        /// </para>
        /// </param>
        public MailNotSentException(string message, Exception innerException, MailMessage mailMessage)
			: base(message, innerException)
		{ this.mailMessage = mailMessage; }

        #endregion

        #region Override Functions

        /// <summary>
        /// Overrides the default ToString() method behavior
        /// <para xml:lang="es">
        /// Sobreescribe el comportamiento del metodo por defecto ToString()
        /// </para>
        /// </summary>
        /// <returns>
        /// Customized string for MailNotSentException
        /// <para xml:lang="es">
        /// cadena personalizada para Excepción de correo no enviada
        /// </para>
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