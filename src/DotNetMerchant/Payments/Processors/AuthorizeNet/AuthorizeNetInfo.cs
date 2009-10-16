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
    public class AuthorizeNetInfo : CreditCardPaymentInfoBase
    {
        [Parameter("x_method")]
        public override string PaymentMethod { get; set; }

        [Parameter("x_card_name")]
        public override string CreditCardName { get; set; }

        [Parameter("x_card_num")]
        public override string CreditCardNumber { get; set; }

        [Parameter("x_exp_date")]
        public override string CreditCardExpiry { get; set; }

        [Parameter("x_card_code")]
        public override string CreditCardVerificationCode { get; set; }

        [Parameter("x_type")]
        public override string TransactionType { get; set; }

        [Parameter("x_trans_id")]
        public override string TransactionId { get; set; }

        [Parameter("x_amount")]
        public override double TransactionAmount { get; set; }

        [Parameter("x_version")]
        public string Version { get; set; }

        [Parameter("x_description")]
        public string Description { get; set; }

        [Parameter("x_delim_data")]
        public bool DelimitData
        {
            get { return true; }
        }

        [Parameter("x_delim_char")]
        public char? DelimitCharacter
        {
            get { return '|'; }
        }

        [Parameter("x_login")]
        public string Login { get; set; }

        [Parameter("x_tran_key")]
        public string TransactionKey { get; set; }

        [Parameter("x_invoice_num")]
        public string InvoiceNumber { get; set; }

        [Parameter("x_po_num")]
        public string PurchaseOrderNumber { get; set; }

        [Parameter("x_currency_code")]
        public string CurrencyCode { get; set; }

        // refactor to discrete objects with a mapping strategy
        [Parameter("x_ship_to_first_name")]
        public override string ShipToFirstName { get; set; }

        [Parameter("x_ship_to_last_name")]
        public override string ShipToLastName { get; set; }

        public override string ShipToEmail { get; set; }

        [Parameter("x_ship_to_company")]
        public override string ShipToCompany { get; set; }

        [Parameter("x_ship_to_phone")]
        public override string ShipToPhone { get; set; }

        [Parameter("x_ship_to_address")]
        public override string ShipToAddress { get; set; }

        [Parameter("x_ship_to_city")]
        public override string ShipToCity { get; set; }

        [Parameter("x_ship_to_state")]
        public override string ShipToState { get; set; }

        [Parameter("x_ship_to_zip")]
        public override string ShipToZip { get; set; }

        [Parameter("x_ship_to_country")]
        public override string ShipToCountry { get; set; }

        [Parameter("x_first_name")]
        public override string BillToFirstName { get; set; }

        [Parameter("x_last_name")]
        public override string BillToLastName { get; set; }

        public override string BillToEmail { get; set; }

        [Parameter("x_address")]
        public override string BillToCompany { get; set; }

        [Parameter("x_phone")]
        public override string BillToPhone { get; set; }

        [Parameter("x_company")]
        public override string BillToAddress { get; set; }

        [Parameter("x_city")]
        public override string BillToCity { get; set; }

        [Parameter("x_state")]
        public override string BillToState { get; set; }

        [Parameter("x_zip")]
        public override string BillToZip { get; set; }

        [Parameter("x_country")]
        public override string BillToCountry { get; set; }
    }
}