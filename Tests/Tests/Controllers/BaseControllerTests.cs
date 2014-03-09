using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Tests.Objects.Controllers;
using Tests.Helpers;

namespace Template.Tests.Tests.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private Mock<BaseControllerStub> controllerMock;
        private Mock<IRoleProvider> roleProviderMock;
        private BaseControllerStub controller;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            var httpContextMock = new HttpContextBaseMock();
            controllerMock = new Mock<BaseControllerStub>() { CallBase = true };
            var requestContext = httpContextMock.HttpContext.Request.RequestContext;

            controllerMock.Object.Url = new UrlHelper(requestContext);
            controllerMock.Object.ControllerContext = new ControllerContext();
            controllerMock.Object.ControllerContext.HttpContext = httpContextMock.HttpContextBase;
            httpContextMock.IdentityMock.Setup(mock => mock.Name).Returns("TestAccountId");

            roleProviderMock = new Mock<IRoleProvider>();
            controller = controllerMock.Object;
            accountId = "TestAccountId";
        }

        #region Constructor: BaseController()

        [Test]
        public void BaseController_HasNullRoleProvider()
        {
            Assert.IsNull(controller.BaseRoleProvider);
        }

        #endregion

        #region Method: RedirectToLocal(String url)

        [Test]
        public void RedirectToLocal_RedirectsToLocal()
        {
            var result = controller.BaseRedirectToLocal("/");
            var redirectResult = result as RedirectResult;

            Assert.AreEqual(typeof(RedirectResult), result.GetType());
            Assert.AreEqual("/", redirectResult.Url);
        }

        [Test]
        public void RedirectToLocal_RedirectsToDefault()
        {
            var expected = controller.BaseRedirectToDefault();
            var actual = controller.BaseRedirectToLocal("http://www.foo.com") as RedirectToRouteResult;

            Assert.AreEqual(expected.RouteValues["language"], actual.RouteValues["language"]);
            Assert.AreEqual(expected.RouteValues["controller"], actual.RouteValues["controller"]);
            Assert.AreEqual(expected.RouteValues["action"], actual.RouteValues["action"]);
            Assert.AreEqual(expected.RouteValues["area"], actual.RouteValues["area"]);
            controllerMock.Protected().Verify("RedirectToDefault", Times.Once());
        }

        #endregion

        #region Method: RedirectToDefault()

        [Test]
        public void RedirectToDefault_RedirectsToDefault()
        {
            controller.RouteData.Values["language"] = "lt-LT";
            var result = controller.BaseRedirectToDefault();

            Assert.AreEqual(controller.RouteData.Values["language"], result.RouteValues["language"]);
            Assert.AreEqual(String.Empty, result.RouteValues["controller"]);
            Assert.AreEqual(String.Empty, result.RouteValues["action"]);
            Assert.AreEqual(String.Empty, result.RouteValues["area"]);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Test]
        public void RedirectsToUnauthorized_RedirectsToUnauthorized()
        {
            controller.RouteData.Values["language"] = "lt-LT";
            var result = controller.BaseRedirectToUnauthorized();

            Assert.AreEqual("lt-LT", result.RouteValues["language"]);
            Assert.AreEqual(String.Empty, result.RouteValues["area"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Unauthorized", result.RouteValues["action"]);
        }

        #endregion

        #region Method: OnAuthorization(AuthorizationContext filterContext)

        [Test]
        public void OnAuthorization_IfNotAuthorizedRedirectsToUnauthorized()
        {
            var actionDescriptorMock = new Mock<ActionDescriptor>() { CallBase = true };
            var filterContext = new AuthorizationContext(controller.ControllerContext, actionDescriptorMock.Object);
            filterContext.RouteData.Values["controller"] = "CO";
            filterContext.RouteData.Values["language"] = "LT";
            filterContext.RouteData.Values["action"] = "AC";
            filterContext.RouteData.Values["area"] = "AR";

            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "AC")).Returns(false);
            controller.BaseRoleProvider = roleProviderMock.Object;
            controller.BaseOnAuthorization(filterContext);

            var expected = controller.BaseRedirectToUnauthorized();
            var actual = filterContext.Result as RedirectToRouteResult;

            Assert.AreEqual(expected.RouteValues["controller"], actual.RouteValues["controller"]);
            Assert.AreEqual(expected.RouteValues["language"], actual.RouteValues["language"]);
            Assert.AreEqual(expected.RouteValues["action"], actual.RouteValues["action"]);
            Assert.AreEqual(expected.RouteValues["area"], actual.RouteValues["area"]);
        }

        [Test]
        public void OnAuthorization_IfAuthorizedHasNullResult()
        {
            var actionDescriptorMock = new Mock<ActionDescriptor>() { CallBase = true };
            var filterContext = new AuthorizationContext(controller.ControllerContext, actionDescriptorMock.Object);
            filterContext.RouteData.Values["controller"] = "CO";
            filterContext.RouteData.Values["action"] = "AC";
            filterContext.RouteData.Values["area"] = "AR";

            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "AC")).Returns(true);
            controller.BaseRoleProvider = roleProviderMock.Object;
            controller.BaseOnAuthorization(filterContext);

            Assert.IsNull(filterContext.Result);
        }

        #endregion

        #region Method: IsAuthorizedFor(String action)

        [Test]
        public void IsAuthorizedFor_IfRoleProviderNullReturnsTrue()
        {
            Assert.IsTrue(controller.BaseIsAuthorizedFor(null));
        }

        [Test]
        public void IsAuthorizedFor_ReturnsRoleProviderResult()
        {
            var actionDescriptorMock = new Mock<ActionDescriptor>() { CallBase = true };
            var filterContext = new AuthorizationContext(controller.ControllerContext, actionDescriptorMock.Object);

            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "AC")).Returns(true);
            controller.BaseRoleProvider = roleProviderMock.Object;
            controller.RouteData.Values["controller"] = "CO";
            controller.RouteData.Values["area"] = "AR";

            Assert.IsTrue(controller.BaseIsAuthorizedFor("AC"));
            roleProviderMock.Verify(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "AC"), Times.Once());
        }

        #endregion

        #region Method: IsAuthorizedFor(String area, String controller, String action)

        [Test]
        public void IsAuthorizedForOverload_IfRoleProviderNullReturnsTrue()
        {
            Assert.IsTrue(controller.BaseIsAuthorizedFor(null, null, null));
        }

        [Test]
        public void IsAuthorizedForOverload_ReturnsRoleProviderResult()
        {
            var actionDescriptorMock = new Mock<ActionDescriptor>() { CallBase = true };
            var filterContext = new AuthorizationContext(controller.ControllerContext, actionDescriptorMock.Object);

            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "AC")).Returns(true);
            controller.BaseRoleProvider = roleProviderMock.Object;

            Assert.IsTrue(controller.BaseIsAuthorizedFor("AR", "CO", "AC"));
            roleProviderMock.Verify(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "AC"), Times.Once());
        }

        #endregion
    }
}
