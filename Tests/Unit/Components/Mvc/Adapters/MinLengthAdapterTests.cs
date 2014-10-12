using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Shared;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class MinLengthAdapterTests
    {
        private ModelMetadata metadata;

        [SetUp]
        public void SetUp()
        {
            metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "MinLength");
        }

        #region Constructor: MinLengthAdapter(ModelMetadata metadata, ControllerContext context, MinLengthAttribute attribute)

        [Test]
        public void MinLengthAdapter_SetsMinLengthErrorMessage()
        {
            MinLengthAttribute attribute = new MinLengthAttribute(128);
            new MinLengthAdapter(metadata, new ControllerContext(), attribute);

            String expected = Validations.FieldMustBeWithMinLengthOf;
            String actual = attribute.ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
