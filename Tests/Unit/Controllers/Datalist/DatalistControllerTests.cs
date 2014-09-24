using Datalist;
using MvcTemplate.Components.Datalists;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class DatalistControllerTests
    {
        private DatalistController controller;
        private AbstractDatalist datalist;
        private IUnitOfWork unitOfWork;
        private DatalistFilter filter;

        [SetUp]
        public void SetUp()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            controller = Substitute.ForPartsOf<DatalistController>(unitOfWork);

            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            datalist = Substitute.For<AbstractDatalist>();
            filter = new DatalistFilter();
        }

        [TearDown]
        public void TearDown()
        {
            GlobalizationManager.Provider = null;
            HttpContext.Current = null;
        }

        #region Method: GetData(AbstractDatalist datalist, DatalistFilter filter, Dictionary<String, Object> additionalFilters = null)

        [Test]
        public void GetData_SetsDatalistCurrentFilter()
        {
            controller.GetData(datalist, filter);

            DatalistFilter actual = datalist.CurrentFilter;
            DatalistFilter expected = filter;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetData_SetsEmptyAdditionalFilters()
        {
            controller.GetData(datalist, filter, null);

            Int32 actual = filter.AdditionalFilters.Count;
            Int32 expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetData_SetsAdditionalFilters()
        {
            Dictionary<String, Object> expected = new Dictionary<String, Object>() { { "Key", "Value" } };
            controller.GetData(datalist, filter, expected);

            Dictionary<String, Object> actual = filter.AdditionalFilters;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetData_ReturnsPublicJsonData()
        {
            datalist.GetData().Returns(new DatalistData());

            JsonResult actual = controller.GetData(datalist, filter);
            DatalistData expectedData = datalist.GetData();

            Assert.AreEqual(JsonRequestBehavior.AllowGet, actual.JsonRequestBehavior);
            Assert.AreSame(expectedData, actual.Data);
        }

        #endregion

        #region Method: Role(DatalistFilter filter)

        [Test]
        public void Roles_GetsRolesData()
        {
            controller.When(sub => sub.GetData(Arg.Any<BaseDatalist<Role, RoleView>>(), filter, null)).DoNotCallBase();
            controller.GetData(Arg.Any<BaseDatalist<Role, RoleView>>(), filter, null).Returns(new JsonResult());
            GlobalizationManager.Provider = GlobalizationProviderFactory.CreateProvider();

            JsonResult expected = controller.GetData(null, filter, null);
            JsonResult actual = controller.Role(filter);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesUnitOfWork()
        {
            controller.Dispose();

            unitOfWork.Received().Dispose();
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
