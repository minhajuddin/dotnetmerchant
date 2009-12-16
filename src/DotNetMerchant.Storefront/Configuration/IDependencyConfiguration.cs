using System.Reflection;

namespace DotNetMerchant.Storefront.Configuration
{
    public interface IDependencyConfiguration
    {
        void Configure(Assembly assembly);
    }
}