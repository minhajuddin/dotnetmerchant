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

        public static Identity None { get; private set; }

        public Identity Id
        {
            get { return this; }
        }

        #region IComparable<ValueType> Members

        public int CompareTo(ValueType other)
        {
            if(other is long)
            {
                return ((long) this).CompareTo(other);
            }

            if(other is int)
            {
                return ((int) this).CompareTo(other);
            }

            if(other is Guid)
            {
                return ((Guid) this).CompareTo(other);
            }

            return -1;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj.GetType() == typeof (Identity) && Equals((Identity) obj);
        }

        public override int GetHashCode()
        {
            return (Key != null ? Key.GetHashCode() : 0);
        }

        public static bool operator ==(Identity left, Identity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identity left, Identity right)
        {
            return !left.Equals(right);
        }

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

        public static implicit operator Identity(Guid value)
        {
            return new Identity(value);
        }

        public static implicit operator Identity(int value)
        {
            return new Identity(value);
        }

        public static implicit operator Identity(long value)
        {
            return new Identity(value);
        }

        public static implicit operator Guid(Identity value)
        {
            if (value.Key == null)
            {
                return Guid.Empty;
            }

            return (Guid) value.Key;
        }

        public static implicit operator int(Identity value)
        {
            if (value.Key == null)
            {
                return -1;
            }

            return (int) value.Key;
        }

        public static implicit operator long(Identity value)
        {
            if (value.Key == null)
            {
                return -1;
            }

            return (long) value.Key;
        }

        public override string ToString()
        {
            if(this == None)
            {
                return "Identity.None";
            }

            return Key == null ? base.ToString() : Key.ToString();
        }
    }
}