using DotNetMerchant.Web.Attributes;

namespace DotNetMerchant.Web.Query
{
    /// <summary>
    /// A query object for posting a single XML entity.
    /// </summary>
    internal class XmlQueryInfo : IWebQueryInfo
    {
        [WebEntity] public string Xml { get; set; }
    }
}
