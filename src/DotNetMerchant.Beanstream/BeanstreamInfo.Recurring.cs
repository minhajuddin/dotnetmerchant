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
using Hammock.Attributes.Specialized;
using Hammock.Attributes.Validation;

namespace DotNetMerchant.Beanstream
{
    partial class BeanstreamInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether the transaction is a recurring billing request.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this transaction is meant for recurring billing; otherwise, <c>false</c>.
        /// </value>
        [Parameter("trnRecurring"), BooleanToInteger]
        public bool IsRecurring { get; set; }

        /// <summary>
        /// Gets or sets the billing period.
        /// </summary>
        /// <value>The billing period.</value>
        [Parameter("rbBillingPeriod")]
        public string BillingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the billing increment.
        /// </summary>
        /// <value>The billing increment.</value>
        [Parameter("rbBillingIncrement")]
        public int BillingIncrement { get; set; }

        /// <summary>
        /// Gets or sets the first billing.
        /// </summary>
        /// <value>The first billing.</value>
        [Parameter("rbFirstBilling"), DateTimeFormat("MMddyyyy")]
        public DateTime FirstBilling { get; set; }

        /// <summary>
        /// Gets or sets the billing expiry.
        /// </summary>
        /// <value>The billing expiry.</value>
        [Parameter("rbExpiry"), DateTimeFormat("MMddyyyy")]
        public DateTime? Expiry { get; set; }

        /// <summary>
        /// Gets or sets the second billing.
        /// </summary>
        /// <value>The second billing.</value>
        [Parameter("rbSecondBilling"), DateTimeFormat("MMddyyyy")]
        public DateTime? SecondBilling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to charge for the first payment
        /// immediately, or wait until the first billing date to charge for the subscription.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the charge will occur immediately; otherwise, <c>false</c>.
        /// </value>
        [Parameter("rbCharge"), BooleanToInteger]
        public bool ChargeOnReceipt { get; set; }

        /// <summary>
        /// Gets or sets the recurring billing service version.
        /// </summary>
        /// <value>The recurring billing service version.</value>
        [Parameter("serviceVersion")]
        public string ServiceVersion { get; set; }

        /// <summary>
        /// Gets or sets the type of the operation.
        /// </summary>
        /// <value>The type of the operation.</value>
        [Parameter("operationType")]
        public string OperationType { get; set; }

        /// <summary>
        /// Gets or sets the pass code.
        /// </summary>
        /// <value>The pass code.</value>
        [Parameter("passCode")]
        public string PassCode { get; set; }
    }
}