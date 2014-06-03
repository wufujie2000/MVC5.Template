using Moq;
using NUnit.Framework;
using Template.Components.Security;

namespace Template.Tests.Unit.Security
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
            IRoleProvider expected = new Mock<IRoleProvider>().Object;
            RoleFactory.Provider = expected;

            IRoleProvider actual = RoleFactory.Provider;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
