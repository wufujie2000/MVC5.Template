using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Security;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.ProfileView;
using Template.Services;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Services
{
    [TestFixture]
    public class ProfileServiceTests
    {
        private ProfileService service;
        private AContext context;
        private Account account;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            HttpMock httpMock = new HttpMock();
            HttpContext.Current = httpMock.HttpContext;
            service = new ProfileService(new UnitOfWork(context));

            service.ModelState = new ModelStateDictionary();
            service.AlertMessages = new MessagesContainer(service.ModelState);
            httpMock.IdentityMock.Setup(mock => mock.Name).Returns(() => account.Id);

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

        #region Method: CanEdit(ProfileView profile)

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            service.ModelState.AddModelError("Key", "ErrorMessages");

            Assert.IsFalse(service.CanEdit(ObjectFactory.CreateProfileView()));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            Account takenAccount = ObjectFactory.CreateAccount();
            takenAccount.Person = ObjectFactory.CreatePerson();
            takenAccount.Person.Id = takenAccount.Person.Id + "1";
            takenAccount.PersonId = takenAccount.Person.Id + "1";
            takenAccount.Username += "1";
            takenAccount.Id += "1";

            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.Username = takenAccount.Username;

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["Username"].Errors[0].ErrorMessage, Validations.UsernameIsAlreadyTaken);
        }

        [Test]
        public void CanEdit_CanNotEditWithIncorrectPassword()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.CurrentPassword += "1";

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["CurrentPassword"].Errors[0].ErrorMessage, Validations.IncorrectPassword);
        }

        [Test]
        public void CanEdit_CanNotEditIfNewPasswordIsTooShort()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.NewPassword = "AaaAaa1";

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["NewPassword"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanEdit_CanNotEditIfNewPasswordDoesNotContainUpperLetter()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.NewPassword = "aaaaaaaaaaaa1";

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["NewPassword"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanEdit_CanNotEditIfNewPasswordDoesNotContainLowerLetter()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.NewPassword = "AAAAAAAAAAA1";

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["NewPassword"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanEdit_CanNotEditIfNewPasswordDoesNotContainADigit()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.NewPassword = "AaAaAaAaAaAa";

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["NewPassword"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanEdit_CanEditUsingItsOwnUsername()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.Username = account.Username.ToUpper();

            Assert.IsTrue(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanEditWithoutSpecifyingNewPassword()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.NewPassword = null;

            Assert.IsTrue(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanEditValidProfile()
        {
            Assert.IsTrue(service.CanEdit(ObjectFactory.CreateProfileView()));
        }

        #endregion

        #region Method: CanDelete(ProfileView profile)

        [Test]
        public void CanDelete_CanNotDeleteWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanDelete(ObjectFactory.CreateProfileView()));
        }

        [Test]
        public void CanDelete_CanNotDeleteWithIncorrectUsername()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.Username = String.Empty;

            Assert.IsFalse(service.CanDelete(profile));
            Assert.AreEqual(service.ModelState["Username"].Errors[0].ErrorMessage, Validations.IncorrectUsername);
        }

        [Test]
        public void CanDelete_CanNotDeleteWithIncorrectPassword()
        {
            ProfileView profile = ObjectFactory.CreateProfileView();
            profile.CurrentPassword += "1";

            Assert.IsFalse(service.CanDelete(profile));
            Assert.AreEqual(service.ModelState["CurrentPassword"].Errors[0].ErrorMessage, Validations.IncorrectPassword);
        }

        [Test]
        public void CanDelete_CanDeleteValidProfileView()
        {
            Assert.IsTrue(service.CanDelete(ObjectFactory.CreateProfileView()));
        }

        #endregion

        #region Method: Edit(ProfileView profile)

        [Test]
        public void Edit_EditsAccount()
        {
            ProfileView profileView = ObjectFactory.CreateProfileView();
            Account expected = context.Set<Account>().Find(account.Id);
            profileView.Username += "1";
            service.Edit(profileView);

            context = new TestingContext();
            Account actual = context.Set<Account>().Find(account.Id);

            Assert.AreEqual(expected.PersonId, actual.PersonId);
            Assert.AreEqual(profileView.Username, actual.Username);
            Assert.IsTrue(BCrypter.Verify(profileView.NewPassword, actual.Passhash));
        }

        [Test]
        public void Edit_LeavesCurrentPassword()
        {
            ProfileView profileView = ObjectFactory.CreateProfileView();
            profileView.NewPassword = null;
            service.Edit(profileView);

            context = new TestingContext();
            Account actual = context.Set<Account>().Find(account.Id);

            Assert.IsTrue(BCrypter.Verify(profileView.CurrentPassword, actual.Passhash));
        }

        [Test]
        public void Edit_EditsPerson()
        {
            String expectedRoleId = account.Person.RoleId;
            ProfileView profileView = ObjectFactory.CreateProfileView();
            profileView.Person.DateOfBirth = null;
            profileView.Person.FirstName += "1";
            profileView.Person.LastName += "1";
            service.Edit(profileView);

            context = new TestingContext();
            PersonView expected = profileView.Person;
            Person actual = context.Set<Person>().Find(account.PersonId);

            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expectedRoleId, actual.RoleId);
        }

        #endregion

        #region Method: Delete(String id)
        
        [Test]
        public void Delete_DeletesAccount()
        {
            if (context.Set<Account>().Find(account.Id) == null)
                Assert.Inconclusive();

            service.Delete(account.Id);
            context = new TestingContext();

            Assert.IsNull(context.Set<Account>().Find(account.Id));
        }

        [Test]
        public void Delete_DeletesUser()
        {
            if (context.Set<Person>().Find(account.PersonId) == null)
                Assert.Inconclusive();

            service.Delete(account.PersonId);
            context = new TestingContext();

            Assert.IsNull(context.Set<Person>().Find(account.PersonId));
        }

        #endregion

        #region Method: AddDeleteDisclaimerMessage()

        [Test]
        public void AddDeleteDisclaimerMessage_AddsDisclaimer()
        {
            service.AddDeleteDisclaimerMessage();
            AlertMessage disclaimer = service.AlertMessages.First();

            Assert.AreEqual(disclaimer.Message, Messages.ProfileDeleteDisclaimer);
            Assert.AreEqual(disclaimer.Type, AlertMessageType.Danger);
            Assert.AreEqual(disclaimer.Key, String.Empty);
            Assert.AreEqual(disclaimer.FadeOutAfter, 0);
        }

        #endregion

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

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
            account.Person = ObjectFactory.CreatePerson();
            account.Person.Role = ObjectFactory.CreateRole();
            account.Person.RoleId = account.Person.Role.Id;
            account.PersonId = account.Person.Id;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            foreach (Person person in context.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Person>().Remove(person);
            foreach (Role role in context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Role>().Remove(role);

            context.SaveChanges();
        }

        #endregion
    }
}
