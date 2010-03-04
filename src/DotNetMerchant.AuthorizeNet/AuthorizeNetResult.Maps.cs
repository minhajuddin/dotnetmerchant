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

using System.Collections.Generic;
using System.Globalization;
using DotNetMerchant.Payments;

namespace DotNetMerchant.AuthorizeNet
{
    partial class AuthorizeNetResult
    {
        private static readonly IDictionary<int, TransactionStatusReason> _reasonMap
            = new Dictionary<int, TransactionStatusReason>
                  {
                      {1, TransactionStatusReason.NoReason},
                      {11, TransactionStatusReason.Duplicate},
                      {54, TransactionStatusReason.InvalidCriteria},
                      {33, TransactionStatusReason.MissingPriorTransaction},
                      {310, TransactionStatusReason.AlreadyVoided},
                  };

        private static readonly IDictionary<int, TransactionStatus> _statusMap
            = new Dictionary<int, TransactionStatus>
                  {
                      {1, TransactionStatus.Approved},
                      {2, TransactionStatus.Declined},
                      {3, TransactionStatus.Error},
                      {4, TransactionStatus.FraudReview}
                  };

        private static readonly CultureInfo _processorCultureInfo = new CultureInfo("en-US");

        public override CultureInfo ProcessorCultureInfo
        {
            get { return _processorCultureInfo; }
        }
    }
}