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

namespace Template.Tests.Unit.Components.Services
{
    [TestFixture]
    public class AccountServiceTests
    {
        private HttpMock httpContextMock;
        private AccountService service;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new AccountService(new UnitOfWork(context));

            httpContextMock = new HttpMock();
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

        #region Method: IsLoggedIn(HttpContextBase context)

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
            AccountView account = new AccountView();
            account.Username = String.Empty;

            Assert.IsFalse(service.CanLogin(account));
            Assert.AreEqual(service.ModelState[String.Empty].Errors[0].ErrorMessage, Validations.IncorrectUsernameOrPassword);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            accountView.Password += "1";

            Assert.IsFalse(service.CanLogin(accountView));
            Assert.AreEqual(service.ModelState[String.Empty].Errors[0].ErrorMessage, Validations.IncorrectUsernameOrPassword);
        }

        [Test]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            accountView.Username = accountView.Username.ToUpper();

            Assert.IsTrue(service.CanLogin(accountView));
        }

        #endregion

        #region Method: Login(HttpContextBase context, AccountView account)

        [Test]
        public void Login_SetsAccountId()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            String expected = accountView.Id;
            accountView.Id = null;

            service.Login(accountView);
            String actual = accountView.Id;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesCookieForAMonth()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);
            
            DateTime actual = HttpContext.Current.Response.Cookies[0].Expires.Date;
            DateTime expected = DateTime.Now.AddMonths(1).Date;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesPersistentCookie()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
        }

        [Test]
        public void Login_CreatesCookieWithoutClientSideAccess()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            Assert.IsTrue(HttpContext.Current.Response.Cookies[0].HttpOnly);
        }

        [Test]
        public void Login_SetAccountIdAsCookieValue()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);
            String expected = accountView.Id;
            String actual = ticket.Name;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_MakesUserCookieExpired()
        {
            AccountView accountView = ObjectFactory.CreateAccountView();
            service.Login(accountView);
            service.Logout();

            DateTime cookieExpirationDate = HttpContext.Current.Response.Cookies[0].Expires;

            Assert.That(cookieExpirationDate, Is.LessThan(DateTime.Now));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            Account account = ObjectFactory.CreateAccount();
            account.Person = ObjectFactory.CreatePerson();
            account.PersonId = account.Person.Id;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Person>().RemoveRange(context.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
