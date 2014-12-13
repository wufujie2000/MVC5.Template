using MvcTemplate.Components.Alerts;
using MvcTemplate.Controllers;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Controllers
{
    [TestFixture]
    public class ValidatedControllerTests
    {
        private ValidatedController<IService, IValidator> controller;
        private IValidator validator;
        private IService service;

        [SetUp]
        public void SetUp()
        {
            service = Substitute.For<IService>();
            validator = Substitute.For<IValidator>();
            controller = Substitute.ForPartsOf<ValidatedController<IService, IValidator>>(service, validator);
        }

        #region Constructor: ValidatedController(TService service, TValidator validator)

        [Test]
        public void ValidatedController_SetsValidator()
        {
            IValidator actual = controller.Validator;
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
