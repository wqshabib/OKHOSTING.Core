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
    /// <para xml:lang="es">
    /// Permite obtener fácilmente un contenido de páginas Web
    /// </para>
    /// </summary>
    public class HttpConnection
	{
        /// <summary>
        /// It represents the address of the website
        /// <para xml:lang="es">
        /// representa la direccion de la pagina web
        /// </para>
        /// </summary>
		string url;

        /// <summary>
        /// It represents the method by which the content of the website is obtained
        /// <para xml:lang="es">
        /// representa el metodo por el que se obtiene el contenido de la pagina web
        /// </para>
        /// </summary>
		string method;

        /// <summary>
        /// It represents the time to complete the connection
        /// <para xml:lang="es">
        /// Representa el tiempo para terminar la conexion
        /// </para>
        /// </summary>
		int timeout;

		/// <summary>
		/// The Url that will be accesed
        /// <para xml:lang="es">
        /// La url que sera accesada
        /// </para>
		/// </summary>
		public string Url
		{
			get { return url; }
			set { url = value; }
		}

        /// <summary>
        /// Http Method that will be used to get request the web page
        /// <para xml:lang="es">
        /// Método http que se utiliza para obtener la solicitud de la página web
        /// </para>
		/// </summary>
		public string Method
		{
			get { return method; }
			set { method = value; }
		}

        /// <summary>
        /// Time in milliseconds, that will take for a request to be considered as timeout
        /// <para xml:lang="es">
        /// Tiempo en milisegundos, que se llevará a una solicitud para ser considerado como tiempo de espera
        /// </para>
        /// </summary>
        public int Timeout
		{
			get { return timeout; }
			set { timeout = value; }
		}

		/// <summary>
		/// Constructs the object
        /// <para xml:lang="es">
        /// Constructor del objeto
        /// </para>
		/// </summary>
		public HttpConnection()
		{
		}

        /// <summary>
        /// Constructs the object
        /// <para xml:lang="es">
        /// Constructor del objeto
        /// </para>
        /// </summary>
        /// <param name="url">
        /// The Url that will be accesed
        /// <para xml:lang="es">
        /// La url que sera accesada
        /// </para>
        /// </param>
        /// <param name="method">
        /// Http Method that will be used to get request the web page
        /// <para xml:lang="es">
        /// Método http que se utiliza para obtener la solicitud de la página web
        /// </para>
        /// </param>
		/// <param name="timeout">
        /// Time in milliseconds, that will take for a request to be considered as timeout
        /// <para xml:lang="es">
        /// Tiempo en milisegundos, que se llevará a una solicitud para ser considerado como tiempo de espera
        /// </para>
        /// </param>
		public HttpConnection(string url, string method, int timeout)
		{
			this.url = url;
			this.method = method;
			this.timeout = timeout;
		}

        /// <summary>
        /// Connects to the Url and returns the HttpWebResponse for handling the response
        /// <para xml:lang="es">
        /// Se conecta a la URL y devuelve el HttpWebResponse para el manejo de la respuesta
        /// </para>
        /// </summary>
        /// <returns>
        /// HttpWebResponse for handling the response
        /// <para xml:lang="es">
        /// HttpWebResponse para el manejo de la respuesta
        /// </para>
        /// </returns>
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
        /// <para xml:lang="es">
        /// Se conecta a la URL y devuelve el contenido completo obtenido en una sola cadena
        /// </para>
        /// </summary>
        /// <returns>
        /// The full content obtained from the Utl in a single string
        /// <para xml:lang="es">
        /// El contenido completo obtenido de la URL en una sola cadena
        /// </para>
        /// </returns>
        public string GetResponseString()
		{
			return GetResponseString(0);
		}

        /// <summary>
        /// Connects to the Url and returns the content obtained in a single string
        /// <para xml:lang="es">
        /// Se conecta a la URL y devuelve el contenido obtenido en una sola cadena
        /// </para>
        /// </summary>
        /// <param name="maxLenght">
        /// Maximum ammount of bytes that will be read from the Url
        /// <para xml:lang="es">
        /// Cantidad máxima de bytes que se lee de la URL
        /// </para>
        /// </param>
        /// <returns>
        /// The content obtained from the Url in a single string, delimited in seize by maxLenght
        /// <para xml:lang="es">
        /// El contenido obtenido de la dirección URL en una única cadena, delimitada en tomar con el maxLength
        /// </para>
        /// </returns>
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