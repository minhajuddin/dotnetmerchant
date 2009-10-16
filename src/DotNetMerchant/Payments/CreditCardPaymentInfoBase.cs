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

namespace DotNetMerchant.Payments
{
    /// <summary>
    /// Payment info for credit card transactions.
    /// </summary>
    public abstract class CreditCardPaymentInfoBase :
        ICreditCardPaymentInfo
    {
        /// <summary>
        /// Gets or sets the ship to email.
        /// </summary>
        /// <value>The ship to email.</value>
        public abstract string ShipToEmail { get; set; }

        #region ICreditCardPaymentInfo Members

        public abstract string PaymentMethod { get; set; }
        public abstract string TransactionType { get; set; }
        public abstract double TransactionAmount { get; set; }
        public abstract string TransactionId { get; set; }

        public abstract string CreditCardName { get; set; }
        public abstract string CreditCardNumber { get; set; }
        public abstract string CreditCardExpiry { get; set; }
        public abstract string CreditCardVerificationCode { get; set; }

        public abstract string BillToFirstName { get; set; }
        public abstract string BillToLastName { get; set; }
        public abstract string BillToEmail { get; set; }
        public abstract string BillToCompany { get; set; }
        public abstract string BillToPhone { get; set; }
        public abstract string BillToAddress { get; set; }
        public abstract string BillToCity { get; set; }
        public abstract string BillToState { get; set; }
        public abstract string BillToZip { get; set; }
        public abstract string BillToCountry { get; set; }

        public abstract string ShipToFirstName { get; set; }
        public abstract string ShipToLastName { get; set; }
        public abstract string ShipToCompany { get; set; }
        public abstract string ShipToPhone { get; set; }
        public abstract string ShipToAddress { get; set; }
        public abstract string ShipToCity { get; set; }
        public abstract string ShipToState { get; set; }
        public abstract string ShipToZip { get; set; }
        public abstract string ShipToCountry { get; set; }

        #endregion
    }
}