using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Alerts;
using Template.Controllers.Profile;
using Template.Objects;
using Template.Resources.Views.AccountView;
using Template.Services;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Controllers.Profile
{
    [TestFixture]
    public class ProfileControllerTests
    {
        private Mock<IAccountsService> serviceMock;
        private ProfileController controller;
        private ProfileEditView profile;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            profile = new ProfileEditView();

            serviceMock = new Mock<IAccountsService>(MockBehavior.Strict);
            serviceMock.SetupAllProperties();

            controller = new ProfileController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new HttpMock().HttpContextBase;
            accountId = controller.ControllerContext.HttpContext.User.Identity.Name;
        }

        #region Method: Edit()

        [Test]
        public void Edit_OnGetRedirectsToLogoutIfAccountDoesNotExist()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.Edit() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Edit_ReturnsCurrentProfileEditView()
        {
            serviceMock.Setup(mock => mock.GetView<ProfileEditView>(accountId)).Returns(new ProfileEditView());
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);

            ProfileEditView actual = (controller.Edit() as ViewResult).Model as ProfileEditView;
            ProfileEditView expected = serviceMock.Object.GetView<ProfileEditView>(accountId);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(ProfileEditView profile)

        [Test]
        public void Edit_RedirectsToLogoutIfAccountDoesNotExist()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.Edit(null) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Edit_EditsProfileIfCanEdit()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(profile));

            controller.Edit(profile);

            serviceMock.Verify(mock => mock.Edit(profile), Times.Once());
        }

        [Test]
        public void Edit_AddsProfileUpdatedMessage()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(true);
            serviceMock.Setup(mock => mock.Edit(profile));

            controller.Edit(profile);

            AlertMessage actual = serviceMock.Object.AlertMessages.First();

            Assert.AreEqual(MessagesContainer.DefaultFadeOut, actual.FadeOutAfter);
            Assert.AreEqual(Messages.ProfileUpdated, actual.Message);
            Assert.AreEqual(AlertMessageType.Success, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
        }

        [Test]
        public void Edit_DoesNotEditProfileIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(false);
            serviceMock.Setup(mock => mock.Edit(profile));

            controller.Edit(profile);

            serviceMock.Verify(mock => mock.Edit(profile), Times.Never());
        }

        [Test]
        public void Edit_ReturnsSameModel()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(false);
            serviceMock.Setup(mock => mock.Edit(profile));

            Assert.AreSame(profile, (controller.Edit(profile) as ViewResult).Model);
        }

        #endregion

        #region Method: Delete()

        [Test]
        public void Delete_RedirectsToLogoutIfAccountDoesNotExist()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.Delete() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Delete_AddsDeleteDisclaimerMessage()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            controller.Delete();

            AlertMessage actual = serviceMock.Object.AlertMessages.First();

            Assert.AreEqual(Messages.ProfileDeleteDisclaimer, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        [Test]
        public void Delete_ReturnsNullModel()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);

            Assert.IsNull((controller.Delete() as ViewResult).Model);
        }

        #endregion

        #region Method: DeleteConfirmed(AccountView profile)

        [Test]
        public void DeleteConfirmed_RedirectsToLogoutIfAccountDoesNotExist()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.DeleteConfirmed(account) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void DeleteConfirmed_AddsDeleteDisclaimerMessageIfCanNotDelete()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanDelete(account)).Returns(false);
            controller.DeleteConfirmed(account);

            AlertMessage actual = serviceMock.Object.AlertMessages.First();

            Assert.AreEqual(Messages.ProfileDeleteDisclaimer, actual.Message);
            Assert.AreEqual(AlertMessageType.Danger, actual.Type);
            Assert.AreEqual(String.Empty, actual.Key);
            Assert.AreEqual(0, actual.FadeOutAfter);
        }

        [Test]
        public void DeleteConfirmed_ReturnsNullModelIfCanNotDelete()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanDelete(account)).Returns(false);

            Assert.IsNull((controller.DeleteConfirmed(account) as ViewResult).Model);
        }

        [Test]
        public void DeleteConfirmed_DeletesProfileIfCanDelete()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanDelete(account)).Returns(true);
            serviceMock.Setup(mock => mock.Delete(accountId));
            controller.DeleteConfirmed(account);

            serviceMock.Verify(mock => mock.Delete(accountId), Times.Once());
        }

        [Test]
        public void DeleteConfirmed_AfterSuccessfulDeleteRedirectsToAuthLogout()
        {
            AccountView account = new AccountView();
            serviceMock.Setup(mock => mock.Delete(accountId));
            serviceMock.Setup(mock => mock.CanDelete(account)).Returns(true);
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            RedirectToRouteResult actual = controller.DeleteConfirmed(account) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        #endregion
    }
}
