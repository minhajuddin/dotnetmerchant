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
using DotNetMerchant.Extensions;
using DotNetMerchant.Payments.Model;
using DotNetMerchant.Payments.Model.Specifications.Cards;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Extensions
{
    [TestFixture]
    public class SpecificationExtensionsTests
    {
        [Test]
        public void Can_satisfy_generic_specification()
        {
            var card = new CreditCard("4111 1111 1111 1111",
                                      "John Q Customer",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);

            var satisfied = card.Satisfies<VisaSpecification>();

            Assert.IsTrue(satisfied);
        }

        [Test]
        public void Can_satisfy_non_generic_specification()
        {
            var specification = new VisaSpecification();

            var card = new CreditCard("4111 1111 1111 1111",
                                      "John Q Customer",
                                      DateTime.Now.Month,
                                      DateTime.Now.Year);

            var satisfied = card.Satisfies(specification);

            Assert.IsTrue(satisfied);
        }
    }
}