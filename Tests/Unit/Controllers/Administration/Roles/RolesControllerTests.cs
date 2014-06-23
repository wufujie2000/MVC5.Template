using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Controllers.Administration;
using Template.Objects;
using Template.Services;

namespace Template.Tests.Unit.Controllers.Administration
{
    [TestFixture]
    public class RolesControllerTests
    {
        private Mock<IRolesService> serviceMock;
        private RolesController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IRolesService>(MockBehavior.Strict);
            serviceMock.SetupAllProperties();

            controller = new RolesController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.RouteData = new RouteData();
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            IQueryable<RoleView> expected = new[] { new RoleView() }.AsQueryable();
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

            Assert.IsNull(actual.Name);
            Assert.IsNotNull(actual.PrivilegesTree);
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
            MethodInfo createMethod = controller
                .GetType()
                .GetMethods()
                .First(method =>
                    method.Name == "Create" &&
                    method.GetCustomAttribute<HttpPostAttribute>() != null);

            CustomAttributeData customParameterAttribute = createMethod.GetParameters().First().CustomAttributes.First();

            Assert.AreEqual(typeof(BindAttribute), customParameterAttribute.AttributeType);
            Assert.AreEqual("Exclude", customParameterAttribute.NamedArguments.First().MemberName);
            Assert.AreEqual("Id", customParameterAttribute.NamedArguments.First().TypedValue.Value);
        }

        [Test]
        public void Create_SeedsPrivilegesTreeIfCanNotCreate()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(false);

            controller.Create(role);

            serviceMock.Verify(mock => mock.SeedPrivilegesTree(role), Times.Once());
        }

        [Test]
        public void Create_ReturnsSameModelIfCanNotCreate()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(false);

            Assert.AreSame(role, (controller.Create(role) as ViewResult).Model);
        }

        [Test]
        public void Create_CreatesRoleView()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(true);
            serviceMock.Setup(mock => mock.Create(role));
            controller.Create(role);

            serviceMock.Verify(mock => mock.Create(role), Times.Once());
        }

        [Test]
        public void Create_AfterSuccessfulCreateRedirectsToIndex()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.CanCreate(role)).Returns(true);
            serviceMock.Setup(mock => mock.Create(role));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<RolesController> controllerMock = new Mock<RolesController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.Create(role) as RedirectToRouteResult;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsRoleView()
        {
            RoleView expected = new RoleView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            RoleView actual = controller.Details("Test").Model as RoleView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsRoleView()
        {
            RoleView expected = new RoleView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            RoleView actual = controller.Edit("Test").Model as RoleView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView role)

        [Test]
        public void Edit_SeedsPrivilegesTreeIfCanNotEdit()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(false);

            controller.Edit(role);

            serviceMock.Verify(mock => mock.SeedPrivilegesTree(role), Times.Once());
        }

        [Test]
        public void Edit_ReturnsSameModelIfCanNotEdit()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.SeedPrivilegesTree(role));
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(false);

            Assert.AreSame(role, (controller.Edit(role) as ViewResult).Model);
        }

        [Test]
        public void Edit_EditsRoleView()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(role));
            controller.Edit(role);

            serviceMock.Verify(mock => mock.Edit(role), Times.Once());
        }

        [Test]
        public void Edit_AfterSuccessfulEditRedirectsToIndex()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.CanEdit(role)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(role));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<RolesController> controllerMock = new Mock<RolesController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.Edit(role) as RedirectToRouteResult;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsRoleView()
        {
            RoleView expected = new RoleView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            RoleView actual = controller.Delete("Test").Model as RoleView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(String id)

        [Test]
        public void DeleteConfirmed_ReturnsModelIfCanNotDelete()
        {
            RoleView role = new RoleView();
            serviceMock.Setup(mock => mock.GetView(role.Id)).Returns(role);
            serviceMock.Setup(mock => mock.CanDelete(role.Id)).Returns(false);

            Assert.AreSame(role, (controller.DeleteConfirmed(role.Id) as ViewResult).Model);
        }

        [Test]
        public void DeleteConfirmed_DeletesRoleView()
        {
            String roleId = "Test";
            serviceMock.Setup(mock => mock.CanDelete(roleId)).Returns(true);
            serviceMock.Setup(mock => mock.Delete(roleId));
            controller.DeleteConfirmed(roleId);

            serviceMock.Verify(mock => mock.Delete(roleId), Times.Once());
        }

        [Test]
        public void Delete_AfterSuccessfulDeleteRedirectsToIndexIfAuthorized()
        {
            String roleId = "Test";
            serviceMock.Setup(mock => mock.CanDelete(roleId)).Returns(true);
            serviceMock.Setup(mock => mock.Delete(roleId));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<RolesController> controllerMock = new Mock<RolesController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.DeleteConfirmed(roleId) as RedirectToRouteResult;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
