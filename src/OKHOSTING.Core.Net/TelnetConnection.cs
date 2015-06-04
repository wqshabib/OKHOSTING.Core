// minimalistic telnet implementation
// conceived by Tom Janssens on 2007/06/06  for codeproject
//
// http://www.corebvba.be

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace OKHOSTING.Core.Net
{
	/// <summary>
	/// Telnet verbs
	/// </summary>
	enum Verbs
	{
		WILL = 251,
		WONT = 252,
		DO = 253,
		DONT = 254,
		IAC = 255
	}

	/// <summary>
	/// Telnet options
	/// </summary>
	enum Options
	{
		SGA = 3
	}
		
	/// <summary>
	/// Implements the funcionality for telnet protocol
	/// </summary>
	public class TelnetConnection
	{

		#region Fields

		/// <summary>
		/// Private TCP socket used for telnet comunication
		/// </summary>
		private TcpClient tcpSocket;

		/// <summary>
		/// Timeout for telnet commands execution (on milliseconds)
		/// </summary>
		public int Timeout;

		#endregion

		#region Constructors and destructors

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="Hostname">
		/// Host name or address
		/// </param>
		/// <param name="Port">
		/// Port for comunication
		/// </param>
		public TelnetConnection(string Hostname, int Port) : this(Hostname, Port, 1000) { }

		/// <summary>
		/// Class constructor
		/// </summary>
		/// <param name="Hostname">
		/// Host name or address
		/// </param>
		/// <param name="Port">
		/// Port for comunication
		/// </param>
		/// <param name="Timeout">
		/// Timeout for telnet commands execution (on milliseconds)
		/// </param>
		public TelnetConnection(string Hostname, int Port, int Timeout)
		{
			//Initializing socket and comunications timeout
			tcpSocket = new TcpClient(Hostname, Port);
			this.Timeout = Timeout;
		}

		/// <summary>
		/// Class destructor
		/// </summary>
		~TelnetConnection() { Close(); }

		#endregion

		#region Telnet client implementation

		/// <summary>
		/// Return a boolean value that indicates if the connection is currently established
		/// </summary>
		public bool IsConnected
		{ get { return tcpSocket.Client.Connected; } }

		/// <summary>
		/// Try to do login on telnet server
		/// </summary>
		/// <param name="Username">
		/// User name for login
		/// </param>
		/// <param name="Password">
		/// Password for login
		/// </param>
		/// <returns>
		/// Output from operation
		/// </returns>
		public string Login(string Username, string Password)
		{ return Login(Username, Password, this.Timeout); }

		/// <summary>
		/// Try to do login on telnet server
		/// </summary>
		/// <param name="Username">
		/// User name for login
		/// </param>
		/// <param name="Password">
		/// Password for login
		/// </param>
		/// <param name="LoginTimeOutMs">
		/// Timeout for login operation (On milliseconds)
		/// </param>
		/// <returns>
		/// Output from operation
		/// </returns>
		public string Login(string Username, string Password, int LoginTimeOutMs)
		{

			//Reading input...
			string s = Read(LoginTimeOutMs);
			
			//Validating input
			if (!s.TrimEnd().EndsWith(":")) throw new Exception("Failed to connect : no login prompt");

			//Writting Username on TCP stream
			WriteLine(Username);

			//Reading input
			s += Read(LoginTimeOutMs);

			//Validating input
			if (!s.TrimEnd().EndsWith(":")) throw new Exception("Failed to connect : no password prompt");

			//Writing password
			WriteLine(Password);

			//Reading input
			s += Read(LoginTimeOutMs);

			//Returning result
			return s;
		}

		/// <summary>
		/// Write a telnet command on TCP stream (ended with \n)
		/// </summary>
		/// <param name="cmd">
		/// Command to write
		/// </param>
		public void WriteLine(string cmd)
		{ Write(cmd + "\n"); }

		/// <summary>
		/// Write a telnet command on TCP stream
		/// </summary>
		/// <param name="cmd">
		/// Command to write
		/// </param>
		public void Write(string cmd)
		{
			//Validating if telnet connection is open
			if (tcpSocket.Client.Connected)
			{
				//Preparing command to be written
				byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("\0xFF", "\0xFF\0xFF"));

				//Writting command on stream
				tcpSocket.GetStream().Write(buf, 0, buf.Length);
			}
		}

		/// <summary>
		/// Process the current telnet comunication with default comunication timeout
		/// </summary>
		/// <returns>
		/// Telnet comunication result
		/// </returns>
		public string Read()
		{ return this.Read(this.Timeout); }
		
		/// <summary>
		/// Process the current telnet comunication
		/// </summary>
		/// <param name="Timeout">
		/// Timeout for telnet commands execution (on milliseconds)
		/// </param>
		/// <returns>
		/// Telnet comunication result
		/// </returns>
		public string Read(int Timeout)
		{
			//Local vars
			string result = null;

			//Validating if tcp socket is already connected
			if (tcpSocket.Client.Connected)
			{
				//Creating string builder
				StringBuilder sb = new StringBuilder();

				do
				{
					//Processing...
					ParseTelnet(sb);
					
					//Waiting for response...
					Thread.Sleep(Timeout);

				} while (tcpSocket.Client.Available > 0);	//Reading until end of response...

				//Getting the string result
				result = sb.ToString();
			}

			//Returning the result
			return result;
		}

		/// <summary>
		/// Process the current telnet comunication
		/// </summary>
		/// <param name="sb">
		/// StringBuilder variable for temporaly stores comunication input
		/// </param>
		private void ParseTelnet(StringBuilder sb)
		{
			//Reading response...
			while (tcpSocket.Client.Available > 0)
			{
				//Loading input 
				int input = tcpSocket.GetStream().ReadByte();
				
				//Processing the input
				switch (input)
				{
						//End of input...
					case -1: 
						break;

						//IAC Verb
					case (int)Verbs.IAC:
						//Interpret as command
						int inputverb = tcpSocket.GetStream().ReadByte();
						if (inputverb == -1) break;

						//Processing...
						switch (inputverb)
						{
							case (int)Verbs.IAC:
								//Literal IAC = 255 escaped, so append char 255 to string
								sb.Append(inputverb);
								break;

							case (int)Verbs.DO:
							case (int)Verbs.DONT:
							case (int)Verbs.WILL:
							case (int)Verbs.WONT:
								//Reply to all commands with "WONT", unless it is SGA (suppres go ahead)
								int inputoption = tcpSocket.GetStream().ReadByte();
								if (inputoption == -1) break;

								tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);

								if (inputoption == (int)Options.SGA)
									tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);

								else
									tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);

								tcpSocket.GetStream().WriteByte((byte)inputoption);
								break;

							default:
								break;
						}
						break;

						//Acumulating input on buffer
					default:
						sb.Append((char)input);
						break;
				}
			}
		}

		/// <summary>
		/// Closes the current TCP Connection and in error case ignores it
		/// </summary>
		public void Close()
		{
			try { tcpSocket.Close(); }
			catch { /* Ignoring error */ }
		}

		#endregion

	}
}