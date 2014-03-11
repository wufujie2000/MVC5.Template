using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Tests.Objects
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

        [Test]
        public void RoleView_RolePrivilegesAreEmpty()
        {
            CollectionAssert.IsEmpty(new RoleView().RolePrivileges);
        }

        #endregion
    }
}
