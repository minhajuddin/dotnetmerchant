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

namespace DotNetMerchant.Payments
{
    /// <summary>
    /// The possible transaction types in a typical credit card processing workflow.
    /// </summary>
    public enum CreditCardTransactionType
    {
        /// <summary>
        /// The transaction type is unknown or not currently set.
        /// </summary>
        Unknown,
        /// <summary>
        /// A combined pre-authorization and capture is performed.
        /// </summary>
        Purchase,
        /// <summary>
        /// A customer's funds are pre-authorized before capturing.
        /// </summary>
        PreAuthorization,
        /// <summary>
        /// A customer's funds are claimed; this is a true charge.
        /// </summary>
        Capture,
        /// <summary>
        /// A pre-existing authorization is cancelled.
        /// </summary>
        Void,
        /// <summary>
        /// A pre-captured amount is refunded.
        /// </summary>
        Credit
    }
}