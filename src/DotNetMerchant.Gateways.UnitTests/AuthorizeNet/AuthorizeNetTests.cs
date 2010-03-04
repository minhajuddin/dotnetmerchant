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

using DotNetMerchant.Gateways.UnitTests;
using DotNetMerchant.Payments;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Processors.AuthorizeNet
{
    [TestFixture]
    public partial class AuthorizeNetTests : ProcessorTestBase
    {
        public AuthorizeNetTests() : base("authorizedotnet",
                                          "loginId",
                                          "transactionKey",
                                          null)
        {
        }

        [Test]
        public void Can_capture_authorized_transaction()
        {
            const double amount = 0.05;

            var auth = ProcessAuth(amount);
            Assert.IsTrue(auth.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(auth.TransactionStatusReason == TransactionStatusReason.NoReason);

            var capture = ProcessCapture(amount, auth.TransactionId);
            Assert.IsTrue(capture.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(capture.TransactionStatusReason == TransactionStatusReason.NoReason);
        }

        [Test]
        public void Can_duplicate_void_authorization_request_safely()
        {
            var auth = ProcessAuth(0.04);
            Assert.IsTrue(auth.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(auth.TransactionStatusReason == TransactionStatusReason.NoReason);

            var firstVoid = ProcessVoid(auth.TransactionId);
            Assert.IsTrue(firstVoid.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(firstVoid.TransactionStatusReason == TransactionStatusReason.NoReason);

            var secondVoid = ProcessVoid(auth.TransactionId);
            Assert.IsNotNull(secondVoid);
            Assert.IsTrue(secondVoid.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(secondVoid.TransactionStatusReason == TransactionStatusReason.AlreadyVoided);
        }

        [Test]
        public void Can_handle_duplicate_auth_request_safely()
        {
            // different amounts to avoid test collisions
            var firstAuth = ProcessAuth(0.02);

            Assert.IsNotNull(firstAuth);
            Assert.IsTrue(firstAuth.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(firstAuth.TransactionStatusReason == TransactionStatusReason.NoReason);

            var secondAuth = ProcessAuth(0.02);

            Assert.IsNotNull(secondAuth);
            Assert.IsTrue(secondAuth.TransactionStatus == TransactionStatus.Error);
            Assert.IsTrue(secondAuth.TransactionStatusReason == TransactionStatusReason.Duplicate);
        }

        [Test]
        [Ignore("Test accounts do not have the facility to credit a transaction as they must be live.")]
        public void Can_perform_credit_transaction()
        {
            const double amount = 0.18;

            var purchase = ProcessPurchase(amount);
            Assert.IsTrue(purchase.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(purchase.TransactionStatusReason == TransactionStatusReason.NoReason);

            var refund = ProcessCredit(amount, purchase.TransactionId);
            Assert.IsTrue(refund.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(refund.TransactionStatusReason == TransactionStatusReason.NoReason);
        }

        [Test]
        public void Can_perform_purchase_transaction()
        {
            const double amount = 0.06;

            var purchase = ProcessPurchase(amount);
            Assert.IsTrue(purchase.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(purchase.TransactionStatusReason == TransactionStatusReason.NoReason);
        }

        [Test]
        public void Can_process_authorization_request()
        {
            var response = ProcessAuth(0.01);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(response.TransactionStatusReason == TransactionStatusReason.NoReason);
        }

        [Test]
        public void Can_void_authorization_request()
        {
            var auth = ProcessAuth(0.07);
            Assert.IsTrue(auth.TransactionStatus == TransactionStatus.Approved);

            var voided = ProcessVoid(auth.TransactionId);

            Assert.IsNotNull(voided);
            Assert.IsTrue(voided.TransactionStatus == TransactionStatus.Approved);
            Assert.IsTrue(voided.TransactionStatusReason == TransactionStatusReason.NoReason);
        }
    }
}