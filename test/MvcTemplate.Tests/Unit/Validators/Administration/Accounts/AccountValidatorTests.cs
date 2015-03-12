using MvcTemplate.Components.Security;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Tests.Data;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Validators
{
    public class AccountValidatorTests : IDisposable
    {
        private AccountValidator validator;
        private TestingContext context;
        private IHasher hasher;

        public AccountValidatorTests()
        {
            context = new TestingContext();
            hasher = Substitute.For<IHasher>();
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(true);

            validator = new AccountValidator(new UnitOfWork(context), hasher);
            validator.ModelState = new ModelStateDictionary();

            TearDownData();
            SetUpData();
        }
        public void Dispose()
        {
            validator.Dispose();
            context.Dispose();
        }

        #region Method: CanRecover(AccountRecoveryView view)

        [Fact]
        public void CanRecover_CanNotRecoverAccountWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        [Fact]
        public void CanRecover_CanRecoverValidAccount()
        {
            Assert.True(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanReset(AccountResetView view)

        [Fact]
        public void CanReset_CanNotResetAccountWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanReset(ObjectFactory.CreateAccountResetView()));
        }

        [Fact]
        public void CanReset_CanNotResetAccountWithExpiredToken()
        {
            Account account = context.Set<Account>().Single();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(-5);
            context.SaveChanges();

            Assert.False(validator.CanReset(ObjectFactory.CreateAccountResetView()));
        }

        [Fact]
        public void CanReset_AddsErorrMessageThenCanNotResetAccountWithExpiredToken()
        {
            Account account = context.Set<Account>().Single();
            account.RecoveryTokenExpirationDate = DateTime.Now.AddMinutes(-5);
            context.SaveChanges();

            validator.CanReset(ObjectFactory.CreateAccountResetView());

            String expected = Validations.RecoveryTokenExpired;
            String actual = validator.Alerts.Single().Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanReset_CanResetValidAccount()
        {
            Assert.True(validator.CanRecover(ObjectFactory.CreateAccountRecoveryView()));
        }

        #endregion

        #region Method: CanLogin(AccountLoginView view)

        [Fact]
        public void CanLogin_CanNotLoginWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Key", "ErrorMesages");

            Assert.False(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        [Fact]
        public void CanLogin_CanNotLoginFromNonExistingAccount()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            AccountLoginView account = new AccountLoginView();

            Assert.False(validator.CanLogin(account));
        }

        [Fact]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithNotExistingAccount()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            AccountLoginView account = new AccountLoginView();

            validator.CanLogin(account);

            String actual = validator.ModelState[""].Errors[0].ErrorMessage;
            String expected = Validations.IncorrectUsernameOrPassword;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanLogin_CanNotLoginWithIncorrectPassword()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();

            Assert.False(validator.CanLogin(account));
        }

        [Fact]
        public void CanLogin_AddsErrorMessageThenCanNotLoginWithIncorrectPassword()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            account.Password += "Incorrect";

            validator.CanLogin(account);

            String actual = validator.ModelState[""].Errors[0].ErrorMessage;
            String expected = Validations.IncorrectUsernameOrPassword;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanLogin_CanLoginWithCaseInsensitiveUsername()
        {
            AccountLoginView account = ObjectFactory.CreateAccountLoginView();
            account.Username = account.Username.ToUpper();

            Assert.True(validator.CanLogin(account));
        }

        [Fact]
        public void CanLogin_CanLoginWithValidAccount()
        {
            Assert.True(validator.CanLogin(ObjectFactory.CreateAccountLoginView()));
        }

        #endregion

        #region Method: CanRegister(AccountView view)

        [Fact]
        public void CanRegister_CanNotRegisterWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Key", "Error");

            Assert.False(validator.CanRegister(ObjectFactory.CreateAccountView()));
        }

        [Fact]
        public void CanRegister_CanNotRegisterWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "DifferentValue";

            Assert.False(validator.CanRegister(account));
        }

        [Fact]
        public void CanRegister_AddsErorrMessageThenCanNotRegisterWithAlreadyTakenUsername()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Username = account.Username.ToLower();
            account.Id += "DifferentValue";
            validator.CanRegister(account);

            String actual = validator.ModelState["Username"].Errors[0].ErrorMessage;
            String expected = Validations.UsernameIsAlreadyTaken;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRegister_CanNotRegisterWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Id += "DifferentValue";

            Assert.False(validator.CanRegister(account));
        }

        [Fact]
        public void CanRegister_AddsErrorMessageThenCanNotRegisterWithAlreadyUsedEmail()
        {
            AccountView account = ObjectFactory.CreateAccountView();
            account.Id += "DifferentValue";
            validator.CanRegister(account);

            String actual = validator.ModelState["Email"].Errors[0].ErrorMessage;
            String expected = Validations.EmailIsAlreadyUsed;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRegister_CanRegisterValidAccount()
        {
            Assert.True(validator.CanRegister(ObjectFactory.CreateAccountView("2")));
        }

        #endregion

        #region Method: CanEdit(ProfileEditView view)

        [Fact]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Key", "ErrorMessages");

            Assert.False(validator.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        [Fact]
        public void CanEdit_CanNotEditWithIncorrectPassword()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Password += "1";

            Assert.False(validator.CanEdit(profile));
        }

        [Fact]
        public void CanEdit_AddsErrorMessageThenCanNotEditWithIncorrectPassword()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Password += "1";
            validator.CanEdit(profile);

            String expected = Validations.IncorrectPassword;
            String actual = validator.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            Account takenAccount = ObjectFactory.CreateAccount("2");
            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = takenAccount.Username;

            Assert.False(validator.CanEdit(profile));
        }

        [Fact]
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

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEdit_CanEditUsingItsOwnUsername()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username = profile.Username.ToUpper();

            Assert.True(validator.CanEdit(profile));
        }

        [Fact]
        public void CanEdit_CanNotEditToAlreadyUsedEmail()
        {
            Account usedEmailAccount = ObjectFactory.CreateAccount("2");
            context.Set<Account>().Add(usedEmailAccount);

            context.SaveChanges();

            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = usedEmailAccount.Email;

            Assert.False(validator.CanEdit(profile));
        }

        [Fact]
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

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEdit_CanEditUsingItsOwnEmail()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = profile.Email.ToUpper();

            Assert.True(validator.CanEdit(profile));
        }

        [Fact]
        public void CanEdit_CanEditValidProfile()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateProfileEditView()));
        }

        #endregion

        #region Method: CanEdit(AccountEditView view)

        [Fact]
        public void CanEdit_CanNotEditAccountWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        [Fact]
        public void CanEdit_CanEditValidAccount()
        {
            Assert.True(validator.CanEdit(ObjectFactory.CreateAccountEditView()));
        }

        #endregion

        #region Method: CanDelete(ProfileDeleteView view)

        [Fact]
        public void CanEdit_CanNotDeleteWithInvalidModelState()
        {
            validator.ModelState.AddModelError("Test", "Test");

            Assert.False(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
        }

        [Fact]
        public void CanDelete_CanNotDeleteWithIncorrectPassword()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            ProfileDeleteView profile = ObjectFactory.CreateProfileDeleteView();
            profile.Password += "1";

            Assert.False(validator.CanDelete(profile));
        }

        [Fact]
        public void CanDelete_AddsErrorMessageThenCanNotDeleteWithIncorrectPassword()
        {
            hasher.VerifyPassword(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
            ProfileDeleteView profile = ObjectFactory.CreateProfileDeleteView();
            profile.Password += "1";

            validator.CanDelete(profile);

            String expected = Validations.IncorrectPassword;
            String actual = validator.ModelState["Password"].Errors[0].ErrorMessage;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanDelete_CanDeleteValidProfileDeleteView()
        {
            Assert.True(validator.CanDelete(ObjectFactory.CreateProfileDeleteView()));
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
            context.Set<RolePrivilege>().RemoveRange(context.Set<RolePrivilege>());
            context.Set<Account>().RemoveRange(context.Set<Account>());
            context.Set<Role>().RemoveRange(context.Set<Role>());
            context.SaveChanges();
        }

        #endregion
    }
}
