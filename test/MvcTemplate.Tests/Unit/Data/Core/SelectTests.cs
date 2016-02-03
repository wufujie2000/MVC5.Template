using AutoMapper.QueryableExtensions;
using MvcTemplate.Data.Core;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Objects;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace MvcTemplate.Tests.Unit.Data.Core
{
    public class SelectTests : IDisposable
    {
        private Select<TestModel> select;
        private TestingContext context;

        public SelectTests()
        {
            context = new TestingContext();
            select = new Select<TestModel>(context.Set<TestModel>());

            context.Set<TestModel>().RemoveRange(context.Set<TestModel>());
            context.Set<TestModel>().Add(ObjectFactory.CreateTestModel());
            context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
        }

        #region Property: ElementType

        [Fact]
        public void ElementType_IsModelType()
        {
            Object actual = (select as IQueryable).ElementType;
            Object expected = typeof(TestModel);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Property: Expression

        [Fact]
        public void Expression_IsSetsExpression()
        {
            DbSet<TestModel> set = Substitute.For<DbSet<TestModel>, IQueryable>();
            ((IQueryable)set).Expression.Returns(Expression.Constant(0));
            TestingContext context = Substitute.For<TestingContext>();
            context.Set<TestModel>().Returns(set);

            select = new Select<TestModel>(context.Set<TestModel>());

            Object actual = ((IQueryable)select).Expression;
            Object expected = ((IQueryable)set).Expression;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Property: Provider

        [Fact]
        public void Provider_IsSetsProvider()
        {
            Object expected = (context.Set<TestModel>() as IQueryable).Provider;
            Object actual = (select as IQueryable).Provider;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Where(Expression<Func<TModel, Boolean>> predicate)

        [Fact]
        public void Where_Filters()
        {
            IEnumerable<TestModel> actual = select.Where(model => true);
            IEnumerable<TestModel> expected = context.Set<TestModel>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Where_ReturnsItself()
        {
            Object actual = select.Where(model => true);
            Object expected = select;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: To<TView>()

        [Fact]
        public void To_ProjectsSet()
        {
            IEnumerable<Int32> expected = context.Set<TestModel>().ProjectTo<TestView>().Select(view => view.Id).ToArray();
            IEnumerable<Int32> actual = select.To<TestView>().Select(view => view.Id).ToArray();

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: GetEnumerator()

        [Fact]
        public void GetEnumerator_ReturnsSetEnumerator()
        {
            IEnumerable<TestModel> expected = context.Set<TestModel>();
            IEnumerable<TestModel> actual = select.ToArray();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetEnumerator_ReturnsSameEnumerator()
        {
            IEnumerable<TestModel> expected = context.Set<TestModel>();
            IEnumerable<TestModel> actual = select;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
