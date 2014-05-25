using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Controllers.Profile;
using Template.Objects;
using Template.Services;
using Template.Tests.Helpers;

namespace Template.Tests.Unit.Controllers.Profile
{
    [TestFixture]
    public class ProfileControllerTests
    {
        private Mock<IProfileService> serviceMock;
        private ProfileController controller;
        private ProfileView profile;
        private String accountId;

        [SetUp]
        public void SetUp()
        {
            profile = new ProfileView();

            serviceMock = new Mock<IProfileService>(MockBehavior.Strict);
            serviceMock.SetupAllProperties();

            controller = new ProfileController(serviceMock.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new HttpMock().HttpContextBase;
            accountId = controller.ControllerContext.HttpContext.User.Identity.Name;
        }

        #region Method: Edit()

        [Test]
        public void Edit_OnGetRedirectsToLogoutIfAccountDoesNotExistAnymore()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.Edit() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Edit_ReturnsCurrentProfileView()
        {
            serviceMock.Setup(mock => mock.GetView(accountId)).Returns(new ProfileView());
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);

            ProfileView actual = (controller.Edit() as ViewResult).Model as ProfileView;
            ProfileView expected = serviceMock.Object.GetView(accountId);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: Edit(ProfileView profile)

        [Test]
        public void Edit_OnPostRedirectsToLogoutIfAccountDoesNotExistAnymore()
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
        public void Edit_DoesNotEditProfileIfCanNotEdit()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(false);
            serviceMock.Setup(mock => mock.Edit(profile));

            controller.Edit(profile);

            serviceMock.Verify(mock => mock.Edit(profile), Times.Never());
        }

        [Test]
        public void Edit_ReturnsNullModel()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(false);
            serviceMock.Setup(mock => mock.Edit(profile));

            Assert.IsNull((controller.Edit(profile) as ViewResult).Model);
        }

        #endregion

        #region Method: Delete()

        [Test]
        public void Delete_RedirectsToLogoutIfAccountDoesNotExistAnymore()
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
            serviceMock.Setup(mock => mock.GetView(accountId)).Returns(profile);
            serviceMock.Setup(mock => mock.AddDeleteDisclaimerMessage());
            controller.Delete();

            serviceMock.Verify(mock => mock.AddDeleteDisclaimerMessage(), Times.Once());
        }

        [Test]
        public void Delete_SetsUsernameToEmptyString()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.GetView(accountId)).Returns(profile);
            serviceMock.Setup(mock => mock.AddDeleteDisclaimerMessage());
            profile.Username = "Username";

            controller.Delete();

            Assert.AreEqual(String.Empty, profile.Username);
        }

        [Test]
        public void Delete_ReturnsCurrentProfileView()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.GetView(accountId)).Returns(profile);
            serviceMock.Setup(mock => mock.AddDeleteDisclaimerMessage());

            ProfileView actual = (controller.Delete() as ViewResult).Model as ProfileView;
            ProfileView expected = profile;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(ProfileView profile)

        [Test]
        public void DeleteConfirmed_RedirectsToLogoutIfAccountDoesNotExistAnymore()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(false);
            RedirectToRouteResult actual = controller.DeleteConfirmed(profile) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void DeleteConfirmed_ReturnsNullModelIfCanNotDelete()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanDelete(profile)).Returns(false);

            Assert.IsNull((controller.DeleteConfirmed(profile) as ViewResult).Model);
        }

        [Test]
        public void DeleteConfirmed_DeletesProfileIfCanDelete()
        {
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            serviceMock.Setup(mock => mock.CanDelete(profile)).Returns(true);
            serviceMock.Setup(mock => mock.Delete(accountId));
            controller.DeleteConfirmed(profile);

            serviceMock.Verify(mock => mock.Delete(accountId), Times.Once());
        }

        [Test]
        public void DeleteConfirmed_AfterSuccessfulDeleteRedirectsToAuthLogout()
        {
            serviceMock.Setup(mock => mock.Delete(accountId));
            serviceMock.Setup(mock => mock.CanDelete(profile)).Returns(true);
            serviceMock.Setup(mock => mock.AccountExists(accountId)).Returns(true);
            RedirectToRouteResult actual = controller.DeleteConfirmed(profile) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        #endregion
    }
}
