using MvcTemplate.Components.Alerts;
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
        private ValidatedControllerProxy controller;
        private IValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = Substitute.For<IValidator>();
            IService service = Substitute.For<IService>();
            controller = new ValidatedControllerProxy(service, validator);
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
        public void ValidatedController_SetsValidatorAlerts()
        {
            AlertsContainer expected = controller.Alerts;
            AlertsContainer actual = validator.Alerts;

            Assert.AreSame(expected, actual);
        }

        [Test]
        public void ValidatedController_SetsModelState()
        {
            ModelStateDictionary expected = controller.ModelState;
            ModelStateDictionary actual = validator.ModelState;

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
