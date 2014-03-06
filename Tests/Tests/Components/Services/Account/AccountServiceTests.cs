using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class AccountServiceTests
    {
        private HttpContextBaseMock httpContextMock;
        private AccountService service;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new AccountService(new UnitOfWork(context));
            httpContextMock = new HttpContextBaseMock();
            service.HttpContext = httpContextMock.HttpContextBase;
            service.ModelState = new ModelStateDictionary();
            HttpContext.Current = httpContextMock.HttpContext;

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

        #region Method: IsLoggedIn()

        [Test]
        public void IsLoggedIn_ReturnsTrueThenUserIsAuthenticated()
        {
            httpContextMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(true);

            Assert.IsTrue(service.IsLoggedIn());
        }

        [Test]
        public void IsLoggedIn_ReturnsFalseThenUserIsNotAuthenticated()
        {
            httpContextMock.IdentityMock.Setup(mock => mock.IsAuthenticated).Returns(false);

            Assert.IsFalse(service.IsLoggedIn());
        }

        #endregion

        #region Method: CanLogin(AccountView accountView)

        [Test]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "ErrorMesages");
            Assert.IsFalse(service.CanLogin(ObjectFactory.CreateAccountView()));
        }

        [Test]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            var account = new AccountView();
            account.Username = String.Empty;

            Assert.IsFalse(service.CanLogin(account));
            Assert.AreEqual(service.ModelState[String.Empty].Errors[0].ErrorMessage, Validations.IncorrectUsernameOrPassword);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            var accountView = ObjectFactory.CreateAccountView();
            accountView.Password += "1";

            Assert.IsFalse(service.CanLogin(accountView));
            Assert.AreEqual(service.ModelState[String.Empty].Errors[0].ErrorMessage, Validations.IncorrectUsernameOrPassword);
        }

        [Test]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            var accountView = ObjectFactory.CreateAccountView();
            accountView.Username = accountView.Username.ToUpper();

            Assert.IsTrue(service.CanLogin(accountView));
        }

        #endregion

        #region Method: Login(AccountView account)

        [Test]
        public void Login_SetsAccountId()
        {
            var accountView = ObjectFactory.CreateAccountView();
            var expectedId = accountView.Id;
            accountView.Id = null;

            service.Login(accountView);

            Assert.AreEqual(expectedId, accountView.Id);
        }

        [Test]
        public void Login_CreatesCookieForAMonth()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            var expectedExpireDate = DateTime.Now.AddMonths(1).Date;

            Assert.AreEqual(expectedExpireDate, service.HttpContext.Response.Cookies[0].Expires.Date);
        }

        [Test]
        public void Login_CreatesPersistentCookie()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            var ticket = FormsAuthentication.Decrypt(service.HttpContext.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
        }

        [Test]
        public void Login_CreatesCookieWithoutClientSideAccess()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            Assert.IsFalse(service.HttpContext.Response.Cookies[0].HttpOnly);
        }

        [Test]
        public void Login_SetAccountIdAsCookieValue()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            var ticket = FormsAuthentication.Decrypt(service.HttpContext.Response.Cookies[0].Value);

            Assert.AreEqual(accountView.Id, ticket.Name);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_MakesUserCookieExpired()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);
            service.Logout();

            Assert.Less(service.HttpContext.Response.Cookies[0].Expires, DateTime.Now);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            var account = ObjectFactory.CreateAccount();
            account.User = ObjectFactory.CreateUser();
            account.UserId = account.User.Id;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            var testId = TestContext.CurrentContext.Test.Name;
            foreach (var user in context.Set<User>().Where(user => user.Id.StartsWith(testId)))
                context.Set<User>().Remove(user);

            context.SaveChanges();
        }

        #endregion
    }
}
