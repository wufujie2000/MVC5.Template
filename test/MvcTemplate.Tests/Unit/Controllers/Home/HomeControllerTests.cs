using MvcTemplate.Controllers;
using MvcTemplate.Services;
using NSubstitute;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class HomeControllerTests : ControllerTests
    {
        private HomeController controller;
        private IAccountService service;

        public HomeControllerTests()
        {
            service = Substitute.For<IAccountService>();
            controller = Substitute.ForPartsOf<HomeController>(service);

            ReturnCurrentAccountId(controller, "Test");
        }

        #region Method: Index()

        [Fact]
        public void Index_NotActive_RedirectsToLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToRouteResult actual = controller.Index() as RedirectToRouteResult;

            Assert.Equal("Auth", actual.RouteValues["controller"]);
            Assert.Equal("Logout", actual.RouteValues["action"]);
            Assert.Equal(2, actual.RouteValues.Count);
        }

        [Fact]
        public void Index_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            ViewResult actual = controller.Index() as ViewResult;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Error()

        [Fact]
        public void Error_ReturnsEmptyView()
        {
            ViewResult actual = controller.Error() as ViewResult;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: NotFound()

        [Fact]
        public void NotFound_ReturnsEmptyView()
        {
            ViewResult actual = controller.NotFound() as ViewResult;

            Assert.Null(actual.Model);
        }

        #endregion

        #region Method: Unauthorized()

        [Fact]
        public void Unauthorized_NotActive_RedirectsToLogout()
        {
            service.IsActive(controller.CurrentAccountId).Returns(false);

            RedirectToRouteResult actual = controller.Unauthorized() as RedirectToRouteResult;

            Assert.Equal("Auth", actual.RouteValues["controller"]);
            Assert.Equal("Logout", actual.RouteValues["action"]);
            Assert.Equal(2, actual.RouteValues.Count);
        }

        [Fact]
        public void Unauthorized_ReturnsEmptyView()
        {
            service.IsActive(controller.CurrentAccountId).Returns(true);

            ViewResult actual = controller.Unauthorized() as ViewResult;

            Assert.Null(actual.Model);
        }

        #endregion
    }
}
