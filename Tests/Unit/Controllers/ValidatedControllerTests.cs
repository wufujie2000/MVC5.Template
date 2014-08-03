using MvcTemplate.Services;
using MvcTemplate.Tests.Helpers;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class ValidatedControllerTests
    {
        private ValidatedControllerStub controller;
        private IValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = Substitute.For<IValidator>();
            IService service = Substitute.For<IService>();
            controller = new ValidatedControllerStub(service, validator);
            controller.ControllerContext = Substitute.For<ControllerContext>();
            controller.ControllerContext.HttpContext = new HttpMock().HttpContextBase;
        }

        [TearDown]
        public void TearDwon()
        {
            controller.Dispose();
        }

        #region Constructor: ValidatedController(TService service, TValidator validator)

        [Test]
        public void ValidatedController_SetsValidator()
        {
            IValidator actual = controller.BaseValidator;
            IValidator expected = validator;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void ServicedController_SetsModelState()
        {
            ModelStateDictionary expected = validator.ModelState;
            ModelStateDictionary actual = controller.ModelState;

            Assert.AreSame(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Test]
        public void Dispose_DisposesValidator()
        {
            controller.Dispose();

            validator.Received().Dispose();
        }

        [Test]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
