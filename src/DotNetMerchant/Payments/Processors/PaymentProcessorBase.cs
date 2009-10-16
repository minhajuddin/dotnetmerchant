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
using DotNetMerchant.Model.Extensions;
using DotNetMerchant.Payments.Authentication;
using DotNetMerchant.Payments.Model;
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
        protected CreditCardType _amex = CreditCardType.Amex;
        protected CreditCard _creditCard;
        protected CreditCardTransactionType _creditCardTransactionType;

        protected CreditCardType _diners = CreditCardType.DinersClub;
        protected CreditCardType _discover = CreditCardType.Discover;
        protected T _info;
        protected CreditCardType _jcb = CreditCardType.Jcb;
        protected CreditCardType _masterCard = CreditCardType.MasterCard;
        protected PaymentMethod _paymentMethod;
        protected CreditCardType _visa = CreditCardType.Visa;

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

        public abstract Uri ProductionUri { get; }

        public virtual Uri DevelopmentUri
        {
            get { return ProductionUri; }
        }

        public abstract string Name { get; }
        public abstract string DisplayName { get; }
        public abstract Uri HomepageUri { get; }
        public abstract IEnumerable<CreditCardType> SupportedCreditCardTypes { get; }
        public abstract IEnumerable<RegionInfo> SupportedRegions { get; }
        public IAuthenticator Authenticator { get; set; }

        #endregion

        protected abstract void MapTransactionType();
        protected abstract void MapPaymentMethod();
        protected abstract void MapAuthenticator();

        protected K Request(T info)
        {
            var uri = Mode == OperationMode.Production
                          ? ProductionUri
                          : DevelopmentUri;

            return Request(info, uri);
        }

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

        protected K Request(XDocument xml)
        {
            var uri = Mode == OperationMode.Production
                          ? ProductionUri
                          : DevelopmentUri;

            return Request(xml, uri);
        }
        
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

        protected static void ValidateAmount(Money amount)
        {
            if (amount <= 0.0)
            {
                throw new ArgumentException("You must provide a non-zero amount to authorize.");
            }
        }

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

        protected void ValidateAmountCard(Money amount, CreditCard card)
        {
            ValidateAmount(amount);
            ValidateCreditCard(card);
        }

        protected void ValidateAmountCardTransaction(Money amount, CreditCard card, string transactionId)
        {
            ValidateAmount(amount);
            ValidateCreditCard(card);
            ValidateTransaction(transactionId);
        }

        protected static void ValidateAmountTransaction(Money amount, string transactionId)
        {
            ValidateAmount(amount);
            ValidateTransaction(transactionId);
        }

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

        protected virtual void SetCreditCard(CreditCard card)
        {
            ValidateCreditCard(card);

            _info.CreditCardName = _creditCard.CardholderName;
            _info.CreditCardNumber = _creditCard.AccountNumber.Insecure();
            _info.CreditCardVerificationCode = _creditCard.VerificationCode.Insecure();
            _info.CreditCardExpiry = _creditCard.ExpiryDate.ToPaddedString();
        }

        protected K RequestWithMoneyAndCard(CreditCardTransactionType type, Money amount, CreditCard card)
        {
            ProcessWithMoneyAndCard(amount, card, type);

            return Request(_info);
        }

        protected K RequestWithTransaction(CreditCardTransactionType type, string transactionId)
        {
            ProcessWithTransaction(type, transactionId);

            return Request(_info);
        }

        protected K RequestWithMoneyCardAndTransaction(CreditCardTransactionType type, Money amount, CreditCard card,
                                                       string transactionId)
        {
            ProcessWithMoneyAndCard(amount, card, type);
            ProcessWithMoneyAndTransaction(amount, transactionId, type);

            return Request(_info);
        }

        protected void ProcessWithMoneyAndTransaction(Money amount, string transactionId, CreditCardTransactionType type)
        {
            ValidateAmountTransaction(amount, transactionId);

            _creditCardTransactionType = type;
            _info.TransactionAmount = amount;
            _info.TransactionId = transactionId;
        }

        protected void ProcessWithTransaction(CreditCardTransactionType type, string transactionId)
        {
            ValidateTransaction(transactionId);

            _creditCardTransactionType = type;
            _info.TransactionId = transactionId;
        }

        protected void ProcessWithMoneyAndCard(Money amount, CreditCard card, CreditCardTransactionType type)
        {
            ValidateAmountCard(amount, card);

            _creditCardTransactionType = type;
            _paymentMethod = PaymentMethod.CreditCard;
            _creditCard = card;
            _info.TransactionAmount = amount;
        }

        protected static void ValidateSubscription(Subscription subscription)
        {
            if(subscription.TrialPeriod.HasValue && 
               !subscription.TrialPeriod.Value.Equals(subscription.Period))
            {
                throw new ArgumentException("Authorize.net does not support trial periods that do not match the billing period.");
            }
        }

        protected virtual void ValidateBillingAddress()
        {
            return;
        }
    }
}