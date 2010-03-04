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
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Model;

namespace DotNetMerchant.Billing
{
    /// <summary>
    /// A periodic payment is a recurring payment that repeats based on 
    /// a given <see cref="Period" />. A typical example of a periodic payment
    /// is a <see cref="Subscription" />.
    /// </summary>
    public interface IPeriodicPayment : IPayment
    {
        /// <summary>
        /// Gets the period.
        /// </summary>
        /// <value>The period.</value>
        Period Period { get; }

        /// <summary>
        /// Gets the total amount.
        /// </summary>
        /// <value>The total amount.</value>
        Money TotalAmount { get; }

        /// <summary>
        /// Gets the total payments.
        /// </summary>
        /// <value>The total payments.</value>
        int? TotalPayments { get; }

        /// <summary>
        /// Gets the period start date.
        /// </summary>
        /// <value>The period start date.</value>
        DateTime PeriodStartDate { get; }

        /// <summary>
        /// Gets the period end date.
        /// </summary>
        /// <value>The period end date.</value>
        DateTime? PeriodEndDate { get; }

        /// <summary>
        /// Gets the payment start date.
        /// </summary>
        /// <value>The payment start date.</value>
        DateTime PaymentStartDate { get; }
    }
}