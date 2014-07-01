using Moq;
using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Validators;
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
        private Mock<IHasher> hasherMock;
        private Context context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(true);

            validator = new AccountValidator(new UnitOfWork(context), hasherMock.Object);
            validator.ModelState = new ModelStateDictionary();
            HttpContext.Current = new HttpMock().HttpContext;

            TearDownData();
            SetUpData();
        }

        public void TearDown()
        {
            context.Dispose();
            validator.Dispose();
            HttpContext.Current = null;
        }

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
            account.Username = String.Empty;

            Assert.IsFalse(validator.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithNotExistingAccount()
        {
            AccountLoginView account = new AccountLoginView();
            account.Username = String.Empty;
            validator.CanLogin(account);

            String actual = validator.ModelState[String.Empty].Errors[0].ErrorMessage;
            String expected = Validations.IncorrectUsernameOrPassword;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();

            Assert.IsFalse(validator.CanLogin(account));
        }

        [Test]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            account.Password += "Incorrect";
            validator.CanLogin(account);

            String actual = validator.ModelState[String.Empty].Errors[0].ErrorMessage;
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
            AccountView account = ObjectFactory.CreateAccountView(1);
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
            Assert.IsTrue(validator.CanRegister(ObjectFactory.CreateAccountView(2)));
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
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Password += "1";

            Assert.IsFalse(validator.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
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
            Account takenAccount = ObjectFactory.CreateAccount();
            takenAccount.Username += "1";
            takenAccount.Id += "1";

            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = takenAccount.Username;

            Assert.IsFalse(validator.CanEdit(profile));
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
            validator.CanEdit(profile);

            String expected = Validations.UsernameIsAlreadyTaken;
            String actual = validator.ModelState["Username"].Errors[0].ErrorMessage;

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
        public void CanEdit_CanEditWithoutSpecifyingNewPassword()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = null;

            Assert.IsTrue(validator.CanEdit(profile));
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

            Assert.IsFalse(validator.CanEdit(profile));
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
            validator.CanEdit(profile);

            String expected = Validations.EmailIsAlreadyUsed;
            String actual = validator.ModelState["Email"].Errors[0].ErrorMessage;

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

            Assert.IsFalse(validator.CanEdit(new AccountEditView()));
        }

        [Test]
        public void CanEdit_CanEditValidAccount()
        {
            Assert.IsTrue(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        #endregion

        #region Method: CanDelete(AccountView view)

        [Test]
        public void CanDelete_CanNotDeleteWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountView profile = ObjectFactory.CreateAccountView();
            profile.Password += "1";

            Assert.IsFalse(validator.CanDelete(profile));
        }

        [Test]
        public void CanDelete_AddsErrorMessageThenCanNotDeleteWithIncorrectPassword()
        {
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(false);
            AccountView profile = ObjectFactory.CreateAccountView();
            profile.Password += "1";
            validator.CanDelete(profile);

            String expected = Validations.IncorrectPassword;
            String actual = validator.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanDelete_CanDeleteValidAccountView()
        {
            Assert.IsTrue(validator.CanDelete(ObjectFactory.CreateAccountView()));
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
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Role>().RemoveRange(context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
