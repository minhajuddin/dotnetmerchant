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
using DotNetMerchant.AuthorizeNet;
using DotNetMerchant.Extensions;
using DotNetMerchant.Model;
using DotNetMerchant.Payments.Model;

namespace DotNetMerchant.Payments.Processors.AuthorizeNet
{
    partial class AuthorizeNetProcessor
    {
        #region ISupportCreditCards<AuthorizeNetResult> Members

        /// <summary>
        /// The service endpoint used for secure transactions.
        /// </summary>
        public Uri CreditCardProductionUri
        {
            get { return "https://secure.authorize.net/gateway/transact.dll".Uri(); }
        }

        /// <summary>
        /// The service endpoint used for testing transactions, if available.
        /// </summary>
        public Uri CreditCardDevelopmentUri
        {
            get { return "https://test.authorize.net/gateway/transact.dll".Uri(); }
        }

        /// <summary>
        /// Authorizes the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public AuthorizeNetResult Authorize(Money amount, CreditCard card)
        {
            var uri = GetCreditCardUri(this);

            return RequestWithMoneyAndCard(CreditCardTransactionType.PreAuthorization, amount, card, uri);
        }

        /// <summary>
        /// Captures the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        public AuthorizeNetResult Capture(Money amount, CreditCard card, string transactionId)
        {
            var uri = GetCreditCardUri(this);

            return RequestWithMoneyCreditCardAndTransaction(CreditCardTransactionType.Capture, amount, card,
                                                            transactionId, uri);
        }

        /// <summary>
        /// Purchases the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public AuthorizeNetResult Purchase(Money amount, CreditCard card)
        {
            var uri = GetCreditCardUri(this);

            return RequestWithMoneyAndCard(CreditCardTransactionType.Purchase, amount, card, uri);
        }

        /// <summary>
        /// Voids the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        public AuthorizeNetResult Void(string transactionId)
        {
            ValidateTransaction(transactionId);

            _info.TransactionId = transactionId;
            _creditCardTransactionType = CreditCardTransactionType.Void;

            var uri = GetCreditCardUri(this);

            return Request(_info, uri);
        }

        /// <summary>
        /// Credits the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        public AuthorizeNetResult Credit(Money amount, CreditCard card, string transactionId)
        {
            ValidateAmountTransaction(amount, transactionId);

            _creditCard = card;
            _creditCardTransactionType = CreditCardTransactionType.Credit;
            _info.TransactionId = transactionId;
            _info.TransactionAmount = amount;

            var uri = GetCreditCardUri(this);

            return Request(_info, uri);
        }

        #endregion
    }
}