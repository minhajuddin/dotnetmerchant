#region License

// The MIT License
// 
// Copyright (c) 2009 Conatus Creative, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using DotNetMerchant.Web.Caching;
using DotNetMerchant.Web.Extensions;
using DotNetMerchant.Extensions;
using DotNetMerchant.Web.Attributes;

namespace DotNetMerchant.Web.Query
{
    /// <summary>
    /// A general-purpose web query engine. Supports asynchronous calls, caching,
    /// and dynamic header / parameter generation.
    /// </summary>
    internal abstract class WebQueryBase
    {
        private static readonly object _sync = new object();
        private Uri _request;
        private WebResponse _response;

        protected WebQueryBase(IWebQueryInfo info)
        {
            Info = info;
            Headers = BuildRequestHeaders();
            Parameters = BuildRequestParameters();
            ParseUserAgent();
            ParseWebEntity();
        }
        
        public IWebQueryInfo Info { get; private set; }
        public string UserAgent { get; private set; }

        public IDictionary<string, string> Headers { get; protected set; }
        public WebParameterCollection Parameters { get; protected set; }

        public WebResponse Response
        {
            get
            {
                lock (_sync)
                {
                    return _response;
                }
            }
            set
            {
                lock (_sync)
                {
                    _response = value;
                }
            }
        }

        public Uri RequestUri
        {
            get
            {
                lock (_sync)
                {
                    return _request;
                }
            }
            set
            {
                lock (_sync)
                {
                    _request = value;
                }
            }
        }

        public WebMethod Method { get; set; }
        public string Proxy { get; set; }
        public string AuthorizationHeader { get; protected set; }
        public bool UseCompression { get; set; }

        protected virtual WebRequest BuildPostWebRequest(string url, out byte[] content)
        {
            return Entity == null
                       ? BuildFormPostWebRequest(url, out content)
                       : BuildEntityPostWebRequest(url, out content);
        }

        private WebRequest BuildEntityPostWebRequest(string url, out byte[] content)
        {
            url = AppendParameters(url);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = Entity.ContentType;

            if (!Proxy.IsNullOrBlank())
            {
                request.Proxy = new WebProxy(Proxy);
            }

            if (!UserAgent.IsNullOrBlank())
            {
                request.UserAgent = UserAgent;
            }

            if (UseCompression)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip;
            }

            AppendHeaders(request);

            content = Entity.ContentEncoding.GetBytes(Entity.Content.ToString());
            request.ContentLength = content.Length;
            return request;
        }

        private WebRequest BuildFormPostWebRequest(string url, out byte[] content)
        {
            url = AppendParameters(url);

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!Proxy.IsNullOrBlank())
            {
                request.Proxy = new WebProxy(Proxy);
                SetAuthorizationHeader(request, "Proxy-Authorization");
            }

            if (!UserAgent.IsNullOrBlank())
            {
                request.UserAgent = UserAgent;
            }

