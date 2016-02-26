using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;

namespace OKHOSTING.Core.Net4.Net
{
	/// <summary>
	/// Allows to easily get a web page content
	/// </summary>
	public class HttpConnection
	{
		string url;
		string method;
		int timeout;

		/// <summary>
		/// The Url that will be accesed
		/// </summary>
		public string Url
		{
			get { return url; }
			set { url = value; }
		}

		/// <summary>
		/// Http Method that will be used to get request the web page
		/// </summary>
		public string Method
		{
			get { return method; }
			set { method = value; }
		}

		/// <summary>
		/// Time in milliseconds, that will take for a request to be considered as timeout
		/// </summary>
		public int Timeout
		{
			get { return timeout; }
			set { timeout = value; }
		}

		/// <summary>
		/// Constructs the object
		/// </summary>
		public HttpConnection()
		{
		}

		/// <summary>
		/// Constructs the object
		/// </summary>
		/// <param name="url">The Url that will be accesed</param>
		/// <param name="method">Http Method that will be used to get request the web page</param>
		/// <param name="timeout">Time in milliseconds, that will take for a request to be considered as timeout</param>
		public HttpConnection(string url, string method, int timeout)
		{
			this.url = url;
			this.method = method;
			this.timeout = timeout;
		}

		/// <summary>
		/// Connects to the Url and returns the HttpWebResponse for handling the response
		/// </summary>
		/// <returns>HttpWebResponse for handling the response</returns>
		public HttpWebResponse GetResponse()
		{
			HttpWebRequest request;

			// prepare the web page we will be asking for
			request = (HttpWebRequest)WebRequest.Create("http://" + this.Url);
			request.UserAgent = "OKHOSTING.WebMonitor";
			request.Method = this.Method;
			request.Timeout = this.Timeout;

			// execute the request
			return (HttpWebResponse) request.GetResponse();
		}

		/// <summary>
		/// Connects to the Url and returns the full content obtained in a single string
		/// </summary>
		/// <returns>The full content obtained from the Utl in a single string</returns>
		public string GetResponseString()
		{
			return GetResponseString(0);
		}
		
		/// <summary>
		/// Connects to the Url and returns the content obtained in a single string
		/// </summary>
		/// <param name="maxLenght">Maximum ammount of bytes that will be read from the Url</param>
		/// <returns>The content obtained from the Utl in a single string, delimited in seize by maxLenght</returns>
		public string GetResponseString(int maxLenght)
		{
			byte[] buf;
			HttpWebResponse response;
			int count;
			string tempString;

			// used on each read operation
			buf = new byte[maxLenght];

			// execute the request
			response = GetResponse();

			// we will read data via the response stream
			Stream resStream = response.GetResponseStream();

			tempString = null;
			count = 0;
			
			// fill the buffer with data
			resStream.ReadTimeout = Timeout;
			count = resStream.Read(buf, 0, maxLenght);

			// make sure we read some data
			if (count != 0)
			{
				// translate from bytes to ASCII text
				tempString = Encoding.ASCII.GetString(buf, 0, count);
			}

			// print out page source
			return tempString;
		}
	}
}