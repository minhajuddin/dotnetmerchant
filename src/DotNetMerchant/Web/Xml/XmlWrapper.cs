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
