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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Xunit;
using Xunit.Extensions;

namespace MvcTemplate.Tests.Unit.Services
{
    public class AccountServiceTests : IDisposable
    {
        private TestingContext context;
        private AccountService service;
        private IMailClient mailClient;
        private String accountId;
        private IHasher hasher;

        public AccountServiceTests()
        {
            context = new TestingContext();
            hasher = Substitute.For<IHasher>();
            mailClient = Substitute.For<IMailClient>();
            hasher.HashPassword(Arg.Any<String>()).Returns("Hashed");

            Authorization.Provider = Substitute.For<IAuthorizationProvider>();
            service = new AccountService(new UnitOfWork(context), mailClient, hasher);

            TearDownData();
            SetUpData();
        }
        public void Dispose()
        {
            Authorization.Provider = null;
            HttpContext.Current = null;
            service.Dispose();
            context.Dispose();
        }

        #region Method: IsLoggedIn(IPrincipal user)

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsLoggedIn_ReturnsUserAuthenticationStatus(Boolean expected)
        {
            IPrincipal user = Substitute.For<IPrincipal>();
            user.Identity.IsAuthenticated.Returns(expected);

            Boolean actual = service.IsLoggedIn(user);

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: AccountExists(String accountId)

        [Fact]
        public void AccountExists_ReturnsTrueIfAccountExists()
        {
            Assert.True(service.AccountExists(accountId));
        }

        [Fact]
        public void AccountExists_ReturnsFalseIfAccountDoesNotExist()
        {
            Assert.False(service.AccountExists("Test"));
        }

        #endregion

        #region Method: GetViews()

        [Fact]
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
                Assert.Equal(expected.Current.CreationDate, actual.Current.CreationDate);
                Assert.Equal(expected.Current.Username, actual.Current.Username);
                Assert.Equal(expected.Current.Password, actual.Current.Password);
                Assert.Equal(expected.Current.RoleName, actual.Current.RoleName);
                Assert.Equal(expected.Current.RoleId, actual.Current.RoleId);
                Assert.Equal(expected.Current.Email, actual.Current.Email);
                Assert.Equal(expected.Current.Id, actual.Current.Id);
            }
        }

        #endregion

        #region Method: Get<TView>(String id)

        [Fact]
        public void GetView_GetsViewById()
        {
            Account account = context.Set<Account>().Single();

            AccountView actual = service.Get<AccountView>(accountId);
            AccountView expected = Mapper.Map<AccountView>(account);

            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Password, actual.Password);
            Assert.Equal(expected.RoleName, actual.RoleName);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Recover(AccountRecoveryView view, HttpRequestBase request)

        [Fact]
        public void Recover_DoesNotSendRecoveryInformationForNonExistingAccount()
        {
            AccountRecoveryView account = ObjectFactory.CreateAccountRecoveryView();
            account.Email = "not@existing.email";

            service.Recover(account, null);

            mailClient.DidNotReceive().SendAsync(Arg.Any<String>(), Arg.Any<String>(), Arg.Any<String>());
        }

