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
            var billTo = new Address { FirstName = "John", LastName = "Customer" };

            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            authDotNet.SetBillingAddress(billTo);

            var result = authDotNet.CreateRecurringBilling(subscription, creditCard);

            Assert.IsNotNull(result);
        }
    }
}