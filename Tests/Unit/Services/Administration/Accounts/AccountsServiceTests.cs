using AutoMapper;
using AutoMapper.QueryableExtensions;
using Moq;
using MvcTemplate.Components.Alerts;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
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
    public class AccountsServiceTests
    {
        private Mock<IHasher> hasherMock;
        private AccountsService service;
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
            service = new AccountsService(new UnitOfWork(context), hasherMock.Object);
            service.Alerts = new AlertsContainer();
            service.ModelState = new ModelStateDictionary();
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

        #region Method: CanLogin(AccountLoginView view)

        [Test]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "ErrorMesages");

            Assert.IsFalse(service.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        [Test]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            AccountLoginView account = new AccountLoginView();
            account.Username = String.Empty;

            Assert.IsFalse(service.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithNotExistingAccount()
        {
            AccountLoginView account = new AccountLoginView();
            account.Username = String.Empty;
            service.CanLogin(account);

            String expected = Validations.IncorrectUsernameOrPassword;
            Alert actualMessage = service.Alerts.First();

            Assert.AreEqual(AlertTypes.Danger, actualMessage.Type);
            Assert.AreEqual(expected, actualMessage.Message);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();

            Assert.IsFalse(service.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            account.Password += "Incorrect";
            service.CanLogin(account);

            String expected = Validations.IncorrectUsernameOrPassword;
            Alert actualMessage = service.Alerts.First();

            Assert.AreEqual(AlertTypes.Danger, actualMessage.Type);
            Assert.AreEqual(expected, actualMessage.Message);
        }

        [Test]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            account.Username = account.Username.ToUpper();

            Assert.IsTrue(service.CanLogin(account));
        }

        [Test]
        public void CanLogin_CanLoginWithValidAccount()
        {
            Assert.IsTrue(service.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        #endregion

        #region Method: CanRegister(AccountView view)

        [Test]
        public void CanRegister_CanNotRegisterWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "Error");

            Assert.IsFalse(service.CanRegister(ObjectFactory.CreateAccountView()));
        }

        [Test]
        public void CanRegister_CanNotRegisterWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "DifferentValue";

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErorrMessageThenCanNotRegisterWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "DifferentValue";
            service.CanRegister(account);

            String actual = service.ModelState["Username"].Errors[0].ErrorMessage;
            String expected = Validations.UsernameIsAlreadyTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanRegister_CanNotRegisterWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView(1);
            account.Id += "DifferentValue";

            Assert.IsFalse(service.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErrorMessageThenCanNotRegisterWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Id += "DifferentValue";
            service.CanRegister(account);

            String actual = service.ModelState["Email"].Errors[0].ErrorMessage;
            String expected = Validations.EmailIsAlreadyUsed;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanRegister_CanRegisterValidAccount()
        {
            Assert.IsTrue(service.CanRegister(ObjectFactory.CreateAccountView(2)));
        }

        #endregion

        #region Method: CanEdit(ProfileEditView view)

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "ErrorMessages");

            Assert.IsFalse(service.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        [Test]
        public void CanEdit_CanNotEditWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Password += "1";

            Assert.IsFalse(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Password += "1";
            service.CanEdit(profile);

            String expected = Validations.IncorrectPassword;
            String actual = service.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            Account takenAccount = ObjectFactory.CreateAccount();
            takenAccount.Username += "1";
            takenAccount.Id += "1";

            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = takenAccount.Username;

            Assert.IsFalse(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditToAlreadyTakenUsername()
        {
            Account takenAccount = ObjectFactory.CreateAccount();
            takenAccount.Username += "1";
            takenAccount.Id += "1";

            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = takenAccount.Username;
            service.CanEdit(profile);

            String expected = Validations.UsernameIsAlreadyTaken;
            String actual = service.ModelState["Username"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditUsingItsOwnUsername()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = profile.Username.ToUpper();

            Assert.IsTrue(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanEditWithoutSpecifyingNewPassword()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = null;

            Assert.IsTrue(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyUsedEmail()
        {
            Account takenEmailAccount = ObjectFactory.CreateAccount();
            takenEmailAccount.Username += "1";
            takenEmailAccount.Id += "1";

            context.Set<Account>().Add(takenEmailAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();

            Assert.IsFalse(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErorrMessageThenCanNotEditToAlreadyUsedEmail()
        {
            Account takenEmailAccount = ObjectFactory.CreateAccount();
            takenEmailAccount.Username += "1";
            takenEmailAccount.Id += "1";

            context.Set<Account>().Add(takenEmailAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            service.CanEdit(profile);

            String expected = Validations.EmailIsAlreadyUsed;
            String actual = service.ModelState["Email"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditUsingItsOwnEmail()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = profile.Email.ToUpper();

            Assert.IsTrue(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanEditValidProfile()
        {
            Assert.IsTrue(service.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        #endregion

        #region Method: CanEdit(AccountEditView view)

        [Test]
        public void CanEdit_CanNotEditAccountWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanEdit(new AccountEditView()));
        }

        [Test]
        public void CanEdit_CanEditValidAccount()
        {
            Assert.IsTrue(service.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        #endregion

        #region Method: CanDelete(AccountView view)

        [Test]
        public void CanDelete_CanNotDeleteWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountView profile = ObjectFactory.CreateAccountView();
            profile.Password += "1";

            Assert.IsFalse(service.CanDelete(profile));
        }

        [Test]
        public void CanDelete_AddsErrorMessageThenCanNotDeleteWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountView profile = ObjectFactory.CreateAccountView();
            profile.Password += "1";
            service.CanDelete(profile);

            String expected = Validations.IncorrectPassword;
            String actual = service.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanDelete_CanDeleteValidAccountView()
        {
            Assert.IsTrue(service.CanDelete(ObjectFactory.CreateAccountView()));
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