using Datalist;
using MvcTemplate.Components.Datalists;
using MvcTemplate.Controllers;
using MvcTemplate.Data.Core;
using MvcTemplate.Objects;
using NSubstitute;
using System;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class DatalistControllerTests : IDisposable
    {
        private DatalistController controller;
        private AbstractDatalist datalist;
        private IUnitOfWork unitOfWork;
        private DatalistFilter filter;

        public DatalistControllerTests()
        {
            unitOfWork = Substitute.For<IUnitOfWork>();
            controller = Substitute.ForPartsOf<DatalistController>(unitOfWork);

            HttpContext.Current = HttpContextFactory.CreateHttpContext();
            datalist = Substitute.For<AbstractDatalist>();
            filter = new DatalistFilter();
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

        #region Method: GetData(AbstractDatalist datalist, DatalistFilter filter)

        [Fact]
        public void GetData_SetsCurrentFilter()
        {
            controller.GetData(datalist, filter);

            DatalistFilter actual = datalist.CurrentFilter;
            DatalistFilter expected = filter;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetData_ReturnsPublicJson()
        {
            datalist.GetData().Returns(new DatalistData());

            JsonResult actual = controller.GetData(datalist, filter);
            Object expectedData = datalist.GetData();

            Assert.Equal(JsonRequestBehavior.AllowGet, actual.JsonRequestBehavior);
            Assert.Same(expectedData, actual.Data);
        }

        #endregion

        #region Method: Role(DatalistFilter filter)

        [Fact]
        public void Role_ReturnsRolesData()
        {
            Object expected = GetData<Datalist<Role, RoleView>>(controller);
            Object actual = controller.Role(filter);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_UnitOfWork()
        {
            controller.Dispose();

            unitOfWork.Received().Dispose();
        }

        [Fact]
        public void Dispose_MultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion

        #region Test helpers

        private JsonResult GetData<TDatalist>(DatalistController controller) where TDatalist : AbstractDatalist
        {
            controller.When(sub => sub.GetData(Arg.Any<TDatalist>(), filter)).DoNotCallBase();
            controller.GetData(Arg.Any<TDatalist>(), filter).Returns(new JsonResult());

            return controller.GetData(null, filter);
        }

        #endregion
    }
}
