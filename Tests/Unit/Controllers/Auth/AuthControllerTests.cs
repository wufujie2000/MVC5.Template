using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Controllers.Auth;
using Template.Objects;
using Template.Services;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Controllers.Auth
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> serviceMock;
        private AuthController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IAuthService>(MockBehavior.Strict);
            serviceMock.SetupAllProperties();

            controller = new AuthController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.Url = new UrlHelper(new HttpMock().HttpContext.Request.RequestContext);
        }

        #region Method: Login(String returnUrl)

        [Test]
        public void Login_RedirectsToUrlIfAlreadyLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(true);
            RedirectResult result = controller.Login("/Home/Index") as RedirectResult;

            Assert.AreEqual("/Home/Index", result.Url);
        }

        [Test]
        public void Login_ReturnsNullModelIfNotLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);

            Assert.IsNull((controller.Login("/") as ViewResult).Model);
        }

        #endregion

        #region Method: Login(LoginView account, String returnUrl)

        [Test]
        public void Login_ReturnsNullModelIfCanNotLogin()
        {
            LoginView account = new LoginView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(false);

            Assert.IsNull((controller.Login(account, null) as ViewResult).Model);
        }

        [Test]
        public void Login_LogsInAccount()
        {
            LoginView account = new LoginView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);
            serviceMock.Setup(mock => mock.Login(account));
            controller.Login(account, null);

            serviceMock.Verify(mock => mock.Login(account), Times.Once());
        }

        [Test]
        public void Login_RedirectsToUrlIfCanLogin()
        {
            LoginView account = new LoginView();
            serviceMock.Setup(mock => mock.Login(account));
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);
            RedirectResult result = controller.Login(account, "/Home/Index") as RedirectResult;

            Assert.IsNotNull("/Home/Index", result.Url);
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
