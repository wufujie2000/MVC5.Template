using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Template.Components.Logging;
using Template.Data.Core;
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
            Assert.AreSame(context.Repository<Account>(), unitOfWork.Repository<Account>());
        }

        #endregion

        #region Method: ToModel<TView, TModel>(TView view)

        [Test]
        public void ToModel_ConvertsViewToModel()
        {
            AccountView view = ObjectFactory.CreateAccountView();
            Account expected = Mapper.Map<AccountView, Account>(view);
            Account actual = unitOfWork.ToModel<AccountView, Account>(view);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: ToView<TModel, TView>(TModel model)

        [Test]
        public void ToView_ConvertsModelToView()
        {
            Account model = ObjectFactory.CreateAccount();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            AccountView expected = Mapper.Map<Account, AccountView>(model);
            AccountView actual = unitOfWork.ToView<Account, AccountView>(model);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: Rollback()

        [Test]
        public void RollBack_RollbacksChanges()
        {
            Account model = ObjectFactory.CreateAccount();
            context.Set<Account>().Add(model);

            unitOfWork.Rollback();
            unitOfWork.Commit();

            Assert.IsNull(unitOfWork.Repository<Account>().GetById(model.Id));
        }

        #endregion

        #region Method: Commit()

        [Test]
        public void Commit_SavesChanges()
        {
            Account expected = ObjectFactory.CreateAccount();
            unitOfWork.Repository<Account>().Insert(expected);
            unitOfWork.Commit();

            Account actual = unitOfWork.Repository<Account>().GetById(expected.Id);
            unitOfWork.Repository<Account>().Delete(expected.Id);
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
        public void Dispose_CanBeDisposedMultipleTimes()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        #endregion
    }
}
