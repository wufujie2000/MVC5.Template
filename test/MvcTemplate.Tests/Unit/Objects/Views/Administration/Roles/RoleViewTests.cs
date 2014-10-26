using MvcTemplate.Objects;
using NUnit.Framework;

namespace MvcTemplate.Tests.Unit.Objects
{
    [TestFixture]
    public class RoleViewTests
    {
        #region Constructor: RoleView()

        [Test]
        public void RoleView_PrivilegesTreeIsNotNull()
        {
            Assert.IsNotNull(new RoleView().PrivilegesTree);
        }

        #endregion
    }
}
