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
using DotNetMerchant.Extensions;
using DotNetMerchant.Model;
using DotNetMerchant.Payments;
using DotNetMerchant.Payments.Authentication;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors;
using DotNetMerchant.Payments.Processors.Beanstream;

namespace DotNetMerchant.Beanstream
{
    /// <summary>
    /// A Canadian-based payment gateway.
    /// </summary>
    public partial class BeanstreamProcessor : PaymentProcessorBase<BeanstreamInfo, BeanstreamResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeanstreamProcessor"/> class.
        /// </summary>
        /// <param name="merchantId">The merchant id.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public BeanstreamProcessor(string merchantId,
                                   string username,
                                   string password) :
                                       base(new AuthenticationTriplet
                                                {
                                                    First = username,
                                                    Second = password,
                                                    Third = merchantId
                                                })
        {
        }

        /// <summary>
        /// The identifying name for this payment processor. Used to match configuration values.
        /// </summary>
        public override string Name
        {
            get { return "beanstream"; }
        }

        /// <summary>
        /// The friendly name for this payment processor.
        /// </summary>
        public override string DisplayName
        {
            get { return "Beanstream"; }
        }

        /// <summary>
        /// The homepage for this payment processor or API.
        /// </summary>
        public override Uri HomepageUri
        {
            get { return "http://www.beanstream.com".Uri(); }
        }

        /// <summary>
        /// The regions this payment processor will support.
        /// </summary>
        public override IEnumerable<RegionInfo> SupportedRegions
        {
            get { return "CA".Region().AsEnumerable(); }
        }

        /// <summary>
        /// The credit cards this payment processor can support. 
        /// Express multiple types using enum flag syntax.
        /// </summary>
        public override IEnumerable<CreditCardType> SupportedCreditCardTypes
        {
            get { 
                return CreditCardType.Visa
                    .And(CreditCardType.MasterCard)
                    .And(CreditCardType.Amex);
            }
        }

        /// <summary>
        /// Maps the authenticator.
        /// </summary>
        protected override void MapAuthenticator()
        {
            if (Authenticator == null)
            {
                return;
            }

            _info.MerchantId = ((AuthenticationTriplet) Authenticator).Third;
            _info.Username = Authenticator.First;
            _info.Password = Authenticator.Second;
        }

        /// <summary>
        /// Maps the payment method.
        /// </summary>
        protected override void MapPaymentMethod()
        {
            switch (_paymentMethod)
            {
                case PaymentMethod.DebitCard:
                    _info.PaymentMethod = "IO";
                    break;
                case PaymentMethod.CreditCard:
                    _info.PaymentMethod = "CC";
                    break;
            }
        }

        /// <summary>
        /// Maps the type of the transaction.
        /// </summary>
        protected override void MapTransactionType()
        {
            switch (_creditCardTransactionType)
            {
                case CreditCardTransactionType.Capture:
                    _info.TransactionType = "PAC";
                    break;
                case CreditCardTransactionType.Credit:
                    _info.TransactionType = "R";
                    break;
                case CreditCardTransactionType.PreAuthorization:
                    _info.TransactionType = "PA";
                    break;
                case CreditCardTransactionType.Purchase:
                    _info.TransactionType = "P";
                    break;
                case CreditCardTransactionType.Void:
                    _info.TransactionType = "VP";
                    break;
            }
        }

        /// <summary>
        /// Sets the credit card.
        /// </summary>
        /// <param name="card">The card.</param>
        protected override void SetCreditCard(CreditCard card)
        {
            base.SetCreditCard(card);

            _info.CreditCardExpiryMonth = _info.CreditCardExpiry.Substring(0, 2);
            _info.CreditCardExpiryYear = _info.CreditCardExpiry.Substring(2, 2);
        }

        /// <summary>
        /// Sets the order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        public void SetOrderNumber(string orderNumber)
        {
            _info.OrderNumber = orderNumber.Trim();
        }
    }
}