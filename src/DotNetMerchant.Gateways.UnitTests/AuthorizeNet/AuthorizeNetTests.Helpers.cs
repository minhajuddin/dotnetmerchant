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
using DotNetMerchant.AuthorizeNet;
using DotNetMerchant.Billing.Model;
using DotNetMerchant.Model;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Processors.AuthorizeNet;

namespace DotNetMerchant.UnitTests.Processors.AuthorizeNet
{
    partial class AuthorizeNetTests
    {
        private AuthorizeNetResult ProcessCredit(double amount, string transactionId)
        {
            var creditCard = new CreditCard("4111 1111 1111 1111", "John Q Customer", 09, 09);
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            return authDotNet.Credit(amount, creditCard, transactionId);
        }

        private AuthorizeNetResult ProcessCapture(double amount, string transactionId)
        {
            var creditCard = new CreditCard("4111 1111 1111 1111", "John Q Customer", 09, 09);
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            return authDotNet.Capture(amount, creditCard, transactionId);
        }

        private AuthorizeNetResult ProcessPurchase(double amount)
        {
            var creditCard = new CreditCard("4111 1111 1111 1111", "John Q Customer", 09, 09);
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            return authDotNet.Purchase(amount, creditCard);
        }

        private AuthorizeNetResult ProcessAuth(double amount)
        {
            var creditCard = new CreditCard("4111 1111 1111 1111", "John Q Customer", 11, 09);
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);

            return authDotNet.Authorize(amount, creditCard);
        }

        private AuthorizeNetResult ProcessVoid(string transactionId)
        {
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            return authDotNet.Void(transactionId);
        }

        private AuthorizeNetResult CreateRecurringBilling(Subscription subscription)
        {
            var expiryMonth = DateTime.UtcNow.Month;
            var expiryYear = DateTime.UtcNow.Year;
            var creditCard = new CreditCard("4111 1111 1111 1111", "John Q Customer", expiryMonth, expiryYear);

            return CreateRecurringBilling(subscription, creditCard);
        }

        private AuthorizeNetResult CreateRecurringBilling(Subscription subscription, CreditCard card)
        {
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            var billTo = new Address {FirstName = "John", LastName = "Customer"};

            authDotNet.SetBillingAddress(billTo);

            var result = authDotNet.CreateRecurringBilling(subscription, card);
            return result;
        }

        private AuthorizeNetResult UpdateRecurringBilling(Subscription subscription, CreditCard card)
        {
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);
            var billTo = new Address {FirstName = "John", LastName = "Customer"};

            authDotNet.SetBillingAddress(billTo);

            var result = authDotNet.UpdateRecurringBilling(subscription, card);

            return result;
        }

        private AuthorizeNetResult CancelRecurringBilling(Subscription subscription)
        {
            var authDotNet = new AuthorizeNetProcessor(CredentialFirst, CredentialSecond);

            var result = authDotNet.CancelRecurringBilling(subscription);

            return result;
        }
    }
}