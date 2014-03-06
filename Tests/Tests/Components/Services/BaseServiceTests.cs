using Moq;
using NUnit.Framework;
using System.Web.Routing;
using Template.Components.Services;
using Template.Data.Core;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class BaseServiceTests
    {
        private RouteValueDictionary routeValues;
        private BaseService service;

        [SetUp]
        public void SetUp()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var mock = new Mock<BaseService>(unitOfWorkMock.Object) { CallBase = true };

            service = mock.Object;
            service.HttpContext = new HttpContextBaseMock().HttpContextBase;
            routeValues = service.HttpContext.Request.RequestContext.RouteData.Values;
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
        }

        #region Property: CurrentAccountId

        [Test]
        public void CurrentAccountId_IsCurrent()
        {
            Assert.AreEqual(service.HttpContext.User.Identity.Name, service.CurrentAccountId);
        }

        #endregion

        #region Property: CurrentLanguage

        [Test]
        public void CurrentLanguage_IsCurrent()
        {
            Assert.AreEqual(routeValues["language"] = "L", service.CurrentLanguage);
        }

        #endregion

        #region Property: CurrentArea

        [Test]
        public void CurrentArea_IsCurrent()
        {
            Assert.AreEqual(routeValues["area"] = "A", service.CurrentArea);
        }

        #endregion

        #region Property: CurrentController

        [Test]
        public void CurrentController_IsCurrent()
        {
            ;
            Assert.AreEqual(routeValues["controller"] = "C", service.CurrentController);
        }

        #endregion

        #region Property: CurrentAction

        [Test]
        public void CurrentAction_IsCurrent()
        {
            Assert.AreEqual(routeValues["action"] = "A", service.CurrentAction);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanDisposeMultipleTimes()
        {
            service.Dispose();
            service.Dispose();
        }

        #endregion
    }
}
