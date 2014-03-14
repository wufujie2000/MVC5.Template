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
            Assert.AreEqual(context.Repository<Person>(), unitOfWork.Repository<Person>());
        }

        #endregion

        #region Method: ToModel<TView, TModel>(TView view)

        [Test]
        public void ToModel_ConvertsViewToModel()
        {
            var view = ObjectFactory.CreateUserView();
            var expected = Mapper.Map<UserView, Account>(view);
            var actual = unitOfWork.ToModel<UserView, Account>(view);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: ToView<TModel, TView>(TModel model)

        [Test]
        public void ToView_ConvertsModelToView()
        {
            var model = ObjectFactory.CreatePerson();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            var expected = Mapper.Map<Person, PersonView>(model);
            var actual = unitOfWork.ToView<Person, PersonView>(model);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Rollback()

        [Test]
        public void RollBack_RollbacksChanges()
        {
            var model = ObjectFactory.CreatePerson();
            context.Set<Person>().Add(model);

            unitOfWork.Rollback();
            unitOfWork.Commit();

            Assert.IsNull(unitOfWork.Repository<Person>().GetById(model.Id));
        }

        #endregion

        #region Method: Commit()

        [Test]
        public void Commit_SavesChanges()
        {
            var expected = ObjectFactory.CreatePerson();
            unitOfWork.Repository<Person>().Insert(expected);
            unitOfWork.Commit();

            var actual = unitOfWork.Repository<Person>().GetById(expected.Id);
            unitOfWork.Repository<Person>().Delete(expected);
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
