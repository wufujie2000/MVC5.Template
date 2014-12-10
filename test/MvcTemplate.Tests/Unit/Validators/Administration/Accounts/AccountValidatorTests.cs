using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Tests.Data;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Validators
{
    [TestFixture]
    public class AccountValidatorTests
    {
        private AccountValidator validator;
        private TestingContext context;
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            hasher = Substitute.For<IHasher>();
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(true);

            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            validator = new AccountValidator(new UnitOfWork(context), hasher);
            validator.ModelState = new ModelStateDictionary();

            TearDownData();
            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
            validator.Dispose();
            context.Dispose();
        }

        #region Method: CanRecover(AccountRecoveryView view)

        [Test]
        public void CanRecover_CanNotRecoverAccountWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        [Test]
        public void CanRecover_CanRecoverValidAccount()
        {
            Assert.IsTrue(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanReset(AccountResetView view)

        [Test]
        public void CanReset_CanNotResetAccountWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(validator.CanReset(ObjectFactory.CreateAccountResetView()));
        }

        [Test]
        public void CanReset_CanNotResetAccountWithExpiredToken()
        {
            Account account = context.Set<Account>().Single();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(-5);
            context.SaveChanges();

            Assert.IsFalse(validator.CanReset(ObjectFactory.CreateAccountResetView()));
        }

        [Test]
        public void CanReset_AddsErorrMessageThenCanNotResetAccountWithExpiredToken()
        {
            Account account = context.Set<Account>().Single();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(-5);
            context.SaveChanges();

            validator.CanReset(ObjectFactory.CreateAccountResetView());

            String expected = Validations.RecoveryTokenExpired;
            String actual = validator.Alerts.Single().Message;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanReset_CanResetValidAccount()
        {
            Assert.IsTrue(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanLogin(AccountLoginView view)

        [Test]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Key", "ErrorMesages");

            Assert.IsFalse(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        [Test]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            AccountLoginView account = new AccountLoginView();

            Assert.IsFalse(validator.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithNotExistingAccount()
        {
            AccountLoginView account = new AccountLoginView();
            validator.CanLogin(account);

            String actual = validator.ModelState[""].Errors[0].ErrorMessage;
            String expected = Validations.IncorrectUsernameOrPassword;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();

            Assert.IsFalse(validator.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithIncorrectPassword()
        {
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            account.Password += "Incorrect";
            validator.CanLogin(account);

            String actual = validator.ModelState[""].Errors[0].ErrorMessage;
            String expected = Validations.IncorrectUsernameOrPassword;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            account.Username = account.Username.ToUpper();

            Assert.IsTrue(validator.CanLogin(account));
        }

        [Test]
        public void CanLogin_CanLoginWithValidAccount()
        {
            Assert.IsTrue(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        #endregion

        #region Method: CanRegister(AccountView view)

        [Test]
        public void CanRegister_CanNotRegisterWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Key", "Error");

            Assert.IsFalse(validator.CanRegister(ObjectFactory.CreateAccountView()));
        }

        [Test]
        public void CanRegister_CanNotRegisterWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "DifferentValue";

            Assert.IsFalse(validator.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErorrMessageThenCanNotRegisterWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "DifferentValue";
            validator.CanRegister(account);

            String actual = validator.ModelState["Username"].Errors[0].ErrorMessage;
            String expected = Validations.UsernameIsAlreadyTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanRegister_CanNotRegisterWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Id += "DifferentValue";

            Assert.IsFalse(validator.CanRegister(account));
        }

        [Test]
        public void CanRegister_AddsErrorMessageThenCanNotRegisterWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Id += "DifferentValue";
            validator.CanRegister(account);

            String actual = validator.ModelState["Email"].Errors[0].ErrorMessage;
            String expected = Validations.EmailIsAlreadyUsed;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanRegister_CanRegisterValidAccount()
        {
            Assert.IsTrue(validator.CanRegister(ObjectFactory.CreateAccountView("2")));
        }

        #endregion

        #region Method: CanEdit(ProfileEditView view)

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Key", "ErrorMessages");

            Assert.IsFalse(validator.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        [Test]
        public void CanEdit_CanNotEditWithIncorrectPassword()
        {
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Password += "1";

            Assert.IsFalse(validator.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditWithIncorrectPassword()
        {
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Password += "1";
            validator.CanEdit(profile);

            String expected = Validations.IncorrectPassword;
            String actual = validator.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            Account takenAccount = ObjectFactory.CreateAccount("2");
            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = takenAccount.Username;

            Assert.IsFalse(validator.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditToAlreadyTakenUsername()
        {
            Account takenAccount = ObjectFactory.CreateAccount("2");
            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = takenAccount.Username;
            validator.CanEdit(profile);

            String actual = validator.ModelState["Username"].Errors[0].ErrorMessage;
            String expected = Validations.UsernameIsAlreadyTaken;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditUsingItsOwnUsername()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = profile.Username.ToUpper();

            Assert.IsTrue(validator.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyUsedEmail()
        {
            Account usedEmailAccount = ObjectFactory.CreateAccount("2");
            context.Set<Account>().Add(usedEmailAccount);

            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = usedEmailAccount.Email;

            Assert.IsFalse(validator.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErorrMessageThenCanNotEditToAlreadyUsedEmail()
        {
            Account usedEmailAccount = ObjectFactory.CreateAccount("2");
            context.Set<Account>().Add(usedEmailAccount);

            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = usedEmailAccount.Email;
            validator.CanEdit(profile);

            String actual = validator.ModelState["Email"].Errors[0].ErrorMessage;
            String expected = Validations.EmailIsAlreadyUsed;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditUsingItsOwnEmail()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = profile.Email.ToUpper();

            Assert.IsTrue(validator.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanEditValidProfile()
        {
            Assert.IsTrue(validator.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        #endregion

        #region Method: CanEdit(AccountEditView view)

        [Test]
        public void CanEdit_CanNotEditAccountWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        [Test]
        public void CanEdit_CanEditValidAccount()
        {
            Assert.IsTrue(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        #endregion

        #region Method: CanDelete(ProfileDeleteView view)

        [Test]
        public void CanEdit_CanNotDeleteWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
        }

        [Test]
        public void CanDelete_CanNotDeleteWithIncorrectPassword()
        {
            ProfileDeleteView profile = ObjectFactory.CreateProfileDeleteView();
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            profile.Password += "1";

            Assert.IsFalse(validator.CanDelete(profile));
        }

        [Test]
        public void CanDelete_AddsErrorMessageThenCanNotDeleteWithIncorrectPassword()
        {
            ProfileDeleteView profile = ObjectFactory.CreateProfileDeleteView();
            hasher.Verify(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            profile.Password += "1";

            validator.CanDelete(profile);

            String expected = Validations.IncorrectPassword;
            String actual = validator.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanDelete_CanDeleteValidProfileDeleteView()
        {
            Assert.IsTrue(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            Account account = ObjectFactory.CreateAccount();
            account.Role = ObjectFactory.CreateRole();
            account.RoleId = account.Role.Id;

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
