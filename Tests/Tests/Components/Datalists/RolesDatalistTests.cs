using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Datalists;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Datalists
{
    [TestFixture]
    public class RolesDatalistTests
    {
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpMock().HttpContext;
            context = new TestingContext();

            context.Set<Role>().RemoveRange(context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();

            for (Int32 i = 0; i < 100; i++)
                context.Set<Role>().Add(ObjectFactory.CreateRole(i));

            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Method: GetModels()

        [Test]
        public void GetModels_GetsAllModels()
        {
            var expected = context.Set<Role>().Select(role => role.Id);
            var actual = new RolesDatalistStub().BaseGetModels().Select(role => role.Id);

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
