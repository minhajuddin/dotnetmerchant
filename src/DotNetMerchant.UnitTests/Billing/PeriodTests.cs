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
using System.Linq;
using DotNetMerchant.Billing;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Billing
{
    [TestFixture]
    public class PeriodTests
    {
        [Test]
        public void Can_get_payment_occurrences()
        {
            var start = new DateTime(2009, 09, 01);
            var end = new DateTime(2010, 09, 01);

            var period = Period.Weekly;
            var occurrences = period.GetOccurrences(start, end);

            foreach (var occurrence in occurrences)
            {
                Console.WriteLine("{0}({1})", occurrence, occurrence.DayOfWeek);
            }

            Assert.AreEqual(52, occurrences.Count());
        }

        [Test]
        public void Can_get_payment_occurrences_when_start_date_falls_on_a_weekend()
        {
            var start = new DateTime(2009, 09, 05);
            var end = new DateTime(2010, 09, 05);

            var period = Period.Weekly;
            var occurrences = period.GetOccurrences(start, end);

            foreach (var occurrence in occurrences)
            {
                Console.WriteLine(occurrence + "(" + occurrence.DayOfWeek + ")");
            }

            Assert.AreEqual(52, occurrences.Count());
        }
    }
}