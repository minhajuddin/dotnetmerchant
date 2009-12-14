using System.IO;
using System.Reflection;
using System.Web.Hosting;
using DotNetMerchant.Storefront.Extensions;

namespace DotNetMerchant.Storefront.Presentation
{
    public class EmbeddedView : VirtualFile
    {
        private readonly string _virtualPath;
        private readonly IEmbeddedViewRegistry _registry;

        public EmbeddedView(string virtualPath, IEmbeddedViewRegistry registry)
            : base(virtualPath)
        {
            _virtualPath = virtualPath;
            _registry = registry;
        }

        public override Stream Open()
        {
            Assembly assembly;

            var virtualPath = _virtualPath.ToLowerInvariant().Substring(1);

            lock (_registry)
            {
                _registry.TryGetKey(virtualPath, out assembly);
            }

            // Should there be another cache layer?

            Stream resourceStream = null;

            if (assembly != null)
            {
                var assemblyName = assembly.GetName().Name;

                // Example: MvcKits.Core.Views.Shared.ComponentMenu.ascx
                virtualPath = assemblyName + _virtualPath.Replace("/", ".");

                resourceStream = assembly.GetManifestResourceStream(virtualPath);
            }

            if (resourceStream != null)
            {
                return resourceStream;
            }

            throw new FileNotFoundException(string.Format("Embedded view '{0}' not found.", _virtualPath));
        }
    }
}