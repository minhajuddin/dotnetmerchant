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
using System.Xml.Linq;
using DotNetMerchant.AuthorizeNet;
using DotNetMerchant.Billing;
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Extensions;
using DotNetMerchant.Model;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Web.Xml;

namespace DotNetMerchant.Payments.Processors.AuthorizeNet
{
    partial class AuthorizeNetProcessor
    {
        #region ISupportRecurringBilling<AuthorizeNetResult> Members

        /// <summary>
        /// Creates the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public AuthorizeNetResult CreateRecurringBilling(Subscription subscription, CreditCard card)
        {
            ValidateSubscription(subscription);
            ValidateAmountCard(subscription.PaymentAmount, card);
            ValidateBillingAddress();

            var document = BuildRecurringBillingRequest(RecurringBillingTransactionType.Create,
                                                        subscription,
                                                        card);

            var result = SendRecurringBillingRequest(document);
            if (!result.TransactionId.IsNullOrBlank())
            {
                subscription.ReferenceId = Convert.ToInt64(result.TransactionId);
            }

            return result;
        }

        /// <summary>
        /// Updates the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public AuthorizeNetResult UpdateRecurringBilling(Subscription subscription, CreditCard card)
        {
            ValidateSubscription(subscription);
            ValidateAmountCard(subscription.PaymentAmount, card);
            ValidateBillingAddress();

            var document = BuildRecurringBillingRequest(RecurringBillingTransactionType.Update,
                                                        subscription,
                                                        card);

            return SendRecurringBillingRequest(document);
        }

        /// <summary>
        /// Cancels the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns></returns>
        public AuthorizeNetResult CancelRecurringBilling(Subscription subscription)
        {
            var document = BuildRecurringBillingRequest(RecurringBillingTransactionType.Cancel,
                                                        subscription,
                                                        null);

            return SendRecurringBillingRequest(document);
        }

        #endregion

        private AuthorizeNetResult SendRecurringBillingRequest(XDocument document)
        {
            var uri = Mode == OperationMode.Production
                          ? RecurringBillingProductionUri
                          : RecurringBillingDevelopmentUri;

            var response = Request(document, uri);
            return response;
        }

        private XDocument BuildRecurringBillingRequest(RecurringBillingTransactionType type,
                                                       Subscription subscription,
                                                       CreditCard card)
        {
            XNamespace xmlns = "AnetApi/xml/v1/schema/AnetApiSchema.xsd";
            var xml = new XmlWrapper(xmlns);

            var hasSubscription = subscription != null;

            var hasId = hasSubscription && subscription.Id != Identity.None;
            var hasRefId = hasSubscription && subscription.ReferenceId.HasValue;

            var refId = hasSubscription ? subscription.Id : Identity.None;
            var subscriptionId = hasSubscription
                                     ? subscription.ReferenceId.ValueOr(Identity.None)
                                     : Identity.None;

            if (type == RecurringBillingTransactionType.Cancel)
            {
                subscription = null;
            }

            return new XDocument(
                XmlWrapper.Declare("1.0", "utf-8")
                , xml.Tag("ARB{0}SubscriptionRequest".FormatWith(type)
                          , xml.Tag("merchantAuthentication"
                                    , xml.Tag("name", Authenticator.First)
                                    , xml.Tag("transactionKey", Authenticator.Second))
                          , xml.If(hasId, xml.Tag("refId", refId))
                          , xml.If(hasRefId, xml.Tag("subscriptionId", subscriptionId))
                          , xml.If(hasSubscription, BuildSubscriptionNode(subscription, xml, card))
                      )
                );
        }

        private XElement BuildSubscriptionNode(Subscription subscription,
                                               XmlWrapper xml,
                                               CreditCard card)
        {
            if (subscription == null)
            {
                return null;
            }

            var unit = subscription.Period.Frequency == PeriodFrequency.Months ? "months" : "days";

            int length;
            switch (subscription.Period.Frequency)
            {
                case PeriodFrequency.Months:
                case PeriodFrequency.Days:
                    length = subscription.Period.Quantifier;
                    break;
                case PeriodFrequency.Weeks:
                    length = subscription.Period.Quantifier*7;
                    break;
                case PeriodFrequency.Years:
                    length = subscription.Period.Quantifier*365;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (unit.Equals("months") && length > 12 ||
                unit.Equals("days") && length > 365)
            {
                throw new ArgumentException(
                    "Authorize.net only supports recurring billing periods up to one year in length.");
            }

            return xml.Tag("subscription"
                           , xml.If(!subscription.Name.IsNullOrBlank(), xml.Tag("name", subscription.Name))
                           , xml.Tag("paymentSchedule"
                                     , xml.Tag("interval"
                                               , xml.Tag("length", length)
                                               , xml.Tag("unit", unit)
                                           )
                                     , xml.Tag("startDate", subscription.PaymentStartDate.ToString("yyyy-MM-dd"))
                                     , xml.Tag("totalOccurrences", subscription.TotalPayments.ValueOr(9999))
                                     ,
                                     xml.If(subscription.TrialPayments.HasValue,
                                            xml.Tag("trialOccurrences", subscription.TrialPayments.ValueOr(0)))
                                 )
                           , xml.Tag("amount", (double) subscription.PaymentAmount)
                           ,
                           xml.If(subscription.TrialPayments.HasValue,
                                  xml.Tag("trialAmount", (double) subscription.TrialAmount.ValueOr(0)))
                           , xml.Tag("payment"
                                     , xml.Tag("creditCard"
                                               , xml.Tag("cardNumber", card.AccountNumber.Insecure())
                                               , xml.Tag("expirationDate", card.ExpiryDate.ToString("yyyy-MM"))
                                               ,
                                               xml.If(!card.VerificationCode.IsNullOrBlank(),
                                                      xml.Tag("cardCode", card.VerificationCode.Insecure())))
                                 )
                           , xml.Tag("billTo"
                                     , xml.Tag("firstName", _info.BillToFirstName)
                                     , xml.Tag("lastName", _info.BillToLastName)
                                 )
                );
        }
    }
}