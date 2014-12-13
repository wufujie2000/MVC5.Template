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
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class ProfileControllerTests : AControllerTests
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
            validator = Substitute.For<IAccountValidator>();
            service = Substitute.For<IAccountService>();

            profileDelete = ObjectFactory.CreateProfileDeleteView("Edition");
            profileEdit = ObjectFactory.CreateProfileEditView("Edition");
            account = new AccountView();

            controller = new ProfileController(service, validator);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = HttpContextFactory.CreateHttpContextBase();

            accountId = controller.User.Identity.Name;
        }

        #region Method: Edit()

        [Test]
        public void Edit_OnGetRedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.Edit() as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual(2, actual.RouteValues.Count);
        }

        [Test]
        public void Edit_ReturnsCurrentProfileEditView()
        {
            service.Get<ProfileEditView>(accountId).Returns(new ProfileEditView());
            service.AccountExists(accountId).Returns(true);

            Object expected = service.Get<ProfileEditView>(accountId);
            Object actual = (controller.Edit() as ViewResult).Model;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Edit([Bind(Exclude = "Id")] ProfileEditView profile)

        [Test]
        public void Edit_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "Edit");
        }

        [Test]
        public void Edit_RedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.Edit(null) as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual(2, actual.RouteValues.Count);
        }

        [Test]
        public void Edit_BeforeEditingSetsProfileIdToCurrentAccountId()
        {
            service.AccountExists(accountId).Returns(true);
            validator.CanEdit(profileEdit).Returns(true);

            profileEdit.Id = accountId + "Edition";
            controller.Edit(profileEdit);

            validator.Received().CanEdit(Arg.Is<ProfileEditView>(view => view == profileEdit && profileEdit.Id == accountId));
            service.Received().Edit(Arg.Is<ProfileEditView>(view => view == profileEdit && profileEdit.Id == accountId));
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

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual(2, actual.RouteValues.Count);
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

        #region Method: DeleteConfirmed([Bind(Exclude = "Id")] AccountView profile)

        [Test]
        public void DeleteConfirmed_ProtectsFromOverpostingId()
        {
            ProtectsFromOverpostingId(controller, "DeleteConfirmed");
        }

        [Test]
        public void DeleteConfirmed_RedirectsToLogoutIfAccountDoesNotExist()
        {
            service.AccountExists(accountId).Returns(false);

            RedirectToRouteResult actual = controller.DeleteConfirmed(profileDelete) as RedirectToRouteResult;

            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual(2, actual.RouteValues.Count);
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
        public void DeleteConfirmed_BeforeDeletingSetsProfileIdToCurrentAccountId()
        {
            validator.CanDelete(profileDelete).Returns(true);
            service.AccountExists(accountId).Returns(true);

            profileDelete.Id = accountId + "Edition";
            controller.DeleteConfirmed(profileDelete);

            validator.Received().CanDelete(Arg.Is<ProfileDeleteView>(view => view == profileDelete && profileDelete.Id == accountId));
        }

        [Test]
        public void DeleteConfirmed_DeletesProfile()
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
            Assert.AreEqual(2, actual.RouteValues.Count);
        }

        #endregion
    }
}
