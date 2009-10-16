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

namespace DotNetMerchant.Payments
{
    /// <summary>
    /// Common results defining the result of a payment processor transaction request.
    /// </summary>
    public interface IPaymentProcessorResult
    {
        /// <summary>
        /// The original URL sent to the payment processor, useful for debugging request parameters.
        /// </summary>
        Uri RequestUri { get; set; }

        /// <summary>
        /// The unique identifier for the resulting or reference transaction.
        /// </summary>
        string TransactionId { get; }

        /// <summary>
        /// The status of the transaction for this result.
        /// </summary>
        TransactionStatus TransactionStatus { get; }

        /// <summary>
        /// The reason for the <see cref="TransactionStatus"/> for this result.
        /// </summary>
        TransactionStatusReason TransactionStatusReason { get; }

        /// <summary>
        /// Populates a processor result based on a given response string.
        /// </summary>
        /// <param name="response">The response from the payment processor.</param>
        void PopulateFromResponse(string response);
    }
}