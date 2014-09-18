using AutoMapper;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private UnitOfWork unitOfWork;
        private IAuditLogger logger;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            logger = Substitute.For<IAuditLogger>();
            unitOfWork = new UnitOfWork(context, logger);
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
            IRepository<Account> actual = unitOfWork.Repository<Account>();
            IRepository<Account> expected = context.Repository<Account>();

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: ToModel<TView, TModel>(TView view)

        [Test]
        public void ToModel_ConvertsViewToModel()
        {
            AccountView view = ObjectFactory.CreateAccountView();

            Account expected = Mapper.Map<AccountView, Account>(view);
            Account actual = unitOfWork.ToModel<AccountView, Account>(view);

            TestHelper.PropertyWiseEqual(expected, actual);
        }

        #endregion

        #region Method: ToView<TModel, TView>(TModel model)

        [Test]
        public void ToView_ConvertsModelToView()
        {
            Account model = ObjectFactory.CreateAccount();
            model.Role = ObjectFactory.CreateRole();
            model.RoleId = model.Role.Id;

            AccountView actual = unitOfWork.ToView<Account, AccountView>(model);
            AccountView expected = Mapper.Map<Account, AccountView>(model);

            TestHelper.PropertyWiseEqual(expected, actual);
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
            Account expected = ObjectFactory.CreateAccount(2);
            unitOfWork.Repository<Account>().Insert(expected);
            unitOfWork.Commit();

            Account actual = unitOfWork.Repository<Account>().GetById(expected.Id);
            unitOfWork.Repository<Account>().Delete(expected.Id);
            unitOfWork.Commit();

            TestHelper.PropertyWiseEqual(expected, actual);
        }

        [Test]
        public void Commit_LogsEntities()
        {
            unitOfWork.Commit();

            logger.Received().Log(Arg.Any<IEnumerable<DbEntityEntry<BaseModel>>>());
            logger.Received().Save();
        }

        [Test]
        public void Commit_DoesNotSaveLogsOnFailedCommit()
        {
            try
            {
                unitOfWork.Repository<Account>().Insert(new Account());
                unitOfWork.Commit();
            }
            catch
            {
            }

            logger.Received().Log(Arg.Any<IEnumerable<DbEntityEntry<BaseModel>>>());
            logger.DidNotReceive().Save();
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DiposesLogger()
        {
            unitOfWork.Dispose();

            logger.Received().Dispose();
        }

        [Test]
        public void Dispose_DiposesContext()
        {
            AContext context = Substitute.For<AContext>();
            UnitOfWork unitOfWork = new UnitOfWork(context);

            unitOfWork.Dispose();

            context.Received().Dispose();
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            unitOfWork.Dispose();
            unitOfWork.Dispose();
        }

        #endregion
    }
}
