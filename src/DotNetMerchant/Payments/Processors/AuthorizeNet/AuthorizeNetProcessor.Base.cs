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
using DotNetMerchant.Payments.Authentication;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Workflow;

namespace DotNetMerchant.Payments.Processors.AuthorizeNet
{
    /// <summary>
    /// A US-based payment gateway.
    /// </summary>
    public partial class AuthorizeNetProcessor :
        PaymentProcessorBase<AuthorizeNetInfo, AuthorizeNetResult>,
        ISupportCreditCards<AuthorizeNetResult>,
        ISupportRecurringBilling<AuthorizeNetResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeNetProcessor"/> class.
        /// </summary>
        /// <param name="loginId">The login id.</param>
        /// <param name="transactionKey">The transaction key.</param>
        public AuthorizeNetProcessor(string loginId, string transactionKey) :
            base(new AuthenticationPair {First = loginId, Second = transactionKey})
        {
        }

        /// <summary>
        /// The identifying name for this payment processor. Used to match configuration values.
        /// </summary>
        public override string Name
        {
            get { return "authorizedotnet"; }
        }

        /// <summary>
        /// The friendly name for this payment processor.
        /// </summary>
        public override string DisplayName
        {
            get { return "Authorize.net"; }
        }

        /// <summary>
        /// The homepage for this payment processor or API.
        /// </summary>
        public override Uri HomepageUri
        {
            get { return "http://authorize.net".Uri(); }
        }

        /// <summary>
        /// The regions this payment processor will support.
        /// </summary>
        public override IEnumerable<RegionInfo> SupportedRegions
        {
            get { return "US".Region().AsEnumerable(); }
        }

        /// <summary>
        /// The credit cards this payment processor can support. 
        /// Express multiple types using enum flag syntax.
        /// </summary>
        public override IEnumerable<CreditCardType> SupportedCreditCardTypes
        {
            get
            {
                return CreditCardType.Visa
                    .And(CreditCardType.MasterCard)
                    .And(CreditCardType.Amex)
                    .And(CreditCardType.Discover);
            }
        }

        #region ISupportRecurringBilling<AuthorizeNetResult> Members

        /// <summary>
        /// Gets the recurring billing production URI.
        /// </summary>
        /// <value>The recurring billing production URI.</value>
        public Uri RecurringBillingProductionUri
        {
            get { return "https://api.authorize.net/xml/v1/request.api".Uri(); }
        }

        /// <summary>
        /// Gets the recurring billing development URI.
        /// </summary>
        /// <value>The recurring billing development URI.</value>
        public Uri RecurringBillingDevelopmentUri
        {
            get { return "https://apitest.authorize.net/xml/v1/request.api".Uri(); }
        }

        #endregion

        /// <summary>
        /// Sets the invoice number.
        /// </summary>
        /// <param name="number">The number.</param>
        public void SetInvoiceNumber(string number)
        {
            _info.InvoiceNumber = number;
        }

        /// <summary>
        /// Sets the purchase order number.
        /// </summary>
        /// <param name="number">The number.</param>
        public void SetPurchaseOrderNumber(string number)
        {
            _info.PurchaseOrderNumber = number;
        }

        /// <summary>
        /// Maps the type of the transaction.
        /// </summary>
        protected override void MapTransactionType()
        {
            switch (_creditCardTransactionType)
            {
                case CreditCardTransactionType.Capture:
                    _info.TransactionType = "PRIOR_AUTH_CAPTURE";
                    break;
                case CreditCardTransactionType.PreAuthorization:
                    _info.TransactionType = "AUTH_ONLY";
                    break;
                case CreditCardTransactionType.Purchase:
                    _info.TransactionType = "AUTH_CAPTURE";
                    break;
                case CreditCardTransactionType.Void:
                    _info.TransactionType = "VOID";
                    break;
                case CreditCardTransactionType.Credit:
                    _info.TransactionType = "CREDIT";
                    break;
            }
        }

        /// <summary>
        /// Maps the payment method.
        /// </summary>
        protected override void MapPaymentMethod()
        {
            switch (_paymentMethod)
            {
                case PaymentMethod.CreditCard:
                    _info.PaymentMethod = "CC";
                    break;
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

            _info.Login = Authenticator.First;
            _info.TransactionKey = Authenticator.Second;
        }

        /// <summary>
        /// Validates the billing address.
        /// </summary>
        protected override void ValidateBillingAddress()
        {
            if (_info.BillToFirstName.IsNullOrBlank() ||
                _info.BillToLastName.IsNullOrBlank())
            {
                throw new ArgumentException(
                    "Authorize.net requires the first and last name of the billing recipient to create a recurring billing/");
            }
        }
    }
}