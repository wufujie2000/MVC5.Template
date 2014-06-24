using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Services;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Services
{
    [TestFixture]
    public class AccountsServiceTests
    {
        private AccountsService service;
        private String accountId;
        private Context context;
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            Mock<IHasher> hasherMock = new Mock<IHasher>(MockBehavior.Strict);
            hasherMock.Setup(mock => mock.HashPassword(It.IsAny<String>())).Returns("Hashed");
            service = new AccountsService(new UnitOfWork(context), hasherMock.Object);
            service.ModelState = new ModelStateDictionary();
            hasher = hasherMock.Object;

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
            context.Dispose();
        }

        #region Method: CanCreate(AccountView view)

        [Test]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanCreate(new AccountView()));
        }

        [Test]
        public void CanCreate_CanNotCreateWithNullUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = null;

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateWithNullUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = null;

            service.CanCreate(account);

            String expected = String.Format(Template.Resources.Shared.Validations.FieldIsRequired, Titles.Username);
            String actual = service.ModelState["Username"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateWithEmptyUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = String.Empty;

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateWithEmptyUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = String.Empty;

            service.CanCreate(account);

            String expected = String.Format(Template.Resources.Shared.Validations.FieldIsRequired, Titles.Username);
            String actual = service.ModelState["Username"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "1";

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "OtherIdValue";
            service.CanCreate(account);

            String expected = Validations.UsernameIsAlreadyTaken;
            String actual = service.ModelState["Username"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordIsNull()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = null;

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateIfPasswordIsNull()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = null;

            service.CanCreate(account);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordIsTooShort()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "AaaAaa1";

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateIfPasswordIsTooShort()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "AaaAaa1";

            service.CanCreate(account);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordDoesNotContainUpperLetter()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "aaaaaaaaaaaa1";

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateIfPasswordDoesNotContainUpperLetter()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "aaaaaaaaaaaa1";

            service.CanCreate(account);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordDoesNotContainLowerLetter()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "AAAAAAAAAAA1";

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateIfPasswordDoesNotContainLowerLetter()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "AAAAAAAAAAA1";

            service.CanCreate(account);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordDoesNotContainADigit()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "AaAaAaAaAaAa";

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateIfPasswordDoesNotContainADigit()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Password = "AaAaAaAaAaAa";

            service.CanCreate(account);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateWithNullEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Email = null;

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateWithNullEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Email = null;

            service.CanCreate(account);

            String expected = String.Format(Template.Resources.Shared.Validations.FieldIsRequired, Titles.Email);
            String actual = service.ModelState["Email"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanNotCreateWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();

            Assert.IsFalse(service.CanCreate(account));
        }

        [Test]
        public void CanCreate_AddsErorrMessageThenCanNotCreateWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            service.CanCreate(account);

            String expected = Validations.EmailIsAlreadyUsed;
            String actual = service.ModelState["Email"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanCreate_CanCreateValidAccount()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Email += "s";

            Assert.IsTrue(service.CanCreate(account));
        }

        #endregion

        #region Method: Create(AccountView view)

        [Test]
        public void Create_CreatesAccount()
        {
            TearDownData();

            AccountView expected = ObjectFactory.CreateAccountView();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Create(expected);

            Account actual = context.Set<Account>().SingleOrDefault(account => account.Id == expected.Id);

            Assert.AreEqual(hasher.HashPassword(expected.Password), actual.Passhash);
            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Email, actual.Email);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Edit(AccountView view)

        [Test]
        public void Edit_EditsAccount()
        {
            Role role = ObjectFactory.CreateRole(2);
            context.Set<Role>().Add(role);
            context.SaveChanges();

            AccountView expected = service.GetView(accountId);
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
            AccountView account = service.GetView(accountId);
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
            AccountView account = service.GetView(accountId);
            account.Password += "Edition";
            service.Edit(account);

            String actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id).Passhash;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Edit_DoesNotEditAccountsEmail()
        {
            AccountView account = service.GetView(accountId);
            String expected = account.Email;

            account.Email = "Edit_DoesNotEditAccountsEmail@tests.com";
            service.Edit(account);

            String actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id).Email;

            Assert.AreEqual(expected, actual);
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