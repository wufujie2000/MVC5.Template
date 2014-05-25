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
            RoleProviderFactory.SetInstance(null);
        }

        #region Static method: SetInstance(IRoleProvider instance)

        [Test]
        public void SetInstance_SetsInstance()
        {
            IRoleProvider expected = new Mock<IRoleProvider>().Object;
            RoleProviderFactory.SetInstance(expected);

            IRoleProvider actual = RoleProviderFactory.Instance;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
