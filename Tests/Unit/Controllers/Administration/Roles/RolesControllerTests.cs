using Moq;
using Moq.Protected;
using MvcTemplate.Controllers.Administration;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers.Administration
{
    [TestFixture]
    public class RolesControllerTests : AControllerTests
    {
        private Mock<IRoleValidator> validatorMock;
        private Mock<IRoleService> serviceMock;
        private RolesController controller;
        private RoleView role;

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IRoleValidator>(MockBehavior.Strict);
            serviceMock = new Mock<IRoleService>(MockBehavior.Strict);
            validatorMock.SetupAllProperties();
            serviceMock.SetupAllProperties();
            role = new RoleView();

            controller = new RolesController(
                serviceMock.Object, validatorMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.RouteData = new RouteData();
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            IQueryable<RoleView> expected = new[] { role }.AsQueryable();
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected);
            Object actual = controller.Index().Model;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsNewRoleView()
        {
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(It.IsAny<RoleView>()));
            RoleView actual = controller.Create().Model as RoleView;

            Assert.IsNotNull(actual.PrivilegesTree);
            Assert.IsNull(actual.Name);
        }

        [Test]
        public void Create_SeedsPrivilegesTree()
        {
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(It.IsAny<RoleView>()));
            RoleView role = controller.Create().Model as RoleView;

            controller.Create();

            serviceMock.Verify(mock => mock.SeedPrivilegesTree(role), Times.Once());
        }

        #endregion

        #region Method: Create([Bind(Exclude = "Id")] RoleView role)

        [Test]
        public void Create_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Create");
        }

        [Test]
        public void Create_SeedsPrivilegesTreeIfCanNotCreate()
        {
            validatorMock.Setup(mock => mock.CanCreate(role)).Returns(false);
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));

            controller.Create(role);

            serviceMock.Verify(mock => mock.SeedPrivilegesTree(role), Times.Once());
        }

        [Test]
        public void Create_ReturnsSameModelIfCanNotCreate()
        {
            validatorMock.Setup(mock => mock.CanCreate(role)).Returns(false);
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));

            Assert.AreSame(role, (controller.Create(role) as ViewResult).Model);
        }

        [Test]
        public void Create_CreatesRoleView()
        {
            validatorMock.Setup(mock => mock.CanCreate(role)).Returns(true);
            serviceMock.Setup(mock => mock.Create(role));

            controller.Create(role);

            serviceMock.Verify(mock => mock.Create(role), Times.Once());
        }

        [Test]
        public void Create_AfterSuccessfulCreateRedirectsToIndex()
        {
            validatorMock.Setup(mock => mock.CanCreate(role)).Returns(true);
            serviceMock.Setup(mock => mock.Create(role));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<RolesController> controllerMock = new Mock<RolesController>(serviceMock.Object, validatorMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.Create(role) as RedirectToRouteResult;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsRoleView()
        {
            serviceMock.Setup(mock => mock.GetView(role.Id)).Returns(role);

            Object actual = controller.Details(role.Id).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsRoleView()
        {
            serviceMock.Setup(mock => mock.GetView(role.Id)).Returns(role);

            Object actual = controller.Edit(role.Id).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView role)

        [Test]
        public void Edit_SeedsPrivilegesTreeIfCanNotEdit()
        {
            validatorMock.Setup(mock => mock.CanEdit(role)).Returns(false);
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));

            controller.Edit(role);

            serviceMock.Verify(mock => mock.SeedPrivilegesTree(role), Times.Once());
        }

        [Test]
        public void Edit_ReturnsSameModelIfCanNotEdit()
        {
            validatorMock.Setup(mock => mock.CanEdit(role)).Returns(false);
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));

            Object actual = (controller.Edit(role) as ViewResult).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Edit_EditsRoleView()
        {
            validatorMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(role));

            controller.Edit(role);

            serviceMock.Verify(mock => mock.Edit(role), Times.Once());
        }

        [Test]
        public void Edit_AfterSuccessfulEditRedirectsToIndex()
        {
            validatorMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(role));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<RolesController> controllerMock = new Mock<RolesController>(serviceMock.Object, validatorMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.Edit(role) as RedirectToRouteResult;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsRoleView()
        {
            serviceMock.Setup(mock => mock.GetView(role.Id)).Returns(role);

            Object actual = controller.Delete(role.Id).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(String id)

        [Test]
        public void DeleteConfirmed_DeletesRoleView()
        {
            serviceMock.Setup(mock => mock.Delete(role.Id));

            controller.DeleteConfirmed(role.Id);

            serviceMock.Verify(mock => mock.Delete(role.Id), Times.Once());
        }

        [Test]
        public void Delete_AfterSuccessfulDeleteRedirectsToIndexIfAuthorized()
        {
            serviceMock.Setup(mock => mock.Delete(role.Id));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<RolesController> controllerMock = new Mock<RolesController>(serviceMock.Object, validatorMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.DeleteConfirmed(role.Id) as RedirectToRouteResult;

            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
