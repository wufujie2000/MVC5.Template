using MvcTemplate.Controllers;
using MvcTemplate.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController controller;
        private IAccountService service;

        [SetUp]
        public void SetUp()
        {
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            service = Substitute.For<IAccountService>();

            controller = new HomeController(service);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;
            service.AccountExists(httpContext.User.Identity.Name).Returns(true);
        }

        #region Method: Index()

        [Test]
        public void Index_RedirectsToLogoutIfAccountDoesNotExist()
        {
            String currentAccountId = controller.HttpContext.User.Identity.Name;
            service.AccountExists(currentAccountId).Returns(false);

            RedirectToRouteResult actual = controller.Index() as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual(2, actual.RouteValues.Count);
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
            String currentAccountId = controller.HttpContext.User.Identity.Name;
            service.AccountExists(currentAccountId).Returns(false);

            RedirectToRouteResult actual = controller.Unauthorized() as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual(2, actual.RouteValues.Count);
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
