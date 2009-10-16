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

namespace DotNetMerchant.Payments.Model
{
    partial class CreditCard
    {
        private const string AmexNumber = "370000000000002";
        private const string DiscoverNumber = "6011111111111117";
        private const string MasterCardNumber = "5555555555554444";
        private const string VisaNumber = "4111111111111111";

        public static CreditCard VisaTestCard
        {
            get
            {
                return new CreditCard(CreditCardType.Visa,
                                      VisaNumber,
                                      "Test Card",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);
            }
        }

        public static CreditCard MasterCardTestCard
        {
            get
            {
                return new CreditCard(CreditCardType.MasterCard,
                                      MasterCardNumber,
                                      "Test Card",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);
            }
        }

        public static CreditCard AmexTestCard
        {
            get
            {
                return new CreditCard(CreditCardType.Amex,
                                      AmexNumber,
                                      "Test Card",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);
            }
        }

        public static CreditCard DiscoverTestCard
        {
            get
            {
                return new CreditCard(CreditCardType.Discover,
                                      DiscoverNumber,
                                      "Test Card",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);
            }
        }
    }
}