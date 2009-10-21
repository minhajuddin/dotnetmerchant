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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Extensions;
using DotNetMerchant.Model;
using DotNetMerchant.Payments.Authentication;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Model.Extensions;
using DotNetMerchant.Payments.Workflow;
using DotNetMerchant.Web;
using DotNetMerchant.Web.Query;
using DotNetMerchant.Web.Query.Basic;

namespace DotNetMerchant.Payments.Processors
{
    /// <summary>
    /// The base class for payment processors.
    /// </summary>
    /// <typeparam name="T">The typed processor info to use.</typeparam>
    /// <typeparam name="K">The typed processor result to use.</typeparam>
    public abstract class PaymentProcessorBase<T, K> : IPaymentProcessor<T>
        where T : ICreditCardPaymentInfo, new()
        where K : IPaymentProcessorResult, new()
    {
        #region todo: remove protected members and consider placement of state
        /// <summary>
        /// Internal state for storing a credit card.
        /// </summary>
        protected CreditCard _creditCard;

        /// <summary>
        /// Internal state for storing a payment method
        /// </summary>
        protected PaymentMethod _paymentMethod;

        /// <summary>
        /// Internal state for store a credit card transaction type
        /// </summary>
        protected CreditCardTransactionType _creditCardTransactionType;

        /// <summary>
        /// Internal state for store processor info.
        /// </summary>
        protected T _info; 
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentProcessorBase&lt;T, K&gt;"/> class.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        protected PaymentProcessorBase(IAuthenticator authenticator)
        {
            _info = new T();

            Authenticator = authenticator;
        }

        /// <summary>
        /// Gets or sets the operation mode.
        /// </summary>
        /// <value>The operation mode.</value>
        public OperationMode Mode { get; set; }

        #region IPaymentProcessor<T> Members

        /// <summary>
        /// The identifying name for this payment processor. Used to match configuration values.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The friendly name for this payment processor.
        /// </summary>
        public abstract string DisplayName { get; }
        
        /// <summary>
        /// The homepage for this payment processor or API.
        /// </summary>
        public abstract Uri HomepageUri { get; }

        /// <summary>
        /// The credit cards this payment processor can support. 
        /// Express multiple types using enum flag syntax.
        /// </summary>
        public abstract IEnumerable<CreditCardType> SupportedCreditCardTypes { get; }

        /// <summary>
        /// The regions this payment processor will support.
        /// </summary>
        public abstract IEnumerable<RegionInfo> SupportedRegions { get; }
        
        /// <summary>
        /// The authentication scheme used to identify the merchant account
        /// requesting payment processing from this service.
        /// </summary>
        public IAuthenticator Authenticator { get; set; }

        #endregion

        /// <summary>
        /// Maps the type of the transaction.
        /// </summary>
        protected abstract void MapTransactionType();

        /// <summary>
        /// Maps the payment method.
        /// </summary>
        protected abstract void MapPaymentMethod();

        /// <summary>
        /// Maps the authenticator.
        /// </summary>
        protected abstract void MapAuthenticator();

        /// <summary>
        /// Makes a request to the payment processor
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        protected K Request(T info, Uri uri)
        {
            MapAuthenticator();
            MapPaymentMethod();
            MapTransactionType();

            if (_creditCard != null)
            {
                SetCreditCard(_creditCard);
            }

            return RequestImpl(info, uri);
        }

        /// <summary>
        /// Makes a request to the payment processor with an XML payload.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        protected K Request(XDocument xml, Uri uri)
        {
            var info = new XmlQueryInfo {Xml = xml.ToString()};
            return RequestImpl(info, uri);
        }

        private static K RequestImpl(IWebQueryInfo info, Uri uri)
        {
            var query = new BasicAuthWebQuery(info) {Method = WebMethod.Post};
            var response = query.Request(uri.ToString());

            var result = new K {RequestUri = query.RequestUri};
            result.PopulateFromResponse(response);
            return result;
        }

        /// <summary>
        /// Validates the amount of money passed into a processing method.
        /// </summary>
        /// <param name="amount">The amount.</param>
        protected static void ValidateAmount(Money amount)
        {
            if (amount <= 0.0)
            {
                throw new ArgumentException("You must provide a non-zero amount to authorize.");
            }
        }

        /// <summary>
        /// Validates the credit card passed into a processing method.
        /// </summary>
        /// <param name="card">The card.</param>
        protected void ValidateCreditCard(CreditCard card)
        {
            if (card == null || !card.IsValid || card.IsExpired)
            {
                throw new ArgumentException("You can only authorize a transaction with a known valid card.");
            }

            if (!SupportedCreditCardTypes.Contains(card.Type))
            {
                throw new ArgumentException("This payment processor does not support the provided credit card.");
            }
        }

        /// <summary>
        /// Validates the amount of money passed into a processing method.
        /// Validates the credit card passed into a processing method.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        protected void ValidateAmountCard(Money amount, CreditCard card)
        {
            ValidateAmount(amount);
            ValidateCreditCard(card);
        }

        /// <summary>
        /// Validates the amount of money passed into a processing method.
        /// Validates the credit card passed into a processing method.
        /// Validates the transaction ID passed into the processing method.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="transactionId">The transaction ID.</param>
        protected void ValidateAmountCardTransaction(Money amount, CreditCard card, string transactionId)
        {
            ValidateAmount(amount);
            ValidateCreditCard(card);
            ValidateTransaction(transactionId);
        }

        /// <summary>
        /// Validates the amount of money passed into a processing method.
        /// Validates the transaction ID passed into the processing method.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="transactionId">The transaction ID.</param>
        protected static void ValidateAmountTransaction(Money amount, string transactionId)
        {
            ValidateAmount(amount);
            ValidateTransaction(transactionId);
        }

        /// <summary>
        /// Validates the transaction ID passed into the processing method.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        protected static void ValidateTransaction(string transactionId)
        {
            if (transactionId.IsNullOrBlank())
            {
                throw new ArgumentException("You must provide a valid transaction Id.");
            }
        }

        /// <summary>
        /// Sets the shipping address.
        /// </summary>
        /// <param name="shippingAddress">The shipping address.</param>
        public virtual void SetShippingAddress(Address shippingAddress)
        {
            if (shippingAddress == null)
            {
                return;
            }

            _info.ShipToFirstName = shippingAddress.FirstName;
            _info.ShipToLastName = shippingAddress.LastName;
            _info.ShipToCompany = shippingAddress.Company;
            _info.ShipToPhone = shippingAddress.Phone;
            _info.ShipToAddress = shippingAddress.AddressLine;
            _info.ShipToCity = shippingAddress.City;
            _info.ShipToState = shippingAddress.State;
            _info.ShipToZip = shippingAddress.Zip;
            _info.ShipToCountry = shippingAddress.Country;
        }

        /// <summary>
        /// Sets the billing address.
        /// </summary>
        /// <param name="billingAddress">The billing address.</param>
        public virtual void SetBillingAddress(Address billingAddress)
        {
            if (billingAddress == null)
            {
                return;
            }

            _info.BillToFirstName = billingAddress.FirstName;
            _info.BillToLastName = billingAddress.LastName;
            _info.BillToEmail = billingAddress.Email;
            _info.BillToCompany = billingAddress.Company;
            _info.BillToPhone = billingAddress.Phone;
            _info.BillToAddress = billingAddress.AddressLine;
            _info.BillToCity = billingAddress.City;
            _info.BillToState = billingAddress.State;
            _info.BillToZip = billingAddress.Zip;
            _info.BillToCountry = billingAddress.Country;
        }

        /// <summary>
        /// Sets the credit card.
        /// </summary>
        /// <param name="card">The card.</param>
        protected virtual void SetCreditCard(CreditCard card)
        {
            ValidateCreditCard(card);

            _info.CreditCardName = _creditCard.CardholderName;
            _info.CreditCardNumber = _creditCard.AccountNumber.Insecure();
            _info.CreditCardVerificationCode = _creditCard.VerificationCode.Insecure();
            _info.CreditCardExpiry = _creditCard.ExpiryDate.ToPaddedString();
        }

        /// <summary>
        /// Sends the request with money and credit card.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        protected K RequestWithMoneyAndCard(CreditCardTransactionType type, 
                                            Money amount, 
                                            CreditCard card,
                                            Uri uri)
        {
            ProcessWithMoneyAndCreditCard(amount, card, type);

            return Request(_info, uri);
        }

