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
using System.Collections.Generic;
using System.Globalization;
using DotNetMerchant.Payments.Authentication;
using DotNetMerchant.Payments.Model;

namespace DotNetMerchant.Payments
{
    /// <summary>
    /// An external source that provides payment services.
    /// </summary>
    public interface IPaymentProcessor
    {
        /// <summary>
        /// The identifying name for this payment processor. Used to match configuration values.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The friendly name for this payment processor.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// The homepage for this payment processor or API.
        /// </summary>
        Uri HomepageUri { get; }

        /// <summary>
        /// The regions this payment processor will support.
        /// </summary>
        IEnumerable<RegionInfo> SupportedRegions { get; }

        /// <summary>
        /// The credit cards this payment processor can support. 
        /// Express multiple types using enum flag syntax.
        /// </summary>
        IEnumerable<CreditCardType> SupportedCreditCardTypes { get; }

        /// <summary>
        /// The authentication scheme used to identify the merchant account
        /// requesting payment processing from this service.
        /// </summary>
        IAuthenticator Authenticator { get; set; }
    }

    /// <summary>
    /// An external source that provides payment services.
    /// </summary>
    /// <typeparam name="T">The type of payment info state</typeparam>
    public interface IPaymentProcessor<T> : IPaymentProcessor where T : IPaymentInfo
    {

    }
}