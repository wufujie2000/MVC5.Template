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

namespace Template.Tests.Unit.Controllers.Administration.Users
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
            user = ObjectFactory.CreateUserView();
            serviceMock = new Mock<IUsersService>();
            serviceMock.Setup(mock => mock.CanEdit(user)).Returns(true);
            serviceMock.Setup(mock => mock.CanCreate(user)).Returns(true);
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(user);
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(true);
            controllerMock = new Mock<UsersController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller = controllerMock.Object;
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            var expected = new[] { user };
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected.AsQueryable());
            var actual = (controller.Index() as ViewResult).Model;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsEmptyUserView()
        {
            var actual = (controller.Create() as ViewResult).Model as UserView;

            Assert.IsNotNull(actual.Id);
            Assert.IsNull(actual.Person);
            Assert.IsNull(actual.Password);
            Assert.IsNull(actual.Username);
        }

        #endregion

        #region Method: Create(UserView user)

        [Test]
        public void Create_ReturnsEmptyViewIfCanNotCreate()
        {
            serviceMock.Setup(mock => mock.CanCreate(user)).Returns(false);

            Assert.IsNull((controller.Create(user) as ViewResult).Model);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            controller.Create(user);

            serviceMock.Verify(mock => mock.Create(user), Times.Once());
        }

        [Test]
        public void Create_AfterCreateRedirectsToIndex()
        {
            var result = controller.Create(user) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsUserView()
        {
            var actual = (controller.Details("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(user, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsUserView()
        {
            var actual = (controller.Edit("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(user, actual);
        }

        #endregion

        #region Method: Edit(UserView user)

        [Test]
        public void Edit_ReturnsEmptyViewIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(user)).Returns(false);

            Assert.IsNull((controller.Edit(user) as ViewResult).Model);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            controller.Edit(user);

            serviceMock.Verify(mock => mock.Edit(user), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToDefaultIfUserIsNoLongerAuthorizedForIndex()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            var expected = new RouteValueDictionary();
            expected["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(expected));
            var actual = (controller.Edit(user) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(expected["action"], actual["action"]);
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            var result = controller.Edit(user) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsViewWithDeleteModel()
        {
            var actual = (controller.Delete("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(user, actual);
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
