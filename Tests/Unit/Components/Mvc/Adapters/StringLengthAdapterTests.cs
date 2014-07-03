using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Shared;
using MvcTemplate.Tests.Objects;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class StringLengthAdapterTests
    {
        private ModelMetadata metadata;

        [SetUp]
        public void SetUp()
        {
            metadata = new DataAnnotationsModelMetadataProvider()
                .GetMetadataForProperty(null, typeof(TestView), "Text");

        }

        #region Constructor: StringLengthAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)

        [Test]
        public void StringLengthAdapter_SetsAttributeMaxLengthErrorMessage()
        {
            StringLengthAttribute attribute = new StringLengthAttribute(128);
            new StringLengthAdapter(metadata, new ControllerContext(), attribute);

            Assert.AreEqual(attribute.ErrorMessage, Validations.FieldMustNotExceedLength);
        }

        [Test]
        public void StringLengthAdapter_SetsAttributeMinMaxLengthErrorMessage()
        {
            StringLengthAttribute attribute = new StringLengthAttribute(128) { MinimumLength = 4 };
            new StringLengthAdapter(metadata, new ControllerContext(), attribute);

            Assert.AreEqual(attribute.ErrorMessage, Validations.FieldMustBeInRangeOfLength);
        }

        #endregion
    }
}
