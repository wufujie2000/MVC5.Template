using MvcTemplate.Components.Security;
using NSubstitute;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Security
{
    [TestFixture]
    public class RoleFactoryTests
    {
        [TearDown]
        public void TearDown()
        {
            RoleFactory.Provider = null;
        }

        #region Static property: Provider

        [Test]
        public void Provider_GetsAndSetsProviderInstance()
        {
            IRoleProvider expected = Substitute.For<IRoleProvider>();
            RoleFactory.Provider = expected;

            IRoleProvider actual = RoleFactory.Provider;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
