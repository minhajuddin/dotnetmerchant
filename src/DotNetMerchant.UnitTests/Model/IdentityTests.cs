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
using DotNetMerchant.Model;
using NUnit.Framework;

namespace DotNetMerchant.UnitTests.Model
{
    [TestFixture]
    public class IdentityTests
    {
        [Test]
        public void Can_compare_no_identity_to_empty_identities()
        {
            var guid = Guid.Empty;
            const int number = -1;

            var equalByGuid = Identity.None.Equals(guid);
            var equalByInt = Identity.None.Equals(number);

            Assert.IsTrue(equalByGuid);
            Assert.IsTrue(equalByInt);

            equalByInt = number.Equals(Identity.None);
            equalByGuid = guid.Equals(Identity.None);

            Assert.IsTrue(equalByGuid);
            Assert.IsTrue(equalByInt);
        }

        [Test]
        public void Can_compare_identity_to_guid()
        {
            var id = Guid.NewGuid();

            Identity identity = id;

            Assert.AreEqual(id, (Guid)identity);
        }

        [Test]
        public void Can_compare_identity_to_int()
        {
            const int id = 123456;

            Identity identity = 123456;

            Assert.AreEqual(id, identity);
        }

        [Test]
        public void Can_compare_identity_to_long()
        {
            const long id = 123456;

            Identity identity = (long)123456;

            Assert.AreEqual(id, identity);
        }
    }
}