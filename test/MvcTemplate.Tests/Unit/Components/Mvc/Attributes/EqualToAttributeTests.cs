using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class EqualToAttributeTests
    {
        private EqualToAttribute attribute;

        [SetUp]
        public void SetUp()
        {
            attribute = new EqualToAttribute("Total");
        }

        #region Constructor: EqualToAttribute(String otherPropertyName)

        [Test]
        public void EqualToAttribute_SetsOtherPropertyName()
        {
            String actual = new EqualToAttribute("Other").OtherPropertyName;
            String expected = "Other";

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: FormatErrorMessage(String name)

        [Test]
        public void FormatErrorMessage_FormatsErrorMessage()
        {
            attribute.OtherPropertyDisplayName = "Other";

            String expected = String.Format(Validations.FieldMustBeEqualTo, "Sum", attribute.OtherPropertyDisplayName);
            String actual = attribute.FormatErrorMessage("Sum");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: IsValid(Object value, ValidationContext validationContext)

        [Test]
        public void IsValid_OnEqualValuesReturnsNull()
        {
            AttributesModel model = new AttributesModel();
            ValidationContext context = new ValidationContext(model);

            Assert.IsNull(attribute.GetValidationResult(model.Sum, context));
        }

        [Test]
        public void IsValid_SetsOtherPropertyDisplayName()
        {
            AttributesModel model = new AttributesModel { Total = 10 };
            ValidationContext context = new ValidationContext(model);

            attribute.GetValidationResult(model.Sum, context);

            String expected = ResourceProvider.GetPropertyTitle(context.ObjectType, attribute.OtherPropertyName);
            String actual = attribute.OtherPropertyDisplayName;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsValid_OnNotEqualValuesReturnsValidationResult()
        {
            AttributesModel model = new AttributesModel { Total = 10 };
            ValidationContext context = new ValidationContext(model);

            ValidationResult expected = new ValidationResult(attribute.FormatErrorMessage(context.DisplayName));
            ValidationResult actual = attribute.GetValidationResult(model.Sum, context);

            Assert.AreEqual(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
