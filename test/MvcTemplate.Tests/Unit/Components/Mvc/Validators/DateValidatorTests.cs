using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Form;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class DateValidatorTests
    {
        private DateValidator validator;
        private ModelMetadata metadata;

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
        public void GetClientValidationRules_ReturnsDateValidationRule()
        {
            ModelClientValidationRule actual = validator.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule
            {
                ValidationType = "date",
                ErrorMessage = String.Format(Validations.FieldMustBeDate, metadata.GetDisplayName())
            };

            Assert.AreEqual(expected.ValidationType, actual.ValidationType);
            Assert.AreEqual(expected.ErrorMessage, actual.ErrorMessage);
            CollectionAssert.IsEmpty(actual.ValidationParameters);
        }

        #endregion
    }
}
