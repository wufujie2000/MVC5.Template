using MvcTemplate.Components.Mvc;
using MvcTemplate.Tests.Objects;
using System;
using System.Linq;
using System.Web.Mvc;
using Xunit;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    public class FileSizeAdapterTests
    {
        #region GetClientValidationRules()

        [Fact]
        public void GetClientValidationRules_ReturnsFileSizeValidationRule()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "FileSize");
            FileSizeAdapter adapter = new FileSizeAdapter(metadata, new ControllerContext(), new FileSizeAttribute(12.25));
            String errorMessage = new FileSizeAttribute(12.25).FormatErrorMessage(metadata.GetDisplayName());

            ModelClientValidationRule actual = adapter.GetClientValidationRules().Single();
            ModelClientValidationRule expected = new ModelClientValidationRule();
            expected.ValidationParameters.Add("max", 12845056.00M);
            expected.ValidationType = "filesize";
            expected.ErrorMessage = errorMessage;

            Assert.Equal(expected.ValidationParameters["max"], actual.ValidationParameters["max"]);
            Assert.Equal(expected.ValidationParameters.Count, actual.ValidationParameters.Count);
            Assert.Equal(expected.ValidationType, actual.ValidationType);
            Assert.Equal(expected.ErrorMessage, actual.ErrorMessage);
        }

        #endregion
    }
}
