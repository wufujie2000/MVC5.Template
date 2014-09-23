using AutoMapper;
using AutoMapper.QueryableExtensions;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcTemplate.Tests.Unit.Services
{
    [TestFixture]
    public class AccountServiceTests
    {
        private TestingContext context;
        private AccountService service;
        private IMailClient mailClient;
        private String accountId;
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            hasher = Substitute.For<IHasher>();
            mailClient = Substitute.For<IMailClient>();
            hasher.HashPassword(Arg.Any<String>()).Returns("Hashed");
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(true);

            HttpContext.Current = new HttpMock().HttpContext;
            Authorization.Provider = Substitute.For<IAuthorizationProvider>();

            service = new AccountService(new UnitOfWork(context), mailClient, hasher);

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
            service.Dispose();
            context.Dispose();
        }

        #region Method: IsLoggedIn()

        [Test]
        public void IsLoggedIn_ReturnsTrueThenAccountIsAuthenticated()
        {
            HttpContext.Current.User.Identity.IsAuthenticated.Returns(true);

            Assert.IsTrue(service.IsLoggedIn());
        }

        [Test]
        public void IsLoggedIn_ReturnsFalseThenAccountIsNotAuthenticated()
        {
            HttpContext.Current.User.Identity.IsAuthenticated.Returns(false);

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
                .OrderByDescending(account => account.CreationDate);

            TestHelper.EnumPropertyWiseEqual(expected, actual);
        }

        #endregion

        #region Method: GetView<TView>(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            Account account = context.Set<Account>().SingleOrDefault();

            AccountView actual = service.GetView<AccountView>(accountId);
            AccountView expected = Mapper.Map<AccountView>(account);

            TestHelper.PropertyWiseEqual(expected, actual);
        }

        #endregion

        #region Method: Recover(AccountRecoveryView view)

        [Test]
        public void Recover_DoesNotSendRecoveryInformation()
        {
            AccountRecoveryView account = ObjectFactory.CreateAccountRecoveryView();
            account.Email = "not@existing.email";

            service.Recover(account);

            mailClient.DidNotReceive().Send(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>());
        }

        [Test]
        public void Recover_UpdatesAccountRecoveryInformation()
        {
            AccountRecoveryView account = ObjectFactory.CreateAccountRecoveryView();
            Account expected = context.Set<Account>().AsNoTracking().Single();
            String oldToken = expected.RecoveryToken;
            account.Email = account.Email.ToLower();

            service.Recover(account);

            Account actual = context.Set<Account>().Single();
            expected.RecoveryTokenExpirationDate = actual.RecoveryTokenExpirationDate;
            expected.RecoveryToken = actual.RecoveryToken;

            Assert.AreEqual(actual.RecoveryTokenExpirationDate.Value.Ticks, DateTime.Now.AddMinutes(30).Ticks, 10000000);
            Assert.AreNotEqual(oldToken, actual.RecoveryToken);
            TestHelper.PropertyWiseEqual(expected, actual);
            Assert.IsNotNull(actual.RecoveryToken);
        }

        [Test]
        public void Recover_SendsRecoveryInformation()
        {
            HttpRequest request = HttpContext.Current.Request;
            String scheme = HttpContext.Current.Request.Url.Scheme;
            Account recoveredAccount = context.Set<Account>().Single();
            UrlHelper urlHelper = new UrlHelper(request.RequestContext);
            AccountRecoveryView account = ObjectFactory.CreateAccountRecoveryView();

            service.Recover(account);

            String expectedEmail = account.Email;
            String expectedEmailSubject = Messages.RecoveryEmailSubject;
            String recoveryUrl = urlHelper.Action("Reset", "Auth", new { token = recoveredAccount.RecoveryToken }, scheme);
            String expectedEmailBody = String.Format(Messages.RecoveryEmailBody, recoveryUrl);

            mailClient.Received().Send(expectedEmail, expectedEmailSubject, expectedEmailBody);
        }

        #endregion

        #region Method: Reset(AccountResetView view)

        [Test]
        public void Reset_ResetsAccount()
        {
            AccountResetView accountReset = ObjectFactory.CreateAccountResetView();
            Account expected = context.Set<Account>().AsNoTracking().Single();
            hasher.HashPassword(accountReset.NewPassword).Returns("Reset");

            service.Reset(accountReset);

            Account actual = context.Set<Account>().Single();
            expected.RecoveryTokenExpirationDate = null;
            expected.RecoveryToken = null;
            expected.Passhash = "Reset";

            TestHelper.PropertyWiseEqual(expected, actual);
        }

        #endregion

        #region Method: Register(AccountView view)

        [Test]
        public void Register_CreatesAccount()
        {
            AccountView expected = ObjectFactory.CreateAccountView(2);
            service.Register(expected);

            Account actual = context.Set<Account>().SingleOrDefault(model => model.Id == expected.Id);

            Assert.AreEqual(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsNull(actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RecoveryToken);
            Assert.IsNull(actual.RoleId);
        }

        [Test]
        public void Register_LowersEmailValue()
        {
            AccountView view = ObjectFactory.CreateAccountView(2);
            String expected = view.Email.ToLower();
            view.Email = view.Email.ToUpper();

            service.Register(view);

            Account model = context.Set<Account>().SingleOrDefault(account => account.Id == view.Id);

            Assert.AreEqual(expected, model.Email);
            Assert.AreEqual(expected, view.Email);
        }

        #endregion

        #region Method: Edit(ProfileEditView view)

        [Test]
        public void Edit_EditsProfile()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            Account expected = context.Set<Account>().SingleOrDefault();
            profile.Email = "test@tests.com";
            profile.Username += "1";
            service.Edit(profile);

            Account actual = context.Set<Account>().SingleOrDefault();

            Assert.AreEqual(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(hasher.HashPassword(profile.NewPassword), actual.Passhash);
            Assert.AreEqual(expected.RecoveryToken, actual.RecoveryToken);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Email, actual.Email);
        }

        [Test]
        public void Edit_LowersEmailValue()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            String expected = view.Email.ToLower();
            view.Email = view.Email.ToUpper();

            service.Edit(view);

            Account model = context.Set<Account>().SingleOrDefault();

            Assert.AreEqual(expected, model.Email);
            Assert.AreEqual(expected, view.Email);
        }

        [Test]
        public void Edit_LeavesCurrentPasswordAfterEditing()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = null;
            service.Edit(profile);

            Account actual = context.Set<Account>().SingleOrDefault();

            Assert.IsTrue(hasher.Verify(profile.Password, actual.Passhash));
        }

        [Test]
        public void Edit_LeavesCurrentRoleAfterEditing()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username += "New username";
            service.Edit(profile);

            String actual = context.Set<Account>().SingleOrDefault().RoleId;
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

            Account actual = context.Set<Account>().SingleOrDefault();

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void Edit_DoesNotEditAccountsUsername()
        {
            AccountEditView account = service.GetView<AccountEditView>(accountId);
            String expected = account.Username;
            account.Username += "Edition";
            service.Edit(account);

            String actual = context.Set<Account>().SingleOrDefault().Username;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Edit_DoesNotEditAccountsPassword()
        {
            String expected = context.Set<Account>().SingleOrDefault().Passhash;
            AccountEditView account = service.GetView<AccountEditView>(accountId);
            account.Username += "Edition";
            service.Edit(account);

            String actual = context.Set<Account>().SingleOrDefault().Passhash;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Edit_RefreshesAuthorizationProvider()
        {
            AccountEditView account = service.GetView<AccountEditView>(accountId);

            service.Edit(account);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesAccount()
        {
            Assert.IsTrue(context.Set<Account>().Any(account => account.Id == accountId));

            service.Delete(accountId);

            Assert.IsFalse(context.Set<Account>().Any());
        }

        #endregion

        #region Method: Login(String username)

        [Test]
        public void Login_CreatesPersistentCookie()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
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
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}