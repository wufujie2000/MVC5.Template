using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Security;
using Template.Tests.Objects;
using Tests.Helpers;

namespace Template.Tests.Unit.Controllers
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
            var httpContextMock = new HttpMock();
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
        public void BaseController_RoleProviderIsNull()
        {
            Assert.IsNull(controller.BaseRoleProvider);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action)

        [Test]
        public void RedirectIfAuthorized_RedirectsToDefaultIfNotAuthorized()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Action").Returns(false);

            RouteValueDictionary actual = controller.BaseRedirectIfAuthorized("Action").RouteValues;
            RouteValueDictionary expected = controller.BaseRedirectToDefault().RouteValues;

            Assert.AreEqual(expected["language"], actual["language"]);
            Assert.AreEqual(expected["controller"], actual["controller"]);
            Assert.AreEqual(expected["action"], actual["action"]);
            Assert.AreEqual(expected["area"], actual["area"]);
        }

        [Test]
        public void RedirectIfAuthorized_RedirectsToActionIfAuthorized()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Action").Returns(true);

            RouteValueDictionary actual = controller.BaseRedirectIfAuthorized("Action").RouteValues;
            RouteValueDictionary expected = controller.BaseRedirectToAction("Action").RouteValues;

            Assert.AreEqual(expected["language"], actual["language"]);
            Assert.AreEqual(expected["controller"], actual["controller"]);
            Assert.AreEqual(expected["action"], actual["action"]);
            Assert.AreEqual(expected["area"], actual["area"]);
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
            var expected = controller.BaseRedirectToDefault().RouteValues;
            var actual = (controller.BaseRedirectToLocal("http://www.foo.com") as RedirectToRouteResult).RouteValues;

            controllerMock.Protected().Verify("RedirectToDefault", Times.Once());
            Assert.AreEqual(expected["controller"], actual["controller"]);
            Assert.AreEqual(expected["language"], actual["language"]);
            Assert.AreEqual(expected["action"], actual["action"]);
            Assert.AreEqual(expected["area"], actual["area"]);
        }

        #endregion

        #region Method: RedirectToDefault()

        [Test]
        public void RedirectToDefault_RedirectsToDefault()
        {
            controller.RouteData.Values["language"] = "lt-LT";
            var actual = controller.BaseRedirectToDefault().RouteValues;

            Assert.AreEqual(String.Empty, actual["controller"]);
            Assert.AreEqual(String.Empty, actual["action"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("lt-LT", actual["language"]);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Test]
        public void RedirectsToUnauthorized_RedirectsToUnauthorized()
        {
            controller.RouteData.Values["language"] = "lt-LT";
            var actual = controller.BaseRedirectToUnauthorized().RouteValues;

            Assert.AreEqual("lt-LT", actual["language"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
            Assert.AreEqual("Unauthorized", actual["action"]);
        }

        #endregion

        #region Method: OnAuthorization(AuthorizationContext filterContext)

        [Test]
        public void OnAuthorization_SetsResultToRedirectToUnauthorized()
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

            var expected = controller.BaseRedirectToUnauthorized().RouteValues;
            var actual = (filterContext.Result as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(expected["controller"], actual["controller"]);
            Assert.AreEqual(expected["language"], actual["language"]);
            Assert.AreEqual(expected["action"], actual["action"]);
            Assert.AreEqual(expected["area"], actual["area"]);
        }

        [Test]
        public void OnAuthorization_OnAuthorizedSetsResultToNull()
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
        public void IsAuthorizedFor_OnNullRoleProviderReturnsTrue()
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
        public void IsAuthorizedFor_Overload_OnNullRoleProviderReturnsTrue()
        {
            Assert.IsTrue(controller.BaseIsAuthorizedFor(null, null, null));
        }

        [Test]
        public void IsAuthorizedFor_Overload_ReturnsRoleProviderResult()
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
