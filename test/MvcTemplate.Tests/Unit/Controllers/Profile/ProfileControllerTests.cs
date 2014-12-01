using MvcTemplate.Components.Alerts;
using MvcTemplate.Controllers;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class ProfileControllerTests
    {
        private ProfileDeleteView profileDelete;
        private ProfileController controller;
        private ProfileEditView profileEdit;
        private IAccountValidator validator;
        private IAccountService service;
        private AccountView account;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            validator = Substitute.For<IAccountValidator>();
            service = Substitute.For<IAccountService>();

            profileDelete = new ProfileDeleteView();
            profileEdit = new ProfileEditView();
            account = new AccountView();

            controller = new ProfileController(service, validator);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;
            accountId = httpContext.User.Identity.Name;
        }

        #region Method: Edit()

        [Test]
        public void Edit_OnGetRedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.Edit() as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        [Test]
        public void Edit_ReturnsCurrentProfileEditView()
        {
            service.GetView<ProfileEditView>(accountId).Returns(new ProfileEditView());
            service.AccountExists(accountId).Returns(true);

            Object expected = service.GetView<ProfileEditView>(accountId);
            Object actual = (controller.Edit() as ViewResult).Model;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit(ProfileEditView profile)

        [Test]
        public void Edit_RedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.Edit(null) as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        [Test]
        public void Edit_EditsProfile()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(true);

            controller.Edit(profileEdit);

            service.Received().Edit(profileEdit);
        }

        [Test]
        public void Edit_AddsProfileUpdatedMessage()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(true);

            controller.Edit(profileEdit);
            Alert actual = controller.Alerts.Single();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(Messages.ProfileUpdated, actual.Message);
            Assert.AreEqual(AlertTypes.Success, actual.Type);
        }

        [Test]
        public void Edit_DoesNotEditProfileIfCanNotEdit()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(false);

            controller.Edit(profileEdit);

            service.DidNotReceive().Edit(profileEdit);
        }

        [Test]
        public void Edit_ReturnsSameModel()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(false);

            Assert.AreSame(profileEdit, (controller.Edit(profileEdit) as ViewResult).Model);
        }

        #endregion

        #region Method: Delete()

        [Test]
        public void Delete_RedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.Delete() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Delete_AddsDeleteDisclaimerMessage()
        {
            service.AccountExists(accountId).Returns(true);

            controller.Delete();
            Alert actual = controller.Alerts.Single();

            Assert.AreEqual(Messages.ProfileDeleteDisclaimer, actual.Message);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        [Test]
        public void Delete_ReturnsNullModel()
        {
            service.AccountExists(accountId).Returns(true);

            Object model = (controller.Delete() as ViewResult).Model;

            Assert.IsNull(model);
        }

        #endregion

        #region Method: DeleteConfirmed(AccountView profile)

        [Test]
        public void DeleteConfirmed_RedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.DeleteConfirmed(profileDelete) as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        [Test]
        public void DeleteConfirmed_AddsDeleteDisclaimerMessageIfCanNotDelete()
        {
            validator.CanDelete(profileDelete).Returns(false);
            service.AccountExists(accountId).Returns(true);

            controller.DeleteConfirmed(profileDelete);
            Alert actual = controller.Alerts.Single();

            Assert.AreEqual(Messages.ProfileDeleteDisclaimer, actual.Message);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        [Test]
        public void DeleteConfirmed_ReturnsNullModelIfCanNotDelete()
        {
            validator.CanDelete(profileDelete).Returns(false);
            service.AccountExists(accountId).Returns(true);

            Object model = (controller.DeleteConfirmed(profileDelete) as ViewResult).Model;

            Assert.IsNull(model);
        }

        [Test]
        public void DeleteConfirmed_DeletesProfileIfCanDelete()
        {
            validator.CanDelete(profileDelete).Returns(true);
            service.AccountExists(accountId).Returns(true);

            controller.DeleteConfirmed(profileDelete);

            service.Received().Delete(accountId);
        }

        [Test]
        public void DeleteConfirmed_AfterSuccessfulDeleteRedirectsToAuthLogout()
        {
            validator.CanDelete(profileDelete).Returns(true);
            service.AccountExists(accountId).Returns(true);

            RedirectToRouteResult actual = controller.DeleteConfirmed(profileDelete) as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        #endregion
    }
}
