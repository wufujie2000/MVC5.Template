using AutoMapper;
using AutoMapper.QueryableExtensions;
using Moq;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MvcTemplate.Tests.Unit.Services
{
    [TestFixture]
    public class AccountServiceTests
    {
        private Mock<IHasher> hasherMock;
        private AccountService service;
        private HttpMock httpMock;
        private String accountId;
        private Context context;
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            httpMock = new HttpMock();
            context = new TestingContext();
            HttpContext.Current = httpMock.HttpContext;
            hasherMock = new Mock<IHasher>(MockBehavior.Strict);
            hasherMock.Setup(mock => mock.HashPassword(It.IsAny<String>())).Returns("Hashed");
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(true);
            service = new AccountService(new UnitOfWork(context), hasherMock.Object);
            hasher = hasherMock.Object;

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

        #region Method: AccountExists(String accountId)

        [Test]
        public void AccountExists_ReturnsTrueIfAccountExists()
        {
            Assert.IsTrue(service.AccountExists(accountId));
        }

        [Test]
        public void AccountExists_ReturnsFalseIfAccountDoesNotExist()
        {
            Assert.IsFalse(service.AccountExists("Test"));
        }

        #endregion

        #region Method: GetViews()

        [Test]
        public void GetViews_GetsAccountViews()
        {
            IEnumerable<AccountView> actual = service.GetViews();
            IEnumerable<AccountView> expected = context
                .Set<Account>()
                .Project()
                .To<AccountView>()
                .OrderByDescending(account => account.EntityDate);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: GetView<TView>(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            Account account = context.Set<Account>().SingleOrDefault(model => model.Id == accountId);
            AccountView expected = Mapper.Map<Account, AccountView>(account);
            AccountView actual = service.GetView<AccountView>(accountId);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Register(AccountView view)

        [Test]
        public void Register_CreatesAccount()
        {
            TearDownData();

            AccountView expected = ObjectFactory.CreateAccountView();
            service.Register(expected);

            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            Assert.AreEqual(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RoleId);
        }

        #endregion

        #region Method: Edit(ProfileEditView view)

        [Test]
        public void Edit_EditsProfile()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            Account expected = context.Set<Account>().SingleOrDefault(acc => acc.Id == accountId);
            profile.Username += "1";
            service.Edit(profile);

            Account actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == accountId);

            Assert.AreEqual(hasher.HashPassword(profile.NewPassword), actual.Passhash);
            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
        }

        [Test]
        public void Edit_LeavesCurrentPasswordAfterEditing()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = null;
            service.Edit(profile);

            Account actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == accountId);

            Assert.IsTrue(hasher.Verify(profile.Password, actual.Passhash));
        }

        [Test]
        public void Edit_LeavesCurrentRoleAfterEditing()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username += "New username";
            service.Edit(profile);

            String actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == accountId).RoleId;
            String expected = accountId;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountEditView view)

        [Test]
        public void Edit_EditsAccount()
        {
            Role role = ObjectFactory.CreateRole(2);
            context.Set<Role>().Add(role);
            context.SaveChanges();

            AccountEditView expected = service.GetView<AccountEditView>(accountId);
            expected.RoleId = role.Id;
            service.Edit(expected);

            context = new TestingContext();
            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void Edit_DoesNotEditAccountsUsername()
        {
            AccountEditView account = service.GetView<AccountEditView>(accountId);
            String expected = account.Username;
            account.Username += "Edition";
            service.Edit(account);

            String actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id).Username;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Edit_DoesNotEditAccountsPassword()
        {
            String expected = context.Set<Account>().SingleOrDefault(acc => acc.Id == accountId).Passhash;
            AccountEditView account = service.GetView<AccountEditView>(accountId);
            account.Username += "Edition";
            service.Edit(account);

            String actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id).Passhash;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Edit_DoesNotEditAccountsEmail()
        {
            AccountEditView account = service.GetView<AccountEditView>(accountId);
            String expected = account.Email;

            account.Email = "Edit_DoesNotEditAccountsEmail@tests.com";
            service.Edit(account);

            String actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id).Email;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesAccount()
        {
            if (context.Set<Account>().SingleOrDefault(acc => acc.Id == accountId) == null)
                Assert.Inconclusive();

            service.Delete(accountId);

            Assert.IsNull(context.Set<Account>().SingleOrDefault(acc => acc.Id == accountId));
        }

        #endregion

        #region Method: Login(String username)

        [Test]
        public void Login_CreatesCookieForAMonth()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username);

            DateTime actual = HttpContext.Current.Response.Cookies[0].Expires.Date;
            DateTime expected = DateTime.Now.AddMonths(1).Date;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesPersistentCookie()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
        }

        [Test]
        public void Login_CreatesCookieWithoutClientSideAccess()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username);

            Assert.IsTrue(HttpContext.Current.Response.Cookies[0].HttpOnly);
        }

        [Test]
        public void Login_SetAccountIdAsCookieValue()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);
            String expected = account.Id;
            String actual = ticket.Name;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Test]
        public void Logout_MakesAccountCookieExpired()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username);
            service.Logout();

            DateTime cookieExpirationDate = HttpContext.Current.Response.Cookies[0].Expires;

            Assert.That(cookieExpirationDate, Is.LessThan(DateTime.Now));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            Account account = ObjectFactory.CreateAccount();
            account.Role = ObjectFactory.CreateRole();
            account.RoleId = account.Role.Id;
            accountId = account.Id;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Role>().RemoveRange(context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}