using Moq;
using NUnit.Framework;
using System;
using System.Security.Principal;
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
        private Mock<IIdentity> identityMock;
        private ProfileView profile;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IProfileService>();
            profile = ObjectFactory.CreateProfileView();
            controller = new ProfileController(serviceMock.Object);

            HttpMock httpMock = new HttpMock();
            identityMock = httpMock.IdentityMock;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpMock.HttpContextBase;

            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(true);
            serviceMock.Setup(mock => mock.GetView(identityMock.Object.Name)).Returns(profile);
            serviceMock.Setup(mock => mock.AccountExists(identityMock.Object.Name)).Returns(true);
        }

        #region Method: Edit()

        [Test]
        public void Edit_OnGetRedirectsToLogoutIfAccountDoesNotExistAnymore()
        {
            identityMock.Setup<String>(mock => mock.Name).Returns("NotExistingId");

            RedirectToRouteResult actual = controller.Edit() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Edit_ReturnsCurrentProfileView()
        {
            ProfileView actual = (controller.Edit() as ViewResult).Model as ProfileView;
            
            serviceMock.Verify(mock => mock.GetView(identityMock.Object.Name), Times.Once());
            Assert.AreEqual(profile, actual);
        }

        #endregion

        #region Method: Edit(ProfileView profile)

        [Test]
        public void Edit_OnPostRedirectsToLogoutIfAccountDoesNotExistAnymore()
        {
            identityMock.Setup<String>(mock => mock.Name).Returns("NotExistingId");

            RedirectToRouteResult actual = controller.Edit(profile) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Edit_CallsServiceEdit()
        {
            controller.Edit(profile);

            serviceMock.Verify(mock => mock.Edit(profile), Times.Once());
        }

        [Test]
        public void Edit_ReturnsEmptyView()
        {
            Assert.IsNull((controller.Edit(profile) as ViewResult).Model);
        }

        #endregion

        #region Method: Delete()

        [Test]
        public void Delete_RedirectsToLogoutIfAccountDoesNotExistAnymore()
        {
            identityMock.Setup<String>(mock => mock.Name).Returns("NotExistingId");

            RedirectToRouteResult actual = controller.Delete() as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void Delete_CallsServiceAddDeleteDisclaimerMessage()
        {
            controller.Delete();

            serviceMock.Verify(mock => mock.AddDeleteDisclaimerMessage(), Times.Once());
        }

        [Test]
        public void Delete_SetsUsernameToEmptyString()
        {
            profile.Username = "Username";
            controller.Delete();

            Assert.AreEqual(String.Empty, profile.Username);
        }

        [Test]
        public void Delete_ReturnsCurrentProfileView()
        {
            ProfileView actual = (controller.Delete() as ViewResult).Model as ProfileView;

            serviceMock.Verify(mock => mock.GetView(identityMock.Object.Name), Times.Once());
            Assert.AreEqual(profile, actual);
        }

        #endregion

        #region Method: DeleteConfirmed(ProfileView profile)

        [Test]
        public void DeleteConfirmed_RedirectsToLogoutIfAccountDoesNotExistAnymore()
        {
            identityMock.Setup<String>(mock => mock.Name).Returns("NotExistingId");

            RedirectToRouteResult actual = controller.DeleteConfirmed(profile) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        [Test]
        public void DeleteConfirmed_ReturnsViewIfCanNotDelete()
        {
            serviceMock.Setup(mock => mock.CanDelete(profile)).Returns(false);

            Assert.IsNotNull(controller.DeleteConfirmed(profile) as ViewResult);
        }

        [Test]
        public void DeleteConfirmed_CallsServiceDelete()
        {
            serviceMock.Setup(mock => mock.CanDelete(profile)).Returns(true);
            controller.DeleteConfirmed(profile);

            serviceMock.Verify(mock => mock.Delete(identityMock.Object.Name), Times.Once());
        }

        [Test]
        public void DeleteConfirmed_RedirectsToAccountLogout()
        {
            serviceMock.Setup(mock => mock.CanDelete(profile)).Returns(true);
            RedirectToRouteResult actual = controller.DeleteConfirmed(profile) as RedirectToRouteResult;

            Assert.AreEqual("Logout", actual.RouteValues["action"]);
            Assert.AreEqual("Auth", actual.RouteValues["controller"]);
        }

        #endregion
    }
}
