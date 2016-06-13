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
    /// <para xml:lang="es">
    /// Permite leer / escritura la configuración de archivos App.config para Windows y Web.config para la web
    /// </para>
    /// </summary>
    public class AppConfig : System.Configuration.AppSettingsReader
	{
		/// <summary>
		/// Creates a new instance and loads the current app configuration
        /// <para xml:lang="es">
        /// Crea una nueva instancia y carga la actual configuracion de la aplicacion
        /// </para>
		/// </summary>
		public AppConfig(): this(System.Web.HttpContext.Current == null ? ((Assembly.GetEntryAssembly()).GetName()).Name + ".exe.config" : System.Web.HttpContext.Current.Server.MapPath("~/Web.config"))
		{
		}

        /// <summary>
        /// Creates a new instance and loads the current app configuration
        /// <para xml:lang="es">
        /// Crea una nueva instancia y carga la actual configuracion de la aplicacion
        /// </para>
        /// </summary>
        /// <param name="fileName">
        /// file name
        /// <para xml:lang="es">
        /// Nombre del archivo
        /// </para>
        /// </param>
		protected AppConfig(string fileName)
		{
			// load the config file 
			FileName = fileName;

			Content = new XmlDocument();
			Content.Load(FileName);
		}

		/// <summary>
		/// Path for the current configuration file
        /// <para xml:lang="es">
        /// Ruta del archivo de configuracion actual
        /// </para>
		/// </summary>
		public readonly string FileName = String.Empty;

		/// <summary>
		/// Content of the current configuration
        /// <para xml:lang="es">
        /// Contenido de la actual configuracion
        /// </para>
		/// </summary>
		public readonly XmlDocument Content;

        /// <summary>
        /// Sets a value in the appSettings section
        /// <para xml:lang="es">
        /// Establece un valor en la sección appsettings
        /// </para>
        /// </summary>
        /// <param name="key">
        /// Key of the value
        /// <para xml:lang="es">
        /// Clave del valor
        /// </para>
        /// </param>
        /// <param name="value">
        /// Actual value
        /// <para xml:lang="es">
        /// Valor actual
        /// </para>
        /// </param>
        /// <returns>
        /// True is change was succesfull, false otherwise
        /// <para xml:lang="es">
        /// True si el cambio se realizo correctamente, de lo contrario devuelve false
        /// </para>
        /// </returns>
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
        /// <para xml:lang="es">
        /// Elimina un valor en la sección appsettings
        /// </para>
        /// </summary>
        /// <param name="key">
        /// Key of the value
        /// <para xml:lang="es">
        /// Clave del valor
        /// </para>
        /// </param>
        /// <returns>
        /// True is change was succesfull, false otherwise
        /// <para xml:lang="es">
        /// True si el cambio se realizo correctamente, de lo contrario devuelve false
        /// </para>
        /// </returns>
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
        /// <para xml:lang="es">
        /// Actualiza una sección de la configuración de la aplicación, por lo que se vuelve a cargar desde el disco
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// Name of the section to refresh
        /// <para xml:lang="es">
        /// Nombre de la seccion a refrescar
        /// </para>
        /// </param>
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