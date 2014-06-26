using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
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
    public class ProfileServiceTests
    {
        private Mock<IHasher> hasherMock;
        private ProfileService service;
        private AContext context;
        private Account account;
        private IHasher hasher;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            HttpMock httpMock = new HttpMock();
            HttpContext.Current = httpMock.HttpContext;
            hasherMock = new Mock<IHasher>(MockBehavior.Strict);
            hasherMock.Setup(mock => mock.Verify(It.IsAny<String>(), It.IsAny<String>())).Returns(true);
            hasherMock.Setup(mock => mock.HashPassword(It.IsAny<String>())).Returns("Hashed");
            service = new ProfileService(new UnitOfWork(context), hasherMock.Object);
            httpMock.IdentityMock.Setup(mock => mock.Name).Returns(() => account.Id);
            hasher = hasherMock.Object;

            service.ModelState = new ModelStateDictionary();
            service.AlertMessages = new MessagesContainer();

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

        #region Method: AccountExists(String accountId)

        [Test]
        public void AccountExists_ReturnsTrueIfAccountExistsInDatabase()
        {
            Assert.IsTrue(service.AccountExists(account.Id));
        }

        [Test]
        public void AccountExists_ReturnsFalseIfAccountDoesNotExistInDatabase()
        {
            Assert.IsFalse(service.AccountExists("Test"));
        }

        #endregion

        #region Method: CanEdit(ProfileEditView profile)

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
        public void CanEdit_CanNotEditIfNewPasswordIsTooShort()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "AaaAaa1";

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["NewPassword"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditIfNewPasswordIsTooShort()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "AaaAaa1";
            service.CanEdit(profile);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["NewPassword"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanNotEditIfNewPasswordDoesNotContainUpperLetter()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "aaaaaaaaaaaa1";

            Assert.IsFalse(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditIfNewPasswordDoesNotContainUpperLetter()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "aaaaaaaaaaaa1";
            service.CanEdit(profile);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["NewPassword"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanNotEditIfNewPasswordDoesNotContainLowerLetter()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "AAAAAAAAAAA1";

            Assert.IsFalse(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditIfNewPasswordDoesNotContainLowerLetter()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "AAAAAAAAAAA1";
            service.CanEdit(profile);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["NewPassword"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanNotEditIfNewPasswordDoesNotContainADigit()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "AaAaAaAaAaAa";

            Assert.IsFalse(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErrorMessageThenCanNotEditIfNewPasswordDoesNotContainADigit()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = "AaAaAaAaAaAa";
            service.CanEdit(profile);

            String expected = Validations.IllegalPassword;
            String actual = service.ModelState["NewPassword"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanEdit_CanEditWithoutSpecifyingNewPassword()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.NewPassword = null;

            Assert.IsTrue(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanNotEditWithNullEmail()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = null;

            Assert.IsFalse(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_AddsErorrMessageThenCanNotEditWithNullEmail()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Email = null;

            service.CanEdit(profile);

            String expected = String.Format(Template.Resources.Shared.Validations.FieldIsRequired, Titles.Email);
            String actual = service.ModelState["Email"].Errors[0].ErrorMessage;

            Assert.AreEqual(expected, actual);
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

        #region Method: CanDelete(AccountView profile)

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

        #region Method: Edit(ProfileEditView profile)

        [Test]
        public void Edit_EditsAccount()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            Account expected = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id);
            profile.Username += "1";
            service.Edit(profile);

            Account actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id);

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

            Account actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id);

            Assert.IsTrue(hasher.Verify(profile.Password, actual.Passhash));
        }

        [Test]
        public void Edit_LeavesCurrentRoleAfterEditing()
        {
            ProfileEditView profile = ObjectFactory.CreateProfileEditView();
            profile.Username += "New username";
            service.Edit(profile);

            String actual = context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id).RoleId;
            String expected = account.RoleId;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesAccount()
        {
            if (context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id) == null)
                Assert.Inconclusive();

            service.Delete(account.Id);

            Assert.IsNull(context.Set<Account>().SingleOrDefault(acc => acc.Id == account.Id));
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
            context.Set<Account>().RemoveRange(context.Set<Account>().Where(account => account.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Role>().RemoveRange(context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
