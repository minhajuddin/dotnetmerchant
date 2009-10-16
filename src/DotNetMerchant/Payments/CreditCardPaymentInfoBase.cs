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

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>The payment method.</value>
        public abstract string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        public abstract string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        /// <value>The transaction amount.</value>
        public abstract double TransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>The transaction id.</value>
        public abstract string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the credit card.
        /// </summary>
        /// <value>The name of the credit card.</value>
        public abstract string CreditCardName { get; set; }

        /// <summary>
        /// Gets or sets the credit card number.
        /// </summary>
        /// <value>The credit card number.</value>
        public abstract string CreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the credit card expiry.
        /// </summary>
        /// <value>The credit card expiry.</value>
        public abstract string CreditCardExpiry { get; set; }

        /// <summary>
        /// Gets or sets the credit card verification code.
        /// </summary>
        /// <value>The credit card verification code.</value>
        public abstract string CreditCardVerificationCode { get; set; }

        /// <summary>
        /// Gets or sets the first name of the bill to.
        /// </summary>
        /// <value>The first name of the bill to.</value>
        public abstract string BillToFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the bill to.
        /// </summary>
        /// <value>The last name of the bill to.</value>
        public abstract string BillToLastName { get; set; }

        /// <summary>
        /// Gets or sets the bill to email.
        /// </summary>
        /// <value>The bill to email.</value>
        public abstract string BillToEmail { get; set; }

        /// <summary>
        /// Gets or sets the bill to company.
        /// </summary>
        /// <value>The bill to company.</value>
        public abstract string BillToCompany { get; set; }

        /// <summary>
        /// Gets or sets the bill to phone.
        /// </summary>
        /// <value>The bill to phone.</value>
        public abstract string BillToPhone { get; set; }

        /// <summary>
        /// Gets or sets the bill to address.
        /// </summary>
        /// <value>The bill to address.</value>
        public abstract string BillToAddress { get; set; }

        /// <summary>
        /// Gets or sets the bill to city.
        /// </summary>
        /// <value>The bill to city.</value>
        public abstract string BillToCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the bill to.
        /// </summary>
        /// <value>The state of the bill to.</value>
        public abstract string BillToState { get; set; }

        /// <summary>
        /// Gets or sets the bill to zip.
        /// </summary>
        /// <value>The bill to zip.</value>
        public abstract string BillToZip { get; set; }

        /// <summary>
        /// Gets or sets the bill to country.
        /// </summary>
        /// <value>The bill to country.</value>
        public abstract string BillToCountry { get; set; }

        /// <summary>
        /// Gets or sets the first name of the ship to.
        /// </summary>
        /// <value>The first name of the ship to.</value>
        public abstract string ShipToFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the ship to.
        /// </summary>
        /// <value>The last name of the ship to.</value>
        public abstract string ShipToLastName { get; set; }

        /// <summary>
        /// Gets or sets the ship to company.
        /// </summary>
        /// <value>The ship to company.</value>
        public abstract string ShipToCompany { get; set; }

        /// <summary>
        /// Gets or sets the ship to phone.
        /// </summary>
        /// <value>The ship to phone.</value>
        public abstract string ShipToPhone { get; set; }

        /// <summary>
        /// Gets or sets the ship to address.
        /// </summary>
        /// <value>The ship to address.</value>
        public abstract string ShipToAddress { get; set; }

        /// <summary>
        /// Gets or sets the ship to city.
        /// </summary>
        /// <value>The ship to city.</value>
        public abstract string ShipToCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the ship to.
        /// </summary>
        /// <value>The state of the ship to.</value>
        public abstract string ShipToState { get; set; }

        /// <summary>
        /// Gets or sets the ship to zip.
        /// </summary>
        /// <value>The ship to zip.</value>
        public abstract string ShipToZip { get; set; }

        /// <summary>
        /// Gets or sets the ship to country.
        /// </summary>
        /// <value>The ship to country.</value>
        public abstract string ShipToCountry { get; set; }

        #endregion
    }
}