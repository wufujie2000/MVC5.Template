using MvcTemplate.Components.Alerts;
using MvcTemplate.Controllers;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Views.AccountView;
using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
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
        private ProfileController controller;
        private IAccountValidator validator;
        private IAccountService service;
        private ProfileEditView profile;
        private AccountView account;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            HttpContextBase httpContext = HttpContextFactory.CreateHttpContextBase();
            validator = Substitute.For<IAccountValidator>();
            service = Substitute.For<IAccountService>();

            profile = new ProfileEditView();
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
        public void Edit_EditsProfileIfCanEdit()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profile).Returns(true);

            controller.Edit(profile);

            service.Received().Edit(profile);
        }

        [Test]
        public void Edit_AddsProfileUpdatedMessage()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profile).Returns(true);

            controller.Edit(profile);
            Alert actual = controller.Alerts.Single();

            Assert.AreEqual(AlertsContainer.DefaultFadeout, actual.FadeoutAfter);
            Assert.AreEqual(Messages.ProfileUpdated, actual.Message);
            Assert.AreEqual(AlertTypes.Success, actual.Type);
        }

        [Test]
        public void Edit_DoesNotEditProfileIfCanNotEdit()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profile).Returns(false);

            controller.Edit(profile);

            service.DidNotReceive().Edit(profile);
        }

        [Test]
        public void Edit_ReturnsSameModel()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profile).Returns(false);

            Assert.AreSame(profile, (controller.Edit(profile) as ViewResult).Model);
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

            RedirectToRouteResult actual = controller.DeleteConfirmed(account) as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        [Test]
        public void DeleteConfirmed_AddsDeleteDisclaimerMessageIfCanNotDelete()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanDelete(account).Returns(false);

            controller.DeleteConfirmed(account);
            Alert actual = controller.Alerts.Single();

            Assert.AreEqual(Messages.ProfileDeleteDisclaimer, actual.Message);
            Assert.AreEqual(AlertTypes.Danger, actual.Type);
            Assert.AreEqual(0, actual.FadeoutAfter);
        }

        [Test]
        public void DeleteConfirmed_ReturnsNullModelIfCanNotDelete()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanDelete(account).Returns(false);

            Object model = (controller.DeleteConfirmed(account) as ViewResult).Model;

            Assert.IsNull(model);
        }

        [Test]
        public void DeleteConfirmed_DeletesProfileIfCanDelete()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanDelete(account).Returns(true);

            controller.DeleteConfirmed(account);

            service.Received().Delete(accountId);
        }

        [Test]
        public void DeleteConfirmed_AfterSuccessfulDeleteRedirectsToAuthLogout()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanDelete(account).Returns(true);

            RedirectToRouteResult actual = controller.DeleteConfirmed(account) as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
        }

        #endregion
    }
}
