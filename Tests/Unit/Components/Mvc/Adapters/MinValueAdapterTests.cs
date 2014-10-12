using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class MinValueAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Test]
        public void GetClientValidationRules_ReturnsMinRangeValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "MinValue");
            MinValueAdapter adapter = new MinValueAdapter(metadata, new ControllerContext(), new MinValueAttribute(128));
            String errorMessage = new MinValueAttribute(128).FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ValidationParameters.Add("min", 128M);
            expected.ErrorMessage = errorMessage;
            expected.ValidationType = "range";

            Assert.AreEqual(expected.ValidationParameters["min"], actual.ValidationParameters["min"]);
            Assert.AreEqual(expected.ValidationType, actual.ValidationType);
            Assert.AreEqual(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
