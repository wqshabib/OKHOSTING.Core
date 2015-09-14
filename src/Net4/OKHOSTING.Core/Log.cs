using System;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace OKHOSTING.Core
{

	/// <summary>
	/// Allows an easy creation and manipulation of log files
	/// </summary>
	public class Log
	{
		#region Constants
		
		/// <summary>
		/// General, non critical information
		/// </summary>
		public const string Information = "Information";

		/// <summary>
		/// Unhandled exception
		/// </summary>
		public const string Exception = "Exception";

		/// <summary>
		/// Handled exception, that does not cause an application crash
		/// </summary>
		public const string HandledException = "HandledException";

		/// <summary>
		/// Security related log
		/// </summary>
		public const string Security = "Security";

		/// <summary>
		/// Debug information
		/// </summary>
		public const string Debug = "Debug";

		#endregion

		#region Fields and properties

		/// <summary>
		/// The kind of logs that this log contains. This will be the name of the log file
		/// </summary>
		/// <example>Information, Exception, Security</example>
		private string type;

		/// <summary>
		/// The kind of logs that this log contains. This will be the name of the log file
		/// </summary>
		/// <example>Information, Exception, Security</example>
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
		/// </summary>
		public string Source;

		/// <summary>
		/// Message of the log, the log itself
		/// </summary>
		public string Message;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a new Log
		/// </summary>
		public Log() : this(string.Empty, string.Empty, Log.Information) { }

		/// <summary>
		/// Creates a new Log
		/// </summary>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		public Log(string message) : this(string.Empty, message, Log.Information) { }

		/// <summary>
		/// Creates a new Log
		/// </summary>
		/// <param name="source">
		/// Source (method, class or custom value) that is writing the Log
		/// </param>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		public Log(string source, string message) : this(source, message, Log.Information) { }

		/// <summary>
		/// Creates a new Log
		/// </summary>
		/// <param name="source">
		/// Source (method, class or custom value) that is writing the Log
		/// </param>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		/// <param name="logType">
		/// The kind of logs that this log contains. This will be the name of the log file
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
		/// </summary>
		/// <example>"C:\MyApp\Custom\Logs\Information.log"</example>
		public string FullPath
		{
			get { return DirectoryPath + Type + ".log"; }
		}

		/// <summary>
		/// Writes this log at the end of a log file
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
		/// </summary>
		/// <example>C:\MyApp\Custom\Logs\</example>
		public static string DirectoryPath
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory + @"Custom\Logs\";
			}
		}

		/// <summary>
		/// Writes a log message at the end of a log file
		/// </summary>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		public static Log Write(string message)
		{
			return Write(string.Empty, message, Log.Information);
		}

		/// <summary>
		/// Writes a log message at the end of a log file
		/// </summary>
		/// <param name="source">
		/// Source (method, class or custom value) that is writing the Log
		/// </param>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		public static Log Write(string source, string message)
		{
			return Write(source, message, Log.Information);
		}

		/// <summary>
		/// Writes a log message at the end of a log file
		/// </summary>
		/// <param name="source">
		/// Source (method, class or custom value) that is writing the Log
		/// </param>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		/// <param name="logType">
		/// The kind of logs that this log contains. This will be the name of the log file
		/// </param>
		/// <returns></returns>
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
		/// </summary>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		[Conditional("DEBUG")]
		public static void WriteDebug(string message)
		{
			WriteDebug(string.Empty, message);
		}

		/// <summary>
		/// Writes a log message at the end of a log file only 
		/// if the DEBUG symbol is present at compilation time
		/// </summary>
		/// <param name="source">
		/// Source (method, class or custom value) that is writing the Log
		/// </param>
		/// <param name="message">
		/// Message of the log, the log itself
		/// </param>
		[Conditional("DEBUG")]
		public static void WriteDebug(string source, string message)
		{
			Write(source, message, Debug);
		}

		#endregion
	}
}