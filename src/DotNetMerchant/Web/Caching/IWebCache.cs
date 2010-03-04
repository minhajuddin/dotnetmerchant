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

using System;
using System.Web.Caching;

namespace DotNetMerchant.Web.Caching
{
    /// <summary>
    /// A cache that uses ASP.NET's web cache dependency and priority features.
    /// </summary>
    public interface IWebCache : IClientCache
    {
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependency">The dependency.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="removedCallback">The removed callback.</param>
        void Add(string key, object value, CacheDependency dependency, DateTime absoluteExpiration,
                 TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback removedCallback);

        /// <summary>
        /// Inserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependencies">The dependencies.</param>
        void Insert(string key, object value, CacheDependency dependencies);

        /// <summary>
        /// Inserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration);

        /// <summary>
        /// Inserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        void Insert(string key, object value, CacheDependency dependencies, TimeSpan slidingExpiration);

        /// <summary>
        /// Inserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="onRemoveCallback">The on remove callback.</param>
        void Insert(string key, object value, CacheDependency dependencies, TimeSpan slidingExpiration,
                    CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);

        /// <summary>
        /// Inserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="onRemoveCallback">The on remove callback.</param>
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
                    CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);

        /// <summary>
        /// Inserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        /// <param name="onUpdateCallback">The on update callback.</param>
        void Insert(string key, object value, CacheDependency dependencies, TimeSpan slidingExpiration,
                    CacheItemUpdateCallback onUpdateCallback);

        /// <summary>
        /// Inserts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="dependencies">The dependencies.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="onUpdateCallback">The on update callback.</param>
        void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration,
                    CacheItemUpdateCallback onUpdateCallback);

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();
    }
}