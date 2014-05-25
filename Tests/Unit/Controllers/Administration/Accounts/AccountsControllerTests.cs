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
    public class AccountsControllerTests
    {
        private Mock<IAccountsService> serviceMock;
        private AccountsController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IAccountsService>(MockBehavior.Strict);
            serviceMock.SetupAllProperties();

            controller = new AccountsController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.RouteData = new RouteData();
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            IQueryable<AccountView> expected = new[] { new AccountView() }.AsQueryable();
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected);
            Object actual = controller.Index().Model;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsNewAccountView()
        {
            AccountView actual = controller.Create().Model as AccountView;

            Assert.IsNull(actual.RoleId);
            Assert.IsNull(actual.RoleName);
            Assert.IsNull(actual.Password);
        }

        #endregion

        #region Method: Create(AccountView account)

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
        public void Create_ReturnsNullModelIfCanNotCreate()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.CanCreate(account)).Returns(false);

            Assert.IsNull((controller.Create(account) as ViewResult).Model);
        }

        [Test]
        public void Create_CreatesAccountView()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.CanCreate(account)).Returns(true);
            serviceMock.Setup(mock => mock.Create(account));
            controller.Create(account);

            serviceMock.Verify(mock => mock.Create(account), Times.Once());
        }

        [Test]
        public void Create_AfterSuccessfulCreateRedirectsToIndexIfAuthorized()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.CanCreate(account)).Returns(true);
            serviceMock.Setup(mock => mock.Create(account));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<AccountsController> controllerMock = new Mock<AccountsController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.Create(account) as RedirectToRouteResult;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsAccountView()
        {
            AccountView expected = new AccountView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            AccountView actual = controller.Details("Test").Model as AccountView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsAccountView()
        {
            AccountView expected = new AccountView();
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(expected);
            AccountView actual = controller.Edit("Test").Model as AccountView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountView account)

        [Test]
        public void Edit_ReturnsNullModelIfCanNotEdit()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.CanEdit(account)).Returns(false);

            Assert.IsNull((controller.Edit(account) as ViewResult).Model);
        }

        [Test]
        public void Edit_EditsAccountView()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.CanEdit(account)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(account));
            controller.Edit(account);

            serviceMock.Verify(mock => mock.Edit(account), Times.Once());
        }

        [Test]
        public void Edit_AfterSuccessfulEditRedirectsToIndexIfAuthorized()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.CanEdit(account)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(account));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<AccountsController> controllerMock = new Mock<AccountsController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.Edit(account) as RedirectToRouteResult;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
