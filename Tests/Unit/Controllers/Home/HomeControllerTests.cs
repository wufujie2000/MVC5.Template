using MvcTemplate.Components.Alerts;
using MvcTemplate.Controllers.Home;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers.Home
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController controller;
        private IAccountService service;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            HttpMock httpMock = new HttpMock();
            HttpContext.Current = httpMock.HttpContext;
            service = Substitute.For<IAccountService>();
            accountId = HttpContext.Current.User.Identity.Name;
            service.AccountExists(accountId).Returns(true);

            controller = new HomeController(service);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpMock.HttpContextBase;
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Method: Index()

        [Test]
        public void Index_RedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.Index() as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        [Test]
        public void Index_ReturnsViewWithNullModel()
        {
            Object model = (controller.Index() as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: Error()

        [Test]
        public void Error_ReturnsViewWithNullModell()
        {
            Object model = (controller.Error() as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: NotFound()

        [Test]
        public void NotFound_ReturnsViewWithNullModel()
        {
            Object model = (controller.NotFound() as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: Unauthorized()

        [Test]
        public void Unauthorized_RedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.Unauthorized() as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        [Test]
        public void Unauthorized_ReturnsViewWithNullModel()
        {
            Object model = (controller.Unauthorized() as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion
    }
}
