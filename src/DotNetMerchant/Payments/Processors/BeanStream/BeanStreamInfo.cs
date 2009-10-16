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
using DotNetMerchant.Web.Attributes;

namespace DotNetMerchant.Payments.Processors.BeanStream
{
    /// <summary>
    /// A state management class for the <see cref="BeanStreamProcessor" />.
    /// </summary>
    [Serializable]
    public partial class BeanStreamInfo : CreditCardPaymentInfoBase
    {
        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        [Parameter("trnType")]
        public override string TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        /// <value>The payment method.</value>
        [Parameter("paymentMethod")]
        public override string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        /// <value>The transaction amount.</value>
        [Parameter("trnAmount")]
        public override double TransactionAmount { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>The transaction id.</value>
        [Parameter("trnId")]
        public override string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the credit card verification code.
        /// </summary>
        /// <value>The credit card verification code.</value>
        [Parameter("trnCardCvd")]
        public override string CreditCardVerificationCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the credit card.
        /// </summary>
        /// <value>The name of the credit card.</value>
        [Parameter("trnCardOwner")]
        public override string CreditCardName { get; set; }

        /// <summary>
        /// Gets or sets the credit card number.
        /// </summary>
        /// <value>The credit card number.</value>
        [Parameter("trnCardNumber")]
        public override string CreditCardNumber { get; set; }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>The order number.</value>
        [Parameter("trnOrderNumber")]
        public string OrderNumber { get; set; }

        [Parameter("adjId")]
        public string AdjustmentId { get; set; }

        /// <summary>
        /// Gets or sets the credit card expiry month.
        /// </summary>
        /// <value>The credit card expiry month.</value>
        [Parameter("trnExpMonth")]
        public string CreditCardExpiryMonth { get; set; }

        /// <summary>
        /// Gets or sets the credit card expiry year.
        /// </summary>
        /// <value>The credit card expiry year.</value>
        [Parameter("trnExpYear")]
        public string CreditCardExpiryYear { get; set; }

        // Split up above
        /// <summary>
        /// Gets or sets the credit card expiry.
        /// </summary>
        /// <value>The credit card expiry.</value>
        public override string CreditCardExpiry { get; set; }

        /// <summary>
        /// Gets the type of the request.
        /// </summary>
        /// <value>The type of the request.</value>
        [Parameter("requestType")]
        public string RequestType
        {
            get { return "BACKEND"; }
        }

        /// <summary>
        /// Gets or sets the merchant id.
        /// </summary>
        /// <value>The merchant id.</value>
        [Parameter("merchant_id")]
        public string MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        [Parameter("username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [Parameter("password")]
        public string Password { get; set; }

        [Parameter("ordName")] // concat with separator?
            public override string BillToFirstName { get; set; }

        //[Parameter("ordName")]
        public override string BillToLastName { get; set; }

        [Parameter("ordEmailAddress")] // regex validator?
            public override string BillToEmail { get; set; }

        [Parameter("ordAddress1")]
        public override string BillToCompany { get; set; }

        [Parameter("ordPhoneNumber")]
        public override string BillToPhone { get; set; }

        [Parameter("ordAddress1")]
        public override string BillToAddress { get; set; }

        [Parameter("ordAddress2")]
        public string BillToAddress2 { get; set; }

        [Parameter("ordCity")]
        public override string BillToCity { get; set; }

        [Parameter("ordProvince")]
        public override string BillToState { get; set; }

        [Parameter("ordPostalCode")]
        public override string BillToZip { get; set; }

        [Parameter("ordCountry")]
        public override string BillToCountry { get; set; }

        public override string ShipToFirstName { get; set; }
        public override string ShipToLastName { get; set; }
        public override string ShipToEmail { get; set; }
        public override string ShipToCompany { get; set; }
        public override string ShipToPhone { get; set; }
        public override string ShipToAddress { get; set; }
        public override string ShipToCity { get; set; }
        public override string ShipToState { get; set; }
        public override string ShipToZip { get; set; }
        public override string ShipToCountry { get; set; }
    }
}