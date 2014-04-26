using AutoMapper.QueryableExtensions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Data.Core;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Components.Services
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
            service.ModelState = new ModelStateDictionary();
        }

        [TearDown]
        public void TearDown()
        {
            foreach (TestModel model in context.Set<TestModel>().Where(model => model.Id.StartsWith(ObjectFactory.TestId)))
                context.Set<TestModel>().Remove(model);

            context.SaveChanges();
            service.Dispose();
            context.Dispose();
        }

        #region Method: CanCreate(TView view)

        [Test]
        public void CanCreate_OnNoModelErrorsReturnsTrue()
        {
            Assert.IsTrue(service.CanCreate(null));
        }

        [Test]
        public void CanCreate_OnModelErrorsReturnsFalse()
        {
            service.ModelState.AddModelError(String.Empty, String.Empty);

            Assert.IsFalse(service.CanCreate(null));
        }

        #endregion

        #region Method: CanEdit(TView view)

        [Test]
        public void CanEdit_OnNoModelErrorsReturnsTrue()
        {
            Assert.IsTrue(service.CanEdit(null));
        }

        [Test]
        public void CanEdit_OnModelErrorsReturnsFalse()
        {
            service.ModelState.AddModelError(String.Empty, String.Empty);

            Assert.IsFalse(service.CanEdit(null));
        }

        #endregion

        #region Method: CanDelete(String id)

        [Test]
        public void CanDelete_OnNoModelErrorsReturnsTrue()
        {
            Assert.IsTrue(service.CanDelete(null));
        }

        [Test]
        public void CanDelete_OnModelErrorsReturnsFalse()
        {
            service.ModelState.AddModelError(String.Empty, String.Empty);

            Assert.IsFalse(service.CanDelete(null));
        }

        #endregion

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
            TestModel expected = context.Set<TestModel>().Find("1");
            TestView actual = service.GetView("1");

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Text, actual.Text);
        }

        #endregion

        #region Method: Create(TView view)

        [Test]
        public void Create_CreatesView()
        {
            TestView expected = ObjectFactory.CreateTestView();
            service.Create(expected);

            TestModel actual = context.Set<TestModel>().Find(expected.Id);

            Assert.AreEqual(expected.Text, actual.Text);
        }

        #endregion

        #region Method: Edit(TView view)

        [Test]
        public void Edit_EditsView()
        {
            TestModel model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            TestView expected = service.GetView(model.Id);
            expected.Text = "EditedText";
            service.Edit(expected);

            TestModel actual = context.Set<TestModel>().Find(expected.Id);

            Assert.AreEqual(expected.Text, actual.Text);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeleteView()
        {
            TestView expected = ObjectFactory.CreateTestView();
            service.Create(expected);

            if (context.Set<TestModel>().Find(expected.Id) == null)
                Assert.Inconclusive();

            service.Delete(expected.Id);
            context = new TestingContext();

            Assert.IsNull(context.Set<TestModel>().Find(expected.Id));
        }

        #endregion
    }
}
