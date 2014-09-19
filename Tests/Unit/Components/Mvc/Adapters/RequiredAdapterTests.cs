using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
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
                .GetMetadataForProperty(null, typeof(TestView), "Id");
            new RequiredAdapter(metadata, new ControllerContext(), attribute);
        }

        #region Constructor: RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)

        [Test]
        public void RequiredAdapter_SetsErrorMessage()
        {
            Assert.AreEqual(attribute.ErrorMessage, Validations.FieldIsRequired);
        }

        #endregion
    }
}
