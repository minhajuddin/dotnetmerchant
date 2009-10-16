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
using System.IO;
using System.Net;
using System.Text;
using DotNetMerchant.Extensions;
using DotNetMerchant.Web.Extensions;

namespace DotNetMerchant.Web.Query.Basic
{
    /// <summary>
    /// A web query engine for making requests that use basic HTTP authorization.
    /// </summary>
    internal class BasicAuthWebQuery : WebQueryBase
    {
        private readonly string _password;
        private readonly string _username;

        public BasicAuthWebQuery(IWebQueryInfo info, string username, string password) :
            this(info)
        {
            _username = username;
            _password = password;
        }

        public BasicAuthWebQuery(IWebQueryInfo info) :
            base(info)
        {
            
        }

        public bool HasAuth
        {
            get
            {
                return
                    (!_username.IsNullOrBlank()
                     && !String.IsNullOrEmpty(_password));
            }
        }
        
        protected override void SetAuthorizationHeader(WebRequest request, string header)
        {
            if (!HasAuth)
            {
                return;
            }

            // NetworkCredentials always makes two trips, even if with PreAuthenticate
            var credentials = new NetworkCredential(_username, _password)
                .ToAuthorizationHeader();
            AuthorizationHeader = header;

            request.PreAuthenticate = true;
            request.Headers[header] = credentials;
        }

        protected override HttpWebRequest BuildMultiPartFormRequest(string url,
                                                                    IEnumerable<HttpPostParameter> parameters,
                                                                    out byte[] bytes)
        {
            var boundary = Guid.NewGuid().ToString();
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            request.Method = "POST";

            SetAuthorizationHeader(request, "Authorization");

            var contents = BuildMultiPartFormRequestParameters(boundary, parameters);
            var payload = contents.ToString();

            bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(payload);

            request.ContentLength = bytes.Length;
            return request;
        }

        protected override string ExecuteGet(string url)
        {
            Response = null;

            var client = CreateWebQueryClient();
            if (HasAuth)
            {
                client.Credentials = new NetworkCredential(_username, _password);
            }

            var requestArgs = new WebQueryRequestEventArgs(url);
            OnQueryRequest(requestArgs);

            try
            {
                using (var stream = client.OpenRead(url))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var result = reader.ReadToEnd();
                        return result;
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

        protected override void ExecuteGetAsync(string url)
        {
            Response = null;

            var client = CreateWebQueryClient();

            if (HasAuth)
            {
                client.Credentials = new NetworkCredential(_username, _password);
            }

            client.OpenReadCompleted += ClientOpenReadCompleted;
            client.OpenReadAsync(new Uri(url));
        }
    }
}