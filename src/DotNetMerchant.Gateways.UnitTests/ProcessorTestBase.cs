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
using System.Xml.Linq;

namespace DotNetMerchant.Gateways.UnitTests
{
    public abstract class ProcessorTestBase
    {
        protected ProcessorTestBase(string processorName,
                                    string firstCredential,
                                    string secondCredential,
                                    string thirdCredential)
        {
            LoadProcessor(processorName, 
                          firstCredential, 
                          secondCredential, 
                          thirdCredential);
        }

        public XDocument Document { get; protected set; }
        public string CredentialFirst { get; protected set; }
        public string CredentialSecond { get; protected set; }
        public string CredentialThird { get; protected set; }

        public void LoadProcessor(string processorName,
                                  string firstCredential, 
                                  string secondCredential,
                                  string thirdCredential)
        {
            Document = XDocument.Load("setup.xml");
            var setup = Document.Element("setup");
            string first = null;
            string second = null;
            string third = null;
            
            if (setup != null)
            {
                var processorRoot = setup.Element("processors");
                if (processorRoot != null)
                {
                    var processors = processorRoot.Elements("processor");

                    foreach (var processor in processors)
                    {
                        var name = processor.Attribute("name");
                        if (name == null || !name.Value.ToLower().Equals(processorName.ToLower()))
                        {
                            continue;
                        }

                        var development = processor.Element("development");
                        if (development == null)
                        {
                            continue;
                        }

                        first = GetCredential(development, firstCredential);
                        second = GetCredential(development, secondCredential);
                        third = GetCredential(development, thirdCredential);
                    }
                }
            }

            if (string.IsNullOrEmpty(first) && 
                string.IsNullOrEmpty(second) &&
                string.IsNullOrEmpty(third))
            {
                var message = string.Format("Could not load processor credentials for {0} from setup.xml", processorName);
                throw new Exception(message);
            }

            CredentialFirst = first;
            CredentialSecond = second;
            CredentialThird = third;
        }

        private static string GetCredential(XContainer development, XName element)
        {
            var passwordElement = development.Element(element);
            var result = passwordElement != null
                             ? passwordElement.Value.Trim()
                             : null;
            return result;
        }
    }
}