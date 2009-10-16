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
using DotNetMerchant.Specifications;

namespace DotNetMerchant.Payments.Model.Specifications
{
    // http://en.wikipedia.org/wiki/Luhn_algorithm
    /*
        def check_number(digits):
        _sum = 0
        alt = False
        for d in reversed(digits):
            assert 0 <= d <= 9
            if alt:
                d *= 2
                if d > 9:
                    d -= 9
            _sum += d
            alt = not alt
        return (_sum % 10) == 0 
     */

    internal class ValidCreditCardSpecification : SpecificationBase<CreditCard>
    {
        public override bool IsSatisfiedBy(CreditCard instance)
        {
            var isValid = instance.ExpiryDate >= DateTime.Today;

            if (isValid)
            {
                var accountNumber = instance.AccountNumber.Insecure()
                    .Replace("-", "").Replace(" ", "");

                var digits = accountNumber.ToCharArray();
                Array.Reverse(digits);
                var sum = 0;
                var alt = false;

                foreach (var d in digits)
                {
                    var check = d >= 0 && d <= 9;
                    if (!check)
                    {
                        continue;
                    }

                    var mult = 0;
                    if (alt)
                    {
                        mult = d*2;
                        if (mult > 9)
                        {
                            mult -= 9;
                        }
                    }

                    sum += mult;
                    alt = !alt;
                }

                isValid = sum%10 == 0;
            }

            return isValid;
        }
    }
}