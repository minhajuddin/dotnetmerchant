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

using DotNetMerchant.Model;

namespace DotNetMerchant.Payments.Workflow
{
    /// <summary>
    /// A contract for processing providers that can authorize and capture money.
    /// </summary>
    /// <typeparam name="T">The processor result</typeparam>
    /// <typeparam name="K">The payment method</typeparam>
    public interface IPurchaseAuthorizationWorkflow<T, K> : IPurchaseWorkflow<T, K>
        where T : IPaymentProcessorResult
        where K : IPaymentMethod
    {
        /// <summary>
        /// Authorizes the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <returns></returns>
        T Authorize(Money amount, K card);

        /// <summary>
        /// Captures the specified amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="card">The card.</param>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        T Capture(Money amount, K card, string transactionId);

        /// <summary>
        /// Voids the specified transaction id.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <returns></returns>
        T Void(string transactionId);
    }
}