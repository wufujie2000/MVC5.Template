using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
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
            baseController.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            baseController.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult actual = baseController.RedirectToLocal("http://www.test.com");
            ActionResult expected = baseController.RedirectToDefault();

            Assert.AreSame(expected, actual);
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
            RouteValueDictionary actual = baseController.RedirectToDefault().RouteValues;

            Assert.AreEqual(baseController.RouteData.Values["language"], actual["language"]);
            Assert.AreEqual(String.Empty, actual["controller"]);
            Assert.AreEqual(String.Empty, actual["action"]);
            Assert.AreEqual(String.Empty, actual["area"]);
        }

        #endregion

        #region Method: RedirectToNotFound()

        [Test]
        public void RedirectToNotFound_RedirectsToNotFound()
        {
            RouteValueDictionary actual = baseController.RedirectToNotFound().RouteValues;

            Assert.AreEqual(baseController.RouteData.Values["language"], actual["language"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
            Assert.AreEqual("NotFound", actual["action"]);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Test]
        public void RedirectsToUnauthorized_RedirectsToUnauthorized()
        {
            RouteValueDictionary actual = baseController.RedirectToUnauthorized().RouteValues;

            Assert.AreEqual(baseController.RouteData.Values["language"], actual["language"]);
            Assert.AreEqual("Unauthorized", actual["action"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action)

        [Test]
        public void RedirectIfAuthorized_RedirectsToDefaultIfNotAuthorized()
        {
            baseController.IsAuthorizedFor("Action").Returns(false);
            baseController.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            baseController.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

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
            baseController.When(sub => sub.RedirectToUnauthorized()).DoNotCallBase();
            baseController.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            baseController.RedirectToUnauthorized().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            String area = baseController.RouteData.Values["area"] as String;
            String action = baseController.RouteData.Values["action"] as String;
            String controller = baseController.RouteData.Values["controller"] as String;
            RoleFactory.Provider.IsAuthorizedFor(accountId, area, controller, action).Returns(false);
            AuthorizationContext context = new AuthorizationContext(baseController.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());

            baseController.BaseOnAuthorization(context);

            ActionResult expected = baseController.RedirectToUnauthorized();
            ActionResult actual = context.Result;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnAuthorization_SetsResultToNullThenAuthorized()
        {
            AuthorizationContext context = new AuthorizationContext(baseController.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());
            RoleFactory.Provider.IsAuthorizedFor(accountId, "Area", "Controller", "Action").Returns(true);
            baseController.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            context.RouteData.Values["controller"] = "Controller";
            context.RouteData.Values["action"] = "Action";
            context.RouteData.Values["area"] = "Area";

            baseController.BaseOnAuthorization(context);

            Assert.IsNull(context.Result);
        }

        #endregion

        #region Method: OnActionExecuted(ActionExecutedContext context)

        [Test]
        public void OnActionExecuted_SetsAlertsToSessionThenAlertsInSessionAreNull()
        {
            baseController.Session["Alerts"] = null;
            baseController.BaseOnActionExecuted(new ActionExecutedContext());

            Object actual = baseController.Session["Alerts"];
            Object expected = baseController.Alerts;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void OnActionExecuted_MergesAlertsToSession()
        {
            HttpContextBase context = baseController.HttpContext;

            BaseControllerStub firstController = new BaseControllerStub();
            firstController.ControllerContext = new ControllerContext();
            firstController.ControllerContext.HttpContext = context;
            firstController.Alerts.AddError("ErrorTest1");

            BaseControllerStub secondController = new BaseControllerStub();
            secondController.ControllerContext = new ControllerContext();
            secondController.ControllerContext.HttpContext = context;
            secondController.Alerts.AddError("ErrorTest2");

            firstController.BaseOnActionExecuted(new ActionExecutedContext());
            secondController.BaseOnActionExecuted(new ActionExecutedContext());

            AlertsContainer actual = baseController.Session["Alerts"] as AlertsContainer;
            AlertsContainer expected = new AlertsContainer();
            expected.AddError("ErrorTest1");
            expected.AddError("ErrorTest2");

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void OnActionExecuted_DoesNotMergeSameContainers()
        {
            baseController.Alerts.AddError("ErrorTest");
            baseController.BaseOnActionExecuted(new ActionExecutedContext());
            baseController.BaseOnActionExecuted(new ActionExecutedContext());

            AlertsContainer actual = baseController.Session["Alerts"] as AlertsContainer;
            AlertsContainer expected = new AlertsContainer();
            expected.AddError("ErrorTest");

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: NotEmptyView(Object model)

        [Test]
        public void NotEmptyView_RedirectsToNotFoundIfModelIsNull()
        {
            baseController.When(sub => sub.RedirectToNotFound()).DoNotCallBase();
            baseController.RedirectToNotFound().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = baseController.RedirectToNotFound();
            ActionResult actual = baseController.NotEmptyView(null);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void NotEmptyView_ReturnsViewResultIfModelIsNotNull()
        {
            Object expected = new Object();
            Object actual = (baseController.NotEmptyView(expected) as ViewResult).Model;

            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
