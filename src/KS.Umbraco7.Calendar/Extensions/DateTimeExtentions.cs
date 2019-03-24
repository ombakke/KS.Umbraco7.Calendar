using System;

namespace KS.Umbraco7.Calendar.Extensions
{
    /// <summary>
    /// Various extension methods for <see cref="DateTime"/>.
    /// </summary>
    public static class DateTimeExtensions
    {
        ///<summary>Gets the first week day following a date.</summary>
        ///<param name="date">The date.</param>
        ///<param name="dayOfWeek">The day of week to return.</param>
        ///<returns>The first dayOfWeek day following date, or date if it is on dayOfWeek.</returns>
        public static DateTime Next(this DateTime date, DayOfWeek dayOfWeek)
        {
            return date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);
        }

        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current)
        {
            DateTime first = current.AddDays(1 - current.Day);
            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime first = current.First();

            if (first.DayOfWeek != dayOfWeek)
            {
                first = first.Next(dayOfWeek);
            }

            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

            DateTime last = current.First().AddDays(daysInMonth - 1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime last = current.Last();

            if (last.DayOfWeek == dayOfWeek)
            {
                return last;
            }
            else if ((int)dayOfWeek == 0) {
                return last.AddDays((int)last.DayOfWeek * -1);
            }
            else if (last.DayOfWeek < dayOfWeek)
            {
                return last.AddDays(Math.Abs((7 + last.DayOfWeek) - dayOfWeek) * -1);
            }
            else
            { 
                return last.AddDays(Math.Abs(dayOfWeek - last.DayOfWeek) * -1); 
            }
        }

        /// <summary>
        /// Gets a DateTime representing the spesified DayOfWeek in the nthWeek in the month of given DateTime
        /// </summary>
        /// <param name="date">The current date</param>
        /// <param name="nthWeek">The week in the month you want</param>
        /// <param name="dayOfWeek">The day of week you want</param>
        /// <returns>DateTime</returns>
        public static DateTime GetNthWeekofMonth(this DateTime current, int nthWeek, DayOfWeek dayOfWeek)
        {
            return current.AddDays((current.Day *-1) + 1).Next(dayOfWeek).AddDays((nthWeek - 1) * 7);
        }
    }
}