        /// <summary>
        /// Sends the request with a transaction.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        protected K RequestWithTransaction(CreditCardTransactionType type, 
                                           string transactionId, 
                                           Uri uri)
        {
            ProcessWithTransaction(type, transactionId);

            return Request(_info, uri);
        }

        /// <summary>
        /// Sends the request with money, credit card and a transaction.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="uri">The URI.</param>
        /// <returns></returns>
        protected K RequestWithMoneyCreditCardAndTransaction(CreditCardTransactionType type, 
                                                       Money amount, 
                                                       CreditCard card,
                                                       string transactionId,
                                                       Uri uri)
        {
            ProcessWithMoneyAndCreditCard(amount, card, type);
            ProcessWithMoneyAndTransaction(amount, transactionId, type);

            return Request(_info, uri);
        }

        /// <summary>
        /// Processes the request with money and a transaction.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="type">The type.</param>
        protected void ProcessWithMoneyAndTransaction(Money amount, string transactionId, CreditCardTransactionType type)
        {
            ValidateAmountTransaction(amount, transactionId);

            _creditCardTransactionType = type;
            _info.TransactionAmount = amount;
            _info.TransactionId = transactionId;
        }

        /// <summary>
        /// Processes the request with a transaction.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="transactionId">The transaction id.</param>
        protected void ProcessWithTransaction(CreditCardTransactionType type, string transactionId)
        {
            ValidateTransaction(transactionId);

            _creditCardTransactionType = type;
            _info.TransactionId = transactionId;
        }

        /// <summary>
        /// Processes the transaction with money and a credit card.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="type">The type.</param>
        protected void ProcessWithMoneyAndCreditCard(Money amount, CreditCard card, CreditCardTransactionType type)
        {
            ValidateAmountCard(amount, card);

            _creditCardTransactionType = type;
            _paymentMethod = PaymentMethod.CreditCard;
            _creditCard = card;
            _info.TransactionAmount = amount;
        }

        /// <summary>
        /// Validates the subcription. In <see cref="PaymentProcessorBase{T,K}"/>
        /// this is a no-op call. You must override to validate in a descendent processor.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        protected virtual void ValidateSubscription(Subscription subscription)
        {
            return;
        }

        /// <summary>
        /// Validates the billing address. In <see cref="PaymentProcessorBase{T,K}"/>
        /// this is a no-op call. You must override to validate in a descendent processor.
        /// </summary>
        protected virtual void ValidateBillingAddress()
        {
            return;
        }

        /// <summary>
        /// Gets the credit card URI.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <returns></returns>
        protected Uri GetCreditCardUri(ISupportCreditCards<K> processor)
        {
            return Mode == OperationMode.Production
                       ? processor.CreditCardProductionUri
                       : processor.CreditCardDevelopmentUri;
        }

        /// <summary>
        /// Gets the recurring billing URI.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <returns></returns>
        protected Uri GetRecurringBillingUri(ISupportRecurringBilling<K> processor)
        {
            return Mode == OperationMode.Production
                       ? processor.RecurringBillingProductionUri
                       : processor.RecurringBillingDevelopmentUri;
        }

        /// <summary>
        /// Gets the billing profile URI.
        /// </summary>
        /// <param name="processor">The processor.</param>
        /// <returns></returns>
        protected Uri GetBillingProfileUri(ISupportBillingProfiles<K> processor)
        {
            return Mode == OperationMode.Production
                       ? processor.BillingProfileProductionUri
                       : processor.BillingProfileDevelopmentUri;
        }
    }
}