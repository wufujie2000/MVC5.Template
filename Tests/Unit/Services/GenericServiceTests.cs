using AutoMapper.QueryableExtensions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
            service.ModelState = new ModelStateDictionary();

            context.Set<TestModel>().RemoveRange(context.Set<TestModel>().Where(model => model.Id.StartsWith(ObjectFactory.TestId)));
            context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            service.Dispose();
            context.Dispose();
        }

        #region Method: CanCreate(TView view)

        [Test]
        public void CanCreate_CanCreateWithValidModelState()
        {
            Assert.IsTrue(service.CanCreate(null));
        }

        [Test]
        public void CanCreate_CanNotCreateWithInvalidModelState()
        {
            service.ModelState.AddModelError(String.Empty, String.Empty);

            Assert.IsFalse(service.CanCreate(null));
        }

        #endregion

        #region Method: CanEdit(TView view)

        [Test]
        public void CanEdit_CanEditWithValidModelState()
        {
            Assert.IsTrue(service.CanEdit(null));
        }

        [Test]
        public void CanEdit_CanNotEditWithInvalidModelState()
        {
            service.ModelState.AddModelError(String.Empty, String.Empty);

            Assert.IsFalse(service.CanEdit(null));
        }

        #endregion

        #region Method: CanDelete(String id)

        [Test]
        public void CanDelete_CanDeleteWithValidModelState()
        {
            Assert.IsTrue(service.CanDelete(null));
        }

        [Test]
        public void CanDelete_CanNotDeleteWithInvalidModelState()
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
            context.Set<TestModel>().Add(ObjectFactory.CreateTestModel());
            context.SaveChanges();

            TestModel expected = context.Set<TestModel>().SingleOrDefault(model => model.Id == ObjectFactory.TestId + "1");
            TestView actual = service.GetView(ObjectFactory.TestId + "1");

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

            TestModel actual = context.Set<TestModel>().SingleOrDefault(model => model.Id == expected.Id);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
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

            context = new TestingContext();
            TestModel actual = context.Set<TestModel>().SingleOrDefault(testModel => testModel.Id == expected.Id);

            Assert.AreEqual(expected.EntityDate, actual.EntityDate);
            Assert.AreEqual(expected.Text, actual.Text);
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeletesView()
        {
            TestView expected = ObjectFactory.CreateTestView();
            service.Create(expected);

            if (context.Set<TestModel>().SingleOrDefault(model => model.Id == expected.Id) == null)
                Assert.Inconclusive();

            service.Delete(expected.Id);

            Assert.IsNull(context.Set<TestModel>().SingleOrDefault(model => model.Id == expected.Id));
        }

        #endregion
    }
}
