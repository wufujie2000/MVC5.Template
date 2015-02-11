using AutoMapper;
using AutoMapper.QueryableExtensions;
using MvcTemplate.Components.Mail;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
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

            HttpContext.Current = HttpContextFactory.CreateHttpContext();
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
            IEnumerator<AccountView> actual = service.GetViews().GetEnumerator();
            IEnumerator<AccountView> expected = context
                .Set<Account>()
                .Project()
                .To<AccountView>()
                .OrderByDescending(account => account.CreationDate)
                .GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.CreationDate, actual.Current.CreationDate);
                Assert.AreEqual(expected.Current.Username, actual.Current.Username);
                Assert.AreEqual(expected.Current.Password, actual.Current.Password);
                Assert.AreEqual(expected.Current.RoleName, actual.Current.RoleName);
                Assert.AreEqual(expected.Current.RoleId, actual.Current.RoleId);
                Assert.AreEqual(expected.Current.Email, actual.Current.Email);
                Assert.AreEqual(expected.Current.Id, actual.Current.Id);
            }
        }

        #endregion

        #region Method: Get<TView>(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            Account account = context.Set<Account>().Single();

            AccountView actual = service.Get<AccountView>(accountId);
            AccountView expected = Mapper.Map<AccountView>(account);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.RoleName, actual.RoleName);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
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
            Account account = context.Set<Account>().AsNoTracking().Single();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);

            AccountRecoveryView recoveryAccount = ObjectFactory.CreateAccountRecoveryView();
            recoveryAccount.Email = recoveryAccount.Email.ToLower();

            service.Recover(recoveryAccount);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.AreEqual(expected.RecoveryTokenExpirationDate.Value.Ticks, actual.RecoveryTokenExpirationDate.Value.Ticks, TimeSpan.TicksPerSecond * 2);
            Assert.AreNotEqual(expected.RecoveryToken, actual.RecoveryToken);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Passhash, actual.Passhash);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
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
            Account account = context.Set<Account>().AsNoTracking().Single();
            hasher.HashPassword(accountReset.NewPassword).Returns("Reset");
            account.RecoveryTokenExpirationDate = null;
            account.RecoveryToken = null;
            account.Passhash = "Reset";

            service.Reset(accountReset);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.AreEqual(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.RecoveryToken, actual.RecoveryToken);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Passhash, actual.Passhash);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Register(AccountView view)

        [Test]
        public void Register_CreatesAccount()
        {
            AccountView account = ObjectFactory.CreateAccountView("2");
            service.Register(account);

            Account actual = context.Set<Account>().AsNoTracking().Single(model => model.Id == account.Id);
            AccountView expected = account;

            Assert.AreEqual(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsNull(actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.IsNull(actual.RecoveryToken);
            Assert.IsNull(actual.RoleId);
            Assert.IsNull(actual.Role);
        }

        [Test]
        public void Register_LowersEmailValue()
        {
            AccountView view = ObjectFactory.CreateAccountView("2");
            String expected = view.Email.ToLower();
            view.Email = view.Email.ToUpper();

            service.Register(view);

            Account model = context.Set<Account>().AsNoTracking().Single(account => account.Id == view.Id);

            Assert.AreEqual(expected, model.Email);
            Assert.AreEqual(expected, view.Email);
        }

        #endregion

        #region Method: Edit(ProfileEditView view)

        [Test]
        public void Edit_EditsProfile()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            Account account = context.Set<Account>().AsNoTracking().Single();
            account.Passhash = hasher.HashPassword(profile.NewPassword);
            account.Email = profile.Email = "test@tests.com";
            account.Username = profile.Username += "1";

            service.Edit(profile);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.AreEqual(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.RecoveryToken, actual.RecoveryToken);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Passhash, actual.Passhash);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void Edit_LowersEmailValue()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            String expected = view.Email.ToLower();
            view.Email = view.Email.ToUpper();

            service.Edit(view);

            Account model = context.Set<Account>().AsNoTracking().Single();

            Assert.AreEqual(expected, model.Email);
            Assert.AreEqual(expected, view.Email);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void Edit_OnNotSpecifiedNewPasswordDoesNotEditPassword(String newPassword)
        {
            String expected = context.Set<Account>().AsNoTracking().Single().Passhash;
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = newPassword;

            service.Edit(profile);

            String actual = context.Set<Account>().AsNoTracking().Single().Passhash;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountEditView view)

        [Test]
        public void Edit_EditsAccountsRoleOnly()
        {
            AccountEditView accountEdit = ObjectFactory.CreateAccountEditView();
            Account account = context.Set<Account>().AsNoTracking().Single();
            account.RoleId = accountEdit.RoleId = null;
            accountEdit.Username += "Edition";

            service.Edit(accountEdit);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.AreEqual(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.AreEqual(expected.RecoveryToken, actual.RecoveryToken);
            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Passhash, actual.Passhash);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void Edit_RefreshesAuthorizationProvider()
        {
            AccountEditView account = ObjectFactory.CreateAccountEditView();

            service.Edit(account);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesAccount()
        {
            service.Delete(accountId);

            CollectionAssert.IsEmpty(context.Set<Account>());
        }

        #endregion

        #region Method: Login(String username)

        [Test]
        public void Login_IsCaseInsensitive()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username.ToUpper());

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            String actual = ticket.Name;
            String expected = accountId;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Login_CreatesPersistentAuthenticationTicket()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            service.Login(account.Username);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.IsTrue(ticket.IsPersistent);
        }

        [Test]
        public void Login_SetAccountIdAsAuthenticationTicketValue()
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