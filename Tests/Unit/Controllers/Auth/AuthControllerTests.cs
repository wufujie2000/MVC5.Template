using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Alerts;
using Template.Controllers.Auth;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Services;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Controllers.Auth
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<AuthController> controllerMock;
        private Mock<IAuthService> serviceMock;
        private AuthController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IAuthService>(MockBehavior.Strict);
            serviceMock.SetupAllProperties();

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
            MethodInfo createMethod = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == "Register" &&
                    method.GetCustomAttribute<HttpPostAttribute>() != null);

            CustomAttributeData customParameterAttribute = createMethod.GetParameters().First().CustomAttributes.First();

            Assert.AreEqual(typeof(BindAttribute), customParameterAttribute.AttributeType);
            Assert.AreEqual("Exclude", customParameterAttribute.NamedArguments.First().MemberName);
            Assert.AreEqual("Id", customParameterAttribute.NamedArguments.First().TypedValue.Value);
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
            AccountView expected = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.CanRegister(expected)).Returns(false);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);

            Assert.AreSame(expected, (controller.Register(expected) as ViewResult).Model);
        }

        [Test]
        public void Register_RegistersAccount()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.CanRegister(account)).Returns(true);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);
            serviceMock.Setup(mock => mock.Register(account));

            controller.Register(account);

            serviceMock.Verify(mock => mock.Register(account), Times.Once());
        }

        [Test]
        public void Register_AddsSuccessfulRegistrationMessage()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);
            serviceMock.Setup(mock => mock.CanRegister(account)).Returns(true);
            serviceMock.Setup(mock => mock.Register(account));

            controller.Register(account);

            AlertMessage actual = serviceMock.Object.AlertMessages.First();

            Assert.AreEqual(MessagesContainer.DefaultFadeOut, actual.FadeOutAfter);
            Assert.AreEqual(Messages.SuccesfulRegistration, actual.Message);
            Assert.AreEqual(AlertMessageType.Success, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
        }

        [Test]
        public void Register_RedirectsToLoginAfterSuccessfulRegistration()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.CanRegister(account)).Returns(true);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);
            serviceMock.Setup(mock => mock.Register(account));

            RedirectToRouteResult result = controller.Register(account) as RedirectToRouteResult;
            RouteValueDictionary actual = result.RouteValues;

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
            ActionResult expected = new RedirectResult("/Home/Index");
            controllerMock.Protected().Setup<ActionResult>("RedirectToLocal", "/Home/Index").Returns(expected);
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(true);

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
            AccountLoginView account = new AccountLoginView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(false);

            Assert.IsNull((controller.Login(account, null) as ViewResult).Model);
        }

        [Test]
        public void Login_LogsInAccount()
        {
            AccountLoginView account = new AccountLoginView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);
            serviceMock.Setup(mock => mock.Login(account));
            controller.Login(account, null);

            serviceMock.Verify(mock => mock.Login(account), Times.Once());
        }

        [Test]
        public void Login_RedirectsToUrlIfCanLogin()
        {
            AccountLoginView account = new AccountLoginView();
            ActionResult expected = new RedirectResult("/Home/Index");
            controllerMock.Protected().Setup<ActionResult>("RedirectToLocal", "/Home/Index").Returns(expected);
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);
            serviceMock.Setup(mock => mock.Login(account));

            ActionResult actual = controller.Login(account, "/Home/Index");

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
            Object expected = "Login";
            serviceMock.Setup(mock => mock.Logout());
            Object actual = controller.Logout().RouteValues["action"];

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
