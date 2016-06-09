/**
 * DatabaseInfo.java
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
    /// It represents the information database
    /// <para xml:lang="es">
    /// Representa la informacion de la base de datos
    /// </para>
    /// </summary>
	public class DatabaseInfo
	{
        /// <summary>
        /// 
        /// </summary>
		public static int COUNTRY_EDITION = 1;
        /// <summary>
        /// 
        /// </summary>
		public static int REGION_EDITION_REV0 = 7;
        /// <summary>
        /// 
        /// </summary>
		public static int REGION_EDITION_REV1 = 3;
        /// <summary>
        /// 
        /// </summary>
		public static int CITY_EDITION_REV0 = 6;
        /// <summary>
        /// 
        /// </summary>
		public static int CITY_EDITION_REV1 = 2;
        /// <summary>
        /// 
        /// </summary>
		public static int ORG_EDITION = 5;
        /// <summary>
        /// 
        /// </summary>
		public static int ISP_EDITION = 4;
        /// <summary>
        /// 
        /// </summary>
		public static int PROXY_EDITION = 8;
        /// <summary>
        /// 
        /// </summary>
		public static int ASNUM_EDITION = 9;
        /// <summary>
        /// 
        /// </summary>
		public static int NETSPEED_EDITION = 10;

        //private static SimpleDateFormat formatter = new SimpleDateFormat("yyyyMMdd");
        /// <summary>
        /// Determines the information database
        /// <para xml:lang="es">
        /// Determina la informacion de la base de datos
        /// </para>
        /// </summary>
        private String info;

        /// <summary>
        /// Creates a new DatabaseInfo object given the database info String.
        /// <para xml:lang="es">
        /// Crea una nueva base de datos de información objeto dado la información de la base de datos de cadena.
        /// </para>
        /// </summary>
        /// <param name="info"></param>
        public DatabaseInfo(String info)
		{
			this.info = info;
		}

        /// <summary>
        /// Returns the type of database
        /// <para xml:lang="es">
        /// Devuelve el tipo de la base de datos
        /// </para>
        /// </summary>
        /// <returns>
        /// Type database
        /// <para xml:lang="es">
        /// Tipo de la base de datos
        /// </para>
        /// </returns>
        public int getType()
		{
			if ((info == null) | (info == ""))
			{
				return COUNTRY_EDITION;
			}
			else
			{
				// Get the type code from the database info string and then
				// subtract 105 from the value to preserve compatability with
				// databases from April 2003 and earlier.
				return Convert.ToInt32(info.Substring(4, 7)) - 105;
			}
		}

        /// <summary>
        /// Returns true if the database is the premium version.
        /// <para xml:lang="es">
        /// Devuelve true si la version de la base de datos es premium
        /// </para>
        /// </summary>
        /// <returns>
        /// True if if the data is premium version otherwise returns false
        /// <para xml:lang="es">
        /// True si si la version de datos es premium de lo contrario devuelve false
        /// </para>
        /// </returns>
		public bool isPremium()
		{
			return info.IndexOf("FREE") < 0;
		}

        /// <summary>
        /// Returns the date of the database
        /// <para xml:lang="es">
        /// Devuelve la fecha de la base de datos
        /// </para>
        /// </summary>
        /// <returns>
        /// date of the database
        /// <para xml:lang="es">
        /// Fecha de la base de datos
        /// </para>
        /// </returns>
        public DateTime getDate()
		{
			for (int i = 0; i < info.Length - 9; i++)
			{
				if (Char.IsWhiteSpace(info[i]) == true)
				{
					String dateString = info.Substring(i + 1, i + 9);
					try
					{
						//synchronized (formatter) {
						return DateTime.ParseExact(dateString, "yyyyMMdd", null);
						//}
					}
					catch (Exception e)
					{
						Console.Write(e.Message);
					}
					break;
				}
			}
			return DateTime.Now;
		}

        /// <summary>
        /// Returns the information database
        /// <para xml:lang="es">
        /// Devuelve la informacion de la base de datos
        /// </para>
        /// </summary>
        /// <returns>
        /// information database
        /// <para xml:lang="es">
        /// informacion de la base de datos
        /// </para>
        /// </returns>
		public String toString()
		{
			return info;
		}
	}
}