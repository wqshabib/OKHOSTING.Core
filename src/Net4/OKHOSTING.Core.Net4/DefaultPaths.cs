using System;

namespace OKHOSTING.Core.Net4
{
    /// <summary>
    /// Defines folder paths for common uses in applications
    /// <para xml:lang="es">
    /// Define las rutas de carpetas para los usos comunes en aplicaciones
    /// </para>
    /// </summary>
    public class DefaultPaths
	{
        /// <summary>
        /// Gets the base path where the application is running
        /// <para xml:lang="es">
        /// Obtiene la ruta de la base donde se ejecuta la aplicación
        /// </para>
        /// </summary>
        public static string Base
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

        /// <summary>
        /// Gets the custom directory path. This is a read-write directory where all custom files should be placed
        /// <para xml:lang="es">
        /// Obtiene la ruta del directorio personalizado. Este es un directorio de lectura-escritura, donde se deben colocar todos los archivos personalizados
        /// </para>
        /// </summary>
        /// <remarks>
        /// Administrator must manually set read-write permissions to this directory (and subdirectories) on system setup
        /// <para xml:lang="es">
        /// Administrador debe configurar manualmente permisos a este directorio (y subdirectorios) en la configuración del sistema de escritura-lectura
        /// </para>
        /// </remarks>
        public static string Custom
		{
			get
			{
				return System.IO.Path.Combine(Base, "Custom");
			}
		}
	}
}