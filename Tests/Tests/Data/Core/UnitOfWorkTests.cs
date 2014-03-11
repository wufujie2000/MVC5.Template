using AutoMapper;
using NUnit.Framework;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

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
            context = new TestingContext();
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
            var view = ObjectFactory.CreateUserView();
            var expected = Mapper.Map<UserView, User>(view);
            var actual = unitOfWork.ToModel<UserView, User>(view);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: ToView<TModel, TView>(TModel model)

        [Test]
        public void ToView_ConvertsModelToView()
        {
            var model = ObjectFactory.CreateUser();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            var expected = Mapper.Map<User, UserView>(model);
            var actual = unitOfWork.ToView<User, UserView>(model);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Rollback()

        [Test]
        public void RollBack_RollbacksChanges()
        {
            var model = ObjectFactory.CreateUser();
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
            var expected = ObjectFactory.CreateUser();
            unitOfWork.Repository<User>().Insert(expected);
            unitOfWork.Commit();

            var actual = unitOfWork.Repository<User>().GetById(expected.Id);
            unitOfWork.Repository<User>().Delete(expected);
            unitOfWork.Commit();

            TestHelper.PropertyWiseEquals(expected, actual);
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
