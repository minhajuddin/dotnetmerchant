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