using System;

namespace OKHOSTING.Core
{
	/// <summary>
	/// Extensions methods for System.DateTime
	/// <para xml:lang="es">
	/// Extensiones métodos para System.DateTime
	/// </para>
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Lets you easily figure out if a DateTime holds a date value that is a weekend.
		/// <para xml:lang="es">
		/// Permite definir facilmente si una fecha es un fin de semana
		/// </para>
		/// </summary>
		/// <param name="value">
		/// Date to assess whether it is a weekend
		/// <para xml:lang="es">
		/// Fecha que se evaluará si es un fin de semana
		/// </para>
		/// </param>
		/// <returns>
		/// If the day of the date entered is Saturday or Sunday, it returns true, otherwise returns false
		/// <para xml:lang="es">
		/// Si el dia de la fecha ingresada es Sabado o Domingo, devuelve true, de lo contrario devuelve false
		/// </para>
		/// </returns>
		public static bool IsWeekend(this DateTime value)
		{
			return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
		}

		/// <summary>
		/// Gets the last date of the month of the DateTime.
		/// <para xml:lang="es">
		/// Obtiene la última fecha del mes de la DateTime.
		/// </para>
		/// </summary>
		/// <param name="dateTime">
		/// Date and time of the last date and time of the month get
		/// <para xml:lang="es">
		/// Fecha y hora  de la que se obtendrá la última fecha y hora  del mes
		/// </para>
		/// </param>
		/// <returns>
		/// Returns the date and time of the last day of the reporting month
		/// <para xml:lang="es">
		/// Devuelve la fecha y hora del ultimo dia del mes indicado
		/// </para>
		/// </returns>
		public static DateTime GetLastDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
		}
	}
}
