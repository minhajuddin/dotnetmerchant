#region License

// The MIT License
//  
// Copyright (c) 2010 Conatus Creative, Inc.
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
using DotNetMerchant.Extensions;
using DotNetMerchant.Model;
using DotNetMerchant.Payments;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors.Beanstream;
using DotNetMerchant.Payments.Workflow;

namespace DotNetMerchant.Beanstream
{
    partial class BeanstreamProcessor : ISupportCreditCards<BeanstreamResult>
    {
        /// <summary>
        /// Purchases the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public BeanstreamResult Purchase(Money amount, CreditCard card)
        {
            var uri = GetCreditCardUri(this);

            return RequestWithMoneyAndCard(CreditCardTransactionType.Purchase, amount, card, uri);
        }

        /// <summary>
        /// Credits the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns></returns>
        public BeanstreamResult Credit(Money amount, CreditCard card, string transactionId)
        {
            var uri = GetCreditCardUri(this);

            return RequestWithMoneyCreditCardAndTransaction(CreditCardTransactionType.Credit, amount, card, transactionId, uri);
        }

        /// <summary>
        /// Authorizes the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public BeanstreamResult Authorize(Money amount, CreditCard card)
        {
            var uri = GetCreditCardUri(this);

            return RequestWithMoneyAndCard(CreditCardTransactionType.PreAuthorization, amount, card, uri);
        }

        /// <summary>
        /// Captures the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns></returns>
        public BeanstreamResult Capture(Money amount, CreditCard card, string transactionId)
        {
            var uri = GetCreditCardUri(this);

            _info.AdjustmentId = transactionId;

            return RequestWithMoneyCreditCardAndTransaction(CreditCardTransactionType.Capture, amount, card, transactionId, uri);
        }

        /// <summary>
        /// Voids the specified transaction.
        /// </summary>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns></returns>
        public BeanstreamResult Void(string transactionId)
        {
            var uri = GetCreditCardUri(this);

            _info.AdjustmentId = transactionId;

            return RequestWithTransaction(CreditCardTransactionType.Void, transactionId, uri);
        }

        /// <summary>
        /// The service endpoint used for secure credit card transactions.
        /// </summary>
        public Uri CreditCardProductionUri
        {
            get { return "https://www.beanstream.com/scripts/process_transaction.asp".Uri(); }
        }

        /// <summary>
        /// The service endpoint used for testing credit card transactions, if available.
        /// </summary>
        public Uri CreditCardDevelopmentUri
        {
            get { return CreditCardProductionUri;  }
        }
    }
}