using System;
using System.Collections.Generic;
using System.Web;
using System.Diagnostics;
using System.Threading;

namespace OKHOSTING.Core.Net4.Net
{
	/// <summary>
	/// Represents a user session in an application, that can be used in web and windows environments
	/// for storing custom session values
	/// </summary>
	public class Session: Dictionary<string, object>, IDisposable
	{
		/// <summary>
		/// Creates a new session instance and invokes OnSession_Start
		/// </summary>
		/// <param name="id">Session ID for the current session</param>
		private Session(string id)
		{
			this.SessionId = id;
		}

		/// <summary>
		/// Destroys the current session instance and invokes OnSession_End
		/// </summary>
		~Session()
		{
			//End();
		}

		/// <summary>
		/// Gets the unique identifier for the session.
		/// </summary>
		public readonly string SessionId;

		/// <summary>
		/// Returns the current sesion Id
		/// </summary>
		public override string ToString()
		{
			return this.SessionId;
		}

		/// <summary>
		/// Ends the current session and clears all session data
		/// </summary>
		public void End()
		{
			OnSession_End();
		}

		#region Static

		/// <summary>
		/// Used internally to create random session ID's
		/// </summary>
		private static Random Random = new Random();
		
		/// <summary>
		/// Retrieves the current session's unique ID
		/// </summary>
		private static string CurrentSessionID
		{
			get
			{
				//Local vars
				string id = null;

				//no longer rely on ASP.NET Session since it's not available on HttpContext.BeginRequest event, rely on cookies instead
				//if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session.SessionID != null)
				//{
				//	//Web Application; Using the SessionID of Session Object
				//	id = HttpContext.Current.Session.SessionID;
				//}

				//this is a web application
				if (HttpContext.Current != null)
				{
					HttpCookie sessionCookie = HttpContext.Current.Request.Cookies["OKHOSTING.Core.Session.Id"];
					
					if (sessionCookie == null)
					{
						sessionCookie = new HttpCookie("OKHOSTING.Core.Session.Id", Session.Random.Next().ToString());
						sessionCookie.Expires = DateTime.Now.AddMinutes(60);
						HttpContext.Current.Response.AppendCookie(sessionCookie);
					}
					
					id = sessionCookie.Value;
				}

				//this is a windows application
				else
				{
					//Windows Application; Using the process and thread ID
					id = Process.GetCurrentProcess().Id.ToString().Trim() + Thread.CurrentThread.ManagedThreadId.ToString().Trim();
				}

				//Retrieving the session ID
				return id;
			}
		}

		/// <summary>
		/// Stores the databases objects currently loaded
		/// </summary>
		private static readonly Dictionary<string, Session> Sessions;

		/// <summary>
		/// Initiates the static session collection
		/// </summary>
		static Session()
		{
			Sessions = new Dictionary<string, Session>();
		}

		/// <summary>
		/// Used to perform Lock operations
		/// </summary>
		public static object Locker = new object();

		/// <summary>
		/// Retrieve the Session associated to the current process
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
		/// </summary>
		public static event EventHandler Session_Start;

		/// <summary>
		/// Raised when a session is ended
		/// </summary>
		public static event EventHandler Session_End;

		/// <summary>
		/// Invoked when a new session is started. Raises Session_Start event and invokes DataType.OnSessionStart() in all loaded DataTypes
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

			//remove cookie if we are in a web context
			if (HttpContext.Current != null)
			{
				HttpCookie sessionCookie = HttpContext.Current.Request.Cookies["OKHOSTING.Core.Session.Id"];

				if (sessionCookie != null)
				{
					sessionCookie.Expires = DateTime.Now.AddDays(-1);
					HttpContext.Current.Response.Cookies.Add(sessionCookie);
				}
			}
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