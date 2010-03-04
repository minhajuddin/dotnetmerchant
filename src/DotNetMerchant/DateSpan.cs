#region License

// DotNetMerchant
// (http://dotnetmerchant.org)
// Copyright (c) 2010 Conatus Creative Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using DotNetMerchant.Extensions;

namespace DotNetMerchant
{
    /// <summary>
    /// A struct similar to <see cref="TimeSpan" /> that stores the elapsed time between two dates,
    /// but does so in a way that respects the number of actual days in the elapsed years and months.
    /// </summary>
    [Serializable]
    public struct DateSpan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateSpan"/> struct.
        /// </summary>
        /// <param name="start">The starting date.</param>
        /// <param name="end">The ending date.</param>
        public DateSpan(DateTime start, DateTime end) : this()
        {
            start = start.ToUniversalTime();
            end = end.ToUniversalTime();

            if (start > end)
            {
                var dtTemp = start;
                start = end;
                end = dtTemp;
            }

            CalculateYears(start, end);
            CalculateMonths(start, end);
            CalculateDays(start, end);
            CalculateHours(start, end);
            CalculateMinutes(start, end);
            CalculateSeconds(start, end);
        }

        /// <summary>
        /// Gets or sets the years.
        /// </summary>
        /// <value>The years spanned.</value>
        public int Years { get; private set; }

        /// <summary>
        /// Gets or sets the months.
        /// </summary>
        /// <value>The months spanned.</value>
        public int Months { get; private set; }

        /// <summary>
        /// Gets or sets the weeks.
        /// </summary>
        /// <value>The weeks spanned.</value>
        public int Weeks { get; private set; }

        /// <summary>
        /// Gets or sets the days.
        /// </summary>
        /// <value>The days spanned.</value>
        public int Days { get; private set; }

        /// <summary>
        /// Gets or sets the hours.
        /// </summary>
        /// <value>The hours spanned.</value>
        public int Hours { get; private set; }

        /// <summary>
        /// Gets or sets the minutes.
        /// </summary>
        /// <value>The minutes spanned.</value>
        public int Minutes { get; private set; }

        /// <summary>
        /// Gets or sets the seconds.
        /// </summary>
        /// <value>The seconds spanned.</value>
        public int Seconds { get; private set; }

        /// <summary>
        /// Gets the difference between two dates, respecting the specified interval.
        /// </summary>
        /// <param name="interval">The date interval to measure.</param>
        /// <param name="start">The starting date.</param>
        /// <param name="end">The ending date.</param>
        /// <returns></returns>
        public static int GetDifference(DateInterval interval, DateTime start, DateTime end)
        {
            var sum = 0;
            var span = new DateSpan(start, end);

            switch (interval)
            {
                case DateInterval.Years:
                    sum += span.Years;
                    break;
                case DateInterval.Months:
                    if (span.Years > 0)
                    {
                        sum += span.Years*12;
                    }
                    sum += span.Months;
                    sum += span.Weeks/4; // Helps resolve lower resolution
                    break;
                case DateInterval.Weeks:
                    sum = GetDifferenceInDays(start, span)/7;
                    break;
                case DateInterval.Days:
                    sum = GetDifferenceInDays(start, span);
                    break;
                case DateInterval.Hours:
                    sum = GetDifferenceInDays(start, span).Hours();
                    sum += span.Hours;
                    break;
                case DateInterval.Minutes:
                    sum = GetDifferenceInDays(start, span).Hours().Minutes();
                    sum += span.Hours.Minutes();
                    sum += span.Minutes;
                    break;
                case DateInterval.Seconds:
                    sum = GetDifferenceInDays(start, span).Hours().Minutes().Seconds();
                    sum += span.Hours.Minutes().Seconds();
                    sum += span.Minutes.Seconds();
                    sum += span.Seconds;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("interval");
            }

            return sum;
        }

