using System;
using System.IO;
using System.Diagnostics;

namespace OKHOSTING.Core.Net4
{
	/// <summary>
	/// Allows an easy creation and manipulation of log files
    /// <para xml:lang="es">
    /// Permite crear y manipular facilmente archivos de log
    /// </para>
	/// </summary>
	public class Log
	{
		#region Constants
		
		/// <summary>
		/// General, non critical information
        /// <para xml:lang="es">
        /// Informacion general no critica
        /// </para>
		/// </summary>
		public const string Information = "Information";

        /// <summary>
        /// Unhandled exception
        /// <para xml:lang="es">
        /// Excepción no controlada
        /// </para>
        /// </summary>
        public const string Exception = "Exception";

        /// <summary>
        /// Handled exception, that does not cause an application crash
        /// <para xml:lang="es">
        /// excepción controlada, que no causa una caída de la aplicación
        /// </para>
		/// </summary>
		public const string HandledException = "HandledException";

        /// <summary>
        /// Security related log
        /// <para xml:lang="es">
        /// relacionada con la seguridad de registro
        /// </para>
        /// </summary>
        public const string Security = "Security";

        /// <summary>
        /// Debug information
        /// <para xml:lang="es">
        /// La información de depuración
        /// </para>
        /// </summary>
        public const string Debug = "Debug";

        #endregion

        #region Fields and properties

        /// <summary>
        /// The kind of logs that this log contains. This will be the name of the log file
        /// <para xml:lang="es">
        /// El tipo de registros que contiene este registro.Este será el nombre del archivo de registro
        /// </para>
        /// </summary>
        /// <example>
        /// Information, Exception, Security
        /// <para xml:lang="es">
        /// Informacion, Excepcion, Seguridad
        /// </para>
        /// </example>
        private string type;

        /// <summary>
        /// The kind of logs that this log contains. This will be the name of the log file
        /// <para xml:lang="es">
        /// El tipo de registros que contiene este registro.Este será el nombre del archivo de registro
        /// </para>
        /// </summary>
        /// <example>
        /// Information, Exception, Security
        /// <para xml:lang="es">
        /// Informacion, Excepcion, Seguridad
        /// </para>
        /// </example>
        public string Type
		{
			get { return this.type; }
			set
			{
				//Validating specified value
				if (value == null)
					throw new ArgumentNullException("value");

				else if (value.Trim() == string.Empty)
					throw new ArgumentException("The value for Log.LogType property can't be an empty string", "value");

				else if (value.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
					throw new ArgumentException("The specified value for Log.LogType property contain invalid OS chars for file names (invalid chars: " + Path.GetInvalidFileNameChars().ToString() + ")", "value");

				else
					this.type = value;
			}
		}

        /// <summary>
        /// Source (method, class or custom value) that is writing the Log
        /// <para xml:lang="es">
        /// Fuente(Método de la clase o valor personalizado) que se registraron en el log
        /// </para>
        /// </summary>
        public string Source;

        /// <summary>
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </summary>
        public string Message;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new Log
        /// <para xml:lang="es">
        /// Crea un nuevo log
        /// </para>
		/// </summary>
		public Log() : this(string.Empty, string.Empty, Log.Information) { }

        /// <summary>
        /// Creates a new Log
        /// <para xml:lang="es">
        /// Crea un nuevo log
        /// </para>
        /// </summary>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        public Log(string message) : this(string.Empty, message, Log.Information) { }

        /// <summary>
        /// Creates a new Log
        /// <para xml:lang="es">
        /// Crea un nuevo log
        /// </para>
        /// </summary>
        /// <param name="source">
        /// Source (method, class or custom value) that is writing the Log
        /// <para xml:lang="es">
        /// Fuente(Método de la clase o valor personalizado) que se registraron en el log
        /// </para>
        /// </param>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        public Log(string source, string message) : this(source, message, Log.Information) { }

        /// <summary>
        /// Creates a new Log
        /// <para xml:lang="es">
        /// Crea un nuevo log
        /// </para>
        /// </summary>
        /// <param name="source">
        /// Source (method, class or custom value) that is writing the Log
        /// <para xml:lang="es">
        /// Fuente(Método de la clase o valor personalizado) que se registraron en el log
        /// </para>
        /// </param>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        /// <param name="logType">
        /// The kind of logs that this log contains. This will be the name of the log file
        /// <para xml:lang="es">
        /// El tipo de registros que contiene este registro. Este será el nombre del archivo de registro
        /// </para>
        /// </param>
        public Log(string source, string message, string logType)
		{
			Source = source;
			Message = message;
			Type = logType;
		}

        #endregion

        #region Methods

        /// <summary>
        /// Gets the path where the log is going to be read/written
        /// <para xml:lang="es">
        /// Obtiene la ruta en la que se va a leer/escribir el registro
        /// </para>
        /// </summary>
        /// <example>
        /// "C:\MyApp\Custom\Logs\Information.log"
        /// <para xml:lang
        /// </example>
        public string FullPath
		{
			get { return DirectoryPath + Type + ".log"; }
		}

        /// <summary>
        /// Writes this log at the end of a log file
        /// <para xml:lang="es">
        /// Escribe este registro al final de un archivo de registro
        /// </para>
        /// </summary>
        public void Write()
		{
			//Local vars
			StreamWriter writer = null;

			//Validating if message is null or empty
			if (string.IsNullOrWhiteSpace(Message))
			{
				throw new InvalidOperationException("Can't write the log because the Message is empty");
			}

			else
			{
				//Prevent null values to eliminate TABs from them
				if (string.IsNullOrWhiteSpace(Source)) Source = "Unknown";

				try
				{
					//Creating file content
					string content =
						DateTime.Now.ToShortDateString() +
						"	" +
						DateTime.Now.ToLongTimeString() +
						"	" +
						Source.Replace('	', ' ') +
						"	" +
						Message.Replace('	', ' ');

					//Creating cursor to file
					writer = File.AppendText(FullPath);

					//Writing content
					writer.WriteLine(content);
				}
				catch{}

				/* On error, this is re - throwed to upper layers */ 

				finally
				{
					//Releasing StreamWriter...
					if (writer != null)
					{
						writer.Close();
						writer = null;
					}
				}
			}
		}

        #endregion

        #region Static

        /// <summary>
        /// Static constructor for class
        /// <para xml:lang="es">
        /// Constructor estático para la clase
        /// </para>
        /// </summary>
        static Log()
		{
			//Ensuring the DirectoryPath exists
			if (!Directory.Exists(DirectoryPath))
			{
				Directory.CreateDirectory(DirectoryPath);
			}
		}

        /// <summary>
        /// Gets the directory path where the logs are going to be read/written
        /// <para xml:lang="es">
        /// Obtiene la ruta del directorio donde se van a leer/escribir los registros
        /// </para>
        /// </summary>
        /// <example>
        /// C:\MyApp\Custom\Logs\
        /// </example>
        public static string DirectoryPath
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory + @"Custom\Logs\";
			}
		}

