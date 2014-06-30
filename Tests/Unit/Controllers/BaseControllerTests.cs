using Moq;
using Moq.Protected;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Security;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private Mock<BaseControllerStub> controllerMock;
        private Mock<IRoleProvider> roleProviderMock;
        private BaseControllerStub baseController;
        private HttpMock httpMock;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            httpMock = new HttpMock();
            controllerMock = new Mock<BaseControllerStub>() { CallBase = true };
            RequestContext requestContext = httpMock.HttpContext.Request.RequestContext;

            accountId = httpMock.HttpContextBase.User.Identity.Name;
            controllerMock.Object.Url = new UrlHelper(requestContext);
            controllerMock.Object.ControllerContext = new ControllerContext();
            controllerMock.Object.ControllerContext.Controller = controllerMock.Object;
            controllerMock.Object.ControllerContext.HttpContext = httpMock.HttpContextBase;
            controllerMock.Object.ControllerContext.RouteData = httpMock.HttpContextBase.Request.RequestContext.RouteData;

            roleProviderMock = new Mock<IRoleProvider>(MockBehavior.Strict);
            baseController = controllerMock.Object;
        }

        [TearDown]
        public void TearDown()
        {
            RoleFactory.Provider = null;
        }

        #region Property: CurrentAccountId

        [Test]
        public void CurrentAccountId_GetsCurrentIdentityName()
        {
            String expected = baseController.ControllerContext.HttpContext.User.Identity.Name;
            String actual = baseController.BaseCurrentAccountId;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: BaseController()

        [Test]
        public void BaseController_SetsRoleProviderFromFactory()
        {
            RoleFactory.Provider = roleProviderMock.Object;
            baseController = new BaseControllerStub();

            IRoleProvider actual = baseController.BaseRoleProvider;
            IRoleProvider expected = RoleFactory.Provider;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseController_CreatessEmptyAlertsContainer()
        {
            CollectionAssert.IsEmpty(baseController.Alerts);
        }

        #endregion

        #region Method: RedirectToLocal(String url)

        [Test]
        public void RedirectToLocal_RedirectsToDefaultIfUrlIsNotLocal()
        {
            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(expected);
            RedirectToRouteResult actual = baseController.BaseRedirectToLocal("http://www.test.com") as RedirectToRouteResult;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RedirectToLocal_RedirectsToLocalIfUrlIsLocal()
        {
            String actual = (baseController.BaseRedirectToLocal("/Home/Index") as RedirectResult).Url;
            String expected = "/Home/Index";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RedirectToDefault()

        [Test]
        public void RedirectToDefault_RedirectsToDefault()
        {
            baseController.RouteData.Values["language"] = "lt-LT";
            RouteValueDictionary actual = baseController.BaseRedirectToDefault().RouteValues;

            Assert.AreEqual(String.Empty, actual["controller"]);
            Assert.AreEqual(String.Empty, actual["action"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("lt-LT", actual["language"]);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Test]
        public void RedirectsToUnauthorized_RedirectsToHomeUnauthorized()
        {
            baseController.RouteData.Values["language"] = "lt-LT";
            RouteValueDictionary actual = baseController.BaseRedirectToUnauthorized().RouteValues;

            Assert.AreEqual("lt-LT", actual["language"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
            Assert.AreEqual("Unauthorized", actual["action"]);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action)

        [Test]
        public void RedirectIfAuthorized_RedirectsToDefaultIfNotAuthorized()
        {
            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(expected);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Action").Returns(false);
            RedirectToRouteResult actual = baseController.BaseRedirectIfAuthorized("Action");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RedirectIfAuthorized_RedirectsToActionIfAuthorized()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Action").Returns(true);

            RouteValueDictionary actual = baseController.BaseRedirectIfAuthorized("Action").RouteValues;
            RouteValueDictionary expected = baseController.BaseRedirectToAction("Action").RouteValues;

            Assert.AreEqual(expected["language"], actual["language"]);
            Assert.AreEqual(expected["controller"], actual["controller"]);
            Assert.AreEqual(expected["action"], actual["action"]);
            Assert.AreEqual(expected["area"], actual["area"]);
        }

        #endregion

        #region Method: IsAuthorizedFor(String action)

        [Test]
        public void IsAuthorizedFor_ReturnsTrueThenAuthorized()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Area", "Controller", "Action").Returns(true);
            baseController.RouteData.Values["controller"] = "Controller";
            baseController.RouteData.Values["area"] = "Area";

            Assert.IsTrue(baseController.BaseIsAuthorizedFor("Action"));
        }

        [Test]
        public void IsAuthorizedFor_ReturnsFalseThenNotAuthorized()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Area", "Controller", "Action").Returns(false);
            baseController.RouteData.Values["controller"] = "Controller";
            baseController.RouteData.Values["area"] = "Area";

            Assert.IsFalse(baseController.BaseIsAuthorizedFor("Action"));
        }

        #endregion

        #region Method: IsAuthorizedFor(String area, String controller, String action)

        [Test]
        public void IsAuthorizedFor_OnNullRoleProviderReturnsTrue()
        {
            baseController.BaseRoleProvider = null;

            Assert.IsTrue(baseController.BaseIsAuthorizedFor(null, null, null));
        }

        [Test]
        public void IsAuthorizedFor_ReturnsRoleProviderResult()
        {
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "AR", "CO", "AC")).Returns(true);
            baseController.BaseRoleProvider = roleProviderMock.Object;

            Assert.IsTrue(baseController.BaseIsAuthorizedFor("AR", "CO", "AC"));
        }

        #endregion

        #region Method: BeginExecuteCore(AsyncCallback callback, Object state)

        [Test]
        public void BeginExecuteCore_SetsThreadCultureFromRequestsRouteValues()
        {
            baseController.RouteData.Values["language"] = "lt-LT";
            baseController.BaseBeginExecuteCore((asyncResult) => { }, null);

            CultureInfo expected = new CultureInfo("lt-LT");

            Assert.AreEqual(expected, CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, CultureInfo.CurrentUICulture);
        }

        #endregion

        #region Method: OnAuthorization(AuthorizationContext filterContext)

        [Test]
        public void OnAuthorization_SetsResultToNullThenNotLoggedIn()
        {
            Mock<ActionDescriptor> describtorMock = new Mock<ActionDescriptor>() { CallBase = true };
            AuthorizationContext filterContext = new AuthorizationContext(baseController.ControllerContext, describtorMock.Object);
            httpMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(false);

            baseController.BaseOnAuthorization(filterContext);

            Assert.IsNull(filterContext.Result);
        }

        [Test]
        public void OnAuthorization_SetsResultToRedirectToUnauthorizedIfNotAuthorized()
        {
            Mock<ActionDescriptor> describtorMock = new Mock<ActionDescriptor>() { CallBase = true };
            AuthorizationContext filterContext = new AuthorizationContext(baseController.ControllerContext, describtorMock.Object);
            httpMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(true);

            String controller = baseController.RouteData.Values["controller"] as String;
            String action = baseController.RouteData.Values["action"] as String;
            String area = baseController.RouteData.Values["area"] as String;

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToUnauthorized").Returns(expected);
            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, area, controller, action)).Returns(false);
            baseController.BaseRoleProvider = roleProviderMock.Object;
            baseController.BaseOnAuthorization(filterContext);

            RedirectToRouteResult actual = filterContext.Result as RedirectToRouteResult;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnAuthorization_SetsResultToNullThenAuthorized()
        {
            Mock<ActionDescriptor> describtorMock = new Mock<ActionDescriptor>() { CallBase = true };
            AuthorizationContext filterContext = new AuthorizationContext(baseController.ControllerContext, describtorMock.Object);
            httpMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(true);

            filterContext.RouteData.Values["controller"] = "Controller";
            filterContext.RouteData.Values["action"] = "Action";
            filterContext.RouteData.Values["area"] = "Area";

            roleProviderMock.Setup(mock => mock.IsAuthorizedFor(accountId, "Area", "Controller", "Action")).Returns(true);
            baseController.BaseRoleProvider = roleProviderMock.Object;
            baseController.BaseOnAuthorization(filterContext);

            Assert.IsNull(filterContext.Result);
        }

        #endregion

        #region Method: OnActionExecuted(ActionExecutedContext context)

        [Test]
        public void OnActionExecuted_SetsAlertsToSessionThenAlertsInSessionAreNull()
        {
            baseController.BaseOnActionExecuted(new ActionExecutedContext());

            AlertsContainer expected = baseController.Alerts;
            Object actual = baseController.Session["Alerts"];

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void OnActionExecuted_MergesAlertsToSession()
        {
            HttpContextBase context = baseController.HttpContext;
            Object expected = baseController.Alerts;
            baseController.Alerts.AddError("First");

            baseController.BaseOnActionExecuted(new ActionExecutedContext());
            AlertsContainer newContainer = new AlertsContainer();
            newContainer.AddError("Second");

            baseController = new BaseControllerStub();
            baseController.ControllerContext = new ControllerContext();
            baseController.ControllerContext.HttpContext = context;

            baseController.BaseOnActionExecuted(new ActionExecutedContext());
            Object actual = baseController.Session["Alerts"];

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void OnActionExecuted_DoesNotMergeSameContainers()
        {
            baseController.Alerts.AddError("First");
            IEnumerable<Alert> expected = baseController.Alerts;

            baseController.BaseOnActionExecuted(new ActionExecutedContext());
            baseController.BaseOnActionExecuted(new ActionExecutedContext());
            IEnumerable<Alert> actual = baseController.Session["Alerts"] as IEnumerable<Alert>;

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
