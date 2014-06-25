using AutoMapper.QueryableExtensions;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Template.Data.Core;
using Template.Services;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Services
{
    [TestFixture]
    public class GenericServiceTests
    {
        private GenericService<TestModel, TestView> service;
        private AContext context;

        [SetUp]
        public void SetUp()
        {
            context = new TestingContext();
            service = new Mock<GenericService<TestModel, TestView>>(new UnitOfWork(context)) { CallBase = true }.Object;

            context.Set<TestModel>().RemoveRange(context.Set<TestModel>().Where(model => model.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
            context.Dispose();
        }

        #region Method: GetViews()

        [Test]
        public void GetViews_GetsViews()
        {
            IEnumerable<TestView> actual = service.GetViews();
            IEnumerable<TestView> expected = context
                .Set<TestModel>()
                .Project()
                .To<TestView>()
                .OrderByDescending(account => account.EntityDate);

            TestHelper.EnumPropertyWiseEquals(expected, actual);
        }

        #endregion

        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            context.Set<TestModel>().Add(ObjectFactory.CreateTestModel());
            context.SaveChanges();

            TestModel expected = context.Set<TestModel>().SingleOrDefault(model => model.Id == ObjectFactory.TestId + "1");
            TestView actual = service.GetView(ObjectFactory.TestId + "1");

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Text, actual.Text);
        }

        #endregion
    }
}
