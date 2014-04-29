using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.UserView;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Components.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UsersService service;
        private Context context;
        private Account account;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new UsersService(new UnitOfWork(context));
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

        #region Method: CanCreate(UserView view)

        [Test]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanCreate(ObjectFactory.CreateUserView()));
        }

        [Test]
        public void CanCreate_CanNotCreateWithAlreadyTakenUsername()
        {
            UserView userView = ObjectFactory.CreateUserView();
            userView.Username = account.Username.ToLower();
            userView.Id += "1";

            Assert.IsFalse(service.CanCreate(userView));
            Assert.AreEqual(service.ModelState["Username"].Errors[0].ErrorMessage, Validations.UsernameIsAlreadyTaken);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordIsTooShort()
        {
            UserView userView = ObjectFactory.CreateUserView();
            userView.Password = "AaaAaa1";

            Assert.IsFalse(service.CanCreate(userView));
            Assert.AreEqual(service.ModelState["Password"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordDoesNotContainUpperLetter()
        {
            UserView userView = ObjectFactory.CreateUserView();
            userView.Password = "aaaaaaaaaaaa1";

            Assert.IsFalse(service.CanCreate(userView));
            Assert.AreEqual(service.ModelState["Password"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordDoesNotContainLowerLetter()
        {
            UserView userView = ObjectFactory.CreateUserView();
            userView.Password = "AAAAAAAAAAA1";

            Assert.IsFalse(service.CanCreate(userView));
            Assert.AreEqual(service.ModelState["Password"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordDoesNotContainADigit()
        {
            UserView userView = ObjectFactory.CreateUserView();
            userView.Password = "AaAaAaAaAaAa";

            Assert.IsFalse(service.CanCreate(userView));
            Assert.AreEqual(service.ModelState["Password"].Errors[0].ErrorMessage, Validations.IllegalPassword);
        }

        [Test]
        public void CanCreate_CanCreateValidUser()
        {
            Assert.IsTrue(service.CanCreate(ObjectFactory.CreateUserView()));
        }

        #endregion

        #region Method: CanEdit(UserView view)

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            service.ModelState.AddModelError("Test", "Test");

            Assert.IsFalse(service.CanEdit(ObjectFactory.CreateUserView()));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            UserView userView = ObjectFactory.CreateUserView();
            userView.Username = account.Username.ToLower();
            userView.Id += "1";

            Assert.IsFalse(service.CanEdit(userView));
            Assert.AreEqual(service.ModelState["Username"].Errors[0].ErrorMessage, Validations.UsernameIsAlreadyTaken);
        }

        [Test]
        public void CanEdit_CanEditValidUser()
        {
            Assert.IsTrue(service.CanEdit(ObjectFactory.CreateUserView()));
        }

        #endregion

        #region Method: Create(UserView view)

        [Test]
        public void Create_CreatesAccount()
        {
            TearDownData();

            UserView expected = ObjectFactory.CreateUserView();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            service.Create(expected);

            Account actual = context.Set<Account>().Find(expected.Id);

            Assert.AreEqual(expected.Id, actual.PersonId);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsTrue(BCrypter.Verify(expected.Password, actual.Passhash));
        }

        [Test]
        public void Create_CreatesUser()
        {
            TearDownData();

            UserView expected = ObjectFactory.CreateUserView();
            Role role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            expected.Person.RoleId = role.Id;
            service.Create(expected);

            Person actual = context.Set<Person>().Find(expected.Id);

            Assert.AreEqual(expected.Person.DateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.Person.FirstName, actual.FirstName);
            Assert.AreEqual(expected.Person.LastName, actual.LastName);
            Assert.AreEqual(expected.Person.RoleId, actual.RoleId);
        }

        #endregion

        #region Method: Edit(UserView view)

        [Test]
        public void Edit_EditsAccount()
        {
            UserView expected = service.GetView(account.PersonId);
            expected.Username += "1";
            service.Edit(expected);

            context = new TestingContext();
            Account actual = context.Set<Account>().Find(expected.Id);

            Assert.AreEqual(expected.Id, actual.PersonId);
            Assert.AreEqual(expected.Username, actual.Username);
        }

        [Test]
        public void Edit_EditsPerson()
        {
            UserView userView = service.GetView(account.PersonId);
            userView.Person.DateOfBirth = null;
            userView.Person.FirstName += "1";
            userView.Person.LastName += "1";
            userView.Person.RoleId = null;
            service.Edit(userView);

            context = new TestingContext();
            PersonView expected = userView.Person;
            Person actual = context.Set<Person>().Find(userView.Id);

            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
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
            context.Set<Person>().RemoveRange(context.Set<Person>().Where(person => person.Id.StartsWith(ObjectFactory.TestId)));
            context.Set<Role>().RemoveRange(context.Set<Role>().Where(role => role.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        #endregion
    }
}
