using NUnit.Framework;
using System.Linq;
using System.Web.Mvc;
using Template.Components.Mvc.Providers;
using Template.Components.Mvc.Validators;
using Template.Tests.Objects.Components.Mvc.Providers;

namespace Template.Tests.Tests.Components.Mvc.Providers
{
    [TestFixture]
    public class DataTypeValidatorProviderTests
    {
        private ControllerContext context;
        private DataTypeValidatorProvider provider;

        [SetUp]
        public void SetUp()
        {
            provider = new DataTypeValidatorProvider();
            context = new ControllerContext();
        }

        #region Method: GetValidators(ModelMetadata metadata, ControllerContext context)

        [Test]
        public void GetValidators_GetsNoValidators()
        {
            var metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(() => null, typeof(ProviderTestModel), "Id");
            CollectionAssert.IsEmpty(provider.GetValidators(metadata, context));
        }

        [Test]
        public void GetValidators_GetsDateValidator()
        {
            var metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(() => null, typeof(ProviderTestModel), "Date");
            CollectionAssert.AreEqual(new[] { typeof(DateValidator) }, provider.GetValidators(metadata, context).Select(validator => validator.GetType()));
        }

        [Test]
        public void GetValidators_GetsNumericValidator()
        {
            var metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(() => null, typeof(ProviderTestModel), "Numeric");
            CollectionAssert.AreEqual(new[] { typeof(NumberValidator) }, provider.GetValidators(metadata, context).Select(validator => validator.GetType()));
        }

        #endregion
    }
}
