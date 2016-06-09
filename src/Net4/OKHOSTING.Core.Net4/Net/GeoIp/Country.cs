/**
 * Country.cs
 *
 * Copyright (C) 2008 MaxMind Inc.  All Rights Reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


using System;
using System.IO;

namespace OKHOSTING.Core.Net4.Net.GeoIp
{
    /// <summary>
    /// It represents the basic data of a country
    /// <para xml:lang="es">
    /// Representa los datos basicos de un pais
    /// </para>
    /// </summary>
	public class Country
	{
        /// <summary>
        /// It represents the code of a country
        /// <para xml:lang="es">
        /// Representa el codigo de un pais
        /// </para>
        /// </summary>
		private String code;

        /// <summary>
        /// It represents the name of a country
        /// <para xml:lang="es">
        /// Representa el nombre de un pais
        /// </para>
        /// </summary>
		private String name;

        /// <summary>
        /// Get the code and name of a country and initializes the class
        /// <para xml:lang="es">
        /// Recibe el codigo y nombre de un pais e inicializa la clase
        /// </para>
        /// </summary>
        /// <param name="code">
        /// Country code
        /// <para xml:lang="es">
        /// Codigo del pais
        /// </para>
        /// </param>
        /// <param name="name">
        /// Country name
        /// <para xml:lang="es">
        /// Nombre del pais
        /// </para>
        /// </param>
        public Country(String code, String name)
		{
			this.code = code;
			this.name = name;
		}

        /// <summary>
        /// Returns the code of a country
        /// <para xml:lang="es">
        /// Devuelve el codigo de un pais
        /// </para>
        /// </summary>
        /// <returns>
        /// Returns the country code
        /// <para xml:lang="es">
        /// Retorna el codigo del pais
        /// </para>
        /// </returns>
        public String getCode()
		{
			return code;
		}

        /// <summary>
        /// Returns the name of a country
        /// <para xml:lang="es">
        /// Devuelve el nombre de un pais
        /// </para>
        /// </summary>
        /// <returns>
        /// Returns the country name
        /// <para xml:lang="es">
        /// Retorna el nombre del pais
        /// </para>
        /// </returns>
        public String getName()
		{
			return name;
		}
	}
}