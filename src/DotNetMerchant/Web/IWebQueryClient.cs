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
using System.IO;
using System.Net;
using System.Security.Permissions;

namespace DotNetMerchant.Web
{
    internal interface IWebQueryClient
    {
        WebResponse Response { get; }
        WebRequest Request { get; }
        ICredentials Credentials { get; set; }

        bool UseCompression { get; set; }
        string ProxyValue { get; set; }

        WebRequest GetWebRequestShim(Uri address);
        WebResponse GetWebResponseShim(WebRequest request, IAsyncResult result);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        void OpenReadAsync(Uri uri);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        void OpenReadAsync(Uri uri, object state);

        event OpenReadCompletedEventHandler OpenReadCompleted;
        WebResponse GetWebResponseShim(WebRequest request);
        Stream OpenRead(string url);
    }
}