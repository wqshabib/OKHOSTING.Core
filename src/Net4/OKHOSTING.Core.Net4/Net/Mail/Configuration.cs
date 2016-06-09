namespace OKHOSTING.Core.Net4.Net.Mail
{
    /// <summary>
    /// Defines configuration values to send emails from an application
    /// <para xml:lang="es">
    /// Define los valores de configuración para enviar mensajes de correo electrónico desde una aplicación
    /// </para>
    /// </summary>
    public class Configuration: Core.Net4.ConfigurationBase
	{
        /// <summary>
        /// Determines whether sending e is active or not
        /// <para xml:lang="es">
        /// Determina si el envío de correos está activo o no
        /// </para>
        /// </summary>
        public bool SendMail;

        /// <summary>
        /// Whether to use authentication or not
        /// <para xml:lang="es">
        /// se usa para saber si utilizar la autenticación o no
        /// </para>
        /// </summary>
        public bool UseAutentication;

		/// <summary>
		/// Username to use if authentication is enabled
        /// <para xml:lang="es">
        /// Nombre de usuario a usar si la autenticacion esta habilitada
        /// </para>
		/// </summary>
		public string UserName;

        /// <summary>
        /// Password to use if authentication is enabled
        /// <para xml:lang="es">
        /// Password de usuario a usar si la autenticacion esta habilitada
        /// </para>
        /// </summary>
        public string Password;

        /// <summary>
        /// Comma separated addresses where all emails will be sent as To
        /// <para xml:lang="es">
        /// Comas que separan las direcciones en las que se envían todos los correos electrónicos como Para
        /// </para>
        /// </summary>
        public string ToAddress;

        /// <summary>
        /// Comma separated addresses where all emails will be sent as CC
        /// <para xml:lang="es">
        /// Comas que separan las direcciones en las que se envían todos los correos electrónicos como CC
        /// </para>
        /// </summary>
        public string CCAddress;

        /// <summary>
        /// Comma separated addresses where all emails will be sent as Bcc
        /// <para xml:lang="es">
        /// Comas que separan las direcciones en las que se envían todos los correos electrónicos como Bcc
        /// </para>
        /// </summary>
        public string BccAddress;

		/// <summary>
		/// Smtp server to use
        /// <para xml:lang="es">
        /// Servidor smtp a usar
        /// </para>
		/// </summary>
		public string SmtpServer;

		/// <summary>
		/// Smtp port server to use
        /// <para xml:lang="es">
        /// Puerto del servidor smtp a usar
        /// </para>
		/// </summary>
		public int SmtpPort;

        /// <summary>
        /// Email address used as email sender for all emails
        /// <para xml:lang="es">
        /// dirección de correo electrónico utiliza como remitente de correo electrónico para todos los mensajes de correo electrónico
        /// </para>
        /// </summary>
        public string FromAddress;

        /// <summary>
        /// Name used as email sender for all emails
        /// <para xml:lang="es">
        /// Nombre utilizado como remitente de correo electrónico para todos los mensajes de correo electrónico
        /// </para>
        /// </summary>
        public string FromName;

        /// <summary>
        /// Email address used as ReplyTo field for all emails
        /// <para xml:lang="es">
        /// Dirección de correo electrónico utilizada como campo Responder a mensajes para todos los correo electrónico 
        /// </para>
        /// </summary>
        public string ReplyToAddress;

        /// <summary>
        /// Name used as ReplyTo field for all emails
        /// <para xml:lang="es">
        /// Nombre utilizado como campo para Responder a todos los mensajes de correo electrónico 
        /// </para>
        /// </summary>
        public string ReplyToName;

		/// <summary>
		/// Current configuration
        /// <para xml:lang="es">
        /// Actual configuracion
        /// </para>
		/// </summary>
		public static Configuration Current;

		/// <summary>
		/// Loads the current configuration
        /// <para xml:lang="es">
        /// Carga la actual configuracion
        /// </para>
		/// </summary>
		static Configuration()
		{
			Current = (Configuration) OKHOSTING.Core.Net4.ConfigurationBase.Load(typeof(Configuration));
		}
	}
}
