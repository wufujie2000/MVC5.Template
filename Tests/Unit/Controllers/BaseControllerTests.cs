using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
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
        private BaseControllerStub baseController;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            HttpMock httpMock = new HttpMock();
            RoleFactory.Provider = Substitute.For<IRoleProvider>();
            accountId = httpMock.HttpContextBase.User.Identity.Name;
            baseController = Substitute.ForPartsOf<BaseControllerStub>();
            baseController.Url = new UrlHelper(httpMock.HttpContext.Request.RequestContext);
            baseController.ControllerContext = new ControllerContext(
                httpMock.HttpContextBase,
                httpMock.HttpContextBase.Request.RequestContext.RouteData,
                baseController);
        }

        [TearDown]
        public void TearDown()
        {
            LocalizationManager.Provider = null;
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
            baseController.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            baseController.When(control => control.RedirectToDefault()).DoNotCallBase();

            RedirectToRouteResult actual = baseController.RedirectToLocal("http://www.test.com") as RedirectToRouteResult;
            RedirectToRouteResult expected = baseController.RedirectToDefault();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RedirectToLocal_RedirectsToLocalIfUrlIsLocal()
        {
            String actual = (baseController.RedirectToLocal("/Home/Index") as RedirectResult).Url;
            String expected = "/Home/Index";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RedirectToDefault()

        [Test]
        public void RedirectToDefault_RedirectsToDefault()
        {
            baseController.RouteData.Values["language"] = "lt";
            RouteValueDictionary actual = baseController.RedirectToDefault().RouteValues;

            Assert.AreEqual(String.Empty, actual["controller"]);
            Assert.AreEqual(String.Empty, actual["action"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("lt", actual["language"]);
        }

        #endregion

        #region Method: RedirectToNotFound()

        [Test]
        public void RedirectToNotFound_RedirectsToNotFound()
        {
            baseController.RouteData.Values["language"] = "lt";
            RouteValueDictionary actual = baseController.RedirectToNotFound().RouteValues;

            Assert.AreEqual("lt", actual["language"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
            Assert.AreEqual("NotFound", actual["action"]);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Test]
        public void RedirectsToUnauthorized_RedirectsToUnauthorized()
        {
            baseController.RouteData.Values["language"] = "lt";
            RouteValueDictionary actual = baseController.RedirectToUnauthorized().RouteValues;

            Assert.AreEqual("lt", actual["language"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
            Assert.AreEqual("Unauthorized", actual["action"]);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action)

        [Test]
        public void RedirectIfAuthorized_RedirectsToDefaultIfNotAuthorized()
        {
            baseController.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            baseController.When(control => control.RedirectToDefault()).DoNotCallBase();
            baseController.IsAuthorizedFor("Action").Returns(false);

            RedirectToRouteResult actual = baseController.RedirectIfAuthorized("Action");
            RedirectToRouteResult expected = baseController.RedirectToDefault();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void RedirectIfAuthorized_RedirectsToActionIfAuthorized()
        {
            baseController.IsAuthorizedFor("Action").Returns(true);

            RouteValueDictionary expected = baseController.BaseRedirectToAction("Action").RouteValues;
            RouteValueDictionary actual = baseController.RedirectIfAuthorized("Action").RouteValues;

            Assert.AreEqual(expected["controller"], actual["controller"]);
            Assert.AreEqual(expected["language"], actual["language"]);
            Assert.AreEqual(expected["action"], actual["action"]);
            Assert.AreEqual(expected["area"], actual["area"]);
        }

        #endregion

        #region Method: IsAuthorizedFor(String action)

        [Test]
        public void IsAuthorizedFor_ReturnsTrueThenAuthorized()
        {
            baseController.IsAuthorizedFor("Area", "Controller", "Action").Returns(true);
            baseController.RouteData.Values["controller"] = "Controller";
            baseController.RouteData.Values["area"] = "Area";

            Assert.IsTrue(baseController.IsAuthorizedFor("Action"));
        }

        [Test]
        public void IsAuthorizedFor_ReturnsFalseThenNotAuthorized()
        {
            baseController.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);
            baseController.RouteData.Values["controller"] = "Controller";
            baseController.RouteData.Values["area"] = "Area";

            Assert.IsFalse(baseController.IsAuthorizedFor("Action"));
        }

        #endregion

        #region Method: IsAuthorizedFor(String area, String controller, String action)

        [Test]
        public void IsAuthorizedFor_OnNullRoleProviderReturnsTrue()
        {
            baseController.BaseRoleProvider = null;

            Assert.IsTrue(baseController.IsAuthorizedFor(null, null, null));
        }

        [Test]
        public void IsAuthorizedFor_ReturnsRoleProviderResult()
        {
            RoleFactory.Provider.IsAuthorizedFor(accountId, "AR", "CO", "AC").Returns(true);

            Assert.IsTrue(baseController.IsAuthorizedFor("AR", "CO", "AC"));
        }

        #endregion

        #region Method: BeginExecuteCore(AsyncCallback callback, Object state)

        [Test]
        public void BeginExecuteCore_SetsThreadCultureFromRequestsRouteValues()
        {
            LocalizationManager.Provider = new LanguageProviderMock().Provider;

            baseController.RouteData.Values["language"] = "lt";
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
            ActionDescriptor describtor = Substitute.ForPartsOf<ActionDescriptor>();
            AuthorizationContext filterContext = new AuthorizationContext(baseController.ControllerContext, describtor);
            baseController.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(false);

            baseController.BaseOnAuthorization(filterContext);

            Assert.IsNull(filterContext.Result);
        }

        [Test]
        public void OnAuthorization_SetsResultToRedirectToUnauthorizedIfNotAuthorized()
        {
            ActionDescriptor describtor = Substitute.ForPartsOf<ActionDescriptor>();
            AuthorizationContext filterContext = new AuthorizationContext(baseController.ControllerContext, describtor);
            baseController.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);

            String controller = baseController.RouteData.Values["controller"] as String;
            String action = baseController.RouteData.Values["action"] as String;
            String area = baseController.RouteData.Values["area"] as String;

            baseController.RedirectToUnauthorized().Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            RoleFactory.Provider.IsAuthorizedFor(accountId, area, controller, action).Returns(false);
            baseController.When(control => control.RedirectToUnauthorized()).DoNotCallBase();

            baseController.BaseOnAuthorization(filterContext);

            RedirectToRouteResult actual = filterContext.Result as RedirectToRouteResult;
            RedirectToRouteResult expected = baseController.RedirectToUnauthorized();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnAuthorization_SetsResultToNullThenAuthorized()
        {
            ActionDescriptor describtor = Substitute.ForPartsOf<ActionDescriptor>();
            AuthorizationContext filterContext = new AuthorizationContext(baseController.ControllerContext, describtor);
            baseController.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);

            filterContext.RouteData.Values["controller"] = "Controller";
            filterContext.RouteData.Values["action"] = "Action";
            filterContext.RouteData.Values["area"] = "Area";

            RoleFactory.Provider.IsAuthorizedFor(accountId, "Area", "Controller", "Action").Returns(true);
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

        #region Method: NotEmptyView(Object model)

        [Test]
        public void NotEmptyView_RedirectsToNotFoundIfModelIsNull()
        {
            baseController.RedirectToNotFound().Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            baseController.When(control => control.RedirectToNotFound()).DoNotCallBase();

            RedirectToRouteResult expected = baseController.RedirectToNotFound();
            ActionResult actual = baseController.NotEmptyView(null);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void NotEmptyView_ReturnsViewResultIfModelIsNotNull()
        {
            Object expected = new Object();
            ViewResult actual = baseController.NotEmptyView(expected) as ViewResult;

            Assert.AreSame(expected, actual.Model);
        }

        #endregion
    }
}
