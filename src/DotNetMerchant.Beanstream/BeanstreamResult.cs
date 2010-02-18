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
using System.Web;
using DotNetMerchant.Beanstream;
using DotNetMerchant.Extensions;

namespace DotNetMerchant.Payments.Processors.Beanstream
{
    /// <summary>
    /// The results for a transaction returning from <see cref="BeanstreamProcessor" />.
    /// </summary>
    public partial class BeanstreamResult : PaymentProcessorResultBase
    {
        //public string TransactionId { get; protected set; }
        //public TransactionStatus TransactionStatus { get; protected set; }
        //public TransactionStatusReason TransactionStatusReason { get; protected set; }

        /// <summary>
        /// Gets or sets the reason text.
        /// </summary>
        /// <value>The reason text.</value>
        /// 
        public string ReasonText { get; private set; }

        /// <summary>
        /// Gets or sets the reason code.
        /// </summary>
        /// <value>The reason code.</value>
        public string ReasonCode { get; private set; }

        /// <summary>
        /// Gets or sets the bank authorization code.
        /// </summary>
        /// <value>The bank authorization code.</value>
        public string BankAuthorizationCode { get; set; }

        /// <summary>
        /// Gets or sets the recurring billing id, if applicable
        /// </summary>
        /// <value>The recurring billing id.</value>
        public long? RecurringBillingId { get; set; }

        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        /// <value>The transaction date.</value>
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Populates the result instance from an API response.
        /// </summary>
        /// <param name="response">The response.</param>
        public override void PopulateFromResponse(string response)
        {
            var pairs = HttpUtility.ParseQueryString(response);

            var approvedValue = pairs["trnApproved"];
            var reasonValue = pairs["messageId"];

            int reasonCode;
            int approvalCode;

            Int32.TryParse(approvedValue, out approvalCode);
            Int32.TryParse(reasonValue, out reasonCode);

            TransactionId = pairs["trnId"];
            TransactionStatus = _statusMap.TryWithKey(approvalCode);
            ReasonCode = reasonValue;
            ReasonText = pairs["messageText"];
            BankAuthorizationCode = pairs["authCode"];
            RecurringBillingId = Convert.ToInt64(pairs["rbAccountId"]);
            TransactionDate = Convert.ToDateTime(pairs["trnDate"]);
        }
    }
}