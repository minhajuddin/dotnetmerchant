using System;
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
        [Test]
        public void Can_create_standard_recurring_billing()
        {
            var result = CreateSubscription();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.TransactionStatus == TransactionStatus.Approved);
            Assert.IsNotNull(result.RecurringBillingId);
        }

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