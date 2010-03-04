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
using DotNetMerchant.Beanstream;
using DotNetMerchant.Billing;
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Model;
using DotNetMerchant.Payments;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors.Beanstream;
using NUnit.Framework;

namespace DotNetMerchant.Gateways.UnitTests.Beanstream
{
    partial class BeanstreamTests
    {
        private BeanstreamResult CreateSubscription()
        {
            var subscription = new Subscription("Test Subscription",
                                                DateTime.UtcNow,
                                                Period.Monthly,
                                                0.01);

            var card = new CreditCard("4111 1111 1111 1111",
                                      "John Q Customer",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);

            var beanstream = new BeanstreamProcessor(CredentialFirst, CredentialSecond, CredentialThird);

            //  Billing address is required for card transactions
            beanstream.SetBillingAddress(new Address
                                             {
                                                 FirstName = "Paul Randal",
                                                 Email = "prandal@mydomain.net",
                                                 Phone = "9999999",
                                                 AddressLine = "1045 Main Street",
                                                 City = "Vancouver",
                                                 State = "BC",
                                                 Zip = "V8R 1J6",
                                                 Country = "CA",
                                             });

            return beanstream.CreateRecurringBilling(subscription, card);
        }

        [Test]
        public void Can_create_standard_recurring_billing()
        {
            var result = CreateSubscription();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.TransactionStatus == TransactionStatus.Approved);
            Assert.IsNotNull(result.RecurringBillingId);
        }

        [Test]
        public void Can_update_standard_recurring_billing()
        {
            // ReferenceId is set here
            var create = CreateSubscription();

            Assert.IsNotNull(create.RecurringBillingId);

            var subscription = new Subscription("Test Subscription",
                                                DateTime.UtcNow,
                                                Period.Monthly,
                                                0.01);

            var card = new CreditCard("4111 1111 1111 1111",
                                      "John Q Customer",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);

            var beanstream = new BeanstreamProcessor(CredentialFirst,
                                                     CredentialSecond,
                                                     CredentialThird);

            var result = beanstream.UpdateRecurringBilling(subscription, card);

            Assert.IsNotNull(result);
        }
    }
}