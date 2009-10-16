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

namespace DotNetMerchant.Payments
{
    /// <summary>
    /// Payment info relevant for credit card transactions.
    /// </summary>
    public interface ICreditCardPaymentInfo : IPaymentInfo
    {
        /// <summary>
        /// Gets or sets the name of the credit card.
        /// </summary>
        /// <value>The name of the credit card.</value>
        string CreditCardName { get; set; }

        /// <summary>
        /// Gets or sets the credit card number.
        /// </summary>
        /// <value>The credit card number.</value>
        string CreditCardNumber { get; set; }
        
        /// <summary>
        /// Gets or sets the credit card expiry.
        /// </summary>
        /// <value>The credit card expiry.</value>
        string CreditCardExpiry { get; set; }
        
        /// <summary>
        /// Gets or sets the credit card verification code.
        /// </summary>
        /// <value>The credit card verification code.</value>
        string CreditCardVerificationCode { get; set; }

        /// <summary>
        /// Gets or sets the first name of the bill to.
        /// </summary>
        /// <value>The first name of the bill to.</value>
        string BillToFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the bill to.
        /// </summary>
        /// <value>The last name of the bill to.</value>
        string BillToLastName { get; set; }
        
        /// <summary>
        /// Gets or sets the bill to email.
        /// </summary>
        /// <value>The bill to email.</value>
        string BillToEmail { get; set; }

        /// <summary>
        /// Gets or sets the bill to company.
        /// </summary>
        /// <value>The bill to company.</value>
        string BillToCompany { get; set; }

        /// <summary>
        /// Gets or sets the bill to phone.
        /// </summary>
        /// <value>The bill to phone.</value>
        string BillToPhone { get; set; }

        /// <summary>
        /// Gets or sets the bill to address.
        /// </summary>
        /// <value>The bill to address.</value>
        string BillToAddress { get; set; }

        /// <summary>
        /// Gets or sets the bill to city.
        /// </summary>
        /// <value>The bill to city.</value>
        string BillToCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the bill to.
        /// </summary>
        /// <value>The state of the bill to.</value>
        string BillToState { get; set; }

        /// <summary>
        /// Gets or sets the bill to zip.
        /// </summary>
        /// <value>The bill to zip.</value>
        string BillToZip { get; set; }

        /// <summary>
        /// Gets or sets the bill to country.
        /// </summary>
        /// <value>The bill to country.</value>
        string BillToCountry { get; set; }

        /// <summary>
        /// Gets or sets the first name of the ship to.
        /// </summary>
        /// <value>The first name of the ship to.</value>
        string ShipToFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the ship to.
        /// </summary>
        /// <value>The last name of the ship to.</value>
        string ShipToLastName { get; set; }

        /// <summary>
        /// Gets or sets the ship to company.
        /// </summary>
        /// <value>The ship to company.</value>
        string ShipToCompany { get; set; }

        /// <summary>
        /// Gets or sets the ship to phone.
        /// </summary>
        /// <value>The ship to phone.</value>
        string ShipToPhone { get; set; }

        /// <summary>
        /// Gets or sets the ship to address.
        /// </summary>
        /// <value>The ship to address.</value>
        string ShipToAddress { get; set; }

        /// <summary>
        /// Gets or sets the ship to city.
        /// </summary>
        /// <value>The ship to city.</value>
        string ShipToCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the ship to.
        /// </summary>
        /// <value>The state of the ship to.</value>
        string ShipToState { get; set; }

        /// <summary>
        /// Gets or sets the ship to zip.
        /// </summary>
        /// <value>The ship to zip.</value>
        string ShipToZip { get; set; }

        /// <summary>
        /// Gets or sets the ship to country.
        /// </summary>
        /// <value>The ship to country.</value>
        string ShipToCountry { get; set; }
    }
}