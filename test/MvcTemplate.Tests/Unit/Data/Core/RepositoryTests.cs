using AutoMapper.QueryableExtensions;
using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    [TestFixture]
    public class RepositoryTests
    {
        private Repository<TestModel> repository;
        private TestingContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            repository = new Repository<TestModel>(context);

            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        #region Property: ElementType

        [Test]
        public void ElementType_IsRepositoryModelType()
        {
            Type expected = (context.Set<TestModel>() as IQueryable).ElementType;
            Type actual = (repository as IQueryable).ElementType;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Property: Expression

        [Test]
        public void Expression_IsContextSetsExpression()
        {
            DbSet<TestModel> databaseSet = Substitute.For<DbSet<TestModel>, IQueryable>();
            (databaseSet as IQueryable).Expression.Returns(Expression.Add(Expression.Constant(0), Expression.Constant(0)));

            AContext context = Substitute.For<AContext>();
            context.Set<TestModel>().Returns(databaseSet);
            repository = new Repository<TestModel>(context);

            Expression expected = (context.Set<TestModel>() as IQueryable).Expression;
            Expression actual = (repository as IQueryable).Expression;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Property: Provider

        [Test]
        public void Provider_IsDbSetsProvider()
        {
            IQueryProvider expected = (context.Set<TestModel>() as IQueryable).Provider;
            IQueryProvider actual = (repository as IQueryable).Provider;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: GetById(Object id)

        [Test]
        public void GetById_GetsModelById()
        {
            TestModel testModel = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(testModel);
            context.SaveChanges();

            TestModel expected = context.Set<TestModel>().AsNoTracking().Single();
            TestModel actual = repository.GetById(testModel.Id);

            Assert.AreEqual(expected.CreationDate, actual.CreationDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        [Test]
        public void GetById_OnModelNotFoundReturnsNull()
        {
            Assert.IsNull(repository.GetById(""));
        }

        #endregion

        #region Method: To<TView>()

        [Test]
        public void To_ProjectsContextsSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            IEnumerable expected = context.Set<TestModel>().Project().To<TestView>().Select(view => view.Id).ToArray();
            IEnumerable actual = repository.To<TestView>().Select(view => view.Id).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Insert(TModel model)

        [Test]
        public void Insert_AddsModelToDbSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            repository.Insert(model);

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

            repository.Update(model);

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
            TestModel editedModel = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(attachedModel);
            editedModel.Text += "Test";

            repository.Update(editedModel);

            DbEntityEntry<TestModel> actual = context.Entry<TestModel>(attachedModel);
            TestModel expected = editedModel;

            Assert.AreEqual(expected.CreationDate, actual.Entity.CreationDate);
            Assert.AreEqual(EntityState.Modified, actual.State);
            Assert.AreEqual(expected.Text, actual.Entity.Text);
            Assert.AreEqual(expected.Id, actual.Entity.Id);
        }

        [Test]
        public void Update_DoesNotModifyCreationDate()
        {
            TestModel model = ObjectFactory.CreateTestModel();

            repository.Update(model);

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

            repository.Delete(model);
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

            repository.Delete(model.Id);
            context.SaveChanges();

            CollectionAssert.IsEmpty(context.Set<TestModel>());
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ReturnsContextsSetsEnumerator()
        {
            repository.Insert(ObjectFactory.CreateTestModel());
            context.SaveChanges();

            IEnumerable expected = context.Set<TestModel>();
            IEnumerable actual = repository.ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEnumerator_ReturnsSameEnumerator()
        {
            repository.Insert(ObjectFactory.CreateTestModel());
            context.SaveChanges();

            IEnumerable expected = context.Set<TestModel>();
            IEnumerable actual = repository;

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
