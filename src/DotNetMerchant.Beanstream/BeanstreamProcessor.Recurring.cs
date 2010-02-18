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
using DotNetMerchant.Extensions;
using DotNetMerchant.Payments;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors.Beanstream;
using DotNetMerchant.Payments.Workflow;

namespace DotNetMerchant.Beanstream
{
    partial class BeanstreamProcessor : ISupportRecurringBilling<BeanstreamResult>
    {
        /// <summary>
        /// Gets the recurring billing production URI.
        /// </summary>
        /// <value>The recurring billing production URI.</value>
        public Uri RecurringBillingProductionUri
        {
            get { return "https://www.beanstream.com/scripts/recurring_billing.asp".Uri(); }
        }

        /// <summary>
        /// Gets the recurring billing development URI.
        /// </summary>
        /// <value>The recurring billing development URI.</value>
        public Uri RecurringBillingDevelopmentUri
        {
            get { return RecurringBillingProductionUri; }
        }

        /// <summary>
        /// Creates the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public BeanstreamResult CreateRecurringBilling(Subscription subscription, CreditCard card)
        {
            ProcessWithMoneyAndCreditCard(subscription.PaymentAmount, card, CreditCardTransactionType.Purchase);

            ValidateSubscription(subscription);
            ValidateBillingAddress();

            SetCreditCard(card);

            _info.IsRecurring = true;
            switch (subscription.Period.Frequency)
            {
                case PeriodFrequency.Days:
                    _info.BillingPeriod = "D";
                    break;
                case PeriodFrequency.Weeks:
                    _info.BillingPeriod = "W";
                    break;
                case PeriodFrequency.Months:
                    _info.BillingPeriod = "M";
                    break;
                case PeriodFrequency.Years:
                    _info.BillingPeriod = "Y";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("subscription");
            }

            _info.BillingIncrement = subscription.Period.Quantifier;
            _info.FirstBilling = subscription.PaymentStartDate.ToLocalTime();

            if (subscription.ProratedStartDate.HasValue)
            {
                _info.SecondBilling = subscription.ProratedStartDate.Value.ToLocalTime();
            }

            if (subscription.PeriodEndDate.HasValue)
            {
                _info.Expiry = subscription.PeriodEndDate.Value.ToLocalTime();
            }

            var uri = GetRecurringBillingUri(this);

            // Creation uses non-recurring billing URI
            var result = Request(_info, uri);
            if (result.RecurringBillingId.HasValue)
            {
                subscription.ReferenceId = Convert.ToInt64(result.RecurringBillingId);
            }

            return result;
        }

        /// <summary>
        /// Updates the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public BeanstreamResult UpdateRecurringBilling(Subscription subscription, CreditCard card)
        {
            _info.ServiceVersion = "1.0";
            _info.OperationType = "M";

            if (!subscription.ReferenceId.HasValue)
            {
                throw new ArgumentException("You must provide a subscription with a reference ID in order to update it.");
            }

            return SendRecurringBillingRequest();
        }

        /// <summary>
        /// Cancels the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns></returns>
        public BeanstreamResult CancelRecurringBilling(Subscription subscription)
        {
            _info.ServiceVersion = "1.0";
            _info.OperationType = "C";


            return SendRecurringBillingRequest();
        }

        private BeanstreamResult SendRecurringBillingRequest()
        {
            var uri = Mode == OperationMode.Production
                          ? RecurringBillingProductionUri
                          : RecurringBillingDevelopmentUri;

            return Request(_info, uri);
        }
    }
}