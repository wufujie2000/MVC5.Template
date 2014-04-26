using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Template.Data.Core;
using Template.Data.Logging;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Data.Core
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
            UserView view = ObjectFactory.CreateUserView();
            Account expected = Mapper.Map<UserView, Account>(view);
            Account actual = unitOfWork.ToModel<UserView, Account>(view);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: ToView<TModel, TView>(TModel model)

        [Test]
        public void ToView_ConvertsModelToView()
        {
            Person model = ObjectFactory.CreatePerson();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            PersonView expected = Mapper.Map<Person, PersonView>(model);
            PersonView actual = unitOfWork.ToView<Person, PersonView>(model);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Rollback()

        [Test]
        public void RollBack_RollbacksChanges()
        {
            Person model = ObjectFactory.CreatePerson();
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
            Person expected = ObjectFactory.CreatePerson();
            unitOfWork.Repository<Person>().Insert(expected);
            unitOfWork.Commit();

            Person actual = unitOfWork.Repository<Person>().GetById(expected.Id);
            unitOfWork.Repository<Person>().Delete(expected.Id);
            unitOfWork.Commit();

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Commit_LogsEntities()
        {
            Mock<IEntityLogger> loggerMock = new Mock<IEntityLogger>();
            IEntityLogger logger = loggerMock.Object;

            unitOfWork = new UnitOfWork(context, logger);
            unitOfWork.Commit();

            loggerMock.Verify(mock => mock.Log(It.IsAny<IEnumerable<DbEntityEntry>>()), Times.Once());
            loggerMock.Verify(mock => mock.SaveLogs(), Times.Once());
        }


        [Test]
        public void Commit_DoesNotSaveLogsOnFailedCommit()
        {
            Mock<IEntityLogger> loggerMock = new Mock<IEntityLogger>();
            IEntityLogger logger = loggerMock.Object;

            unitOfWork = new UnitOfWork(context, logger);
            unitOfWork.Repository<Account>().Insert(new Account());
            try
            {
                unitOfWork.Commit();
            }
            catch
            {
            }
            loggerMock.Verify(mock => mock.Log(It.IsAny<IEnumerable<DbEntityEntry>>()), Times.Once());
            loggerMock.Verify(mock => mock.SaveLogs(), Times.Never());
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
