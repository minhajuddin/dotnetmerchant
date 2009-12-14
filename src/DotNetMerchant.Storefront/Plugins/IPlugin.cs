using DotNetMerchant.Storefront.Services;

namespace DotNetMerchant.Storefront.Plugins
{
    public interface IPlugin
    {
        void Install(IPluginService service);
    }
}