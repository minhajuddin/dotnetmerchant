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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace DotNetMerchant.Billing
{
    /// <summary>
    /// A period of time used in date calculations.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{Frequency} x {Quantifier}")]
    public struct Period
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Period"/> struct.
        /// </summary>
        /// <param name="frequency">The frequency.</param>
        /// <param name="quantifier">The quantifier.</param>
        public Period(PeriodFrequency frequency, int quantifier) : this()
        {
            Frequency = frequency;
            Quantifier = quantifier;
        }

        /// <summary>
        /// Gets or sets the period frequency.
        /// </summary>
        /// <value>The frequency.</value>
        public PeriodFrequency Frequency { get; private set; }

        /// <summary>
        /// Gets or sets the period quantifier.
        /// </summary>
        /// <value>The quantifier.</value>
        public int Quantifier { get; private set; }

        /// <summary>
        /// Monthly
        /// </summary>
        /// <value>A monthly period.</value>
        public static Period Monthly
        {
            get { return new Period(PeriodFrequency.Months, 1); }
        }

        /// <summary>
        /// BiMonthly
        /// </summary>
        /// <value>A bi-monthly period.</value>
        public static Period BiMonthly
        {
            get { return new Period(PeriodFrequency.Months, 2); }
        }

        /// <summary>
        /// Weekly
        /// </summary>
        /// <value>A weekly period.</value>
        public static Period Weekly
        {
            get { return new Period(PeriodFrequency.Weeks, 1); }
        }

        /// <summary>
        /// BiWeekly
        /// </summary>
        /// <value>A bi-weekly period.</value>
        public static Period BiWeekly
        {
            get { return new Period(PeriodFrequency.Weeks, 2); }
        }

        /// <summary>
        /// Annually
        /// </summary>
        /// <value>An annual period</value>
        public static Period Annually
        {
            get { return new Period(PeriodFrequency.Years, 1); }
        }

        /// <summary>
        /// BiAnnually
        /// </summary>
        /// <value>A bi-annual period.</value>
        public static Period BiAnnually
        {
            get { return new Period(PeriodFrequency.Years, 2); }
        }

        /// <summary>
        /// Gets the dates this period occurs between a start and end date.
        /// If an occurrence falls on a weekend, it is deferred to the start
        /// of the next week.
        /// </summary>
        /// <param name="start">The starting date.</param>
        /// <param name="end">The ending date.</param>
        /// <returns>A list of dates representing period occurrences.</returns>
        public IEnumerable<DateTime> GetOccurrences(DateTime start, DateTime end)
        {
            var calendar = CultureInfo.CurrentCulture.Calendar;

            switch (Frequency)
            {
                case PeriodFrequency.Days:
                    return GetOccurrences(DateInterval.Days, this, calendar, start, end);
                case PeriodFrequency.Weeks:
                    return GetOccurrences(DateInterval.Weeks, this, calendar, start, end);
                case PeriodFrequency.Months:
                    return GetOccurrences(DateInterval.Months, this, calendar, start, end);
                case PeriodFrequency.Years:
                    return GetOccurrences(DateInterval.Years, this, calendar, start, end);
                default:
                    throw new ArgumentException("Frequency");
            }
        }


        /// <summary>
        /// Gets the number of occurrences of a specified interval occuring in the given period.
        /// </summary>
        /// <param name="interval">The interval.</param>
        /// <param name="period">The period.</param>
        /// <param name="start">The start date.</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetOccurrences(Period period,
                                                           DateInterval interval,
                                                           DateTime start)
        {
            var calendar = CultureInfo.CurrentCulture.Calendar;

            switch (period.Frequency)
            {
                case PeriodFrequency.Days:
                    var days = calendar.AddDays(start, period.Quantifier);
                    yield return DeferOccurrenceFallingOnWeekend(calendar, days);
                    break;
                case PeriodFrequency.Weeks:
                    var weeks = calendar.AddWeeks(start, period.Quantifier);
                    yield return DeferOccurrenceFallingOnWeekend(calendar, weeks);
                    break;
                case PeriodFrequency.Months:
                    var months = calendar.AddMonths(start, period.Quantifier);
                    yield return DeferOccurrenceFallingOnWeekend(calendar, months);
                    break;
                case PeriodFrequency.Years:
                    var years = calendar.AddYears(start, period.Quantifier);
                    yield return DeferOccurrenceFallingOnWeekend(calendar, years);
                    break;
                default:
                    throw new ArgumentException("period.Frequency");
            }
        }

        private static IEnumerable<DateTime> GetOccurrences(DateInterval interval,
                                                            Period period,
                                                            Calendar calendar,
                                                            DateTime start,
                                                            DateTime end)
        {
            var difference = DateSpan.GetDifference(interval, start, end)/period.Quantifier;

            if (start.Kind == DateTimeKind.Utc)
            {
                start = start.ToLocalTime();
            }

            for (var i = 0; i < difference; i++)
            {
                switch (period.Frequency)
                {
                    case PeriodFrequency.Days:
                        var days = calendar.AddDays(start, period.Quantifier*i);
                        yield return DeferOccurrenceFallingOnWeekend(calendar, days);
                        break;
                    case PeriodFrequency.Weeks:
                        var weeks = calendar.AddWeeks(start, period.Quantifier*i);
                        yield return DeferOccurrenceFallingOnWeekend(calendar, weeks);
                        break;
                    case PeriodFrequency.Months:
                        var months = calendar.AddMonths(start, period.Quantifier*i);
                        yield return DeferOccurrenceFallingOnWeekend(calendar, months);
                        break;
                    case PeriodFrequency.Years:
                        var years = calendar.AddYears(start, period.Quantifier*i);
                        yield return DeferOccurrenceFallingOnWeekend(calendar, years);
                        break;
                    default:
                        throw new ArgumentException("Frequency");
                }
            }
        }

        private static DateTime DeferOccurrenceFallingOnWeekend(Calendar calendar,
                                                                DateTime occurrence)
        {
            if (occurrence.DayOfWeek == DayOfWeek.Saturday)
            {
                occurrence = calendar.AddDays(occurrence, 2);
            }
            if (occurrence.DayOfWeek == DayOfWeek.Sunday)
            {
                occurrence = calendar.AddDays(occurrence, 1);
            }

            return occurrence.ToUniversalTime();
        }
    }
}