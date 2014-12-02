using AutoMapper;
using MvcTemplate.Data.Core;
using MvcTemplate.Data.Logging;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private TestingContext context;
        private UnitOfWork unitOfWork;
        private IAuditLogger logger;

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
            IRepository<TestModel> actual = unitOfWork.Repository<TestModel>();
            IRepository<TestModel> expected = context.Repository<TestModel>();

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: To<TModel>(BaseView view)

        [Test]
        public void ToModel_ConvertsViewToModel()
        {
            TestView view = ObjectFactory.CreateTestView();

            TestModel actual = unitOfWork.To<TestModel>(view);
            TestModel expected = Mapper.Map<TestModel>(view);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: To<TView>(BaseModel model)

        [Test]
        public void ToView_ConvertsModelToView()
        {
            TestModel model = ObjectFactory.CreateTestModel();

            TestView actual = unitOfWork.To<TestView>(model);
            TestView expected = Mapper.Map<TestView>(model);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Rollback()

        [Test]
        public void RollBack_RollbacksChanges()
        {
            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.SaveChanges();

            context.Set<TestModel>().Add(ObjectFactory.CreateTestModel());

            unitOfWork.Rollback();
            unitOfWork.Commit();

            CollectionAssert.IsEmpty(unitOfWork.Repository<TestModel>());
        }

        #endregion

        #region Method: Commit()

        [Test]
        public void Commit_SavesChanges()
        {
            TestModel expected = ObjectFactory.CreateTestModel(2);
            unitOfWork.Repository<TestModel>().Insert(expected);
            unitOfWork.Commit();

            TestModel actual = unitOfWork.Repository<TestModel>().GetById(expected.Id);
            unitOfWork.Repository<TestModel>().Delete(expected.Id);
            unitOfWork.Commit();

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
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
                unitOfWork.Repository<TestModel>().Insert(new TestModel { Text = new String('X', 513) });
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
