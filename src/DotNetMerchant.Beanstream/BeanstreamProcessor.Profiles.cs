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
using DotNetMerchant.Extensions;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors.Beanstream;
using DotNetMerchant.Payments.Workflow;

namespace DotNetMerchant.Beanstream
{
    partial class BeanstreamProcessor : ISupportBillingProfiles<BeanstreamResult>
    {
        #region ISupportBillingProfiles<BeanstreamResult> Members

        /// <summary>
        /// Gets the billing profile production URI.
        /// </summary>
        /// <value>The billing profile production URI.</value>
        public Uri BillingProfileProductionUri
        {
            get { return "https://www.beanstream.com/scripts/payment_profile.asp".Uri(); }
        }

        /// <summary>
        /// Gets the billing profile development URI.
        /// </summary>
        /// <value>The billing profile development URI.</value>
        public Uri BillingProfileDevelopmentUri
        {
            get { return BillingProfileProductionUri; }
        }

        /// <summary>
        /// Creates the billing profile.
        /// </summary>
        /// <param name="billingProfile">The billing profile.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public BeanstreamResult CreateBillingProfile(BillingProfile billingProfile, CreditCard card)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the billing profile.
        /// </summary>
        /// <param name="billingProfile">The billing profile.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        public BeanstreamResult UpdateBillingProfile(BillingProfile billingProfile, CreditCard card)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cancels the billing profile.
        /// </summary>
        /// <param name="billingProfile">The billing profile.</param>
        /// <returns></returns>
        public BeanstreamResult CancelBillingProfile(BillingProfile billingProfile)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}