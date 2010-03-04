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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using DotNetMerchant.Extensions;
using DotNetMerchant.Web.Attributes;
using DotNetMerchant.Web.Query;

namespace DotNetMerchant.Web.Extensions
{
    internal static class WebExtensions
    {
        public static void ParseAttributes<T>(this IWebQueryInfo info,
                                              IEnumerable<PropertyInfo> properties,
                                              IDictionary<string, string> collection)
            where T : Attribute, INamedAttribute
        {
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<T>(true);

                foreach (var attribute in attributes)
                {
                    var value = property.GetValue(info, null);

                    if (value == null)
                    {
                        continue;
                    }

                    if (property.HasCustomAttribute<BooleanToIntegerAttribute>(true))
                    {
                        value = ((bool) value) ? "1" : "0";
                    }

                    var dateFormatAttribute = property
                        .GetCustomAttributes<DateTimeFormatAttribute>(true)
                        .SingleOrDefault();

                    if (dateFormatAttribute != null)
                    {
                        value = ((DateTime) value).ToString(dateFormatAttribute.Format);
                    }

                    var header = value.ToString();

                    if (!header.IsNullOrBlank())
                    {
                        collection.Add(attribute.Name, header);
                    }
                }
            }
        }

        public static string ToAuthorizationHeader(this NetworkCredential credentials)
        {
            var token = "{0}:{1}".FormatWith(credentials.UserName, credentials.Password).GetBytes().ToBase64String();
            return "Basic {0}".FormatWith(token);
        }
    }
}