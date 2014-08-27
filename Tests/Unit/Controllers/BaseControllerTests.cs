using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Components.Security;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class BaseControllerTests
    {
        private BaseControllerProxy controller;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            HttpMock httpMock = new HttpMock();
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            accountId = httpMock.HttpContextBase.User.Identity.Name;
            controller = Substitute.ForPartsOf<BaseControllerProxy>();
            controller.Url = new UrlHelper(httpMock.HttpContext.Request.RequestContext);
            controller.ControllerContext = new ControllerContext(
                httpMock.HttpContextBase,
                httpMock.HttpContextBase.Request.RequestContext.RouteData,
                controller);
        }

        [TearDown]
        public void TearDown()
        {
            GlobalizationManager.Provider = null;
            Authorization.Provider = null;
        }

        #region Property: CurrentAccountId

        [Test]
        public void CurrentAccountId_GetsCurrentIdentityName()
        {
            String expected = controller.ControllerContext.HttpContext.User.Identity.Name;
            String actual = controller.BaseCurrentAccountId;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Constructor: BaseController()

        [Test]
        public void BaseController_SetsAuthorizationProviderFromFactory()
        {
            IAuthorizationProvider actual = controller.BaseAuthorizationProvider;
            IAuthorizationProvider expected = Authorization.Provider;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BaseController_CreatesEmptyAlertsContainer()
        {
            CollectionAssert.IsEmpty(controller.Alerts);
        }

        #endregion

        #region Method: RedirectToLocal(String url)

        [Test]
        public void RedirectToLocal_RedirectsToDefaultIfUrlIsNotLocal()
        {
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult actual = controller.RedirectToLocal("http://www.test.com");
            ActionResult expected = controller.RedirectToDefault();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void RedirectToLocal_RedirectsToLocalIfUrlIsLocal()
        {
            String actual = (controller.RedirectToLocal("/Home/Index") as RedirectResult).Url;
            String expected = "/Home/Index";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: RedirectToDefault()

        [Test]
        public void RedirectToDefault_RedirectsToDefault()
        {
            RouteValueDictionary actual = controller.RedirectToDefault().RouteValues;

            Assert.AreEqual(String.Empty, actual["controller"]);
            Assert.AreEqual(String.Empty, actual["action"]);
            Assert.AreEqual(String.Empty, actual["area"]);
        }

        #endregion

        #region Method: RedirectToNotFound()

        [Test]
        public void RedirectToNotFound_RedirectsToNotFound()
        {
            RouteValueDictionary actual = controller.RedirectToNotFound().RouteValues;

            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
            Assert.AreEqual("NotFound", actual["action"]);
        }

        #endregion

        #region Method: RedirectToUnauthorized()

        [Test]
        public void RedirectsToUnauthorized_RedirectsToUnauthorized()
        {
            RouteValueDictionary actual = controller.RedirectToUnauthorized().RouteValues;

            Assert.AreEqual("Unauthorized", actual["action"]);
            Assert.AreEqual(String.Empty, actual["area"]);
            Assert.AreEqual("Home", actual["controller"]);
        }

        #endregion

        #region Method: RedirectIfAuthorized(String action)

        [Test]
        public void RedirectIfAuthorized_RedirectsToDefaultIfNotAuthorized()
        {
            controller.IsAuthorizedFor("Action").Returns(false);
            controller.When(sub => sub.RedirectToDefault()).DoNotCallBase();
            controller.RedirectToDefault().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            RedirectToRouteResult actual = controller.RedirectIfAuthorized("Action");
            RedirectToRouteResult expected = controller.RedirectToDefault();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void RedirectIfAuthorized_RedirectsToActionIfAuthorized()
        {
            controller.IsAuthorizedFor("Action").Returns(true);

            RouteValueDictionary expected = controller.BaseRedirectToAction("Action").RouteValues;
            RouteValueDictionary actual = controller.RedirectIfAuthorized("Action").RouteValues;

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
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(true);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.IsTrue(controller.IsAuthorizedFor("Action"));
        }

        [Test]
        public void IsAuthorizedFor_ReturnsFalseThenNotAuthorized()
        {
            controller.IsAuthorizedFor("Area", "Controller", "Action").Returns(false);
            controller.RouteData.Values["controller"] = "Controller";
            controller.RouteData.Values["area"] = "Area";

            Assert.IsFalse(controller.IsAuthorizedFor("Action"));
        }

        #endregion

        #region Method: IsAuthorizedFor(String area, String controller, String action)

        [Test]
        public void IsAuthorizedFor_OnNullAuthorizationProviderReturnsTrue()
        {
            controller.BaseAuthorizationProvider = null;

            Assert.IsTrue(controller.IsAuthorizedFor(null, null, null));
        }

        [Test]
        public void IsAuthorizedFor_ReturnsAuthorizationProviderResult()
        {
            Authorization.Provider.IsAuthorizedFor(accountId, "AR", "CO", "AC").Returns(true);

            Assert.IsTrue(controller.IsAuthorizedFor("AR", "CO", "AC"));
        }

        #endregion

        #region Method: BeginExecuteCore(AsyncCallback callback, Object state)

        [Test]
        public void BeginExecuteCore_SetsThreadCultureFromRequestsRouteValues()
        {
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();
            controller.RouteData.Values["language"] = "lt";

            controller.BaseBeginExecuteCore((asyncResult) => { }, null);
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
            AuthorizationContext filterContext = new AuthorizationContext(controller.ControllerContext, describtor);
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(false);

            controller.BaseOnAuthorization(filterContext);

            Assert.IsNull(filterContext.Result);
        }

        [Test]
        public void OnAuthorization_SetsResultToRedirectToUnauthorizedIfNotAuthorized()
        {
            controller.When(sub => sub.RedirectToUnauthorized()).DoNotCallBase();
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            controller.RedirectToUnauthorized().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            AuthorizationContext context = new AuthorizationContext(controller.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());

            controller.BaseOnAuthorization(context);

            ActionResult expected = controller.RedirectToUnauthorized();
            ActionResult actual = context.Result;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnAuthorization_SetsResultToNullThenAuthorized()
        {
            AuthorizationContext context = new AuthorizationContext(controller.ControllerContext, Substitute.ForPartsOf<ActionDescriptor>());
            Authorization.Provider.IsAuthorizedFor(accountId, "Area", "Controller", "Action").Returns(true);
            controller.ControllerContext.HttpContext.User.Identity.IsAuthenticated.Returns(true);
            context.RouteData.Values["controller"] = "Controller";
            context.RouteData.Values["action"] = "Action";
            context.RouteData.Values["area"] = "Area";

            controller.BaseOnAuthorization(context);

            Assert.IsNull(context.Result);
        }

        #endregion

        #region Method: OnActionExecuted(ActionExecutedContext context)

        [Test]
        public void OnActionExecuted_SetsAlertsToTempDataThenAlertsInTempDataAreNull()
        {
            controller.TempData["Alerts"] = null;
            controller.BaseOnActionExecuted(new ActionExecutedContext());

            Object actual = controller.TempData["Alerts"];
            Object expected = controller.Alerts;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void OnActionExecuted_MergesAlertsToTempData()
        {
            HttpContextBase context = controller.HttpContext;

            BaseControllerProxy mergedController = new BaseControllerProxy();
            mergedController.ControllerContext = new ControllerContext();
            mergedController.ControllerContext.HttpContext = context;
            mergedController.TempData = controller.TempData;
            mergedController.Alerts.AddError("ErrorTest2");
            
            IEnumerable<Alert> controllerAlerts = controller.Alerts;
            IEnumerable<Alert> mergedAlerts = mergedController.Alerts;

            controller.Alerts.AddError("ErrorTest1");
            controller.BaseOnActionExecuted(new ActionExecutedContext());
            mergedController.BaseOnActionExecuted(new ActionExecutedContext());

            IEnumerable<Alert> actual = controller.TempData["Alerts"] as AlertsContainer;
            IEnumerable<Alert> expected = controllerAlerts.Union(mergedAlerts);

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: NotEmptyView(Object model)

        [Test]
        public void NotEmptyView_RedirectsToNotFoundIfModelIsNull()
        {
            controller.When(sub => sub.RedirectToNotFound()).DoNotCallBase();
            controller.RedirectToNotFound().Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectToNotFound();
            ActionResult actual = controller.NotEmptyView(null);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void NotEmptyView_ReturnsViewResultIfModelIsNotNull()
        {
            Object expected = new Object();
            Object actual = (controller.NotEmptyView(expected) as ViewResult).Model;

            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
