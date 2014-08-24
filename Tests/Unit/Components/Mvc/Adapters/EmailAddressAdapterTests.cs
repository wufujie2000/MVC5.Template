using MvcTemplate.Components.Mvc;
using MvcTemplate.Objects;
using MvcTemplate.Resources.Shared;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
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
                .GetMetadataForProperty(null, typeof(AccountView), "Email");
            adapter = new EmailAddressAdapter(metadata, new ControllerContext(), attribute);
        }

        #region Constructor: EmailAddressAdapter(ModelMetadata metadata, ControllerContext context, EmailAddressAttribute attribute)

        [Test]
        public void EmailAddressAdapter_SetsErrorMessage()
        {
            String expected = Validations.FieldIsNotValidEmail;
            String actual = attribute.ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Method: GetClientValidationRules()

        [Test]
        public void GetClientValidationRules_ReturnsEmailValidationRule()
        {
            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule()
            {
                ErrorMessage = String.Format(Validations.FieldIsNotValidEmail, metadata.GetDisplayName()),
                ValidationType = "email"
            };

            Assert.AreEqual(expected.ValidationType, actual.ValidationType);
            Assert.AreEqual(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
