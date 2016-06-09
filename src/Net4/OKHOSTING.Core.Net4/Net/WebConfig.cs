/*
 * 
 * Runtime Web.config / App.config Editing
 * By Peter A. Bromberg, Ph.D.
 * Downloaded from http://www.eggheadcafe.com/articles/20030907.asp
 * Modified By Edgard David Ivan Muñoz Chávez
 * 
*/
namespace OKHOSTING.Core.Net4.Net
{
    /// <summary>
    /// Allows read/write configuration in App.config files for Windows and Web.config for web
    /// <para xml:lang="es">
    /// Permite leer/escribir la configuración de archivos App.config para Windows y Web.config para la web
    /// </para>
    /// </summary>
    public class WebConfig : Core.Net4.AppConfig
	{
        /// <summary>
        /// Creates a new instance and loads the current app configuration
        /// <para xml:lang="es">
        /// Crea una nueva instancia y carga la configuración de aplicación actual
        /// </para>
        /// </summary>
        public WebConfig(): base(System.Web.HttpContext.Current.Server.MapPath("web.config"))
		{
		}
	}
}