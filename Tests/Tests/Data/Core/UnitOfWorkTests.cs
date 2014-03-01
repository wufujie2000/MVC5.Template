using AutoMapper;
using NUnit.Framework;
using System;
using Template.Data.Core;
using Template.Objects;

namespace Template.Tests.Tests.Data.Core
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private UnitOfWork unitOfWork;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new Context();
            unitOfWork = new UnitOfWork(context);
        }

        [TearDown]
        public void TearDown()
        {
            unitOfWork.Dispose();
        }

        #region Method: Repository<TModel>()

        [Test]
        public void Repository_UsesContextsRepository()
        {
            Assert.AreEqual(context.Repository<User>(), unitOfWork.Repository<User>());
        }

        #endregion

        #region Method: ToModel<TView, TModel>(TView view)

        [Test]
        public void ToModel_ConvertsViewToModel()
        {
            var view = new UserView()
            {
                Id = "UserViewId",
                UserFirstName = "UserFirstName",
                UserLastName = "UserLastName",
                UserDateOfBirth = DateTime.Now,

                UserRoleId = "UserRoleId",
                UserRoleName = "UserRoleName",

                Username = "Username",
                Password = "Password",
                NewPassword = "NewPassword"
            };

            var expected = Mapper.Map<UserView, User>(view);
            var actual = unitOfWork.ToModel<UserView, User>(view);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.DateOfBirth, actual.DateOfBirth);
            Assert.AreEqual(expected.RoleId, actual.RoleId);
        }

        #endregion

        #region Method: ToView<TModel, TView>(TModel model)

        [Test]
        public void ToView_ConvertsModelToView()
        {
            var model = new User()
            {
                Id = "UserViewId",
                FirstName = "UserFirstName",
                LastName = "UserLastName",
                DateOfBirth = DateTime.Now,

                RoleId = "UserRoleId",
                Role = new Role()
                {
                    Id = "UserRoleId",
                    Name = "RoleName"
                }
            };

            var expected = Mapper.Map<User, UserView>(model);
            var actual = unitOfWork.ToView<User, UserView>(model);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.UserFirstName, actual.UserFirstName);
            Assert.AreEqual(expected.UserLastName, actual.UserLastName);
            Assert.AreEqual(expected.UserDateOfBirth, actual.UserDateOfBirth);

            Assert.AreEqual(expected.UserRoleId, actual.UserRoleId);
            Assert.AreEqual(expected.UserRoleName, actual.UserRoleName);

            Assert.AreEqual(expected.Username, actual.Username);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.NewPassword, actual.NewPassword);
        }

        #endregion

        #region Method: Rollback()

        [Test]
        public void RollBack_RollbacksChanges()
        {
            var model = new User();
            context.Set<User>().Add(model);

            unitOfWork.Rollback();
            unitOfWork.Commit();

            Assert.IsNull(unitOfWork.Repository<User>().GetById(model.Id));
        }

        #endregion

        #region Method: Commit()

        [Test]
        public void Commit_SavesChanges()
        {
            var expected = new User();
            unitOfWork.Repository<User>().Insert(expected);
            unitOfWork.Commit();

            var actual = unitOfWork.Repository<User>().GetById(expected.Id);
            unitOfWork.Repository<User>().Delete(expected);
            unitOfWork.Commit();

            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_CanDisposeMoreThanOnce()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        #endregion
    }
}
