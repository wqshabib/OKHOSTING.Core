/*
 * 
 * Runtime Web.config / App.config Editing
 * By Peter A. Bromberg, Ph.D.
 * Downloaded from http://www.eggheadcafe.com/articles/20030907.asp
 * Modified By Edgard David Ivan Muñoz Chávez
 * 
*/

using System;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Reflection;
using System.Diagnostics;

namespace OKHOSTING.Core
{
	/// <summary>
	/// Allows read/write configuration in App.config files for Windows and Web.config for web
	/// </summary>
	public class WebConfig : AppConfig
	{
		/// <summary>
		/// Creates a new instance and loads the current app configuration
		/// </summary>
		public WebConfig(): base(System.Web.HttpContext.Current.Server.MapPath("web.config"))
		{
		}
	}
}