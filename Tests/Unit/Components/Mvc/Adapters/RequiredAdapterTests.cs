using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Template.Components.Mvc.Adapters;
using Template.Resources.Shared;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Components.Mvc.Adapters
{
    [TestFixture]
    public class RequiredAdapterTests
    {
        private RequiredAttribute requiredAttribute;
        private ModelMetadata metadata;

        [SetUp]
        public void SetUp()
        {
            requiredAttribute = new RequiredAttribute();
            metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(TestView), "Text");
            new RequiredAdapter(metadata, new ControllerContext(), requiredAttribute);
        }

        #region Constructor: RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)

        [Test]
        public void RequiredAdapter_SetsAttributeErrorMessageResourceType()
        {
            Assert.AreEqual(requiredAttribute.ErrorMessageResourceType, typeof(Validations));
        }

        [Test]
        public void RequiredAdapter_SetsAttributeErrorMessageResourceName()
        {
            Assert.AreEqual(requiredAttribute.ErrorMessageResourceName, "FieldIsRequired");
        }

        #endregion
    }
}
