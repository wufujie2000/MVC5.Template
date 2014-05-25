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
        private Context context;
        private Account account;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new AccountsService(new UnitOfWork(context));
            service.ModelState = new ModelStateDictionary();

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
        public void CanCreate_CanCreateValidUser()
        {
            Assert.IsTrue(service.CanCreate(ObjectFactory.CreateAccountView()));
        }

        #endregion

        #region Method: CanEdit(AccountView view)

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanEdit(ObjectFactory.CreateAccountView()));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "1";

            Assert.IsFalse(service.CanEdit(account));
        }

        [Test]
        public void CanEdit_AddsErorrMessageThenCanNotEditToAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "OtherIdValue";
            service.CanEdit(account);

            String expected = Validations.UsernameIsAlreadyTaken;
            String actual = service.ModelState["Username"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditValidUser()
        {
            Assert.IsTrue(service.CanEdit(ObjectFactory.CreateAccountView()));
        }

        #endregion

        #region Method: Create(AccountView view)

        [Test]
        public void Create_CreatesAccountWithTrimmedUsername()
        {
            TearDownData();

            AccountView expected = ObjectFactory.CreateAccountView();
            expected.Username = String.Format("  {0}  ", expected.Username);
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Create(expected);

            Account actual = context.Set<Account>().Find(expected.Id);

            Assert.IsTrue(BCrypter.Verify(expected.Password, actual.Passhash));
            Assert.AreEqual(expected.Username.Trim(), actual.Username);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Edit(AccountView view)

        [Test]
        public void Edit_EditsAccountWithTrimmedUsername()
        {
            Role role = ObjectFactory.CreateRole(2);
            context.Set<Role>().Add(role);
            context.SaveChanges();

            AccountView expected = service.GetView(account.Id);
            expected.Username = String.Format("  1{0}1  ", expected.Username);
            expected.RoleId = role.Id;
            service.Edit(expected);

            context = new TestingContext();
            Account actual = context.Set<Account>().Find(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
            Assert.AreEqual(expected.Username.Trim(), actual.Username);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
            account.Role = ObjectFactory.CreateRole();
            account.RoleId = account.Role.Id;

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