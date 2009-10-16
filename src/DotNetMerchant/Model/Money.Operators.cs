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
using System.Globalization;

namespace DotNetMerchant.Model
{
    partial struct Money
    {
        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            return left._units <= right._units;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            return left._units >= right._units;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            return left._units > right._units;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            return left._units < right._units;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Money left, Money right)
        {
            return left._units == right._units &&
                   left._places == right._places &&
                   left._currencyInfo == right._currencyInfo;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Money left, Money right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator +(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            HarmonizeDecimalPlaces(ref left, ref right);

            left._units += right._units;

            return left;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator -(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            HarmonizeDecimalPlaces(ref left, ref right);

            left._units -= right._units;

            return left;
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator *(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            HarmonizeDecimalPlaces(ref left, ref right);

            var product = Convert.ToDouble(left._units) * Convert.ToDouble(right._units);

            var factor = Math.Pow(10, left._places * 2);

            product /= factor;

            var result = new Money(left._currencyInfo, product);

            return result;
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Money operator /(Money left, Money right)
        {
            EnsureSameCurrency(left, right);

            HarmonizeDecimalPlaces(ref left, ref right);

            var quotient = Convert.ToDouble(left._units)/Convert.ToDouble(right._units);

            var result = new Money(left._currencyInfo, quotient);

            return result;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="DotNetMerchant.Model.Money"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Money(long value)
        {
            return new Money(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double"/> to <see cref="DotNetMerchant.Model.Money"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Money(double value)
        {
            return new Money(CultureInfo.CurrentCulture, value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DotNetMerchant.Model.Money"/> to <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator long(Money value)
        {
            return (long) value.ScaleDown();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DotNetMerchant.Model.Money"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator double(Money value)
        {
            return value.ScaleDown();
        }
    }
}