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
