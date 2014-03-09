using Moq;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Controllers.Profile;
using Template.Objects;

namespace Template.Tests.Tests.Controllers.Profile
{
    [TestFixture]
    public class ProfileControllerTests
    {
        private Mock<IProfileService> serviceMock;
        private ProfileController controller;
        private ProfileView profile;

        [SetUp]
        public void SetUp()
        {
            profile = new ProfileView();
            serviceMock = new Mock<IProfileService>();
            controller = new ProfileController(serviceMock.Object);
            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(true);
            serviceMock.Setup(mock => mock.CurrentAccountId).Returns("Test");
            serviceMock.Setup(mock => mock.GetView("Test")).Returns(profile);
        }

        #region Method: Edit()

        [Test]
        public void Edit_ReturnsViewWithCurrentProfileEditModel()
        {
            var actual = (controller.Edit() as ViewResult).Model as ProfileView;

            serviceMock.Verify(mock => mock.GetView("Test"), Times.Once());
            Assert.AreEqual(profile, actual);
        }

        #endregion

        #region Method: Edit(ProfileView profile)

        [Test]
        public void Edit_CallsServiceEdit()
        {
            controller.Edit(profile);

            serviceMock.Verify(mock => mock.Edit(profile), Times.Once());
        }

        [Test]
        public void Edit_ReturnsView()
        {
            Assert.IsNotNull(controller.Edit(profile) as ViewResult);
        }

        #endregion

        #region Method: Delete()

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
        public void Delete_ReturnsView()
        {
            Assert.IsNotNull(controller.Delete() as ViewResult);
        }

        #endregion

        #region Method: DeleteConfirmed(ProfileView profile)

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

            serviceMock.Verify(mock => mock.Delete("Test"), Times.Once());
        }

        [Test]
        public void DeleteConfirmed_RedirectsToAccountLogout()
        {
            serviceMock.Setup(mock => mock.CanDelete(profile)).Returns(true);
            var result = controller.DeleteConfirmed(profile) as RedirectToRouteResult;

            Assert.AreEqual("Logout", result.RouteValues["action"]);
            Assert.AreEqual("Account", result.RouteValues["controller"]);
        }

        #endregion
    }
}
