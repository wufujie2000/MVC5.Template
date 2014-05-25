using Moq;
using NUnit.Framework;
using Template.Components.Security;

namespace Template.Tests.Unit.Security
{
    [TestFixture]
    public class RoleProviderFactoryTests
    {
        [TearDown]
        public void TearDown()
        {
            RoleProviderFactory.Instance = null;
        }

        #region Static property: Instance

        [Test]
        public void Instance_GetsAndSetsInstance()
        {
            IRoleProvider expected = new Mock<IRoleProvider>().Object;
            RoleProviderFactory.Instance = expected;

            IRoleProvider actual = RoleProviderFactory.Instance;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
