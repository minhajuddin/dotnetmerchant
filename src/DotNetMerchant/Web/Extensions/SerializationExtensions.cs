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

using System.Xml.Linq;
using System.Xml.Serialization;

namespace DotNetMerchant.Web.Extensions
{
    internal static class SerializationExtensions
    {
        public static XDocument ToXml<T>(this T instance)
        {
            var document = new XDocument();
            var serializer = new XmlSerializer(typeof (T));
            
            using(var writer = document.CreateWriter())
            {
                serializer.Serialize(writer, instance);
                writer.Flush();
                return document;
            }
        }

        public static XDocument ToXml<T>(this T instance, params XNamespace[] namespaces)
        {
            var document = new XDocument();
            var serializer = new XmlSerializer(typeof(T));

            using (var writer = document.CreateWriter())
            {
                serializer.Serialize(writer, instance);
                writer.Flush();
                return document;
            }
        }

        public static T FromXml<T>(this XDocument source) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var result = default(T);

            if (source.Root != null)
            {
                using (var reader = source.Root.CreateReader())
                {
                    result = serializer.Deserialize(reader) as T;
                }
            }

            return result;
        }
    }
}