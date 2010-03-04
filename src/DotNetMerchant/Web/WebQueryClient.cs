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
using System.Collections.Generic;
using System.Net;
using DotNetMerchant.Extensions;
using DotNetMerchant.Web.Extensions;
using StringExtensions = DotNetMerchant.Extensions.StringExtensions;

namespace DotNetMerchant.Web
{
    /// <summary>
    /// A web query instance that tracks its <see cref="WebRequest"/> 
    /// and <see cref="WebResponse" /> pair in order to inform a consumer.
    /// </summary>
    internal class WebQueryClient : WebClient, IWebQueryClient
    {
        private readonly IDictionary<string, string> _headers;

        public WebQueryClient(IDictionary<string, string> headers, WebParameterCollection parameters, string userAgent)
        {
            _headers = headers;
            Parameters = parameters;
            UserAgent = userAgent;
        }

        public WebParameterCollection Parameters { get; private set; }
        public string UserAgent { get; private set; }

        #region IWebQueryClient Members

        public WebResponse Response { get; private set; }
        public WebRequest Request { get; private set; }

        public bool UseCompression { get; set; }
        public string ProxyValue { get; set; }

        public WebRequest GetWebRequestShim(Uri address)
        {
            return GetWebRequest(address);
        }

        public WebResponse GetWebResponseShim(WebRequest request)
        {
            return GetWebResponse(request);
        }

        public WebResponse GetWebResponseShim(WebRequest request, IAsyncResult result)
        {
            return GetWebResponse(request, result);
        }

        #endregion

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest) base.GetWebRequest(address);
            if (_headers != null)
            {
                foreach (var header in _headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            if (UseCompression)
            {
                request.AutomaticDecompression = DecompressionMethods.GZip;
            }

            if (Credentials != null)
            {
                var credentials = (NetworkCredential) Credentials;
                request.Headers["Authorization"] = credentials.ToAuthorizationHeader();
                request.PreAuthenticate = true;
            }

            if (Parameters != null)
            {
                var hasParameters = address.Query.Contains("?");
                foreach (var parameter in Parameters)
                {
                    StringExtensions.Concat(address.Query, hasParameters ? "&" : "?");
                    StringExtensions.Concat(address.Query, "{0}={1}".FormatWith(parameter.Name, parameter.Value));
                    hasParameters = true;
                }
            }

            if (!ProxyValue.IsNullOrBlank())
            {
                request.Proxy = new WebProxy(ProxyValue);
                if (Credentials != null)
                {
                    var credentials = (NetworkCredential) Credentials;
                    request.Headers["Proxy-Authorization"] = credentials.ToAuthorizationHeader();
                }
            }

            if (!UserAgent.IsNullOrBlank())
            {
                request.UserAgent = UserAgent;
            }

            Request = request;
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            try
            {
                var response = base.GetWebResponse(request);
                Response = response;
                return response;
            }
            catch (WebException ex)
            {
                return HandleWebException(ex);
            }
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            try
            {
                var response = base.GetWebResponse(request, result);
                Response = response;
                return response;
            }
            catch (WebException ex)
            {
                return HandleWebException(ex);
            }
        }

        private WebResponse HandleWebException(WebException ex)
        {
            if (ex.Response != null && ex.Response is HttpWebResponse)
            {
                Response = ex.Response;
                return ex.Response;
            }

            throw ex;
        }
    }
}