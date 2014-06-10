using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Services;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private AuthService service;
        private HttpMock httpMock;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new AuthService(new UnitOfWork(context));

            httpMock = new HttpMock();
            service.ModelState = new ModelStateDictionary();
            HttpContext.Current = httpMock.HttpContext;

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;

            service.Dispose();
            context.Dispose();
        }

        #region Method: IsLoggedIn(HttpContextBase context)

        [Test]
        public void IsLoggedIn_ReturnsTrueThenAccountIsAuthenticated()
        {
            httpMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(true);

            Assert.IsTrue(service.IsLoggedIn());
        }

        [Test]
        public void IsLoggedIn_ReturnsFalseThenAccountIsNotAuthenticated()
        {
            httpMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(false);

            Assert.IsFalse(service.IsLoggedIn());
        }

        #endregion

        #region Method: CanLogin(AccountView accountView)

        [Test]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "ErrorMesages");

            Assert.IsFalse(service.CanLogin(ObjectFactory.CreateLoginView()));
        }

        [Test]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            LoginView account = new LoginView();
            account.Username = String.Empty;

            Assert.IsFalse(service.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginFromNotExistingAccount()
        {
            LoginView account = new LoginView();
            account.Username = String.Empty;
            service.CanLogin(account);

            String expected = Validations.IncorrectUsernameOrPassword;
            String actual = service.ModelState[String.Empty].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            LoginView loginView = ObjectFactory.CreateLoginView();
            loginView.Password += "Incorrect";

            Assert.IsFalse(service.CanLogin(loginView));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithIncorrectPassword()
        {
            LoginView loginView = ObjectFactory.CreateLoginView();
            loginView.Password += "Incorrect";
            service.CanLogin(loginView);

            String expected = Validations.IncorrectUsernameOrPassword;
            String actual = service.ModelState[String.Empty].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            LoginView loginView = ObjectFactory.CreateLoginView();
            loginView.Username = loginView.Username.ToUpper();

            Assert.IsTrue(service.CanLogin(loginView));
        }

        #endregion

        #region Method: Login(HttpContextBase context, LoginView account)

        [Test]
        public void Login_SetsAccountId()
        {
            LoginView accountView = ObjectFactory.CreateLoginView();
            String expected = accountView.Id;
            accountView.Id = null;

            service.Login(accountView);
            String actual = accountView.Id;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesCookieForAMonth()
        {
            LoginView accountView = ObjectFactory.CreateLoginView();
            service.Login(accountView);

            DateTime actual = HttpContext.Current.Response.Cookies[0].Expires.Date;
            DateTime expected = DateTime.Now.AddMonths(1).Date;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesPersistentCookie()
        {
            LoginView loginView = ObjectFactory.CreateLoginView();
            service.Login(loginView);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
        }

        [Test]
        public void Login_CreatesCookieWithoutClientSideAccess()
        {
            LoginView loginView = ObjectFactory.CreateLoginView();
            service.Login(loginView);

            Assert.IsTrue(HttpContext.Current.Response.Cookies[0].HttpOnly);
        }

        [Test]
        public void Login_SetAccountIdAsCookieValue()
        {
            LoginView loginView = ObjectFactory.CreateLoginView();
            service.Login(loginView);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);
            String expected = loginView.Id;
            String actual = ticket.Name;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_MakesAccountCookieExpired()
        {
            LoginView loginView = ObjectFactory.CreateLoginView();
            service.Login(loginView);
            service.Logout();

            DateTime cookieExpirationDate = HttpContext.Current.Response.Cookies[0].Expires;

            Assert.That(cookieExpirationDate, Is.LessThan(DateTime.Now));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            Account account = ObjectFactory.CreateAccount();

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(account => account.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
