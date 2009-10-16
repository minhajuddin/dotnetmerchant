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
    public partial class AuthorizeNetProcessor : 
        PaymentProcessorBase<AuthorizeNetInfo, AuthorizeNetResult>,
        ISupportCreditCards<AuthorizeNetResult>,
        ISupportRecurringBilling<AuthorizeNetResult>
    {
        public AuthorizeNetProcessor(string loginId, string transactionKey) :
            base(new AuthenticationPair {First = loginId, Second = transactionKey})
        {
        }

        public override string Name
        {
            get { return "authorizedotnet"; }
        }

        public override string DisplayName
        {
            get { return "Authorize.net"; }
        }

        public override Uri HomepageUri
        {
            get { return "http://authorize.net".Uri(); }
        }

        public override IEnumerable<RegionInfo> SupportedRegions
        {
            get { return "US".Region().AsEnumerable(); }
        }

        public override IEnumerable<CreditCardType> SupportedCreditCardTypes
        {
            get { return _visa.And(_masterCard).And(_amex).And(_discover); }
        }

        public override Uri ProductionUri
        {
            get { return "https://secure.authorize.net/gateway/transact.dll".Uri(); }
        }

        public override Uri DevelopmentUri
        {
            get { return "https://test.authorize.net/gateway/transact.dll".Uri(); }
        }

        public Uri RecurringBillingProductionUri
        {
            get { return "https://api.authorize.net/xml/v1/request.api".Uri(); }
        }

        public Uri RecurringBillingDevelopmentUri
        {
            get { return "https://apitest.authorize.net/xml/v1/request.api".Uri(); }
        }

        public void SetInvoiceNumber(string number)
        {
            _info.InvoiceNumber = number;
        }

        public void SetPurchaseOrderNumber(string number)
        {
            _info.PurchaseOrderNumber = number;
        }

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

        protected override void MapPaymentMethod()
        {
            switch (_paymentMethod)
            {
                case PaymentMethod.CreditCard:
                    _info.PaymentMethod = "CC";
                    break;
            }
        }

        protected override void MapAuthenticator()
        {
            if (Authenticator == null)
            {
                return;
            }

            _info.Login = Authenticator.First;
            _info.TransactionKey = Authenticator.Second;
        }

        protected override void ValidateBillingAddress()
        {
            if (_info.BillToFirstName.IsNullOrBlank() ||
                _info.BillToLastName.IsNullOrBlank())
            {
                throw new ArgumentException("Authorize.net requires the first and last name of the billing recipient to create a recurring billing/");
            }
        }
    }
}