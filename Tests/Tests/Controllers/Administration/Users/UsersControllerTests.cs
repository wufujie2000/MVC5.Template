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
        private UserView user;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IUsersService>();
            controllerMock = new Mock<UsersController>(serviceMock.Object) { CallBase = true };
            controller = controllerMock.Object;
            user = new UserView();
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsViewWithModels()
        {
            var expected = new[] { user };
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
            serviceMock.Setup(mock => mock.CanCreate(user)).Returns(false);

            Assert.IsNotNull(controller.Create(user) as ViewResult);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            serviceMock.Setup(mock => mock.CanCreate(user)).Returns(true);
            controller.Create(user);

            serviceMock.Verify(mock => mock.Create(user), Times.Once());
        }

        [Test]
        public void Create_RedirectsToIndexIfCreated()
        {
            serviceMock.Setup(mock => mock.CanCreate(user)).Returns(true);
            var result = controller.Create(user) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsViewWithDetailsModel()
        {
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(user);
            var actual = (controller.Details("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(user, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsViewWithEditModel()
        {
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(user);
            var actual = (controller.Edit("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(user, actual);
        }

        #endregion

        #region Method: Edit(UserView user)

        [Test]
        public void Edit_ReturnsViewIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(user)).Returns(false);

            Assert.IsNotNull(controller.Edit(user) as ViewResult);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(user)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller.Edit(user);

            serviceMock.Verify(mock => mock.Edit(user), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            serviceMock.Setup(mock => mock.CanEdit(user)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var routeValues = new RouteValueDictionary();
            routeValues["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(routeValues));
            var actual = controller.Edit(user) as RedirectToRouteResult;

            Assert.AreEqual(routeValues["action"], actual.RouteValues["action"]);
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            serviceMock.Setup(mock => mock.CanEdit(user)).Returns(true);
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            var result = controller.Edit(user) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsViewWithDeleteModel()
        {
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(user);
            var actual = (controller.Delete("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(user, actual);
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
