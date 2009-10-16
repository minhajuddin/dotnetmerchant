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
using DotNetMerchant.Billing;
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Model;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Billing.Model
{
    [TestFixture]
    public class SubscriptionTests
    {
        [Test]
        public void Can_create_monthly_subscription_with_end_date()
        {
            var start = DateTime.UtcNow;
            var end = start.AddYears(1);

            var subscription = new Subscription
                                   (
                                       "Monthly Subscription",
                                       start,
                                       end,
                                       Period.Monthly,
                                       15
                                   );

            var payments = subscription.TotalPayments;
            var amount = subscription.TotalAmount;

            Assert.AreEqual(12, payments);
            Assert.AreEqual(12*15, amount);
        }

        [Test]
        public void Can_create_monthly_subscription_with_indefinite_end_date()
        {
            var start = DateTime.UtcNow;

            var subscription = new Subscription
                                   (
                                       "Monthly Subscription",
                                       start,
                                       Period.Monthly,
                                       15
                                   );

            var payments = subscription.TotalPayments;
            var amount = subscription.TotalAmount;

            Assert.AreEqual(null, payments);
            Assert.AreEqual(double.PositiveInfinity, (double) amount);
        }

        [Test]
        public void Can_create_monthly_subscription_with_one_month_free_trial()
        {
            var start = DateTime.UtcNow;
            var end = start.AddYears(5);

            var trialPeriod = new Period(PeriodFrequency.Months, 1);

            var subscription = new Subscription("Monthly Subscription with One Month Free",
                                                start, end, Period.Monthly, 15, trialPeriod);

            var payments = subscription.TotalPayments;
            var trialPayments = subscription.TrialPayments;
            var amount = subscription.TotalAmount;
            
            Assert.AreEqual(start.AddMonths(1), subscription.PaymentStartDate);
            Assert.IsNull(trialPayments, "This is a free trial");
            Assert.AreEqual(12 * 5 - 1, payments);
            Assert.AreEqual(5 * 12 * 15 - 15, amount);
        }

        [Test]
        public void Can_create_monthly_subscription_with_non_standard_trial_period()
        {
            var start = DateTime.UtcNow;
            var end = start.AddYears(5);

            var trialPeriod = new Period(PeriodFrequency.Days, 32);

            var subscription = new Subscription("Monthly Subscription with 31 Days Free",
                                                start, end, Period.Monthly, 15, trialPeriod);

            Assert.AreEqual(start.AddDays(32), subscription.PaymentStartDate);

            var payments = subscription.TotalPayments;
            var amount = subscription.TotalAmount;
            
            Assert.AreEqual(12 * 5 - 1, payments);
            Assert.AreEqual(5 * 12 * 15 - 15, amount);

            var next = subscription.NextPaymentDate;
            var prev = subscription.PreviousPaymentDate;

            Assert.AreEqual(subscription.PaymentStartDate, next);
            Assert.IsNull(prev);
            Assert.IsNull(subscription.TrialPayments);
            Assert.IsNull(subscription.TrialAmount);
        }

        [Test]
        public void Can_create_monthly_subscription_with_paid_trial_period()
        {
            var start = DateTime.UtcNow;
            var end = start.AddYears(5);

            var trialPeriod = new Period(PeriodFrequency.Months, 1);

            var subscription = new Subscription("Monthly Subscription with Introductory Price",
                                                start, end,
                                                Period.Monthly, 30,
                                                trialPeriod, 15, 1);

            Assert.IsNotNull(subscription.TrialPayments);
            Assert.AreEqual(60, subscription.TotalPayments); // All occurrences are paid
            Assert.AreEqual(1, subscription.TrialPayments);
        }

        [Test]
        public void Can_create_disparate_trial_and_subscription_periods()
        {
            var start = DateTime.UtcNow;
            var end = start.AddYears(5);

            // Every five days at the introductory price
            var trialPeriod = new Period(PeriodFrequency.Days, 5);

            var subscription = new Subscription("Monthly Subscription with Introductory Price",
                                                start, end,
                                                Period.Monthly, 30,
                                                trialPeriod, 1.00, 1);

            Assert.AreEqual(1771, subscription.TotalAmount);
            Assert.AreEqual(1770, subscription.TotalBillingAmount);
            Assert.AreEqual(1, subscription.TotalTrialAmount.Value);

            Assert.AreEqual(1, subscription.TrialAmount.Value);
            Assert.AreEqual(60, subscription.TotalPayments); // All occurrences are paid
            Assert.AreEqual(1, subscription.TrialPayments);
        }

        [Test]
        public void Can_create_subscription_with_prorated_start_date()
        {
            var start = DateTime.UtcNow;
            var end = start.AddYears(5);

            var prorated = 10d / DateTime.DaysInMonth(start.Year, start.Month);
            Money expected = 30*prorated;
            
            var subscription = new Subscription("Monthly Subscription with Prorated Start Date",
                                                start, end, start.Subtract(TimeSpan.FromDays(10)),
                                                Period.Monthly, 30);

            Assert.AreEqual(expected, subscription.ProratedAmount.Value);
        }
    }
}