using System;
using System.Collections;
using System.Web.Caching;
using System.Web.Hosting;
using DotNetMerchant.Storefront.Configuration;

namespace DotNetMerchant.Storefront.Presentation
{
    public class EmbeddedViewVirtualPathProvider : VirtualPathProvider
    {
        private readonly IEmbeddedViewRegistry _embeddedViewRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedViewVirtualPathProvider"/> class.
        /// </summary>
        /// <param name="dependencyContainer">The dependency container.</param>
        public EmbeddedViewVirtualPathProvider(IDependencyContainer dependencyContainer)
        {
            _embeddedViewRegistry = dependencyContainer.Resolve<IEmbeddedViewRegistry>();
        }

        public override bool FileExists(string virtualPath)
        {
            var isPhysical = base.FileExists(virtualPath);
            return isPhysical || EmbeddedViewExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (base.FileExists(virtualPath))
            {
                // Physical file wins
                return base.GetFile(virtualPath);
            }

            var isEmbedded = EmbeddedViewExists(virtualPath);
            return isEmbedded
                       ? new EmbeddedView(virtualPath, _embeddedViewRegistry)
                       : base.GetFile(virtualPath);
        }

        private bool EmbeddedViewExists(string virtualPath)
        {
            var virtualView = virtualPath.ToLowerInvariant();
            var virtualViewParts = virtualView.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var embeddedView in _embeddedViewRegistry.Values)
            {
                var embeddedViewParts = embeddedView.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                if (embeddedViewParts.Length != virtualViewParts.Length)
                {
                    continue;
                }

                var viewsMatch = true;
                for (var i = 0; i < embeddedViewParts.Length; i++)
                {
                    viewsMatch &= virtualViewParts[i].Equals(embeddedViewParts[i]);
                }

                if (viewsMatch)
                {
                    // Probably want to cache this
                    return true;
                }
            }

            return false;
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies,
                                                           DateTime utcStart)
        {
            return EmbeddedViewExists(virtualPath)
                       ? null
                       : base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
    }
}