using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Shared;
using NUnit.Framework;
using System;
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
            metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "StringLength");
        }

        #region Constructor: StringLengthAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)

        [Test]
        public void StringLengthAdapter_SetsMaxLengthErrorMessage()
        {
            StringLengthAttribute attribute = new StringLengthAttribute(128);
            new StringLengthAdapter(metadata, new ControllerContext(), attribute);

            String actual = Validations.FieldMustNotExceedLength;
            String expected = attribute.ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void StringLengthAdapter_SetsMinMaxLengthErrorMessage()
        {
            StringLengthAttribute attribute = new StringLengthAttribute(128) { MinimumLength = 4 };
            new StringLengthAdapter(metadata, new ControllerContext(), attribute);

            String actual = Validations.FieldMustBeInRangeOfLength;
            String expected = attribute.ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
