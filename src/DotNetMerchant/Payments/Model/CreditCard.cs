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
using System.Security;
using DotNetMerchant.Extensions;
using DotNetMerchant.Payments.Model.Extensions;

namespace DotNetMerchant.Payments.Model
{
    /// <summary>
    /// A credit card.
    /// </summary>
    [Serializable]
    public partial class CreditCard : IPaymentMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCard"/> class.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="cardholderName">The name of the cardholder.</param>
        /// <param name="expiryMonth">The expiry month.</param>
        /// <param name="expiryYear">The expiry year.</param>
        public CreditCard(CreditCardType expectedType,
                          string accountNumber,
                          string cardholderName,
                          int expiryMonth,
                          int expiryYear)
        {
            SetInitialValues(accountNumber, cardholderName, expiryMonth, expiryYear);

            Type = expectedType;
            IsValid = this.IsValid(Type);
            IsExpired = ExpiryDate >= DateTime.Today;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCard"/> class.
        /// </summary>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="cardholderName">The name of the cardholder.</param>
        /// <param name="verificationCode">The verification code.</param>
        /// <param name="expiryMonth">The expiry month.</param>
        /// <param name="expiryYear">The expiry year.</param>
        public CreditCard(CreditCardType expectedType,
                          string accountNumber,
                          string cardholderName,
                          string verificationCode,
                          int expiryMonth,
                          int expiryYear) : this(expectedType, accountNumber, cardholderName, expiryMonth, expiryYear)
        {
            VerificationCode = verificationCode
                .Replace("-", "").Replace(" ", "")
                .Secure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCard"/> class.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="cardholderName">The name of the cardholder.</param>
        /// <param name="expiryMonth">The expiry month.</param>
        /// <param name="expiryYear">The expiry year.</param>
        public CreditCard(string accountNumber,
                          string cardholderName,
                          int expiryMonth,
                          int expiryYear)
        {
            SetInitialValues(accountNumber, cardholderName, expiryMonth, expiryYear);

            CreditCardType type;
            IsValid = this.IsValid(out type);
            IsExpired = DateTime.Today > ExpiryDate;
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCard"/> class.
        /// </summary>
        /// <param name="accountNumber">The account number.</param>
        /// <param name="cardholderName">The name of the cardholder.</param>
        /// <param name="verificationCode">The verification code.</param>
        /// <param name="expiryMonth">The expiry month.</param>
        /// <param name="expiryYear">The expiry year.</param>
        public CreditCard(string accountNumber,
                          string cardholderName,
                          string verificationCode,
                          int expiryMonth,
                          int expiryYear) : this(accountNumber, cardholderName, expiryMonth, expiryYear)
        {
            VerificationCode = verificationCode
                .Replace("-", "").Replace(" ", "")
                .Secure();
        }

        /// <summary>
        /// Gets or sets the verification code.
        /// </summary>
        /// <value>The verification code.</value>
        public SecureString VerificationCode { get; private set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public CreditCardType Type { get; private set; }

        /// <summary>
        /// Gets or sets the name of the cardholder.
        /// </summary>
        /// <value>The name of the cardholder.</value>
        public string CardholderName { get; private set; }

        /// <summary>
        /// Gets or sets the expiry date.
        /// </summary>
        /// <value>The expiry date.</value>
        public DateTime ExpiryDate { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expired.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired { get; private set; }

        #region IPaymentMethod Members

        /// <summary>
        /// Gets the account number.
        /// </summary>
        /// <value>The account number.</value>
        public SecureString AccountNumber { get; private set; }

        #endregion

        private void SetInitialValues(string accountNumber,
                                      string cardholderName,
                                      int expiryMonth,
                                      int expiryYear)
        {
            if (expiryYear < 100)
            {
                expiryYear += 2000;
            }

            ValidateCreditCardInputs(accountNumber,
                                     cardholderName,
                                     expiryMonth,
                                     expiryYear);

            AccountNumber = accountNumber
                .Replace("-", "").Replace(" ", "")
                .Secure();

            CardholderName = cardholderName.Trim();
            var expiryDay = DateTime.DaysInMonth(expiryYear, expiryMonth);
            ExpiryDate = new DateTime(expiryYear, expiryMonth, expiryDay);
        }

        private static void ValidateCreditCardInputs(string accountNumber,
                                                     string cardholderName,
                                                     int expiryMonth,
                                                     int expiryYear)
        {
            if (accountNumber.IsNullOrBlank())
            {
                throw new ArgumentNullException("accountNumber", "You must provide an account number.");
            }

            if (cardholderName.IsNullOrBlank())
            {
                throw new ArgumentNullException("cardholderName", "You must provide a cardholder name.");
            }

            if (expiryMonth < 1 || expiryMonth > 12)
            {
                throw new ArgumentOutOfRangeException("expiryMonth", "You must provide a valid expiration month.");
            }

            // Input in range 100 to infinity
            if (expiryYear < 100)
            {
                throw new ArgumentOutOfRangeException("expiryYear", "You must provide a valid expiration year.");
            }
        }
    }
}