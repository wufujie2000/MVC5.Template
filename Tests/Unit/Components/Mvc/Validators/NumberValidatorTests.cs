using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Mvc.Providers;
using Template.Components.Mvc.Validators;
using Template.Objects;
using Template.Resources.Shared;

namespace Template.Tests.Unit.Components.Mvc.Validators
{
    [TestFixture]
    public class NumberValidatorTests
    {
        private ModelMetadata metadata;
        private NumberValidator validator;

        [SetUp]
        public void SetUp()
        {
            metadata = new DisplayNameMetadataProvider().GetMetadataForProperty(null, typeof(ProfileView), "Username");
            validator = new NumberValidator(metadata, new ControllerContext());
        }

        #region Method: Validate(Object container)

        [Test]
        public void Validate_ReturnsEmptyEnumerable()
        {
            CollectionAssert.IsEmpty(validator.Validate(null));
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Test]
        public void GetClientValidationRules_HasNumberValidationType()
        {
            Assert.AreEqual("number", validator.GetClientValidationRules().First().ValidationType);
        }

        [Test]
        public void GetClientValidationRules_HasValidationMessage()
        {
            Assert.AreEqual(
                String.Format(Validations.FieldMustBeNumeric, metadata.GetDisplayName().ToLower()),
                validator.GetClientValidationRules().First().ErrorMessage);
        }

        [Test]
        public void GetClientValidationRules_ValidationParametersAreEmpty()
        {
            CollectionAssert.IsEmpty(validator.GetClientValidationRules().First().ValidationParameters);
        }

        [Test]
        public void GetClientValidationRules_HasOneRule()
        {
            Assert.AreEqual(1, validator.GetClientValidationRules().Count());
        }

        #endregion
    }
}
