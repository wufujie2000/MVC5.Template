using NUnit.Framework;
using System.Linq;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Datalists;

namespace Template.Tests.Tests.Components.Datalists
{
    [TestFixture]
    public class RolesDatalistTests : HttpContextSetUp
    {
        #region Method: GetModels()

        [Test]
        public void GetModels_GetsUnfilteredModels()
        {
            CollectionAssert.AreEqual(
                new UnitOfWork().Repository<Role>().ProjectTo<RoleView>().Select(role => role.Id),
                new RolesDatalistStub().BaseGetModels().Select(role => role.Id));
        }

        #endregion
    }
}
