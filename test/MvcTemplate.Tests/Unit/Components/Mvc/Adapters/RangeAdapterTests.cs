using MvcTemplate.Components.Mvc;
using MvcTemplate.Resources.Shared;
using NUnit.Framework;
using System;
using System.Web.Mvc;
using DataAnnotations = System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Tests.Unit.Components.Mvc
{
    [TestFixture]
    public class RangeAdapterTests
    {
        private ModelMetadata metadata;

        [SetUp]
        public void SetUp()
        {
            metadata = new DataAnnotationsModelMetadataProvider().GetMetadataForProperty(null, typeof(AdaptersModel), "Range");
        }

        #region Constructor: RangeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute)

        [Test]
        public void RangeAdapter_SetsRangeErrorMessage()
        {
            DataAnnotations.RangeAttribute attribute = new DataAnnotations.RangeAttribute(0, 128);
            new RangeAdapter(metadata, new ControllerContext(), attribute);

            String expected = Validations.FieldMustBeInRange;
            String actual = attribute.ErrorMessage;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
