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
using DotNetMerchant.Extensions;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Model.Specifications;
using DotNetMerchant.Payments.Model.Specifications.Cards;

namespace DotNetMerchant.Model.Extensions
{
    public static class CreditCardExtensions
    {
        /// <summary>
        /// Determines if a given credit card is valid.
        /// </summary>
        /// <param name="card">The card to validate</param>
        /// <returns>Whether the credit card is valid</returns>
        public static bool IsValid(this CreditCard card)
        {
            return card.Satisfies<ValidCreditCardSpecification>();
        }

        public static string ToPaddedString(this DateTime value)
        {
            return value.Month.ToString("00") +
                   value.Year.ToString().Substring(2);
        }

        /// <summary>
        /// Determines if a given credit card is valid based on the specified type.
        /// </summary>
        /// <param name="card">The card to validate</param>
        /// <param name="type">The expected card type; if Unknown, the type is not considered</param>
        /// <returns>Whether the credit card is valid or invalid</returns>
        public static bool IsValid(this CreditCard card, CreditCardType type)
        {
            var result = card.IsValid();

            switch (type)
            {
                case CreditCardType.Visa:
                    return result && card.Satisfies<VisaSpecification>();
                case CreditCardType.MasterCard:
                    return result && card.Satisfies<MasterCardSpecification>();
                case CreditCardType.Amex:
                    return result && card.Satisfies<AmexSpecification>();
                case CreditCardType.Discover:
                    return result && card.Satisfies<DiscoverSpecification>();
                case CreditCardType.DinersClub:
                    return result && card.Satisfies<DinersClubSpecification>();
                case CreditCardType.Jcb:
                    return result && card.Satisfies<JcbSpecification>();
                case CreditCardType.Unknown:
                    return result;
                default:
                    throw new ArgumentException("A credit card type is required", "type");
            }
        }

        /// <summary>
        /// Determines if a given credit card is valid, detecting its type.
        /// </summary>
        /// <param name="card">The card to validate</param>
        /// <param name="type">The type of the card, if identified</param>
        /// <returns>Whether the card is valid, and the card's type</returns>
        public static bool IsValid(this CreditCard card, out CreditCardType type)
        {
            var result = card.IsValid();

            type = CreditCardType.Unknown;

            if (card.Satisfies<VisaSpecification>())
            {
                type = CreditCardType.Visa;
            }
            else if (card.Satisfies<MasterCardSpecification>())
            {
                type = CreditCardType.MasterCard;
            }
            else if (card.Satisfies<AmexSpecification>())
            {
                type = CreditCardType.Amex;
            }
            else if (card.Satisfies<DiscoverSpecification>())
            {
                type = CreditCardType.Discover;
            }
            else if (card.Satisfies<DinersClubSpecification>())
            {
                type = CreditCardType.DinersClub;
            }
            else if (card.Satisfies<JcbSpecification>())
            {
                type = CreditCardType.Jcb;
            }

            return result;
        }
    }
}