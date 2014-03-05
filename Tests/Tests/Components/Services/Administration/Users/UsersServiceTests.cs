using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Security;
using Template.Components.Services;
using Template.Data.Core;
using Template.Objects;
using Template.Resources.Views.UserView;
using Template.Tests.Helpers;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private ModelStateDictionary modelState;
        private UsersService service;
        private Context context;
        private Account account;

        [SetUp]
        public void SetUp()
        {
            modelState = new ModelStateDictionary();
            service = new UsersService(new UnitOfWork());
            context = new Context();

            SetUpData();
        }

        [TearDown]
        public void TearDown()
        {
            TearDownData();

            service.Dispose();
            context.Dispose();
        }

        #region Method: CanCreate(UserView view)

        [Test]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            modelState.AddModelError("Test", "Test");
            Assert.IsFalse(service.CanCreate(ObjectFactory.CreateUserView()));
        }

        [Test]
        public void CanCreate_CanNotCreateWithAlreadyTakenUsername()
        {
            var userView = ObjectFactory.CreateUserView();
            userView.Username = account.Username.ToLower();
            userView.Id += "1";

            Assert.IsFalse(service.CanCreate(userView));
            Assert.AreEqual(1, modelState["Username"].Errors.Count);
            Assert.AreEqual(modelState["Username"].Errors[0].ErrorMessage, Validations.UsernameIsAlreadyTaken);
        }

        [Test]
        public void CanCreate_CanNotCreateIfPasswordIsNotSpecified()
        {
            var userView = ObjectFactory.CreateUserView();
            userView.Password = "       ";

            Assert.IsFalse(service.CanCreate(userView));
            Assert.AreEqual(1, modelState["Password"].Errors.Count);
            Assert.AreEqual(modelState["Password"].Errors[0].ErrorMessage, Validations.PasswordFieldIsRequired);
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
            modelState.AddModelError("Test", "Test");
            Assert.IsFalse(service.CanEdit(ObjectFactory.CreateUserView()));
        }

        [Test]
        public void CanEdit_CanNotEditToAlreadyTakenUsername()
        {
            var userView = ObjectFactory.CreateUserView();
            userView.Username = account.Username.ToLower();
            userView.Id += "1";

            Assert.IsFalse(service.CanEdit(userView));
            Assert.AreEqual(1, modelState["Username"].Errors.Count);
            Assert.AreEqual(modelState["Username"].Errors[0].ErrorMessage, Validations.UsernameIsAlreadyTaken);
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

            var expected = ObjectFactory.CreateUserView();
            var role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            expected.UserRoleId = role.Id;
            service.Create(expected);

            var actual = context.Set<Account>().Find(expected.Id);

            Assert.AreEqual(expected.Id, actual.UserId);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsTrue(BCrypter.Verify(expected.Password, actual.Passhash));
        }

        [Test]
        public void Create_CreatesUser()
        {
            TearDownData();

            var expected = ObjectFactory.CreateUserView();
            var role = ObjectFactory.CreateRole();
            context.Set<Role>().Add(role);
            context.SaveChanges();

            expected.UserRoleId = role.Id;
            service.Create(expected);

            var actual = context.Set<User>().Find(expected.Id);

            Assert.AreEqual(expected.UserDateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.UserFirstName, actual.FirstName);
            Assert.AreEqual(expected.UserLastName, actual.LastName);
            Assert.AreEqual(expected.UserRoleId, actual.RoleId);
        }

        #endregion

        #region Method: Edit(UserView view)

        [Test]
        public void Edit_EditsAccount()
        {
            var expected = service.GetView(account.UserId);
            expected.NewPassword += "1";
            expected.Username += "1";
            service.Edit(expected);

            context = new Context();
            var actual = context.Set<Account>().Find(expected.Id);

            Assert.AreEqual(expected.Id, actual.UserId);
            Assert.AreEqual(expected.Username, actual.Username);
            Assert.IsTrue(BCrypter.Verify(expected.NewPassword, actual.Passhash));
        }

        [Test]
        public void Edit_EditsUser()
        {
            var expected = service.GetView(account.UserId);
            expected.UserDateOfBirth = null;
            expected.UserFirstName += "1";
            expected.UserLastName += "1";
            expected.UserRoleId = null;
            service.Edit(expected);

            context = new Context();
            var actual = context.Set<User>().Find(expected.Id);

            Assert.AreEqual(expected.UserDateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.UserFirstName, actual.FirstName);
            Assert.AreEqual(expected.UserLastName, actual.LastName);
            Assert.AreEqual(expected.UserRoleId, actual.RoleId);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesAccount()
        {
            service.Delete(account.Id);
            context = new Context();
            Assert.IsNull(context.Set<Account>().Find(account.Id));
        }

        [Test]
        public void Delete_DeletesUser()
        {
            service.Delete(account.UserId);
            context = new Context();
            Assert.IsNull(context.Set<User>().Find(account.UserId));
        }

        #endregion

        #region Test helpers

        private void SetUpData()
        {
            account = ObjectFactory.CreateAccount();
            account.User = ObjectFactory.CreateUser();
            account.User.Role = ObjectFactory.CreateRole();
            account.User.RoleId = account.User.Role.Id;
            account.UserId = account.User.Id;

            context.Set<Account>().Add(account);
            context.SaveChanges();
        }
        private void TearDownData()
        {
            var testId = TestContext.CurrentContext.Test.Name;
            foreach (var user in context.Set<User>().Where(user => user.Id.StartsWith(testId)))
                context.Set<User>().Remove(user);

            foreach (var role in context.Set<Role>().Where(role => role.Id.StartsWith(testId)))
                context.Set<Role>().Remove(role);

            context.SaveChanges();
        }

        #endregion
    }
}