            if (UseCompression)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip;
            }

            SetAuthorizationHeader(request, "Authorization");
            AppendHeaders(request);

            content = Encoding.ASCII.GetBytes(url);
            request.ContentLength = content.Length;
            return request;
        }

        protected void AppendHeaders(WebRequest request)
        {
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        protected virtual string AppendParameters(string url)
        {
            var parameters = 0;
            foreach (var parameter in Parameters)
            {
                if (parameter is HttpPostParameter && Method != WebMethod.Post)
                {
                    continue;
                }

                url = DotNetMerchant.Extensions.StringExtensions.Concat(url, parameters > 0 || url.Contains("?") ? "&" : "?");
                url = DotNetMerchant.Extensions.StringExtensions.Concat(url, "{0}={1}".FormatWith(parameter.Name, parameter.Value.UrlEncode()));
                parameters++;
            }

            return url;
        }

        private IDictionary<string, string> BuildRequestHeaders()
        {
            var headers = new Dictionary<string, string>();
            var properties = Info.GetType().GetProperties();

            Info.ParseAttributes<HeaderAttribute>(properties, headers);
            return headers;
        }

        private WebParameterCollection BuildRequestParameters()
        {
            var parameters = new Dictionary<string, string>();
            var properties = Info.GetType().GetProperties();

            Info.ParseAttributes<ParameterAttribute>(properties, parameters);

            ValidateRequiredParameters(properties);
            ValidateSpecificationParameters(properties);
            
            var collection = new WebParameterCollection();
            parameters.ForEach(p => collection.Add(new WebParameter(p.Key, p.Value)));
            
            return collection;
        }

        private void ValidateRequiredParameters(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                if (!property.HasCustomAttribute<RequiredAttribute>(true))
                {
                    // Not required
                    continue;
                }

                if (property.GetValue(Info, null) != null)
                {
                    // Required but has a value
                    continue;
                }

                var message =
                    "Missing a value for '{0}' when it is marked as required."
                        .FormatWith(property.Name);

                throw new ArgumentException(message);
            }
        }

        private void ValidateSpecificationParameters(IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var value = property.GetValue(Info, null);
               
                if (value == null)
                {
                    continue;
                }

                var attributes = property.GetCustomAttributes<SpecificationAttribute>(true);
                foreach(var attribute in attributes)
                {
                    var satisfied = value.Satisfies(attribute.SpecificationType);
                    if (satisfied)
                    {
                        continue;
                    }

                    var message =
                        "The value for '{0}' does not satisfy {1}."
                            .FormatWith(property.Name, attribute.SpecificationType);

                    throw new ArgumentException(message);
                }
            }
        }

        private void ParseUserAgent()
        {
            var properties = Info.GetType().GetProperties();
            var count = 0;
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<UserAgentAttribute>(true);
                count += attributes.Count();
                if (count > 1)
                {
                    throw new ArgumentException("Cannot declare more than one user agent per query");
                }

                if (count < 1)
                {
                    continue;
                }

                if (!UserAgent.IsNullOrBlank())
                {
                    continue;
                }

                var value = property.GetValue(Info, null);
                UserAgent = value != null ? value.ToString() : null;
            }
        }

        private void ParseWebEntity()
        {
            var properties = Info.GetType().GetProperties();
            var count = 0;
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<WebEntityAttribute>(true);
                count += attributes.Count();
                if (count > 1)
                {
                    throw new ArgumentException("Cannot declare more than one entity per query");
                }

                if (count < 1)
                {
                    continue;
                }

                if (Entity != null)
                {
                    continue;
                }

                var value = property.GetValue(Info, null);

                var content = value != null ? value.ToString() : null;
                var contentEncoding = attributes.Single().ContentEncoding;
                var contentType = attributes.Single().ContentType;

                Entity = new WebEntity
                             {
                                 Content = content,
                                 ContentEncoding = contentEncoding,
                                 ContentType = contentType
                             };
            }
        }

        protected WebEntity Entity { get; set; }
        
        protected string HandleWebException(WebException ex)
        {
            if (ex.Response is HttpWebResponse && ex.Response != null)
            {
                Response = ex.Response;

                using (var reader = new StreamReader(Response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();
                    var responseArgs = new WebQueryResponseEventArgs(result);

                    OnQueryResponse(responseArgs);
                    return result;
                }
            }

            throw ex;
        }

        protected IWebQueryClient CreateWebQueryClient()
        {
            var client = CreateWebQueryClient(Headers, Parameters, UserAgent);
            client.ProxyValue = Proxy;
            client.UseCompression = UseCompression;
            return client;
        }

        private static IWebQueryClient CreateWebQueryClient(IDictionary<string, string> headers,
                                                            WebParameterCollection parameters, 
                                                            string userAgent)
        {
            return new WebQueryClient(headers, parameters, userAgent);
        }

        protected abstract void SetAuthorizationHeader(WebRequest request, string header);

        protected virtual void ExecuteGetAsync(string url)
        {
            Response = null;

            var client = CreateWebQueryClient();
            client.OpenReadCompleted += ClientOpenReadCompleted;
            client.OpenReadAsync(new Uri(url));
        }

        protected virtual void PostAsyncRequestCallback(IAsyncResult asyncResult)
        {
            WebRequest request;
            byte[] content;
            Triplet<IClientCache, object, string> store;

            var state = asyncResult.AsyncState as Pair<WebRequest, byte[]>;
            if (state == null)
            {
                // no expiration specified
                if (asyncResult is Pair<WebRequest, Triplet<byte[], IClientCache, string>>)
                {
                    var cacheScheme = (Pair<WebRequest, Triplet<byte[], IClientCache, string>>)asyncResult;
                    var cache = cacheScheme.Second.Second;

                    var url = cacheScheme.First.RequestUri.ToString();
                    var prefix = cacheScheme.Second.Third;
                    var key = CreateCacheKey(prefix, url);

                    var fetch = cache.Get<string>(key);
                    if (fetch != null)
                    {
                        var args = new WebQueryResponseEventArgs(fetch);
                        OnQueryResponse(args);
                        return;
                    }

                    request = cacheScheme.First;
                    content = cacheScheme.Second.First;
                    store = new Triplet<IClientCache, object, string> { First = cache, Second = null, Third = prefix };
                }
                else
                    // absolute expiration specified
                    if (asyncResult is Pair<WebRequest, Pair<byte[], Triplet<IClientCache, DateTime, string>>>)
                    {
                        var cacheScheme =
                            (Pair<WebRequest, Pair<byte[], Triplet<IClientCache, DateTime, string>>>)asyncResult;
                        var url = cacheScheme.First.RequestUri.ToString();
                        var cache = cacheScheme.Second.Second.First;
                        var expiry = cacheScheme.Second.Second.Second;

                        var prefix = cacheScheme.Second.Second.Third;
                        var key = CreateCacheKey(prefix, url);

                        var fetch = cache.Get<string>(key);
                        if (fetch != null)
                        {
                            var args = new WebQueryResponseEventArgs(fetch);
                            OnQueryResponse(args);
                            return;
                        }

                        request = cacheScheme.First;
                        content = cacheScheme.Second.First;
                        store = new Triplet<IClientCache, object, string> { First = cache, Second = expiry, Third = prefix };
                    }
                    else
                        // sliding expiration specified
                        if (asyncResult is Pair<WebRequest, Pair<byte[], Triplet<IClientCache, TimeSpan, string>>>)
                        {
                            var cacheScheme =
                                (Pair<WebRequest, Pair<byte[], Triplet<IClientCache, TimeSpan, string>>>)asyncResult;
                            var url = cacheScheme.First.RequestUri.ToString();
                            var cache = cacheScheme.Second.Second.First;
                            var expiry = cacheScheme.Second.Second.Second;

                            var prefix = cacheScheme.Second.Second.Third;
                            var key = CreateCacheKey(prefix, url);

                            var fetch = cache.Get<string>(key);
                            if (fetch != null)
                            {
                                var args = new WebQueryResponseEventArgs(fetch);
                                OnQueryResponse(args);
                                return;
                            }

                            request = cacheScheme.First;
                            content = cacheScheme.Second.First;
                            store = new Triplet<IClientCache, object, string> { First = cache, Second = expiry, Third = prefix };
                        }
                        else
                        {
                            // unrecognized state signature
                            throw new ArgumentNullException("asyncResult",
                                                            "The asynchronous post failed to return its state");
                        }
            }
            else
            {
                request = state.First;
                content = state.Second;
                store = null;
            }

            // no cached response
            using (var stream = request.EndGetRequestStream(asyncResult))
            {
                stream.Write(content, 0, content.Length);
                stream.Close();

                request.BeginGetResponse(PostAsyncResponseCallback,
                                         new Pair<WebRequest, Triplet<IClientCache, object, string>> { First = request, Second = store });
            }
        }

        protected virtual void PostAsyncResponseCallback(IAsyncResult asyncResult)
        {
            var state = asyncResult.AsyncState as Pair<WebRequest, Triplet<IClientCache, object, string>>;
            if (state == null)
            {
                throw new ArgumentNullException("asyncResult", "The asynchronous post failed to return its state");
            }

            var request = state.First;
            if (request == null)
            {
                throw new ArgumentNullException("asyncResult", "The asynchronous post failed to return a request");
            }

            try
            {
                using (var response = request.EndGetResponse(asyncResult))
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = reader.ReadToEnd();
                        Response = response;

                        if (state.Second != null)
                        {
                            var cache = state.Second.First;
                            var expiry = state.Second.Second;
                            var url = request.RequestUri.ToString();

                            var prefix = state.Second.Third;
                            var key = CreateCacheKey(prefix, url);

                            if (expiry is DateTime)
                            {
                                // absolute
                                cache.Insert(key, result, (DateTime)expiry);
                            }

                            if (expiry is TimeSpan)
                            {
                                // sliding
                                cache.Insert(key, result, (TimeSpan)expiry);
                            }
                        }

                        var args = new WebQueryResponseEventArgs(result);
                        OnQueryResponse(args);
                    }
                }
            }
            catch (WebException ex)
            {
                HandleWebException(ex);
            }
        }

        protected virtual IAsyncResult ExecutePostAsync(string url)
        {
            Response = null;

            byte[] content;
            var request = BuildPostWebRequest(url, out content);

            var state = new Pair<WebRequest, byte[]> { First = request, Second = content };
            var args = new WebQueryRequestEventArgs(url);
            OnQueryRequest(args);

            return request.BeginGetRequestStream(PostAsyncRequestCallback, state);
        }

        protected virtual void ExecutePostAsync(string url, IEnumerable<HttpPostParameter> parameters)
        {
            Response = null;

            byte[] content;
            var request = BuildMultiPartFormRequest(url, parameters, out content);

            var state = new Pair<WebRequest, byte[]> { First = request, Second = content };
            var args = new WebQueryRequestEventArgs(url);
            OnQueryRequest(args);

            request.BeginGetRequestStream(PostAsyncRequestCallback, state);
        }

        // expects real cache key, not prefix
        private void ExecuteGetAsyncAndCache(IClientCache cache, string key, string url, IWebQueryClient client)
        {
            var fetch = cache.Get<string>(key);

            if (fetch != null)
            {
                var args = new WebQueryResponseEventArgs(fetch);
                OnQueryResponse(args);
            }
            else
            {
                var state = new Pair<IClientCache, string>
                {
                    First = cache,
                    Second = key
                };

                client.OpenReadCompleted += ClientOpenReadCompleted;
                client.OpenReadAsync(new Uri(url), state);
            }
        }

        // expects real cache key, not prefix
        private void ExecuteGetAsyncAndCacheWithExpiry(IClientCache cache, string key, string url,
                                                       DateTime absoluteExpiration, IWebQueryClient client)
        {
            var fetch = cache.Get<string>(key);

            if (fetch != null)
            {
                var args = new WebQueryResponseEventArgs(fetch);
                OnQueryResponse(args);
            }
            else
            {
                var state = new Pair<IClientCache, Pair<string, DateTime>>
                {
                    First = cache,
                    Second = new Pair<string, DateTime> { First = key, Second = absoluteExpiration }
                };

                client.OpenReadCompleted += ClientOpenReadCompleted;
                client.OpenReadAsync(new Uri(url), state);
            }
        }

        // expects real cache key, not prefix
        private void ExecuteGetAsyncAndCacheWithExpiry(IClientCache cache, string key, string url,
                                                       TimeSpan slidingExpiration, IWebQueryClient client)
        {
            var fetch = cache.Get<string>(key);

            if (fetch != null)
            {
                var args = new WebQueryResponseEventArgs(fetch);
                OnQueryResponse(args);
            }
            else
            {
                var state = new Pair<IClientCache, Pair<string, TimeSpan>>
                {
                    First = cache,
                    Second = new Pair<string, TimeSpan> { First = key, Second = slidingExpiration }
                };

                client.OpenReadCompleted += ClientOpenReadCompleted;
                client.OpenReadAsync(new Uri(url), state);
            }
        }

        protected virtual void ExecuteGetAsync(string url, string prefixKey, IClientCache cache)
        {
            Response = null;

            var client = CreateWebQueryClient();
            var key = CreateCacheKey(prefixKey, url);

            ThreadPool.QueueUserWorkItem(work => ExecuteGetAsyncAndCache(cache, key, url, client));
        }

        protected virtual void ExecuteGetAsync(string url, string prefixKey, IClientCache cache,
                                               DateTime absoluteExpiration)
        {
            Response = null;

            var client = CreateWebQueryClient();
            var key = CreateCacheKey(prefixKey, url);

            ThreadPool.QueueUserWorkItem(
                work => ExecuteGetAsyncAndCacheWithExpiry(cache, key, url, absoluteExpiration, client));
        }

        protected virtual void ExecuteGetAsync(string url, string prefixKey, IClientCache cache,
                                               TimeSpan slidingExpiration)
        {
            Response = null;

            var client = CreateWebQueryClient();
            var key = CreateCacheKey(prefixKey, url);

            ThreadPool.QueueUserWorkItem(
                work => ExecuteGetAsyncAndCacheWithExpiry(cache, key, url, slidingExpiration, client));
        }

        protected virtual void ClientOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                using (var stream = e.Result)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var result = reader.ReadToEnd();
                        Response = ((IWebQueryClient)sender).Response;

                        if (e.UserState != null)
                        {
                            // no expiration specified
                            if (e.UserState is Pair<IClientCache, string>)
                            {
                                var state = (e.UserState as Pair<IClientCache, string>);
                                state.First.Insert(state.Second, result);
                            }

                            // absolute expiration specified
                            if (e.UserState is Pair<IClientCache, Pair<string, DateTime>>)
                            {
                                var state = e.UserState as Pair<IClientCache, Pair<string, DateTime>>;
                                state.First.Insert(state.Second.First, result, state.Second.Second);
                            }

                            // sliding expiration specified
                            if (e.UserState is Pair<IClientCache, Pair<string, TimeSpan>>)
                            {
                                var state = e.UserState as Pair<IClientCache, Pair<string, TimeSpan>>;
                                state.First.Insert(state.Second.First, result, state.Second.Second);
                            }
                        }

                        // only send query when caching is complete
                        var args = new WebQueryResponseEventArgs(result);
                        OnQueryResponse(args);
                    }
                }
            }
            catch (WebException ex)
            {
                var message = HandleWebException(ex);
                var args = new WebQueryResponseEventArgs(message);

                OnQueryResponse(args);
            }
        }

        protected virtual IAsyncResult ExecutePostAsync(string url, string key, IClientCache cache)
        {
            Response = null;

            byte[] content;
            var request = BuildPostWebRequest(url, out content);

            var state = new Pair<WebRequest, Triplet<byte[], IClientCache, string>>
            {
                First = request,
                Second = new Triplet<byte[], IClientCache, string>
                {
                    First = content,
                    Second = cache,
                    Third = key
                }
            };

            var args = new WebQueryRequestEventArgs(url);
            OnQueryRequest(args);

            return request.BeginGetRequestStream(PostAsyncRequestCallback, state);
        }

        protected virtual IAsyncResult ExecutePostAsync(string url, string prefixKey, IClientCache cache,
                                                        DateTime absoluteExpiration)
        {
            Response = null;

            byte[] content;
            var request = BuildPostWebRequest(url, out content);

            var state = new Pair<WebRequest, Pair<byte[], Triplet<IClientCache, DateTime, string>>>
            {
                First = request,
                Second = new Pair<byte[], Triplet<IClientCache, DateTime, string>>
                {
                    First = content,
                    Second = new Triplet<IClientCache, DateTime, string>
                    {
                        First = cache,
                        Second = absoluteExpiration,
                        Third = prefixKey
                    }
                }
            };

            var args = new WebQueryRequestEventArgs(url);
            OnQueryRequest(args);

            return request.BeginGetRequestStream(PostAsyncRequestCallback, state);
        }

        protected virtual IAsyncResult ExecutePostAsync(string url, string prefixKey, IClientCache cache,
                                                        TimeSpan slidingExpiration)
        {
            Response = null;

            byte[] content;
            var request = BuildPostWebRequest(url, out content);

            var state = new Pair<WebRequest, Pair<byte[], Triplet<IClientCache, TimeSpan, string>>>
            {
                First = request,
                Second = new Pair<byte[], Triplet<IClientCache, TimeSpan, string>>
                {
                    First = content,
                    Second = new Triplet<IClientCache, TimeSpan, string>
                    {
                        First = cache,
                        Second = slidingExpiration,
                        Third = prefixKey
                    }
                }
            };

            var args = new WebQueryRequestEventArgs(url);
            OnQueryRequest(args);

            return request.BeginGetRequestStream(PostAsyncRequestCallback, state);
        }

        private static string CreateCacheKey(string prefix, string url)
        {
            return !prefix.IsNullOrBlank() ? "{0}_{1}".FormatWith(prefix, url) : url;
        }

        protected virtual string ExecuteWithCache(IClientCache cache,
                                                  string url,
                                                  string key,
                                                  Func<IClientCache, string, string> cacheScheme)
        {
            var fetch = cache.Get<string>(CreateCacheKey(key, url));
            if (fetch != null)
            {
                return fetch;
            }

            var result = cacheScheme.Invoke(cache, url);
            return result;
        }

        protected virtual string ExecuteWithCacheAndAbsoluteExpiration(IClientCache cache,
                                                                       string url,
                                                                       string key,
                                                                       DateTime expiry,
                                                                       Func<IClientCache, string, DateTime, string>
                                                                           cacheScheme)
        {
            var fetch = cache.Get<string>(CreateCacheKey(key, url));
            if (fetch != null)
            {
                return fetch;
            }

            var result = cacheScheme.Invoke(cache, url, expiry);
            return result;
        }

        protected virtual string ExecuteWithCacheAndSlidingExpiration(IClientCache cache,
                                                                      string url,
                                                                      string key,
                                                                      TimeSpan expiry,
                                                                      Func<IClientCache, string, TimeSpan, string>
                                                                          cacheScheme)
        {
            var fetch = cache.Get<string>(CreateCacheKey(key, url));
            if (fetch != null)
            {
                return fetch;
            }

            var result = cacheScheme.Invoke(cache, url, expiry);
            return result;
        }

        protected virtual string ExecuteGet(string url, string key, IClientCache cache)
        {
            return ExecuteWithCache(cache, url, key, (c, u) => ExecuteGetAndCache(cache, url, key));
        }

        protected virtual string ExecuteGet(string url, string key, IClientCache cache, DateTime absoluteExpiration)
        {
            return ExecuteWithCacheAndAbsoluteExpiration(cache, url, key, absoluteExpiration,
                                                         (c, u, e) =>
                                                         ExecuteGetAndCacheWithExpiry(cache, url, key,
                                                                                      absoluteExpiration));
        }

        protected virtual string ExecuteGet(string url, string key, IClientCache cache, TimeSpan slidingExpiration)
        {
            return ExecuteWithCacheAndSlidingExpiration(cache, url, key, slidingExpiration,
                                                        (c, u, e) =>
                                                        ExecuteGetAndCacheWithExpiry(cache, url, key, slidingExpiration));
        }

        private string ExecuteGetAndCache(IClientCache cache, string url, string key)
        {
            var result = ExecuteGet(url);
            cache.Insert(CreateCacheKey(key, url), result);
            return result;
        }

        private string ExecuteGetAndCacheWithExpiry(IClientCache cache, string url, string key,
                                                    DateTime absoluteExpiration)
        {
            var result = ExecuteGet(url);
            cache.Insert(CreateCacheKey(key, url), result, absoluteExpiration);
            return result;
        }

        private string ExecuteGetAndCacheWithExpiry(IClientCache cache, string url, string key,
                                                    TimeSpan slidingExpiration)
        {
            var result = ExecuteGet(url);
            cache.Insert(CreateCacheKey(key, url), result, slidingExpiration);
            return result;
        }

        public virtual string ExecutePost(string url, string key, IClientCache cache)
        {
            return ExecuteWithCache(cache, url, key, (c, u) => ExecutePostAndCache(cache, url, key));
        }

        public virtual string ExecutePost(string url, string key, IClientCache cache, DateTime absoluteExpiration)
        {
            return ExecuteWithCacheAndAbsoluteExpiration(cache, url, key, absoluteExpiration,
                                                         (c, u, e) =>
                                                         ExecutePostAndCacheWithExpiry(cache, url, key,
                                                                                       absoluteExpiration));
        }

        public virtual string ExecutePost(string url, string key, IClientCache cache, TimeSpan slidingExpiration)
        {
            return ExecuteWithCacheAndSlidingExpiration(cache, url, key, slidingExpiration,
                                                        (c, u, e) =>
                                                        ExecutePostAndCacheWithExpiry(cache, url, key, slidingExpiration));
        }

        private string ExecutePostAndCache(IClientCache cache, string url, string key)
        {
            var result = ExecutePost(url);
            cache.Insert(CreateCacheKey(key, url), result);
            return result;
        }

        private string ExecutePostAndCacheWithExpiry(IClientCache cache, string url, string key,
                                                     DateTime absoluteExpiration)
        {
            var result = ExecutePost(url);
            cache.Insert(CreateCacheKey(key, url), result, absoluteExpiration);
            return result;
        }

        private string ExecutePostAndCacheWithExpiry(IClientCache cache, string url, string key,
                                                     TimeSpan slidingExpiration)
        {
            var result = ExecutePost(url);
            cache.Insert(CreateCacheKey(key, url), result, slidingExpiration);
            return result;
        }

        protected virtual string ExecuteGet(string url)
        {
            Response = null;

            var client = CreateWebQueryClient();
            var requestArgs = new WebQueryRequestEventArgs(url);
            OnQueryRequest(requestArgs);

            try
            {
                using (var stream = client.OpenRead(url))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                return HandleWebException(ex);
            }
            finally
            {
                Response = client.Response;
            }
        }

        protected virtual string ExecutePost(string url)
        {
            Response = null;

            byte[] content;
            var request = BuildPostWebRequest(url, out content);

            var requestArgs = new WebQueryRequestEventArgs(url);
            OnQueryRequest(requestArgs);

            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(content, 0, content.Length);
                    stream.Close();

                    using (var response = request.GetResponse())
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            var result = reader.ReadToEnd();

                            var responseArgs = new WebQueryResponseEventArgs(result);
                            OnQueryResponse(responseArgs);

                            Response = response;
                            return result;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                return HandleWebException(ex);
            }
        }

        protected virtual string ExecutePost(string url, IEnumerable<HttpPostParameter> parameters)
        {
            Response = null;
            byte[] bytes;
            var request = BuildMultiPartFormRequest(url, parameters, out bytes);

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Flush();

                using (var response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = reader.ReadToEnd();

                        var responseArgs = new WebQueryResponseEventArgs(result);
                        OnQueryResponse(responseArgs);

                        Response = response;
                        return result;
                    }
                }
            }
        }

        protected virtual HttpWebRequest BuildMultiPartFormRequest(string url, IEnumerable<HttpPostParameter> parameters,
                                                                   out byte[] bytes)
        {
            var boundary = Guid.NewGuid().ToString();
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            request.Method = "POST";

            var contents = BuildMultiPartFormRequestParameters(boundary, parameters);
            var payload = contents.ToString();

            bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(payload);
            request.ContentLength = bytes.Length;
            return request;
        }

        protected static StringBuilder BuildMultiPartFormRequestParameters(string boundary,
                                                                           IEnumerable<HttpPostParameter> parameters)
        {
            var header = string.Format("--{0}", boundary);
            var footer = string.Format("--{0}--", boundary);
            var contents = new StringBuilder();

            foreach (var parameter in parameters)
            {
                contents.AppendLine(header);
                switch (parameter.Type)
                {
                    case HttpPostParameterType.File:
                        {
                            var fileBytes = File.ReadAllBytes(parameter.FilePath);
                            const string fileMask = "Content-Disposition: file; name=\"{0}\"; filename=\"{1}\"";
                            var fileHeader = fileMask.FormatWith(parameter.Name, parameter.FileName);
                            var fileData = Encoding.GetEncoding("iso-8859-1").GetString(fileBytes, 0, fileBytes.Length);

                            contents.AppendLine(fileHeader);
                            contents.AppendLine("Content-Type: {0}".FormatWith(parameter.ContentType.ToLower()));
                            contents.AppendLine();
                            contents.AppendLine(fileData);

                            break;
                        }
                    case HttpPostParameterType.Field:
                        {
                            contents.AppendLine("Content-Disposition: form-data; name=\"{0}\"".FormatWith(parameter.Name));
                            contents.AppendLine();
                            contents.AppendLine(parameter.Value);
                            break;
                        }
                }
            }

            contents.AppendLine(footer);
            return contents;
        }

        public virtual void RequestAsync(string url)
        {
            switch (Method)
            {
                case WebMethod.Get:
                    ExecuteGetAsync(url);
                    break;
                case WebMethod.Post:
                    ExecutePostAsync(url);
                    break;
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual void RequestAsync(string url, string key, IClientCache cache)
        {
            switch (Method)
            {
                case WebMethod.Get:
                    ExecuteGetAsync(url, key, cache);
                    break;
                case WebMethod.Post:
                    ExecutePostAsync(url, key, cache);
                    break;
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual void RequestAsync(string url, string key, IClientCache cache, DateTime absoluteExpiration)
        {
            switch (Method)
            {
                case WebMethod.Get:
                    ExecuteGetAsync(url, key, cache, absoluteExpiration);
                    break;
                case WebMethod.Post:
                    ExecutePostAsync(url, key, cache, absoluteExpiration);
                    break;
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual void RequestAsync(string url, string key, IClientCache cache, TimeSpan slidingExpiration)
        {
            switch (Method)
            {
                case WebMethod.Get:
                    ExecuteGetAsync(url, key, cache, slidingExpiration);
                    break;
                case WebMethod.Post:
                    ExecutePostAsync(url, key, cache, slidingExpiration);
                    break;
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual void RequestAsync(string url, IEnumerable<HttpPostParameter> parameters)
        {
            switch (Method)
            {
                case WebMethod.Post:
                    ExecutePostAsync(url, parameters);
                    break;
                default:
                    throw new NotSupportedException("Only HTTP POSTS can use multi-part forms");
            }
        }

        public virtual string Request(string url)
        {
            RequestUri = AppendParameters(url).AsUri();

            switch (Method)
            {
                case WebMethod.Get:
                    return ExecuteGet(url);
                case WebMethod.Post:
                    return ExecutePost(url);
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual string Request(string url, string key, IClientCache cache)
        {
            switch (Method)
            {
                case WebMethod.Get:
                    return ExecuteGet(url, key, cache);
                case WebMethod.Post:
                    return ExecutePost(url, key, cache);
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual string Request(string url, string key, IClientCache cache, DateTime absoluteExpiration)
        {
            switch (Method)
            {
                case WebMethod.Get:
                    return ExecuteGet(url, key, cache, absoluteExpiration);
                case WebMethod.Post:
                    return ExecutePost(url, key, cache, absoluteExpiration);
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual string Request(string url, string key, IClientCache cache, TimeSpan slidingExpiration)
        {
            switch (Method)
            {
                case WebMethod.Get:
                    return ExecuteGet(url, key, cache, slidingExpiration);
                case WebMethod.Post:
                    return ExecutePost(url, key, cache, slidingExpiration);
                case WebMethod.Delete:
                    throw new NotImplementedException("HTTP DELETE not supported yet; use HTTP POST instead");
                default:
                    throw new NotSupportedException("Unknown web method");
            }
        }

        public virtual string Request(string url, IEnumerable<HttpPostParameter> parameters)
        {
            switch (Method)
            {
                case WebMethod.Post:
                    return ExecutePost(url, parameters);
                default:
                    throw new NotSupportedException("Only HTTP POSTS can use post parameters");
            }
        }
        
        public static string QuickGet(string url)
        {
            return QuickGet(url, null, null, null);
        }

        public static string QuickGet(string url, string username, string password)
        {
            return QuickGet(url, null, username, password);
        }

        public static string QuickGet(string url, IDictionary<string, string> headers, string username, string password)
        {
            var client = CreateWebQueryClient(headers, null, null);

            if (!username.IsNullOrBlank() && !password.IsNullOrBlank())
            {
                client.Credentials = new NetworkCredential(username, password);
            }

            try
            {
                using (var stream = client.OpenRead(url))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException)
            {
                return null;
            }
        }

        public virtual void OnQueryResponse(WebQueryResponseEventArgs args)
        {
            if (QueryResponse != null)
            {
                QueryResponse(this, args);
            }
        }

        public virtual event EventHandler<WebQueryRequestEventArgs> QueryRequest;

        public virtual void OnQueryRequest(WebQueryRequestEventArgs args)
        {
            if (QueryRequest != null)
            {
                QueryRequest(this, args);
            }
        }

        public virtual event EventHandler<WebQueryResponseEventArgs> QueryResponse;
    }
}