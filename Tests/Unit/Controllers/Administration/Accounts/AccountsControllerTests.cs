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
    public class AccountsControllerTests
    {
        private Mock<IAccountValidator> validatorMock;
        private Mock<IAccountService> serviceMock;
        private AccountsController controller;
        private AccountEditView accountEdit;
        private AccountView account;

        [SetUp]
        public void SetUp()
        {
            validatorMock = new Mock<IAccountValidator>(MockBehavior.Strict);
            serviceMock = new Mock<IAccountService>(MockBehavior.Strict);
            validatorMock.SetupAllProperties();
            serviceMock.SetupAllProperties();

            accountEdit = new AccountEditView();
            account = new AccountView();

            controller = new AccountsController(
                serviceMock.Object, validatorMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.RouteData = new RouteData();
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            IQueryable<AccountView> expected = new[] { account }.AsQueryable();
            serviceMock.Setup(mock => mock.GetViews()).Returns(expected);
            Object actual = controller.Index().Model;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_ReturnsAccountView()
        {
            serviceMock.Setup(mock => mock.GetView<AccountView>(account.Id)).Returns(account);

            Object actual = controller.Details(account.Id).Model;
            AccountView expected = account;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_ReturnsAccountView()
        {
            serviceMock.Setup(mock => mock.GetView<AccountEditView>(account.Id)).Returns(accountEdit);

            Object actual = controller.Edit(account.Id).Model;
            AccountEditView expected = accountEdit;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountEditView account)

        [Test]
        public void Edit_ReturnsSameModelIfCanNotEdit()
        {
            validatorMock.Setup(mock => mock.CanEdit(accountEdit)).Returns(false);

            Object actual = (controller.Edit(accountEdit) as ViewResult).Model;
            AccountEditView expected = accountEdit;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Edit_EditsAccountView()
        {
            validatorMock.Setup(mock => mock.CanEdit(accountEdit)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(accountEdit));

            controller.Edit(accountEdit);

            serviceMock.Verify(mock => mock.Edit(accountEdit), Times.Once());
        }

        [Test]
        public void Edit_AfterSuccessfulEditRedirectsToIndexIfAuthorized()
        {
            validatorMock.Setup(mock => mock.CanEdit(accountEdit)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(accountEdit));

            RedirectToRouteResult expected = new RedirectToRouteResult(new RouteValueDictionary());
            Mock<AccountsController> controllerMock = new Mock<AccountsController>(serviceMock.Object, validatorMock.Object) { CallBase = true };
            controllerMock.Protected().Setup<RedirectToRouteResult>("RedirectIfAuthorized", "Index").Returns(expected);
            RedirectToRouteResult actual = controllerMock.Object.Edit(accountEdit) as RedirectToRouteResult;

            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
