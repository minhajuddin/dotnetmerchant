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
using DotNetMerchant.Payments.Model;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Model
{
    [TestFixture]
    public class CreditCardTests
    {
        private const string MasterCardNumber = "5555555555554444";
        private const string VisaNumber = "4111111111111111";
        private const string AmexNumber = "370000000000002";
        private const string DiscoverNumber = "6011111111111117";

        private static void AssertCreditCardValidation(string accountNumber, CreditCardType type)
        {
            var cc = new CreditCard(accountNumber,
                                    "John Q Customer",
                                    DateTime.UtcNow.Month,
                                    DateTime.UtcNow.Year);

            Assert.IsTrue(cc.IsValid);
            Assert.IsTrue(cc.Type == type);
        }

        [Test]
        public void Can_validate_test_card_and_identify_amex()
        {
            AssertCreditCardValidation(AmexNumber, CreditCardType.Amex);
        }

        [Test]
        public void Can_validate_test_card_and_identify_discover()
        {
            AssertCreditCardValidation(DiscoverNumber, CreditCardType.Discover);
        }

        [Test]
        public void Can_validate_test_card_and_identify_mastercard()
        {
            AssertCreditCardValidation(MasterCardNumber, CreditCardType.MasterCard);
        }

        [Test]
        public void Can_validate_test_card_and_identify_visa()
        {
            AssertCreditCardValidation(VisaNumber, CreditCardType.Visa);
        }

        [Test]
        public void Can_validate_test_card_expecting_visa()
        {
            var cc = new CreditCard(CreditCardType.Visa,
                                    VisaNumber,
                                    "John Q Customer",
                                    DateTime.UtcNow.Month,
                                    DateTime.UtcNow.Year);

            Assert.IsTrue(cc.IsValid);
        }

        [Test]
        public void Can_validate_test_card_expecting_visa_and_fail()
        {
            var cc = new CreditCard(CreditCardType.Visa,
                                    MasterCardNumber,
                                    "John Q Customer",
                                    DateTime.UtcNow.Month,
                                    DateTime.UtcNow.Year);

            Assert.IsFalse(cc.IsValid);
        }

        [Test]
        public void Can_validate_test_cards()
        {
            var cc = new CreditCard(VisaNumber,
                                    "John Q Customer",
                                    DateTime.UtcNow.Month,
                                    DateTime.UtcNow.Year);

            Assert.IsTrue(cc.IsValid);
        }
    }
}