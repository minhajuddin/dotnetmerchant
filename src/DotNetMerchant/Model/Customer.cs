using System;
using DotNetMerchant.Billing.Model;

namespace DotNetMerchant.Model
{
    public class Customer : IEntity
    {
        public Identity Id
        {
            get { throw new NotImplementedException(); }
        }

        public BillingProfile BillingProfile
        {
            get; set;

        }
    }
}
