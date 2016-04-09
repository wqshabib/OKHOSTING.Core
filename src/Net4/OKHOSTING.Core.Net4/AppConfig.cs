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
using System.Reflection;

namespace OKHOSTING.Core.Net4
{
	/// <summary>
	/// Allows read/write configuration in App.config files for Windows and Web.config for web
	/// </summary>
	public class AppConfig : System.Configuration.AppSettingsReader
	{
		/// <summary>
		/// Creates a new instance and loads the current app configuration
		/// </summary>
		public AppConfig(): this(System.Web.HttpContext.Current == null ? ((Assembly.GetEntryAssembly()).GetName()).Name + ".exe.config" : System.Web.HttpContext.Current.Server.MapPath("~/Web.config"))
		{
		}

		protected AppConfig(string fileName)
		{
			// load the config file 
			FileName = fileName;

			Content = new XmlDocument();
			Content.Load(FileName);
		}

		/// <summary>
		/// Path for the current configuration file
		/// </summary>
		public readonly string FileName = String.Empty;

		/// <summary>
		/// Content of the current configuration
		/// </summary>
		public readonly XmlDocument Content;

		/// <summary>
		/// Sets a value in the appSettings section
		/// </summary>
		/// <param name="key">Key of the value</param>
		/// <param name="value">Actual value</param>
		/// <returns>True is change was succesfull, false otherwise</returns>
		public void SetAppSetting(string key, string value)
		{
			XmlNode settingsNode = null;

			// retrieve the appSettings node 
			settingsNode = Content.SelectSingleNode("//appSettings");

			if (settingsNode == null)
			{
				settingsNode = Content.CreateElement("appSettings");
				Content.DocumentElement.AppendChild(settingsNode);
			}

			// XPath select setting "add" element that contains this key	
			XmlElement addElem = (XmlElement)settingsNode.SelectSingleNode("//add[@key='" + key + "']");
			if (addElem != null)
			{
				addElem.SetAttribute("value", value);
			}
			// not found, so we need to add the element, key and value
			else
			{
				XmlElement entry = Content.CreateElement("add");
				entry.SetAttribute("key", key);
				entry.SetAttribute("value", value);
				settingsNode.AppendChild(entry);
			}

			//save it
			Save();
		}

		/// <summary>
		/// Removes a value in the appSettings section
		/// </summary>
		/// <param name="key">Key of the value</param>
		/// <returns>True is change was succesfull, false otherwise</returns>
		public void RemoveAppSetting(string key)
		{
			XmlNode settingsNode = null;

			// retrieve the appSettings node 
			settingsNode = Content.SelectSingleNode("//appSettings");

			if (settingsNode == null)
			{
				settingsNode = Content.CreateElement("appSettings");
				Content.DocumentElement.AppendChild(settingsNode);
			}

			// XPath select setting "add" element that contains this key to remove   
			settingsNode.RemoveChild(settingsNode.SelectSingleNode("//add[@key='" + key + "']"));

			Save();
		}

		/// <summary>
		/// Refreshes a section from the app configuration, so it is reloded from disk
		/// </summary>
		/// <param name="sectionName">Name of the section to refresh</param>
		public void RefreshSection(string sectionName)
		{
			ConfigurationManager.RefreshSection(sectionName);
		}

		/// <summary>
		/// Saves the content of the Content XmlDocument into the app configuration file
		/// </summary>
		public void Save()
		{
			//save to file
			XmlTextWriter writer = new XmlTextWriter(FileName, null);
			writer.Formatting = Formatting.Indented;
			Content.WriteTo(writer);
			writer.Flush();
			writer.Close();
		}
	}
}