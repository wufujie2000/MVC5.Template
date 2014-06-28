using Moq;
using Moq.Protected;
using MvcTemplate.Controllers.Administration;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers.Administration
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

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsAccountView()
        {
            AccountView expected = new AccountView();
            serviceMock.Setup(mock => mock.GetView<AccountView>("Test")).Returns(expected);
            AccountView actual = controller.Details("Test").Model as AccountView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsAccountView()
        {
            AccountEditView expected = new AccountEditView();
            serviceMock.Setup(mock => mock.GetView<AccountEditView>("Test")).Returns(expected);
            AccountEditView actual = controller.Edit("Test").Model as AccountEditView;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountEditView account)

        [Test]
        public void Edit_ReturnsSameModelIfCanNotEdit()
        {
            AccountEditView account = new AccountEditView();
            serviceMock.Setup(mock => mock.CanEdit(account)).Returns(false);

            Assert.AreSame(account, (controller.Edit(account) as ViewResult).Model);
        }

        [Test]
        public void Edit_EditsAccountView()
        {
            AccountEditView account = new AccountEditView();
            serviceMock.Setup(mock => mock.CanEdit(account)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(account));
            controller.Edit(account);

            serviceMock.Verify(mock => mock.Edit(account), Times.Once());
        }

        [Test]
        public void Edit_AfterSuccessfulEditRedirectsToIndexIfAuthorized()
        {
            AccountEditView account = new AccountEditView();
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
