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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace DotNetMerchant.Web
{
    public class WebParameterCollection : IList<WebParameter>
    {
        private IList<WebParameter> _parameters;

        public WebParameterCollection(IEnumerable<WebParameter> parameters)
        {
            _parameters = new List<WebParameter>(parameters);
        }

        public WebParameterCollection(NameValueCollection collection) : this()
        {
            AddCollection(collection);
        }

        public WebParameterCollection(IDictionary<string, string> collection)
            : this()
        {
            AddCollection(collection);
        }

        public WebParameterCollection()
        {
            _parameters = new List<WebParameter>(0);
        }

        public IEnumerable<string> Names
        {
            get { return _parameters.Select(p => p.Name); }
        }

        public IEnumerable<string> Values
        {
            get { return _parameters.Select(p => p.Value); }
        }

        #region IList<WebParameter> Members

        public IEnumerator<WebParameter> GetEnumerator()
        {
            return _parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(WebParameter parameter)
        {
            _parameters.Add(parameter);
        }

        public void Clear()
        {
            _parameters.Clear();
        }

        public bool Contains(WebParameter parameter)
        {
            return _parameters.Contains(parameter);
        }

        public void CopyTo(WebParameter[] parameters, int arrayIndex)
        {
            _parameters.CopyTo(parameters, arrayIndex);
        }

        public bool Remove(WebParameter parameter)
        {
            return _parameters.Remove(parameter);
        }

        public int Count
        {
            get { return _parameters.Count; }
        }

        public bool IsReadOnly
        {
            get { return _parameters.IsReadOnly; }
        }

        public int IndexOf(WebParameter parameter)
        {
            return _parameters.IndexOf(parameter);
        }

        public void Insert(int index, WebParameter parameter)
        {
            _parameters.Insert(index, parameter);
        }

        public void RemoveAt(int index)
        {
            _parameters.RemoveAt(index);
        }

        public WebParameter this[int index]
        {
            get { return _parameters[index]; }
            set { _parameters[index] = value; }
        }

        #endregion

        private void AddCollection(NameValueCollection collection)
        {
            foreach (var key in collection.AllKeys)
            {
                var parameter = new WebParameter(key, collection[key]);
                _parameters.Add(parameter);
            }
        }

        public void AddRange(NameValueCollection collection)
        {
            AddCollection(collection);
        }

        private void AddCollection(IDictionary<string, string> collection)
        {
            foreach (var key in collection.Keys)
            {
                var parameter = new WebParameter(key, collection[key]);
                _parameters.Add(parameter);
            }
        }

        public void AddRange(IDictionary<string, string> collection)
        {
            AddCollection(collection);
        }

        public void Sort(Comparison<WebParameter> comparison)
        {
            var sorted = new List<WebParameter>(_parameters);
            sorted.Sort(comparison);
            _parameters = sorted;
        }

        public bool RemoveAll(IEnumerable<WebParameter> parameters)
        {
            var success = true;
            var array = parameters.ToArray();
            for (var p = 0; p < array.Length; p++)
            {
                var parameter = array[p];
                success &= _parameters.Remove(parameter);
            }
            return success && array.Length > 0;
        }

        public void Add(string name, string value)
        {
            _parameters.Add(new WebParameter(name, value));
        }
    }
}