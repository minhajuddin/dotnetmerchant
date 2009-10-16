#region License

// The MIT License
// 
// Copyright (c) 2009 Conatus Creative, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using DotNetMerchant.Model;
using DotNetMerchant.Payments;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors.Beanstream;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Processors.Beanstream
{
    [TestFixture]
    public partial class BeanstreamTests : ProcessorTestBase
    {
        public BeanstreamTests() : base("beanstream",
                                        "merchantId",
                                        "username",
                                        "password")
        {

        }

        [Test]
        public void Can_authorize_payment()
        {
            var expiryMonth = DateTime.UtcNow.Month;
            var expiryYear = DateTime.UtcNow.Year;

            var creditCard = new CreditCard("4030000010001234",
                                            "Paul Randal",
                                            "123",
                                            expiryMonth,
                                            expiryYear);
            
            var beanstream = new BeanstreamProcessor(CredentialFirst, 
                                                     CredentialSecond, 
                                                     CredentialThird);

            //  Billing address is required for card transactions
            beanstream.SetBillingAddress(new Address
                                             {
                                                 FirstName = "Paul Randal",
                                                 Email = "prandal@mydomain.net", // required
                                                 Phone = "9999999",
                                                 AddressLine = "1045 Main Street",
                                                 City = "Vancouver",
                                                 State = "BC",
                                                 Zip = "V8R 1J6",
                                                 Country = "CA",
                                             });

            var result = beanstream.Authorize(10.00, creditCard);

            Console.WriteLine(result.RequestUri.OriginalString);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TransactionStatus == TransactionStatus.Approved);
        }

        [Test]
        public void Can_purchase()
        {
            var expiryMonth = DateTime.UtcNow.Month;
            var expiryYear = DateTime.UtcNow.Year;

            var creditCard = new CreditCard("4030000010001234",
                                            "Paul Randal",
                                            expiryMonth, expiryYear);

            var beanstream = new BeanstreamProcessor(CredentialFirst,
                                                     CredentialSecond,
                                                     CredentialThird);

            // Billing address is required for card transactions
            beanstream.SetBillingAddress(new Address
                                             {
                                                 FirstName = "Paul Randal",
                                                 Email = "prandal@mydomain.net", // required
                                                 Phone = "9999999",
                                                 AddressLine = "1045 Main Street",
                                                 City = "Vancouver",
                                                 State = "BC",
                                                 Zip = "V8R 1J6",
                                                 Country = "CA",
                                             });

            var result = beanstream.Purchase(10.00, creditCard);

            Console.WriteLine(result.RequestUri.OriginalString);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TransactionStatus == TransactionStatus.Approved);
        }

        [Test]
        public void Can_capture()
        {
            var expiryMonth = DateTime.UtcNow.Month;
            var expiryYear = DateTime.UtcNow.Year;

            var creditCard = new CreditCard("4030000010001234",
                                            "Paul Randal",
                                            "123",
                                            expiryMonth,
                                            expiryYear);

            var beanstream = new BeanstreamProcessor(CredentialFirst,
                                                     CredentialSecond,
                                                     CredentialThird);

            //  Billing address is required for card transactions
            beanstream.SetBillingAddress(new Address
            {
                FirstName = "Paul Randal",
                Email = "prandal@mydomain.net", // required
                Phone = "9999999",
                AddressLine = "1045 Main Street",
                City = "Vancouver",
                State = "BC",
                Zip = "V8R 1J6",
                Country = "CA",
            });

            var auth = beanstream.Authorize(10.00, creditCard);
            var result = beanstream.Capture(10.00, creditCard, auth.TransactionId);

            Console.WriteLine(result.RequestUri.OriginalString);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TransactionStatus == TransactionStatus.Approved);
        }

        [Test]
        public void Can_void()
        {
            var expiryMonth = DateTime.UtcNow.Month;
            var expiryYear = DateTime.UtcNow.Year;

            var creditCard = new CreditCard("4030000010001234",
                                            "Paul Randal",
                                            "123",
                                            expiryMonth,
                                            expiryYear);

            var beanstream = new BeanstreamProcessor(CredentialFirst,
                                                     CredentialSecond,
                                                     CredentialThird);

            // This is required for every transaction
            beanstream.SetOrderNumber("2232");

            // Billing address is required for card transactions
            beanstream.SetBillingAddress(new Address
            {
                FirstName = "Paul Randal",
                Email = "prandal@mydomain.net", // required
                Phone = "9999999",
                AddressLine = "1045 Main Street",
                City = "Vancouver",
                State = "BC",
                Zip = "V8R 1J6",
                Country = "CA",
            });

            var auth = beanstream.Authorize(10.00, creditCard);
            var capture = beanstream.Capture(10.00, creditCard, auth.TransactionId);
            
            var result = beanstream.Void(capture.TransactionId);
            Console.WriteLine(result.RequestUri.OriginalString);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.TransactionStatus == TransactionStatus.Approved);
        }
    }
}