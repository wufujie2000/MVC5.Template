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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

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

            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            unitOfWork.Dispose();
        }

        #region Method: Select<TModel>()

        [Test]
        public void Select_CreatesSelectForSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            IEnumerable<TestModel> actual = unitOfWork.Select<TestModel>();
            IEnumerable<TestModel> expected = context.Set<TestModel>();

            CollectionAssert.AreEqual(expected, actual);
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

        #region Method: Get<TModel>(String id)

        [Test]
        public void Get_GetsModelById()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            TestModel expected = context.Set<TestModel>().AsNoTracking().Single();
            TestModel actual = unitOfWork.Get<TestModel>(model.Id);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void Get_OnModelNotFoundReturnsNull()
        {
            Assert.IsNull(unitOfWork.Get<TestModel>(""));
        }

        #endregion

        #region Method: GetAs<TModel, TView>(String id)

        [Test]
        public void GetAs_ReturnsModelAsViewById()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            TestView expected = Mapper.Map<TestView>(context.Set<TestModel>().AsNoTracking().Single());
            TestView actual = unitOfWork.GetAs<TestModel, TestView>(model.Id);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Insert<TModel>(TModel model)

        [Test]
        public void Insert_AddsModelToDbSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            unitOfWork.Insert(model);

            TestModel actual = context.Set<TestModel>().Local.Single();
            TestModel expected = model;

            Assert.AreEqual(EntityState.Added, context.Entry<TestModel>(model).State);
            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Update(TModel model)

        [Test]
        public void Update_UpdatesNotAttachedModel()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            model.Text += "Test";

            unitOfWork.Update(model);

            DbEntityEntry<TestModel> actual = context.Entry<TestModel>(model);
            TestModel expected = model;

            Assert.AreEqual(expected.CreationDate, actual.Entity.CreationDate);
            Assert.AreEqual(EntityState.Modified, actual.State);
            Assert.AreEqual(expected.Text, actual.Entity.Text);
            Assert.AreEqual(expected.Id, actual.Entity.Id);
        }

        [Test]
        public void Update_UpdatesAttachedModel()
        {
            TestModel attachedModel = ObjectFactory.CreateTestModel();
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(attachedModel);
            model.Text += "Test";

            unitOfWork.Update(model);

            DbEntityEntry<TestModel> actual = context.Entry<TestModel>(attachedModel);
            TestModel expected = model;

            Assert.AreEqual(expected.CreationDate, actual.Entity.CreationDate);
            Assert.AreEqual(EntityState.Modified, actual.State);
            Assert.AreEqual(expected.Text, actual.Entity.Text);
            Assert.AreEqual(expected.Id, actual.Entity.Id);
        }

        [Test]
        public void Update_DoesNotModifyCreationDate()
        {
            TestModel model = ObjectFactory.CreateTestModel();

            unitOfWork.Update(model);

            Assert.IsFalse(context.Entry(model).Property(prop => prop.CreationDate).IsModified);
        }

        #endregion

        #region Method: Delete(TModel model)

        [Test]
        public void Delete_DeletesModel()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            unitOfWork.Delete(model);
            context.SaveChanges();

            CollectionAssert.IsEmpty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesModelById()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            unitOfWork.Delete<TestModel>(model.Id);
            context.SaveChanges();

            CollectionAssert.IsEmpty(context.Set<TestModel>());
        }

        #endregion

        #region Method: Rollback()

        [Test]
        public void RollBack_RollbacksChanges()
        {
            context.Set<TestModel>().Add(ObjectFactory.CreateTestModel());

            unitOfWork.Rollback();
            unitOfWork.Commit();

            CollectionAssert.IsEmpty(unitOfWork.Select<TestModel>());
        }

        #endregion

        #region Method: Commit()

        [Test]
        public void Commit_SavesChanges()
        {
            DbContext context = Substitute.For<DbContext>();
            unitOfWork = new UnitOfWork(context);

            unitOfWork.Commit();

            context.Received().SaveChanges();
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
                unitOfWork.Insert(new TestModel { Text = new String('X', 513) });
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
            DbContext context = Substitute.For<DbContext>();
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