        [Fact]
        public void Recover_UpdatesAccountRecoveryInformation()
        {
            Account account = context.Set<Account>().AsNoTracking().Single();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(30);
            HttpRequestBase request = HttpContextFactory.CreateHttpContextBase().Request;

            AccountRecoveryView recoveryAccount = ObjectFactory.CreateAccountRecoveryView();
            recoveryAccount.Email = recoveryAccount.Email.ToLower();

            service.Recover(recoveryAccount, request);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.InRange(actual.RecoveryTokenExpirationDate.Value.Ticks,
                expected.RecoveryTokenExpirationDate.Value.Ticks - TimeSpan.TicksPerSecond,
                expected.RecoveryTokenExpirationDate.Value.Ticks + TimeSpan.TicksPerSecond);
            Assert.NotEqual(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
            Assert.NotNull(actual.RecoveryToken);
        }

        [Fact]
        public void Recover_SendsRecoveryInformation()
        {
            HttpRequestBase request = HttpContextFactory.CreateHttpContextBase().Request;
            AccountRecoveryView account = ObjectFactory.CreateAccountRecoveryView();
            Account recoveredAccount = context.Set<Account>().Single();
            UrlHelper url = new UrlHelper(request.RequestContext);
            String scheme = request.Url.Scheme;

            service.Recover(account, request);

            String expectedEmail = account.Email;
            String expectedEmailSubject = Messages.RecoveryEmailSubject;
            String recoveryUrl = url.Action("Reset", "Auth", new { token = recoveredAccount.RecoveryToken }, scheme);
            String expectedEmailBody = String.Format(Messages.RecoveryEmailBody, recoveryUrl);

            mailClient.Received().SendAsync(expectedEmail, expectedEmailSubject, expectedEmailBody);
        }

        #endregion

        #region Method: Reset(AccountResetView view)

        [Fact]
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

            Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Register(AccountView view)

        [Fact]
        public void Register_CreatesAccount()
        {
            AccountView account = ObjectFactory.CreateAccountView("2");
            service.Register(account);

            Account actual = context.Set<Account>().AsNoTracking().Single(model => model.Id == account.Id);
            AccountView expected = account;

            Assert.Equal(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Null(actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Null(actual.RecoveryToken);
            Assert.Null(actual.RoleId);
            Assert.Null(actual.Role);
        }

        [Fact]
        public void Register_LowersEmailValue()
        {
            AccountView view = ObjectFactory.CreateAccountView("2");
            String expected = view.Email.ToLower();
            view.Email = view.Email.ToUpper();

            service.Register(view);

            Account model = context.Set<Account>().AsNoTracking().Single(account => account.Id == view.Id);

            Assert.Equal(expected, model.Email);
            Assert.Equal(expected, view.Email);
        }

        #endregion

        #region Method: Edit(ProfileEditView view)

        [Fact]
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

            Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Edit_LowersEmailValue()
        {
            ProfileEditView view = ObjectFactory.CreateProfileEditView();
            String expected = view.Email.ToLower();
            view.Email = view.Email.ToUpper();

            service.Edit(view);

            Account model = context.Set<Account>().AsNoTracking().Single();

            Assert.Equal(expected, model.Email);
            Assert.Equal(expected, view.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Edit_OnNotSpecifiedNewPasswordDoesNotEditPassword(String newPassword)
        {
            String expected = context.Set<Account>().AsNoTracking().Single().Passhash;
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = newPassword;

            service.Edit(profile);

            String actual = context.Set<Account>().AsNoTracking().Single().Passhash;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountEditView view)

        [Fact]
        public void Edit_EditsAccountsRoleOnly()
        {
            AccountEditView accountEdit = ObjectFactory.CreateAccountEditView();
            Account account = context.Set<Account>().AsNoTracking().Single();
            account.RoleId = accountEdit.RoleId = null;
            accountEdit.Username += "Edition";

            service.Edit(accountEdit);

            Account actual = context.Set<Account>().AsNoTracking().Single();
            Account expected = account;

            Assert.Equal(expected.RecoveryTokenExpirationDate, actual.RecoveryTokenExpirationDate);
            Assert.Equal(expected.RecoveryToken, actual.RecoveryToken);
            Assert.Equal(expected.CreationDate, actual.CreationDate);
            Assert.Equal(expected.Username, actual.Username);
            Assert.Equal(expected.Passhash, actual.Passhash);
            Assert.Equal(expected.RoleId, actual.RoleId);
            Assert.Equal(expected.Email, actual.Email);
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public void Edit_RefreshesAuthorizationProvider()
        {
            AccountEditView account = ObjectFactory.CreateAccountEditView();

            service.Edit(account);

            Authorization.Provider.Received().Refresh();
        }

        #endregion

        #region Method: Delete(String id)

        [Fact]
        public void Delete_DeletesAccount()
        {
            service.Delete(accountId);

            Assert.Empty(context.Set<Account>());
        }

        #endregion

        #region Method: Login(String username)

        [Fact]
        public void Login_IsCaseInsensitive()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(account.Username.ToUpper());

            String actual = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value).Name;
            String expected = accountId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Login_CreatesPersistentAuthenticationTicket()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(account.Username);

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value);

            Assert.True(ticket.IsPersistent);
        }

        [Fact]
        public void Login_SetAccountIdAsAuthenticationTicketValue()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(account.Username);

            String actual = FormsAuthentication.Decrypt(HttpContext.Current.Response.Cookies[0].Value).Name;
            String expected = account.Id;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Logout()

        [Fact]
        public void Logout_MakesAccountCookieExpired()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            HttpContext.Current = HttpContextFactory.CreateHttpContext();

            service.Login(account.Username);
            service.Logout();

            DateTime expirationDate = HttpContext.Current.Response.Cookies[0].Expires;

            Assert.True(expirationDate < DateTime.Now);
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
            context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}