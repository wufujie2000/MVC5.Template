using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Template.Controllers.Administration;
using Template.Objects;
using Template.Services;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Controllers.Administration
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
            IEnumerable<UserView> expected = new[] { user };
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected.AsQueryable());
            Object actual = controller.Index().Model;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsEmptyUserView()
        {
            UserView actual = controller.Create().Model as UserView;

            Assert.IsNotNull(actual.Id);
            Assert.IsNull(actual.Person);
            Assert.IsNull(actual.Password);
            Assert.IsNull(actual.Username);
        }

        #endregion

        #region Method: Create(UserView user)

        [Test]
        public void Create_CanNotOverpostId()
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
        public void Create_AfterSuccessfulCreateRedirectsToDefaultIfNotAuthorized()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            RouteValueDictionary expected = new RouteValueDictionary();
            expected["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(expected));
            RouteValueDictionary actual = (controller.Create(user) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(expected["action"], actual["action"]);
        }

        [Test]
        public void Create_AfterSuccessfulCreateRedirectsToIndexIfAuthorized()
        {
            RedirectToRouteResult result = controller.Create(user) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsUserView()
        {
            UserView actual = (controller.Details("Test") as ViewResult).Model as UserView;

            Assert.AreEqual(user, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsUserView()
        {
            UserView actual = (controller.Edit("Test") as ViewResult).Model as UserView;

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
        public void Edit_RedirectsToDefaultIfNotAuthorizedForIndex()
        {
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(false);
            RouteValueDictionary expected = new RouteValueDictionary();
            expected["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(expected));
            RouteValueDictionary actual = (controller.Edit(user) as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(expected["action"], actual["action"]);
        }

        [Test]
        public void Edit_RedirectsToIndexIfAuthorized()
        {
            RedirectToRouteResult result = controller.Edit(user) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_ReturnsViewWithDeleteModel()
        {
            UserView actual = (controller.Delete("Test") as ViewResult).Model as UserView;

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
            RouteValueDictionary expected = new RouteValueDictionary();
            expected["action"] = "DefaultRoute";

            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectToDefault").Returns(new RedirectToRouteResult(expected));
            RouteValueDictionary actual = (controller.DeleteConfirmed("Test") as RedirectToRouteResult).RouteValues;

            Assert.AreEqual(expected["action"], actual["action"]);
        }

        [Test]
        public void DeleteConfirmed_RedirectsToIndex()
        {
            RedirectToRouteResult result = controller.DeleteConfirmed("Test") as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion
    }
}
