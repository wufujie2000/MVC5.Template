using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Template.Controllers.Administration;
using Template.Objects;
using Template.Services;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Controllers.Administration
{
    [TestFixture]
    public class AccountsControllerTests
    {
        private Mock<AccountsController> controllerMock;
        private Mock<IAccountsService> serviceMock;
        private AccountsController controller;
        private AccountView account;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IAccountsService>();
            account = ObjectFactory.CreateAccountView();
            serviceMock.Setup(mock => mock.CanEdit(account)).Returns(true);
            serviceMock.Setup(mock => mock.CanCreate(account)).Returns(true);
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(account);
            serviceMock.Setup(mock => mock.CanDelete("Test")).Returns(true);
            controllerMock = new Mock<AccountsController>(serviceMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<Boolean>("IsAuthorizedFor", "Index").Returns(true);
            controller = controllerMock.Object;
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            IEnumerable<AccountView> expected = new[] { account };
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected.AsQueryable());
            Object actual = controller.Index().Model;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsEmptyAccountView()
        {
            AccountView actual = controller.Create().Model as AccountView;

            Assert.IsNotNull(actual.Id);
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
        public void Create_ReturnsEmptyViewIfCanNotCreate()
        {
            serviceMock.Setup(mock => mock.CanCreate(account)).Returns(false);

            Assert.IsNull((controller.Create(account) as ViewResult).Model);
        }

        [Test]
        public void Create_CallsServiceCreate()
        {
            controller.Create(account);

            serviceMock.Verify(mock => mock.Create(account), Times.Once());
        }

        [Test]
        public void Create_AfterCreateRedirectsToIndex()
        {
            RedirectToRouteResult result = controller.Create(account) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsAccountView()
        {
            AccountView actual = controller.Details("Test").Model as AccountView;

            Assert.AreEqual(account, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsAccountView()
        {
            AccountView actual = controller.Edit("Test").Model as AccountView;

            Assert.AreEqual(account, actual);
        }

        #endregion

        #region Method: Edit(AccountView account)

        [Test]
        public void Edit_ReturnsEmptyViewIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.CanEdit(account)).Returns(false);

            Assert.IsNull((controller.Edit(account) as ViewResult).Model);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            controller.Edit(account);

            serviceMock.Verify(mock => mock.Edit(account), Times.Once());
        }

        [Test]
        public void Edit_RedirectsToIndex()
        {
            RedirectToRouteResult result = controller.Edit(account) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        #endregion
    }
}
