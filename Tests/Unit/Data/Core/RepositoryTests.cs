using AutoMapper.QueryableExtensions;
using Moq;
using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Tests.Objects;
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
            Mock<DbSet<TestModel>> setMock = new Mock<DbSet<TestModel>>();
            setMock.As<IQueryable>()
                .Setup(mock => mock.Expression)
                .Returns(Expression.Add(Expression.Constant(0), Expression.Constant(0)));

            Mock<AContext> contextMock = new Mock<AContext>();
            contextMock.Setup(mock => mock.Set<TestModel>()).Returns(setMock.Object);
            repository = new Repository<TestModel>(contextMock.Object);

            Expression expected = (contextMock.Object.Set<TestModel>() as IQueryable).Expression;
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

            TestModel expected = context.Set<TestModel>().SingleOrDefault(model => model.Id == testModel.Id);
            TestModel actual = repository.GetById(testModel.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetById_OnModelNotFoundReturnsNull()
        {
            Assert.IsNull(repository.GetById(String.Empty));
        }

        #endregion

        #region Method: ProjectTo<TView>()

        [Test]
        public void ProjectTo_ProjectsContextsSet()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            IEnumerable<String> expected = context.Set<TestModel>().Project().To<TestView>().Select(view => view.Id).ToList();
            IEnumerable<String> actual = repository.ProjectTo<TestView>().Select(view => view.Id).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Insert(TModel model)

        [Test]
        public void Insert_InsertsModel()
        {
            TestModel expected = ObjectFactory.CreateTestModel();
            repository.Insert(expected);
            context.SaveChanges();

            TestModel actual = context.Set<TestModel>().SingleOrDefault(model => model.Id == expected.Id);

            TestHelper.PropertyWiseEquals(expected, actual);
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

            TestModel actual = context.Set<TestModel>().SingleOrDefault(model => model.Id == expected.Id);

            TestHelper.PropertyWiseEquals(expected, actual);
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

            TestModel actual = context.Set<TestModel>().SingleOrDefault(model => model.Id == expected.Id);

            TestHelper.PropertyWiseEquals(expected, actual);
        }

        [Test]
        public void Update_DoesNotModifyEntityDate()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            repository.Update(model);

            Assert.IsFalse(context.Entry(model).Property(prop => prop.EntityDate).IsModified);
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

            TestModel actual = context.Set<TestModel>().SingleOrDefault(model => model.Id == expected.Id);

            Assert.IsNull(actual);
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ReturnsContextsSetEnumerator()
        {
            IEnumerator<TestModel> expected = (context.Set<TestModel>() as IEnumerable<TestModel>).GetEnumerator();
            IEnumerator<TestModel> actual = repository.GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
                TestHelper.PropertyWiseEquals(expected.Current, actual.Current);
        }

        [Test]
        public void GetEnumerator_ReturnsSameEnumerator()
        {
            IEnumerator expected = (context.Set<TestModel>() as IEnumerable).GetEnumerator();
            IEnumerator actual = (repository as IEnumerable).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
                TestHelper.PropertyWiseEquals(expected.Current, actual.Current);
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
