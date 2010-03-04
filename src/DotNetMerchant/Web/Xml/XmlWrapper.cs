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

using System.Xml.Linq;

namespace DotNetMerchant.Web.Xml
{
    internal class XmlWrapper
    {
        private readonly XNamespace _namespace;

        public XmlWrapper(XNamespace @namespace)
        {
            _namespace = @namespace;
        }

        public XDeclaration Declare(string version, string encoding, bool standalone)
        {
            return new XDeclaration(version, encoding, standalone ? "yes" : "no");
        }

        public static XDeclaration Declare(string version, string encoding)
        {
            return new XDeclaration(version, encoding, "yes");
        }

        public XElement Tag(string name, string value)
        {
            return new XElement(_namespace + name, value);
        }

        public XElement Tag(string name, params object[] content)
        {
            return new XElement(_namespace + name, content);
        }

        public XAttribute Attribute(string name, object value)
        {
            return new XAttribute(_namespace + name, value);
        }

        public XElement If(bool condition, XElement tag)
        {
            return condition ? tag : null;
        }
    }
}