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
        public void Index_ReturnsView()
        {
            Assert.IsNotNull(controller.Index() as ViewResult);
        }

        #endregion

        #region Method: Error()

        [Test]
        public void Error_CallsServiceAddSystemErrorMessage()
        {
            controller.Error();

            serviceMock.Verify(mock => mock.AddSystemErrorMessage(), Times.Once());
        }

        [Test]
        public void Error_ReturnsView()
        {
            Assert.IsNotNull(controller.Error() as ViewResult);
        }

        #endregion

        #region Method: NotFound()

        [Test]
        public void NotFound_CallsServiceAddPageNotFoundMessage()
        {
            controller.NotFound();

            serviceMock.Verify(mock => mock.AddPageNotFoundMessage(), Times.Once());
        }

        [Test]
        public void NotFound_ReturnsView()
        {
            Assert.IsNotNull(controller.NotFound() as ViewResult);
        }

        #endregion

        #region Method: Unauthorized()

        [Test]
        public void Unauthorized_CallsServiceAddUnauthorizedMessage()
        {
            controller.Unauthorized();

            serviceMock.Verify(mock => mock.AddUnauthorizedMessage(), Times.Once());
        }

        [Test]
        public void Unauthorized_ReturnsView()
        {
            Assert.IsNotNull(controller.Unauthorized() as ViewResult);
        }

        #endregion
    }
}
