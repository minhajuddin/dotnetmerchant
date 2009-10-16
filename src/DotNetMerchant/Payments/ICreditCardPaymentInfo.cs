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

using DotNetMerchant.Web.Query;

namespace DotNetMerchant.Payments
{
    public interface ICreditCardPaymentInfo : IPaymentInfo
    {
        string CreditCardName { get; set; }
        string CreditCardNumber { get; set; }
        string CreditCardExpiry { get; set; }
        string CreditCardVerificationCode { get; set; }

        string BillToFirstName { get; set; }
        string BillToLastName { get; set; }
        string BillToEmail { get; set; }
        string BillToCompany { get; set; }
        string BillToPhone { get; set; }
        string BillToAddress { get; set; }
        string BillToCity { get; set; }
        string BillToState { get; set; }
        string BillToZip { get; set; }
        string BillToCountry { get; set; }

        string ShipToFirstName { get; set; }
        string ShipToLastName { get; set; }
        string ShipToCompany { get; set; }
        string ShipToPhone { get; set; }
        string ShipToAddress { get; set; }
        string ShipToCity { get; set; }
        string ShipToState { get; set; }
        string ShipToZip { get; set; }
        string ShipToCountry { get; set; }
    }
}