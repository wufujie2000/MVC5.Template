using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.ProfileView;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
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
            var httpMock = new HttpMock();
            context = new TestingContext();
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
        public void CanEdit_CanEditUsingItsOwnUsername()
        {
            var profile = ObjectFactory.CreateProfileView();
            profile.Username = account.Username.ToUpper();

            Assert.IsTrue(service.CanEdit(profile));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            var takenAccount = ObjectFactory.CreateAccount();
            takenAccount.Person = ObjectFactory.CreatePerson();
            takenAccount.Person.Id = takenAccount.Person.Id + "1";
            takenAccount.PersonId = takenAccount.Person.Id + "1";
            takenAccount.Username += "1";
            takenAccount.Id += "1";

            context.Set<Account>().Add(takenAccount);
            context.SaveChanges();

            var profile = ObjectFactory.CreateProfileView();
            profile.Username = takenAccount.Username;

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["Username"].Errors[0].ErrorMessage, Validations.UsernameIsAlreadyTaken);
        }

        [Test]
        public void CanEdit_CanNotEditWithIncorrectPassword()
        {
            var profile = ObjectFactory.CreateProfileView();
            profile.CurrentPassword += "1";

            Assert.IsFalse(service.CanEdit(profile));
            Assert.AreEqual(service.ModelState["CurrentPassword"].Errors[0].ErrorMessage, Validations.IncorrectPassword);
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
            var profile = ObjectFactory.CreateProfileView();
            profile.Username = String.Empty;

            Assert.IsFalse(service.CanDelete(profile));
            Assert.AreEqual(service.ModelState["Username"].Errors[0].ErrorMessage, Validations.IncorrectUsername);
        }

        [Test]
        public void CanDelete_CanNotDeleteWithIncorrectPassword()
        {
            var profile = ObjectFactory.CreateProfileView();
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
            var profileView = ObjectFactory.CreateProfileView();
            var expected = context.Set<Account>().Find(profileView.Id);
            profileView.Username += "1";
            service.Edit(profileView);

            context = new TestingContext();
            var actual = context.Set<Account>().Find(profileView.Id);

            Assert.AreEqual(expected.PersonId, actual.PersonId);
            Assert.AreEqual(profileView.Username, actual.Username);
            Assert.IsTrue(BCrypter.Verify(profileView.NewPassword, actual.Passhash));
        }

        [Test]
        public void Edit_EditsPerson()
        {
            var profileView = ObjectFactory.CreateProfileView();
            profileView.Person.DateOfBirth = null;
            profileView.Person.FirstName += "1";
            profileView.Person.LastName += "1";
            service.Edit(profileView);

            context = new TestingContext();
            var expected = profileView.Person;
            var actual = context.Set<Person>().Find(profileView.Id);

            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
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
            var disclaimer = service.AlertMessages.First();

            Assert.AreEqual(disclaimer.Message, Messages.ProfileDeleteDisclaimer);
            Assert.AreEqual(disclaimer.Type, AlertMessageType.Danger);
            Assert.AreEqual(disclaimer.Key, String.Empty);
            Assert.AreEqual(disclaimer.FadeOutAfter, 0);
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
            account.Person = ObjectFactory.CreatePerson();
            account.PersonId = account.Person.Id;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            foreach (var person in context.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<Person>().Remove(person);

            context.SaveChanges();
        }

        #endregion
    }
}
