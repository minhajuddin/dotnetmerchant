#region License

// DotNetMerchant
// (http://dotnetmerchant.org)
// Copyright (c) 2010 Conatus Creative Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;

namespace DotNetMerchant.Model
{
    /// <summary>
    /// This struct represents a database row key for a model object. 
    /// When loading entities by identity, pass in a <see cref="Guid"/>, <see cref="int" />, or <see cref="long" />,
    /// depending on your own persistence configuration.
    /// </summary>
    public struct Identity : IComparable<ValueType>
    {
        static Identity()
        {
            None = new Identity();
        }

        private Identity(ValueType key)
            : this()
        {
            Key = key;
        }

        private ValueType Key { get; set; }

        /// <summary>
        /// Gets the value equal to no identity.
        /// </summary>
        public static Identity None { get; private set; }

        #region IComparable<ValueType> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(ValueType other)
        {
            if (other is long)
            {
                return ((long) this).CompareTo(other);
            }

            if (other is int)
            {
                return ((int) this).CompareTo(other);
            }

            if (other is Guid)
            {
                return ((Guid) this).CompareTo(other);
            }

            return -1;
        }

        #endregion

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj.GetType() == typeof (Identity) && Equals((Identity) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Identity left, Identity right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Identity left, Identity right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Compares equality between this instance and a value.
        /// </summary>
        /// <param name="other">The other value.</param>
        /// <returns></returns>
        public bool Equals(Identity other)
        {
            if (other.Key == null && Key == null)
            {
                return true;
            }

            if (other.Key == null && Key != null)
            {
                return false;
            }

            var internalKey = Key;

            if (internalKey == null)
            {
                if (other.Key != null)
                {
                    var type = other.Key.GetType();
                    if (type.FullName.Equals("System.Guid"))
                    {
                        internalKey = Guid.Empty;
                    }
                    else
                    {
                        // long, int
                        internalKey = -1;
                    }
                }
            }

            return Equals(other.Key, internalKey);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Guid"/> to <see cref="DotNetMerchant.Model.Identity"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Identity(Guid value)
        {
            return new Identity(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="DotNetMerchant.Model.Identity"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Identity(int value)
        {
            return new Identity(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int64"/> to <see cref="DotNetMerchant.Model.Identity"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Identity(long value)
        {
            return new Identity(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DotNetMerchant.Model.Identity"/> to <see cref="System.Guid"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Guid(Identity value)
        {
            if (value.Key == null)
            {
                return Guid.Empty;
            }

            return (Guid) value.Key;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DotNetMerchant.Model.Identity"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(Identity value)
        {
            if (value.Key == null)
            {
                return -1;
            }

            return (int) value.Key;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DotNetMerchant.Model.Identity"/> to <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator long(Identity value)
        {
            if (value.Key == null)
            {
                return -1;
            }

            return (long) value.Key;
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing a fully qualified type name.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            if (this == None)
            {
                return "Identity.None";
            }

            return Key == null ? base.ToString() : Key.ToString();
        }
    }
}