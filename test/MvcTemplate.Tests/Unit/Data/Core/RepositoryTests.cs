using AutoMapper.QueryableExtensions;
using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    [TestFixture]
    public class RepositoryTests
    {
        private Repository<TestModel> repository;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            repository = new Repository<TestModel>(context);

            TearDownData();
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
        public void Provider_IsContextSetsProvider()
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

            TestModel expected = context.Set<TestModel>().Single();
            TestModel actual = repository.GetById(testModel.Id);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void GetById_OnModelNotFoundReturnsNull()
        {
            Assert.IsNull(repository.GetById(String.Empty));
        }

        #endregion

        #region Method: To<TView>()

        [Test]
        public void To_ProjectsContextsSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            IEnumerable<String> expected = context.Set<TestModel>().Project().To<TestView>().Select(view => view.Id).ToList();
            IEnumerable<String> actual = repository.To<TestView>().Select(view => view.Id).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Insert(TModel model)

        [Test]
        public void Insert_InsertsModel()
        {
            repository.Insert(ObjectFactory.CreateTestModel());
            context.SaveChanges();

            TestModel actual = context.Set<TestModel>().Single();
            TestModel expected = repository.Single();

            TestHelper.PropertyWiseEqual(expected, actual);
        }

        #endregion

        #region Method: Update(TModel model)

        [Test]
        public void Update_UpdatesAttachedModel()
        {
            TestModel expected = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(expected);
            context.SaveChanges();

            expected.Text += "Test";
            repository.Update(expected);
            context.SaveChanges();

            TestModel actual = context.Set<TestModel>().Single();

            TestHelper.PropertyWiseEqual(expected, actual);
        }

        [Test]
        public void Update_UpdatesNotAttachedModel()
        {
            TestModel expected = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(expected);
            context.SaveChanges();

            expected.Text += "Test";
            context = new TestingContext();
            repository = new Repository<TestModel>(context);
            repository.Update(expected);
            context.SaveChanges();

            TestModel actual = context.Set<TestModel>().Single();

            TestHelper.PropertyWiseEqual(expected, actual);
        }

        [Test]
        public void Update_DoesNotModifyCreationDate()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            repository.Update(model);

            Assert.IsFalse(context.Entry(model).Property(prop => prop.CreationDate).IsModified);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesModelById()
        {
            TestModel expected = ObjectFactory.CreateTestModel();
            repository.Insert(expected);
            context.SaveChanges();

            repository.Delete(expected.Id);
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

            IEnumerable<TestModel> expected = context.Set<TestModel>();
            IEnumerable<TestModel> actual = repository.ToList();

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

        #region Test helpers

        private void TearDownData()
        {
            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.SaveChanges();
        }

        #endregion
    }
}
