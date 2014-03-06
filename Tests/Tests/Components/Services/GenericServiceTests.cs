using AutoMapper.QueryableExtensions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Data.Core;
using Template.Tests.Data;
using Template.Tests.Helpers;
using Template.Tests.Objects.Components.Services;

namespace Template.Tests.Tests.Components.Services
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
            var testId = TestContext.CurrentContext.Test.Name;
            foreach (var model in context.Set<TestModel>().Where(model => model.Id.StartsWith(testId)))
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
            var expected = context.Set<TestModel>().Project().To<TestView>().OrderByDescending(account => account.Id).GetEnumerator();
            var actual = service.GetViews().GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
            {
                Assert.AreEqual(expected.Current.Id, actual.Current.Id);
                Assert.AreEqual(expected.Current.Text, actual.Current.Text);
            }
        }

        #endregion
        
        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            var expected = context.Set<TestModel>().Find("1");
            var actual = service.GetView("1");

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Text, actual.Text);
        }

        #endregion

        #region Method: Create(TView view)

        [Test]
        public void Create_CreatesView()
        {
            var expected = ObjectFactory.CreateTestView();
            service.Create(expected);

            var actual = context.Set<TestModel>().Find(expected.Id);

            Assert.AreEqual(expected.Text, actual.Text);
        }

        #endregion

        #region Method: Edit(TView view)
        
        [Test]
        public void Edit_EditsView()
        {
            var model = ObjectFactory.CreateTestModel();
            context.Set<TestModel>().Add(model);
            context.SaveChanges();

            var expected = service.GetView(model.Id);
            expected.Text = "EditedText";
            service.Edit(expected);

            var actual = context.Set<TestModel>().Find(expected.Id);

            Assert.AreEqual(expected.Text, actual.Text);
        }

        #endregion

        #region Method: Delete(String id)
        
        [Test]
        public void Delete_DeleteView()
        {
            var expected = ObjectFactory.CreateTestView();
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
