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

namespace Template.Tests.Unit.Controllers.Datalists
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

            Assert.AreEqual(filter, datalist.CurrentFilter);
        }

        [Test]
        public void GetData_SetsAdditionalFilters()
        {
            Dictionary<String, Object> additionalFilters = new Dictionary<String, Object>() { { "Key", "Value" } };
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
            DatalistData expectedData = new DatalistData();
            datalistMock.Setup(mock => mock.GetData()).Returns(expectedData);
            JsonResult jsonResult = controller.GetData(datalist, filter);
            Object actualData = jsonResult.Data;

            Assert.AreEqual(JsonRequestBehavior.AllowGet, jsonResult.JsonRequestBehavior);
            Assert.AreEqual(expectedData, actualData);
        }

        #endregion

        #region Method: Roles(DatalistFilter filter)

        [Test]
        public void Roles_CallsGetDataWithParameters()
        {
            JsonResult expectedResult = new JsonResult();
            controllerMock.Setup(mock => mock.GetData(It.IsAny<BaseDatalist<Role, RoleView>>(), filter, null)).Returns(expectedResult);
            JsonResult actualResult = controller.Role(filter);

            controllerMock.Verify(mock => mock.GetData(It.IsAny<BaseDatalist<Role, RoleView>>(), filter, null), Times.Once());
            Assert.AreEqual(expectedResult, actualResult);
        }

        #endregion
    }
}
