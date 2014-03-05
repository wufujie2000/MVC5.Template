using NUnit.Framework;
using System.Linq;
using System.Web;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Objects.Components.Datalists;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Datalists
{
    [Ignore]
    [TestFixture]
    public class RolesDatalistTests
    {
        #region Method: GetModels()

        [Test]
        public void GetModels_GetsUnfilteredModels()
        {
            HttpContext.Current = new HttpContextBaseMock().Context;

            CollectionAssert.AreEqual(
                new UnitOfWork().Repository<Role>().Query<RoleView>().Select(role => role.Id),
                new RolesDatalistStub().BaseGetModels().Select(role => role.Id));

            HttpContext.Current = null;
        }

        #endregion
    }
}
