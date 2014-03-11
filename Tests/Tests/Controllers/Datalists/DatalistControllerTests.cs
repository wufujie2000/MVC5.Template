using Datalist;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Template.Components.Datalists;
using Template.Controllers.Datalist;
using Tests.Helpers;

namespace Template.Tests.Tests.Controllers.Datalists
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
            controllerMock = new Mock<DatalistController>() { CallBase = true };
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

            Assert.AreEqual(filter, datalist.CurrentFilter);
        }

        [Test]
        public void GetData_SetsAdditionalFilters()
        {
            var additionalFilters = new Dictionary<String, Object>() { { "Key", "Value" } };
            controller.GetData(datalist, filter, additionalFilters);

            Assert.AreEqual(additionalFilters, filter.AdditionalFilters);
        }

        [Test]
        public void GetData_CallsAbstractDatalistGetData()
        {
            controller.GetData(datalist, filter);

            datalistMock.Verify(mock => mock.GetData(), Times.Once());
        }

        [Test]
        public void GetData_ReturnsPublicJsonData()
        {
            var expectedData = new DatalistData();
            datalistMock.Setup(mock => mock.GetData()).Returns(expectedData);
            var jsonResult = controller.GetData(datalist, filter);
            var actualData = jsonResult.Data;

            Assert.AreEqual(JsonRequestBehavior.AllowGet, jsonResult.JsonRequestBehavior);
            Assert.AreEqual(expectedData, actualData);
        }

        #endregion

        #region Method: Roles(DatalistFilter filter)

        [Test]
        public void Roles_CallsGetDataWithParameters()
        {
            var expectedResult = new JsonResult();
            controllerMock.Setup(mock => mock.GetData(It.IsAny<RolesDatalist>(), filter, null)).Returns(expectedResult);
            var actualResult = controller.Roles(filter);

            controllerMock.Verify(mock => mock.GetData(It.IsAny<RolesDatalist>(), filter, null), Times.Once());
            Assert.AreEqual(expectedResult, actualResult);
        }

        #endregion
    }
}
