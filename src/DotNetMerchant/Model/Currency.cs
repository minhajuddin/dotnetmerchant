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

namespace DotNetMerchant.Model
{
    /// <summary>
    /// Represents world currency by numeric and alphabetic values, as per ISO 4217:
    /// http://www.iso.org/iso/currency_codes_list-1. This enum is implicitly converted
    /// to <see cref="CurrencyInfo" /> instances internally, so you only need to reference this
    /// code to work with rich currency objects. 
    /// </summary>
    [Serializable]
    public enum Currency : ushort
    {
        /// <summary>
        /// USD
        /// </summary>
        USD = 840,
        /// <summary>
        /// CAD
        /// </summary>
        CAD = 124,
        /// <summary>
        /// EUR
        /// </summary>
        EUR = 978,
        /// <summary>
        /// AUD
        /// </summary>
        AUD = 036,
        /// <summary>
        /// GBP
        /// </summary>
        GBP = 826,
        /// <summary>
        /// INR
        /// </summary>
        INR = 356,
        /// <summary>
        /// JPY
        /// </summary>
        JPY = 392,
        /// <summary>
        /// CHF
        /// </summary>
        CHF = 756,
        /// <summary>
        /// NZD
        /// </summary>
        NZD = 554
    }
}