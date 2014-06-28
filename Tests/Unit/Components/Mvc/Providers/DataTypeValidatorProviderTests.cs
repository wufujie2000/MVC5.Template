using MvcTemplate.Components.Mvc;
using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class DataTypeValidatorProviderTests
    {
        private DataTypeValidatorProvider provider;

        [SetUp]
        public void SetUp()
        {
            provider = new DataTypeValidatorProvider();
        }

        #region Method: GetValidators(ModelMetadata metadata, ControllerContext context)

        [Test]
        public void GetValidators_GetsNoValidators()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(ProviderModel), "Id");

            CollectionAssert.IsEmpty(provider.GetValidators(metadata, new ControllerContext()));
        }

        [Test]
        public void GetValidators_GetsDateValidator()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(ProviderModel), "Date");

            CollectionAssert.AreEqual(new[] { typeof(DateValidator) }, provider.GetValidators(metadata, new ControllerContext()).Select(validator => validator.GetType()));
        }

        [Test]
        public void GetValidators_GetsNumericValidator()
        {
            ModelMetadata metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(ProviderModel), "Numeric");

            CollectionAssert.AreEqual(new[] { typeof(NumberValidator) }, provider.GetValidators(metadata, new ControllerContext()).Select(validator => validator.GetType()));
        }

        #endregion
    }
}
