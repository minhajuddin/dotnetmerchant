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
using System.Linq;
using System.Xml.Linq;
using DotNetMerchant.Extensions;

namespace DotNetMerchant.Payments.Processors.AuthorizeNet
{
    public partial class AuthorizeNetResult : PaymentProcessorResultBase
    {
        /*
        Uri RequestUri { get; set; }
        string TransactionId { get; }
        TransactionStatus TransactionStatus { get; }
        TransactionStatusReason TransactionStatusReason { get; }
        */

        public string ReasonText { get; private set; }
        public string ReasonCode { get; private set; }

        public override void PopulateFromResponse(string response)
        {
            /*
             * <?xml version="1.0" encoding="utf-8"?>
             * <ARBCreateSubscriptionResponse 
             *      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
             *      xmlns:xsd="http://www.w3.org/2001/XMLSchema" 
             *      xmlns="AnetApi/xml/v1/schema/AnetApiSchema.xsd">
             *      <messages>
             *          <resultCode>Ok</resultCode>
             *          <message>
             *              <code>I00001</code>
             *              <text>Successful.</text>
             *          </message>
             *      </messages>
             *      <subscriptionId>592454</subscriptionId>
             * </ARBCreateSubscriptionResponse>
             */

            if (RequestUri.ToString().Contains("xml"))
            {
                PopulateXmlResult(response);
            }
            else
            {
                PopulateTransactionResult(response);
            }
        }

        private void PopulateXmlResult(string response)
        {
            var document = XDocument.Parse(response, LoadOptions.PreserveWhitespace);
            
            if(document.Root == null)
            {
                return;
            }

            var ns = document.GetDefaultNamespace();
            var messagesElement = (XElement) document.Root.FirstNode;
            var resultCode = messagesElement.Element(ns + "resultCode");

            if (document.Root.Nodes().Count() > 1)
            {
                var subscriptionId = document.Root.Nodes().ToArray()[1] as XElement;
                if (subscriptionId != null)
                {
                    TransactionId = subscriptionId.Value;
                }
            }
        }

        private void PopulateTransactionResult(string response)
        {
            var tokens = response.Split(new[] {'|'}, StringSplitOptions.None);
            var responseCode = tokens[0];
            var reasonCode = tokens[2];
            var reasonText = tokens[3];
            var transactionId = tokens[6];

            int responseId;
            int reasonId;

            Int32.TryParse(responseCode, out responseId);
            Int32.TryParse(reasonCode, out reasonId);

            TransactionStatus = _statusMap.TryWithKey(responseId);
            TransactionStatusReason = _reasonMap.TryWithKey(reasonId);
            ReasonText = reasonText;
            ReasonCode = reasonCode;

            TransactionId = transactionId;
        }
    }
}