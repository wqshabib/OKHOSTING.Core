using System;

namespace OKHOSTING.Core.Extensions
{
	/// <summary>
	/// Extensions methods for System.DateTime
	/// </summary>
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Lets you easily figure out if a DateTime holds a date value that is a weekend.
		/// </summary>
		public static bool IsWeekend(this DateTime value)
		{
			return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
		}

		/// <summary>
		/// Gets the last date of the month of the DateTime.  
		/// </summary>
		public static DateTime GetLastDayOfMonth(this DateTime dateTime)
		{
			return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
		}
	}
}
