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
using DotNetMerchant.Billing;
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Model;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors.AuthorizeNet;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Processors.AuthorizeNet
{
    partial class AuthorizeNetTests
    {
        [Test]
        public void Can_cancel_standard_recurring_billing()
        {
            var subscription = new Subscription("Test Subscription",
                                                DateTime.UtcNow,
                                                Period.Monthly,
                                                0.02);

            var result = CreateRecurringBilling(subscription);

            Assert.IsNotNull(result);
            Assert.IsTrue(subscription.ReferenceId.HasValue);

            result = CancelRecurringBilling(subscription);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Can_create_paid_trial_recurring_billing()
        {
            var trialPeriod = new Period(PeriodFrequency.Months, 1);
            var subscription = new Subscription("Test Subscription",
                                                DateTime.UtcNow,
                                                Period.Monthly, 0.10,
                                                trialPeriod, 0.05, 2);

            var expiryMonth = DateTime.UtcNow.Month;
            var expiryYear = DateTime.UtcNow.Year;

            var creditCard = new CreditCard("4111 1111 1111 1111", "John Q Customer", expiryMonth, expiryYear);
            var billTo = new Address {FirstName = "John", LastName = "Customer"};

            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            authDotNet.SetBillingAddress(billTo);

            var result = authDotNet.CreateRecurringBilling(subscription, creditCard);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Can_create_standard_recurring_billing()
        {
            var subscription = new Subscription("Test Subscription",
                                                DateTime.UtcNow,
                                                Period.Monthly,
                                                0.01);

            var result = CreateRecurringBilling(subscription);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Can_update_standard_recurring_billing()
        {
            var expiryMonth = DateTime.UtcNow.Month;
            var expiryYear = DateTime.UtcNow.Year;
            var creditCard = new CreditCard("4111 1111 1111 1111", "John Q Customer", expiryMonth, expiryYear);

            var subscription = new Subscription("Test Subscription",
                                                DateTime.UtcNow,
                                                Period.Monthly,
                                                0.03);

            var result = CreateRecurringBilling(subscription, creditCard);

            Assert.IsNotNull(result);
            Assert.IsTrue(subscription.ReferenceId.HasValue);

            result = UpdateRecurringBilling(subscription, creditCard);

            Assert.IsNotNull(result);
        }
    }
}