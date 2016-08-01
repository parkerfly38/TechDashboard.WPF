using System;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Tools.Sage100
{

    public class Utilities
    {
        /// <summary>
        /// Takes a string date and a string time and converts to a DateTime object
        /// </summary>
        /// <param name="date">Date string in yyyy-MM-dd format.</param>
        /// <param name="time">Time string in 24-hour hh:mm format.</param>
        /// <returns>A DateTime object representing the given date/time parameters.</returns>
        public static DateTime ConvertToDateTime(string date, string time)
        {
            string[] brokenDate = date.Split(new char[] { '-', '/' }, StringSplitOptions.RemoveEmptyEntries);
            string[] brokenTime = time.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                DateTime returnValue =
                    new DateTime(
                        int.Parse(brokenDate[0]), int.Parse(brokenDate[1]), int.Parse(brokenDate[2]),
                        int.Parse(brokenTime[0]), int.Parse(brokenTime[1]), 0
                    );

                return returnValue;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
