using Moq;
using NUnit.Framework;
using System;
using System.Web;
using System.Web.Mvc;
using Template.Components.Services;
using Template.Controllers.Profile;
using Template.Objects;
using Template.Tests.Helpers;
using Tests.Helpers;

namespace Template.Tests.Tests.Controllers.Profile
{
    [TestFixture]
    public class ProfileControllerTests
    {
        private Mock<IProfileService> serviceMock;
        private ProfileController controller;
        private HttpContextBase httpContext;
        private ProfileView profile;

        [SetUp]
        public void SetUp()
        {
            profile = ObjectFactory.CreateProfileView();
            serviceMock = new Mock<IProfileService>();
            controller = new ProfileController(serviceMock.Object);

            httpContext = new HttpMock().HttpContextBase;
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = httpContext;

            serviceMock.Setup(mock => mock.CanEdit(profile)).Returns(true);
            serviceMock.Setup(mock => mock.GetView(httpContext.User.Identity.Name)).Returns(profile);
        }

        #region Method: Edit()

        [Test]
        public void Edit_ReturnsCurrentProfileView()
        {
            var actual = (controller.Edit() as ViewResult).Model as ProfileView;

            serviceMock.Verify(mock => mock.GetView(httpContext.User.Identity.Name), Times.Once());
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
        public void Edit_ReturnsEmptyView()
        {
            Assert.IsNull((controller.Edit(profile) as ViewResult).Model);
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
        public void Delete_ReturnsCurrentProfileView()
        {
            var actual = (controller.Delete() as ViewResult).Model as ProfileView;

            serviceMock.Verify(mock => mock.GetView(httpContext.User.Identity.Name), Times.Once());
            Assert.AreEqual(profile, actual);
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

            serviceMock.Verify(mock => mock.Delete(httpContext.User.Identity.Name), Times.Once());
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
