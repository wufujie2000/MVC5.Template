using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Components.Services;
using Template.Controllers.Administration;
using Template.Objects;
using Template.Tests.Helpers;

namespace Template.Tests.Tests.Controllers.Administration.Roles
{
    [TestFixture]
    public class RolesControllerTests
    {
        private Mock<RolesController> controllerMock;
        private Mock<IRolesService> serviceMock;
        private RolesController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IRolesService>();
            controllerMock = new Mock<RolesController>(serviceMock.Object) { CallBase = true };
            controller = controllerMock.Object;
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsViewWithModels()
        {
            var expected = new[] { ObjectFactory.CreateRoleView() };
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected.AsQueryable());
            var actual = controller.Index() as ViewResult;

            Assert.AreEqual(expected, actual.Model);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsViewWithEmptyModel()
        {
            var actual = (controller.Create() as ViewResult).Model as RoleView;

            Assert.IsNull(actual.Name);
            Assert.IsNotNull(actual.Id);
            Assert.IsNotNull(actual.PrivilegesTree);
            CollectionAssert.IsEmpty(actual.RolePrivileges);
        }

        [Test]
        public void Create_SeedsNewPrivilegesTree()
        {
            var actual = (controller.Create() as ViewResult).Model as RoleView;

            serviceMock.Verify(mock => mock.SeedPrivilegesTree(actual), Times.Once());
        }

        #endregion

        #region Method: Create(RoleView role)

        [Test]
        public void Create_ReturnsViewIfCanNotCreate()
        {
            var roleView = new RoleView();
            serviceMock.Setup(mock => mock.CanCreate(roleView)).Returns(false);

            Assert.IsNotNull(controller.Create(roleView) as ViewResult);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            var roleView = new RoleView();
            serviceMock.Setup(mock => mock.CanCreate(roleView)).Returns(true);
            controller.Create(roleView);

            serviceMock.Verify(mock => mock.Create(roleView), Times.Once());
        }

        [Test]
        public void Create_RedirectsToIndexIfCreated()
        {
            var roleView = new RoleView();
            serviceMock.Setup(mock => mock.CanCreate(roleView)).Returns(true);
            var result = controller.Create(roleView) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsViewWithDetailsModel()
        {
            var expected = new RoleView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            var actual = (controller.Details("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsViewWithEditModel()
        {
            var expected = new RoleView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            var actual = (controller.Edit("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView role)

        [Test]
        public void Edit_ReturnsViewIfCanNotEdit()
        {
            var roleView = new RoleView();
            serviceMock.Setup(mock => mock.CanEdit(roleView)).Returns(false);

            Assert.IsNotNull(controller.Edit(roleView) as ViewResult);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            var roleView = new RoleView();
            serviceMock.Setup(mock => mock.CanEdit(roleView)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller.Edit(roleView);

            serviceMock.Verify(mock => mock.Edit(roleView), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            var roleView = new RoleView();
            serviceMock.Setup(mock => mock.CanEdit(roleView)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var routeValues = new RouteValueDictionary();
            routeValues["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(routeValues));
            var actual = controller.Edit(roleView) as RedirectToRouteResult;

            Assert.AreEqual(routeValues["action"], actual.RouteValues["action"]);
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            var roleView = new RoleView();
            serviceMock.Setup(mock => mock.CanEdit(roleView)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            var result = controller.Edit(roleView) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsViewWithDeleteModel()
        {
            var expected = new RoleView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            var actual = (controller.Delete("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(String id)

        [Test]
        public void DeleteConfirmed_ReturnsViewIfCanNotDelete()
        {
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(false);

            Assert.IsNotNull(controller.DeleteConfirmed("Test") as ViewResult);
        }

        [Test]
        public void DeleteConfirmed_CallsServiceDelete()
        {
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller.DeleteConfirmed("Test");

            serviceMock.Verify(mock => mock.Delete("Test"), Times.Once());
        }

        [Test]
        public void DeleteConfirmed_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var routeValues = new RouteValueDictionary();
            routeValues["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(routeValues));
            var actual = controller.DeleteConfirmed("Test") as RedirectToRouteResult;

            Assert.AreEqual(routeValues["action"], actual.RouteValues["action"]);
        }

        [Test]
        public void DeleteConfirmed_RedirectsToIndex()
        {
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            var result = controller.DeleteConfirmed("Test") as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion
    }
}
