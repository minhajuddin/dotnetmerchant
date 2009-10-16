using System;
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Payments.Model;

namespace DotNetMerchant.Payments.Workflow
{
    internal interface ISupportBillingProfiles<T> where T : IPaymentProcessorResult
    {
        Uri BillingProfileProductionUri { get; }
        Uri BillingProfileDevelopmentUri { get; }
        
        T CreateBillingProfile(BillingProfile billingProfile, CreditCard card);
        T UpdateBillingProfile(BillingProfile billingProfile, CreditCard card);
        T CancelBillingProfile(BillingProfile billingProfile);
    }
}