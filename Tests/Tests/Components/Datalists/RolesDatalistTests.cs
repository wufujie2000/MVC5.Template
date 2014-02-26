using NUnit.Framework;
using System.Linq;
using System.Web;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Datalists;

namespace Template.Tests.Tests.Components.Datalists
{
    [TestFixture]
    public class RolesDatalistTests
    {
        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = HttpFactory.MockHttpContext();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

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
