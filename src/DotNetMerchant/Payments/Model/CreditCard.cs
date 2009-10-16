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

using System;
using System.Security;
using DotNetMerchant.Extensions;
using DotNetMerchant.Model.Extensions;

namespace DotNetMerchant.Payments.Model
{
    [Serializable]
    public partial class CreditCard : IPaymentMethod
    {
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

        public SecureString VerificationCode { get; private set; }
        public CreditCardType Type { get; private set; }
        public string CardholderName { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public bool IsValid { get; private set; }
        public bool IsExpired { get; private set; }

        #region IPaymentMethod Members

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