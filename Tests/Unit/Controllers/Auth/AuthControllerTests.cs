using Moq;
using Moq.Protected;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Controllers.Auth;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers.Auth
{
    [TestFixture]
    public class AuthControllerTests : AControllerTests
    {
        private Mock<AuthController> controllerMock;
        private Mock<IAccountsService> serviceMock;
        private AccountLoginView accountLogin;
        private AuthController controller;
        private AccountView account;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IAccountsService>(MockBehavior.Strict);
            serviceMock.SetupAllProperties();
            
            accountLogin = ObjectFactory.CreateAccountLoginView();
            account = ObjectFactory.CreateAccountView();

            serviceMock.Object.Alerts = new AlertsContainer();
            controllerMock = new Mock<AuthController>(serviceMock.Object) { CallBase = true };
            controllerMock.Object.Url = new UrlHelper(new HttpMock().HttpContext.Request.RequestContext);
            controllerMock.Object.ControllerContext = new ControllerContext();
            controller = controllerMock.Object;
        }

        #region Method: Register()

        [Test]
        public void Register_RedirectsToDefaultlIfAlreadyLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(true);

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(expected);
            ActionResult actual = controller.Register();

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Register_ReturnsNullModelIfNotLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);

            Assert.IsNull((controller.Register() as ViewResult).Model);
        }

        #endregion

        #region Method: Register(AccountView account)

        [Test]
        public void Register_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Register");
        }

        [Test]
        public void Register_OnPostRedirectsToDefaultlIfAlreadyLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(true);

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(expected);
            ActionResult actual = controller.Register(null);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Register_ReturnsSameModelIfCanNotRegister()
        {
            serviceMock.Setup(mock => mock.CanRegister(account)).Returns(false);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);

            Object actual = (controller.Register(account) as ViewResult).Model;
            AccountView expected = account;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Register_RegistersAccount()
        {
            serviceMock.Setup(mock => mock.CanRegister(account)).Returns(true);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);
            serviceMock.Setup(mock => mock.Register(account));

            controller.Register(account);

            serviceMock.Verify(mock => mock.Register(account), Times.Once());
        }

        [Test]
        public void Register_AddsSuccessfulRegistrationMessage()
        {
            serviceMock.Setup(mock => mock.CanRegister(account)).Returns(true);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);
            serviceMock.Setup(mock => mock.Register(account));

            controller.Register(account);

            Alert actual = serviceMock.Object.Alerts.First();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(Messages.SuccesfulRegistration, actual.Message);
            Assert.AreEqual(AlertTypes.Success, actual.Type);
        }

        [Test]
        public void Register_RedirectsToLoginAfterSuccessfulRegistration()
        {
            serviceMock.Setup(mock => mock.CanRegister(account)).Returns(true);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);
            serviceMock.Setup(mock => mock.Register(account));

            RouteValueDictionary actual = (controller.Register(account) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual("Login", actual["action"]);
            Assert.IsNull(actual["controller"]);
            Assert.IsNull(actual["language"]);
            Assert.IsNull(actual["area"]);
        }

        #endregion

        #region Method: Login(String returnUrl)

        [Test]
        public void Login_RedirectsToUrlIfAlreadyLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(true);

            ActionResult expected = new RedirectResult("/Home/Index");
            controllerMock.Protected().Setup<ActionResult>("RedirectToLocal", "/Home/Index").Returns(expected);
            ActionResult actual = controller.Login("/Home/Index");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_ReturnsNullModelIfNotLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);

            Assert.IsNull((controller.Login("/") as ViewResult).Model);
        }

        #endregion

        #region Method: Login(AccountLoginView account, String returnUrl)

        [Test]
        public void Login_ReturnsNullModelIfCanNotLogin()
        {
            serviceMock.Setup(mock => mock.CanLogin(accountLogin)).Returns(false);

            Assert.IsNull((controller.Login(accountLogin, null) as ViewResult).Model);
        }

        [Test]
        public void Login_LogsInAccount()
        {
            serviceMock.Setup(mock => mock.CanLogin(accountLogin)).Returns(true);
            serviceMock.Setup(mock => mock.Login(accountLogin.Username));

            controller.Login(accountLogin, null);

            serviceMock.Verify(mock => mock.Login(accountLogin.Username), Times.Once());
        }

        [Test]
        public void Login_RedirectsToUrlIfCanLogin()
        {
            serviceMock.Setup(mock => mock.CanLogin(accountLogin)).Returns(true);
            serviceMock.Setup(mock => mock.Login(account.Username));

            ActionResult expected = new RedirectResult("/Home/Index");
            controllerMock.Protected().Setup<ActionResult>("RedirectToLocal", "/Home/Index").Returns(expected);
            ActionResult actual = controller.Login(accountLogin, "/Home/Index");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_LogsOut()
        {
            serviceMock.Setup(mock => mock.Logout());

            controller.Logout();

            serviceMock.Verify(mock => mock.Logout(), Times.Once());
        }

        [Test]
        public void Logout_RedirectsToLogin()
        {
            serviceMock.Setup(mock => mock.Logout());

            Object actual = controller.Logout().RouteValues["action"];
            Object expected = "Login";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
