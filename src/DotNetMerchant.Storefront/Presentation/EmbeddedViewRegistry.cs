using System.Collections.Generic;
using System.Reflection;

namespace DotNetMerchant.Storefront.Presentation
{
    public class EmbeddedViewRegistry : Dictionary<Assembly, string>, IEmbeddedViewRegistry
    {

    }
}