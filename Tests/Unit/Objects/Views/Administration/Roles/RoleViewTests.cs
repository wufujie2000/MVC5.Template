using NUnit.Framework;
using Template.Objects;

namespace Template.Tests.Unit.Objects
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
