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

namespace Template.Tests.Tests.Controllers.Administration.Roles
{
    [TestFixture]
    public class RolesControllerTests
    {
        private Mock<RolesController> controllerMock;
        private Mock<IRolesService> serviceMock;
        private RolesController controller;
        private RoleView role;

        [SetUp]
        public void SetUp()
        {
            role = new RoleView();
            serviceMock = new Mock<IRolesService>();
            controllerMock = new Mock<RolesController>(serviceMock.Object) { CallBase = true };
            controller = controllerMock.Object;
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsViewWithModels()
        {
            var expected = new[] { role };
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
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(false);

            Assert.IsNotNull(controller.Create(role) as ViewResult);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(true);
            controller.Create(role);

            serviceMock.Verify(mock => mock.Create(role), Times.Once());
        }

        [Test]
        public void Create_RedirectsToIndexIfCreated()
        {
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(true);
            var result = controller.Create(role) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsViewWithDetailsModel()
        {
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(role);
            var actual = (controller.Details("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(role, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsViewWithEditModel()
        {
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(role);
            var actual = (controller.Edit("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(role, actual);
        }

        #endregion

        #region Method: Edit(RoleView role)

        [Test]
        public void Edit_ReturnsViewIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(false);

            Assert.IsNotNull(controller.Edit(role) as ViewResult);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller.Edit(role);

            serviceMock.Verify(mock => mock.Edit(role), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var routeValues = new RouteValueDictionary();
            routeValues["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(routeValues));
            var actual = controller.Edit(role) as RedirectToRouteResult;

            Assert.AreEqual(routeValues["action"], actual.RouteValues["action"]);
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            var result = controller.Edit(role) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsViewWithDeleteModel()
        {
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(role);
            var actual = (controller.Delete("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(role, actual);
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
