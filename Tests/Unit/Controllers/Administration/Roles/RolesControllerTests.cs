using MvcTemplate.Controllers.Administration;
using MvcTemplate.Objects;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcTemplate.Tests.Unit.Controllers.Administration
{
    [TestFixture]
    public class RolesControllerTests : AControllerTests
    {
        private RolesController controller;
        private IRoleValidator validator;
        private IRoleService service;
        private RoleView role;

        [SetUp]
        public void SetUp()
        {
            validator = Substitute.For<IRoleValidator>();
            service = Substitute.For<IRoleService>();
            role = new RoleView();

            controller = Substitute.ForPartsOf<RolesController>(service, validator);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.RouteData = new RouteData();
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsModelsView()
        {
            IQueryable<RoleView> expected = new[] { role }.AsQueryable();
            service.GetViews().Returns(expected);

            Object actual = controller.Index().Model;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Create()

        [Test]
        public void Create_ReturnsNewRoleView()
        {
            RoleView actual = controller.Create().Model as RoleView;

            Assert.IsNotNull(actual.PrivilegesTree);
            Assert.IsNull(actual.Name);
        }

        [Test]
        public void Create_SeedsPrivilegesTree()
        {
            controller.Create();

            service.SeedPrivilegesTree(role);
        }

        #endregion

        #region Method: Create([Bind(Exclude = "Id")] RoleView role)

        [Test]
        public void Create_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Create");
        }

        [Test]
        public void Create_SeedsPrivilegesTreeIfCanNotCreate()
        {
            validator.CanCreate(role).Returns(false);

            controller.Create(role);

            service.Received().SeedPrivilegesTree(role);
        }

        [Test]
        public void Create_ReturnsSameModelIfCanNotCreate()
        {
            validator.CanCreate(role).Returns(false);

            Assert.AreSame(role, (controller.Create(role) as ViewResult).Model);
        }

        [Test]
        public void Create_CreatesRoleView()
        {
            validator.CanCreate(role).Returns(true);

            controller.Create(role);

            service.Received().Create(role);
        }

        [Test]
        public void Create_AfterSuccessfulCreateRedirectsToIndex()
        {
            controller.RedirectIfAuthorized("Index").Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            validator.CanCreate(role).Returns(true);

            RedirectToRouteResult actual = controller.Create(role) as RedirectToRouteResult;
            RedirectToRouteResult expected = controller.RedirectIfAuthorized("Index");

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Details(String id)

        [Test]
        public void Details_OnNotFoundModelRedirectsToNotFound()
        {
            controller.RedirectToNotFound().Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            controller.When(control => control.RedirectToNotFound()).DoNotCallBase();
            service.GetView(String.Empty).Returns((RoleView)null);

            RedirectToRouteResult expected = controller.RedirectToNotFound();
            Object actual = controller.Details(String.Empty);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Details_ReturnsRoleView()
        {
            service.GetView(role.Id).Returns(role);

            Object actual = (controller.Details(role.Id) as ViewResult).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit(String id)

        [Test]
        public void Edit_OnNotFoundModelRedirectsToNotFound()
        {
            controller.RedirectToNotFound().Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            controller.When(control => control.RedirectToNotFound()).DoNotCallBase();
            service.GetView(String.Empty).Returns((RoleView)null);

            RedirectToRouteResult expected = controller.RedirectToNotFound();
            Object actual = controller.Edit(String.Empty);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Edit_ReturnsRoleView()
        {
            service.GetView(role.Id).Returns(role);

            Object actual = (controller.Edit(role.Id) as ViewResult).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit(RoleView role)

        [Test]
        public void Edit_SeedsPrivilegesTreeIfCanNotEdit()
        {
            validator.CanEdit(role).Returns(false);

            controller.Edit(role);

            service.Received().SeedPrivilegesTree(role);
        }

        [Test]
        public void Edit_ReturnsSameModelIfCanNotEdit()
        {
            validator.CanEdit(role).Returns(false);

            Object actual = (controller.Edit(role) as ViewResult).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Edit_EditsRoleView()
        {
            validator.CanEdit(role).Returns(true);

            controller.Edit(role);

            service.Received().Edit(role);
        }

        [Test]
        public void Edit_AfterSuccessfulEditRedirectsToIndex()
        {
            controller.RedirectIfAuthorized("Index").Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            validator.CanEdit(role).Returns(true);

            RedirectToRouteResult actual = controller.Edit(role) as RedirectToRouteResult;
            RedirectToRouteResult expected = controller.RedirectIfAuthorized("Index");

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Delete(String id)

        [Test]
        public void Delete_OnNotFoundModelRedirectsToNotFound()
        {
            controller.RedirectToNotFound().Returns(new RedirectToRouteResult(new RouteValueDictionary()));
            controller.When(control => control.RedirectToNotFound()).DoNotCallBase();
            service.GetView(String.Empty).Returns((RoleView)null);

            RedirectToRouteResult expected = controller.RedirectToNotFound();
            Object actual = controller.Delete(String.Empty);

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void Delete_ReturnsRoleView()
        {
            service.GetView(role.Id).Returns(role);

            Object actual = (controller.Delete(role.Id) as ViewResult).Model;
            RoleView expected = role;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(String id)

        [Test]
        public void DeleteConfirmed_DeletesRoleView()
        {
            controller.DeleteConfirmed(role.Id);

            service.Received().Delete(role.Id);
        }

        [Test]
        public void Delete_AfterSuccessfulDeleteRedirectsToIndexIfAuthorized()
        {
            controller.RedirectIfAuthorized("Index").Returns(new RedirectToRouteResult(new RouteValueDictionary()));

            RedirectToRouteResult actual = controller.DeleteConfirmed(role.Id) as RedirectToRouteResult;
            RedirectToRouteResult expected = controller.RedirectIfAuthorized("Index");

            Assert.AreSame(expected, actual);
        }

        #endregion
    }
}