        /// <summary>
        /// Writes a log message at the end of a log file
        /// <para xml:lang="es">
        /// Escribe un mensaje de registro al final de un archivo de registro
        /// </para>
        /// </summary>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        public static Log Write(string message)
		{
			return Write(string.Empty, message, Log.Information);
		}

        /// <summary>
        /// Writes a log message at the end of a log file
        /// <para xml:lang="es">
        /// Escribe un mensaje de registro al final de un archivo de registro
        /// </para>
        /// </summary>
        /// <param name="source">
        /// Source (method, class or custom value) that is writing the Log
        /// <para xml:lang="es">
        /// Fuente (Método de la clase o valor personalizado) que se registraron en el log
        /// </para>
        /// </param>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        public static Log Write(string source, string message)
		{
			return Write(source, message, Log.Information);
		}

        /// <summary>
        /// Writes a log message at the end of a log file
        /// <para xml:lang="es">
        /// Escribe un mensaje de registro al final de un archivo de registro
        /// </para>
        /// </summary>
        /// <param name="source">
        /// Source (method, class or custom value) that is writing the Log
        /// <para xml:lang="es">
        /// Fuente (Método de la clase o valor personalizado) que se registraron en el log
        /// </para>
        /// </param>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        /// <param name="logType">
        /// The kind of logs that this log contains. This will be the name of the log file
        /// <para xml:lang="es">
        /// El tipo de registros que contiene este registro. Este será el nombre del archivo de registro
        /// </para>
        /// </param>
        /// <returns>
        /// Log file
        /// <para xml:lang="es">
        /// Archivo log
        /// </para>
        /// </returns>
        public static Log Write(string source, string message, string logType)
		{
			//Creating log...
			Log log = new Log(source, message, logType);

			//Writting to media...
			log.Write();

			//Returning to caller...
			return log;
		}

        /// <summary>
        /// Writes a log message at the end of a log file only 
        /// if the DEBUG symbol is present at compilation time
        /// <para xml:lang="es">
        /// Escribe un mensaje de registro al final de un archivo
        /// de registro sólo si el símbolo DEBUG está presente en tiempo de compilación
        /// </para>
        /// </summary>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        [Conditional("DEBUG")]
		public static void WriteDebug(string message)
		{
			WriteDebug(string.Empty, message);
		}

        /// <summary>
        /// Writes a log message at the end of a log file only 
        /// if the DEBUG symbol is present at compilation time
        /// <para xml:lang="es">
        /// Escribe un mensaje de registro al final de un archivo
        /// de registro sólo si el símbolo DEBUG está presente en tiempo de compilación
        /// </para>
        /// </summary>
        /// <param name="source">
        /// Source (method, class or custom value) that is writing the Log
        /// <para xml:lang="es">
        /// Fuente (Método de la clase o valor personalizado) que se registraron en el log
        /// </para>
        /// </param>
        /// <param name="message">
        /// Message of the log, the log itself
        /// <para xml:lang="es">
        /// Mensaje del registro, el propio log
        /// </para>
        /// </param>
        [Conditional("DEBUG")]
		public static void WriteDebug(string source, string message)
		{
			Write(source, message, Debug);
		}

		#endregion
	}
}