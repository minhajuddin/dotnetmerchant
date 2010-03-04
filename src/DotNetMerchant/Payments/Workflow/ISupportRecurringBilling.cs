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
using DotNetMerchant.Payments.Model;

namespace DotNetMerchant.Payments.Workflow
{
    /// <summary>
    /// A contract for processors that support recurring billing. Recurring billing allows
    /// you to continually bill a customer for a subscription without requiring credit card
    /// information stored on your server.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISupportRecurringBilling<T> where T : IPaymentProcessorResult
    {
        /// <summary>
        /// Gets the recurring billing production URI.
        /// </summary>
        /// <value>The recurring billing production URI.</value>
        Uri RecurringBillingProductionUri { get; }

        /// <summary>
        /// Gets the recurring billing development URI.
        /// </summary>
        /// <value>The recurring billing development URI.</value>
        Uri RecurringBillingDevelopmentUri { get; }

        /// <summary>
        /// Creates the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        T CreateRecurringBilling(Subscription subscription, CreditCard card);

        /// <summary>
        /// Updates the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        T UpdateRecurringBilling(Subscription subscription, CreditCard card);

        /// <summary>
        /// Cancels the recurring billing.
        /// </summary>
        /// <param name="subscription">The subscription.</param>
        /// <returns></returns>
        T CancelRecurringBilling(Subscription subscription);
    }
}