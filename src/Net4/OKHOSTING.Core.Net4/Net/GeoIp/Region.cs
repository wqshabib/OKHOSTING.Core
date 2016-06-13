using System;
using System.IO;

namespace OKHOSTING.Core.Net4.Net.GeoIp
{
    /// <summary>
    /// It represents a region of the country
    /// <para xml:lang="es">
    /// Representa una region del pais
    /// </para>
    /// </summary>
	public class Region
	{
        /// <summary>
        /// indicates the country code
        /// <para xml:lang="es">
        /// indica el codigo del pais
        /// </para>
        /// </summary>
		public String countryCode;

        /// <summary>
        /// indicates the country name
        /// <para xml:lang="es">
        /// Indica el nombre del pais
        /// </para>
        /// </summary>
		public String countryName;

        /// <summary>
        /// indicates the name of the region of the country
        /// <para xml:lang="es">
        /// indica el nombre de la region del pais
        /// </para>
        /// </summary>
		public String region;

        /// <summary>
        /// instance of class
        /// <para xml:lang="es">
        /// Instancia de clase
        /// </para>
        /// </summary>
		public Region()
		{
		}

        /// <summary>
        /// Get the code, name of the country and region and creates a new instance
        /// <para xml:lang="es">
        /// Recibe el codigo, nombre del pais y region y crea una nueva instancia
        /// </para>
        /// </summary>
        /// <param name="countryCode">
        /// Country code
        /// <para xml:lang="es">
        /// codigo del pais
        /// </para>
        /// </param>
        /// <param name="countryName">
        /// Country name
        /// <para xml:lang="es">
        /// Nombre del pais
        /// </para>
        /// </param>
        /// <param name="region">
        /// Region name
        /// <para xml:lang="es">
        /// Nombre de la region 
        /// </para>
        /// </param>
		public Region(String countryCode, String countryName, String region)
		{
			this.countryCode = countryCode;
			this.countryName = countryName;
			this.region = region;
		}

        /// <summary>
        /// Return the country code
        /// <para xml:lang="es">
        /// Devuelve el codigo del pais
        /// </para>
        /// </summary>
        /// <returns>
        /// country code
        /// <para xml:lang="es">
        /// codigo del pais
        /// </para>
        /// </returns>
        public String getcountryCode()
		{
			return countryCode;
		}

        /// <summary>
        /// Return the country name
        /// <para xml:lang="es">
        /// Devuelve el nombre del pais
        /// </para>
        /// </summary>
        /// <returns>
        /// Country name
        /// <para xml:lang="es">
        /// Nombre del pais
        /// </para>
        /// </returns>
        public String getcountryName()
		{
			return countryName;
		}

        /// <summary>
        /// Return region of country
        /// <para xml:lang="es">
        /// Devuelve la region del pais
        /// </para>
        /// </summary>
        /// <returns>
        /// Name of region
        /// <para xml:lang="es">
        /// Nombre de la region
        /// </para>
        /// </returns>
        public String getregion()
		{
			return region;
		}
	}


}