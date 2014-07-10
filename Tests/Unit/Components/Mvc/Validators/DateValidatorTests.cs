using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Shared;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class DateValidatorTests
    {
        private ModelMetadata metadata;
        private DateValidator validator;

        [SetUp]
        public void SetUp()
        {
            metadata = new DisplayNameMetadataProvider().GetMetadataForProperty(null, typeof(AccountView), "Username");
            validator = new DateValidator(metadata, new ControllerContext());
        }

        #region Method: Validate(Object container)

        [Test]
        public void Validate_DoesNotValidate()
        {
            CollectionAssert.IsEmpty(validator.Validate(null));
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Test]
        public void GetClientValidationRules_HasDateValidationType()
        {
            Assert.AreEqual("date", validator.GetClientValidationRules().First().ValidationType);
        }

        [Test]
        public void GetClientValidationRules_SetsValidationMessages()
        {
            Assert.AreEqual(
                String.Format(Validations.FieldMustBeDate, metadata.GetDisplayName()),
                validator.GetClientValidationRules().First().ErrorMessage);
        }

        [Test]
        public void GetClientValidationRules_DoesNotHaveValidationParameters()
        {
            CollectionAssert.IsEmpty(validator.GetClientValidationRules().First().ValidationParameters);
        }

        [Test]
        public void GetClientValidationRules_ReturnsOnlyOneRule()
        {
            Assert.AreEqual(1, validator.GetClientValidationRules().Count());
        }

        #endregion
    }
}
