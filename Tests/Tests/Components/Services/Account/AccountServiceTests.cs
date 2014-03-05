using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class AccountServiceTests
    {
        private ModelStateDictionary modelState;
        private AccountService service;
        private AContext context;
        private Account account;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContextBaseMock().Context;
            modelState = new ModelStateDictionary();
            service = new AccountService(new UnitOfWork());
            context = new Context();

            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            TearDownData();

            service.Dispose();
            context.Dispose();
        }

        #region Method: CanLogin(AccountView accountView)

        [Test]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            modelState.AddModelError("Test", "Test");
            Assert.IsFalse(service.CanLogin(ObjectFactory.CreateAccountView()));
        }

        [Test]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            var account = new AccountView();
            account.Username = String.Empty;

            Assert.IsFalse(service.CanLogin(account));
            Assert.AreEqual(1, modelState[String.Empty].Errors.Count);
            Assert.AreEqual(modelState[String.Empty].Errors[0].ErrorMessage, Validations.IncorrectUsernameOrPassword);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            var accountView = ObjectFactory.CreateAccountView();
            accountView.Password += "1";

            Assert.IsFalse(service.CanLogin(accountView));
            Assert.AreEqual(1, modelState[String.Empty].Errors.Count);
            Assert.AreEqual(modelState[String.Empty].Errors[0].ErrorMessage, Validations.IncorrectUsernameOrPassword);
        }

        [Test]
        public void CanLogin_CanLoginWithCaseInsensativeUsername()
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
        public void Login_CreatesCookieForAccount()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            Assert.AreEqual(1, HttpContext.Current.Response.Cookies.Count);
        }

        [Test]
        public void Login_CreatesCookieForAMonth()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            var expectedExpireDate = DateTime.Now.AddMonths(1).Date;

            Assert.AreEqual(expectedExpireDate, HttpContext.Current.Response.Cookies[0].Expires.Date);
        }

        [Test]
        public void Login_CreatesPersistentCookie()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            var ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
        }

        [Test]
        public void Login_CreatesCookieWithoutClientSideAccess()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            Assert.IsFalse(HttpContext.Current.Response.Cookies[0].HttpOnly);
        }

        [Test]
        public void Login_SetAccountIdAsCookieValue()
        {
            var accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            var ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

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

            Assert.AreEqual(1, HttpContext.Current.Response.Cookies.Count);
            Assert.Less(HttpContext.Current.Response.Cookies[0].Expires, DateTime.Now);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
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
