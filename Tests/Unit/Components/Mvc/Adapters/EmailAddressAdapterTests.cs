using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Mvc;
using Template.Resources.Shared;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class EmailAddressAdapterTests
    {
        private EmailAddressAttribute attribute;
        private EmailAddressAdapter adapter;
        private ModelMetadata metadata;

        [SetUp]
        public void SetUp()
        {
            attribute = new EmailAddressAttribute();
            metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(TestView), "Text");
            adapter = new EmailAddressAdapter(metadata, new ControllerContext(), attribute);
        }

        #region Constructor: EmailAddressAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)

        [Test]
        public void EmailAddressAdapter_SetsAttributeErrorMessage()
        {
            Assert.AreEqual(attribute.ErrorMessage, Validations.FieldIsNotValidEmail);
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Test]
        public void GetClientValidationRules_CreatesEmailValidationRule()
        {
            ModelClientValidationRule expected = adapter.GetClientValidationRules().First();
            String expectedErrorMessage = String.Format(Validations.FieldIsNotValidEmail, metadata.GetDisplayName());

            Assert.AreEqual(expectedErrorMessage, expected.ErrorMessage);
            Assert.AreEqual("email", expected.ValidationType);
        }

        #endregion
    }
}
