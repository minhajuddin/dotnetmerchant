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
using NUnit.Framework;

namespace DotNetMerchant.UnitTests
{
    [TestFixture]
    public class DateSpanTests
    {
        [Test]
        public void Can_get_date_difference_in_days()
        {
            var start = DateTime.Now;
            var end = DateTime.Now.AddDays(5);

            var diff = DateSpan.GetDifference(DateInterval.Days, start, end);

            Assert.AreEqual(5, diff);
        }

        [Test]
        public void Can_get_date_difference_in_days_spanning_one_month()
        {
            var start = new DateTime(2009, 09, 30);
            var end = new DateTime(2009, 10, 01);

            var days = DateSpan.GetDifference(DateInterval.Days, start, end);
            Assert.AreEqual(1, days);
        }

        [Test]
        public void Can_get_date_difference_in_days_spanning_one_week()
        {
            var start = new DateTime(2009, 09, 30);
            var end = start.AddDays(10);

            var days = DateSpan.GetDifference(DateInterval.Days, start, end);
            var weeks = DateSpan.GetDifference(DateInterval.Weeks, start, end);

            Assert.AreEqual(10, days);
            Assert.AreEqual(1, weeks);
        }

        [Test]
        public void Can_get_date_difference_in_days_spanning_two_months()
        {
            var start = new DateTime(2009, 09, 30);
            var end = new DateTime(2009, 11, 04); // 4 days in November, 31 in October

            var days = DateSpan.GetDifference(DateInterval.Days, start, end);
            Assert.AreEqual(35, days);
        }

        [Test]
        public void Can_handle_composite_spans()
        {
            var start = new DateTime(2009, 9, 30);
            var end = new DateTime(2009, 10, 31);

            var span = new DateSpan(start, end);

            Assert.AreEqual(1, span.Months);
            Assert.AreEqual(1, span.Days);
        }
    }
}