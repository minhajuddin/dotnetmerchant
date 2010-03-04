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
    /// A contract for supporting secure payment profiles. A billing profile
    /// allows you to store credit card information offsite to help safeguard customer
    /// data.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISupportBillingProfiles<T> where T : IPaymentProcessorResult
    {
        /// <summary>
        /// Gets the billing profile production URI.
        /// </summary>
        /// <value>The billing profile production URI.</value>
        Uri BillingProfileProductionUri { get; }

        /// <summary>
        /// Gets the billing profile development URI.
        /// </summary>
        /// <value>The billing profile development URI.</value>
        Uri BillingProfileDevelopmentUri { get; }

        /// <summary>
        /// Creates the billing profile.
        /// </summary>
        /// <param name="billingProfile">The billing profile.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        T CreateBillingProfile(BillingProfile billingProfile, CreditCard card);

        /// <summary>
        /// Updates the billing profile.
        /// </summary>
        /// <param name="billingProfile">The billing profile.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        T UpdateBillingProfile(BillingProfile billingProfile, CreditCard card);

        /// <summary>
        /// Cancels the billing profile.
        /// </summary>
        /// <param name="billingProfile">The billing profile.</param>
        /// <returns></returns>
        T CancelBillingProfile(BillingProfile billingProfile);
    }
}