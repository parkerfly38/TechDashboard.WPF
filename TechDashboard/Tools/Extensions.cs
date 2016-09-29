using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TechDashboard.Tools
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the index of the specified object in the collection.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="obj">The object.</param>
        /// <returns>If found returns index otherwise -1</returns>
        public static int IndexOf(this IEnumerable self, object obj)
        {
            int index = -1;

            var enumerator = self.GetEnumerator();
            enumerator.Reset();
            int i = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Equals(obj))
                {
                    index = i;
                    break;
                }

                i++;
            }

            return index;
        }
    }

    public static class DateTimeExtensions
    {
        public static string ToSage100TimeString(this DateTime dateTimeObj)
        {
            return dateTimeObj.ToString("HHmm");
        }

        public static string ToSage100DateString(this DateTime dateTimeObj)
        {
            return dateTimeObj.ToString("yyyyMMdd");
        }
    }

    public static class TimeSpanExtensions
    {
        public static string ToSage100TimeString(this TimeSpan timeSpanObj)
        {
            return new DateTime(timeSpanObj.Ticks).ToString("HHmm");
        }
    }

    public static class StringExtensions
    {
        public static TimeSpan ToSage100TimeSpan(this string sage100Time)
        {
            sage100Time = sage100Time.PadLeft(4, '0').PadRight(6, '0');

            TimeSpan returnData =
                new TimeSpan(
                    int.Parse(sage100Time.Substring(0, 2)),
                    int.Parse(sage100Time.Substring(2, 2)),
                    int.Parse(sage100Time.Substring(4, 2))
                );

            return returnData;
        }
    }
}
