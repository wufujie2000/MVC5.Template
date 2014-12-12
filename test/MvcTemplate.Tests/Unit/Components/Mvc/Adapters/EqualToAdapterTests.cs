using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class EqualToAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Test]
        public void GetClientValidationRules_SetsOtherPropertyDisplayName()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "EqualTo");
            EqualToAttribute attribute = new EqualToAttribute("StringLength");
            attribute.OtherPropertyDisplayName = null;

            new EqualToAdapter(metadata, new ControllerContext(), attribute).GetClientValidationRules();

            String actual = attribute.OtherPropertyDisplayName;
            String expected = ResourceProvider.GetPropertyTitle(typeof(AdaptersModel), "EqualTo");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetClientValidationRules_ReturnsEqualToValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "EqualTo");
            EqualToAdapter adapter = new EqualToAdapter(metadata, new ControllerContext(), new EqualToAttribute("StringLength"));
            String errorMessage = new EqualToAttribute("StringLength").FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ValidationParameters.Add("other", "*.StringLength");
            expected.ErrorMessage = errorMessage;
            expected.ValidationType = "equalto";

            Assert.AreEqual(expected.ValidationParameters["other"], actual.ValidationParameters["other"]);
            Assert.AreEqual(expected.ValidationType, actual.ValidationType);
            Assert.AreEqual(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
