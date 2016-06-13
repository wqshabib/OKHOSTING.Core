/**
 * Location.cs
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
//using System.Math;

namespace OKHOSTING.Core.Net4.Net.GeoIp
{
    /// <summary>
    /// It represents the location on the planet
    /// <para xml:lang="es">
    /// Representa la ubicación en el planeta
    /// </para>
    /// </summary>
	public class Location
	{
        /// <summary>
        /// Country code
        /// <para xml:lang="es">
        /// Codigo del pais
        /// </para>
        /// </summary>
		public String countryCode;
        /// <summary>
        /// Country name
        /// <para xml:lang="es">
        /// Nombre del pais
        /// </para>
        /// </summary>
		public String countryName;
        /// <summary>
        /// <para xml:lang="es">
        /// Representa el estaddo, delegacion o region de un país
        /// </para>
        /// </summary>
		public String region;
        /// <summary>
        /// It represents a city
        /// <para xml:lang="es">
        /// representa una ciudad
        /// </para>
        /// </summary>
		public String city;
        /// <summary>
        /// Represents the postal code
        /// <para xml:lang="es">
        /// Representa el codigo postal
        /// </para>
        /// </summary>
		public String postalCode;
        /// <summary>
        /// It represents the angular distance between the equator(the Ecuador), and a given point on Earth
        /// <para xml:lang="es">
        /// representa la distancia angular entre la línea ecuatorial (el ecuador), y un punto determinado de la Tierra
        /// </para>
        /// </summary>
		public double latitude;
        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
		public double longitude;
        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
		public int dma_code;
        
        /// <summary>
        /// Represents the area code
        /// <para xml:lang="es">
        /// Representa el codigo de area
        /// </para>
        /// </summary>
		public int area_code;
        /// <summary>
        /// It represents the name of the region
        /// <para xml:lang="es">
        /// Representa el nombre de la region
        /// </para>
        /// </summary>
		public String regionName;
        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
		public int metro_code;
        /// <summary>
        /// It represents the diameter of the earth
        /// <para xml:lang="es">
        /// Representa el diámetro de la tierra
        /// </para>
        /// </summary>
		private static double EARTH_DIAMETER = 2 * 6378.2;
        /// <summary>
        /// It represents the value of the approximation of constant PI
        /// <para xml:lang="es">
        /// Representa el valor de la aproximación de la constante PI
        /// </para>
        /// </summary>
        private static double PI = 3.14159265;
        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
		private static double RAD_CONVERT = PI / 180;

        /// <summary>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </summary>
        /// <param name="loc">
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </param>
        /// <returns>
        /// 
        /// <para xml:lang="es">
        /// 
        /// </para>
        /// </returns>
		public double distance(Location loc)
		{
			double delta_lat, delta_lon;
			double temp;

			double lat1 = latitude;
			double lon1 = longitude;
			double lat2 = loc.latitude;
			double lon2 = loc.longitude;

			// convert degrees to radians
			lat1 *= RAD_CONVERT;
			lat2 *= RAD_CONVERT;

			// find the deltas
			delta_lat = lat2 - lat1;
			delta_lon = (lon2 - lon1) * RAD_CONVERT;

			// Find the great circle distance
			temp = Math.Pow(Math.Sin(delta_lat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(delta_lon / 2), 2);
			return EARTH_DIAMETER * Math.Atan2(Math.Sqrt(temp), Math.Sqrt(1 - temp));
		}
	}
}