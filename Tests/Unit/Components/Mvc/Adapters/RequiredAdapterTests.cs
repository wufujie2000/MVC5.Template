using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Template.Components.Mvc;
using Template.Resources.Shared;
using Template.Tests.Objects;

namespace Template.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class RequiredAdapterTests
    {
        private RequiredAttribute attribute;
        private ModelMetadata metadata;

        [SetUp]
        public void SetUp()
        {
            attribute = new RequiredAttribute();
            metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(TestView), "Text");
            new RequiredAdapter(metadata, new ControllerContext(), attribute);
        }

        #region Constructor: RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)

        [Test]
        public void RequiredAdapter_SetsAttributeErrorMessage()
        {
            Assert.AreEqual(attribute.ErrorMessage, Validations.FieldIsRequired);
        }

        #endregion
    }
}
