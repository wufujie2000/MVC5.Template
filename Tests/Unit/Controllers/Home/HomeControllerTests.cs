using Moq;
using NUnit.Framework;
using System.Web.Mvc;
using Template.Controllers.Home;
using Template.Services;

namespace Template.Tests.Unit.Controllers.Home
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<IHomeService> serviceMock;
        private HomeController controller;

        [SetUp]
        public void SetUp()
        {
            serviceMock = new Mock<IHomeService>();
            controller = new HomeController(serviceMock.Object);
        }

        #region Method: Index()

        [Test]
        public void Index_ReturnsIndexView()
        {
            Assert.IsInstanceOf<ViewResult>(controller.Index());
        }

        #endregion

        #region Method: Error()

        [Test]
        public void Error_AddsSystemErrorMessage()
        {
            controller.Error();

            serviceMock.Verify(mock => mock.AddSystemErrorMessage(), Times.Once());
        }

        [Test]
        public void Error_ReturnsView()
        {
            Assert.IsInstanceOf<ViewResult>(controller.Error());
        }

        #endregion

        #region Method: NotFound()

        [Test]
        public void NotFound_AddsPageNotFoundMessage()
        {
            controller.NotFound();

            serviceMock.Verify(mock => mock.AddPageNotFoundMessage(), Times.Once());
        }

        [Test]
        public void NotFound_ReturnsView()
        {
            Assert.IsInstanceOf<ViewResult>(controller.NotFound());
        }

        #endregion

        #region Method: Unauthorized()

        [Test]
        public void Unauthorized_AddsUnauthorizedMessage()
        {
            controller.Unauthorized();

            serviceMock.Verify(mock => mock.AddUnauthorizedMessage(), Times.Once());
        }

        [Test]
        public void Unauthorized_ReturnsView()
        {
            Assert.IsInstanceOf<ViewResult>(controller.Unauthorized());
        }

        #endregion
    }
}
