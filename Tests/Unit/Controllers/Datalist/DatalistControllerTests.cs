using Datalist;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Template.Components.Datalists;
using Template.Controllers.Datalist;
using Template.Data.Core;
using Template.Objects;
using Template.Tests.Data;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Controllers.Datalist
{
    [TestFixture]
    public class DatalistControllerTests
    {
        private Mock<DatalistController> controllerMock;
        private Mock<AbstractDatalist> datalistMock;
        private DatalistController controller;
        private AbstractDatalist datalist;
        private DatalistFilter filter;

        [SetUp]
        public void SetUp()
        {
            controllerMock = new Mock<DatalistController>(new UnitOfWork(new TestingContext())) { CallBase = true };
            HttpContext.Current = new HttpMock().HttpContext;
            datalistMock = new Mock<AbstractDatalist>();
            controller = controllerMock.Object;
            datalist = datalistMock.Object;
            filter = new DatalistFilter();
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Method: GetData(AbstractDatalist datalist, DatalistFilter filter, Dictionary<String, Object> additionalFilters = null)

        [Test]
        public void GetData_SetsDatalistCurrentFilter()
        {
            controller.GetData(datalist, filter);

            DatalistFilter expected = filter;
            DatalistFilter actual = datalist.CurrentFilter;

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
            datalistMock.Setup(mock => mock.GetData()).Returns(new DatalistData());
            JsonResult jsonResult = controller.GetData(datalist, filter);

            DatalistData expected = datalistMock.Object.GetData();
            DatalistData actual = jsonResult.Data as DatalistData;

            Assert.AreEqual(JsonRequestBehavior.AllowGet, jsonResult.JsonRequestBehavior);
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Roles(DatalistFilter filter)

        [Test]
        public void Roles_GetsRolesData()
        {
            controllerMock.Setup(mock => mock.GetData(It.IsAny<BaseDatalist<Role, RoleView>>(), filter, null)).Returns(new JsonResult());

            JsonResult expected = controllerMock.Object.GetData(null, filter, null);
            JsonResult actual = controller.Role(filter);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
