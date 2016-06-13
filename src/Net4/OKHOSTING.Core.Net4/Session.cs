using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace OKHOSTING.Core.Net4
{
    /// <summary>
    /// Represents a user session in an application, that can be used in web and windows environments
    /// for storing custom session values
    /// <para xml:lang="es">
    /// Representa una sesión de usuario en una aplicación, que se puede utilizar en la web y entornos
    /// Windows para almacenar valores de sesión personalizados
    /// </para>
	/// </summary>
	public class Session: Dictionary<string, object>, IDisposable
	{
        /// <summary>
        /// Creates a new session instance and invokes OnSession_Start
        /// <para xml:lang="es">
        /// Crea una nueva instancia de la sesión e invocar Session_Start
        /// </para>
        /// </summary>
        /// <param name="id">
        /// Session ID for the current session
        /// <para xml:lang="es">
        /// Id de sessión para la actual sessión
        /// </para>
        /// </param>
        private Session(string id)
		{
			this.SessionId = id;
		}

        /// <summary>
        /// Destroys the current session instance and invokes OnSession_End
        /// <para xml:lang="es">
        /// Destruye la instancia actual período de sesiones e invocar Session_End
        /// </para>
        /// </summary>
        ~Session()
		{
			//End();
		}

		/// <summary>
		/// Gets the unique identifier for the session.
        /// <para xml:lang="es">
        /// Obtiene el unico identificador para la sessión
        /// </para>
		/// </summary>
		public readonly string SessionId;

		/// <summary>
		/// Returns the current sesion Id
        /// <para xml:lang="es">
        /// Retorna el id de la actual sessión
        /// </para>
		/// </summary>
		public override string ToString()
		{
			return this.SessionId;
		}

        /// <summary>
        /// Ends the current session and clears all session data
        /// <para xml:lang="es">
        /// Finaliza la sesión actual y borrar todos los datos de la sesión
        /// </para>
        /// </summary>
        public void End()
		{
			OnSession_End();
		}

		#region Static

		/// <summary>
		/// Used internally to create random session ID's
        /// <para xml:lang="es">
        /// Usado internamente para crear un id aleatorio de la sessión
        /// </para>
		/// </summary>
		private static Random Random = new Random();

        /// <summary>
        /// Retrieves the current session's unique ID
        /// <para xml:lang="es">
        /// Recupera la identificación única del actual período de sesiones
        /// </para>
        /// </summary>
        private static string CurrentSessionID
		{
			get
			{
				string id = null;

				//Windows Application; Using the process and thread ID
				id = Process.GetCurrentProcess().Id.ToString().Trim() + Thread.CurrentThread.ManagedThreadId.ToString().Trim();

				//Retrieving the session ID
				return id;
			}
		}

        /// <summary>
        /// Stores the databases objects currently loaded
        /// <para xml:lang="es">
        /// Almacena las bases de datos de objetos actualmente cargada
        /// </para>
        /// </summary>
        private static readonly Dictionary<string, Session> Sessions = new Dictionary<string, Session>();

        /// <summary>
        /// Used to perform Lock operations
        /// <para xml:lang="es">
        /// Se utiliza para realizar operaciones de bloqueo
        /// </para>
        /// </summary>
        public static object Locker = new object();

        /// <summary>
        /// Retrieve the Session associated to the current process
        /// <para xml:lang="es">
        /// Recuperar la sesión asociada al proceso actual
        /// </para>
        /// </summary>
        public static Session Current
		{
			get
			{
				lock (Locker)
				{
					//Getting the session ID
					string id = Session.CurrentSessionID;

					//Retrieving the Session if it's defined, otherwise creating 
					//the session saves in databases collection and retrieving
					if (Sessions.ContainsKey(id))
					{
						return Sessions[id];
					}
					else
					{
						//Loading the Session
						Session current = new Session(id);

						//Storing the Session Object in Sessions collection
						Sessions.Add(id, current);

						//call OnSession_Start to raise Session_Start event
						current.OnSession_Start();

						//Retrieving Session recently created
						return current;
					}
				}
			}
		}

        /// <summary>
        /// Raised when a new session is started
        /// <para xml:lang="es">
        /// Se genera cuando se inicia una nueva sesión
        /// </para>
        /// </summary>
        public static event EventHandler Session_Start;

		/// <summary>
		/// Raised when a session is ended
        /// <para xml:lang="es">
        /// Se genera cuando se termina una sessión
        /// </para>
		/// </summary>
		public static event EventHandler Session_End;

        /// <summary>
        /// Invoked when a new session is started. Raises Session_Start event and invokes DataType.OnSessionStart() in all loaded DataTypes
        /// <para xml:lang="es">
        /// Se invoca cuando se inicia una nueva sesión. Eleva caso Session_Start e invoca DataType.OnSessionStart () en todos los tipos de datos cargados
        /// </para>
        /// </summary>
        private void OnSession_Start()
		{
			////Run PlugIn_OnSessionStart method for all plugins installed and enabled
			//foreach (Configuration.PlugIn plugin in Configuration.Current.PlugIns)
			//{
			//	if (plugin.Enabled)
			//	{
			//		plugin.InvokeOnSessionStartMethod();
			//	}
			//}
			
			if (Session_Start != null) Session_Start(this, new EventArgs());
		}

		/// <summary>
		/// Invoked when a session is ended. Raises Session_End event and invokes DataType.OnSessionEnd() in all loaded DataTypes
		/// <para xml:lang=""
        /// </summary>
		private void OnSession_End()
		{
			////Run PlugIn_OnSessionStart method for all plugins installed and enabled
			//foreach (Configuration.PlugIn plugin in Configuration.Current.PlugIns)
			//{
			//	if (plugin.Enabled)
			//	{
			//		plugin.InvokeOnSessionEndMethod();
			//	}
			//}

			//raise events
			if (Session_End != null) Session_End(this, new EventArgs());
			
			//clear all session data
			this.Clear();

			//remove from the sessions collection
			Sessions.Remove(this.SessionId);
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			base.Clear();
		}

		#endregion
	}
}