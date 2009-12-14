using System.Collections.Generic;
using System.Reflection;

namespace DotNetMerchant.Storefront.Presentation
{
    public interface IEmbeddedViewRegistry : IDictionary<Assembly, string>
    {
        
    }
}