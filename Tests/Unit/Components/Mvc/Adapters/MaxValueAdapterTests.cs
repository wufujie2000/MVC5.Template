using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class MaxValueAdapterTests
    {
        #region Method: GetClientValidationRules()

        [Test]
        public void GetClientValidationRules_ReturnsMaxRangeValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "MaxValue");
            MaxValueAdapter adapter = new MaxValueAdapter(metadata, new ControllerContext(), new MaxValueAttribute(128));
            String errorMessage = new MaxValueAttribute(128).FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ValidationParameters.Add("max", 128M);
            expected.ErrorMessage = errorMessage;
            expected.ValidationType = "range";

            Assert.AreEqual(expected.ValidationParameters["max"], actual.ValidationParameters["max"]);
            Assert.AreEqual(expected.ValidationType, actual.ValidationType);
            Assert.AreEqual(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
