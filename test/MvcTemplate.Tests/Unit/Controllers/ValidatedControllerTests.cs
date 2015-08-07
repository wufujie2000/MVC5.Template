using MvcTemplate.Components.Alerts;
using MvcTemplate.Services;
using MvcTemplate.Validators;
using NSubstitute;
using System;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Controllers
{
    public class ValidatedControllerTests
    {
        private ValidatedControllerProxy controller;
        private IValidator validator;
        private IService service;

        public ValidatedControllerTests()
        {
            service = Substitute.For<IService>();
            validator = Substitute.For<IValidator>();
            controller = Substitute.ForPartsOf<ValidatedControllerProxy>(validator, service);
        }

        #region Constructor: ValidatedController(TService service, TValidator validator)

        [Fact]
        public void ValidatedController_SetsValidator()
        {
            IValidator actual = controller.Validator;
            IValidator expected = validator;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void ValidatedController_SetsValidatorAlerts()
        {
            AlertsContainer expected = controller.Alerts;
            AlertsContainer actual = validator.Alerts;

            Assert.Same(expected, actual);
        }

        [Fact]
        public void ValidatedController_SetsModelState()
        {
            ModelStateDictionary expected = controller.ModelState;
            ModelStateDictionary actual = validator.ModelState;

            Assert.Same(expected, actual);
        }

        #endregion

        #region Method: OnActionExecuting(ActionExecutingContext filterContext)

        [Fact]
        public void OnActionExecuting_SetsServiceCurrentAccountId()
        {
            controller.When(sub => { String get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns("Test");
            validator.CurrentAccountId = null;
            service.CurrentAccountId = null;

            controller.BaseOnActionExecuting(null);

            String expected = controller.CurrentAccountId;
            String actual = service.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OnActionExecuting_SetsValidatorCurrentAccountId()
        {
            controller.When(sub => { String get = sub.CurrentAccountId; }).DoNotCallBase();
            controller.CurrentAccountId.Returns("Test");
            validator.CurrentAccountId = null;
            service.CurrentAccountId = null;

            controller.BaseOnActionExecuting(null);

            String expected = controller.CurrentAccountId;
            String actual = validator.CurrentAccountId;

            Assert.Equal(expected, actual);
        }

        #endregion

        #region Method: Dispose()

        [Fact]
        public void Dispose_DisposesValidatorAndService()
        {
            controller.Dispose();

            service.Received().Dispose();
            validator.Received().Dispose();
        }

        [Fact]
        public void Dispose_CanBeCalledMultipleTimes()
        {
            controller.Dispose();
            controller.Dispose();
        }

        #endregion
    }
}
