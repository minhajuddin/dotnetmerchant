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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace DotNetMerchant.Web.Extensions
{
    internal static class StringExtensions
    {
        public static string UrlEncode(this string value)
        {
            // This is more correct than HttpUtility; 
            // it escapes spaces as %20, not +
            return Uri.EscapeDataString(value);
        }

        public static string UrlDecode(this string value)
        {
#if !SILVERLIGHT
            return HttpUtility.UrlDecode(value);
#else
    // todo look into why HttpUtility is a better choice here
            return Uri.UnescapeDataString(value);
#endif
        }

        public static Uri AsUri(this string value)
        {
            return new Uri(value);
        }

        public static bool IsValidUrl(this string value)
        {
            const string pattern =
                "(([a-zA-Z][0-9a-zA-Z+\\-\\.]*:)?/{0,2}[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?(#[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?";
            return value.Matches(pattern) && value.IsPrefixedByOneOf("http://", "https://", "ftp://");
        }

        internal static bool IsPrefixedByOneOf(this string value, params string[] prefixes)
        {
            return value.IsPrefixedByOneOf(prefixes.ToList());
        }

        internal static bool IsPrefixedByOneOf(this string value, IEnumerable<string> prefixes)
        {
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;

            foreach (var prefix in prefixes)
            {
                if (compareInfo.IsPrefix(value, prefix, CompareOptions.IgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        internal static string EnsurePrefixIsOneOf(this string value, params string[] prefixes)
        {
            return value.EnsurePrefixIsOneOf(prefixes.ToList());
        }

        internal static string EnsurePrefixIsOneOf(this string value, IEnumerable<string> prefixes)
        {
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var prefixed = false;
            foreach (var prefix in prefixes)
            {
                if (compareInfo.IsPrefix(value, prefix, CompareOptions.IgnoreCase))
                {
                    prefixed = true;
                }
            }

            if (!prefixed)
            {
                value = String.Concat(prefixes.First(), value);
            }
            return value;
        }

        public static string RemoveRange(this string input, int startIndex, int endIndex)
        {
            return input.Remove(startIndex, endIndex - startIndex);
        }

        public static bool TryReplace(this string input, string oldValue, string newValue, out string output)
        {
            var value = input.Replace(oldValue, newValue);
            output = value;

            return !output.Equals(input);
        }

        public static Guid AsGuid(this string input)
        {
            return new Guid(input);
        }

        public static string ToBase64String(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }

        public static byte[] GetBytes(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string PercentEncode(this string input)
        {
            var bytes = input.GetBytes();
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(string.Format("%{0:X}", b));
            }
            return sb.ToString();
        }

        public static T TryConvert<T>(this object instance)
        {
            var converted = default(T);
            try
            {
                if (instance != null)
                {
                    converted = (T) Convert.ChangeType(instance, typeof (T), CultureInfo.InvariantCulture);
                }
            }
            catch (InvalidCastException)
            {
                // Bad cast
            }
            catch (FormatException)
            {
                // Illegal value for the type i.e. "13" != bool
            }
            return converted;
        }
    }
}