        private static int GetDifferenceInDays(DateTime start, DateSpan span)
        {
            var sum = 0;

            if (span.Years > 0)
            {
                for (var i = 0; i < span.Years; i++)
                {
                    var year = start.Year + i;
                    sum += DateTime.IsLeapYear(year) ? 366 : 365;
                }
            }

            if (span.Months > 0)
            {
                for (var i = 1; i <= span.Months; i++)
                {
                    var month = start.Month + i;
                    if (month >= 13)
                    {
                        month -= 12;
                    }

                    sum += DateTime.DaysInMonth(start.Year + span.Years, month);
                }
            }

            sum += span.Weeks*7;
            sum += span.Days;

            return sum;
        }

        private void CalculateSeconds(DateTime start, DateTime end)
        {
            Seconds = end.Second - start.Second;

            if (end.Second < start.Second)
            {
                Seconds = 60 - start.Second + end.Second;
            }
        }

        private void CalculateMinutes(DateTime start, DateTime end)
        {
            Minutes = end.Minute - start.Minute;

            if (end.Minute < start.Minute)
            {
                Minutes = 60 - start.Minute + end.Minute;
            }

            if (Minutes <= 0)
            {
                return;
            }

            if (end.Second < start.Second)
            {
                Minutes--;
            }
        }

        private void CalculateHours(DateTime start, DateTime end)
        {
            Hours = end.Hour - start.Hour;

            if (end.Hour < start.Hour)
            {
                Hours = 24 - start.Hour + end.Hour;
            }

            if (Hours <= 0)
            {
                return;
            }

            if (end.Minute >= start.Minute)
            {
                if (end.Minute != start.Minute ||
                    end.Second >= start.Second)
                {
                    return;
                }

                Hours--;
            }
            else
            {
                Hours--;
            }
        }

        private void CalculateDays(DateTime start, DateTime end)
        {
            Days = end.Day - start.Day;

            if (end.Day < start.Day)
            {
                Days = DateTime.DaysInMonth(start.Year, start.Month) - start.Day + end.Day;
            }

            if (Days <= 0)
            {
                return;
            }

            if (end.Hour < start.Hour)
            {
                Days--;
            }
            else if (end.Hour == start.Hour)
            {
                if (end.Minute >= start.Minute)
                {
                    if (end.Minute == start.Minute && end.Second < start.Second)
                    {
                        Days--;
                    }
                }
                else
                {
                    Days--;
                }
            }

            Weeks = Days/7;

            Days = Days%7;
        }

        private void CalculateMonths(DateTime start, DateTime end)
        {
            Months = end.Month - start.Month;

            if (end.Month < start.Month || (end.Month <= start.Month && Years > 1))
            {
                Months = 12 - start.Month + end.Month;
            }

            if (Months <= 0)
            {
                return;
            }

            if (end.Day < start.Day)
            {
                Months--;
            }
            else if (end.Day == start.Day)
            {
                if (end.Hour < start.Hour)
                {
                    Months--;
                }
                else if (end.Hour == start.Hour)
                {
                    if (end.Minute >= start.Minute)
                    {
                        if (end.Minute != start.Minute || end.Second >= start.Second)
                        {
                            return;
                        }

                        Months--;
                    }
                    else
                    {
                        Months--;
                    }
                }
            }
        }

        private void CalculateYears(DateTime start, DateTime end)
        {
            Years = end.Year - start.Year;

            if (Years <= 0)
            {
                return;
            }

            if (end.Month < start.Month)
            {
                Years--;
            }
            else if (end.Month == start.Month)
            {
                if (end.Day >= start.Day)
                {
                    if (end.Day != start.Day)
                    {
                        return;
                    }

                    if (end.Hour < start.Hour)
                    {
                        Years--;
                    }

                    else if (end.Hour == start.Hour)
                    {
                        if (end.Minute >= start.Minute)
                        {
                            if (end.Minute != start.Minute)
                            {
                                return;
                            }

                            if (end.Second >= start.Second)
                            {
                                return;
                            }

                            Years--;
                        }
                        else
                        {
                            Years--;
                        }
                    }
                }
                else
                {
                    Years--;
                }
            }
        }
    }
}