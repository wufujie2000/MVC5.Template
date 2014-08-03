using Datalist;
using MvcTemplate.Components.Datalists;
using MvcTemplate.Components.Mvc;
using MvcTemplate.Controllers.Datalist;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using MvcTemplate.Tests.Data;
using MvcTemplate.Tests.Helpers;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers.Datalist
{
    [TestFixture]
    public class DatalistControllerTests
    {
        private DatalistController controller;
        private AbstractDatalist datalist;
        private DatalistFilter filter;

        [SetUp]
        public void SetUp()
        {
            controller = new DatalistController(new UnitOfWork(new TestingContext()));
            HttpContext.Current = new HttpMock().HttpContext;
            datalist = Substitute.For<AbstractDatalist>();
            filter = new DatalistFilter();
        }

        [TearDown]
        public void TearDown()
        {
            LocalizationManager.Provider = null;
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
            JsonResult jsonResult = controller.GetData(datalist, filter);

            DatalistData expected = datalist.GetData();
            Object actual = jsonResult.Data;

            Assert.AreEqual(JsonRequestBehavior.AllowGet, jsonResult.JsonRequestBehavior);
            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Roles(DatalistFilter filter)

        [Test]
        public void Roles_GetsRolesData()
        {
            controller = Substitute.For<DatalistController>(Substitute.For<IUnitOfWork>());
            controller.GetData(Arg.Any<BaseDatalist<Role, RoleView>>(), filter, null).Returns(new JsonResult());

            LocalizationManager.Provider = new LanguageProviderMock().Provider;

            JsonResult expected = controller.GetData(null, filter, null);
            JsonResult actual = controller.Role(filter);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
