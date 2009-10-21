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

using DotNetMerchant.Web.Attributes;

namespace DotNetMerchant.Payments.Processors.AuthorizeNet
{
    /// <summary>
    /// A state management class for the <see cref="AuthorizeNetProcessor"/>.
    /// </summary>
    public class AuthorizeNetInfo : CreditCardPaymentInfoBase
    {
        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>The payment method.</value>
        [Parameter("x_method")]
        public override string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the name of the credit card.
        /// </summary>
        /// <value>The name of the credit card.</value>
        [Parameter("x_card_name")]
        public override string CreditCardName { get; set; }

        /// <summary>
        /// Gets or sets the credit card number.
        /// </summary>
        /// <value>The credit card number.</value>
        [Parameter("x_card_num")]
        public override string CreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the credit card expiry.
        /// </summary>
        /// <value>The credit card expiry.</value>
        [Parameter("x_exp_date")]
        public override string CreditCardExpiry { get; set; }

        /// <summary>
        /// Gets or sets the credit card verification code.
        /// </summary>
        /// <value>The credit card verification code.</value>
        [Parameter("x_card_code")]
        public override string CreditCardVerificationCode { get; set; }

        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        [Parameter("x_type")]
        public override string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>The transaction id.</value>
        [Parameter("x_trans_id")]
        public override string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        /// <value>The transaction amount.</value>
        [Parameter("x_amount")]
        public override double TransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [Parameter("x_version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [Parameter("x_description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets a value indicating whether [delimit data].
        /// </summary>
        /// <value><c>true</c> if [delimit data]; otherwise, <c>false</c>.</value>
        [Parameter("x_delim_data")]
        public bool DelimitData
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the delimiter character.
        /// </summary>
        /// <value>The delimiter character.</value>
        [Parameter("x_delim_char")]
        public char? DelimitCharacter
        {
            get { return '|'; }
        }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>The login.</value>
        [Parameter("x_login")]
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets the transaction key.
        /// </summary>
        /// <value>The transaction key.</value>
        [Parameter("x_tran_key")]
        public string TransactionKey { get; set; }

        /// <summary>
        /// Gets or sets the invoice number.
        /// </summary>
        /// <value>The invoice number.</value>
        [Parameter("x_invoice_num")]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Gets or sets the purchase order number.
        /// </summary>
        /// <value>The purchase order number.</value>
        [Parameter("x_po_num")]
        public string PurchaseOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>The currency code.</value>
        [Parameter("x_currency_code")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the first name of the ship to.
        /// </summary>
        /// <value>The first name of the ship to.</value>
        [Parameter("x_ship_to_first_name")]
        public override string ShipToFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the ship to.
        /// </summary>
        /// <value>The last name of the ship to.</value>
        [Parameter("x_ship_to_last_name")]
        public override string ShipToLastName { get; set; }

        //[Parameter("x_ship_to_email")]
        /// <summary>
        /// Gets or sets the ship to email.
        /// </summary>
        /// <value>The ship to email.</value>
        public override string ShipToEmail { get; set; }

        /// <summary>
        /// Gets or sets the ship to company.
        /// </summary>
        /// <value>The ship to company.</value>
        [Parameter("x_ship_to_company")]
        public override string ShipToCompany { get; set; }

        /// <summary>
        /// Gets or sets the ship to phone.
        /// </summary>
        /// <value>The ship to phone.</value>
        [Parameter("x_ship_to_phone")]
        public override string ShipToPhone { get; set; }

        /// <summary>
        /// Gets or sets the ship to address.
        /// </summary>
        /// <value>The ship to address.</value>
        [Parameter("x_ship_to_address")]
        public override string ShipToAddress { get; set; }

        /// <summary>
        /// Gets or sets the ship to city.
        /// </summary>
        /// <value>The ship to city.</value>
        [Parameter("x_ship_to_city")]
        public override string ShipToCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the ship to.
        /// </summary>
        /// <value>The state of the ship to.</value>
        [Parameter("x_ship_to_state")]
        public override string ShipToState { get; set; }

        /// <summary>
        /// Gets or sets the ship to zip.
        /// </summary>
        /// <value>The ship to zip.</value>
        [Parameter("x_ship_to_zip")]
        public override string ShipToZip { get; set; }

        /// <summary>
        /// Gets or sets the ship to country.
        /// </summary>
        /// <value>The ship to country.</value>
        [Parameter("x_ship_to_country")]
        public override string ShipToCountry { get; set; }

        /// <summary>
        /// Gets or sets the first name of the bill to.
        /// </summary>
        /// <value>The first name of the bill to.</value>
        [Parameter("x_first_name")]
        public override string BillToFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the bill to.
        /// </summary>
        /// <value>The last name of the bill to.</value>
        [Parameter("x_last_name")]
        public override string BillToLastName { get; set; }

        //[Parameter("x_bill_to_email")]
        /// <summary>
        /// Gets or sets the bill to email.
        /// </summary>
        /// <value>The bill to email.</value>
        public override string BillToEmail { get; set; }

        /// <summary>
        /// Gets or sets the bill to company.
        /// </summary>
        /// <value>The bill to company.</value>
        [Parameter("x_address")]
        public override string BillToCompany { get; set; }

        /// <summary>
        /// Gets or sets the bill to phone.
        /// </summary>
        /// <value>The bill to phone.</value>
        [Parameter("x_phone")]
        public override string BillToPhone { get; set; }

        /// <summary>
        /// Gets or sets the bill to address.
        /// </summary>
        /// <value>The bill to address.</value>
        [Parameter("x_company")]
        public override string BillToAddress { get; set; }

        /// <summary>
        /// Gets or sets the bill to city.
        /// </summary>
        /// <value>The bill to city.</value>
        [Parameter("x_city")]
        public override string BillToCity { get; set; }

        /// <summary>
        /// Gets or sets the state of the bill to.
        /// </summary>
        /// <value>The state of the bill to.</value>
        [Parameter("x_state")]
        public override string BillToState { get; set; }

        /// <summary>
        /// Gets or sets the bill to zip.
        /// </summary>
        /// <value>The bill to zip.</value>
        [Parameter("x_zip")]
        public override string BillToZip { get; set; }

        /// <summary>
        /// Gets or sets the bill to country.
        /// </summary>
        /// <value>The bill to country.</value>
        [Parameter("x_country")]
        public override string BillToCountry { get; set; }
    }
}