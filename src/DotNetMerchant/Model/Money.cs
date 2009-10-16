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
using System.Text;
using DotNetMerchant.Extensions;

namespace DotNetMerchant.Model
{
    /// <summary>
    /// An abstraction of money for e-commerce operations.
    /// Money is immutable and coupled to the <see cref="CurrencyInfo" /> it belongs
    /// to at all times. In most cases, the framework will attempt to determine
    /// the correct <see cref="CurrencyInfo" /> on its own based on the culture of
    /// the thread viewing the money, unless an explicit currency is provided.
    /// </summary>
    [Serializable]
    public partial struct Money : IComparable<Money>,
                                  IEquatable<Money>,
                                  IFormattable
    {
        private readonly DateTime _createdDate;
        private readonly CurrencyInfo _currencyInfo;
        private double? _override;
        private int _places;
        private long _units;

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct.
        /// </summary>
        /// <param name="units">The units.</param>
        public Money(long units) : this(CultureInfo.CurrentCulture, units)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Money(double value) : this(CultureInfo.CurrentCulture, value)
        {
        }

        private Money(CurrencyInfo currencyInfo) : this()
        {
            _createdDate = DateTime.UtcNow;
            _currencyInfo = currencyInfo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct.
        /// </summary>
        /// <param name="currencyInfo">The currency info.</param>
        /// <param name="units">The units.</param>
        public Money(CurrencyInfo currencyInfo, long units) : this(currencyInfo)
        {
            _units = units;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct.
        /// </summary>
        /// <param name="currencyInfo">The currency info.</param>
        /// <param name="value">The value.</param>
        public Money(CurrencyInfo currencyInfo, double value) : this(currencyInfo)
        {
            _units = ScaleUp(value);
        }

        /// <summary>
        /// The time this <see cref="Money" /> instance was created.
        /// </summary>
        public DateTime CreatedDate
        {
            get { return _createdDate; }
        }

        /// <summary>
        /// Gets the currency info.
        /// </summary>
        /// <value>The currency info.</value>
        public CurrencyInfo CurrencyInfo
        {
            get { return _currencyInfo; }
        }

        #region IComparable<Money> Members

        public int CompareTo(Money other)
        {
            return other._units.CompareTo(_units);
        }

        #endregion

        #region IEquatable<Money> Members

        public bool Equals(Money other)
        {
            return other == this;
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            var value = ScaleDown();

            return value.ToString(format, formatProvider);
        }

        #endregion

        /// <summary>
        /// Compares equality between this instance and a value.
        /// </summary>
        /// <param name="other">The other value.</param>
        /// <returns></returns>
        public bool Equals(double other)
        {
            return other == ScaleDown();
        }

        /// <summary>
        /// Compares equality between this instance and a value.
        /// </summary>
        /// <param name="other">The other value.</param>
        /// <returns></returns>
        public bool Equals(long other)
        {
            return other == ScaleUp(ScaleDown());
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. 
        ///                 </param><filterpriority>2</filterpriority>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return other.GetType() == typeof (Money) &&
                   Equals((Money) other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _units.GetHashCode();
        }

        private double ScaleDown()
        {
            if (_override.HasValue)
            {
                return _override.Value;
            }

            var numberFormat = _currencyInfo.DisplayCulture.NumberFormat;

            var scalingFactor = Math.Pow(10, _places);

            var scaled = _units/scalingFactor;

            var rounded = Math.Round(scaled, numberFormat.CurrencyDecimalDigits);

            return rounded;
        }

        private long ScaleUp(double value)
        {
            if (double.IsInfinity(value))
            {
                _override = value;
                return long.MaxValue;
            }

            var places = value.CountDecimalPlaces();

            var scalingFactor = Math.Pow(10, places);

            var scaled = Convert.ToInt64(value*scalingFactor);

            _places = places;

            return scaled;
        }

        private static void EnsureSameCurrency(Money left, Money right)
        {
            if (left._currencyInfo != right._currencyInfo)
            {
                throw new ArithmeticException("The currency of both arguments must match to perform this operation.");
            }
        }

        private static void HarmonizeDecimalPlaces(ref Money left, ref Money right)
        {
            var scaleFactor = Math.Abs(right._places - left._places);

            if (right._places > left._places)
            {
                left._places += scaleFactor;

                left._units *= (long) Math.Pow(10, scaleFactor);
            }

            if (right._places < left._places)
            {
                right._places += scaleFactor;

                right._units *= (long) Math.Pow(10, scaleFactor);
            }
        }

        public override string ToString()
        {
            return ToString("c", _currencyInfo.DisplayCulture.NumberFormat);
        }

        /// <summary>
        /// Displays the current instance as it would appear in a specified culture.
        /// </summary>
        /// <param name="displayCulture">The display culture.</param>
        /// <returns></returns>
        public string DisplayIn(CultureInfo displayCulture)
        {
            return DisplayIn(displayCulture, true);
        }

        /// <summary>
        /// Displays the value of this instance in a non-native culture, preserving
        /// the characteristics of the native <see cref="CurrencyInfo" /> but respecting 
        /// target cultural formatting.
        /// </summary>
        /// <param name="displayCulture">The culture to display this money in</param>
        /// <param name="disambiguateMatchingSymbol">If <code>true</code>, if the native culture uses the same currency symbol as the display culture, the ISO currency code is appended to the value to help differentiate the native currency.</param>
        /// <returns>A value representing this instance in another culture</returns>
        public string DisplayIn(CultureInfo displayCulture, bool disambiguateMatchingSymbol)
        {
            var sb = new StringBuilder();

            var nativeCulture = CurrencyInfo.DisplayCulture;
            if (displayCulture == nativeCulture)
            {
                return nativeCulture.ToString();
            }

            var nativeNumberFormat = nativeCulture.NumberFormat;
            nativeNumberFormat = (NumberFormatInfo) nativeNumberFormat.Clone();

            var displayNumberFormat = displayCulture.NumberFormat;
            nativeNumberFormat.CurrencyGroupSeparator = displayNumberFormat.CurrencyGroupSeparator;
            nativeNumberFormat.CurrencyDecimalSeparator = displayNumberFormat.CurrencyDecimalSeparator;

            sb.Append(ToString("c", nativeNumberFormat));

            // If the currency symbol of the display culture matches this money, add the code
            if (nativeNumberFormat.CurrencySymbol.Equals(displayNumberFormat.CurrencySymbol))
            {
                var currencyCode = new RegionInfo(nativeCulture.LCID).ISOCurrencySymbol;
                sb.Append(" ").Append(currencyCode);
            }

            return sb.ToString();
        }
    }
}