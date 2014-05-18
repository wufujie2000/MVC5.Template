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
            serviceMock = new Mock<IAuthService>();
            controller = new AuthController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.Url = new UrlHelper(new HttpMock().HttpContext.Request.RequestContext);
        }

        #region Method: Login(String returnUrl)

        [Test]
        public void Login_RedirectsToUrlIfAlreadyLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(true);
            RedirectResult result = controller.Login("/") as RedirectResult;

            Assert.AreEqual("/", result.Url);
        }

        [Test]
        public void Login_ReturnsEmptyViewIfNotLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);

            Assert.IsNull((controller.Login("/") as ViewResult).Model);
        }

        #endregion

        #region Method: Login(LoginView account, String returnUrl)

        [Test]
        public void Login_IfCanNotLoginReturnsView()
        {
            LoginView account = ObjectFactory.CreateLoginView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(false);

            Assert.IsNotNull(controller.Login(account, null) as ViewResult);
        }

        [Test]
        public void Login_CallsLogin()
        {
            LoginView account = ObjectFactory.CreateLoginView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);
            controller.Login(account, null);

            serviceMock.Verify(mock => mock.CanLogin(account), Times.Once());
        }

        [Test]
        public void Login_RedirectsToUrlIfCanLogin()
        {
            LoginView account = ObjectFactory.CreateLoginView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);
            RedirectResult result = controller.Login(account, "/") as RedirectResult;

            Assert.IsNotNull("/", result.Url);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_LogsOut()
        {
            controller.Logout();

            serviceMock.Verify(mock => mock.Logout(), Times.Once());
        }

        [Test]
        public void Logout_RedirectsToLogin()
        {
            Object expected = "Login";
            Object actual = controller.Logout().RouteValues["action"];

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
