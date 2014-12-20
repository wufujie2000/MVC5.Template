using AutoMapper.QueryableExtensions;
using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    [TestFixture]
    public class SelectTests
    {
        private Select<TestModel> select;
        private TestingContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            select = new Select<TestModel>(context.Set<TestModel>());

            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.Set<TestModel>().Add(ObjectFactory.CreateTestModel());
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        #region Property: ElementType

        [Test]
        public void ElementType_IsModelType()
        {
            Type actual = (select as IQueryable).ElementType;
            Type expected = typeof(TestModel);

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Property: Expression

        [Test]
        public void Expression_IsSetsExpression()
        {
            DbContext context = Substitute.For<DbContext>();
            DbSet<TestModel> set = Substitute.For<DbSet<TestModel>, IQueryable>();
            (set as IQueryable).Expression.Returns(Expression.Constant(0));
            context.Set<TestModel>().Returns(set);

            select = new Select<TestModel>(context.Set<TestModel>());

            Expression actual = (select as IQueryable).Expression;
            Expression expected = (set as IQueryable).Expression;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Property: Provider

        [Test]
        public void Provider_IsSetsProvider()
        {
            IQueryProvider expected = (context.Set<TestModel>() as IQueryable).Provider;
            IQueryProvider actual = (select as IQueryable).Provider;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Where(Expression<Func<TModel, Boolean>> predicate)

        [Test]
        public void Where_FiltersSelection()
        {
            IQueryable<TestModel> expected = context.Set<TestModel>().Where(model => model.Id == null);
            IQueryable<TestModel> actual = select.Where(model => model.Id == null);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Where_ReturnsSameSelect()
        {
            ISelect<TestModel> actual = select.Where(model => model.Id == null);
            ISelect<TestModel> expected = select;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: To<TView>()

        [Test]
        public void To_ProjectsSetTo()
        {
            IEnumerable expected = context.Set<TestModel>().Project().To<TestView>().Select(view => view.Id).ToArray();
            IEnumerable actual = select.To<TestView>().Select(view => view.Id).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetEnumerator()

        [Test]
        public void GetEnumerator_ReturnsContextsSetsEnumerator()
        {
            IEnumerable expected = context.Set<TestModel>();
            IEnumerable actual = select.ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetEnumerator_ReturnsSameEnumerator()
        {
            IEnumerable expected = context.Set<TestModel>();
            IEnumerable actual = select;

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
