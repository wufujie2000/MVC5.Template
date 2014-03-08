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

namespace Template.Tests.Tests.Controllers.Administration.Users
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<UsersController> controllerMock;
        private Mock<IUsersService> serviceMock;
        private UsersController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IUsersService>();
            controllerMock = new Mock<UsersController>(serviceMock.Object) { CallBase = true };
            controller = controllerMock.Object;
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsViewWithModels()
        {
            var expected = new[] { new UserView() };
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected.AsQueryable());
            var actual = controller.Index() as ViewResult;

            Assert.AreEqual(expected, actual.Model);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsViewWithEmptyModel()
        {
            var actual = (controller.Create() as ViewResult).Model as UserView;

            Assert.IsNotNull(actual.Id);
            Assert.IsNull(actual.Password);
            Assert.IsNull(actual.Username);
            Assert.IsNull(actual.UserRoleId);
            Assert.IsNull(actual.NewPassword);
            Assert.IsNull(actual.UserLastName);
            Assert.IsNull(actual.UserRoleName);
            Assert.IsNull(actual.UserFirstName);
            Assert.IsNull(actual.UserDateOfBirth);
        }

        #endregion

        #region Method: Create(UserView user)

        [Test]
        public void Create_ReturnsViewIfCanNotCreate()
        {
            var UserView = new UserView();
            serviceMock.Setup(mock => mock.CanCreate(UserView)).Returns(false);

            Assert.IsNotNull(controller.Create(UserView) as ViewResult);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            var UserView = new UserView();
            serviceMock.Setup(mock => mock.CanCreate(UserView)).Returns(true);
            controller.Create(UserView);

            serviceMock.Verify(mock => mock.Create(UserView), Times.Once());
        }

        [Test]
        public void Create_RedirectsToIndexIfCreated()
        {
            var UserView = new UserView();
            serviceMock.Setup(mock => mock.CanCreate(UserView)).Returns(true);
            var result = controller.Create(UserView) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsViewWithDetailsModel()
        {
            var expected = new UserView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            var actual = (controller.Details("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsViewWithEditModel()
        {
            var expected = new UserView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            var actual = (controller.Edit("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(UserView user)

        [Test]
        public void Edit_ReturnsViewIfCanNotEdit()
        {
            var UserView = new UserView();
            serviceMock.Setup(mock => mock.CanEdit(UserView)).Returns(false);

            Assert.IsNotNull(controller.Edit(UserView) as ViewResult);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            var UserView = new UserView();
            serviceMock.Setup(mock => mock.CanEdit(UserView)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller.Edit(UserView);

            serviceMock.Verify(mock => mock.Edit(UserView), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            var UserView = new UserView();
            serviceMock.Setup(mock => mock.CanEdit(UserView)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var routeValues = new RouteValueDictionary();
            routeValues["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(routeValues));
            var actual = controller.Edit(UserView) as RedirectToRouteResult;

            Assert.AreEqual(routeValues["action"], actual.RouteValues["action"]);
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            var UserView = new UserView();
            serviceMock.Setup(mock => mock.CanEdit(UserView)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            var result = controller.Edit(UserView) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsViewWithDeleteModel()
        {
            var expected = new UserView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            var actual = (controller.Delete("Test") as ViewResult).Model as UserView;

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
