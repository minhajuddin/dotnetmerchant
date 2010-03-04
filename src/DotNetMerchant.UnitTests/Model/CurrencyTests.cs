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

using System.Globalization;
using System.Threading;
using DotNetMerchant.Model;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Model
{
    [TestFixture]
    public class CurrencyTests
    {
        [Test]
        public void Can_create_currency_using_culture_info()
        {
            CurrencyInfo currencyInfo = new CultureInfo("fr-FR");
            Assert.IsNotNull(currencyInfo);
        }

        [Test]
        public void Can_create_currency_using_currency_code()
        {
            CurrencyInfo currencyInfo = Currency.NZD;
            Assert.IsNotNull(currencyInfo);
        }

        [Test]
        public void Can_create_currency_using_current_culture()
        {
            CurrencyInfo currencyInfo = CultureInfo.CurrentCulture;
            Assert.IsNotNull(currencyInfo);
        }

        [Test]
        public void Can_create_currency_using_region_info()
        {
            CurrencyInfo currencyInfo = new RegionInfo("CA");
            Assert.IsNotNull(currencyInfo);
        }

        [Test]
        public void Currency_creation_creates_meaningful_display_cultures()
        {
            // If I'm from France, and I reference Canadian Dollars,
            // then the default culture for CAD should be fr-CA
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            CurrencyInfo currencyInfo = Currency.CAD;
            Assert.AreEqual(currencyInfo.DisplayCulture, new CultureInfo("fr-CA"));

            // If I'm from England, and I reference Canadian Dollars,
            // then the default culture for CAD should be en-CA
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            currencyInfo = Currency.CAD;
            Assert.AreEqual(currencyInfo.DisplayCulture, new CultureInfo("en-CA"));

            // If I'm from Germany, and I reference Canadian Dollars,
            // then the default culture for CAD should be invariant German
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            currencyInfo = Currency.CAD;
            Assert.AreEqual(currencyInfo.DisplayCulture, new CultureInfo("de"));
        }

        [Test]
        public void Currency_creation_creates_meaningful_native_regions()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            CurrencyInfo currencyInfo = Currency.EUR;
            Assert.AreEqual(currencyInfo.NativeRegion, new RegionInfo("FR"));

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-CA");
            currencyInfo = Currency.CAD;
            Assert.AreEqual(currencyInfo.NativeRegion, new RegionInfo("CA"));
        }
    }
}