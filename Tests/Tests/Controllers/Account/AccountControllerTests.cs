using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Controllers.Account;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Controllers.Account
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IAccountService> serviceMock;
        private AccountController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IAccountService>();
            controller = new AccountController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.Url = new UrlHelper(new HttpMock().HttpContext.Request.RequestContext);
        }

        #region Method: Login(String returnUrl)

        [Test]
        public void Login_RedirectsToUrlIfAlreadyLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(true);
            var result = controller.Login("/") as RedirectResult;

            Assert.AreEqual("/", result.Url);
        }

        [Test]
        public void Login_ReturnsViewIfNotLoggedIn()
        {
            serviceMock.Setup(mock => mock.IsLoggedIn()).Returns(false);

            Assert.IsNotNull(controller.Login("/") as ViewResult);
        }

        #endregion

        #region Method: Login(AccountView account, String returnUrl)

        [Test]
        public void Login_IfCanNotLoginReturnsView()
        {
            var account = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(false);

            Assert.IsNotNull(controller.Login(account, null) as ViewResult);
        }

        [Test]
        public void Login_CallsLogin()
        {
            var account = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);
            controller.Login(account, null);

            serviceMock.Verify(mock => mock.CanLogin(account), Times.Once());
        }

        [Test]
        public void Login_RedirectsToUrlIfCanLogin()
        {
            var account = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.CanLogin(account)).Returns(true);

            var result = controller.Login(account, "/") as RedirectResult;

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
            var result = controller.Logout() as RedirectToRouteResult;

            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        #endregion
    }
}
