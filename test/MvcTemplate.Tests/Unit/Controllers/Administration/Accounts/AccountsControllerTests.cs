using MvcTemplate.Controllers.Administration;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers.Administration
{
    public class AccountsControllerTests : AControllerTests
    {
        private AccountCreateView accountCreate;
        private AccountsController controller;
        private IAccountValidator validator;
        private AccountEditView accountEdit;
        private IAccountService service;
        private AccountView account;

        public AccountsControllerTests()
        {
            validator = Substitute.For<IAccountValidator>();
            service = Substitute.For<IAccountService>();

            accountCreate = new AccountCreateView();
            accountEdit = new AccountEditView();
            account = new AccountView();

            controller = Substitute.ForPartsOf<AccountsController>(validator, service);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.RouteData = new RouteData();
        }

        #region Method: Index()

        [Fact]
        public void Index_GetsAccountViews()
        {
            service.GetViews().Returns(new AccountView[0].AsQueryable());

            Object actual = controller.Index().Model;
            Object expected = service.GetViews();

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Fact]
        public void Create_ReturnsEmptyView()
        {
            Assert.Null(controller.Create().Model);
        }

        #endregion

        #region Method: Create(AccountCreateView account)

        [Fact]
        public void Create_ProtectsFromOverpostingId()
        {
            ProtectsFromOverposting(controller, "Create", "Id");
        }

        [Fact]
        public void Create_ReturnsSameModelIfCanNotCreate()
        {
            validator.CanCreate(accountCreate).Returns(false);

            Object actual = (controller.Create(accountCreate) as ViewResult).Model;
            Object expected = accountCreate;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Create_CreatesAccount()
        {
            validator.CanCreate(accountCreate).Returns(true);

            controller.Create(accountCreate);

            service.Received().Create(accountCreate);
        }

        [Fact]
        public void Create_AfterCreateRedirectsToIndex()
        {
            validator.CanCreate(accountCreate).Returns(true);
            controller.When(sub => sub.RedirectIfAuthorized("Index")).DoNotCallBase();
            controller.RedirectIfAuthorized("Index").Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectIfAuthorized("Index");
            ActionResult actual = controller.Create(accountCreate);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Details(String id)

        [Fact]
        public void Details_ReturnsNotEmptyView()
        {
            service.Get<AccountView>(account.Id).Returns(account);
            controller.When(sub => sub.NotEmptyView(account)).DoNotCallBase();
            controller.NotEmptyView(account).Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.NotEmptyView(account);
            ActionResult actual = controller.Details(account.Id);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Fact]
        public void Edit_ReturnsNotEmptyView()
        {
            service.Get<AccountEditView>(accountEdit.Id).Returns(accountEdit);
            controller.When(sub => sub.NotEmptyView(accountEdit)).DoNotCallBase();
            controller.NotEmptyView(accountEdit).Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.NotEmptyView(accountEdit);
            ActionResult actual = controller.Edit(accountEdit.Id);

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: Edit(AccountEditView account)

        [Fact]
        public void Edit_ReturnsSameModelIfCanNotEdit()
        {
            validator.CanEdit(accountEdit).Returns(false);

            Object actual = (controller.Edit(accountEdit) as ViewResult).Model;
            Object expected = accountEdit;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void Edit_EditsAccount()
        {
            validator.CanEdit(accountEdit).Returns(true);

            controller.Edit(accountEdit);

            service.Received().Edit(accountEdit);
        }

        [Fact]
        public void Edit_AfterEditRedirectsToIndex()
        {
            validator.CanEdit(accountEdit).Returns(true);
            controller.When(sub => sub.RedirectIfAuthorized("Index")).DoNotCallBase();
            controller.RedirectIfAuthorized("Index").Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            ActionResult expected = controller.RedirectIfAuthorized("Index");
            ActionResult actual = controller.Edit(accountEdit);

            Assert.Same(expected, actual);
        }

        #endregion
    }
}
