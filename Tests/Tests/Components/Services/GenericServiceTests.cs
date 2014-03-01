using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using Template.Objects;
using Template.Tests.Objects.Components.Services;

namespace Template.Tests.Tests.Components.Services
{
    [TestFixture]
    public class GenericServiceTests
    {
        private ModelStateDictionary modelState;
        private GenericServiceStub service;

        [SetUp]
        public void SetUp()
        {
            modelState = new ModelStateDictionary();
            service = new GenericServiceStub(modelState);
        }

        #region Constructor: GenericService(ModelStateDictionary modelState) : base(modelState)

        [Test]
        public void GenericService_OnNullModelStateThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new GenericServiceStub(null));
        }

        [Test]
        public void GenericService_SetsModelState()
        {
            Assert.AreEqual(modelState, service.BaseModelState);
        }

        #endregion

        #region Method: CanCreate(TView view)

        [Test]
        public void CanCreate_OnNoModelErrorsReturnsTrue()
        {
            Assert.IsTrue(service.CanCreate(null));
        }

        [Test]
        public void CanCreate_OnModelErrorsReturnsFalse()
        {
            modelState.AddModelError(String.Empty, String.Empty);
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
            modelState.AddModelError(String.Empty, String.Empty);
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
            modelState.AddModelError(String.Empty, String.Empty);
            Assert.IsFalse(service.CanDelete(null));
        }

        #endregion

        #region Method: GetViews()

        [Test]
        public void GetViews_GetsViews()
        {
            var expected = service
                .BaseUnitOfWork
                .Repository<Account>()
                .ProjectTo<AccountView>()
                .OrderByDescending(account => account.Id)
                .Select(account => account.Id)
                .GetEnumerator();
            var actual = service.GetViews().Select(account => account.Id).GetEnumerator();

            while (expected.MoveNext() | actual.MoveNext())
                Assert.AreEqual(expected.Current, actual.Current);
        }

        #endregion

        #region Method: GetView(String id)

        [Test]
        public void GetView_GetsViewById()
        {
            var user = new UserView();
            service.Create(user);

            var actual = service.GetView(user.Id);
            service.Delete(user.Id);

            Assert.AreEqual(user.Id, actual.Id);
        }

        #endregion

        #region Method: Create(TView view)

        [Test]
        public void Create_CreatesView()
        {
            var user = new UserView();
            service.Create(user);

            var actual = service.GetView(user.Id);
            service.Delete(user.Id);

            Assert.IsNotNull(actual);
        }

        #endregion

        #region Method: Edit(TView view)

        [Test]
        public void Edit_EditsView()
        {
            var user = new UserView();
            service.Create(user);

            var expected = "Test";
            user.UserFirstName = expected;
            service.Edit(user);
            
            var actual = service.GetView(user.Id).UserFirstName;
            service.Delete(user.Id);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_DeleteView()
        {
            var user = new UserView();
            service.Create(user);
            service.Delete(user.Id);

            Assert.IsNull(service.GetView(user.Id));
        }

        #endregion
    }
}
