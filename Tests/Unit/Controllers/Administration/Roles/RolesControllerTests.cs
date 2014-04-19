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

namespace Template.Tests.Unit.Controllers.Administration
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
            role = ObjectFactory.CreateRoleView();
            serviceMock = new Mock<IRolesService>();
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(true);
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(role);
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(true);
            controllerMock = new Mock<RolesController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller = controllerMock.Object;
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            var expected = new[] { role };
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected.AsQueryable());
            var actual = (controller.Index() as ViewResult).Model;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsEmptyRoleView()
        {
            var actual = (controller.Create() as ViewResult).Model as RoleView;

            Assert.IsNull(actual.Name);
            Assert.IsNotNull(actual.Id);
            Assert.IsNotNull(actual.PrivilegesTree);
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
        public void Create_ReturnsEmptyViewIfCanNotCreate()
        {
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(false);

            Assert.IsNull((controller.Create(role) as ViewResult).Model);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            controller.Create(role);

            serviceMock.Verify(mock => mock.Create(role), Times.Once());
        }

        [Test]
        public void Create_RedirectsToIndexIfCreated()
        {
            var result = controller.Create(role) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsRoleView()
        {
            var actual = (controller.Details("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(role, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsRoleView()
        {
            var actual = (controller.Edit("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(role, actual);
        }

        #endregion

        #region Method: Edit(RoleView role)

        [Test]
        public void Edit_ReturnsEmptyViewIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(false);

            Assert.IsNull((controller.Edit(role) as ViewResult).Model);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            controller.Edit(role);

            serviceMock.Verify(mock => mock.Edit(role), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var expected = new RouteValueDictionary();
            expected["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(expected));
            var actual = (controller.Edit(role) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(expected["action"], actual["action"]);
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            var result = controller.Edit(role) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsRoleView()
        {
            var actual = (controller.Delete("Test") as ViewResult).Model as RoleView;

            Assert.AreEqual(role, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(String id)

        [Test]
        public void DeleteConfirmed_ReturnsEmptyViewIfCanNotDelete()
        {
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(false);

            Assert.IsNull((controller.DeleteConfirmed("Test") as ViewResult).Model);
        }

        [Test]
        public void DeleteConfirmed_CallsServiceDelete()
        {
            controller.DeleteConfirmed("Test");

            serviceMock.Verify(mock => mock.Delete("Test"), Times.Once());
        }

        [Test]
        public void DeleteConfirmed_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var expected = new RouteValueDictionary();
            expected["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(expected));
            var actual = (controller.DeleteConfirmed("Test") as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(expected["action"], actual["action"]);
        }

        [Test]
        public void DeleteConfirmed_RedirectsToIndex()
        {
            var result = controller.DeleteConfirmed("Test") as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion
    }
}
