using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Template.Components.Mvc.Adapters;
using Template.Objects;
using Template.Resources;
using Template.Resources.Shared;

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
                .GetMetadataForProperty(null, typeof(RoleView), "Name");
            new RequiredAdapter(metadata, new ControllerContext(), requiredAttribute);
        }

        #region Constructor: RequiredAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)

        [Test]
        public void RequiredAdapter_SetsMetadataDisplayName()
        {
            String expected = ResourceProvider.GetPropertyTitle(typeof(RoleView), "Name");
            String actual = metadata.DisplayName;

            Assert.AreEqual(expected, actual);
        }

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
